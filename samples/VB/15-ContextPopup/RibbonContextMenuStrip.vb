Imports System.Text
Imports System.ComponentModel
Imports WinForms.Ribbon

Namespace _15_ContextPopup
	Public Class RibbonContextMenuStrip
		Inherits ContextMenuStrip
		Private _contextPopupID As UInteger
		Private _ribbon As RibbonStrip

		Public Sub New(ByVal ribbon_Renamed As RibbonStrip, ByVal contextPopupID As UInteger)
			MyBase.New()
			_contextPopupID = contextPopupID
			_ribbon = ribbon_Renamed
		End Sub

		Protected Overrides Sub OnOpening(ByVal e As CancelEventArgs)
			_ribbon.ShowContextPopup(_contextPopupID, Cursor.Position.X, Cursor.Position.Y)
			e.Cancel = True
		End Sub
	End Class
End Namespace
