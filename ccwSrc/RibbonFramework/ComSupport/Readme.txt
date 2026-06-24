Differences in classes, structs, interface to Microsoft WinForms project:

ComHelpers.cs:
PInvokeCore.LoadRegTypeLib => PInvoke.LoadRegTypeLib
1 modification depends on Cswin version 3.296

GlobalInterfaceTable.cs:
PInvokeCore.CoCreateInstance( => PInvoke.CoCreateInstance(

GlobalUsings.cs

SAFEARRAY:
PInvokeCore => PInvoke

ComSafeArrayScope:
PInvokeCore => PInvoke
get indexer accepts now null IUnknown* values. Native UIRibbon set null values into SAFEARRAY.

SafeArrayScope:
PInvokeCore => PInvoke
comment VARIANT parts because I don't put VARIANT into RibbonFramework project.