Imports WinForms.Ribbon

Namespace _07_RibbonColor
	Partial Public Class Form1
		Inherits Form
		Private _ribbonItems As RibbonItems

		Public Sub New()
			InitializeComponent()
			_ribbonItems = New RibbonItems(_ribbon)
			_ribbonItems.Init()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			_ribbonItems.Load()
		End Sub
	End Class
End Namespace
