NTSTATUS LdrLoadDll(__int64 a1, int* a2, __int64 a3, ULONGLONG* a4) {
    ULONGLONG v8, v9, v10, v11, v12, v13, v14, v15, v17, v18, v19[15];
    char v20;

    if (a2) {
        v8 = *a2;
        v9 = 2 * (v8 & 4);
        v10 = v9 | 0x40;
        if (!(v8 & 2))
            v10 = v9;
        v11 = v10 | 0x80;
        if (!(v8 & 0x800000))
            v11 = v10;
        v12 = v11 | 0x100;
        if (!(v8 & 0x1000))
            v12 = v11;
        v13 = v12 | 0x400000;
        if (*a2 >= 0)
            v13 = v12;
    } else {
        v13 = 0;
    }

    if (LdrpDebugFlags & 9) {
        printf("DLL name: %wZ\n", a3);
    }

    if ((LdrpPolicyBits & 4) == 0 && (a1 & 0x401) == 1025) {
        return 3221225485LL;
    }

    if ((v13 & 8) == 0 || (LdrpPolicyBits & 8) != 0) {
        if (NtCurrentTeb()->SameTebFlags & 0x2000) {
            v14 = -1073740004;
        } else {
            LdrpInitializeDllPath(*(_QWORD*)(a3 + 8), a1, v19);
            v14 = LdrpLoadDll(a3, v19, v13, &v18);
            if (v20)
                RtlReleasePath(v19[0]);
            if (v14 >= 0) {
                v15 = v18;
                *a4 = *(_QWORD*)(v18 + 48);
                LdrpDereferenceModule(v15);
            }
        }
    } else {
        if (LdrpDebugFlags & 3) {
            printf("Nonpackaged process attempted to load a packaged DLL.\n");
        }
        if (LdrpDebugFlags & 0x10) {
            __debugbreak();
        }
        v14 = -1073741398;
    }

    if (LdrpDebugFlags & 9) {
        printf("Status: 0x%08lx\n", v14);
    }

    return v14;
}
