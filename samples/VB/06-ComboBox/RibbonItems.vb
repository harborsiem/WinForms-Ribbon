Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems

		Public Sub Init()

			AddHandler _ButtonDropA.Click, AddressOf _buttonDropA_ExecuteEvent
			AddHandler _ButtonDropB.Click, AddressOf _buttonDropB_ExecuteEvent
			AddHandler _ButtonDropC.Click, AddressOf _buttonDropC_ExecuteEvent
			AddHandler _ButtonDropD.Click, AddressOf _buttonDropD_ExecuteEvent
			AddHandler _ButtonDropE.Click, AddressOf _buttonDropE_ExecuteEvent
			AddHandler _ButtonDropF.Click, AddressOf _buttonDropF_ExecuteEvent

			InitComboBoxes()
		End Sub

		Private Sub _buttonDropA_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' get selected item index from combo box 1
			Dim selectedItemIndex As Integer = _ComboBox1.SelectedItem

			If selectedItemIndex = -1 Then
				MessageBox.Show("No item is selected in simple combo")
			Else
				Dim label As String
				label = _ComboBox1.GalleryItemItemsSource.Item(selectedItemIndex).Label
				MessageBox.Show("Selected item in simple combo is: " & label)
			End If
		End Sub

		Private Sub _buttonDropB_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' get string value from combo box 2
			Dim stringValue As String = _ComboBox2.StringValue
			MessageBox.Show("String value in advanced combo is: " & stringValue)
		End Sub

		Private Sub _buttonDropC_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			' enumerate over items
			For Each propSet As GalleryItemPropertySet In ComboBox1.GalleryItemItemsSource
				Dim label As String = propSet.Label
				MessageBox.Show("Label = " & label)
			Next
		End Sub

		Private Sub _buttonDropD_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			AddHandler _ComboBox1.GalleryItemItemsSource.ChangedEvent, AddressOf _uiCollectionChangedEvent_ChangedEvent
		End Sub

		Private Sub _buttonDropE_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			Dim itemsSource1 As UICollection(Of GalleryItemPropertySet) = _ComboBox1.GalleryItemItemsSource
			Dim count As Integer
			count = itemsSource1.Count
			count += 1
			itemsSource1.Add(New GalleryItemPropertySet() With {.Label = "Label " & count.ToString(), .CategoryId = -1})
		End Sub

		Private Sub _buttonDropF_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)
			RemoveHandler _ComboBox1.GalleryItemItemsSource.ChangedEvent, AddressOf _uiCollectionChangedEvent_ChangedEvent
		End Sub

		Private Sub _uiCollectionChangedEvent_ChangedEvent(ByVal sender As Object, ByVal e As CollectionChangedEventArgs)
			MessageBox.Show("Got ChangedEvent. Action = " & e.Action.ToString())
		End Sub

		Private Sub InitComboBoxes()
			_ComboBox1.RepresentativeString = "Label 1"
			_ComboBox2.RepresentativeString = "XXXXXXXXXXX"
			_ComboBox3.RepresentativeString = "XXXXXXXXXXX"

			_ComboBox1.Label = "Simple Combo"
			_ComboBox2.Label = "Advanced Combo"
			_ComboBox3.Label = "Another Combo"

			AddHandler _ComboBox1.ItemsSourceReady, AddressOf _comboBox1_ItemsSourceReady

			AddHandler _ComboBox2.ItemsSourceReady, AddressOf _comboBox2_CategoriesReady
			AddHandler _ComboBox2.ItemsSourceReady, AddressOf _comboBox2_ItemsSourceReady

			AddHandler _ComboBox3.ItemsSourceReady, AddressOf _comboBox3_ItemsSourceReady
		End Sub

		Private Sub _comboBox1_ItemsSourceReady()
			' set combobox1 items
			Dim itemsSource1 As UICollection(Of GalleryItemPropertySet) = _ComboBox1.GalleryItemItemsSource
			itemsSource1.Clear()
			itemsSource1.Add(New GalleryItemPropertySet() With {.Label = "Label 1", .CategoryId = -1})
			itemsSource1.Add(New GalleryItemPropertySet() With {.Label = "Label 2", .CategoryId = -1})
			itemsSource1.Add(New GalleryItemPropertySet() With {.Label = "Label 3", .CategoryId = -1})
		End Sub

		Private Sub _comboBox2_CategoriesReady()
			' set _comboBox2 categories
			Dim categories2 As UICollection(Of CategoriesPropertySet) = _ComboBox2.GalleryCategories
			categories2.Clear()
			categories2.Add(New CategoriesPropertySet() With {.Label = "Category 1", .CategoryId = 1})
			categories2.Add(New CategoriesPropertySet() With {.Label = "Category 2", .CategoryId = 2})
		End Sub

		Private Sub _comboBox2_ItemsSourceReady()
			' set _comboBox2 items
			Dim itemsSource2 As UICollection(Of GalleryItemPropertySet) = _ComboBox2.GalleryItemItemsSource
			itemsSource2.Clear()
			itemsSource2.Add(New GalleryItemPropertySet() With {.Label = "Label 1", .CategoryId = 1})
			itemsSource2.Add(New GalleryItemPropertySet() With {.Label = "Label 2", .CategoryId = 1})
			itemsSource2.Add(New GalleryItemPropertySet() With {.Label = "Label 3", .CategoryId = 2})
		End Sub

		Private Sub _comboBox3_ItemsSourceReady()
			' set combobox3 items
			Dim itemsSource3 As UICollection(Of GalleryItemPropertySet) = _ComboBox3.GalleryItemItemsSource
			itemsSource3.Clear()
			itemsSource3.Add(New GalleryItemPropertySet() With {.Label = "Label 1", .CategoryId = -1})
			itemsSource3.Add(New GalleryItemPropertySet() With {.Label = "Label 2", .CategoryId = -1})
			itemsSource3.Add(New GalleryItemPropertySet() With {.Label = "Label 3", .CategoryId = -1})
		End Sub

	End Class
End Namespace
