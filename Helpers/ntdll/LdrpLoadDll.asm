__int64 __fastcall LdrpLoadDll(__int64 a1, int a2, unsigned int a3, __int64 *a4) ; 40 55 53 56 57 41 56 41 57 48 8D 6C 24 88
{
  unsigned int v8; // [rsp+40h] [rbp-C0h] BYREF
  unsigned int v9; // [rsp+48h] [rbp-B8h] BYREF
  int v10; // [rsp+50h] [rbp-B0h] BYREF
  __int16 *v11; // [rsp+58h] [rbp-A8h]
  __int16 v12[128]; // [rsp+60h] [rbp-A0h] BYREF

  v8 = a3;
  LdrpLogDllState(0LL, a1, 5288LL);
  v10 = 0x1000000;
  v11 = v12;
  v12[0] = 0;
  v9 = LdrpPreprocessDllName(a1, &v10, 0LL, &v8);
  if ( (v9 & 0x80000000) == 0 )
    LdrpLoadDllInternal((__int64)&v10, a2, v8, 4, 0LL, 0LL, a4, (int *)&v9);
  if ( v12 != v11 )
    NtdllpFreeStringRoutine();
  v10 = 0x1000000;
  v11 = v12;
  v12[0] = 0;
  LdrpLogDllState(0LL, a1, 5289LL);
  return v9;
}
