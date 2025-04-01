using System;
using System.Collections.Generic;
using System.Text;

namespace WinForms.Ribbon
{
    internal enum HeaderResKeys
    {
        None = 0,
        Keytip,
        LabelTitle,
        LabelDescription,
        TooltipTitle,
        TooltipDescription,
        SmallImages,
        LargeImages,
        SmallHighContrastImages,
        LargeHighContrastImages,
        SmallImages_96,
        LargeImages_96,
        SmallHighContrastImages_96,
        LargeHighContrastImages_96,
        SmallImages_120,
        LargeImages_120,
        SmallHighContrastImages_120,
        LargeHighContrastImages_120,
        SmallImages_144,
        LargeImages_144,
        SmallHighContrastImages_144,
        LargeHighContrastImages_144,
        SmallImages_192,
        LargeImages_192,
        SmallHighContrastImages_192,
        LargeHighContrastImages_192
    }

    /// <summary>
    /// Resource Id's for a RibbonControl (IRibbonControl)
    /// </summary>
    internal class HeaderResIds
    {
        public ushort CommandId;
        //strings
        public ushort KeytipId;
        public ushort LabelTitleId;
        public ushort LabelDescriptionId;
        public ushort TooltipTitleId;
        public ushort TooltipDescriptionId;
        //images
        public ushort SmallImages;
        public ushort LargeImages;
        public ushort SmallHighContrastImages;
        public ushort LargeHighContrastImages;
        public ushort SmallImages_96;
        public ushort LargeImages_96;
        public ushort SmallHighContrastImages_96;
        public ushort LargeHighContrastImages_96;
        public ushort SmallImages_120;
        public ushort LargeImages_120;
        public ushort SmallHighContrastImages_120;
        public ushort LargeHighContrastImages_120;
        public ushort SmallImages_144;
        public ushort LargeImages_144;
        public ushort SmallHighContrastImages_144;
        public ushort LargeHighContrastImages_144;
        public ushort SmallImages_192;
        public ushort LargeImages_192;
        public ushort SmallHighContrastImages_192;
        public ushort LargeHighContrastImages_192;

        public UIImage? GetSmallImages()
        {
            return null;
        }

        public UIImage? GetLargeImages()
        {
            return null;
        }

        public UIImage? GetSmallHighContrastImages()
        {
            return null;
        }

        public UIImage? GetLargeHighContrastImages()
        {
            return null;
        }
    }
}
