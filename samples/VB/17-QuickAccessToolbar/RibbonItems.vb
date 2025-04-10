Imports System
Imports System.IO

Namespace WinForms.Ribbon

    Partial Class RibbonItems
		Private _stream As Stream

		Public Sub Init()
			AddHandler _ButtonNew.Click, AddressOf _buttonNew_ExecuteEvent
			AddHandler _ButtonSave.Click, AddressOf _buttonSave_ExecuteEvent
			AddHandler _ButtonOpen.Click, AddressOf _buttonOpen_ExecuteEvent

			' register to the QAT customize button
			AddHandler QAT.Click, AddressOf _ribbonQuickAccessToolbar_ExecuteEvent
		End Sub

		Private Sub _buttonNew_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' changing QAT commands list 
			Dim itemsSource As UICollection(Of QatCommandPropertySet) = QAT.QatItemsSource
			itemsSource.Clear()
			itemsSource.Add(New QatCommandPropertySet() With {.CommandId = CUInt(Cmd.cmdButtonNew)})
			itemsSource.Add(New QatCommandPropertySet() With {.CommandId = CUInt(Cmd.cmdButtonOpen)})
			itemsSource.Add(New QatCommandPropertySet() With {.CommandId = CUInt(Cmd.cmdButtonSave)})
		End Sub

		Private Sub _buttonSave_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' save ribbon QAT settings 
			_stream = New MemoryStream()
			_ribbon.SaveSettingsToStream(_stream)
		End Sub

		Private Sub _buttonOpen_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			If _stream Is Nothing Then
				Return
			End If

			' load ribbon QAT settings 
			_stream.Position = 0
			_ribbon.LoadSettingsFromStream(_stream)
		End Sub

		Private Sub _ribbonQuickAccessToolbar_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			MessageBox.Show("Open customize commands dialog..")
		End Sub

	End Class
End Namespace
