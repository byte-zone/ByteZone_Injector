#include "./includes.h"


const wchar_t* DriverPath = L"C:\\Windows\\System32\\Drivers\\gdrv.sys";

int wmain(int argc, wchar_t** argv)
{

	NTSTATUS Status = STATUS_UNSUCCESSFUL;


	
	if (DropDriverFromBytes(DriverPath)) // Drop driver from bytes 
	{
		Status = WindLoadDriver((PWCHAR)DriverPath, argv[1], FALSE);

		if (NT_SUCCESS(Status))
		{
			printf("Driver loadded\n");
			DeleteFile((PWSTR)DriverPath);
		}

		if (!NT_SUCCESS(Status))
			printf("Error: %08X\n", Status);

		return false;
	}
}

