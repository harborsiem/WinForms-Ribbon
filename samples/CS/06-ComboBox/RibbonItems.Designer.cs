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
            public const uint cmdTabDrop = 1014;
            public const uint cmdGroupDrop = 1015;
            public const uint cmdButtonDropA = 1008;
            public const uint cmdButtonDropB = 1009;
            public const uint cmdButtonDropC = 1010;
            public const uint cmdButtonDropD = 1011;
            public const uint cmdButtonDropE = 1012;
            public const uint cmdButtonDropF = 1013;
            public const uint cmdGroupMore = 1017;
            public const uint cmdComboBox1 = 1018;
            public const uint cmdComboBox2 = 1019;
            public const uint cmdTabSecond = 1020;
            public const uint cmdGroupSecond = 1021;
            public const uint cmdComboBox3 = 1022;
        }

        // ContextPopup CommandName

        public RibbonStrip Ribbon { get; private set; }
        public RibbonTab TabDrop { get; private set; }
        public RibbonGroup GroupDrop { get; private set; }
        public RibbonButton ButtonDropA { get; private set; }
        public RibbonButton ButtonDropB { get; private set; }
        public RibbonButton ButtonDropC { get; private set; }
        public RibbonButton ButtonDropD { get; private set; }
        public RibbonButton ButtonDropE { get; private set; }
        public RibbonButton ButtonDropF { get; private set; }
        public RibbonGroup GroupMore { get; private set; }
        public RibbonComboBox ComboBox1 { get; private set; }
        public RibbonComboBox ComboBox2 { get; private set; }
        public RibbonTab TabSecond { get; private set; }
        public RibbonGroup GroupSecond { get; private set; }
        public RibbonComboBox ComboBox3 { get; private set; }

        public RibbonItems(RibbonStrip ribbon)
        {
            if (ribbon == null)
                throw new ArgumentNullException(nameof(ribbon), "Parameter is null");
            this.Ribbon = ribbon;
            TabDrop = new RibbonTab(ribbon, Cmd.cmdTabDrop);
            GroupDrop = new RibbonGroup(ribbon, Cmd.cmdGroupDrop);
            ButtonDropA = new RibbonButton(ribbon, Cmd.cmdButtonDropA);
            ButtonDropB = new RibbonButton(ribbon, Cmd.cmdButtonDropB);
            ButtonDropC = new RibbonButton(ribbon, Cmd.cmdButtonDropC);
            ButtonDropD = new RibbonButton(ribbon, Cmd.cmdButtonDropD);
            ButtonDropE = new RibbonButton(ribbon, Cmd.cmdButtonDropE);
            ButtonDropF = new RibbonButton(ribbon, Cmd.cmdButtonDropF);
            GroupMore = new RibbonGroup(ribbon, Cmd.cmdGroupMore);
            ComboBox1 = new RibbonComboBox(ribbon, Cmd.cmdComboBox1);
            ComboBox2 = new RibbonComboBox(ribbon, Cmd.cmdComboBox2);
            TabSecond = new RibbonTab(ribbon, Cmd.cmdTabSecond);
            GroupSecond = new RibbonGroup(ribbon, Cmd.cmdGroupSecond);
            ComboBox3 = new RibbonComboBox(ribbon, Cmd.cmdComboBox3);
        }

    }
}
