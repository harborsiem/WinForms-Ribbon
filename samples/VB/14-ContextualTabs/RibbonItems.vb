Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

        Public Sub Init()
            AddHandler _ButtonSelect.Click, AddressOf _buttonSelect_ExecuteEvent
            AddHandler _ButtonUnselect.Click, AddressOf _buttonUnselect_ExecuteEvent
        End Sub

        Private Sub _buttonSelect_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
            If _TabGroupTableTools.ContextAvailable <> ContextAvailability.Active Then
                _TabGroupTableTools.ContextAvailable = ContextAvailability.Active
            End If
        End Sub

        Private Sub _buttonUnselect_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
            If _TabGroupTableTools.ContextAvailable <> ContextAvailability.NotAvailable Then
                _TabGroupTableTools.ContextAvailable = ContextAvailability.NotAvailable
            End If
        End Sub

    End Class
End Namespace
