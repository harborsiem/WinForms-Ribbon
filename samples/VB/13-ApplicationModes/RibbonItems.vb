Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

        Public Sub Init()
            AddHandler _ButtonSwitchToAdvanced.Click, AddressOf _buttonSwitchToAdvanced_ExecuteEvent
            AddHandler _ButtonSwitchToSimple.Click, AddressOf _buttonSwitchToSimple_ExecuteEvent
        End Sub

        Private Sub _buttonSwitchToAdvanced_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
            _ribbon.SetModes(1)
        End Sub

        Private Sub _buttonSwitchToSimple_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
            _ribbon.SetModes(0)
        End Sub

    End Class
End Namespace
