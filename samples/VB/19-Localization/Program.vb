'#Const german = 1
'comment or uncomment line before to change language
Imports System.Threading
Imports System.Globalization

Namespace _19_Localization
	Friend NotInheritable Class Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
		End Sub
		<STAThread>
		Shared Sub Main()
#If german Then
			Thread.CurrentThread.CurrentCulture = New CultureInfo("de-DE")
			Thread.CurrentThread.CurrentUICulture = New CultureInfo("de-DE")
#Else
			Thread.CurrentThread.CurrentCulture = New CultureInfo("en-us")
			Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-us")
#End If
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			Application.Run(New Form1())
		End Sub
	End Class
End Namespace
