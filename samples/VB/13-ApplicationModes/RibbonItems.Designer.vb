'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System

Namespace Global.WinForms.Ribbon
    Partial Class RibbonItems
        Private Class Cmd
            Public Const cmdTabMain As UInteger = 1001
            Public Const cmdGroupCommon As UInteger = 1002
            Public Const cmdButtonNew As UInteger = 1005
            Public Const cmdButtonOpen As UInteger = 1006
            Public Const cmdButtonSave As UInteger = 1007
            Public Const cmdGroupSimple As UInteger = 1003
            Public Const cmdButtonSwitchToAdvanced As UInteger = 1011
            Public Const cmdButtonDropA As UInteger = 1008
            Public Const cmdGroupAdvanced As UInteger = 1004
            Public Const cmdButtonSwitchToSimple As UInteger = 1012
            Public Const cmdButtonDropB As UInteger = 1009
            Public Const cmdButtonDropC As UInteger = 1010
        End Class

        ' ContextPopup CommandName

        Private _ribbon As RibbonStrip
        Public ReadOnly Property Ribbon As RibbonStrip
            Get
                Return _ribbon
            End Get
        End Property
        Private _TabMain As RibbonTab
        Public ReadOnly Property TabMain As RibbonTab
            Get
                Return _TabMain
            End Get
        End Property
        Private _GroupCommon As RibbonGroup
        Public ReadOnly Property GroupCommon As RibbonGroup
            Get
                Return _GroupCommon
            End Get
        End Property
        Private _ButtonNew As RibbonButton
        Public ReadOnly Property ButtonNew As RibbonButton
            Get
                Return _ButtonNew
            End Get
        End Property
        Private _ButtonOpen As RibbonButton
        Public ReadOnly Property ButtonOpen As RibbonButton
            Get
                Return _ButtonOpen
            End Get
        End Property
        Private _ButtonSave As RibbonButton
        Public ReadOnly Property ButtonSave As RibbonButton
            Get
                Return _ButtonSave
            End Get
        End Property
        Private _GroupSimple As RibbonGroup
        Public ReadOnly Property GroupSimple As RibbonGroup
            Get
                Return _GroupSimple
            End Get
        End Property
        Private _ButtonSwitchToAdvanced As RibbonButton
        Public ReadOnly Property ButtonSwitchToAdvanced As RibbonButton
            Get
                Return _ButtonSwitchToAdvanced
            End Get
        End Property
        Private _ButtonDropA As RibbonButton
        Public ReadOnly Property ButtonDropA As RibbonButton
            Get
                Return _ButtonDropA
            End Get
        End Property
        Private _GroupAdvanced As RibbonGroup
        Public ReadOnly Property GroupAdvanced As RibbonGroup
            Get
                Return _GroupAdvanced
            End Get
        End Property
        Private _ButtonSwitchToSimple As RibbonButton
        Public ReadOnly Property ButtonSwitchToSimple As RibbonButton
            Get
                Return _ButtonSwitchToSimple
            End Get
        End Property
        Private _ButtonDropB As RibbonButton
        Public ReadOnly Property ButtonDropB As RibbonButton
            Get
                Return _ButtonDropB
            End Get
        End Property
        Private _ButtonDropC As RibbonButton
        Public ReadOnly Property ButtonDropC As RibbonButton
            Get
                Return _ButtonDropC
            End Get
        End Property

        Public Sub New(ByVal ribbon As RibbonStrip)
            If ribbon Is Nothing Then
                Throw New ArgumentNullException(NameOf(ribbon), "Parameter is Nothing")
            End If
            _ribbon = ribbon
            _TabMain = New RibbonTab(_ribbon, Cmd.cmdTabMain)
            _GroupCommon = New RibbonGroup(_ribbon, Cmd.cmdGroupCommon)
            _ButtonNew = New RibbonButton(_ribbon, Cmd.cmdButtonNew)
            _ButtonOpen = New RibbonButton(_ribbon, Cmd.cmdButtonOpen)
            _ButtonSave = New RibbonButton(_ribbon, Cmd.cmdButtonSave)
            _GroupSimple = New RibbonGroup(_ribbon, Cmd.cmdGroupSimple)
            _ButtonSwitchToAdvanced = New RibbonButton(_ribbon, Cmd.cmdButtonSwitchToAdvanced)
            _ButtonDropA = New RibbonButton(_ribbon, Cmd.cmdButtonDropA)
            _GroupAdvanced = New RibbonGroup(_ribbon, Cmd.cmdGroupAdvanced)
            _ButtonSwitchToSimple = New RibbonButton(_ribbon, Cmd.cmdButtonSwitchToSimple)
            _ButtonDropB = New RibbonButton(_ribbon, Cmd.cmdButtonDropB)
            _ButtonDropC = New RibbonButton(_ribbon, Cmd.cmdButtonDropC)
        End Sub

    End Class
End Namespace
