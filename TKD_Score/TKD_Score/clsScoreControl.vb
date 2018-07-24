Public Class clsScoreControl

    Public Enum eFightType
        GapPoint = 1
        NoLimit = 2
        GepPointRound2 = 3
    End Enum

    Public Enum eDBType
        Connection = 0
        NotConnection = 1
    End Enum

    Public Enum eSendWind
        Database = 1
        ServerFolder = 2
    End Enum

    Private Shared Sound As New clsPlaySound
    Public Shared iSoundId As Integer = 0
    Public Shared iSoundAlertId As Integer = 0
    Public Shared TimeBreak As Integer = 20
    Public Shared FightType As eFightType = eFightType.GapPoint
    Public Shared ConnType As eDBType = eDBType.Connection
    Public Shared dtRed As New DataTable
    Public Shared dtBlue As New DataTable
    'Add for check send win player to db or file
    Public Shared bSendWin As eSendWind = eSendWind.Database
    Public Shared sSendFolder As String = ""


#Region " Key Code For Alt and Control A, B, C , .... "
    Public Enum KeyData
        Alt_A = 262209
        Alt_B = 262210
        Alt_C = 262211
        Alt_D = 262212
        Alt_E = 262213
        Alt_F = 262214
        Alt_G = 262215
        Alt_H = 262216
        Alt_I = 262217
        Alt_J = 262218
        Alt_K = 262219
        Alt_L = 262220
        Alt_M = 262221
        Alt_N = 262222
        Alt_O = 262223
        Alt_P = 262224
        Alt_Q = 262225
        Alt_R = 262226
        Alt_S = 262227
        Alt_T = 262228
        Alt_U = 262229
        Alt_V = 262230
        Alt_W = 262231
        Alt_X = 262232
        Alt_1 = 262193
        Alt_2 = 262194
        Alt_3 = 262195
        Alt_4 = 262196
        Alt_5 = 262197
        Alt_6 = 262198
        Alt_7 = 262199
        Alt_8 = 262200
        Alt_9 = 262201

        Ctl_A = 131137
        Ctl_B = 131138
        Ctl_C = 131139
        Ctl_D = 131140
        Ctl_E = 131141
        Ctl_F = 131142
        Ctl_G = 131143
        Ctl_H = 131144
        Ctl_I = 131145
        Ctl_J = 131146
        Ctl_K = 131147
        Ctl_L = 131148
        Ctl_M = 131149
        Ctl_N = 131150
        Ctl_O = 131151
        Ctl_P = 131152
        Ctl_Q = 131153
        Ctl_R = 131154
        Ctl_S = 131155
        Ctl_T = 131156
        Ctl_U = 131157
        Ctl_V = 131158
        Ctl_W = 131159
        Ctl_X = 131160
        Ctl_1 = 131121
        Ctl_2 = 131122
        Ctl_3 = 131123
        Ctl_4 = 131124
        Ctl_5 = 131125
        Ctl_6 = 131126
        Ctl_7 = 131127
        Ctl_8 = 131128
        Ctl_9 = 131129

        Space = 32
    End Enum

#End Region

#Region " Enum  Game Status "
    Public Enum eGameStatus
        EndGame = 0
        Runing = 1
        Break = 2
        EndRound = 3
        Start = 4
        Kyeshi = 5
        StopGame = 6
    End Enum
#End Region

#Region " ข้อมูลของผู้แข่งขัน "
    Public Shared iRedId As Integer
    Public Shared sRedName As String = ""
    Public Shared sRedTeam As String = ""
    Public Shared iBlueId As Integer
    Public Shared sBlueName As String = ""
    Public Shared sBlueTeam As String = ""
#End Region

    Public Enum Player As Integer
        None = 0
        Red = 1
        Blue = 2
    End Enum

    Public Enum RoundType As Integer
        Auto = 0
        Manula = 1
    End Enum

    Public Shared sServer_IP As String = ""
    Public Shared eRoundType As RoundType = RoundType.Auto

    Public Shared WinPlayer As Player = Player.None ' สีที่ชนะการแข่งขัน
    Private Shared iRedScore As Integer = 0
    Private Shared iBlueScore As Integer = 0

    Private Shared iRed_Y_Card As Integer = 0 ' ใบเหลือของแดง
    Private Shared iBlue_Y_Card As Integer = 0 ' ใบเหลืองของน้ำเงิน

    Private Shared iRed_R_Card As Integer = 0 ' ใบแดงของแดง
    Private Shared iBlue_R_Card As Integer = 0 ' ใบแดงของน้ำเงิน

    Private Shared Red_one As New clsScoreMaster
    Private Shared Red_2 As New clsScoreMaster
    Private Shared Red_3 As New clsScoreMaster
    Private Shared Red_4 As New clsScoreMaster
    Private Shared Red_5 As New clsScoreMaster

    Private Shared Blue_1 As New clsScoreMaster
    Private Shared Blue_2 As New clsScoreMaster
    Private Shared Blue_3 As New clsScoreMaster
    Private Shared Blue_4 As New clsScoreMaster
    Private Shared Blue_5 As New clsScoreMaster


    Public Shared frmShow As frmOutSize
    Public Shared frmControl As Form2
    Public Shared GameStatus As eGameStatus = eGameStatus.Start

    Public Shared Jude As Integer = 1

    Private Shared iRound As Integer = 0
    Public Shared Property Round() As Integer
        Get
            Return iRound
        End Get
        Set(ByVal value As Integer)
            iRound = value

        End Set
    End Property

    Private Shared iMaxPoint As Integer = 12
    Public Shared Property MaxPoint() As Integer
        Get
            Return iMaxPoint
        End Get
        Set(ByVal value As Integer)
            iMaxPoint = value
        End Set
    End Property

    Private Shared iMaxRound As Integer = 4
    Public Shared Property MaxRound() As Integer
        Get
            Return iMaxRound
        End Get
        Set(ByVal value As Integer)
            iMaxRound = value
        End Set
    End Property

    Private Shared iMax_Red_Round1 As Integer = 0
    Private Shared iMax_Red_Round2 As Integer = 0
    Private Shared iMax_Red_Round3 As Integer = 0
    Private Shared iMax_Red_Round4 As Integer = 0
    Private Shared iMax_Blue_Round1 As Integer = 0
    Private Shared iMax_Blue_Round2 As Integer = 0
    Private Shared iMax_Blue_Round3 As Integer = 0
    Private Shared iMax_Blue_Round4 As Integer = 0

    Private Shared runTime As DateTime
    Private Shared KyeShiTime As DateTime
    Public Shared min As Integer = 60
    Public Shared KyeShi As Integer = 1
    Public Shared FieldName As String = ""
    Public Shared FieldId As Integer = 1
    Public Shared FieldSeq As Double = 0

    Public Shared Sub SetJude()
        Red_one.Jude = Jude
        Red_2.Jude = Jude
        Red_3.Jude = Jude
        Red_4.Jude = Jude
        Red_5.Jude = Jude

        Blue_1.Jude = Jude
        Blue_2.Jude = Jude
        Blue_3.Jude = Jude
        Blue_4.Jude = Jude
        Blue_5.Jude = Jude

    End Sub

    Public Shared Sub SetKeyFor_1()
        'กดปุ่น 1 คะแนน แดง
        With Red_one
            .Key_Alt_1 = KeyData.Alt_A
            .Key_Alt_2 = KeyData.Alt_I
            .Key_Alt_3 = KeyData.Alt_Q
            .Key_Alt_4 = KeyData.Alt_1

            .Key_Ctl_1 = KeyData.Ctl_A
            .Key_Ctl_2 = KeyData.Ctl_I
            .Key_Ctl_3 = KeyData.Ctl_Q
            .Key_Ctl_4 = KeyData.Ctl_1
            .Jude = Jude
            .AddValue = 1
        End With
        'กดปุ่น 2 คะแนน แดง
        With Red_2
            .Key_Alt_1 = KeyData.Alt_B
            .Key_Alt_2 = KeyData.Alt_J
            .Key_Alt_3 = KeyData.Alt_R
            .Key_Alt_4 = KeyData.Alt_2

            .Key_Ctl_1 = KeyData.Ctl_B
            .Key_Ctl_2 = KeyData.Ctl_J
            .Key_Ctl_3 = KeyData.Ctl_R
            .Key_Ctl_4 = KeyData.Ctl_2
            .Jude = Jude
            .AddValue = 2
        End With
        'กดปุ่น 3 คะแนน แดง
        With Red_3
            .Key_Alt_1 = KeyData.Alt_C
            .Key_Alt_2 = KeyData.Alt_K
            .Key_Alt_3 = KeyData.Alt_S
            .Key_Alt_4 = KeyData.Alt_3

            .Key_Ctl_1 = KeyData.Ctl_C
            .Key_Ctl_2 = KeyData.Ctl_K
            .Key_Ctl_3 = KeyData.Ctl_S
            .Key_Ctl_4 = KeyData.Ctl_3
            .Jude = Jude
            .AddValue = 3
        End With
        'กดปุ่น 4 คะแนน แดง
        With Red_4
            .Key_Alt_1 = KeyData.Alt_D
            .Key_Alt_2 = KeyData.Alt_L
            .Key_Alt_3 = KeyData.Alt_T
            .Key_Alt_4 = KeyData.Alt_4

            .Key_Ctl_1 = KeyData.Ctl_D
            .Key_Ctl_2 = KeyData.Ctl_L
            .Key_Ctl_3 = KeyData.Ctl_T
            .Key_Ctl_4 = KeyData.Ctl_4
            .Jude = Jude
            .AddValue = 4
        End With

        With Red_5
            .Key_Alt_1 = KeyData.Alt_9
            .Jude = Jude
            .AddValue = 5
        End With
       
        With Blue_1
            .Key_Alt_1 = KeyData.Alt_E
            .Key_Alt_2 = KeyData.Alt_M
            .Key_Alt_3 = KeyData.Alt_U
            .Key_Alt_4 = KeyData.Alt_5

            .Key_Ctl_1 = KeyData.Ctl_E
            .Key_Ctl_2 = KeyData.Ctl_M
            .Key_Ctl_3 = KeyData.Ctl_U
            .Key_Ctl_4 = KeyData.Ctl_5
            .Jude = Jude
            .AddValue = 1
        End With
        With Blue_2
            .Key_Alt_1 = KeyData.Alt_F
            .Key_Alt_2 = KeyData.Alt_N
            .Key_Alt_3 = KeyData.Alt_V
            .Key_Alt_4 = KeyData.Alt_6

            .Key_Ctl_1 = KeyData.Ctl_F
            .Key_Ctl_2 = KeyData.Ctl_N
            .Key_Ctl_3 = KeyData.Ctl_V
            .Key_Ctl_4 = KeyData.Ctl_6
            .Jude = Jude
            .AddValue = 2
        End With
        With Blue_3
            .Key_Alt_1 = KeyData.Alt_G
            .Key_Alt_2 = KeyData.Alt_O
            .Key_Alt_3 = KeyData.Alt_W
            .Key_Alt_4 = KeyData.Alt_7

            .Key_Ctl_1 = KeyData.Ctl_G
            .Key_Ctl_2 = KeyData.Ctl_O
            .Key_Ctl_3 = KeyData.Ctl_W
            .Key_Ctl_4 = KeyData.Ctl_7
            .Jude = Jude
            .AddValue = 3
        End With
        With Blue_4
            .Key_Alt_1 = KeyData.Alt_H
            .Key_Alt_2 = KeyData.Alt_P
            .Key_Alt_3 = KeyData.Alt_X
            .Key_Alt_4 = KeyData.Alt_8

            .Key_Ctl_1 = KeyData.Ctl_H
            .Key_Ctl_2 = KeyData.Ctl_P
            .Key_Ctl_3 = KeyData.Ctl_X
            .Key_Ctl_4 = KeyData.Ctl_8
            .Jude = Jude
            .AddValue = 4
        End With

        With Blue_5
            .Key_Alt_1 = KeyData.Ctl_9
            .Jude = Jude
            .AddValue = 5
        End With


    End Sub

    Public Shared Sub SetSeqRed(ByVal iSeq As String)
        frmShow.lblRedSeq.Text = iSeq
    End Sub

    Public Shared Sub SetSeqBlue(ByVal iSeq As String)
        frmShow.lblBlueSeq.Text = iSeq
    End Sub

    Public Shared Sub AddBlueScoreByValue(ByVal iValue)
        Dim OldBlue As Integer = iBlueScore

        If GameStatus <> eGameStatus.Runing Then
            Exit Sub
        End If

        iBlueScore += iValue
      
        If iBlueScore > 0 Then
            frmShow.lblBlue.Text = iBlueScore
            frmControl.lblBlue.Text = iBlueScore
        End If

        CheckWinByScore()
        ShowWinner()
    End Sub
    Public Shared Sub AddRedScoreByValue(ByVal iValue)
        Dim OldRed As Integer = iRedScore

        If GameStatus <> eGameStatus.Runing Then
            Exit Sub
        End If

        iRedScore += iValue
        
        If iRedScore > 0 Then
            frmShow.lblRed.Text = iRedScore
            frmControl.lblRed.Text = iRedScore
        End If

        CheckWinByScore()
        ShowWinner()
    End Sub

    Public Shared Sub AddScoreByKeyData(ByVal KeyData As KeyData)
        Dim OldRed As Integer = iRedScore
        Dim OldBlue As Integer = iBlueScore

        If GameStatus <> eGameStatus.Runing Then
            Exit Sub
        End If

        iRedScore += clsScoreControl.Red_one.SetKeyData(KeyData)
        iRedScore += clsScoreControl.Red_2.SetKeyData(KeyData)
        iRedScore += clsScoreControl.Red_3.SetKeyData(KeyData)
        iRedScore += clsScoreControl.Red_4.SetKeyData(KeyData)
        iRedScore += clsScoreControl.Red_5.SetKeyData(KeyData)
        If iRedScore > 0 Then
            frmShow.lblRed.Text = iRedScore
            frmControl.lblRed.Text = iRedScore
        End If


        iBlueScore += clsScoreControl.Blue_1.SetKeyData(KeyData)
        iBlueScore += clsScoreControl.Blue_2.SetKeyData(KeyData)
        iBlueScore += clsScoreControl.Blue_3.SetKeyData(KeyData)
        iBlueScore += clsScoreControl.Blue_4.SetKeyData(KeyData)
        iBlueScore += clsScoreControl.Blue_5.SetKeyData(KeyData)
        If iBlueScore > 0 Then
            frmShow.lblBlue.Text = iBlueScore
            frmControl.lblBlue.Text = iBlueScore
        End If

        CheckWinByScore()
        ShowWinner()
    End Sub

    Public Shared Sub AddRedScore()
        iRedScore += 1

        frmShow.lblRed.Text = iRedScore
        frmControl.lblRed.Text = iRedScore

        If GameStatus = eGameStatus.Runing Then
            CheckWinByScore()
        End If
    End Sub


    Public Shared Sub MinusRedScore()
        If iRedScore <> 0 Then
            iRedScore -= 1
        End If


        frmShow.lblRed.Text = iRedScore
        frmControl.lblRed.Text = iRedScore

        If GameStatus = eGameStatus.Runing Then
            CheckWinByScore()
        End If
    End Sub

    Public Shared Sub AddBlueScore()

        iBlueScore += 1

        frmShow.lblBlue.Text = iBlueScore
        frmControl.lblBlue.Text = iBlueScore

        If GameStatus = eGameStatus.Runing Then
            CheckWinByScore()
        End If

    End Sub


    Public Shared Sub MinusBlueScore()
        If iBlueScore <> 0 Then
            iBlueScore -= 1
        End If

        frmShow.lblBlue.Text = iBlueScore
        frmControl.lblBlue.Text = iBlueScore

        If GameStatus = eGameStatus.Runing Then
            CheckWinByScore()
        End If

    End Sub

    Public Shared Sub ShowUpdateRound()
        frmControl.lblRound.Text = "Round : " & iRound
        frmShow.lblRound.Text = "R : " & iRound
    End Sub

    Private Shared Sub ShowUpdateRoundEnd()
        frmControl.lblRound.Text = "Round : " & iRound
        frmShow.lblRound.Text = "R : " & iRound
    End Sub

#Region " ส่วนการจัดการใบเหลือง แดง "
    ''' <summary>
    ''' clear ข้อมูลของใบเหลืองแดง ทั้งหมดให้เท่ากับ 0
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub ClearCard()
        iRed_Y_Card = 0
        iBlue_Y_Card = 0 ' ใบเหลืองของน้ำเงิน
        iRed_R_Card = 0 ' ใบแดงของแดง
        iBlue_R_Card = 0 ' ใบแดงของน้ำเงิน

        With frmControl
            .lblRed_Yellow.Text = iRed_Y_Card
            .lblRed_Red.Text = iRed_R_Card
            .lblBlue_Yellow.Text = iBlue_Y_Card
            .lblBlue_Red.Text = iBlue_R_Card
            '.lblRed_Card1.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card1.Tag = "N"
            '.lblRed_Card2.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card2.Tag = "N"
            '.lblRed_Card3.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card3.Tag = "N"
            '.lblRed_Card4.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card4.Tag = "N"
            '.lblRed_Card5.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card5.Tag = "N"
            '.lblRed_Card6.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card6.Tag = "N"
            '.lblRed_Card7.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card7.Tag = "N"
            '.lblRed_Card8.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card8.Tag = "N"

            '.lblBlue_Card1.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card1.Tag = "N"
            '.lblBlue_Card2.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card2.Tag = "N"
            '.lblBlue_Card3.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card3.Tag = "N"
            '.lblBlue_Card4.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card4.Tag = "N"
            '.lblBlue_Card5.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card5.Tag = "N"
            '.lblBlue_Card6.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card6.Tag = "N"
            '.lblBlue_Card7.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card7.Tag = "N"
            '.lblBlue_Card8.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card8.Tag = "N"
        End With

        With frmShow
            .lblRed_Yellow.Text = iRed_Y_Card
            .lblRed_Red.Text = iRed_R_Card
            .lblBlue_Yellow.Text = iBlue_Y_Card
            .lblBlue_Red.Text = iBlue_R_Card
            '.lblRed_Card1.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card2.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card3.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card4.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card5.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card6.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card7.BackgroundImage = .CardList.Images(0)
            '.lblRed_Card8.BackgroundImage = .CardList.Images(0)

            '.lblBlue_Card1.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card1.Tag = "N"
            '.lblBlue_Card2.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card2.Tag = "N"
            '.lblBlue_Card3.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card3.Tag = "N"
            '.lblBlue_Card4.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card4.Tag = "N"
            '.lblBlue_Card5.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card5.Tag = "N"
            '.lblBlue_Card6.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card6.Tag = "N"
            '.lblBlue_Card7.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card7.Tag = "N"
            '.lblBlue_Card8.BackgroundImage = .CardList.Images(0)
            '.lblBlue_Card8.Tag = "N"
        End With
    End Sub

#Region " Control ทีม แดง "
    ''' <summary>
    ''' เพิ่มใบเหลืองให้กับ ทีม แดง
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub AddRed_Yel_Card()
        iRed_Y_Card += 1

        With frmControl
            .lblRed_Yellow.Text = iRed_Y_Card
        End With
        With frmShow
            .lblRed_Yellow.Text = iRed_Y_Card
        End With
        If iRed_Y_Card Mod 2 = 0 Then
            AddBlueScore()
        End If
        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub

    ''' <summary>
    ''' เพิ่มใบแดงให้กับ ทีม แดง
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub AddRed_Red_Card()
        iRed_R_Card += 1

        With frmControl
            .lblRed_Red.Text = iRed_R_Card
        End With
        With frmShow
            .lblRed_Red.Text = iRed_R_Card
        End With
        If iRound = 4 Then
            If iRed_R_Card Mod 2 = 0 Then
                AddBlueScore()
            End If
        Else
            AddBlueScore()
        End If

        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub

    ''' <summary>
    ''' ลดใบเหลืองให้กับ ทีม แดง
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub MinusRed_Yel()
        If iRed_Y_Card <= 0 Then
            Exit Sub
        End If
        iRed_Y_Card -= 1

        With frmControl
            .lblRed_Yellow.Text = iRed_Y_Card
        End With
        With frmShow
            .lblRed_Yellow.Text = iRed_Y_Card
        End With
        If iRed_Y_Card Mod 2 = 1 Then
            MinusBlueScore()
        End If
        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub

    ''' <summary>
    ''' ลดใบแดงให้กับ ทีม แดง
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub MinusRed_Red_Card()
        If iRed_R_Card <= 0 Then
            Exit Sub
        End If
        iRed_R_Card -= 1

        With frmControl
            .lblRed_Red.Text = iRed_R_Card
        End With
        With frmShow
            .lblRed_Red.Text = iRed_R_Card
        End With
        MinusBlueScore()
        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub
#End Region

#Region " Control ทีม น้ำเงิน "
    ''' <summary>
    ''' เพิ่มใบเหลืองให้กับ ทีม น้ำเงิน
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub AddBlue_Yel_Card()
        iBlue_Y_Card += 1

        With frmControl
            .lblBlue_Yellow.Text = iBlue_Y_Card
        End With
        With frmShow
            .lblBlue_Yellow.Text = iBlue_Y_Card
        End With
        If iBlue_Y_Card Mod 2 = 0 Then
            AddRedScore()
        End If
        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub

    ''' <summary>
    ''' เพิ่มใบแดงให้กับ ทีม น้ำเงิน
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub AddBlue_Red_Card()
        iBlue_R_Card += 1

        With frmControl
            .lblBlue_Red.Text = iBlue_R_Card
        End With
        frmShow.lblBlue_Red.Text = iBlue_R_Card
        If iRound = 4 Then
            If iBlue_R_Card Mod 2 = 0 Then
                AddRedScore()
            End If
        Else
            AddRedScore()
        End If

        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub

    ''' <summary>
    ''' ลดใบเหลืองให้กับ ทีม น้ำเงิน
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub MinusBlue_Yel_Card()
        If iBlue_Y_Card <= 0 Then
            Exit Sub
        End If
        iBlue_Y_Card -= 1

        With frmControl
            .lblBlue_Yellow.Text = iBlue_Y_Card
        End With
        frmShow.lblBlue_Yellow.Text = iBlue_Y_Card
        If iBlue_Y_Card Mod 2 = 1 Then
            MinusRedScore()
        End If
        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub

    ''' <summary>
    ''' ลดใบแดงให้กับ ทีม น้ำเงิน
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub MinusBlue_Red_Card()
        If iBlue_R_Card <= 0 Then
            Exit Sub
        End If
        iBlue_R_Card -= 1

        With frmControl
            .lblBlue_Red.Text = iBlue_R_Card
        End With
        frmShow.lblBlue_Red.Text = iBlue_R_Card
        MinusRedScore()
        Sound.Play_Sound_Alert_RoundEndNum(iSoundAlertId)
    End Sub
#End Region


    Private Shared Sub SweatB_To_A(ByRef ControlOrigin As PictureBox, ByVal ControlPath As PictureBox, ByRef ShowOrigin As PictureBox)
        ControlOrigin.Tag = ControlPath.Tag
        ControlOrigin.BackgroundImage = ControlPath.BackgroundImage
        ShowOrigin.BackgroundImage = ControlPath.BackgroundImage

    End Sub
#End Region

    ''' <summary>
    ''' Check ว่าชนะ โดยมีใบเหลือ 1 แต้ม แดง 2 แต้ม รวมได้ 8 แต้มหรือเปล่า
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function WinByCard() As Boolean
        Dim ret As Boolean = False
        If iRed_Y_Card + (iRed_R_Card * 2) >= 8 Then
            GameStatus = eGameStatus.EndGame
            WinPlayer = Player.Blue
            ret = True
        ElseIf iBlue_Y_Card + (iBlue_R_Card * 2) >= 8 Then
            GameStatus = eGameStatus.EndGame
            WinPlayer = Player.Red
            ret = True
        End If
        Return ret
    End Function

    ''' <summary>
    ''' ใช้ตรวจสอบผลการแข่งขันในกรณี หมดเวลา เท่านั้น
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CheckEndRoundByTime()

        Select Case clsScoreControl.FightType
            Case eFightType.GapPoint
                If clsScoreControl.Round = 3 Then
                    If iRedScore > iBlueScore Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore > iRedScore Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                End If
            Case eFightType.NoLimit
                If clsScoreControl.Round = 3 Then
                    If iRedScore > iBlueScore Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore > iRedScore Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                End If
            Case eFightType.GepPointRound2
                If clsScoreControl.Round = 2 Then
                    If iRedScore - iBlueScore >= iMaxPoint Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore - iRedScore >= iMaxPoint Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                ElseIf clsScoreControl.Round = 3 Then
                    If iRedScore > iBlueScore Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore > iRedScore Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                End If
        End Select
        ''บันทึกผลการแข่งขันของรอบนี้
        'Select Case iRound
        '    Case 1
        '        iMax_Red_Round1 = iRedScore
        '        iMax_Blue_Round1 = iBlueScore
        '    Case 2
        '        iMax_Red_Round2 = iRedScore
        '        iMax_Blue_Round2 = iBlueScore
        '        If iRedScore - iBlueScore >= iMaxPoint OrElse (iRound > 3 And iRedScore > iBlueScore) Then
        '            WinPlayer = Player.Red
        '            GameStatus = eGameStatus.EndGame
        '        ElseIf iBlueScore - iRedScore >= iMaxPoint OrElse (iRound > 3 And iBlueScore > iRedScore) Then
        '            WinPlayer = Player.Blue
        '            GameStatus = eGameStatus.EndGame
        '        End If
        '    Case 3
        '        iMax_Red_Round3 = iRedScore
        '        iMax_Blue_Round3 = iBlueScore
        '        If iRedScore > iBlueScore Then
        '            WinPlayer = Player.Red
        '        ElseIf iBlueScore > iRedScore Then
        '            WinPlayer = Player.Blue
        '        End If
        'End Select

        If iRound = 4 Then
            GameStatus = eGameStatus.EndGame 'จบเกมเพราะ รอบที่ 4 เป็นรอบสุดท้ายแล้วก็หมดเวลาแล้วด้วย
        End If
        'บอกเพิ่มเพื่อรอการแข่งขันในรอบต่อไป
        If WinPlayer = Player.None Then
            If GameStatus = eGameStatus.Runing Then
                GameStatus = eGameStatus.Break
            ElseIf GameStatus = eGameStatus.Break Then
                GameStatus = eGameStatus.EndRound ' จบการแข่งขันในรอบนั้น ๆ
                iRound += 1
            End If

            'If iRound = 4 AndAlso WinPlayer = Player.None Then
            '    PlayEndSound(iSoundId)
            'End If
        Else
            GameStatus = eGameStatus.EndGame
        End If

        'ShowUpdateRoundEnd() 'แสดงการจบการแข่งขัน
        'ShowWinner()
        Select Case WinPlayer
            Case Player.Red
                'PlayEndSound(iSoundId)

                For i As Integer = 0 To 5
                    Red_B()
                Next
                MessageBox.Show("แดงชนะจบการแข่งขัน")
            Case Player.Blue
                'PlayEndSound(iSoundId)
                For i As Integer = 0 To 5
                    Blue_B()
                Next
                MessageBox.Show("น้ำเงินชนะจบการแข่งขัน")
        End Select

        Sound = New clsPlaySound
    End Sub

    Public Shared Sub write_log_file()
        Dim log_folder As String = Application.StartupPath + "\log"
        Dim file_name As String = log_folder + "\" + Format(Date.Now, "yyyyMMddHH") + "_" + frmControl.lblField.Text.Trim + ".csv"

        If Not System.IO.Directory.Exists(log_folder) Then
            System.IO.Directory.CreateDirectory(log_folder)
        End If

        Dim Fs As New System.IO.FileStream(file_name, IO.FileMode.Append, IO.FileAccess.Write, IO.FileShare.Write)
        Dim Sw As New System.IO.StreamWriter(Fs)
        Try
            Sw.WriteLine("Round:" + frmControl.lblRound.Text.Trim + ",Field:" + frmControl.lblField.Text.Trim + ",KYESHI:" + frmControl.lblKyeShi.Text.Trim)
            Sw.WriteLine("RedID:" + frmControl.txtRedId.Text.Trim + ",Name:" + frmControl.lblRedName.Text.Trim + ",Team:" + frmControl.lblRedTeam.Text.Trim + ",Position:" + frmControl.cboPositionRed.Text.Trim)
            Sw.WriteLine("BlueID:" + frmControl.txtBlueId.Text.Trim + ",Name:" + frmControl.lblBlueName.Text.Trim + ",Team:" + frmControl.lblBlueTeam.Text.Trim + ",Position:" + frmControl.cboPositionBlue.Text.Trim)
            Sw.WriteLine("Red/Blue,Score,Red Card,Yellow Card,Seq")
            Sw.WriteLine("Red," + frmControl.lblRed.Text.Trim + "," + frmControl.lblRed_Red.Text.Trim + "," + frmControl.lblRed_Yellow.Text.Trim + "," + frmControl.txtRedSeq.Text.Trim)
            Sw.WriteLine("Blue," + frmControl.lblBlue.Text.Trim + "," + frmControl.lblBlue_Red.Text.Trim + "," + frmControl.lblBlue_Yellow.Text.Trim + "," + frmControl.txtBlueSeq.Text.Trim)
            Sw.Close()
            Fs.Close()
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' ตรวจสอบเงือนไขการชนะครั้งสุดท้าย
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub CheckWinByScore()
        'check ผู้ชนะครั้งสุดท้ายตามเงือนไขการกำหนด Round
        'iMaxPoit ถ้ามากกว่าหรือเท่ากับให้ชนะไปเลย
        Select Case FightType
            Case eFightType.GapPoint
                If iRedScore - iBlueScore >= iMaxPoint OrElse (iRound > 3 And iRedScore > iBlueScore) Then
                    WinPlayer = Player.Red
                    GameStatus = eGameStatus.EndGame
                ElseIf iBlueScore - iRedScore >= iMaxPoint OrElse (iRound > 3 And iBlueScore > iRedScore) Then
                    WinPlayer = Player.Blue
                    GameStatus = eGameStatus.EndGame
                ElseIf iRound = 4 Then
                    If iRedScore > iBlueScore Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore > iRedScore Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                End If
            Case eFightType.GepPointRound2
                If iRound = 3 Then
                    If iRedScore - iBlueScore >= iMaxPoint Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore - iRedScore >= iMaxPoint Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                ElseIf iRound = 4 Then
                    If iRedScore > iBlueScore Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore > iRedScore Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                End If

            Case eFightType.NoLimit
                If iRound = 4 Then
                    If iRedScore > iBlueScore Then
                        WinPlayer = Player.Red
                        GameStatus = eGameStatus.EndGame
                    ElseIf iBlueScore > iRedScore Then
                        WinPlayer = Player.Blue
                        GameStatus = eGameStatus.EndGame
                    End If
                End If
        End Select
        If GameStatus = eGameStatus.EndGame Then
            write_log_file()
            ShowWinner()
        End If
        Sound = New clsPlaySound

    End Sub

#Region " แสดงรายการผู้ชนะ "
    ''' <summary>
    ''' แสดงผู้ชนะ
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub ShowWinner()
        Select Case WinPlayer
            Case Player.Red
                PlayEndSound(iSoundId)

                For i As Integer = 0 To 5
                    Red_B()
                Next
                MessageBox.Show("แดงชนะจบการแข่งขัน")
            Case Player.Blue
                PlayEndSound(iSoundId)
                For i As Integer = 0 To 5
                    Blue_B()
                Next
                MessageBox.Show("น้ำเงินชนะจบการแข่งขัน")
        End Select
    End Sub


    Private Shared Sub Red_B()
        frmShow.lblRed.BackColor = Color.White
        frmControl.lblRed.BackColor = Color.White
        frmShow.lblRed.Refresh()
        frmControl.lblRed.Refresh()
        System.Threading.Thread.Sleep(200)
        frmShow.lblRed.BackColor = Color.Yellow
        frmControl.lblRed.BackColor = Color.Yellow
        frmShow.lblRed.Refresh()
        frmControl.lblRed.Refresh()
        System.Threading.Thread.Sleep(200)
        frmShow.lblRed.BackColor = Color.Red
        frmControl.lblRed.BackColor = Color.Black
        frmShow.lblRed.Refresh()
        frmControl.lblRed.Refresh()
        System.Threading.Thread.Sleep(200)
    End Sub
    Private Shared Sub Blue_B()
        frmShow.lblBlue.BackColor = Color.White
        frmControl.lblBlue.BackColor = Color.White
        frmShow.lblBlue.Refresh()
        frmControl.lblBlue.Refresh()
        System.Threading.Thread.Sleep(200)
        frmShow.lblBlue.BackColor = Color.Yellow
        frmControl.lblBlue.BackColor = Color.Yellow
        frmShow.lblBlue.Refresh()
        frmControl.lblBlue.Refresh()
        System.Threading.Thread.Sleep(200)
        frmShow.lblBlue.BackColor = Color.MidnightBlue
        frmControl.lblBlue.BackColor = Color.Black
        frmShow.lblBlue.Refresh()
        frmControl.lblBlue.Refresh()
        System.Threading.Thread.Sleep(200)
    End Sub
    Public Shared Sub PlayEndSound(ByVal iSound As Integer)
        Select Case iSound
            Case 0
                Sound.Play_Sound_RoundEnd()
            Case Else
                Sound.Play_Sound_RoundEndNum(iSound)
        End Select
    End Sub
#End Region


    Public Shared Sub UpdateTime(ByVal pDate As DateTime)
        frmControl.lblTime.Text = Format(pDate, "mm:ss")
        frmControl.lblTime.Refresh()
        frmShow.lblTime.Text = Format(pDate, "mm:ss")
        frmShow.lblTime.Refresh()
    End Sub
    Public Shared Sub UpdateKyeShiTime(ByVal pDate As DateTime)
        frmShow.lblKyeShi.Visible = True
        frmControl.lblKyeShi.Text = Format(pDate, "mm:ss")
        frmShow.lblKyeShi.Text = "K " & Format(pDate, "mm:ss")
        If GameStatus = eGameStatus.Runing Then
            frmShow.lblKyeShi.Visible = True
        End If
    End Sub

    Public Shared Sub setShowRunTimeDefault()
        runTime = New DateTime(2012, 1, 2, 12, 0, 0)
        runTime = runTime.AddSeconds(min)
        frmControl.lblTime.Text = Format(runTime, "mm:ss")
        frmShow.lblTime.Text = Format(runTime, "mm:ss")

        KyeShiTime = New DateTime(2012, 1, 2, 12, KyeShi, 0)
        frmControl.lblKyeShi.Text = Format(KyeShiTime, "mm:ss")
        frmShow.lblKyeShi.Text = "K " & Format(KyeShiTime, "mm:ss")
    End Sub


    ''' <summary>
    ''' Clear คะแนนทุกอย่างให้เป็น 0
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub ClearScore()
        iRedScore = 0
        iBlueScore = 0
        iMax_Red_Round1 = 0
        iMax_Red_Round2 = 0
        iMax_Red_Round3 = 0
        iMax_Red_Round4 = 0
        iMax_Blue_Round1 = 0
        iMax_Blue_Round2 = 0
        iMax_Blue_Round3 = 0
        iMax_Blue_Round4 = 0

        frmShow.lblRed.Text = iRedScore
        frmControl.lblRed.Text = iRedScore

        frmShow.lblBlue.Text = iBlueScore
        frmControl.lblBlue.Text = iBlueScore
    End Sub

    ''' <summary>
    ''' Clear ค่าของใบเหลือง ใบแดง กับ แต้มที่ได้
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub ClearAllScreen()
        ClearCard() 'Clear ใบเหลือ ใบแดง
        ClearScore()
        With frmControl
            .lblField.Text = FieldName & Format(FieldSeq, "0.##")
            .txtRedId.Text = iRedId
            .lblRedName.Text = sRedName
            .lblRedTeam.Text = "ทีม " & sRedTeam
            .txtBlueId.Text = iBlueId
            .lblBlueName.Text = sBlueName
            .lblBlueTeam.Text = "ทีม " & sBlueTeam
        End With

        With frmShow
            .lblField.Text = FieldName & Format(FieldSeq, "0.##")
            .lblRedName.Text = sRedName
            .lblRedTeam.Text = "ทีม " & sRedTeam
            .lblBlueName.Text = sBlueName
            .lblBlueTeam.Text = "ทีม " & sBlueTeam
        End With


        setShowRunTimeDefault()
    End Sub

    Public Function SaveData()
        If WinPlayer = Player.Red Then

        ElseIf WinPlayer = Player.Blue Then

        End If
        Return True
    End Function

    Public Shared Sub SetDisableKyeshi()
        frmShow.lblKyeShi.Visible = True
    End Sub

    Public Shared Sub SetEnableKyeshi()
        frmShow.lblKyeShi.Visible = True
    End Sub

    Public Shared Sub SetOutScreenOnly()
        With frmShow
            .lblField.Text = FieldName & Format(FieldSeq, "0.##")
            .lblRedName.Text = sRedName.Replace("_", " ")
            .lblRedTeam.Text = sRedTeam.Replace("_", " ")
            .lblBlueName.Text = sBlueName.Replace("_", " ")
            .lblBlueTeam.Text = sBlueTeam.Replace("_", " ")
        End With
    End Sub
    Public Shared Sub SetName()
        With frmControl
            .lblField.Text = FieldName & Format(FieldSeq, "0.##")
            If IsNumeric(iRedId) AndAlso iRedId > 0 Then
                .txtRedId.Text = iRedId
            Else
                .txtRedId.Text = ""
            End If

            If .lblRedName.Text <> sRedName Then
                .lblRedName.Text = sRedName
            End If


            If .lblRedTeam.Text.Trim <> "ทีม " & sRedTeam.Trim Then
                .lblRedTeam.Text = "ทีม " & sRedTeam
            End If


            If IsNumeric(iBlueId) AndAlso iBlueId > 0 Then
                .txtBlueId.Text = iBlueId
            Else

                .txtBlueId.Text = ""
            End If

            If .lblBlueName.Text.Trim <> sBlueName.Trim Then
                .lblBlueName.Text = sBlueName
            End If

            If .lblBlueTeam.Text.Trim <> "ทีม " & sBlueTeam.Trim Then
                .lblBlueTeam.Text = "ทีม " & sBlueTeam
            End If

        End With

        With frmShow
            .lblField.Text = FieldName & Format(FieldSeq, "0.##")
            .lblRedName.Text = sRedName
            .lblRedTeam.Text = "ทีม " & sRedTeam
            .lblBlueName.Text = sBlueName
            .lblBlueTeam.Text = "ทีม " & sBlueTeam
        End With
    End Sub

    Public Shared Sub setRedSeq(ByVal iValue As Integer)
        frmShow.lblRedSeq.Text = iValue
        frmControl.txtRedSeq.Text = iValue
    End Sub

    Public Shared Sub setBlueSeq(ByVal iValue As Integer)
        frmShow.lblBlueSeq.Text = iValue
        frmControl.txtBlueSeq.Text = iValue
    End Sub
End Class
