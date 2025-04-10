Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

        Public Sub Init()
			_ApplicationMenu.TooltipTitle = "Menu"
			_ApplicationMenu.TooltipDescription = "Application main menu"

			AddHandler _ButtonNew.Click, AddressOf _buttonNew_ExecuteEvent
		End Sub

		Private Sub _buttonNew_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			MessageBox.Show("new button pressed")
		End Sub

	End Class
End Namespace
