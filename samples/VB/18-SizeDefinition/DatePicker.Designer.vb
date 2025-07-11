<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DatePicker
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        MonthCalendar = New MonthCalendar()
        SuspendLayout()
        ' 
        ' MonthCalendar
        ' 
        MonthCalendar.Dock = DockStyle.Fill
        MonthCalendar.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        MonthCalendar.Location = New Point(0, 0)
        MonthCalendar.Name = "MonthCalendar"
        MonthCalendar.TabIndex = 0
        ' 
        ' DatePicker
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(178, 162)
        Controls.Add(MonthCalendar)
        FormBorderStyle = FormBorderStyle.None
        MaximizeBox = False
        MinimizeBox = False
        Name = "DatePicker"
        ShowIcon = False
        ShowInTaskbar = False
        StartPosition = FormStartPosition.Manual
        Text = "DatePicker"
        ResumeLayout(False)
    End Sub

    Friend WithEvents MonthCalendar As MonthCalendar
End Class
