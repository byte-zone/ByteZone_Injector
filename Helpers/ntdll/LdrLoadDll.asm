__int64 __fastcall LdrLoadDll(__int64 a1, int *a2, __int64 a3, _QWORD *a4)
{
  int v8; // eax
  int v9; // ecx
  int v10; // edx
  int v11; // r8d
  int v12; // ecx
  unsigned int v13; // ebx
  int v14; // ebx
  __int64 v15; // rcx
  __int64 v17; // [rsp+28h] [rbp-C0h]
  __int64 v18; // [rsp+30h] [rbp-B8h] BYREF
  __int64 v19[15]; // [rsp+40h] [rbp-A8h] BYREF
  char v20; // [rsp+BCh] [rbp-2Ch]

  if ( a2 )
  {
    v8 = *a2;
    v9 = 2 * (*a2 & 4);
    v10 = v9 | 0x40;
    if ( (v8 & 2) == 0 )
      v10 = v9;
    v11 = v10 | 0x80;
    if ( (*a2 & 0x800000) == 0 )
      v11 = v10;
    v12 = v11 | 0x100;
    if ( (*a2 & 0x1000) == 0 )
      v12 = v11;
    v13 = v12 | 0x400000;
    if ( *a2 >= 0 )
      v13 = v12;
  }
  else
  {
    v13 = 0;
  }
  if ( (LdrpDebugFlags & 9) != 0 )
    LdrpLogDbgPrint(
      (unsigned int)"minkernel\\ntdll\\ldrapi.c",
      151,
      (unsigned int)"LdrLoadDll",
      3,
      "DLL name: %wZ\n",
      a3);
  if ( (LdrpPolicyBits & 4) == 0 && (a1 & 0x401) == 1025 )
    return 3221225485LL;
  if ( (v13 & 8) == 0 || (LdrpPolicyBits & 8) != 0 )
  {
    if ( (NtCurrentTeb()->SameTebFlags & 0x2000) != 0 )
    {
      v14 = -1073740004;
    }
    else
    {
      LdrpInitializeDllPath(*(_QWORD *)(a3 + 8), a1, v19);
      v14 = LdrpLoadDll(a3, v19, v13, &v18);
      if ( v20 )
        RtlReleasePath(v19[0]);
      if ( v14 >= 0 )
      {
        v15 = v18;
        *a4 = *(_QWORD *)(v18 + 48);
        LdrpDereferenceModule(v15);
      }
    }
  }
  else
  {
    if ( (LdrpDebugFlags & 3) != 0 )
      LdrpLogDbgPrint(
        (unsigned int)"minkernel\\ntdll\\ldrapi.c",
        172,
        (unsigned int)"LdrLoadDll",
        0,
        "Nonpackaged process attempted to load a packaged DLL.\n");
    if ( (LdrpDebugFlags & 0x10) != 0 )
      __debugbreak();
    v14 = -1073741398;
  }
  if ( (LdrpDebugFlags & 9) != 0 )
  {
    LODWORD(v17) = v14;
    LdrpLogDbgPrint(
      (unsigned int)"minkernel\\ntdll\\ldrapi.c",
      204,
      (unsigned int)"LdrLoadDll",
      4,
      "Status: 0x%08lx\n",
      v17);
  }
  return (unsigned int)v14;
}
