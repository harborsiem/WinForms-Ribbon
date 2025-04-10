Imports WinForms.Ribbon
Imports System.IO

Namespace _17_QuickAccessToolbar

	Partial Public Class Form1
		Inherits Form
		Private _ribbonItems As RibbonItems

		Private _stream As Stream

		Public Sub New()
			InitializeComponent()
			_ribbonItems = New RibbonItems(_ribbon)
			_ribbonItems.Init()
		End Sub
	End Class
End Namespace
