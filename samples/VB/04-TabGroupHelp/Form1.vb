Imports WinForms.Ribbon

Namespace _04_TabGroupHelp
	Partial Public Class Form1
		Inherits Form
		Private _ribbonItems As RibbonItems

		Public Sub New()
			InitializeComponent()
			_ribbonItems = New RibbonItems(_ribbon)
			_ribbonItems.Init(Me)
		End Sub
	End Class
End Namespace
