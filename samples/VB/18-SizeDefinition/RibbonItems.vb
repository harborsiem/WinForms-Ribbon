Imports System
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports _18_SizeDefinition

Namespace WinForms.Ribbon

    Partial Class RibbonItems
        Public Sub New(ByVal ribbon As RibbonStrip, ByVal withNames As Boolean)
            Me.New(ribbon)
            If withNames Then
                TabHome.Name = NameOf(TabHome)
                GroupParagraph.Name = NameOf(GroupParagraph)
                DecreaseIndent.Name = NameOf(DecreaseIndent)
                IncreaseIndent.Name = NameOf(IncreaseIndent)
                StartList.Name = NameOf(StartList)
                LineSpacing.Name = NameOf(LineSpacing)
                AlignLeft.Name = NameOf(AlignLeft)
                AlignCenter.Name = NameOf(AlignCenter)
                AlignRight.Name = NameOf(AlignRight)
                Justify.Name = NameOf(Justify)
                Paragraph.Name = NameOf(Paragraph)
                TabSpecialLayouts.Name = NameOf(TabSpecialLayouts)
                Group1.Name = NameOf(Group1)
                Combo1.Name = NameOf(Combo1)
                Button1.Name = NameOf(Button1)
                Combo2.Name = NameOf(Combo2)
                Button2.Name = NameOf(Button2)
                Hidden1.Name = NameOf(Hidden1)
                Group2.Name = NameOf(Group2)
                Hidden2.Name = NameOf(Hidden2)
                Group3.Name = NameOf(Group3)
                ButtonLabel.Name = NameOf(ButtonLabel)
                Group4.Name = NameOf(Group4)
                Group5.Name = NameOf(Group5)
                ButtonDate.Name = NameOf(ButtonDate)
            End If
        End Sub

        Public Sub Init()
            Hidden1.Enabled = False
            Hidden2.Enabled = False
            ButtonLabel.Enabled = False
            Combo1.RepresentativeString = "XXXXXX"
            Combo2.RepresentativeString = "XXXXXX"
            AddHandler Combo1.ItemsSourceReady, AddressOf Combo1_ItemsSourceReady
            AddHandler Combo1.SelectedIndexChanged, AddressOf Combo1_SelectedIndexChanged
            AddHandler Combo2.ItemsSourceReady, AddressOf Combo2_ItemsSourceReady
            AddHandler Combo2.SelectedIndexChanged, AddressOf Combo2_SelectedIndexChanged

            ButtonDate.Label = DateTime.Now.ToShortDateString
            AddHandler ButtonDate.Click, AddressOf ButtonDateTime_ExecuteEvent

        End Sub

        Private Sub Combo1_SelectedIndexChanged(ByVal sender As Object, ByVal e As GalleryItemEventArgs)
            Dim value As String = Combo1.StringValue
            Dim selected As SelectedItem(Of GalleryItemPropertySet) = e.SelectedItem
            Dim index As Integer = selected.SelectedItemIndex
            Dim propertySet As GalleryItemPropertySet = selected.PropertySet
            Dim indexFromCombo As Integer = Combo1.SelectedItem
        End Sub

        Private Sub Combo2_SelectedIndexChanged(ByVal sender As Object, ByVal e As GalleryItemEventArgs)
            Dim value As String = Combo2.StringValue
            Dim selected As SelectedItem(Of GalleryItemPropertySet) = e.SelectedItem
            Dim index As Integer = selected.SelectedItemIndex
            Dim propertySet As GalleryItemPropertySet = selected.PropertySet
            Dim indexFromCombo As Integer = Combo2.SelectedItem
        End Sub

        Private Sub Combo1_ItemsSourceReady()
            ' set combobox1 items
            Dim collection As UICollection(Of GalleryItemPropertySet) = Combo1.GalleryItemItemsSource
            collection.Clear()
            collection.Add(New GalleryItemPropertySet() With {.Label = "1", .CategoryId = -1})
            collection.Add(New GalleryItemPropertySet() With {.Label = "2", .CategoryId = -1})
            collection.Add(New GalleryItemPropertySet() With {.Label = "3", .CategoryId = -1})
        End Sub

        Private Sub Combo2_ItemsSourceReady()
            ' set combobox1 items
            Dim collection As UICollection(Of GalleryItemPropertySet) = Combo2.GalleryItemItemsSource
            collection.Clear()
            collection.Add(New GalleryItemPropertySet() With {.Label = "1", .CategoryId = -1})
            collection.Add(New GalleryItemPropertySet() With {.Label = "2", .CategoryId = -1})
            collection.Add(New GalleryItemPropertySet() With {.Label = "3", .CategoryId = -1})
        End Sub

        Private Sub ButtonDateTime_ExecuteEvent(ByVal sender As Object, ByVal e As EventArgs)

            Dim dialog As DatePicker = New DatePicker()
            Dim x As Integer = System.Windows.Forms.Cursor.Position.X
            Dim y As Integer = System.Windows.Forms.Cursor.Position.Y
            dialog.Location = New Point(x, y + 10)
            If dialog.ShowDialog() = DialogResult.OK Then
                ButtonDate.Label = dialog.Label
            End If
        End Sub
    End Class
End Namespace
