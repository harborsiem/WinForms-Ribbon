//*****************************************************************************
//
//  File:       RibbonQuickAccessToolbar.cs
//
//  Contents:   Helper class that wraps the ribbon quick access toolbar.
//
//*****************************************************************************

using System;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for qat property provider interface
    /// </summary>
    internal interface IQatPropertyProvider
    {
        /// <summary>
        /// Items source property
        /// </summary>
        IUICollection? ItemsSource { get; }
    }

    /// <summary>
    /// Helper class that wraps the ribbon quick access toolbar.
    /// </summary>
    public sealed class RibbonQuickAccessToolbar : RibbonStripItem,
        IExecuteEventsProvider, IQatPropertyProvider
    {
        /// <summary>
        /// handler for the customize button
        /// </summary>
        private RibbonButton? _customizeButton;

        /// <summary>
        /// Initializes a new instance of the Ribbon QuickAccessToolbar (QAT)
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">Command id attached to this control</param>
        public RibbonQuickAccessToolbar(RibbonStrip ribbon, uint commandId) : base(ribbon, commandId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Ribbon QuickAccessToolbar (QAT)
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">Command id attached to this control</param>
        /// <param name="customizeCommandId">Customize Command id attached to this control</param>
        public RibbonQuickAccessToolbar(RibbonStrip ribbon, uint commandId, uint customizeCommandId) : this(ribbon, commandId)
        {
            _customizeButton = new RibbonButton(ribbon, customizeCommandId);
        }

        #region IRibbonControl Members

        private protected override unsafe HRESULT ExecuteImpl(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            return HRESULT.S_OK;
        }

        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            fixed (PROPVARIANT* newValueLocal = &newValue) { }
            if (key == RibbonProperties.ItemsSource)
            {
                if (currentValue is not null && QatItemsSource == null)
                {
                    PROPVARIANT currentValueLocal = *currentValue;
                    IUICollection cpCollection;
                    UIPropVariant.UIPropertyToInterface<IUICollection>(RibbonProperties.ItemsSource, currentValueLocal, out cpCollection!);
                    //(*currentValue).Clear(); //PropVariantClear ??? => no
                    QatItemsSource = new UICollection<QatCommandPropertySet>(cpCollection, this, CollectionType.QatItemsSource);
                }
            }
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Items source property
        /// </summary>
        IUICollection? IQatPropertyProvider.ItemsSource
        {
            get
            {
                if (Ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = Ribbon.Framework.GetUICommandProperty(CommandId, RibbonProperties.ItemsSource, out propvar);
                    if (hr.Succeeded)
                    {
                        IUICollection result;
                        UIPropVariant.UIPropertyToInterface<IUICollection>(RibbonProperties.ItemsSource, propvar, out result!);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Managed Items source property
        /// </summary>
        public UICollection<QatCommandPropertySet>? QatItemsSource
        {
            get; private set;
        }

        #endregion

        /// <summary>
        /// Invalidate QatItemsSource or ItemsSource if one change a value
        /// </summary>
        public unsafe void InvalidateItemsSource()
        {
            if (Ribbon.Framework != null)
            {
                fixed (PROPERTYKEY* pItemsSource = &RibbonProperties.ItemsSource)
                    Ribbon.Framework.InvalidateUICommand(CommandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pItemsSource);
            }
        }

        #region IExecuteEventsProvider Members

        /// <summary>
        /// The customizeButton Click event
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IExecuteEventsProvider.ExecuteEvent
        {
            add
            {
                if (_customizeButton != null)
                {
                    ((IExecuteEventsProvider)_customizeButton).ExecuteEvent += value;
                }
            }
            remove
            {
                if (_customizeButton != null)
                {
                    ((IExecuteEventsProvider)_customizeButton).ExecuteEvent -= value;
                }
            }
        }

        #endregion

        /// <summary>
        /// The customizeButton Click event
        /// </summary>
        public event EventHandler<EventArgs>? Click
        {
            add
            {
                if (_customizeButton != null)
                {
                    _customizeButton.Click += value;
                }
            }
            remove
            {
                if (_customizeButton != null)
                {
                    _customizeButton.Click -= value;
                }
            }
        }

        private protected override void OnDestroyUICommand(uint commandId, UI_COMMANDTYPE typeID)
        {
            base.OnDestroyUICommand(commandId, typeID);
            if (CommandType != CommandType.Unknown)
            {
                if (QatItemsSource != null)
                    QatItemsSource.Destroy();
            }
        }
    }
}
