section .text
global _start

extern LpdLoadDll
extern OpenProcess
extern GetLastError

_start:
    mov eax, [LpdLoadDll]
    push dword 0x001F0FFF
    push dword 0
    push dword process_id
    call dword [OpenProcess]
    mov ebx, eax
    test ebx, ebx
    jz error_exit
    push dword flags
    push dll_path
    push ebx
    call eax
    test eax, eax
    jnz error_exit
    mov eax, 0
    jmp exit

error_exit:
    mov eax, 1

exit:
    xor ebx, ebx
    int 0x80

section .data
dll_path db 'C:\path\to\your\DLL.dll', 0
process_id dd 1234
flags dd 0

section .extern
extern LpdLoadDll: dd
extern OpenProcess: dword
extern GetLastError: dword
