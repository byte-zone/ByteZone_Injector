using ByteZone_Injector;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static SysCall_ManualMap;
using static SysCall_Native;



class SysCall_Native
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

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesRead);
}


class SysCall_Flags
{

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
    public const uint PAGE_EXECUTE_READ = 0x20;

}


/* IOCTL */
class SysCall_KernelMode
{
    
    // Define IOCTL control codes
    public const uint IOCTL_INJECT_CODE_NATIVE = 0xAF1B92;
    public const uint IOCTL_INJECT_CODE_MANUAL_MAP = 0x72E4FC;
    public const uint IOCTL_INJECT_CODE_THREAD_HIJACKING = 0x4D38A7;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);
}

class SysCall_LdrpLoadDll
{
    delegate IntPtr LdrpLoadDllDelegate(bool Redirected, string DllPath, IntPtr DllCharacteristics, IntPtr DllName, out IntPtr BaseAddress, bool CallInit);
}



class SysCall_ManualMap
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetLastError();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, int dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

    public const uint PROCESS_CREATE_THREAD = 0x0002;
    public const uint PROCESS_QUERY_INFORMATION = 0x0400;
    public const uint PROCESS_VM_OPERATION = 0x0008;
    public const uint PROCESS_VM_WRITE = 0x0020;
    public const uint PROCESS_VM_READ = 0x0010;
    public const uint MEM_COMMIT = 0x00001000;
    public const uint MEM_RESERVE = 0x00002000;
    public const uint PAGE_READWRITE = 0x04;
    public const uint MEM_RELEASE = 0x8000;

}

class InjectionMethods
{
    /* Helper 
     * So in order to get LdrpLoadDll address we need to find it dynamically 
     * We can't use GetProcAddress directly because it's not exposed for external use and does not have a direct export from ntdll.dll
     * Please read here for more details https://doxygen.reactos.org/d8/d55/ldrutils_8c_source.html
     * You might dig into ntdll.dll to find the offsets, i did but I'm not sure 
     */

    private IntPtr FindLdrpLoadDllAddress(IntPtr hProcess, IntPtr ntdllModuleHandle)
    {

        byte[] peHeader = new byte[4096]; 
        UIntPtr bytesRead;
        ReadProcessMemory(hProcess, ntdllModuleHandle, peHeader, (uint)peHeader.Length, out bytesRead);

        int peHeaderOffset = BitConverter.ToInt32(peHeader, 0x3C); // PE offset called at 0x3C
        int exportDirectoryRvaOffset = peHeaderOffset + 0x78; 
        int exportDirectoryRva = BitConverter.ToInt32(peHeader, exportDirectoryRvaOffset);

        byte[] exportDirectory = new byte[40]; 
        ReadProcessMemory(hProcess, (IntPtr)(ntdllModuleHandle.ToInt64() + exportDirectoryRva), exportDirectory, (uint)exportDirectory.Length, out bytesRead);

        int eatRvaOffset = 0x1C; // EAT/RVA called at 0x1C
        int eatRva = BitConverter.ToInt32(exportDirectory, eatRvaOffset);

        byte[] eat = new byte[4]; 
        ReadProcessMemory(hProcess, (IntPtr)(ntdllModuleHandle.ToInt64() + eatRva), eat, (uint)eat.Length, out bytesRead);

        int ldrpLoadDllRva = BitConverter.ToInt32(eat, 0);

        // Calculate the VA of LdrpLoadDll
        int ldrpLoadDllVa = ldrpLoadDllRva + (int)ntdllModuleHandle.ToInt64();

        return (IntPtr)ldrpLoadDllVa;

        // You might need to reverse the module by yourself because my calculation might be incorrect 
    }


    public bool NativeInjection { get; set; }
    public bool ManualMapInjection { get; set; }
    public bool KernelModeInjection { get; set; }
    public bool ThreadHijacking { get; set; }
    public bool LdrpLoadDll { get; set; }





    public void NtThreadHijacking(string processName, string dllPath)
    {

    }


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
        IntPtr hProcess = SysCall_Native.OpenProcess(SysCall_Flags.PROCESS_CREATE_THREAD | SysCall_Flags.PROCESS_QUERY_INFORMATION | SysCall_Flags.PROCESS_VM_OPERATION | SysCall_Flags.PROCESS_VM_WRITE | SysCall_Flags.PROCESS_VM_READ, false, processes[0].Id);
        if (hProcess == IntPtr.Zero)
        {
            MessageBox.Show("Failed to open process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Allocate memory 
        IntPtr allocAddress = SysCall_Native.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), SysCall_Flags.MEM_COMMIT | SysCall_Flags.MEM_RESERVE, SysCall_Flags.PAGE_READWRITE);
        if (allocAddress == IntPtr.Zero)
        {
            MessageBox.Show("Failed to allocate memory in target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SysCall_Native.CloseHandle(hProcess);
            return;
        }

        // Write the DLL path to the allocated memory
        byte[] bytes = System.Text.Encoding.Default.GetBytes(dllPath);
        UIntPtr bytesWritten;
        if (!SysCall_Native.WriteProcessMemory(hProcess, allocAddress, bytes, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten))
        {
            MessageBox.Show("Failed to write to target process memory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SysCall_Native.CloseHandle(hProcess);
            return;
        }

        // Get the address of LoadLibraryA in the kernel32.dll module
        IntPtr loadLibraryAddr = SysCall_Native.GetProcAddress(SysCall_Native.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
        if (loadLibraryAddr == IntPtr.Zero)
        {
            MessageBox.Show("Failed to get address of LoadLibraryA.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SysCall_Native.CloseHandle(hProcess);
            return;
        }

        // Create a remote thread in the target process to execute LoadLibraryA with the DLL path
        IntPtr hThread = SysCall_Native.CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocAddress, 0, IntPtr.Zero);
        if (hThread == IntPtr.Zero)
        {
            MessageBox.Show("Failed to create remote thread.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SysCall_Native.CloseHandle(hProcess);
            return;
        }

        MessageBox.Show("DLL injected successfully.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Housekeeping
        SysCall_Native.CloseHandle(hProcess);
        SysCall_Native.CloseHandle(hThread);
    }



    public void NtLdrpLoadDll(string processName, string dllPath)
    {
        Process[] processes = Process.GetProcessesByName(processName);
        if (processes.Length == 0)
        {
            MessageBox.Show("Process not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Process targetProcess = processes[0];
        IntPtr hProcess = OpenProcess(SysCall_Flags.PROCESS_CREATE_THREAD | SysCall_Flags.PROCESS_QUERY_INFORMATION | SysCall_Flags.PROCESS_VM_OPERATION | SysCall_Flags.PROCESS_VM_WRITE | SysCall_Flags.PROCESS_VM_READ, false, targetProcess.Id);
        if (hProcess == IntPtr.Zero)
        {
            MessageBox.Show("Failed to open process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            IntPtr ldrpLoadDllAddr = FindLdrpLoadDllAddress(hProcess, GetModuleHandle("ntdll.dll"));
            if (ldrpLoadDllAddr == IntPtr.Zero)
            {
                MessageBox.Show("Failed to find the address of LdrpLoadDll.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            IntPtr allocAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), SysCall_Flags.MEM_COMMIT | SysCall_Flags.MEM_RESERVE, SysCall_Flags.PAGE_READWRITE);
            if (allocAddress == IntPtr.Zero)
            {
                MessageBox.Show("Failed to allocate memory in target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            byte[] bytes = Encoding.Default.GetBytes(dllPath);
            UIntPtr bytesWritten;
            if (!WriteProcessMemory(hProcess, allocAddress, bytes, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten))
            {
                MessageBox.Show("Failed to write to target process memory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, ldrpLoadDllAddr, allocAddress, 0, IntPtr.Zero);
            if (hThread == IntPtr.Zero)
            {
                MessageBox.Show("Failed to create remote thread.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("DLL injected successfully using LdrpLoadDll.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Housekeeping
            CloseHandle(hProcess);
            CloseHandle(hThread);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (hProcess != IntPtr.Zero)
            {
                CloseHandle(hProcess);
            }
        }
    }





    /// <summary>
    /// Performs kernel mode injection by communicating with a driver using IOCTL.
    /// </summary>
    /// <param name="driverPath">The path to the driver used for injection.</param>
    /// <param name="payload">The payload to be injected.</param>
    public void NtKernelModeInjection(string driverPath, byte[] payload)
    {
        // Open a handle to the driver
        IntPtr hDevice = SysCall_KernelMode.CreateFile(driverPath, SysCall_Flags.GENERIC_READ | SysCall_Flags.GENERIC_WRITE, 0, IntPtr.Zero, SysCall_Flags.OPEN_EXISTING, 0, IntPtr.Zero);

        if (hDevice.ToInt32() != -1)
        {
            // Allocate memory for the payload
            IntPtr payloadPtr = Marshal.AllocHGlobal(payload.Length);
            Marshal.Copy(payload, 0, payloadPtr, payload.Length);
            uint bytesReturned;

            // Send IOCTL request to the driver for injection
            bool success = SysCall_KernelMode.DeviceIoControl(hDevice, SysCall_KernelMode.IOCTL_INJECT_CODE_NATIVE, payloadPtr, (uint)payload.Length, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

            // Check if injection was successful
            if (success)
            {
                MessageBox.Show("Injection successful!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Injection failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Housekeeping
            Marshal.FreeHGlobal(payloadPtr);
            SysCall_Native.CloseHandle(hDevice);
        }
        else
        {
            MessageBox.Show("Failed to open handle to driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }




    public void NtManualMapInjection(string processName, string dllPath)
    {
        try
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                throw new InvalidOperationException("Process not found.");
            }

            Process targetProcess = processes[0];
            IntPtr hProcess = OpenProcess((int)(SysCall_ManualMap.PROCESS_CREATE_THREAD | SysCall_ManualMap.PROCESS_QUERY_INFORMATION | SysCall_ManualMap.PROCESS_VM_OPERATION | SysCall_ManualMap.PROCESS_VM_WRITE | SysCall_ManualMap.PROCESS_VM_READ), false, targetProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to open process.");
            }


            byte[] dllBytes = System.IO.File.ReadAllBytes(dllPath);

            IntPtr remoteMemory = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)dllBytes.Length, SysCall_Flags.MEM_COMMIT | SysCall_Flags.MEM_RESERVE, SysCall_Flags.PAGE_READWRITE);
            if (remoteMemory == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to allocate memory in target process.");
            }

            try
            {

                UIntPtr bytesWritten;
                if (!WriteProcessMemory(hProcess, remoteMemory, dllBytes, (uint)dllBytes.Length, out bytesWritten))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to write DLL into target process memory.");
                }

                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (loadLibraryAddr == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to get address of LoadLibraryA.");
                }

                byte[] shellcode = ShellcodeGenerator.CallLoadLibrary((ulong)remoteMemory, (ulong)loadLibraryAddr);

                IntPtr remoteShellcode = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)shellcode.Length, SysCall_Flags.MEM_COMMIT | SysCall_Flags.MEM_RESERVE, SysCall_Flags.PAGE_READWRITE);
                if (remoteShellcode == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to allocate memory for shellcode in target process.");
                }

                try
                {

                    if (!WriteProcessMemory(hProcess, remoteShellcode, shellcode, (uint)shellcode.Length, out bytesWritten))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to write shellcode into target process memory.");
                    }

                    if (!VirtualProtectEx(hProcess, remoteShellcode, (uint)shellcode.Length, SysCall_Flags.PAGE_EXECUTE_READ, out _))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to change memory protection for shellcode.");
                    }

                    IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, remoteShellcode, IntPtr.Zero, 0, IntPtr.Zero);
                    if (hThread == IntPtr.Zero)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to create remote thread.");
                    }

                    Console.WriteLine("DLL mapped successfully using manual mapping.");

                    
                    CloseHandle(hThread);
                    CloseHandle(hProcess);
                }
                finally
                {
                    
                    if (remoteShellcode != IntPtr.Zero)
                    {
                        VirtualFreeEx(hProcess, remoteShellcode, 0, (int)SysCall_ManualMap.MEM_RELEASE);
                    }
                }
            }
            finally
            {
                
                if (remoteMemory != IntPtr.Zero)
                {
                    VirtualFreeEx(hProcess, remoteMemory, 0, (int)SysCall_ManualMap.MEM_RELEASE);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

}


public static unsafe class ShellcodeGenerator
{
    public static byte[] CallLoadLibrary(ulong allocatedImagePath, ulong loadLibraryPointer)
    {
        byte[] shellcode = new byte[]
        {
            0x9C, 0x50, 0x53, 0x51, 0x52, 0x41, 0x50, 0x41, 0x51, 0x41, 0x52, 0x41, 0x53, // push     REGISTERS
            0x48, 0x83, 0xEC, 0x28, // sub      RSP, 0x28
            0x48, 0xB9, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // movabs   RCX, 0x0000000000000000 ; Image path
            0x48, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // movabs   RAX, 0x0000000000000000 ; Pointer to LoadLibrary
            0xFF, 0xD0, // call     RAX
            0x48, 0x83, 0xC4, 0x28, // add      RSP, 0x28
            0x41, 0x5B, 0x41, 0x5A, 0x41, 0x59, 0x41, 0x58, 0x5A, 0x59, 0x5B, 0x58, 0x9D, // pop      REGISTER
            0xC3 // ret
        };

        fixed (byte* shellcodePointer = shellcode)
        {
            *(ulong*)(shellcodePointer + 19) = allocatedImagePath;
            *(ulong*)(shellcodePointer + 29) = loadLibraryPointer;
        }

        return shellcode;
    }

    public static byte[] CallDllMain(ulong remoteImage, ulong entrypoint, bool safeCall)
    {
        byte[] shellcode;

        if (safeCall)
        {
            shellcode = new byte[]
            {
                0x9C, 0x50, 0x53, 0x51, 0x52, 0x41, 0x50, 0x41, 0x51, 0x41, 0x52, 0x41, 0x53, // push     REGISTERS
                0x48, 0x83, 0xEC, 0x28, // sub      RSP, 0x28
                0x48, 0xB9, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // movabs   RCX, 0x0000000000000000
                0x48, 0xC7, 0xC2, 0x01, 0x00, 0x00, 0x00, // mov      rdx, 0x1
                0x4D, 0x31, 0xC0, // xor      r8, r8
                0x48, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // movabs   RAX, 0x0000000000000000
                0xFF, 0xD0, // call     RAX
                0x48, 0x83, 0xC4, 0x28, // add      RSP, 0x28
                0x41, 0x5B, 0x41, 0x5A, 0x41, 0x59, 0x41, 0x58, 0x5A, 0x59, 0x5B, 0x58, 0x9D, // pop      REGISTERS
                0xC3 // ret
            };

            fixed (byte* shellcodePointer = shellcode)
            {
                *(ulong*)(shellcodePointer + 19) = remoteImage;
                *(ulong*)(shellcodePointer + 39) = entrypoint;
            }
        }
        else
        {
            shellcode = new byte[]
            {
                0x48, 0x83, 0xEC, 0x28, // sub      RSP, 0x28
                0x48, 0xB9, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // movabs   RCX, 0x0000000000000000
                0x48, 0xC7, 0xC2, 0x01, 0x00, 0x00, 0x00, // mov      rdx, 0x1
                0x4D, 0x31, 0xC0, // xor      r8, r8
                0x48, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // movabs   RAX, 0x0000000000000000
                0xFF, 0xD0, // call     RAX
                0x48, 0x83, 0xC4, 0x28, // add      RSP, 0x28
                0xC3 // ret
            };

            fixed (byte* shellcodePointer = shellcode)
            {
                *(ulong*)(shellcodePointer + 6) = remoteImage;
                *(ulong*)(shellcodePointer + 26) = entrypoint;
            }
        }

        return shellcode;
    }
}
