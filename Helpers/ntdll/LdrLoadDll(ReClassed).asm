Function: LdrLoadDll(a1, a2, a3, a4)
    // Declarations
    v8, v9, v10, v11, v12, v13, v14, v15, v17, v18, v19[15]: __int64
    v20: char

    // Check if a2 is not null to determine load options
    if a2:
        v8 = *a2
        v9 = 2 * (v8 & 4)
        v10 = v9 | 0x40
        if (v8 & 2) == 0:
            v10 = v9
        v11 = v10 | 0x80
        if (v8 & 0x800000) == 0:
            v11 = v10
        v12 = v11 | 0x100
        if (v8 & 0x1000) == 0:
            v12 = v11
        v13 = v12 | 0x400000
        if *a2 >= 0:
            v13 = v12
    else:
        v13 = 0
    
    // Log debug information about DLL name
    if (LdrpDebugFlags & 9) != 0:
        LdrpLogDbgPrint("minkernel\\ntdll\\ldrapi.c", 151, "LdrLoadDll", 3, "DLL name: %wZ\n", a3)
    
    // Check if loading a packaged DLL in a non-packaged process
    if (LdrpPolicyBits & 4) == 0 && (a1 & 0x401) == 1025:
        return 3221225485
    
    // Check if loading packaged DLLs is allowed
    if (v13 & 8) == 0 || (LdrpPolicyBits & 8) != 0:
        // Check if current thread is within a TEB
        if (NtCurrentTeb()->SameTebFlags & 0x2000) != 0:
            v14 = -1073740004
        else:
            // Initialize DLL path and load DLL
            LdrpInitializeDllPath(*(_QWORD *)(a3 + 8), a1, v19)
            v14 = LdrpLoadDll(a3, v19, v13, &v18)
            if v20:
                RtlReleasePath(v19[0])
            if v14 >= 0:
                v15 = v18
                *a4 = *(_QWORD *)(v18 + 48)
                LdrpDereferenceModule(v15)
    else:
        // Log error for attempting to load a packaged DLL in a non-packaged process
        if (LdrpDebugFlags & 3) != 0:
            LdrpLogDbgPrint("minkernel\\ntdll\\ldrapi.c", 172, "LdrLoadDll", 0, "Nonpackaged process attempted to load a packaged DLL.\n")
        // Break into debugger if debugging is enabled
        if (LdrpDebugFlags & 0x10) != 0:
            __debugbreak()
        v14 = -1073741398
    
    // Log debug information about the status
    if (LdrpDebugFlags & 9) != 0:
        LODWORD(v17) = v14
        LdrpLogDbgPrint("minkernel\\ntdll\\ldrapi.c", 204, "LdrLoadDll", 4, "Status: 0x%08lx\n", v17)
    
    return v14
