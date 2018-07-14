Public Class frmOutSize
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblRed.Text = "0"
        clsScoreControl.frmShow = Me
        'lblTime.Text = Format(Runtime, "mm:ss")
        lblKyeShi.Visible = False
        lblRedSeq.Text = ""
        lblBlueSeq.Text = ""
    End Sub
End Class