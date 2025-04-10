Imports System

Namespace WinForms.Ribbon

    Partial Class RibbonItems
        Private _recentItems_Renamed As IList(Of RecentItemsPropertySet)

        Public Sub Init()
            AddHandler RecentItems.SelectedChanged, AddressOf _recentItems_ExecuteEvent
            AddHandler RecentItems.PinnedChanged, AddressOf RecentItems_PinnedChanged
        End Sub

        Public Sub Load()
            InitRecentItems()
        End Sub

        Private Sub RecentItems_PinnedChanged(ByVal sender As Object, ByVal e As PinnedChangedEventArgs)
            For i As Integer = i To e.ChangedPinnedIndices.Count
                Dim propertySet As RecentItemsPropertySet = RecentItems.RecentItems.Item(i)
                Dim label As String = propertySet.Label
                Dim LabelDescription As String = propertySet.LabelDescription
                Dim pinned As Boolean = propertySet.Pinned
            Next
        End Sub

        Private Sub InitRecentItems()
            ' prepare list of recent items
            _recentItems_Renamed = RecentItems.RecentItems
            _recentItems_Renamed.Add(New RecentItemsPropertySet() With {.Label = "Recent item 1", .LabelDescription = "Recent item 1 description", .Pinned = True})
            _recentItems_Renamed.Add(New RecentItemsPropertySet() With {.Label = "Recent item 2", .LabelDescription = "Recent item 2 description", .Pinned = False})

        End Sub

        Private Sub _recentItems_ExecuteEvent(ByVal sender As Object, ByVal e As SelectedRecentEventArgs)
            Dim selectedItem As Integer = e.SelectedItem.SelectedItemIndex
            Dim propertySet As RecentItemsPropertySet = e.SelectedItem.PropertySet
            Dim label As String = propertySet.Label
            Dim LabelDescription As String = propertySet.LabelDescription
            Dim pinned As Boolean = propertySet.Pinned
            Dim maxCount As Integer = RecentItems.MaxCount
            MessageBox.Show("Selected Recent index: " + e.SelectedItem.SelectedItemIndex.ToString() + Environment.NewLine + "MaxCount: " + maxCount.ToString())
        End Sub

    End Class
End Namespace
