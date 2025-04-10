Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

        Public Sub Init()
            AddHandler ButtonDropB.Click, AddressOf _buttonDropB_ExecuteEvent
        End Sub

        Private Sub _buttonDropB_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
            MessageBox.Show("drop B button pressed")
        End Sub

    End Class
End Namespace
