Imports System
Imports System.Text

Namespace WinForms.Ribbon

    Partial Class RibbonItems
		Private exitOn As Boolean = False

		Public Sub Init()
			AddHandler _ButtonDropA.Click, AddressOf _buttonDropA_ExecuteEvent
			AddHandler _ButtonDropB.Click, AddressOf _buttonDropB_ExecuteEvent
		End Sub

		Private Sub _buttonDropA_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' load bitmap from file
			Dim bitmap_Renamed As Bitmap = New Bitmap("..\..\..\Res\Drop32.bmp")
			'bitmap_Renamed.MakeTransparent()

			' set large image property
			_ButtonDropA.LargeImage = New UIImage(Ribbon, bitmap_Renamed)
		End Sub

		Private Sub _buttonDropB_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			Dim supportedImageSizes As New List(Of Integer)() From {32, 48, 64}

			Dim bitmap_Renamed As Bitmap
			Dim bitmapFileName As New StringBuilder()

			Dim selectedImageSize As Integer
			If supportedImageSizes.Contains(SystemInformation.IconSize.Width) Then
				selectedImageSize = SystemInformation.IconSize.Width
			Else
				selectedImageSize = 32
			End If

			exitOn = Not exitOn
			Dim exitStatus As String = If(exitOn, "on", "off")

			bitmapFileName.AppendFormat("..\..\..\Res\Exit{0}{1}.bmp", exitStatus, selectedImageSize)

			bitmap_Renamed = New Bitmap(bitmapFileName.ToString())
			'bitmap_Renamed.MakeTransparent()

			_ButtonDropB.LargeImage = New UIImage(Ribbon, bitmap_Renamed)
		End Sub

	End Class
End Namespace
