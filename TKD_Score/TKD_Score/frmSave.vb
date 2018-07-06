Public Class frmSave

    Dim dtPosition As New DataTable
    Dim dtWin As New DataTable

    Public frmSta As Boolean = False

    Private Sub frmSave_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sql As String = ""
        sql = " select * from [ตำแหน่ง]"
        clsSys.conn.getData(sql, dtPosition)

        If Not IsNothing(dtPosition) AndAlso dtPosition.Rows.Count > 0 Then
            With cboPositionRed
                .DataSource = dtPosition.Copy
                .DisplayMember = "Desc"
                .ValueMember = "ID"
                .SelectedValue = -1
                .Text = ""
            End With
            With cboPositionBlue
                .DataSource = dtPosition.Copy
                .DisplayMember = "Desc"
                .ValueMember = "ID"
                .SelectedValue = -1
                .Text = ""
            End With
        End If

        sql = " SELECT * FROM M_WIN_Reason"
        clsSys.conn.getData(sql, dtWin)

        With cboWin
            .DataSource = dtWin
            .DisplayMember = "Win_Desc"
            .ValueMember = "Win_ID"
            .SelectedValue = 3
        End With

        lblBlueName.Text = clsScoreControl.sBlueName
        lblBlueTeam.Text = clsScoreControl.sBlueTeam
        lblRedName.Text = clsScoreControl.sRedName
        lblRedTeam.Text = clsScoreControl.sRedTeam

        'หาว่ารอบนี้เป็น การแข่งขันรอบสุดท้ายของสาย หรือ รอบก่อนสุดท้ายก่อนสาย
        If CheckIsLast() Then
            If clsScoreControl.WinPlayer = clsScoreControl.Player.Red Then
                cboPositionRed.SelectedValue = 0
                cboPositionBlue.SelectedValue = 1
            ElseIf clsScoreControl.WinPlayer = clsScoreControl.Player.Blue Then
                cboPositionRed.SelectedValue = 1
                cboPositionBlue.SelectedValue = 0
            Else
                cboPositionRed.SelectedValue = -1
                cboPositionBlue.SelectedValue = -1
            End If
        ElseIf CheckIsSecondFromLast() Then ' รอบรองสุดท้าย
            If clsScoreControl.WinPlayer = clsScoreControl.Player.Red Then
                cboPositionRed.SelectedValue = -1
                cboPositionBlue.SelectedValue = 2
            ElseIf clsScoreControl.WinPlayer = clsScoreControl.Player.Blue Then
                cboPositionRed.SelectedValue = 2
                cboPositionBlue.SelectedValue = -1
            Else
                cboPositionRed.SelectedValue = -1
                cboPositionBlue.SelectedValue = -1
            End If
        End If

        If clsScoreControl.WinPlayer = clsScoreControl.Player.Red Then
            cboWinA.SelectedIndex = 0
        ElseIf clsScoreControl.WinPlayer = clsScoreControl.Player.Blue Then
            cboWinA.SelectedIndex = 1
        Else
            cboWinA.SelectedIndex = -1
        End If

    End Sub

    ''' <summary>
    ''' ตรวจสอบว่าเป็นรอบสุดท้ายของสาย
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckIsLast() As Boolean
        Dim ret = False
        Dim sql As String = ""
        Dim dt As New DataTable
        sql = "  SELECT * FROM DataLine"
        sql &= " WHERE FieldID = " & clsScoreControl.FieldId & " AND ShowSeq = " & clsScoreControl.FieldSeq
        clsSys.conn.getData(sql, dt)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)("NextSeq")) Then
                ret = True
            End If
        End If
        Return ret
    End Function

    ''' <summary>
    ''' ตรวสอบว่าเป็นรอบรองสุดท้าย
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckIsSecondFromLast() As Boolean
        Dim ret = False
        Dim sql As String = ""
        Dim NextSeq As Double = 0
        Dim dt As New DataTable
        sql = "  SELECT * FROM DataLine"
        sql &= " WHERE FieldID = " & clsScoreControl.FieldId & " AND ShowSeq = " & clsScoreControl.FieldSeq
        clsSys.conn.getData(sql, dt)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)("NextSeq")) Then
                ret = False
            Else
                Dim WeightId As Integer = dt.Rows(0)("WeightId")
                Dim SexId As Integer = dt.Rows(0)("SexId")
                NextSeq = dt.Rows(0)("NextSeq")
                sql = "  SELECT * FROM DataLine"
                sql &= " WHERE FieldID = " & clsScoreControl.FieldId & " AND Seq = " & NextSeq & " AND WeightId = " & WeightId & " AND SexId = " & SexId
                Dim dt2 As New DataTable
                clsSys.conn.getData(sql, dt2)
                If Not IsNothing(dt2) AndAlso dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0)("NextSeq")) Then
                        ret = True
                    End If
                End If
            End If
        End If
        Return ret
    End Function

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        frmSta = SaveData()
        Me.Close()
    End Sub

    Private Function SaveData() As Boolean
        'Set Winplayer
        If cboWinA.SelectedIndex = 0 Then
            clsScoreControl.WinPlayer = clsScoreControl.Player.Red
        ElseIf cboWinA.SelectedIndex = 1 Then
            clsScoreControl.WinPlayer = clsScoreControl.Player.Blue
        Else
            clsScoreControl.WinPlayer = clsScoreControl.Player.None
        End If
        If clsScoreControl.bSendWin = clsScoreControl.eSendWind.Database Then
            SaveToDB(clsScoreControl.FieldId, clsScoreControl.FieldSeq, cboPositionRed.SelectedValue, cboPositionBlue.SelectedValue, _
                             cboWin.SelectedValue, clsScoreControl.WinPlayer, clsScoreControl.iRedId, clsScoreControl.iBlueId)
        ElseIf clsScoreControl.bSendWin = clsScoreControl.eSendWind.ServerFolder Then
            If clsScoreControl.sSendFolder.Trim = "" Then
                MessageBox.Show("กำหนด Path ผิดพลาด กรุณา setup ใหม่")
            Else
                SaveToFile(clsScoreControl.FieldId, clsScoreControl.FieldSeq, cboPositionRed.SelectedValue, cboPositionBlue.SelectedValue, _
                               cboWin.SelectedValue, clsScoreControl.WinPlayer, clsScoreControl.iRedId, clsScoreControl.iBlueId)
            End If
        End If

        Return True
    End Function

    Private Function SaveToDB(ByVal pFieldId As Integer, ByVal pFieldSeq As Integer, _
                              ByVal pRedPosition As Integer, ByVal pBluePosition As Integer, _
                              ByVal pWinType As Integer, ByVal pWinPlayer As clsScoreControl.Player, _
                              ByVal pRedId As Integer, ByVal pBlueId As Integer) As Boolean
        Dim sql As String = ""
        Dim dtDataLine As New DataTable
        sql = " SELECT * FROM DataLine WHERE FieldID = " & pFieldId & " AND ShowSeq = " & pFieldSeq
        Try
            clsSys.conn.getData(sql, dtDataLine)
            If Not IsNothing(dtDataLine) AndAlso dtDataLine.Rows.Count > 0 Then
                Dim drLine As DataRow = dtDataLine.Rows(0)
                'FieldId = 2 and WeightId = 8 and SexId = 2 and PageId = 1
                Dim Weightid As Integer = drLine("WeightId")
                Dim SexId As Integer = drLine("SexId")
                Dim PageId As Integer = drLine("PageId")
                Dim Seq As Integer = drLine("Seq")
                Dim NextSeq As Integer = IIf(IsDBNull(drLine("NextSeq")), -1, drLine("NextSeq"))
                Dim iRedId As Integer = drLine("Athlete_2")
                Dim iBlueId As Integer = drLine("Athlete_1")
                Dim iWin_Athlete As Integer = IIf(IsDBNull(drLine("Next_Athlete")), 0, drLine("Next_Athlete"))

                If pRedPosition >= 0 Then
                    'Update Postion For Red
                    sql = " UPDATE M_Athlete SET [ตำแหน่ง] = " & pRedPosition & " , PrintSta = 0 "
                    sql &= " WHERE AthleteID = " & iRedId
                    clsSys.conn.runSQL(sql)
                End If

                If pBluePosition >= 0 Then
                    'Update Postion For Red
                    sql = " UPDATE M_Athlete SET [ตำแหน่ง] = " & pBluePosition & " , PrintSta = 0 "
                    sql &= " WHERE AthleteID = " & iBlueId
                    clsSys.conn.runSQL(sql)
                End If

                'Update Now Seq with win condition บันทึกเงือนไขการชนะ
                sql = " UPDATE DataLine SET Win_Id = " & pWinType
                Select Case pWinPlayer
                    Case clsScoreControl.Player.Blue
                        sql &= " , Win_Flag = 1"
                    Case clsScoreControl.Player.Red
                        sql &= " , Win_Flag = 2"
                    Case clsScoreControl.Player.None
                End Select
                sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & Seq
                If Not clsSys.conn.runSQL(sql) Then
                    Return False
                End If

                'Update Athlete For Next Seq บันทึกผู้ชนะไปรอบถัดไป
                Dim DtNext As New DataTable
                sql = " SELECT * FROM DataLine"
                sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & NextSeq
                clsSys.conn.getData(sql, DtNext)
                If Not IsNothing(DtNext) AndAlso DtNext.Rows.Count > 0 Then
                    'มีข้อมูล Next Seq ให้ Update
                    'Check ข้อมูล Athlete
                    Dim WinId As Integer
                    Dim Ath_1 As Integer = IIf(IsDBNull(DtNext.Rows(0)("Athlete_1")), -1, DtNext.Rows(0)("Athlete_1"))
                    Dim Ath_2 As Integer = IIf(IsDBNull(DtNext.Rows(0)("Athlete_2")), -1, DtNext.Rows(0)("Athlete_2"))
                    Select Case pWinPlayer
                        Case clsScoreControl.Player.None : WinId = -1
                        Case clsScoreControl.Player.Red : WinId = pRedId
                        Case clsScoreControl.Player.Blue : WinId = pBlueId
                    End Select

                    If iWin_Athlete = 0 Then
                        If Ath_1 < 0 Then
                            sql = " UPDATE DataLine SET Athlete_1 = " & WinId
                            sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                            sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & NextSeq
                            If Not clsSys.conn.runSQL(sql) Then
                                Return False
                            End If

                            'Update Now Seq with win condition บันทึกเงือนไขการชนะ
                            sql = " UPDATE DataLine SET Next_Athlete = 1"
                            sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                            sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & Seq
                            If Not clsSys.conn.runSQL(sql) Then
                                Return False
                            End If
                        ElseIf Ath_2 < 0 Then
                            sql = " UPDATE DataLine SET Athlete_2 = " & WinId
                            sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                            sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & NextSeq
                            If Not clsSys.conn.runSQL(sql) Then
                                Return False
                            End If

                            sql = " UPDATE DataLine SET Next_Athlete = 2"
                            sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                            sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & Seq
                            If Not clsSys.conn.runSQL(sql) Then
                                Return False
                            End If
                        End If

                    ElseIf iWin_Athlete = 1 Then
                        sql = " UPDATE DataLine SET Athlete_1 = " & WinId
                        sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                        sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & NextSeq
                        If Not clsSys.conn.runSQL(sql) Then
                            Return False
                        End If
                    ElseIf iWin_Athlete = 2 Then
                        sql = " UPDATE DataLine SET Athlete_2 = " & WinId
                        sql &= " WHERE FieldId = " & pFieldId & " AND WeightId = " & Weightid
                        sql &= "     AND SexId = " & SexId & " AND PageId = " & PageId & " AND SEQ = " & NextSeq
                        If Not clsSys.conn.runSQL(sql) Then
                            Return False
                        End If
                    End If

                End If

            End If

        Catch ex As Exception

        End Try
        Return True
    End Function


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Public Function SaveToFile(ByVal pFieldId As Integer, ByVal pFieldSeq As Integer, _
                             ByVal pRedPosition As Integer, ByVal pBluePosition As Integer, _
                             ByVal pWinType As Integer, ByVal pWinPlayer As clsScoreControl.Player, _
                             ByVal pRedId As Integer, ByVal pBlueId As Integer) As Boolean

        Dim TargetFile As String = clsScoreControl.sSendFolder.Trim & "F" & pFieldId & "S" & pFieldSeq & "_" & Format(Now, "yyyyMMddHHmmss") & ".csv"
        Dim SortFile As String = Application.StartupPath & "\Data\" & "F" & pFieldId & "S" & pFieldSeq & "_" & Format(Now, "yyyyMMddHHmmss") & ".csv"
        Dim SortOkFile As String = Application.StartupPath & "\Data\OK\" & "F" & pFieldId & "S" & pFieldSeq & "_" & Format(Now, "yyyyMMddHHmmss") & ".csv"
        Dim SortNGFile As String = Application.StartupPath & "\Data\NG\" & "F" & pFieldId & "S" & pFieldSeq & "_" & Format(Now, "yyyyMMddHHmmss") & ".csv"
        Dim sta As Boolean = True

        Try
            'SendHeader()
            Dim Fs As New System.IO.FileStream(SortFile, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.Write)
            Dim Sw As New System.IO.StreamWriter(Fs)

            Sw.WriteLine(pFieldId & "|" & pFieldSeq & "|" & pRedPosition & "|" & pBluePosition & "|" & pWinType & "|" & pWinPlayer & "|" & pRedId & "|" & pBlueId)

            Sw.Close()
            Fs.Close()

            System.IO.File.Copy(SortFile, TargetFile, True)


        Catch ex As Exception
            sta = False
        End Try

        Try
            If sta = True Then
                System.IO.File.Copy(SortFile, SortOkFile, True)
                System.IO.File.Delete(SortFile)
                'System.IO.File.Move(SortFile, SortOkFile)
            Else
                System.IO.File.Copy(SortFile, SortNGFile, True)
                System.IO.File.Delete(SortFile)
                'System.IO.File.Move(SortFile, SortNGFile)
            End If
        Catch ex As Exception
            sta = False
        End Try

        Return sta
    End Function
End Class