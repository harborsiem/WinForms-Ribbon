Imports System
Imports System.Windows.Forms
Imports _04_TabGroupHelp

Namespace WinForms.Ribbon

    Partial Class RibbonItems
		Private _self As Form
		Public Sub Init(self As Form)
			_self = self

			AddHandler ButtonExit.Click, AddressOf _exitButton_ExecuteEvent
			AddHandler _HelpButton.Click, AddressOf _helpButton_ExecuteEvent
		End Sub


		Private Sub _exitButton_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' Close form asynchronously since we are in a ribbon event 
			' handler, so the ribbon is still in use, and calling Close 
			' will eventually call _ribbon.DestroyFramework(), which is 
			' a big no-no, if you still use the ribbon.
			_self.BeginInvoke(New MethodInvoker(AddressOf _self.Close))
		End Sub

		Private Sub _helpButton_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			MessageBox.Show("Help button pressed")
		End Sub

	End Class
End Namespace
