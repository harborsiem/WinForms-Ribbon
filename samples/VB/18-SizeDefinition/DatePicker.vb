Public Class DatePicker
    Private _label As String
    Public ReadOnly Property Label As String
        Get
            Return _label
        End Get
    End Property

    Private Sub MonthCalendar_DateSelected(sender As Object, e As DateRangeEventArgs) Handles MonthCalendar.DateSelected
        _label = e.Start.ToShortDateString()
        DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class