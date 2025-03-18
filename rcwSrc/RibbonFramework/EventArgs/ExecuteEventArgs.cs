//*****************************************************************************
//
//  File:       ExecuteEventsArgs.cs
//
//  Contents:   Definition for execute events arguments 
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
    /// Definition for execute events arguments
    /// </summary>
    public sealed class ExecuteEventArgs : EventArgs
    {
        private PROPERTYKEY? _key;
        private PROPVARIANT? _currentValue;
        private IUISimplePropertySet? _commandExecutionProperties;

        /// <summary>
        /// Initializes a new instance of the ExecuteEventArgs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currentValue"></param>
        /// <param name="commandExecutionProperties"></param>
        internal unsafe ExecuteEventArgs(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            if (key is null)
                _key = null;
            else
                _key = *key;
            if (currentValue is null)
            {
                _currentValue = null;
            }
            else
            {
                _currentValue = *currentValue;
            }
            _commandExecutionProperties = commandExecutionProperties;
        }

        /// <summary>
        /// Get the key
        /// </summary>
        public PROPERTYKEY? Key
        {
            get
            {
                return _key;
            }
        }

        /// <summary>
        /// Get the current value
        /// </summary>
        public PROPVARIANT? CurrentValue
        {
            get
            {
                return _currentValue;
            }
        }

        /// <summary>
        /// Get the Command Execution Properties
        /// </summary>
        public IUISimplePropertySet? CommandExecutionProperties
        {
            get
            {
                return _commandExecutionProperties;
            }
        }
    }
}
