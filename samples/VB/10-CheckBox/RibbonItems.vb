Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

        Public Sub Init()
            AddHandler _Button.Click, AddressOf _button_ExecuteEvent
        End Sub

        Private Sub _button_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
            MessageBox.Show("checkbox check status is: " & _CheckBox.BooleanValue.ToString())
        End Sub

    End Class
End Namespace
