# Windows Ribbon Framework for WinForms .NET (Core)

The package contains .NET wrapper classes for the COM based Windows Ribbon Framework.
Microsofts Windows Ribbon Framework is available since Windows 7.
This .NET wrapper is a COM callable wrapper (CCW) and is present in the library RibbonFramework.dll which is inside the package.
The CCW wrapper has less Garbage collections and is faster then a RCW wrapper.

More informations about usage are inside Github project page [WinForms-Ribbon](https://github.com/harborsiem/WinForms-Ribbon) with samples and wiki pages.

For developing the user interface you have to write XAML like markup file with commands and views first.
Empty  markup file:

```xml

 <?xml version='1.0' encoding='utf-8'?>
<Application xmlns='http://schemas.microsoft.com/windows/2009/Ribbon'>
    <Application.Commands>
    </Application.Commands>

    <Application.Views>
        <ContextPopup>
        </ContextPopup>
    	<Ribbon>
    	</Ribbon>
    </Application.Views>
</Application>

```

This can be done with the application RibbonTools64, which helps designing, building and previewing the user interface.
This application is available on the project page as a MSI setup with full source code.

After finishing the first step, you create a .NET Windows Form project with this Nuget package and place a RibbonStrip control to the Form.
For next steps see documentation and samples on the project page.
