﻿Imports WinForms.Ribbon

Namespace _18_SizeDefinition
    Partial Public Class Form1
        Inherits Form
        Private _ribbonItems As RibbonItems
        Public Sub New()
            InitializeComponent()
            _ribbonItems = New RibbonItems(_ribbon, True)
            _ribbonItems.Init()
        End Sub
    End Class
End Namespace
