Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblRed.Text = "0"
        'clsScoreControl.frmShow = Me
        'lblTime.Text = Format(Runtime, "mm:ss")
        lblKyeShi.Visible = False

    End Sub

    Private Sub btnImage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImage1.Click
        Dim frm As New OpenFileDialog
        If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim InfoIcon As New Bitmap(frm.FileName)
            btnImage1.BackgroundImage = InfoIcon
        End If
    End Sub
End Class
