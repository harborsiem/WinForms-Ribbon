//****************************************************************************
//
//  File:       RibbonProperties.cs
//
//  Contents:   Properties related to the Windows Ribbon Framework, based on 
//              UIRibbon.idl from windows 7 SDK
//
//****************************************************************************

using System;
using System.Collections.Generic;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;

namespace Windows.Win32.UI.Ribbon
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Properties related to the Windows Ribbon Framework, based on 
    /// UIRibbon.idl from windows 7 SDK or later windows SDK
    /// </summary>
    internal static class RibbonProperties
    {
        static RibbonProperties()
        {
            InitPropertyKeyNames();
        }

        public static string? KeyToString(PROPERTYKEY key)
        {
            byte[] guid = key.fmtid.ToByteArray();
            int index = BitConverter.ToInt32(guid, 0);
            string name = RibbonProperties.GetPropertyKeyName(index);
            if (string.IsNullOrEmpty(name))
                return null;
            string vtString;
            VARENUM vt = (VARENUM)(key.pid);
            if ((vt & VARENUM.VT_VECTOR) != 0)
                vtString = "VT_VECTOR" + " | " + (vt & ~VARENUM.VT_VECTOR).ToString();
            else if ((vt & VARENUM.VT_ARRAY) != 0)
                vtString = "VT_ARRAY" + " | " + (vt & ~VARENUM.VT_ARRAY).ToString();
            else
                vtString = vt.ToString();
            return "PROPERTYKEY." + name + " : " + vtString + " : " + key.fmtid.ToString();
        }

        private static Dictionary<int, string> propertyKeyNames = new Dictionary<int, string>();

        public static string GetPropertyKeyName(int key)
        {
            if (propertyKeyNames.TryGetValue(key, out var value))
            {
                return value;
            }
            return string.Empty;
        }

        private static void InitPropertyKeyNames()
        {
            //propertyKeyNames = new Dictionary<int, string>();
            propertyKeyNames.Add(1, nameof(Enabled));
            propertyKeyNames.Add(2, nameof(LabelDescription));
            propertyKeyNames.Add(3, nameof(Keytip));
            propertyKeyNames.Add(4, nameof(Label));
            propertyKeyNames.Add(5, nameof(TooltipDescription));
            propertyKeyNames.Add(6, nameof(TooltipTitle));
            propertyKeyNames.Add(7, nameof(LargeImage));
            propertyKeyNames.Add(8, nameof(LargeHighContrastImage));
            propertyKeyNames.Add(9, nameof(SmallImage));
            propertyKeyNames.Add(10, nameof(SmallHighContrastImage));

            propertyKeyNames.Add(100, nameof(CommandId));
            propertyKeyNames.Add(101, nameof(ItemsSource));
            propertyKeyNames.Add(102, nameof(Categories));
            propertyKeyNames.Add(103, nameof(CategoryId));
            propertyKeyNames.Add(104, nameof(SelectedItem));
            propertyKeyNames.Add(105, nameof(CommandType));
            propertyKeyNames.Add(106, nameof(ItemImage));

            propertyKeyNames.Add(200, nameof(BooleanValue));
            propertyKeyNames.Add(201, nameof(DecimalValue));
            propertyKeyNames.Add(202, nameof(StringValue));
            propertyKeyNames.Add(203, nameof(MaxValue));
            propertyKeyNames.Add(204, nameof(MinValue));
            propertyKeyNames.Add(205, nameof(Increment));
            propertyKeyNames.Add(206, nameof(DecimalPlaces));
            propertyKeyNames.Add(207, nameof(FormatString));
            propertyKeyNames.Add(208, nameof(RepresentativeString));

            propertyKeyNames.Add(300, nameof(FontProperties));
            propertyKeyNames.Add(301, nameof(FontProperties_Family));
            propertyKeyNames.Add(302, nameof(FontProperties_Size));
            propertyKeyNames.Add(303, nameof(FontProperties_Bold));
            propertyKeyNames.Add(304, nameof(FontProperties_Italic));
            propertyKeyNames.Add(305, nameof(FontProperties_Underline));
            propertyKeyNames.Add(306, nameof(FontProperties_Strikethrough));
            propertyKeyNames.Add(307, nameof(FontProperties_VerticalPositioning));
            propertyKeyNames.Add(308, nameof(FontProperties_ForegroundColor));
            propertyKeyNames.Add(309, nameof(FontProperties_BackgroundColor));
            propertyKeyNames.Add(310, nameof(FontProperties_ForegroundColorType));
            propertyKeyNames.Add(311, nameof(FontProperties_BackgroundColorType));
            propertyKeyNames.Add(312, nameof(FontProperties_ChangedProperties));
            propertyKeyNames.Add(313, nameof(FontProperties_DeltaSize));

            propertyKeyNames.Add(350, nameof(RecentItems));
            propertyKeyNames.Add(351, nameof(Pinned));

            propertyKeyNames.Add(400, nameof(Color));
            propertyKeyNames.Add(401, nameof(ColorType));
            propertyKeyNames.Add(402, nameof(ColorMode));
            propertyKeyNames.Add(403, nameof(ThemeColorsCategoryLabel));
            propertyKeyNames.Add(404, nameof(StandardColorsCategoryLabel));
            propertyKeyNames.Add(405, nameof(RecentColorsCategoryLabel));
            propertyKeyNames.Add(406, nameof(AutomaticColorLabel));
            propertyKeyNames.Add(407, nameof(NoColorLabel));
            propertyKeyNames.Add(408, nameof(MoreColorsLabel));
            propertyKeyNames.Add(409, nameof(ThemeColors));
            propertyKeyNames.Add(410, nameof(StandardColors));
            propertyKeyNames.Add(411, nameof(ThemeColorsTooltips));
            propertyKeyNames.Add(412, nameof(StandardColorsTooltips));

            propertyKeyNames.Add(1000, nameof(Viewable));
            propertyKeyNames.Add(1001, nameof(Minimized));
            propertyKeyNames.Add(1002, nameof(QuickAccessToolbarDock));

            propertyKeyNames.Add(1100, nameof(ContextAvailable));

            propertyKeyNames.Add(2000, nameof(GlobalBackgroundColor));
            propertyKeyNames.Add(2001, nameof(GlobalHighlightColor));
            propertyKeyNames.Add(2002, nameof(GlobalTextColor));
            propertyKeyNames.Add(2003, nameof(ApplicationButtonColor));
            propertyKeyNames.Add(2004, nameof(DarkModeRibbon));
        }

        // Core command properties
        public static readonly PROPERTYKEY Enabled = CreateRibbonPropertyKey(1, VARENUM.VT_BOOL);
        public static readonly PROPERTYKEY LabelDescription = CreateRibbonPropertyKey(2, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY Keytip = CreateRibbonPropertyKey(3, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY Label = CreateRibbonPropertyKey(4, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY TooltipDescription = CreateRibbonPropertyKey(5, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY TooltipTitle = CreateRibbonPropertyKey(6, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY LargeImage = CreateRibbonPropertyKey(7, VARENUM.VT_UNKNOWN); // IUIImage
        public static readonly PROPERTYKEY LargeHighContrastImage = CreateRibbonPropertyKey(8, VARENUM.VT_UNKNOWN); // IUIImage
        public static readonly PROPERTYKEY SmallImage = CreateRibbonPropertyKey(9, VARENUM.VT_UNKNOWN); // IUIImage
        public static readonly PROPERTYKEY SmallHighContrastImage = CreateRibbonPropertyKey(10, VARENUM.VT_UNKNOWN); // IUIImage

        // Collections properties
        public static readonly PROPERTYKEY CommandId = CreateRibbonPropertyKey(100, VARENUM.VT_UI4);
        public static readonly PROPERTYKEY ItemsSource = CreateRibbonPropertyKey(101, VARENUM.VT_UNKNOWN); // IEnumUnknown or IUICollection (IUISimplePropertySet)
        public static readonly PROPERTYKEY Categories = CreateRibbonPropertyKey(102, VARENUM.VT_UNKNOWN); // IEnumUnknown or IUICollection (IUISimplePropertySet)
        public static readonly PROPERTYKEY CategoryId = CreateRibbonPropertyKey(103, VARENUM.VT_UI4);
        public static readonly PROPERTYKEY SelectedItem = CreateRibbonPropertyKey(104, VARENUM.VT_UI4);
        public static readonly PROPERTYKEY CommandType = CreateRibbonPropertyKey(105, VARENUM.VT_UI4);
        public static readonly PROPERTYKEY ItemImage = CreateRibbonPropertyKey(106, VARENUM.VT_UNKNOWN); // IUIImage

        // Control properties
        public static readonly PROPERTYKEY BooleanValue = CreateRibbonPropertyKey(200, VARENUM.VT_BOOL);
        public static readonly PROPERTYKEY DecimalValue = CreateRibbonPropertyKey(201, VARENUM.VT_DECIMAL);
        public static readonly PROPERTYKEY StringValue = CreateRibbonPropertyKey(202, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY MaxValue = CreateRibbonPropertyKey(203, VARENUM.VT_DECIMAL);
        public static readonly PROPERTYKEY MinValue = CreateRibbonPropertyKey(204, VARENUM.VT_DECIMAL);
        public static readonly PROPERTYKEY Increment = CreateRibbonPropertyKey(205, VARENUM.VT_DECIMAL);
        public static readonly PROPERTYKEY DecimalPlaces = CreateRibbonPropertyKey(206, VARENUM.VT_UI4);
        public static readonly PROPERTYKEY FormatString = CreateRibbonPropertyKey(207, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY RepresentativeString = CreateRibbonPropertyKey(208, VARENUM.VT_LPWSTR);

        // Font control properties
        public static readonly PROPERTYKEY FontProperties = CreateRibbonPropertyKey(300, VARENUM.VT_UNKNOWN); // IPropertyStore
        public static readonly PROPERTYKEY FontProperties_Family = CreateRibbonPropertyKey(301, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY FontProperties_Size = CreateRibbonPropertyKey(302, VARENUM.VT_DECIMAL);
        public static readonly PROPERTYKEY FontProperties_Bold = CreateRibbonPropertyKey(303, VARENUM.VT_UI4); // UI_FONTPROPERTIES
        public static readonly PROPERTYKEY FontProperties_Italic = CreateRibbonPropertyKey(304, VARENUM.VT_UI4); // UI_FONTPROPERTIES
        public static readonly PROPERTYKEY FontProperties_Underline = CreateRibbonPropertyKey(305, VARENUM.VT_UI4); // UI_FONTPROPERTIES
        public static readonly PROPERTYKEY FontProperties_Strikethrough = CreateRibbonPropertyKey(306, VARENUM.VT_UI4); // UI_FONTPROPERTIES
        public static readonly PROPERTYKEY FontProperties_VerticalPositioning = CreateRibbonPropertyKey(307, VARENUM.VT_UI4); // UI_FONTVERTICALPOSITION
        public static readonly PROPERTYKEY FontProperties_ForegroundColor = CreateRibbonPropertyKey(308, VARENUM.VT_UI4); // COLORREF
        public static readonly PROPERTYKEY FontProperties_BackgroundColor = CreateRibbonPropertyKey(309, VARENUM.VT_UI4); // COLORREF
        public static readonly PROPERTYKEY FontProperties_ForegroundColorType = CreateRibbonPropertyKey(310, VARENUM.VT_UI4); // UI_SWATCHCOLORTYPE
        public static readonly PROPERTYKEY FontProperties_BackgroundColorType = CreateRibbonPropertyKey(311, VARENUM.VT_UI4); // UI_SWATCHCOLORTYPE
        public static readonly PROPERTYKEY FontProperties_ChangedProperties = CreateRibbonPropertyKey(312, VARENUM.VT_UNKNOWN); // IPropertyStore
        public static readonly PROPERTYKEY FontProperties_DeltaSize = CreateRibbonPropertyKey(313, VARENUM.VT_UI4); // UI_FONTDELTASIZE 

        // Application menu properties
        public static readonly PROPERTYKEY RecentItems = CreateRibbonPropertyKey(350, (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN)); //SAFEARRAY, IUISimplePropertySet
        public static readonly PROPERTYKEY Pinned = CreateRibbonPropertyKey(351, VARENUM.VT_BOOL);

        // Color picker properties
        public static readonly PROPERTYKEY Color = CreateRibbonPropertyKey(400, VARENUM.VT_UI4); // COLORREF
        public static readonly PROPERTYKEY ColorType = CreateRibbonPropertyKey(401, VARENUM.VT_UI4); // UI_SWATCHCOLORTYPE
        public static readonly PROPERTYKEY ColorMode = CreateRibbonPropertyKey(402, VARENUM.VT_UI4); // UI_SWATCHCOLORMODE
        public static readonly PROPERTYKEY ThemeColorsCategoryLabel = CreateRibbonPropertyKey(403, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY StandardColorsCategoryLabel = CreateRibbonPropertyKey(404, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY RecentColorsCategoryLabel = CreateRibbonPropertyKey(405, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY AutomaticColorLabel = CreateRibbonPropertyKey(406, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY NoColorLabel = CreateRibbonPropertyKey(407, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY MoreColorsLabel = CreateRibbonPropertyKey(408, VARENUM.VT_LPWSTR);
        public static readonly PROPERTYKEY ThemeColors = CreateRibbonPropertyKey(409, (VARENUM.VT_VECTOR | VARENUM.VT_UI4));
        public static readonly PROPERTYKEY StandardColors = CreateRibbonPropertyKey(410, (VARENUM.VT_VECTOR | VARENUM.VT_UI4));
        public static readonly PROPERTYKEY ThemeColorsTooltips = CreateRibbonPropertyKey(411, (VARENUM.VT_VECTOR | VARENUM.VT_LPWSTR));
        public static readonly PROPERTYKEY StandardColorsTooltips = CreateRibbonPropertyKey(412, (VARENUM.VT_VECTOR | VARENUM.VT_LPWSTR));

        // Ribbon properties
        public static readonly PROPERTYKEY Viewable = CreateRibbonPropertyKey(1000, VARENUM.VT_BOOL);
        public static readonly PROPERTYKEY Minimized = CreateRibbonPropertyKey(1001, VARENUM.VT_BOOL);
        public static readonly PROPERTYKEY QuickAccessToolbarDock = CreateRibbonPropertyKey(1002, VARENUM.VT_UI4);

        // Contextual tabset properties
        public static readonly PROPERTYKEY ContextAvailable = CreateRibbonPropertyKey(1100, VARENUM.VT_UI4);

        // Color properties
        // These are specified using hue, saturation and brightness.  The background, highlight and text colors of all controls
        // will be adjusted to the specified hues and saturations.  The brightness does not represent a component of a specific
        // color, but rather the overall brightness of the controls - 0x00 is darkest, 0xFF is lightest.
        public static readonly PROPERTYKEY GlobalBackgroundColor = CreateRibbonPropertyKey(2000, VARENUM.VT_UI4); // UI_HSBCOLOR
        public static readonly PROPERTYKEY GlobalHighlightColor = CreateRibbonPropertyKey(2001, VARENUM.VT_UI4); // UI_HSBCOLOR
        public static readonly PROPERTYKEY GlobalTextColor = CreateRibbonPropertyKey(2002, VARENUM.VT_UI4); // UI_HSBCOLOR
        public static readonly PROPERTYKEY ApplicationButtonColor = CreateRibbonPropertyKey(2003, VARENUM.VT_UI4); // UI_HSBCOLOR
        public static readonly PROPERTYKEY DarkModeRibbon = CreateRibbonPropertyKey(2004, VARENUM.VT_BOOL);

        public static PROPERTYKEY CreateRibbonPropertyKey(Int32 index, VARENUM vt)
        {
            return new PROPERTYKEY()
            {
                fmtid = new Guid(index, 0x7363, 0x696e, 0x84, 0x41, 0x79, 0x8a, 0xcf, 0x5a, 0xeb, 0xb7),
                pid = (uint)vt
            };
        }

        /// <summary>
        /// Get the name of a given PROPERTYKEY
        /// </summary>
        /// <param name="propertyKey">PROPERTYKEY</param>
        /// <returns>Name of the PROPERTYKEY</returns>
        /// <remarks>This function is used for debugging.</remarks>
        public static string GetPropertyKeyName(in PROPERTYKEY propertyKey)
        {
            byte[] guid = propertyKey.fmtid.ToByteArray();
            int index = BitConverter.ToInt32(guid, 0);
            return GetPropertyKeyName(index);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
