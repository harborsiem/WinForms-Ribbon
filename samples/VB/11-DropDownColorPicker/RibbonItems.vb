Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

        Public Sub Init()
			AddHandler _ButtonListColors.Click, AddressOf _buttonListColors_ExecuteEvent
		End Sub

		Public Sub Load()
			InitDropDownColorPickers()
		End Sub

		Private Sub _buttonListColors_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			Dim colors() As Color = DropDownColorPickerThemeColors.ThemeColors
			Dim colorsTooltips() As String = DropDownColorPickerThemeColors.ThemeColorsTooltips

			Dim stringBuilder As New System.Text.StringBuilder()

			For i As Integer = 0 To colors.Length - 1
				stringBuilder.AppendFormat("{0} = {1}" & vbLf, colorsTooltips(i), colors(i).ToString())
			Next i

			MessageBox.Show(stringBuilder.ToString())
		End Sub

		Private Sub InitDropDownColorPickers()
			' common properties
			DropDownColorPickerThemeColors.Label = "Theme Colors"
			AddHandler DropDownColorPickerThemeColors.ColorChanged, AddressOf _themeColors_ExecuteEvent

			' set labels
			DropDownColorPickerThemeColors.AutomaticColorLabel = "My Automatic"
			DropDownColorPickerThemeColors.MoreColorsLabel = "My More Colors"
			DropDownColorPickerThemeColors.NoColorLabel = "My No Color"
			DropDownColorPickerThemeColors.RecentColorsCategoryLabel = "My Recent Colors"
			DropDownColorPickerThemeColors.StandardColorsCategoryLabel = "My Standard Colors"
			DropDownColorPickerThemeColors.ThemeColorsCategoryLabel = "My Theme Colors"

			' set colors
			DropDownColorPickerThemeColors.ThemeColorsTooltips = New String() {"yellow", "green", "red", "blue"}
			DropDownColorPickerThemeColors.ThemeColors = New Color() {Color.Yellow, Color.Green, Color.Red, Color.Blue}
		End Sub

		Private Sub _themeColors_ExecuteEvent(ByVal sender As Object, ByVal e As ColorPickerEventArgs)
			MessageBox.Show("Selected color is " & DropDownColorPickerThemeColors.Color.ToString())
		End Sub

	End Class
End Namespace
