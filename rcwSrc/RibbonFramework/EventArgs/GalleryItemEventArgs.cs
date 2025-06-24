//#define WithDefault

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Variant;

namespace WinForms.Ribbon
{
    /// <summary>
    /// The EventArgs for GalleryItem controls  (RibbonComboBox, ...)
    /// </summary>
    public sealed class GalleryItemEventArgs : EventArgs
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="selected"></param>
        private GalleryItemEventArgs(SelectedItem<GalleryItemPropertySet> selected)
        {
            SelectedItem = selected;
        }

        /// <summary>
        /// The selected Item index
        /// </summary>
        public SelectedItem<GalleryItemPropertySet> SelectedItem { get; private set; }

        /// <summary>
        /// Creates a GalleryItemEventArgs from ExecuteEventArgs of a Ribbon Gallery Control (RibbonComboBox, ...) event
        /// </summary>
        /// <param name="sender">Parameter from event: sender</param>
        /// <param name="e">Parameters from event: ExecuteEventArgs</param>
        /// <returns></returns>
        internal static GalleryItemEventArgs? Create(object sender, ExecuteEventArgs e)
        {
            bool isItemClass = false;
            if (sender is RibbonComboBox cBox)
            {
                isItemClass = true;
            }
            if (!isItemClass && sender is RibbonDropDownGallery ddGallery)
            {
                if ((UI_COMMANDTYPE)ddGallery.CommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COLLECTION)
                    isItemClass = true;
            }
            if (!isItemClass && sender is RibbonSplitButtonGallery sbGallery)
            {
                if ((UI_COMMANDTYPE)sbGallery.CommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COLLECTION)
                    isItemClass = true;
            }
            if (!isItemClass && sender is RibbonInRibbonGallery irGallery)
            {
                if ((UI_COMMANDTYPE)irGallery.CommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COLLECTION)
                    isItemClass = true;
            }
            if (!isItemClass)
                throw new ArgumentException("Not an ItemsControl", nameof(sender));
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
        internal static unsafe GalleryItemEventArgs? Create(in PROPERTYKEY key, in PROPVARIANT currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            GalleryItemPropertySet? propSet;
            SelectedItem<GalleryItemPropertySet> selected;
            int selectedItemIndex = -1;
            if (key == RibbonProperties.SelectedItem)
            {
                // get selected item index
                uint uintResult = (uint)currentValue; //PropVariantToUInt32
                selectedItemIndex = (int)uintResult;
                propSet = GetGalleryItemProperty(commandExecutionProperties);
                selected = new SelectedItem<GalleryItemPropertySet>(selectedItemIndex, propSet);
                GalleryItemEventArgs e = new GalleryItemEventArgs(selected);
                return e;
            }
            return null;
        }

        /// <summary>
        /// returns a GalleryItemPropertySet from IUISimplePropertySet
        /// </summary>
        /// <param name="commandExecutionProperties"></param>
        /// <returns>GalleryItemPropertySet</returns>
        internal static unsafe GalleryItemPropertySet? GetGalleryItemProperty(IUISimplePropertySet? commandExecutionProperties)
        {
            GalleryItemPropertySet? propSet;
            if (commandExecutionProperties != null)
            {
                propSet = commandExecutionProperties as GalleryItemPropertySet;
            }
            else
            {
                return null;
            }

            //We need this part for a RibbonComboBox with markup IsEditable
            //but this ComboBox has no image

            if (propSet == null)
                propSet = new GalleryItemPropertySet(commandExecutionProperties);

            return propSet;
        }
    }
}
