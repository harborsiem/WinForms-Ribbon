# Changelog
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](https://semver.org/).

### RibbonFramework V1.1.2, RibbonTools64 V8.1.1

#### Changed (RibbonFramework)

- Optimizations
- Bugfix possible NullReferenceException in Galleries

#### Changed (RibbonTools64)

- using RibbonFramework version 1.1.2

#### Changed (Samples)

- Suppress warnings for Galleries

### RibbonFramework V1.1.1, RibbonTools64 V8.1.0

#### Changed (RibbonFramework)

- Internal optimizations
- .NET Framework (4.6.2 and later) and .NET 8 (and later) support for RCW RibbonFramework
- .NET 8 (and later) support for CCW RibbonFramework
- New Nuget package for [RCW RibbonFramework](https://www.nuget.org/packages/RibbonFramework.RCW)

### RibbonFramework V1.1.0, RibbonTools64 V8.1.0

#### Changed (RibbonFramework)

- Freeing native resources as soon as possible
- Nullable Keytips, Label, LabelDescription, TooltipTitle, TooltipDescription
- Internal optimizations
- Bugfixes

#### Changed (RibbonTools64)

- Image conversion from Svg files added. [Svg, version 3.4.7](https://www.nuget.org/packages/svg)
- Preview function now uses RibbonFramework.CCW package
- High Dpi support
- Advanced color choosing
- Internal optimizations

### RibbonFramework V1.0.0, RibbonTools64 V8.0.2

#### Changed (RibbonFramework)

- UIImage comments
- MarkupHeader (RibbonStrip) added for Embedded Resource of RibbonMarkup.h
- Parser for RibbonMarkup.h to get all resource Ids
- For all string values like Label, LabelDescription, TooltipTitle, TooltipDescription, Keytip in RibbonButton
 and other controls we can get the displayed values if you use RibbonStrip.MarkupHeader
- bugfixes and internal optimizations

#### Changed (RibbonTools64)

- Visual Basic code generation: Namespace now Global.WinForms.Ribbon in RibbonItems
- Missing H-File reported in RibbonItems.Designer

#### Samples

- Visual Basic samples added
- C# Localization modified