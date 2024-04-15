## What is ByteZone Injector?
ByteZone Injector is a user-mode injector written in C# .NET 8.0, designed for educational purposes in computer science. It allows you to delve into injection techniques and gain a deeper understanding of how applications interact with the system.

### Project Structure:
**ByteZone Injector consists of two main files:**
**classes.cs:**
This file contains the core logic for injection methods and GUI interaction. It defines two main classes:
- **DllImports**
This class encapsulates all Windows API imports and flags used throughout the injector, providing a central location for these definitions.
- **InjectionMethods**
This class houses the actual injection techniques. Currently, it supports Native Injection, but future development plans include additional methods.



### Multiple Injection Methods
ByteZone Injector offers various injection techniques to experiment with, providing a hands-on learning experience for understanding application loading and execution mechanisms. 


### Current Features
**Native Injection:**
This version of ByteZone Injector currently supports native injection using LoadLibraryA, providing a solid foundation for understanding injection mechanisms.

### Clean and Well-Commented Code
The codebase is well-structured and includes clear comments, making it easy to learn from and understand as you experiment with the current Native Injection feature.

### Installation
1. Download and install the .NET 8.0 SDK from here
2. Download ByteZone injector from here
3. Build and run the project
### Roadmap (Features in Development):
**Development is ongoing to integrate additional injection techniques, including:**
- [ ] Settings tab
- [ ] Kernel injection using IOCTL with a custom driver (driver under development)
- [ ] LdrpLoadDll (planned for future release)
- [ ] Manual mapping (planned for future release)
- [ ] Thread hijacking (planned for future release)

> [!NOTE]
> Kernel injection, manual mapping, and thread hijacking features are not yet available in this version.
