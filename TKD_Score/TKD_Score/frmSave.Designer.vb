<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSave
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cboPositionRed = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblRedTeam = New System.Windows.Forms.Label
        Me.lblRedName = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.cboPositionBlue = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblBlueTeam = New System.Windows.Forms.Label
        Me.lblBlueName = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboWin = New System.Windows.Forms.ComboBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.cboWinA = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Red
        Me.Panel1.Controls.Add(Me.cboPositionRed)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblRedTeam)
        Me.Panel1.Controls.Add(Me.lblRedName)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(460, 140)
        Me.Panel1.TabIndex = 1
        '
        'cboPositionRed
        '
        Me.cboPositionRed.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold)
        Me.cboPositionRed.FormattingEnabled = True
        Me.cboPositionRed.Location = New System.Drawing.Point(93, 92)
        Me.cboPositionRed.Name = "cboPositionRed"
        Me.cboPositionRed.Size = New System.Drawing.Size(345, 35)
        Me.cboPositionRed.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(8, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 27)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "ตำแหน่ง"
        '
        'lblRedTeam
        '
        Me.lblRedTeam.AutoSize = True
        Me.lblRedTeam.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRedTeam.ForeColor = System.Drawing.Color.White
        Me.lblRedTeam.Location = New System.Drawing.Point(8, 49)
        Me.lblRedTeam.Name = "lblRedTeam"
        Me.lblRedTeam.Size = New System.Drawing.Size(117, 27)
        Me.lblRedTeam.TabIndex = 1
        Me.lblRedTeam.Text = "ทีม test...1"
        '
        'lblRedName
        '
        Me.lblRedName.AutoSize = True
        Me.lblRedName.Font = New System.Drawing.Font("Comic Sans MS", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRedName.ForeColor = System.Drawing.Color.White
        Me.lblRedName.Location = New System.Drawing.Point(8, 6)
        Me.lblRedName.Name = "lblRedName"
        Me.lblRedName.Size = New System.Drawing.Size(157, 30)
        Me.lblRedName.TabIndex = 0
        Me.lblRedName.Text = "พรนภา พิมภา"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.Blue
        Me.Panel2.Controls.Add(Me.cboPositionBlue)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.lblBlueTeam)
        Me.Panel2.Controls.Add(Me.lblBlueName)
        Me.Panel2.Location = New System.Drawing.Point(481, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(459, 140)
        Me.Panel2.TabIndex = 2
        '
        'cboPositionBlue
        '
        Me.cboPositionBlue.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold)
        Me.cboPositionBlue.FormattingEnabled = True
        Me.cboPositionBlue.Location = New System.Drawing.Point(92, 93)
        Me.cboPositionBlue.Name = "cboPositionBlue"
        Me.cboPositionBlue.Size = New System.Drawing.Size(345, 35)
        Me.cboPositionBlue.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(7, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 27)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "ตำแหน่ง"
        '
        'lblBlueTeam
        '
        Me.lblBlueTeam.AutoSize = True
        Me.lblBlueTeam.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBlueTeam.ForeColor = System.Drawing.Color.White
        Me.lblBlueTeam.Location = New System.Drawing.Point(7, 49)
        Me.lblBlueTeam.Name = "lblBlueTeam"
        Me.lblBlueTeam.Size = New System.Drawing.Size(117, 27)
        Me.lblBlueTeam.TabIndex = 1
        Me.lblBlueTeam.Text = "ทีม test...1"
        '
        'lblBlueName
        '
        Me.lblBlueName.AutoSize = True
        Me.lblBlueName.Font = New System.Drawing.Font("Comic Sans MS", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBlueName.ForeColor = System.Drawing.Color.White
        Me.lblBlueName.Location = New System.Drawing.Point(7, 6)
        Me.lblBlueName.Name = "lblBlueName"
        Me.lblBlueName.Size = New System.Drawing.Size(157, 30)
        Me.lblBlueName.TabIndex = 0
        Me.lblBlueName.Text = "พรนภา พิมภา"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(12, 173)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 27)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "ชนะโดย"
        '
        'cboWin
        '
        Me.cboWin.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold)
        Me.cboWin.FormattingEnabled = True
        Me.cboWin.Location = New System.Drawing.Point(97, 170)
        Me.cboWin.Name = "cboWin"
        Me.cboWin.Size = New System.Drawing.Size(843, 35)
        Me.cboWin.TabIndex = 4
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(12, 210)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(100, 41)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(840, 210)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 41)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cboWinA
        '
        Me.cboWinA.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold)
        Me.cboWinA.FormattingEnabled = True
        Me.cboWinA.Items.AddRange(New Object() {"แดงชนะ", "น้ำเงินชนะ"})
        Me.cboWinA.Location = New System.Drawing.Point(322, 216)
        Me.cboWinA.Name = "cboWinA"
        Me.cboWinA.Size = New System.Drawing.Size(334, 35)
        Me.cboWinA.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(232, 219)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 27)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "ผู้ชนะ"
        '
        'frmSave
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(952, 263)
        Me.Controls.Add(Me.cboWinA)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.cboWin)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSave"
        Me.Text = "บันทึก"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblRedTeam As System.Windows.Forms.Label
    Friend WithEvents lblRedName As System.Windows.Forms.Label
    Friend WithEvents cboPositionRed As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents cboPositionBlue As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblBlueTeam As System.Windows.Forms.Label
    Friend WithEvents lblBlueName As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboWin As System.Windows.Forms.ComboBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cboWinA As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
