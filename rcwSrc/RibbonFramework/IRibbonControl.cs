//*****************************************************************************
//
//  File:       IRibbonControl.cs
//
//  Contents:   Definition of IRibbonControl interface. 
//              Each ribbon control helper class should implement this 
//              interface according to the control's actions and properties.
//
//*****************************************************************************

using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Each ribbon control helper class should implement this
    /// interface according to the control's actions and properties.
    /// </summary>
    public interface IRibbonControl
    {
        /// <summary>
        /// Associated RibbonStrip to this control
        /// </summary>
        RibbonStrip Ribbon { get; }

        /// <summary>
        /// Command Id of the Ribbon control 
        /// </summary>
        uint CommandId { get; }

        ///// <summary>
        ///// Handles IUICommandHandler.Execute function for this ribbon control
        ///// </summary>
        ///// <param name="verb">the mode of execution</param>
        ///// <param name="key">the property that has changed</param>
        ///// <param name="currentValue">the new value of the property that has changed</param>
        ///// <param name="commandExecutionProperties">additional data for this execution</param>
        ///// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        //unsafe HRESULT Execute(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties);

        ///// <summary>
        ///// Handles IUICommandHandler.UpdateProperty function for this ribbon control
        ///// </summary>
        ///// <param name="key">The Property Key to update</param>
        ///// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        ///// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        ///// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        //unsafe HRESULT UpdateProperty(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue);

        /// <summary>
        /// Gets or sets the object that contains data about the control
        /// </summary>
        object? Tag { get; set; }

        /// <summary>
        /// The CommandType of the Control
        /// If the CommandType is UI_CommandType.Unknown (UI_COMMANDTYPE.UI_COMMANDTYPE_UNKNOWN) then the Control is not initialized by the Framework
        /// </summary>
        UI_CommandType CommandType { get; }
    }
}
