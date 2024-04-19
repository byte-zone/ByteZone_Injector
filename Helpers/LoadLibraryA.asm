section .text
global _start

extern LoadLibraryA
extern GetProcAddress
extern FreeLibrary

_start:
    push dword 0
    push dll_name
    call LoadLibraryA
    mov ebx, eax

    push dword 0
    push dword "DllMain"
    push ebx
    call GetProcAddress
    mov ecx, eax

    push dword 0
    push dword DLL_PROCESS_ATTACH
    call ecx

    push ebx
    call FreeLibrary

    mov eax, 1
    xor ebx, ebx
    int 0x80

section .data
dll_name db 'example.dll', 0 ; Change the DLL name as needed
