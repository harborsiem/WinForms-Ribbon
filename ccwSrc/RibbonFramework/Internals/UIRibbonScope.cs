using System;
using System.Collections.Generic;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;

namespace WinForms.Ribbon
{
    internal unsafe ref struct UIRibbonScope
    {
        private ComScope<IUIRibbon> BaseScope { get; set; }
        public ComScope<IPropertyStore> PropertyStoreScope { get; private set; }
        public bool IsNull => BaseScope.IsNull;

        public UIRibbonScope(RibbonStrip ribbon)
        {
            HRESULT hr;
            ComScope<IUIRibbon> uiRibbonScope = new ComScope<IUIRibbon>(null);
            using var framework = ribbon.Framework!.GetInterface();
            hr = framework.Value->GetView(0, IID.Get<IUIRibbon>(), (void**)&uiRibbonScope);
            hr.ThrowOnFailure();
            BaseScope = uiRibbonScope;
            ComScope<IPropertyStore> scope = ComScope<IPropertyStore>.QueryFrom(uiRibbonScope.Value);
            PropertyStoreScope = scope;
        }

        public void Dispose()
        {
            if (!PropertyStoreScope.IsNull)
                PropertyStoreScope.Dispose();
            if (!BaseScope.IsNull)
                BaseScope.Dispose();
        }
    }
}
