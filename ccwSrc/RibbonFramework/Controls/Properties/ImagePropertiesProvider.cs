//*****************************************************************************
//
//  File:       ImagePropertiesProvider.cs
//
//  Contents:   Definition for image properties provider 
//
//*****************************************************************************

using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.System.Com;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for image properties provider interface
    /// </summary>
    public unsafe interface IImagePropertiesProvider
    {
        /// <summary>
        /// Large image property
        /// </summary>
        UIImage? LargeImage { get; set; }

        /// <summary>
        /// Small image property
        /// </summary>
        UIImage? SmallImage { get; set; }

        /// <summary>
        /// Large high contrast image property
        /// </summary>
        UIImage? LargeHighContrastImage { get; set; }

        /// <summary>
        /// Small high contrast image property
        /// </summary>
        UIImage? SmallHighContrastImage { get; set; }
    }

    /// <summary>
    /// Implementation of IImagePropertiesProvider
    /// </summary>
    public sealed unsafe class ImagePropertiesProvider : BasePropertiesProvider, IImagePropertiesProvider
    {
        /// <summary>
        /// ImagePropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        /// <param name="item">ribbon control</param>
        public ImagePropertiesProvider(RibbonStrip ribbon, uint commandId, RibbonStripItem item)
            : base(ribbon, commandId)
        {
            _item = item;
            // add supported properties
            _supportedProperties.Add(RibbonProperties.LargeImage);
            _supportedProperties.Add(RibbonProperties.SmallImage);
            _supportedProperties.Add(RibbonProperties.LargeHighContrastImage);
            _supportedProperties.Add(RibbonProperties.SmallHighContrastImage);
        }

        private readonly RibbonStripItem _item;
        private UIImage? _largeImage;
        private UIImage? _smallImage;
        private UIImage? _largeHighContrastImage;
        private UIImage? _smallHighContrastImage;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            fixed (PROPVARIANT* newValueLocal = &newValue) { }
            if (key == RibbonProperties.LargeImage)
            {
                if (_largeImage != null)
                {
                    UIPropVariant.UIInitPropertyFromImage(RibbonProperties.LargeImage, _largeImage.UIImageHandle, out newValue);
                }
            }
            else if (key == RibbonProperties.SmallImage)
            {
                if (_smallImage != null)
                {
                    UIPropVariant.UIInitPropertyFromImage(RibbonProperties.SmallImage, _smallImage.UIImageHandle, out newValue);
                }
            }
            else if (key == RibbonProperties.LargeHighContrastImage)
            {
                if (_largeHighContrastImage != null)
                {
                    UIPropVariant.UIInitPropertyFromImage(RibbonProperties.LargeHighContrastImage, _largeHighContrastImage.UIImageHandle, out newValue);
                }
            }
            else if (key == RibbonProperties.SmallHighContrastImage)
            {
                if (_smallHighContrastImage != null)
                {
                    UIPropVariant.UIInitPropertyFromImage(RibbonProperties.SmallHighContrastImage, _smallHighContrastImage.UIImageHandle, out newValue);
                }
            }

            return HRESULT.S_OK;
        }

        #region IImagePropertiesProvider Members

        /// <summary>
        /// Large image property
        /// </summary>
        public unsafe UIImage? LargeImage
        {
            get
            {
                return _largeImage;
            }
            set
            {
                _largeImage = value;
                if (_ribbon.Framework != null)
                {
                    fixed (PROPERTYKEY* pLargeImage = &RibbonProperties.LargeImage)
                    {
                        HRESULT hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pLargeImage);
                    }
                }
            }
        }

        /// <summary>
        /// Small image property
        /// </summary>
        public unsafe UIImage? SmallImage
        {
            get
            {
                return _smallImage;
            }
            set
            {
                _smallImage = value;
                if (_ribbon.Framework != null)
                {
                    fixed (PROPERTYKEY* pSmallImage = &RibbonProperties.SmallImage)
                    {
                        HRESULT hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pSmallImage);
                    }
                }
            }
        }

        /// <summary>
        /// Large high contrast image property
        /// </summary>
        public unsafe UIImage? LargeHighContrastImage
        {
            get
            {
                return _largeHighContrastImage;
            }
            set
            {
                _largeHighContrastImage = value;
                if (_ribbon.Framework != null)
                {
                    fixed (PROPERTYKEY* pLargeHighContrastImage = &RibbonProperties.LargeHighContrastImage)
                    {
                        HRESULT hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pLargeHighContrastImage);
                    }
                }
            }
        }

        /// <summary>
        /// Small high contrast image property
        /// </summary>
        public unsafe UIImage? SmallHighContrastImage
        {
            get
            {
                return _smallHighContrastImage;
            }
            set
            {
                _smallHighContrastImage = value;
                if (_ribbon.Framework != null)
                {
                    fixed (PROPERTYKEY* pSmallHighContrastImage = &RibbonProperties.SmallHighContrastImage)
                    {
                        HRESULT hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pSmallHighContrastImage);
                    }
                }
            }
        }

        #endregion
    }
}
