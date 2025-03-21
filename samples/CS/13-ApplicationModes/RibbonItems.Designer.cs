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
            public const uint cmdTabMain = 1001;
            public const uint cmdGroupCommon = 1002;
            public const uint cmdButtonNew = 1005;
            public const uint cmdButtonOpen = 1006;
            public const uint cmdButtonSave = 1007;
            public const uint cmdGroupSimple = 1003;
            public const uint cmdButtonSwitchToAdvanced = 1011;
            public const uint cmdButtonDropA = 1008;
            public const uint cmdGroupAdvanced = 1004;
            public const uint cmdButtonSwitchToSimple = 1012;
            public const uint cmdButtonDropB = 1009;
            public const uint cmdButtonDropC = 1010;
        }

        // ContextPopup CommandName

        public RibbonStrip Ribbon { get; private set; }
        public RibbonTab TabMain { get; private set; }
        public RibbonGroup GroupCommon { get; private set; }
        public RibbonButton ButtonNew { get; private set; }
        public RibbonButton ButtonOpen { get; private set; }
        public RibbonButton ButtonSave { get; private set; }
        public RibbonGroup GroupSimple { get; private set; }
        public RibbonButton ButtonSwitchToAdvanced { get; private set; }
        public RibbonButton ButtonDropA { get; private set; }
        public RibbonGroup GroupAdvanced { get; private set; }
        public RibbonButton ButtonSwitchToSimple { get; private set; }
        public RibbonButton ButtonDropB { get; private set; }
        public RibbonButton ButtonDropC { get; private set; }

        public RibbonItems(RibbonStrip ribbon)
        {
            if (ribbon == null)
                throw new ArgumentNullException(nameof(ribbon), "Parameter is null");
            this.Ribbon = ribbon;
            TabMain = new RibbonTab(ribbon, Cmd.cmdTabMain);
            GroupCommon = new RibbonGroup(ribbon, Cmd.cmdGroupCommon);
            ButtonNew = new RibbonButton(ribbon, Cmd.cmdButtonNew);
            ButtonOpen = new RibbonButton(ribbon, Cmd.cmdButtonOpen);
            ButtonSave = new RibbonButton(ribbon, Cmd.cmdButtonSave);
            GroupSimple = new RibbonGroup(ribbon, Cmd.cmdGroupSimple);
            ButtonSwitchToAdvanced = new RibbonButton(ribbon, Cmd.cmdButtonSwitchToAdvanced);
            ButtonDropA = new RibbonButton(ribbon, Cmd.cmdButtonDropA);
            GroupAdvanced = new RibbonGroup(ribbon, Cmd.cmdGroupAdvanced);
            ButtonSwitchToSimple = new RibbonButton(ribbon, Cmd.cmdButtonSwitchToSimple);
            ButtonDropB = new RibbonButton(ribbon, Cmd.cmdButtonDropB);
            ButtonDropC = new RibbonButton(ribbon, Cmd.cmdButtonDropC);
        }

    }
}
