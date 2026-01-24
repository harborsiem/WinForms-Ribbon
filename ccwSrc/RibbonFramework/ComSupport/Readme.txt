Differences in classes, structs, interface to Microsoft WinForms project:

ComHelpers.cs:
internal static => static in class modifier
public static ComScope<ITypeInfo> GetRegisteredTypeInfo( => internal static ComScope<ITypeInfo> GetRegisteredTypeInfo(
PInvokeCore.LoadRegTypeLib => PInvoke.LoadRegTypeLib

HRESULT.cs:
=> remove internal from struct modifier

GlobalInterfaceTable.cs:
PInvokeCore.CoCreateInstance( => PInvoke.CoCreateInstance(

GlobalUsings.cs

FILETIME:
internal partial struct => partial struct

SAFEARRAY:
internal => 
PInvokeCore => PInvoke

SR.cs added

ComSafeArrayScope:
PInvokeCore => PInvoke
get indexer accepts now null IUnknown* values. Native UIRibbon set them into SAFEARRAY.

SafeArrayScope:
PInvokeCore => PInvoke
comment VARIANT parts because I don't put VARIANT here.