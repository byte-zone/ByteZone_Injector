section .text
global _start

extern LoadLibraryA
extern GetProcAddress
extern FreeLibrary

_start:
    ; Load the DLL
    push dword 0 ; reserved parameter, must be zero
    push dll_name ; pointer to the name of the DLL to load
    call LoadLibraryA
    mov ebx, eax ; store the handle to the loaded DLL

    ; Get the address of DllMain
    push dword 0 ; reserved parameter, must be zero
    push dword "DllMain" ; name of the function to retrieve
    push ebx ; handle to the loaded DLL
    call GetProcAddress
    mov ecx, eax ; store the address of DllMain

    ; Call DllMain
    push dword 0 ; reserved parameter, must be zero
    push dword DLL_PROCESS_ATTACH ; dwReason = DLL_PROCESS_ATTACH
    call ecx ; call DllMain

    ; Free the DLL
    push ebx ; handle to the loaded DLL
    call FreeLibrary

    ; Exit
    mov eax, 1 ; exit code
    xor ebx, ebx ; no error
    int 0x80

section .data
dll_name db 'example.dll', 0 ; Change the DLL name as needed
