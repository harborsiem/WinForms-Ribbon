Imports System.Text
Imports System.IO
Imports WinForms.Ribbon

Namespace _19_Localization

	Partial Public Class Form1
		Inherits Form
		Private _ribbonItems As RibbonItems

		Public Sub New()
			InitializeComponent()
			_ribbonItems = New RibbonItems(_ribbonControl)
			_ribbonItems.Init()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		End Sub

		Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
		End Sub

	End Class
End Namespace
