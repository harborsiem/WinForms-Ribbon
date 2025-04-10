Imports System
Imports System.Text
Imports System.IO

Namespace Global.WinForms.Ribbon

	Partial Class RibbonItems
		Private exitOn As Boolean = False

		Public Sub Init()
			AddHandler ButtonOne.Click, AddressOf _buttonDropA_ExecuteEvent
			AddHandler ButtonTwo.Click, AddressOf _buttonDropB_ExecuteEvent
		End Sub

		Private Sub _buttonDropA_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' load bitmap from file
			Dim bitmap_Renamed As Bitmap = GetResourceBitmap("Drop32.bmp")
			'bitmap_Renamed.MakeTransparent()

			' set large image property
			ButtonOne.LargeImage = New UIImage(Ribbon, bitmap_Renamed)
		End Sub

		Private Sub _buttonDropB_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			Dim supportedImageSizes As New List(Of Integer)() From {32, 48, 64}

			Dim bitmapFileName As New StringBuilder()

			Dim selectedImageSize As Integer
			If supportedImageSizes.Contains(SystemInformation.IconSize.Width) Then
				selectedImageSize = SystemInformation.IconSize.Width
			Else
				selectedImageSize = 32
			End If

			exitOn = Not exitOn
			Dim exitStatus As String = If(exitOn, "On", "Off")

			Dim bitmap_Renamed = GetResourceBitmap(String.Format("Exit{0}{1}.bmp", exitStatus, selectedImageSize))
			'bitmap_Renamed.MakeTransparent()

			ButtonTwo.LargeImage = New UIImage(Ribbon, bitmap_Renamed)
		End Sub

		Private Function GetResourceBitmap(ByVal name As String) As Bitmap
			Dim resourceName As String = String.Format("_19_Localization.Res.{0}", name)
			Using stream = Me.GetType().Assembly.GetManifestResourceStream(resourceName)
				Dim bitmap_Renamed = New Bitmap(stream)
				Return bitmap_Renamed
			End Using
		End Function

	End Class
End Namespace
