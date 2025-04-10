Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

        Public Sub Init()
			AddHandler ButtonDropA.Click, AddressOf _buttonDropA_ExecuteEvent
		End Sub

		Private Sub _buttonDropA_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			InitSpinner()
		End Sub

		Private Sub InitSpinner()
			_Spinner.DecimalPlaces = 2
			_Spinner.DecimalValue = 1.8D
			_Spinner.TooltipTitle = "Height"
			_Spinner.TooltipDescription = "Enter height in meters."
			_Spinner.MaxValue = 2.5D
			_Spinner.MinValue = 0
			_Spinner.Increment = 0.01D
			_Spinner.FormatString = " m"
			_Spinner.RepresentativeString = "2.50 m"
			_Spinner.Label = "Height:"
		End Sub

	End Class
End Namespace
