## What is ByteZone Injector?

ByteZone Injector is a user-mode injector written in C# .NET 8.0, designed for educational purposes in computer science. It allows you to delve into injection techniques and gain a deeper understanding of how applications interact with the system.

### Project Structure

ByteZone Injector consists of two main files:

#### `classes.cs`
This file contains the core logic for injection methods and GUI interaction. It defines two main classes:

- **DllImports:** This class encapsulates all Windows API imports and flags used throughout the injector, providing a central location for these definitions.
  ```csharp
  class DllImports
  {
      /* Memory allocation */
  public const uint MEM_COMMIT = 0x00001000; // MEM_COMMIT is a Windows constant used with Windows API calls
  public const uint MEM_RESERVE = 0x00002000; // MEM_RESERVE is a Windows constant used with Windows API calls
  public const uint PAGE_READWRITE = 4; // PAGE_READWRITE is a Windows constant used with Windows API calls
  }
  ```

- **InjectionMethods:** This class houses the actual injection techniques. Currently, it supports Native Injection, but future development plans include additional methods.
  ```csharp
  class InjectionMethods
  {

        IntPtr hProcess = DllImports.OpenProcess(DllImports.PROCESS_CREATE_THREAD | DllImports.PROCESS_QUERY_INFORMATION | DllImports.PROCESS_VM_OPERATION | DllImports.PROCESS_VM_WRITE | DllImports.PROCESS_VM_READ, false, processes[0].Id);
        if (hProcess == IntPtr.Zero)
        {
            MessageBox.Show("Failed to open process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
  }
  ```
#### `Main.cs`
contains the main entry point and user interface logic for the ByteZone Injector application. It provides functionality to select a process and a DLL file for injection, as well as options for choosing the injection method. Key features include:
1. Retrieval of running processes and display in a list.
2. Selection of a DLL file through a file dialog.
3. Native injection of the selected DLL into the chosen process.
4. Handling of user interactions such as button clicks and dropdown selections.
5. Display of process names and their corresponding IDs in a list.
6. Management of the user interface components such as buttons, dropdowns, and labels.
### Multiple Injection Methods

ByteZone Injector offers various injection techniques to experiment with, providing a hands-on learning experience for understanding application loading and execution mechanisms.

### Current Features

**Native Injection:**
This version of ByteZone Injector currently supports native injection using LoadLibraryA, providing a solid foundation for understanding injection mechanisms.
```csharp
// Sample code for native injection
I_Method.NtNativeInjection(S_ProcessName, dllPath);
```

### Clean and Well-Commented Code

The codebase is well-structured and includes clear comments, making it easy to learn from and understand as you experiment with the current Native Injection feature.

### Installation

1. Download and install the .NET 8.0 SDK from [here](link).
2. Download ByteZone injector from [here](link).
3. Build and run the project.

### Roadmap (Features in Development)

Development is ongoing to integrate additional injection techniques, including:
- [ ] Better Gui
- [ ] Settings tab
- [ ] Kernel injection using IOCTL with a custom driver (driver under development)
- [ ] LdrpLoadDll (planned for future release)
- [ ] Manual mapping (planned for future release)
- [ ] Thread hijacking (planned for future release)

> [!NOTE]
> Kernel injection, manual mapping, and thread hijacking features are not yet available in this version.
