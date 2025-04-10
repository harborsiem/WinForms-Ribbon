Imports WinForms.Ribbon

Namespace _03_MenuDropDown
    Partial Public Class Form1
        Inherits Form
        Private _ribbonItems As RibbonItems

        Public Sub New()
            InitializeComponent()
            _ribbonItems = New RibbonItems(_ribbon)
            _ribbonItems.Init()
        End Sub
    End Class
End Namespace
