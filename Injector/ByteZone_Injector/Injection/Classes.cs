using ByteZone_Injector;
using System.Diagnostics;
using System.Runtime.InteropServices;



class DllImports
{
    /* Windows API Imports */
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId); // OpenProcess function (processthreadsapi.h) 
                                                                                                        // https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocess


    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName); // GetModuleHandleA function (libloaderapi.h)
                                                                      // https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlea

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName); // GetProcAddress function (libloaderapi.h)
                                                                          // https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getprocaddress

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect); // VirtualAllocEx function (memoryapi.h)
                                                                                                                                // https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualallocex

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten); // WriteProcessMemory function (memoryapi.h)
                                                                                                                                                   // https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-writeprocessmemory

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess,
      IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId); //  CreateRemoteThread function (processthreadsapi.h)
                                                                                                                                        // https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-createremotethread
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);

    /* IOCTL */
    // Define IOCTL control codes
    public const uint IOCTL_INJECT_CODE = 0x222000; // Example control code for injecting code

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);




    /*
     * Process Security and Access Rights
     * https://learn.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights
     */


    /* Privileges */
    public const int PROCESS_CREATE_THREAD = 0X0002; // Required to create a thread in the process.

    /*
     * PROCESS_QUERY_INFORMATION Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName).
     * A handle that has the
     * PROCESS_QUERY_INFORMATION access right is automatically granted
     */
    public const int PROCESS_QUERY_INFORMATION = 0x0400;


    public const int PROCESS_VM_WRITE = 0x0020; // Required to write to memory in a process using WriteProcessMemory.
    public const int PROCESS_VM_READ = 0x0010; // Required to read memory in a process using ReadProcessMemory.

    public const int PROCESS_VM_OPERATION = 0x0008; // Required to perform an operation on the address space of a process 

    /* Memory allocation */
    public const uint MEM_COMMIT = 0x00001000; // MEM_COMMIT is a Windows constant used with Windows API calls
    public const uint MEM_RESERVE = 0x00002000; // MEM_RESERVE is a Windows constant used with Windows API calls
    public const uint PAGE_READWRITE = 4; // PAGE_READWRITE is a Windows constant used with Windows API calls

    /* Generic Access Rights Win32 */
    // https://learn.microsoft.com/en-us/windows/win32/secauthz/generic-access-rights
    public const uint GENERIC_READ = 0x80000000; // Read access
    public const uint GENERIC_WRITE = 0x40000000; // Write access
    
    /* 
     * Opens a file or device, only if it exists.
     * If the specified file or device does not exist, the
     * function fails and the last-error code is set to ERROR_FILE_NOT_FOUND (2).
     * https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea
     */
    public const int OPEN_EXISTING = 3; 

}
class InjectionMethods
{
    public bool NativeInjection;
    public bool ManualMapInjection;
    public bool KernelModeInjection;
    public bool ThreadHijacking;

    /// <summary>
    /// Performs native injection into the specified process using the provided DLL path.
    /// </summary>
    /// <param name="processName">The name of the target process.</param>
    /// <param name="dllPath">The path to the DLL to be injected.</param>
    public void NtNativeInjection(string processName, string dllPath)
    {
        // Get the process by name
        Process[] processes = Process.GetProcessesByName(processName);
        if (processes.Length == 0)
        {
            MessageBox.Show("Process not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Open the target process
        IntPtr hProcess = DllImports.OpenProcess(DllImports.PROCESS_CREATE_THREAD | DllImports.PROCESS_QUERY_INFORMATION | DllImports.PROCESS_VM_OPERATION | DllImports.PROCESS_VM_WRITE | DllImports.PROCESS_VM_READ, false, processes[0].Id);
        if (hProcess == IntPtr.Zero)
        {
            MessageBox.Show("Failed to open process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Allocate memory in the target process
        IntPtr allocAddress = DllImports.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), DllImports.MEM_COMMIT | DllImports.MEM_RESERVE, DllImports.PAGE_READWRITE);
        if (allocAddress == IntPtr.Zero)
        {
            MessageBox.Show("Failed to allocate memory in target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DllImports.CloseHandle(hProcess);
            return;
        }

        // Write the DLL path to the allocated memory
        byte[] bytes = System.Text.Encoding.Default.GetBytes(dllPath);
        UIntPtr bytesWritten;
        if (!DllImports.WriteProcessMemory(hProcess, allocAddress, bytes, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten))
        {
            MessageBox.Show("Failed to write to target process memory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DllImports.CloseHandle(hProcess);
            return;
        }

        // Get the address of LoadLibraryA in the kernel32.dll module
        IntPtr loadLibraryAddr = DllImports.GetProcAddress(DllImports.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
        if (loadLibraryAddr == IntPtr.Zero)
        {
            MessageBox.Show("Failed to get address of LoadLibraryA.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DllImports.CloseHandle(hProcess);
            return;
        }

        // Create a remote thread in the target process to execute LoadLibraryA with the DLL path
        IntPtr hThread = DllImports.CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocAddress, 0, IntPtr.Zero);
        if (hThread == IntPtr.Zero)
        {
            MessageBox.Show("Failed to create remote thread.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DllImports.CloseHandle(hProcess);
            return;
        }

        MessageBox.Show("DLL injected successfully.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // House Keeping
        DllImports.CloseHandle(hProcess);
        DllImports.CloseHandle(hThread);
    }

    public void NtManualMapInjection()
    {

    }

    /// <summary>
    /// Performs kernel mode injection by communicating with a driver using IOCTL.
    /// </summary>
    /// <param name="driverPath">The path to the driver used for injection.</param>
    /// <param name="payload">The payload to be injected.</param>
    public void NtKernelModeInjection(string driverPath, byte[] payload)
    {
        // Open a handle to the driver
        IntPtr hDevice = DllImports.CreateFile(driverPath, DllImports.GENERIC_READ | DllImports.GENERIC_WRITE, 0, IntPtr.Zero, DllImports.OPEN_EXISTING, 0, IntPtr.Zero);

        if (hDevice.ToInt32() != -1)
        {
            // Allocate memory for the payload
            IntPtr payloadPtr = Marshal.AllocHGlobal(payload.Length);
            Marshal.Copy(payload, 0, payloadPtr, payload.Length);
            uint bytesReturned;

            // Send IOCTL request to the driver for injection
            bool success = DllImports.DeviceIoControl(hDevice, DllImports.IOCTL_INJECT_CODE, payloadPtr, (uint)payload.Length, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

            // Check if injection was successful
            if (success)
            {
                MessageBox.Show("Injection successful!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Injection failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // House Keeping
            Marshal.FreeHGlobal(payloadPtr);
            DllImports.CloseHandle(hDevice);
        }
        else
        {
            MessageBox.Show("Failed to open handle to driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}