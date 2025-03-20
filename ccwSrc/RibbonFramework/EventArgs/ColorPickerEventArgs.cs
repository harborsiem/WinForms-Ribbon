using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Shell.PropertiesSystem;

namespace WinForms.Ribbon
{
    /// <summary>
    /// The EventArgs for RibbonDropDownColorPicker
    /// </summary>
    public sealed class ColorPickerEventArgs : EventArgs
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="colorType"></param>
        /// <param name="color"></param>
        private ColorPickerEventArgs(UI_SWATCHCOLORTYPE colorType, Color? color)
        {
            ColorType = (SwatchColorType)colorType;
            RGBColor = color;
        }

        /// <summary>
        /// The ColorType
        /// </summary>
        public SwatchColorType ColorType { get; private set; }

        /// <summary>
        /// Selected Color
        /// </summary>
        public Color? RGBColor { get; private set; }

        /// <summary>
        /// Creates a ColorPickerEventArgs from ExecuteEventArgs of a RibbonDropDownColorPicker event
        /// </summary>
        /// <param name="sender">Parameter from event: sender</param>
        /// <param name="e">Parameters from event: ExecuteEventArgs</param>
        /// <returns></returns>
        internal static unsafe ColorPickerEventArgs? Create(object sender, ExecuteEventArgs e)
        {
            if (!(sender is RibbonDropDownColorPicker))
                throw new ArgumentException("Not a RibbonDropDownColorPicker", nameof(sender));
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            if (e.Key.HasValue && e.CurrentValue.HasValue)
            {
                return Create(e.Key.Value, e.CurrentValue.Value, e.CommandExecutionProperties);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currentValue"></param>
        /// <param name="commandExecutionProperties"></param>
        /// <returns></returns>
        internal static unsafe ColorPickerEventArgs Create(in PROPERTYKEY key, in PROPVARIANT currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            Color? color = null;
            uint uintResult = (uint)currentValue; //PropVariantToUInt32
            UI_SWATCHCOLORTYPE colorType = (UI_SWATCHCOLORTYPE)uintResult;
            PROPVARIANT propvar = PROPVARIANT.Empty;
            if (commandExecutionProperties != null)
            {
                fixed (PROPERTYKEY* pColor = &RibbonProperties.Color)
                    commandExecutionProperties->GetValue(pColor, &propvar);
                uint colorref = (uint)propvar; //PropVariantToUInt32
                color = ColorTranslator.FromWin32((int)colorref);
            }
            ColorPickerEventArgs e = new ColorPickerEventArgs(colorType, color);
            return e;
        }
    }
}
