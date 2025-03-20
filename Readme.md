# WinForms-Ribbon for .NET8 and later

*Windows Ribbon Framework* for WinForms

**WinForms-Ribbon** is a .NET wrapper for *Microsoft Windows Ribbon Framework* library UIRibbon.dll. It will allow WinForms developers to use *Microsoft Windows Ribbon Framework* library in their WinForms applications.
Supported Windows versions are Windows 10 and later versions.

See also [Microsoft documentation](https://learn.microsoft.com/en-us/windows/win32/windowsribbon/-uiplat-windowsribbon-entry)

This project is a redesign of the WindowsRibbon project. It uses [Microsoft CsWin32](https://github.com/microsoft/CsWin32) for COM interfaces and all other native functions.
It also uses some classes and structs from Microsoft WinForms project on [Github](https://github.com/dotnet/winforms) for easier handling of COM interfaces.

## What's changed

- Namespace for all classes, structs, enums changed to **WinForms.Ribbon**
- Name of class Ribbon changed to RibbonStrip similar to ToolStrip, MenuStrip.
- Events changed from ExecuteEvent to specific event names.
- Some events added
- Invoke functions for events integrated. No Invoke functions for ItemsSourceReady, CategoriesReady 
- Easier persistence of Qat settings with new RibbonStrip property QatSettingsFile.
- Two different RibbonFramework libraries.
  1. One library has Com callable wrappers (CCW) for all COM interfaces for more performance. (Less Garbage collection, ...) 
  2. The other library has Runtime callable wrappers (RCW) for all COM interfaces. This is similar to WindowsRibbon.
- RibbonStrip property ResourceName changed to MarkupResource

## **Project Description**

The project includes the library RibbonFramework, which adds support for *Microsoft Windows Ribbon Framework* to WinForms application.

Read the Wiki Pages for more details on how to use the *RibbonFramework*. (Todo)

Note: You must have Windows 7 SDK (or any later SDK) installed in order to compile (build) the project.
You also have to install C++ Tools in Visual Studio.

For easier designing, building and previewing the Windows Ribbon Framework there is gui and console based tool called **RibbonTools64**

Todo

## Installation:

Todo