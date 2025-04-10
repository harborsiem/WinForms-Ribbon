Imports System
Imports _09_Galleries

Namespace WinForms.Ribbon

    Partial Class RibbonItems
        Private _buttons() As RibbonButton
        Private _form As Form1

        Public Sub Init(form1 As Form1)
            _form = form1
            AddHandler _DropDownGallery.ItemsSourceReady, AddressOf _dropDownGallery_ItemsSourceReady
            AddHandler _SplitButtonGallery.CategoriesReady, AddressOf _splitButtonGallery_CategoriesReady
            AddHandler _SplitButtonGallery.ItemsSourceReady, AddressOf _splitButtonGallery_ItemsSourceReady
            AddHandler _InRibbonGallery.ItemsSourceReady, AddressOf _inRibbonGallery_ItemsSourceReady

            AddHandler _DropDownGallery.SelectedIndexChanged, AddressOf _dropDownGallery_ExecuteEvent
            AddHandler _DropDownGallery.Preview, AddressOf _dropDownGallery_OnPreview
            AddHandler _DropDownGallery.CancelPreview, AddressOf _dropDownGallery_OnCancelPreview
        End Sub

        Private Sub _dropDownGallery_ItemsSourceReady()
            ' set label
            _DropDownGallery.Label = "Size"

            ' set _dropDownGallery items
            Dim itemsSource As UICollection(Of GalleryItemPropertySet) = _DropDownGallery.GalleryItemItemsSource
            itemsSource.Clear()
            For Each image_Renamed As Image In _form.imageListLines.Images
                itemsSource.Add(New GalleryItemPropertySet() With {.ItemImage = New UIImage(Ribbon, CType(image_Renamed, Bitmap))})
                '_ribbon.ConvertToUIImage(CType(image_Renamed, Bitmap))})

            Next image_Renamed
        End Sub

        Private Sub _splitButtonGallery_CategoriesReady()
            ' set _splitButtonGallery categories
            Dim categories As UICollection(Of CategoriesPropertySet) = _SplitButtonGallery.GalleryCategories
            categories.Clear()
            categories.Add(New CategoriesPropertySet() With {.Label = "Category 1", .CategoryId = 1})
        End Sub

        Private Sub _splitButtonGallery_ItemsSourceReady()
            ' set label
            _SplitButtonGallery.Label = "Brushes"

            ' prepare helper classes for commands
            _buttons = New RibbonButton(_form._ImageListBrushes.Images.Count - 1) {}
            Dim i As UInteger
            For i = 0 To CUInt(_buttons.Length - 1)
                _buttons(CInt(i)) = New RibbonButton(_ribbon, 2000UI + i) With {.Label = "Label " & i.ToString(), .LargeImage = New UIImage(Ribbon, CType(_form._ImageListBrushes.Images(CInt(Fix(i))), Bitmap))} '_ribbon.ConvertToUIImage(CType(imageListBrushes.Images(CInt(Fix(i))), Bitmap))}
            Next i

            ' set _splitButtonGallery items
            Dim itemsSource As UICollection(Of GalleryCommandPropertySet) = _SplitButtonGallery.GalleryCommandItemsSource
            itemsSource.Clear()
            i = 0
            For Each image_Renamed As Image In _form._ImageListBrushes.Images
                itemsSource.Add(New GalleryCommandPropertySet() With {.CommandId = 2000UI + i, .CommandType = CommandType.Action, .CategoryId = 1})
                i += 1UI
            Next image_Renamed
        End Sub

        Private Sub _inRibbonGallery_ItemsSourceReady()
            ' set _inRibbonGallery items
            Dim itemsSource As UICollection(Of GalleryItemPropertySet) = _InRibbonGallery.GalleryItemItemsSource
            itemsSource.Clear()
            For Each image_Renamed As Image In _form._ImageListShapes.Images
                itemsSource.Add(New GalleryItemPropertySet() With {.ItemImage = New UIImage(Ribbon, CType(image_Renamed, Bitmap))}) '_ribbon.ConvertToUIImage(CType(image_Renamed, Bitmap))})
            Next image_Renamed
        End Sub

        Private Sub _dropDownGallery_OnCancelPreview(ByVal sender As Object, ByVal e As GalleryItemEventArgs)
            Debug.WriteLine("DropDownGallery::OnCancelPreview")
        End Sub

        Private Sub _dropDownGallery_OnPreview(ByVal sender As Object, ByVal e As GalleryItemEventArgs)
            Debug.WriteLine("DropDownGallery::OnPreview")
        End Sub

        Private Sub _dropDownGallery_ExecuteEvent(ByVal sender As Object, ByVal e As GalleryItemEventArgs)
            Debug.WriteLine("DropDownGallery::ExecuteEvent")
        End Sub

    End Class
End Namespace
