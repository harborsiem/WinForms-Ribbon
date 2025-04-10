Imports System
Imports System.Drawing
Imports _12_FontControl

Namespace WinForms.Ribbon
    ' First write a text to the RichTextBox RichTextBox1 And mark it as selected.
    ' Then you can see what happend with Preview, CancelPreview And Execute by the ribbon fontcontrol

    Partial Class RibbonItems
        Private _form As Form1
        Private WithEvents richTextBox1 As RichTextBox

        Public Sub Init(form As Form1)
            _form = form
            richTextBox1 = form.richTextBox1
            AddHandler RichFont.FontChanged, AddressOf _richFont_ExecuteEvent
            AddHandler RichFont.Preview, AddressOf _richFont_OnPreview
            AddHandler RichFont.CancelPreview, AddressOf _richFont_OnCancelPreview
        End Sub

        Private Sub _richFont_ExecuteEvent(ByVal sender As Object, ByVal e As FontControlEventArgs)
#If DEBUG Then
            PrintFontControlProperties(RichFont)
            PrintChangedProperties(e.ChangedFontValues)
#End If
            ' skip if selected font is not valid
            If (_RichFont.Family Is Nothing) OrElse (_RichFont.Family.Trim() = String.Empty) OrElse (_RichFont.Size = 0) Then
                Return
            End If

            ' prepare font style
            Dim fontStyle_Renamed As FontStyle = FontStyle.Regular
            If _RichFont.Bold = FontProperties.Set Then
                fontStyle_Renamed = fontStyle_Renamed Or FontStyle.Bold
            End If
            If _RichFont.Italic = FontProperties.Set Then
                fontStyle_Renamed = fontStyle_Renamed Or FontStyle.Italic
            End If
            If _RichFont.Underline = FontUnderline.Set Then
                fontStyle_Renamed = fontStyle_Renamed Or FontStyle.Underline
            End If
            If _RichFont.Strikethrough = FontProperties.Set Then
                fontStyle_Renamed = fontStyle_Renamed Or FontStyle.Strikeout
            End If

            ' set selected font
            ' creating a new font can't fail if the font doesn't support the requested style
            ' or if the font family name doesn't exist
            Try
                _form.richTextBox1.SelectionFont = New Font(_RichFont.Family, CSng(_RichFont.Size), fontStyle_Renamed)
            Catch e1 As ArgumentException
            End Try

            ' set selected colors
            _form.richTextBox1.SelectionColor = _RichFont.ForegroundColor
            _form.richTextBox1.SelectionBackColor = _RichFont.BackgroundColor

            ' set subscript / superscript
            Select Case _RichFont.VerticalPositioning
                Case FontVerticalPosition.NotSet, FontVerticalPosition.NotAvailable
                    _form.richTextBox1.SelectionCharOffset = 0

                Case FontVerticalPosition.SuperScript
                    _form.richTextBox1.SelectionCharOffset = 10

                Case FontVerticalPosition.SubScript
                    _form.richTextBox1.SelectionCharOffset = -10
            End Select
        End Sub

        Private Sub _richFont_OnPreview(ByVal sender As Object, ByVal e As FontControlEventArgs)
            Dim dict As Dictionary(Of FontPropertiesEnum, Object) = e.ChangedFontValues
            Dim store As FontPropertyStore = e.CurrentFontStore
            UpdateRichTextBox(dict)
            ' UpdateRichTextBox(store)
        End Sub

        Private Sub _richFont_OnCancelPreview(ByVal sender As Object, ByVal e As FontControlEventArgs)
            Dim dict As Dictionary(Of FontPropertiesEnum, Object) = e.ChangedFontValues
            Dim store As FontPropertyStore = e.CurrentFontStore
            ' UpdateRichTextBox(dict)
            UpdateRichTextBox(store)
        End Sub

        Private Shared Sub PrintFontControlProperties(ByVal fontControl As RibbonFontControl)
            Debug.WriteLine("")
            Debug.WriteLine("FontControl current properties:")
            Debug.WriteLine("Family: " & fontControl.Family)
            Debug.WriteLine("Size: " & fontControl.Size.ToString())
            Debug.WriteLine("Bold: " & fontControl.Bold.ToString())
            Debug.WriteLine("Italic: " & fontControl.Italic.ToString())
            Debug.WriteLine("Underline: " & fontControl.Underline.ToString())
            Debug.WriteLine("Strikethrough: " & fontControl.Strikethrough.ToString())
            Debug.WriteLine("ForegroundColor: " & fontControl.ForegroundColor.ToString())
            Debug.WriteLine("BackgroundColor: " & fontControl.BackgroundColor.ToString())
            Debug.WriteLine("VerticalPositioning: " & fontControl.VerticalPositioning.ToString())
        End Sub

        Private Shared Sub PrintChangedProperties(changedProps As Dictionary(Of FontPropertiesEnum, Object))
            Debug.WriteLine("")
            Debug.WriteLine("FontControl changed properties:")
            If changedProps IsNot Nothing Then
                For Each pair As KeyValuePair(Of FontPropertiesEnum, Object) In changedProps
                    Debug.WriteLine("FontProperties_" & pair.Key.ToString())
                Next
            End If
        End Sub

        Private Sub UpdateRichTextBox(changedProps As Dictionary(Of FontPropertiesEnum, Object))
            Dim family As String = Nothing
            Dim size As Nullable(Of Single) = Nothing
            If (changedProps IsNot Nothing) Then
                If (changedProps.ContainsKey(FontPropertiesEnum.Family)) Then
                    family = CStr(changedProps.Item(FontPropertiesEnum.Family))
                End If
                If (changedProps.ContainsKey(FontPropertiesEnum.Size)) Then
                    size = CSng(CDec(changedProps.Item(FontPropertiesEnum.Size)))
                End If
            End If
            UpdateRichTextBox(family, size)
        End Sub

        Private Sub UpdateRichTextBox(propertyStore As FontPropertyStore)
            UpdateRichTextBox(propertyStore.Family, CSng(propertyStore.Size))
        End Sub

        Private Sub UpdateRichTextBox(newFamily As String, newSize As Nullable(Of Single))
            Dim fontStyle As FontStyle
            Dim family As String
            Dim size As Single

            If (_form.richTextBox1.SelectionFont IsNot Nothing) Then
                fontStyle = _form.richTextBox1.SelectionFont.Style
                family = _form.richTextBox1.SelectionFont.FontFamily.Name
                size = _form.richTextBox1.SelectionFont.Size

            Else
                fontStyle = FontStyle.Regular
                family = String.Empty
                size = 0
            End If
            If (newFamily IsNot Nothing) Then
                family = newFamily
            End If
            If (newSize IsNot Nothing) Then
                size = CSng(newSize)
            End If

            ' creating a New font can't fail if the font doesn't support the requested style
            ' Or if the font family name doesn't exist
            Try
                _form.richTextBox1.SelectionFont = New Font(family, size, fontStyle)
            Catch e1 As ArgumentException
            End Try
        End Sub

        Private Sub richTextBox1_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles richTextBox1.SelectionChanged
            ' update font control font
            If _form.richTextBox1.SelectionFont IsNot Nothing Then
                _RichFont.Family = _form.richTextBox1.SelectionFont.FontFamily.Name
                _RichFont.Size = CDec(_form.richTextBox1.SelectionFont.Size)
                _RichFont.Bold = If(_form.richTextBox1.SelectionFont.Bold, FontProperties.Set, FontProperties.NotSet)
                _RichFont.Italic = If(_form.richTextBox1.SelectionFont.Italic, FontProperties.Set, FontProperties.NotSet)
                _RichFont.Underline = If(_form.richTextBox1.SelectionFont.Underline, FontUnderline.Set, FontUnderline.NotSet)
                _RichFont.Strikethrough = If(_form.richTextBox1.SelectionFont.Strikeout, FontProperties.Set, FontProperties.NotSet)
            Else
                _RichFont.Family = String.Empty
                _RichFont.Size = 0
                _RichFont.Bold = FontProperties.NotAvailable
                _RichFont.Italic = FontProperties.NotAvailable
                _RichFont.Underline = FontUnderline.NotAvailable
                _RichFont.Strikethrough = FontProperties.NotAvailable
            End If

            ' update font control colors
            _RichFont.ForegroundColor = _form.richTextBox1.SelectionColor
            _RichFont.BackgroundColor = _form.richTextBox1.SelectionBackColor

            ' update font control vertical positioning
            Select Case _form.richTextBox1.SelectionCharOffset
                Case 0
                    _RichFont.VerticalPositioning = FontVerticalPosition.NotSet

                Case 10
                    _RichFont.VerticalPositioning = FontVerticalPosition.SuperScript

                Case -10
                    _RichFont.VerticalPositioning = FontVerticalPosition.SubScript
            End Select
        End Sub

    End Class
End Namespace
