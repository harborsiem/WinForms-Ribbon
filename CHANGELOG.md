# Changelog
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](https://semver.org/).

### RibbonFramework V1.0.0, RibbonTools64 V8.1.0

#### Changed (RibbonFramework)

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
 and other controls we can get the displayed values if you use MarkupHeader
- bugfixes and internal optimizations

#### Changed (RibbonTools64)

- Visual Basic code generation: Namespace now Global.WinForms.Ribbon in RibbonItems
- Missing H-File reported in RibbonItems.Designer

#### Samples

- Visual Basic samples added
- C# Localization modified