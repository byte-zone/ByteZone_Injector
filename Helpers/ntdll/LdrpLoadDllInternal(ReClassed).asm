Function LdrpLoadDllInternal(a1, a2, a3, a4, a5, a6, a7, a8):
    // Declarations
    result, v12, v13, v14, v15, v16, v17, v18, v19, v20: __int64
    // Function Body
    if ((LdrpDebugFlags & 9) != 0):
        LdrpLogDbgPrint("minkernel\\ntdll\\ldrapi.c", 425, "LdrpLoadDllInternal", 3, "DLL name: %wZ\n", a1)
    
    // Initialize variables
    *a7 = 0
    v20 = 0
    
    // Attempt to reload DLL fastpath
    result = LdrpFastpthReloadedDll(a1, a3, a6, a7)
    
    // Handle failure to reload DLL
    if result < 0:
        // Check if current thread is within a TEB
        if (NtCurrentTeb()->SameTebFlags & 0x1000) != 0:
            v13 = 1
        else:
            v13 = 0
            LdrpDrainWorkQueue(0)
        
        // Check conditions for detour detection and processing work
        if !a6 || v13 || *((_DWORD *)(*((_QWORD *)(a6 + 152)) + 24)):
            LdrpDetectDetour()
            v12 = a8
            v14 = LdrpFindOrPrepareLoadingModule(a1, a2, a3, a4, a5, v20, a8)
            
            // Handle specific error codes
            if v14 == -1073741515:
                v15 = 1
                LdrpProcessWork(*((QWORD *)(v20 + 176)), v15)
            else if v14 != -1073741267 && v14 < 0:
                *a8 = v14
        else:
            v12 = a8
            *a8 = -1073741515
        
        // Drain work queue
        LdrpDrainWorkQueue(1)
        
        // Handle replaced module
        if v20:
            v16 = LdrpHandleReplacedModule()
            *a7 = v16
            
            if v20 != v16:
                LdrpFreeReplacedModule()
                v20 = *a7
            
            if *((QWORD *)(v20 + 176)):
                LdrpCondenseGraph(*((QWORD *)(v20 + 152)))
                
            if *v12 >= 0:
                v17 = LdrpPrepareModuleForExecution(v20, v12)
                *v12 = v17
                
                if v17 >= 0:
                    v18 = LdrpBuildForwarderLink(a6, v20)
                    *v12 = v18
                    
                    if v18 >= 0 && !LdrInitState:
                        LdrpPinModule(v20)
                        
            result = LdrpFreeLoadContextOfNode(*((QWORD *)(v20 + 152)), a12)
            
            if *v12 < 0:
                *a7 = 0
                LdrpDecrementModuleLoadCountEx(v20, 0)
                result = LdrpDereferenceModule(v20)
        else:
            *v12 = -1073741801
        
        // Drop last in-progress count if not in TEB
        if !v13:
            result = LdrpDropLastInProgressCount()
    else:
        v12 = a8
        *a8 = result
        
    // Log debug information
    if (LdrpDebugFlags & 9) != 0:
        v19 = *v12
        return LdrpLogDbgPrint("minkernel\\ntdll\\ldrapi.c", 655, "LdrpLoadDllInternal", 4, "Status: 0x%08lx\n", v19)
    
    return result
