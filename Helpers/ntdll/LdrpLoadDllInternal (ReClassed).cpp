__int64 LdrpLoadDllInternal(__int64 a1, int a2, unsigned int a3, int a4, __int64 a5, __int64 a6, __int64* a7, int* a8) {
    __int64 result, v12, v13, v14, v15, v16, v17, v18, v19, v20;
    
    if (LdrpDebugFlags & 9) {
        LdrpLogDbgPrint("minkernel\\ntdll\\ldrapi.c", 425, "LdrpLoadDllInternal", 3, "DLL name: %wZ\n", a1);
    }

    *a7 = 0;
    v20 = 0;
    result = LdrpFastpthReloadedDll(a1, a3, a6, a7);
    
    if (result < 0) {
        if (NtCurrentTeb()->SameTebFlags & 0x1000) {
            v13 = 1;
        } else {
            v13 = 0;
            LdrpDrainWorkQueue(0);
        }

        if (!a6 || v13 || *((int*)(*(_QWORD*)(a6 + 152)) + 24)) {
            LdrpDetectDetour();
            v12 = a8;
            v14 = LdrpFindOrPrepareLoadingModule(a1, a2, a3, a4, a5, v20, a8);
            
            if (v14 == -1073741515) {
                v15 = 1;
                LdrpProcessWork(*((__int64*)(v20 + 176)), v15);
            } else if (v14 != -1073741267 && v14 < 0) {
                *a8 = v14;
            }
        } else {
            v12 = a8;
            *a8 = -1073741515;
        }

        LdrpDrainWorkQueue(1);

        if (v20) {
            v16 = LdrpHandleReplacedModule();
            *a7 = v16;

            if (v20 != v16) {
                LdrpFreeReplacedModule();
                v20 = *a7;
            }

            if (*((__int64*)(v20 + 176))) {
                LdrpCondenseGraph(*((__int64*)(v20 + 152)));
            }

            if (*v12 >= 0) {
                v17 = LdrpPrepareModuleForExecution(v20, v12);
                *v12 = v17;

                if (v17 >= 0) {
                    v18 = LdrpBuildForwarderLink(a6, v20);
                    *v12 = v18;

                    if (v18 >= 0 && !LdrInitState) {
                        LdrpPinModule(v20);
                    }
                }
            }

            result = LdrpFreeLoadContextOfNode(*((__int64*)(v20 + 152)), a12);

            if (*v12 < 0) {
                *a7 = 0;
                LdrpDecrementModuleLoadCountEx(v20, 0);
                result = LdrpDereferenceModule(v20);
            }
        } else {
            *v12 = -1073741801;
        }

        if (!v13) {
            result = LdrpDropLastInProgressCount();
        }
    } else {
        v12 = a8;
        *a8 = result;
    }

    if (LdrpDebugFlags & 9) {
        v19 = *v12;
        return LdrpLogDbgPrint("minkernel\\ntdll\\ldrapi.c", 655, "LdrpLoadDllInternal", 4, "Status: 0x%08lx\n", v19);
    }

    return result;
}
