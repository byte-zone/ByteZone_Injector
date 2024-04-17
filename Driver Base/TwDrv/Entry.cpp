#include <ntddk.h>




#define IOCTL_INJECT_CODE CTL_CODE(FILE_DEVICE_UNKNOWN, 0x800, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
NTSTATUS IoControlHandler(PDEVICE_OBJECT DeviceObject, PIRP Irp)
{
	UNREFERENCED_PARAMETER(DeviceObject);
	NTSTATUS status = STATUS_SUCCESS;
	PIO_STACK_LOCATION stack = IoGetCurrentIrpStackLocation(Irp);
	ULONG controlCode = stack->Parameters.DeviceIoControl.IoControlCode;

	switch (controlCode)
	{
	case IOCTL_INJECT_CODE:
			break;
		default:
			status = STATUS_INVALID_DEVICE_REQUEST;
			break;
	}

	Irp->IoStatus.Status = status;
	IoCompleteRequest(Irp, IO_NO_INCREMENT);
	return status;
}

/* Entry Point */
NTSTATUS DriverEntry(PDRIVER_OBJECT DriverObject, PUNICODE_STRING RegistryPath)
{
	UNREFERENCED_PARAMETER(RegistryPath);
	vDbgPrintExWithPrefix("[Tw] Driver loaded\n", NULL, NULL, NULL, NULL);

	NTSTATUS status;
	PDEVICE_OBJECT deviceObject;
	UNICODE_STRING deviceName = RTL_CONSTANT_STRING(L"\\Device\\MyDevice");
	status = IoCreateDevice(DriverObject, 0, &deviceName, FILE_DEVICE_UNKNOWN, 0, FALSE, &deviceObject);
	if (!NT_SUCCESS(status))
	{
		return status;
	}
	UNICODE_STRING symbolicLink = RTL_CONSTANT_STRING(L"\\DosDevices\\MyDevice");

	status = IoCreateSymbolicLink(&symbolicLink, &deviceName);
	if(!NT_SUCCESS(status))
	{
		IoDeleteDevice(deviceObject);
		return status;
	}

	DriverObject->MajorFunction[IRP_MJ_DEVICE_CONTROL] = IoControlHandler;
	return STATUS_SUCCESS;
}

VOID DriverUnload(PDRIVER_OBJECT DriverObject)
{
	UNICODE_STRING symbolicLink = RTL_CONSTANT_STRING(L"\\DosDevices\\MyDevice");
	IoDeleteSymbolicLink(&symbolicLink);
	IoDeleteDevice(DriverObject->DeviceObject);
}
