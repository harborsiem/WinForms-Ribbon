Imports WinForms.Ribbon
Imports System.Drawing

Namespace _09_Galleries

    Partial Public Class Form1
        Inherits Form
        Private _ribbonItems As RibbonItems
        'Public ReadOnly Property _ImageListLines As ImageList
        '    Get
        '        Return imageListLines
        '    End Get
        'End Property

        Public ReadOnly Property _ImageListBrushes As ImageList
            Get
                Return imageListBrushes
            End Get
        End Property

        Public ReadOnly Property _ImageListShapes As ImageList
            Get
                Return imageListShapes
            End Get
        End Property

        Public Sub New()
            InitializeComponent()
            _ribbonItems = New RibbonItems(_ribbon)
            _ribbonItems.Init(Me)
        End Sub

    End Class
End Namespace
