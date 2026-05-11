Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems
        Private _backgroundDefault As UI_HSBCOLOR
        Private _highlightDefault As UI_HSBCOLOR
        Private _textDefault As UI_HSBCOLOR
        Private _appButtonColorDefault As UI_HSBCOLOR
        Private _backgroundCurrent As UI_HSBCOLOR
        Private _highlightCurrent As UI_HSBCOLOR
        Private _textCurrent As UI_HSBCOLOR
        Private _appButtonColorCurrent As UI_HSBCOLOR
        Private _hasAppButtonColor As Boolean

        Public Sub Init()
            AddHandler ToggleDark.ToggleChanged, AddressOf ToggleDark_ToggleChanged
            AddHandler ButtonDefaultColors.Click, AddressOf ButtonDefaultColors_Click
            AddHandler ButtonColor.Click, AddressOf ButtonColor_Click
            AddHandler Ribbon.ViewCreated, AddressOf Ribbon_ViewCreated
        End Sub

        Private Sub ButtonColor_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' set ribbon colors
            _backgroundCurrent = New UI_HSBCOLOR(Color.Wheat)
            _highlightCurrent = New UI_HSBCOLOR(Color.IndianRed)
            _textCurrent = New UI_HSBCOLOR(Color.BlueViolet)
            Ribbon.SetBackgroundColor(_backgroundCurrent)
            Ribbon.SetHighlightColor(_highlightCurrent)
            Ribbon.SetTextColor(_textCurrent)
            If (_hasAppButtonColor) Then
                _appButtonColorCurrent = New UI_HSBCOLOR(Color.BlueViolet)
                Ribbon.SetApplicationButtonColor(_appButtonColorCurrent)
            End If
        End Sub

        Private Sub Ribbon_ViewCreated(ByVal sender As Object, ByVal e As EventArgs)
            _backgroundDefault = Ribbon.GetBackgroundColor()
            _highlightDefault = Ribbon.GetHighlightColor()
            _textDefault = Ribbon.GetTextColor()
            Try
                _appButtonColorDefault = Ribbon.GetApplicationButtonColor()
                _hasAppButtonColor = True
            Catch ex As NotSupportedException
                _hasAppButtonColor = False
            End Try
        End Sub

        Private Sub ButtonDefaultColors_Click(ByVal sender As Object, ByVal e As EventArgs)
            Ribbon.SetBackgroundColor(_backgroundDefault)
            Ribbon.SetHighlightColor(_highlightDefault)
            Ribbon.SetTextColor(_textDefault)
            If (_hasAppButtonColor) Then
                Ribbon.SetApplicationButtonColor(_appButtonColorDefault)
            End If
            _backgroundCurrent = _backgroundDefault
            _highlightCurrent = _highlightDefault
            _textCurrent = _textDefault
            _appButtonColorCurrent = _appButtonColorDefault
        End Sub

        Private Sub ToggleDark_ToggleChanged(ByVal sender As Object, ByVal e As EventArgs)
            Ribbon.SetDarkModeRibbon(ToggleDark.BooleanValue)
            If (ToggleDark.BooleanValue) Then
                ButtonColor.Enabled = False
                ButtonDefaultColors.Enabled = False
            Else
                ButtonColor.Enabled = True
                ButtonDefaultColors.Enabled = True
            End If
        End Sub

        Public Sub Load()
#If NET10_0_OR_GREATER Then
            If (Application.IsDarkModeEnabled) Then
                ToggleDark.BooleanValue = True
                ButtonColor.Enabled = False
                ButtonDefaultColors.Enabled = False
            End If
#End If
        End Sub

    End Class
End Namespace
