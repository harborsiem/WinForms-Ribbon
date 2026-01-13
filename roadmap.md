# Windows Forms Ribbon (RibbonFramework.CCW) Roadmap

## Future

### Integration in *Microsoft Windows Forms* project ?

Going forward, the Microsoft assemblies System.Private.Forms.Core and System.Windows.Forms.Primitives will be used.

For this purpose, the PROPVARIANT implementation from RibbonFramework must be integrated into the System.Private.Forms.Core assembly, analogous to VARIANT.

InternalsVisibleTo RibbonFramework must be set in both Microsoft assemblies.

=> Remove classes, structs from these Microsoft assemblies in RibbonFramework.

Integrate PROPVARIANT from ccwSrc into System.Private.Forms.Core and remove it from RibbonFramework (change PInvoke to PInvokeCore calls).

Integrate the struct extensions from PInvoke.cs into a suitable Microsoft assembly and remove them from RibbonFramework.

Replace PInvoke functions in UIImage with Microsoft assembly functions (DeleteObject, ...).
Implement COLORREF macros GetRValue, ...

Is IManagedWrapper interface necessary in 
 EventLogger,
 RecentItemsPropertySet,
 AbstractPropertySet,
 UIApplication,
 UICollection'1,
 UICollectionChangedEvent ?

### More safely aspects

Longer-term COM objects include: IUIFramework, IUIImageFromBitmap, IUIImage, IUICollection
Is it neccessary to use AgileComPointer and DisposeHelper class ?
This is done in version 1.2.0

How do I terminate RibbonFramework when an unhandled exception occurs in an application, so that all COM objects are released?
Currently, DestroyFramework is called in OnHandleDestroyed within the RibbonStrip which release the COM objects IUIFramework, IUIImageFromBitmap.

Sharing COM objects with IUIImage applies to galleries (GalleryItemPropertySet, GalleryItemEventArgs, UICollection)
or are these automatically shared by the Ribbon Framework?

### Other extensions

If embedded RibbonMarkup.h was set to RibbonStrip.MarkupHeader, we should build UIImage classes like it was done for the strings in Label, LabelDescription, ... properties. But this was more complicated because we have to check more variables like current dpi, High contrast and may be some more.

## Infos

Ribbon COM objects that were created in the RibbonFramework
- IUICommandHandler, IUIApplication (in UIApplication)
- IUISimplePropertySet (in AbstractPropertySet, RecentItemsPropertySet)
- IUICollection (in UICollection<T>)
- IUICollectionChangedEvent (in UICollectionChangedEvent)
- IUIEventLogger (in EventLogger)
- IStream (in ComManagedStream, from Microsoft Windows Forms)

Other used COM objects
- IUnknown
- IEnumUnknown
- IConnectionPointContainer
- IConnectionPoint

## Known bugs in native UIRibbon

Wrong datatype used in PROPVARIANT (VT_I4 instead VT_UI4)
- UI_CONTEXTAVAILABILITY.ContextAvailable when calling IUIFramework->GetUICommandProperty
- UI_SWATCHCOLORTYPE in ForegroundColorType and BackgroundColorType when calling IPropertyStore->GetValue (FontControl)

harborsiem/WindowsRibbon discussions#27: 4 byte Unicode characters in LabelTitle => makes a line break for last character. Remedy: Set an additional line break as last character.

Predefined SizeDefinition ButtonGroupsAndInputs did not work as documented (Medium and Large View: Button3 has wrong place without Button10, example did not compile).

Important: If you don’t add the default item to the items list of a SplitButtonGallery, the items will appear twice! This is probably a bug.

harborsiem/WindowsRibbon discussions#33: RibbonDropDownGallery and RibbonSplitButtonGallery, GalleryItemPropertySet issue with images different in Win10, Win11, x86, x64

DarkMode with FontControl is not looking good, wrong colors used.

DEFINE_UIPROPERTYKEY for DarkMode and ApplicationButtonColor not documented in UIRibbon.h

Is the documention for conversion of UI_HSBCOLOR correctly ?
