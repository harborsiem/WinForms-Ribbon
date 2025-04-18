using System;
using System.Collections.Generic;
using System.Text;

namespace WinForms.Ribbon
{
    //internal enum MarkupResKeys
    //{
    //    None = 0,
    //    Keytip,
    //    LabelTitle,
    //    LabelDescription,
    //    TooltipTitle,
    //    TooltipDescription,
    //    SmallImages,
    //    LargeImages,
    //    SmallHighContrastImages,
    //    LargeHighContrastImages,
    //    SmallImages_96,
    //    LargeImages_96,
    //    SmallHighContrastImages_96,
    //    LargeHighContrastImages_96,
    //    SmallImages_120,
    //    LargeImages_120,
    //    SmallHighContrastImages_120,
    //    LargeHighContrastImages_120,
    //    SmallImages_144,
    //    LargeImages_144,
    //    SmallHighContrastImages_144,
    //    LargeHighContrastImages_144,
    //    SmallImages_192,
    //    LargeImages_192,
    //    SmallHighContrastImages_192,
    //    LargeHighContrastImages_192
    //}

    /// <summary>
    /// Resource Id's for a RibbonControl (IRibbonControl)
    /// </summary>
    public class MarkupResIds
    {
        internal ushort CommandId;
        /// <summary>
        /// CommandName from Markup file
        /// </summary>
        public string CommandName { get; internal set; }
        //strings
        /// <summary>
        /// Id of Keytip from Markup file (0 = not set).
        /// </summary>
        public ushort KeytipId { get; internal set; }
        /// <summary>
        /// Id of LabelTitle from Markup file (0 = not set).
        /// </summary>
        public ushort LabelTitleId { get; internal set; }
        /// <summary>
        /// Id of LabelDescription from Markup file (0 = not set).
        /// </summary>
        public ushort LabelDescriptionId { get; internal set; }
        /// <summary>
        /// Id of TooltipTitle from Markup file (0 = not set).
        /// </summary>
        public ushort TooltipTitleId { get; internal set; }
        /// <summary>
        /// Id of TooltipDescription from Markup file (0 = not set).
        /// </summary>
        public ushort TooltipDescriptionId { get; internal set; }
        //images
        /// <summary>
        /// Id of SmallImages from Markup file (0 = not set).
        /// </summary>
        public ushort SmallImages { get; internal set; }
        /// <summary>
        /// Id of LargeImages from Markup file (0 = not set).
        /// </summary>
        public ushort LargeImages { get; internal set; }
        /// <summary>
        /// Id of SmallHighContrastImages from Markup file (0 = not set).
        /// </summary>
        public ushort SmallHighContrastImages { get; internal set; }
        /// <summary>
        /// Id of LargeHighContrastImages from Markup file (0 = not set).
        /// </summary>
        public ushort LargeHighContrastImages { get; internal set; }
        /// <summary>
        /// Id of SmallImages, DPI = 96, from Markup file (0 = not set).
        /// </summary>
        public ushort SmallImages_96 { get; internal set; }
        /// <summary>
        /// Id of LargeImages, DPI = 96, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeImages_96 { get; internal set; }
        /// <summary>
        /// Id of SmallHighContrastImages, DPI = 96, from Markup file (0 = not set).
        /// </summary>
        public ushort SmallHighContrastImages_96 { get; internal set; }
        /// <summary>
        /// Id of LargeHighContrastImages, DPI = 96, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeHighContrastImages_96 { get; internal set; }
        /// <summary>
        /// Id of SmallImages, DPI = 120 from Markup file (0 = not set).
        /// </summary>
        public ushort SmallImages_120 { get; internal set; }
        /// <summary>
        /// Id of LargeImages, DPI = 120, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeImages_120 { get; internal set; }
        /// <summary>
        /// Id of SmallHighContrastImages, DPI = 120, from Markup file (0 = not set).
        /// </summary>
        public ushort SmallHighContrastImages_120 { get; internal set; }
        /// <summary>
        /// Id of LargeHighContrastImages, DPI = 120, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeHighContrastImages_120 { get; internal set; }
        /// <summary>
        /// Id of SmallImages, DPI = 144, from Markup file (0 = not set).
        /// </summary>
        public ushort SmallImages_144 { get; internal set; }
        /// <summary>
        /// Id of LargeImages, DPI = 144, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeImages_144 { get; internal set; }
        /// <summary>
        /// Id of SmallHighContrastImages, DPI = 144, from Markup file (0 = not set).
        /// </summary>
        public ushort SmallHighContrastImages_144 { get; internal set; }
        /// <summary>
        /// Id of LargeHighContrastImages, DPI = 144, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeHighContrastImages_144 { get; internal set; }
        /// <summary>
        /// Id of SmallImages, DPI = 192, from Markup file (0 = not set).
        /// </summary>
        public ushort SmallImages_192 { get; internal set; }
        /// <summary>
        /// Id of LargeImages, DPI = 192, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeImages_192 { get; internal set; }
        /// <summary>
        /// Id of SmallHighContrastImages, DPI = 192, from Markup file (0 = not set).
        /// </summary>
        public ushort SmallHighContrastImages_192 { get; internal set; }
        /// <summary>
        /// Id of LargeHighContrastImages, DPI = 192, from Markup file (0 = not set).
        /// </summary>
        public ushort LargeHighContrastImages_192 { get; internal set; }

        internal UIImage? GetSmallImages(RibbonStrip ribbon)
        {
            return null;
        }

        internal UIImage? GetLargeImages(RibbonStrip ribbon)
        {
            return null;
        }

        internal UIImage? GetSmallHighContrastImages(RibbonStrip ribbon)
        {
            return null;
        }

        internal UIImage? GetLargeHighContrastImages(RibbonStrip ribbon)
        {
            return null;
        }
    }
}
