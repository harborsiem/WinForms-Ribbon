//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        private static class Cmd
        {
        }

        // ContextPopup CommandName

        public RibbonStrip Ribbon { get; private set; }

        public RibbonItems(RibbonStrip ribbon)
        {
            if (ribbon == null)
                throw new ArgumentNullException(nameof(ribbon), "Parameter is null");
            this.Ribbon = ribbon;
        }

    }
}
