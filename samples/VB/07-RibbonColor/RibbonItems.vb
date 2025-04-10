Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems
        Private _backgroundDefault As UI_HSBCOLOR
        Private _highlightDefault As UI_HSBCOLOR
        Private _textDefault As UI_HSBCOLOR
        Private _backgroundCurrent As UI_HSBCOLOR
        Private _highlightCurrent As UI_HSBCOLOR
        Private _textCurrent As UI_HSBCOLOR

        Public Sub Init()
            AddHandler ToggleDark.ToggleChanged, AddressOf ToggleDark_ToggleChanged
            AddHandler ButtonDefaultColors.Click, AddressOf ButtonDefaultColors_Click
        End Sub

        Private Sub ButtonDefaultColors_Click(ByVal sender As Object, ByVal e As EventArgs)
            Ribbon.SetBackgroundColor(_backgroundDefault)
            Ribbon.SetHighlightColor(_highlightDefault)
            Ribbon.SetTextColor(_textDefault)
            _backgroundCurrent = _backgroundDefault
            _highlightCurrent = _highlightDefault
            _textCurrent = _textDefault
        End Sub

        Private Sub ToggleDark_ToggleChanged(ByVal sender As Object, ByVal e As EventArgs)
            Ribbon.SetDarkModeRibbon(ToggleDark.BooleanValue)
            If (Not ToggleDark.BooleanValue) Then
                Ribbon.SetBackgroundColor(_backgroundCurrent)
                Ribbon.SetHighlightColor(_highlightCurrent)
                Ribbon.SetTextColor(_textCurrent)
            End If
        End Sub

        Public Sub Load()
            _backgroundDefault = Ribbon.GetBackgroundColor()
            _highlightDefault = Ribbon.GetHighlightColor()
            _textDefault = Ribbon.GetTextColor()
            ' set ribbon colors
            _backgroundCurrent = New UI_HSBCOLOR(Color.Wheat)
            _highlightCurrent = New UI_HSBCOLOR(Color.IndianRed)
            _textCurrent = New UI_HSBCOLOR(Color.BlueViolet)
            Ribbon.SetBackgroundColor(_backgroundCurrent)
            Ribbon.SetHighlightColor(_highlightCurrent)
            Ribbon.SetTextColor(_textCurrent)
        End Sub

    End Class
End Namespace
