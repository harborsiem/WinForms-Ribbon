Imports WinForms.Ribbon

Namespace _10_CheckBox

	Partial Public Class Form1
		Inherits Form
		Private _ribbonItems As RibbonItems
		Private _toggleButton As RibbonToggleButton
		Private _checkBox As RibbonCheckBox

		Public Sub New()
			InitializeComponent()
			_ribbonItems = New RibbonItems(_ribbon)
			_ribbonItems.Init()
		End Sub
	End Class
End Namespace
