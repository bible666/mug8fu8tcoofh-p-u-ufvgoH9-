Public Class frmControlSet
    Public okStatus As Boolean = False
    Private iRedId As Integer = -1
    Private iBlueId As Integer = -1
    Private ctl As New clsPlaySound

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If rdbNoLimit.Checked Then
            clsScoreControl.FightType = clsScoreControl.eFightType.NoLimit
        ElseIf rdbGapPoint.Checked Then
            clsScoreControl.FightType = clsScoreControl.eFightType.GapPoint
        ElseIf rdbGapPointRound2.Checked Then
            clsScoreControl.FightType = clsScoreControl.eFightType.GepPointRound2
        End If

        okStatus = True

        clsScoreControl.min = CInt(txtTime.Text.Trim)
        clsScoreControl.KyeShi = CInt(txtKyeShi.Text.Trim)
        If clsScoreControl.ConnType = clsScoreControl.eDBType.Connection Then
            clsScoreControl.FieldName = cboField.Text.Trim.Substring(cboField.Text.Trim.Length - 1) & " "
        Else
            clsScoreControl.FieldName = txtField.Text.Trim & " "
        End If

        clsScoreControl.FieldId = cboField.SelectedValue
        clsScoreControl.FieldSeq = txtRound.Text
        If rdbRoundAuto.Checked Then
            clsScoreControl.Round = 1
            clsScoreControl.eRoundType = clsScoreControl.RoundType.Auto
        ElseIf rdbRoundM.Checked Then
            clsScoreControl.Round = txtShowRound.Text
            clsScoreControl.eRoundType = clsScoreControl.RoundType.Manula
        End If

        clsScoreControl.MaxPoint = txtPoint.Text.Trim
        If rdbJudge1.Checked Then
            clsScoreControl.Jude = 1
        ElseIf rdbJudge2.Checked Then
            clsScoreControl.Jude = 2
        ElseIf rdbJudge3.Checked Then
            clsScoreControl.Jude = 3
        ElseIf rdbJudge4.Checked Then
            clsScoreControl.Jude = 4
        End If
        If rdbRound1.Checked Then
            clsScoreControl.MaxRound = 1
        ElseIf rdbRound2.Checked Then
            clsScoreControl.MaxRound = 2
        ElseIf rdbRound3.Checked Then
            clsScoreControl.MaxRound = 3
        Else
            clsScoreControl.MaxRound = 4
        End If
        clsScoreControl.SetJude()

        clsScoreControl.iRedId = iRedId
        clsScoreControl.sRedName = txtRed.Text.Trim
        clsScoreControl.sRedTeam = txtRedTeam.Text.Trim
        clsScoreControl.iBlueId = iBlueId
        clsScoreControl.sBlueName = txtBlue.Text.Trim
        clsScoreControl.sBlueTeam = txtBlueTeam.Text.Trim
        clsScoreControl.ShowUpdateRound()
        clsScoreControl.WinPlayer = clsScoreControl.Player.None
        clsScoreControl.TimeBreak = txtBreak.Text.Trim
        clsScoreControl.iSoundId = cboSound.SelectedIndex
        clsScoreControl.iSoundAlertId = cboSoundAlert.SelectedIndex

        clsScoreControl.sServer_IP = txtIP.Text.Trim
        If clsScoreControl.sServer_IP.Length > 0 AndAlso clsScoreControl.sServer_IP.Substring(clsScoreControl.sServer_IP.Length - 1, 1) <> "\" Then
            clsScoreControl.sServer_IP &= "\"
        End If
        Select Case cboShowFields.SelectedIndex
            Case 0
                clsScoreControl.sServer_IP &= "Data1"
            Case 1
                clsScoreControl.sServer_IP &= "Data2"
            Case 2
                clsScoreControl.sServer_IP &= "Data3"
            Case 3
                clsScoreControl.sServer_IP &= "Data4"
            Case 4
                clsScoreControl.sServer_IP &= "Data5"
            Case 5
                clsScoreControl.sServer_IP &= "Data6"
            Case 6
                clsScoreControl.sServer_IP &= "Data7"
            Case 7
                clsScoreControl.sServer_IP &= "Data8"
            Case 8
                clsScoreControl.sServer_IP &= "Data9"
            Case 9
                clsScoreControl.sServer_IP &= "Data10"
            Case 10
                clsScoreControl.sServer_IP &= "Data11"
            Case 11
                clsScoreControl.sServer_IP &= "Data12"
            Case 12
                clsScoreControl.sServer_IP &= "Data13"
            Case 13
                clsScoreControl.sServer_IP &= "Data14"
            Case 14
                clsScoreControl.sServer_IP &= "Data15"
            Case 15
                clsScoreControl.sServer_IP &= "Data16"
        End Select

        If txtIP.Text.Trim = "" Then
            clsScoreControl.sServer_IP = ""
        End If

        clsSys.sValueOne = txtValue1.Text
        clsSys.sValueTwo = txtValue2.Text
        clsSys.sValueThree = txtValue3.Text
        clsSys.sValueFour = txtValue4.Text
        clsSys.sValueFive = txtValue5.Text
        Me.Close()
    End Sub

    Private Sub frmControlSet_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If clsScoreControl.ConnType = clsScoreControl.eDBType.Connection Then
            txtField.Visible = False
            cboField.Visible = True
            Dim dt As New DataTable
            Dim sql As String = " SELECT * FROM M_Field"
            clsSys.conn.getData(sql, dt)
            If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
                cboField.DataSource = dt
                cboField.DisplayMember = "FieldNameTh"
                cboField.ValueMember = "FieldId"
                cboField.SelectedValue = clsScoreControl.FieldId
            End If
        Else
            'Not Connection DB
            cboField.Visible = False
            txtField.Visible = True
            txtField.Text = clsScoreControl.FieldName
        End If

        'Set Joy score
        txtValue1.Text = clsSys.sValueOne
        txtValue2.Text = clsSys.sValueTwo
        txtValue3.Text = clsSys.sValueThree
        txtValue4.Text = clsSys.sValueFour
        txtValue5.Text = clsSys.sValueFive


        txtBreak.Text = clsScoreControl.TimeBreak
        cboSound.SelectedIndex = clsScoreControl.iSoundId
        cboSoundAlert.SelectedIndex = clsScoreControl.iSoundAlertId
        txtTime.Text = clsScoreControl.min
        txtKyeShi.Text = clsScoreControl.KyeShi
        txtPoint.Text = clsScoreControl.MaxPoint
        txtRound.Text = clsScoreControl.FieldSeq
        txtShowRound.Text = clsScoreControl.Round
        txtIP.Text = clsScoreControl.sServer_IP
        Select Case clsScoreControl.Jude
            Case 1 : rdbJudge1.Checked = True
            Case 2 : rdbJudge2.Checked = True
            Case 3 : rdbJudge3.Checked = True
            Case 4 : rdbJudge4.Checked = True
        End Select

        Select Case clsScoreControl.MaxRound
            Case 1 : rdbRound1.Checked = True
            Case 2 : rdbRound2.Checked = True
            Case 3 : rdbRound3.Checked = True
            Case 4 : rdbGapPoint.Checked = True
        End Select

        If clsScoreControl.eRoundType = clsScoreControl.RoundType.Auto Then
            rdbRoundAuto.Checked = True
        ElseIf clsScoreControl.eRoundType = clsScoreControl.RoundType.Manula Then
            rdbRoundM.Checked = True
            txtShowRound.Text = clsScoreControl.Round
        End If

        If clsScoreControl.bSendWin = clsScoreControl.eSendWind.Database Then
            rdbSendDb.Checked = True
            txtSendFile.Text = ""
        ElseIf clsScoreControl.bSendWin = clsScoreControl.eSendWind.ServerFolder Then
            rdbSendFile.Checked = True
            txtSendFile.Text = clsScoreControl.sSendFolder
        End If

        iRedId = clsScoreControl.iRedId
        txtRed.Text = clsScoreControl.sRedName
        txtRedTeam.Text = clsScoreControl.sRedTeam
        iBlueId = clsScoreControl.iBlueId
        txtBlue.Text = clsScoreControl.sBlueName
        txtBlueTeam.Text = clsScoreControl.sBlueTeam
        If clsScoreControl.sServer_IP = "" Then
            txtIP.Text = ""
            cboShowFields.SelectedIndex = 0
        Else
            Dim str() As String = clsScoreControl.sServer_IP.Split("\")
            txtIP.Text = ""
            For i As Integer = 0 To str.Length - 2
                txtIP.Text &= str(i) & "\"
            Next
            Select Case str(str.Length - 1)
                Case "Data1"
                    cboShowFields.SelectedIndex = 0
                Case "Data2"
                    cboShowFields.SelectedIndex = 1
                Case "Data3"
                    cboShowFields.SelectedIndex = 2
                Case "Data4"
                    cboShowFields.SelectedIndex = 3
                Case "Data5"
                    cboShowFields.SelectedIndex = 4
                Case "Data6"
                    cboShowFields.SelectedIndex = 5
                Case "Data7"
                    cboShowFields.SelectedIndex = 6
                Case "Data8"
                    cboShowFields.SelectedIndex = 7
                Case "Data9"
                    cboShowFields.SelectedIndex = 8
                Case "Data10"
                    cboShowFields.SelectedIndex = 9
                Case "Data11"
                    cboShowFields.SelectedIndex = 10
                Case "Data12"
                    cboShowFields.SelectedIndex = 11
                Case "Data13"
                    cboShowFields.SelectedIndex = 12
                Case "Data14"
                    cboShowFields.SelectedIndex = 13
                Case "Data15"
                    cboShowFields.SelectedIndex = 14
                Case "Data16"
                    cboShowFields.SelectedIndex = 15
            End Select
        End If


        Select Case clsScoreControl.FightType
            Case clsScoreControl.eFightType.GapPoint
                rdbGapPoint.Checked = True
            Case clsScoreControl.eFightType.NoLimit
                rdbNoLimit.Checked = True
            Case clsScoreControl.eFightType.GepPointRound2
                rdbGapPointRound2.Checked = True
        End Select

    End Sub

    Private Sub txtRound_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRound.TextChanged
        If clsScoreControl.ConnType = clsScoreControl.eDBType.Connection Then
            Dim dt As New DataTable
            If cboField.SelectedIndex >= 0 AndAlso txtRound.Text.Trim <> "" Then
                dt = GetAthleteData(cboField.SelectedValue, txtRound.Text.Trim)
                If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
                    iRedId = IIf(IsDBNull(dt.Rows(0)("RedID")), -1, dt.Rows(0)("RedID"))
                    txtRed.Text = IIf(IsDBNull(dt.Rows(0)("Red")), "", dt.Rows(0)("Red"))
                    txtRedTeam.Text = IIf(IsDBNull(dt.Rows(0)("RedTeam")), "", dt.Rows(0)("RedTeam"))
                    iBlueId = IIf(IsDBNull(dt.Rows(0)("BlueID")), -1, dt.Rows(0)("BlueID"))
                    txtBlue.Text = IIf(IsDBNull(dt.Rows(0)("Blue")), "", dt.Rows(0)("Blue"))
                    txtBlueTeam.Text = IIf(IsDBNull(dt.Rows(0)("BlueTeam")), "", dt.Rows(0)("BlueTeam"))
                Else
                    iRedId = -1
                    txtRed.Text = ""
                    txtRedTeam.Text = ""
                    iBlueId = -1
                    txtBlue.Text = ""
                    txtBlueTeam.Text = ""
                End If
            End If
        End If

    End Sub

    Private Function GetAthleteData(ByVal FieldId As Integer, ByVal ShowSeq As Double) As DataTable
        Dim dt As New DataTable
        Dim sql As String = String.Empty
        sql = " select P1.Prefix_Name + ' ' + A1.Names  + ' ' + A1.Lastname as Blue,A1.AthleteID as BlueID,"
        sql &= "       P2.Prefix_Name + ' ' + A2.Names  + ' ' + A2.Lastname as Red,A2.AthleteID as RedID,"
        sql &= "       A1.Team as BlueTeam,A2.Team as RedTeam"
        sql &= " from DataLine D LEFT JOIN M_Athlete A1 ON D.Athlete_1 = A1.AthleteID"
        sql &= "           LEFT JOIN M_Prefix P1 ON A1.Prefix = P1.Prefix_ID"
        sql &= "           LEFT JOIN M_Athlete A2 ON D.Athlete_2 = A2.AthleteID"
        sql &= "           LEFT JOIN M_Prefix P2 ON A2.Prefix = P2.Prefix_ID"
        sql &= " Where D.FieldId = " & FieldId & " and D.ShowSeq = " & ShowSeq
        clsSys.conn.getData(sql, dt)
        Return dt
    End Function

    Private Sub txtRound_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRound.KeyPress, txtTime.KeyPress _
                                                                   , txtKyeShi.KeyPress, txtRound.KeyPress, txtPoint.KeyPress, txtNowRound.KeyPress, txtNowTime.KeyPress
        Select Case e.KeyChar
            Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", vbBack
                e.Handled = False
            Case Else
                e.Handled = True
        End Select
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If cboSound.SelectedIndex >= 0 Then
            Select Case cboSound.SelectedIndex
                Case 0
                    ctl.Play_Sound_RoundEnd()
                Case Else
                    ctl.Play_Sound_RoundEndNum(cboSound.SelectedIndex)
            End Select
        End If
    End Sub

    Private Sub btnFolderBrower_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFolderBrower.Click
        Dim fol As New FolderBrowserDialog
        fol.Description = "Select the Folder"
        ' Sets the root folder where the browsing starts from 
        fol.RootFolder = Environment.SpecialFolder.Desktop 'Environment.SpecialFolder.MyComputer
        Dim dlgResult As DialogResult = fol.ShowDialog()

        If dlgResult = Windows.Forms.DialogResult.OK Then
            txtSendFile.Text = fol.SelectedPath & "\"
        End If
    End Sub

    Private Sub btnSoundAlert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSoundAlert.Click
        ctl.Play_Sound_Alert_RoundEndNum(cboSoundAlert.SelectedIndex)
    End Sub
End Class