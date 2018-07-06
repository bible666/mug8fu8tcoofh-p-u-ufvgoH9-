Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.DirectX.DirectInput
Imports System.Management
Imports System.Security.Cryptography
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input

Public Class Form2
    Private Enum RunType
        Run = 0
        Bread = 1
    End Enum

    Private Enum eJoyType
        xBox = 0
        PS = 1
    End Enum

    Private joystickDevice1 As Device = Nothing
    Private VibrationCountDown_joy1 As Integer = 0
    Private joystickDevice2 As Device = Nothing
    Private joystickDevice3 As Device = Nothing
    Private joystickDevice4 As Device = Nothing

    Private JoyType As eJoyType = eJoyType.xBox
    Private runTime As DateTime
    Private KyeShiTime As DateTime
    Private KyeShiEnable As Boolean = False
    Private clientSocket As Socket
    Private byteData(1023) As Byte
    Private RunMode As RunType = RunType.Run
    Private isUpdateScore As Boolean = False
    Private IP_Server As String = ""
    Private Msg_From_Server As String = ""
    Private sHeader As String = ""
    Private sDetail As String = ""
    Private iSeq As Integer = 0
    Private sFileSerial As String = ""
    Private isFirst As Boolean = True
    Private isLoadDB As Boolean = False

    Private Sub TimerStart()
        Timer1.Start()
        Timer2.Start()
        Timer3.Start()
        Timer4.Start()
    End Sub

    Private Sub TimerStop()
        Timer1.Stop()
        Timer2.Stop()
        Timer3.Stop()
        Timer4.Stop()
    End Sub

    Private Function GetNewFile(ByVal OldFile As String) As String
        Static Seq As Integer
        Dim NewFile As String = ""

        Seq += 1
        If Seq = 600 Then
            iSeq = 1
        End If
        NewFile = OldFile & "_" & Seq & ".txt"

        Return NewFile
    End Function

#Region " Joy Stick Control "

    Private Sub GetJoyStick()
        ' List of attached joysticks
        Dim gameControllerList As DeviceList = Nothing
        Try
            gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


        ' There is one controller at least?
        If (gameControllerList.Count > 0) Then
            For i As Integer = 0 To gameControllerList.Count - 1
                ' Create an object instance
                gameControllerList.MoveNext()
                Dim deviceInstance As DeviceInstance = gameControllerList.Current ' gameControllerList(i)
                Select Case i
                    Case 0
                        joystickDevice1 = New Device(deviceInstance.InstanceGuid)
                        With joystickDevice1
                            .SetCooperativeLevel(Me, CooperativeLevelFlags.Background Or CooperativeLevelFlags.NonExclusive)
                            .SetDataFormat(DeviceDataFormat.Joystick)
                            .Acquire()
                        End With

                    Case 1
                        joystickDevice2 = New Device(deviceInstance.InstanceGuid)
                        With joystickDevice2
                            .SetCooperativeLevel(Me, CooperativeLevelFlags.Background Or CooperativeLevelFlags.NonExclusive)
                            .SetDataFormat(DeviceDataFormat.Joystick)
                            .Acquire()
                        End With
                    Case 2
                        joystickDevice3 = New Device(deviceInstance.InstanceGuid)
                        With joystickDevice3
                            .SetCooperativeLevel(Me, CooperativeLevelFlags.Background Or CooperativeLevelFlags.NonExclusive)
                            .SetDataFormat(DeviceDataFormat.Joystick)
                            .Acquire()
                        End With
                    Case 3
                        joystickDevice4 = New Device(deviceInstance.InstanceGuid)
                        With joystickDevice4
                            .SetCooperativeLevel(Me, CooperativeLevelFlags.Background Or CooperativeLevelFlags.NonExclusive)
                            .SetDataFormat(DeviceDataFormat.Joystick)
                            .Acquire()
                        End With
                End Select
            Next

        End If
    End Sub

    Public Sub GrdRedLast()
        If grdData_Red.RowCount > 0 Then
            grdData_Red.CurrentCell = grdData_Red.Rows(grdData_Red.RowCount - 1).Cells(0)
        End If
    End Sub

    Public Sub GrdBlueLast()
        If grdDataBlue.RowCount > 0 Then
            grdDataBlue.CurrentCell = grdDataBlue.Rows(grdDataBlue.RowCount - 1).Cells(0)
        End If
    End Sub

  
    ''' <summary>
    ''' Get Event From JoyStick 1
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub joystickPolling1()
        Static isRedInput As Boolean = False ' ยังไม่มีการกด
        Static isBlueInput As Boolean = False

        Select Case JoyType
            Case eJoyType.xBox
                ''******************* Code For XBox Joy
                Dim payerIndex As PlayerIndex = PlayerIndex.One
                Dim gamePadState As GamePadState

                VibrationCountDown_joy1 = 1

                gamePadState = GamePad.GetState(payerIndex)

                If (gamePadState.DPad.Down = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    lbl_J_R_1.BackColor = Color.Red
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_A)
                        isRedInput = True
                        'Add Red Score
                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.DPad.Up = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_B)
                        isRedInput = True
                        lbl_J_R_1.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.Buttons.LeftShoulder = Input.ButtonState.Pressed) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_C)
                        isRedInput = True
                        lbl_J_R_1.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf CInt((gamePadState.Triggers.Left * 100)) = 100 Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_D)
                        isRedInput = True
                        lbl_J_R_1.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)

                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.Buttons.Back = Input.ButtonState.Pressed) Then
                    MessageBox.Show("is back button")
                ElseIf (gamePadState.Buttons.Start = ButtonState.Pressed) Then
                    MessageBox.Show("is start")
                Else
                    isRedInput = False
                    lbl_J_R_1.BackColor = Color.Black
                End If

                If (gamePadState.Buttons.A = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_E)
                        isBlueInput = True
                        lbl_J_B_1.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.Y = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_F)
                        isBlueInput = True
                        lbl_J_B_1.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.RightShoulder = Input.ButtonState.Pressed) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_G)
                        isBlueInput = True
                        lbl_J_B_1.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf CInt((gamePadState.Triggers.Right * 100)) = 100 Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_H)
                        isBlueInput = True
                        lbl_J_B_1.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "1"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                Else
                    isBlueInput = False
                    lbl_J_B_1.BackColor = Color.Black
                End If
            Case eJoyType.PS
                ' ****************** Code For PS Joy
                If IsNothing(joystickDevice1) Then
                    Exit Sub
                End If
                Try
                    joystickDevice1.Poll()
                    Dim state As JoystickState = joystickDevice1.CurrentJoystickState

                    If state.Y = 65535 Then ' Down Key
                        lbl_J_R_1.BackColor = Color.Red
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_A)
                            isRedInput = True
                            'Add Red Score
                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If
                    If state.Y = 0 Then 'Up Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_B)
                            isRedInput = True
                            lbl_J_R_1.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If
                    If state.GetButtons(6) > 0 Then ' L1 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_C)
                            isRedInput = True
                            lbl_J_R_1.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If
                    If state.GetButtons(4) > 0 Then ' L2 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_D)
                            isRedInput = True
                            lbl_J_R_1.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)

                            GrdRedLast()
                        End If

                    End If
                    If state.GetButtons(2) > 0 Then ' X Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_E)
                            isBlueInput = True
                            lbl_J_B_1.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(0) > 0 Then ' /_\ Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_F)
                            isBlueInput = True
                            lbl_J_B_1.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)
                            GrdBlueLast()
                        End If

                    End If

                    If state.GetButtons(7) > 0 Then ' R1 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_G)
                            isBlueInput = True
                            lbl_J_B_1.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)
                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(5) > 0 Then ' R2 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_H)
                            isBlueInput = True
                            lbl_J_B_1.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "1"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If

                    If Not ((state.Y = 65535) OrElse (state.Y = 0) OrElse (state.GetButtons(6) > 0) OrElse (state.GetButtons(4) > 0)) Then
                        isRedInput = False
                        lbl_J_R_1.BackColor = Color.Black

                    End If

                    If Not ((state.GetButtons(2) > 0) OrElse (state.GetButtons(0) > 0) OrElse (state.GetButtons(7) > 0) _
                        OrElse (state.GetButtons(5) > 0)) Then ' R2 Key
                        isBlueInput = False
                        lbl_J_B_1.BackColor = Color.Black
                    End If

                Catch ex As Exception

                End Try
        End Select
      
    End Sub

    ''' <summary>
    ''' Get Event From JoyStick 2
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub joystickPolling2()
        Static isRedInput As Boolean = False ' ยังไม่มีการกด
        Static isBlueInput As Boolean = False

        Select Case JoyType
            Case eJoyType.xBox
                '******************* Code For XBox Joy
                Dim payerIndex As PlayerIndex = PlayerIndex.Two
                Dim gamePadState As GamePadState

                VibrationCountDown_joy1 = 1

                gamePadState = GamePad.GetState(payerIndex)

                If (gamePadState.DPad.Down = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    lbl_J_R_2.BackColor = Color.Red
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_I)
                        isRedInput = True
                        'Add Red Score
                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.DPad.Up = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_J)
                        isRedInput = True
                        lbl_J_R_2.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.Buttons.LeftShoulder = Input.ButtonState.Pressed) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_K)
                        isRedInput = True
                        lbl_J_R_2.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf CInt((gamePadState.Triggers.Left * 100)) = 100 Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_L)
                        isRedInput = True
                        lbl_J_R_2.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)

                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                Else
                    isRedInput = False
                    lbl_J_R_2.BackColor = Color.Black
                End If

                If (gamePadState.Buttons.A = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_M)
                        isBlueInput = True
                        lbl_J_B_2.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.Y = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_N)
                        isBlueInput = True
                        lbl_J_B_2.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.RightShoulder = Input.ButtonState.Pressed) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_O)
                        isBlueInput = True
                        lbl_J_B_2.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf CInt((gamePadState.Triggers.Right * 100)) = 100 Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_P)
                        isBlueInput = True
                        lbl_J_B_2.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "2"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                Else
                    isBlueInput = False
                    lbl_J_B_2.BackColor = Color.Black
                End If
            Case eJoyType.PS
                '****************** Code For PS Joy
                If IsNothing(joystickDevice2) Then
                    Exit Sub
                End If
                Try
                    joystickDevice2.Poll()

                    Dim state As JoystickState = joystickDevice2.CurrentJoystickState

                    If state.Y = 65535 Then ' Down Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_I)
                            isRedInput = True
                            lbl_J_R_2.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)

                            GrdRedLast()
                        End If

                    End If
                    If state.Y = 0 Then 'Up Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_J)
                            isRedInput = True
                            lbl_J_R_2.BackColor = Color.Red
                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If


                    End If
                    If state.GetButtons(6) > 0 Then ' L1 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_K)
                            isRedInput = True
                            lbl_J_R_2.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If

                    If state.GetButtons(4) > 0 Then ' L2 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_L)
                            isRedInput = True
                            lbl_J_R_2.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If
                    If state.GetButtons(2) > 0 Then ' X Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_M)
                            isBlueInput = True
                            lbl_J_B_2.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)
                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(0) > 0 Then ' /_\ Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_N)
                            isBlueInput = True
                            lbl_J_B_2.BackColor = Color.Blue
                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)
                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(7) > 0 Then ' R1 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_O)
                            isBlueInput = True
                            lbl_J_B_2.BackColor = Color.Blue
                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)
                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(5) > 0 Then ' R2 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_P)
                            isBlueInput = True
                            lbl_J_B_2.BackColor = Color.Blue
                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "2"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)
                            GrdBlueLast()
                        End If

                    End If


                    'If Not ((state.Y = 65535) OrElse (state.Y = 0) OrElse (state.GetButtons(6) > 0) OrElse (state.GetButtons(4) > 0) _
                    '         OrElse (state.GetButtons(2) > 0) OrElse (state.GetButtons(0) > 0) OrElse (state.GetButtons(7) > 0) _
                    '        OrElse (state.GetButtons(5) > 0)) Then ' R2 Key
                    '    isInput = False
                    'End If

                    If Not ((state.Y = 65535) OrElse (state.Y = 0) OrElse (state.GetButtons(6) > 0) OrElse (state.GetButtons(4) > 0)) Then
                        isRedInput = False
                        lbl_J_R_2.BackColor = Color.Black
                    End If

                    If Not ((state.GetButtons(2) > 0) OrElse (state.GetButtons(0) > 0) OrElse (state.GetButtons(7) > 0) _
                        OrElse (state.GetButtons(5) > 0)) Then ' R2 Key
                        isBlueInput = False
                        lbl_J_B_2.BackColor = Color.Black
                    End If

                Catch ex As Exception

                End Try
        End Select
       

      
    End Sub

    ''' <summary>
    ''' Get Event From JoyStick 3
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub joystickPolling3()
        Static isRedInput As Boolean = False ' ยังไม่มีการกด
        Static isBlueInput As Boolean = False

        Select Case JoyType
            Case eJoyType.xBox
                '******************* Code For XBox Joy
                Dim payerIndex As PlayerIndex = PlayerIndex.Three
                Dim gamePadState As GamePadState

                VibrationCountDown_joy1 = 1

                gamePadState = GamePad.GetState(payerIndex)

                If (gamePadState.DPad.Down = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    lbl_J_R_3.BackColor = Color.Red
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_Q)
                        isRedInput = True
                        'Add Red Score
                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.DPad.Up = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_R)
                        isRedInput = True
                        lbl_J_R_3.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.Buttons.LeftShoulder = Input.ButtonState.Pressed) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_S)
                        isRedInput = True
                        lbl_J_R_3.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf CInt((gamePadState.Triggers.Left * 100)) = 100 Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_T)
                        isRedInput = True
                        lbl_J_R_3.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)

                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                Else
                    isRedInput = False
                    lbl_J_R_3.BackColor = Color.Black
                End If

                If (gamePadState.Buttons.A = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_U)
                        isBlueInput = True
                        lbl_J_B_3.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.Y = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_V)
                        isBlueInput = True
                        lbl_J_B_3.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.RightShoulder = Input.ButtonState.Pressed) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_W)
                        isBlueInput = True
                        lbl_J_B_3.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf CInt((gamePadState.Triggers.Right * 100)) = 100 Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_X)
                        isBlueInput = True
                        lbl_J_B_3.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "3"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                Else
                    isBlueInput = False
                    lbl_J_B_3.BackColor = Color.Black
                End If
            Case eJoyType.PS
                If IsNothing(joystickDevice3) Then
                    Exit Sub
                End If
                Try
                    joystickDevice3.Poll()

                    Dim state As JoystickState = joystickDevice3.CurrentJoystickState

                    If state.Y = 65535 Then ' Down Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_Q)
                            isRedInput = True
                            lbl_J_R_3.BackColor = Color.Red
                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If
                    If state.Y = 0 Then 'Up Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_R)
                            isRedInput = True
                            lbl_J_R_3.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If


                    End If
                    If state.GetButtons(6) > 0 Then ' L1 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_S)
                            isRedInput = True
                            lbl_J_R_3.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If
                    If state.GetButtons(4) > 0 Then ' L2 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_T)
                            isRedInput = True
                            lbl_J_R_3.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)
                            GrdRedLast()
                        End If

                    End If

                    If state.GetButtons(2) > 0 Then ' X Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_U)
                            isBlueInput = True
                            lbl_J_B_3.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(0) > 0 Then ' /_\ Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_V)
                            isBlueInput = True
                            lbl_J_B_3.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(7) > 0 Then ' R1 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_W)
                            isBlueInput = True
                            lbl_J_B_3.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If

                    If state.GetButtons(5) > 0 Then ' R2 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_X)
                            isBlueInput = True
                            lbl_J_B_3.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "3"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If

                    'If Not ((state.Y = 65535) OrElse (state.Y = 0) OrElse (state.GetButtons(6) > 0) OrElse (state.GetButtons(4) > 0) _
                    '  OrElse (state.GetButtons(2) > 0) OrElse (state.GetButtons(0) > 0) OrElse (state.GetButtons(7) > 0) _
                    '  OrElse (state.GetButtons(5) > 0)) Then ' R2 Key
                    '    isInput = False
                    'End If

                    If Not ((state.Y = 65535) OrElse (state.Y = 0) OrElse (state.GetButtons(6) > 0) OrElse (state.GetButtons(4) > 0)) Then
                        isRedInput = False
                        lbl_J_R_3.BackColor = Color.Black
                    End If

                    If Not ((state.GetButtons(2) > 0) OrElse (state.GetButtons(0) > 0) OrElse (state.GetButtons(7) > 0) _
                        OrElse (state.GetButtons(5) > 0)) Then ' R2 Key
                        isBlueInput = False
                        lbl_J_B_3.BackColor = Color.Black
                    End If
                Catch ex As Exception

                End Try
        End Select
      
    End Sub
    ''' <summary>
    ''' Get Event From JoyStick 4
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub joystickPolling4()
        Static isRedInput As Boolean = False ' ยังไม่มีการกด
        Static isBlueInput As Boolean = False

        Select Case JoyType
            Case eJoyType.xBox
                '******************* Code For XBox Joy
                Dim payerIndex As PlayerIndex = PlayerIndex.Four
                Dim gamePadState As GamePadState

                VibrationCountDown_joy1 = 1

                gamePadState = GamePad.GetState(payerIndex)

                If (gamePadState.DPad.Down = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    lbl_J_R_4.BackColor = Color.Red
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_1)
                        isRedInput = True
                        'Add Red Score
                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.DPad.Up = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Left.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_2)
                        isRedInput = True
                        lbl_J_R_4.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf (gamePadState.Buttons.LeftShoulder = Input.ButtonState.Pressed) Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_3)
                        isRedInput = True
                        lbl_J_R_4.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)
                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                ElseIf CInt((gamePadState.Triggers.Left * 100)) = 100 Then
                    If Not isRedInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_4)
                        isRedInput = True
                        lbl_J_R_4.BackColor = Color.Red

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtRed.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtRed.Rows.Add(nDr)

                        GrdRedLast()
                        'GamePad.SetVibration(payerIndex, 1, 0)
                    End If
                Else
                    isRedInput = False
                    lbl_J_R_4.BackColor = Color.Black
                End If

                If (gamePadState.Buttons.A = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) < 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_5)
                        isBlueInput = True
                        lbl_J_B_4.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "1"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.Y = Input.ButtonState.Pressed) Or (CInt((gamePadState.ThumbSticks.Right.Y + 1.0F) * 100.0F / 2.0F) > 50) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_6)
                        isBlueInput = True
                        lbl_J_B_4.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "2"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf (gamePadState.Buttons.RightShoulder = Input.ButtonState.Pressed) Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_7)
                        isBlueInput = True
                        lbl_J_B_4.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "3"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)
                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                ElseIf CInt((gamePadState.Triggers.Right * 100)) = 100 Then
                    If Not isBlueInput Then
                        clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_8)
                        isBlueInput = True
                        lbl_J_B_4.BackColor = Color.Blue

                        Dim nDr As DataRow
                        nDr = clsScoreControl.dtBlue.NewRow
                        nDr("JoyNo") = "4"
                        nDr("Score") = "4"
                        nDr("Times") = Format(runTime, "mm:ss")
                        clsScoreControl.dtBlue.Rows.Add(nDr)

                        GrdBlueLast()
                        'GamePad.SetVibration(payerIndex, 0, 1)
                    End If
                Else
                    isBlueInput = False
                    lbl_J_B_4.BackColor = Color.Black
                End If
            Case eJoyType.PS
                If IsNothing(joystickDevice4) Then
                    Exit Sub
                End If
                Try
                    joystickDevice4.Poll()

                    Dim state As JoystickState = joystickDevice4.CurrentJoystickState

                    If state.Y = 65535 Then ' Down Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_1)
                            isRedInput = True
                            lbl_J_R_4.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)

                            GrdRedLast()
                        End If

                    End If

                    If state.Y = 0 Then 'Up Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_2)
                            isRedInput = True
                            lbl_J_R_4.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)

                            GrdRedLast()
                        End If

                    End If

                    If state.GetButtons(6) > 0 Then ' L1 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_3)
                            isRedInput = True
                            lbl_J_R_4.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)

                            GrdRedLast()
                        End If

                    End If
                    If state.GetButtons(4) > 0 Then ' L2 Key
                        If Not isRedInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_4)
                            isRedInput = True
                            lbl_J_R_4.BackColor = Color.Red

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtRed.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtRed.Rows.Add(nDr)

                            GrdRedLast()
                        End If

                    End If
                    If state.GetButtons(2) > 0 Then ' X Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_5)
                            isBlueInput = True
                            lbl_J_B_4.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "1"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If

                    If state.GetButtons(0) > 0 Then ' /_\ Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_6)
                            isBlueInput = True
                            lbl_J_B_4.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "2"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If
                    If state.GetButtons(7) > 0 Then ' R1 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_7)
                            isBlueInput = True
                            lbl_J_B_4.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "3"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If

                    If state.GetButtons(5) > 0 Then ' R2 Key
                        If Not isBlueInput Then
                            clsScoreControl.AddScoreByKeyData(clsScoreControl.KeyData.Alt_8)
                            isBlueInput = True
                            lbl_J_B_4.BackColor = Color.Blue

                            Dim nDr As DataRow
                            nDr = clsScoreControl.dtBlue.NewRow
                            nDr("JoyNo") = "4"
                            nDr("Score") = "4"
                            nDr("Times") = Format(runTime, "mm:ss")
                            clsScoreControl.dtBlue.Rows.Add(nDr)

                            GrdBlueLast()
                        End If

                    End If

                    'If Not ((state.Y = 65535) OrElse (state.Y = 0) OrElse (state.GetButtons(6) > 0) OrElse (state.GetButtons(4) > 0) _
                    '  OrElse (state.GetButtons(2) > 0) OrElse (state.GetButtons(0) > 0) OrElse (state.GetButtons(7) > 0) _
                    '  OrElse (state.GetButtons(5) > 0)) Then ' R2 Key
                    '    isInput = False
                    'End If

                    If Not ((state.Y = 65535) OrElse (state.Y = 0) OrElse (state.GetButtons(6) > 0) OrElse (state.GetButtons(4) > 0)) Then
                        isRedInput = False
                        lbl_J_R_4.BackColor = Color.Black
                    End If

                    If Not ((state.GetButtons(2) > 0) OrElse (state.GetButtons(0) > 0) OrElse (state.GetButtons(7) > 0) _
                        OrElse (state.GetButtons(5) > 0)) Then ' R2 Key
                        isBlueInput = False
                        lbl_J_B_4.BackColor = Color.Black
                    End If
                Catch ex As Exception

                End Try
        End Select
      
    End Sub
#End Region

    Private Function getCPU_ID() As String
        Dim cpuID As String = String.Empty
        Dim mc As ManagementClass = New ManagementClass("Win32_Processor")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        For Each mo As ManagementObject In moc
            If (cpuID = String.Empty) Then
                cpuID = mo.Properties("ProcessorId").Value.ToString()
            End If
        Next
        Return cpuID
    End Function

    Private Function Write_Serial_File(ByVal pSerial As String) As Boolean
        Dim Fs As New System.IO.FileStream(sFileSerial, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.Write)
        Dim Sw As New System.IO.StreamWriter(Fs)
        Try
            'SendHeader()

            Sw.WriteLine(pSerial)

            Sw.Close()
            Fs.Close()
        Catch ex As Exception

        End Try
    End Function

    Private Function Read_Serial_File() As String
        Dim Fs As New System.IO.FileStream(sFileSerial, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim Sw As New System.IO.StreamReader(Fs)
        Dim ret As String = ""
        Try
            'SendHeader()

            ret = Sw.ReadToEnd

            Sw.Close()
            Fs.Close()
        Catch ex As Exception

        End Try
        Return ret
    End Function

    Private Function EncodeData(ByVal Data As String, ByVal Key As String) As String
        Dim ret As String = ""
        Dim encrip As New System.IO.MemoryStream
        Dim RC2 As New RC2CryptoServiceProvider
        RC2.Key = Encoding.ASCII.GetBytes(Key)
        Dim iv() As Byte = {11, 12, 33, 50, 78, 25, 72, 84}
        RC2.IV = iv
        Dim myEn As ICryptoTransform = RC2.CreateEncryptor
        Dim pwd() As Byte = Encoding.ASCII.GetBytes(Data)
        Dim myCry As New CryptoStream(encrip, myEn, CryptoStreamMode.Write)
        myCry.Write(pwd, 0, pwd.Length)
        myCry.Close()
        ret = Convert.ToBase64String(encrip.ToArray())
        Return ret
    End Function
    Private Function DecodeData(ByVal Data As String, ByVal Key As String) As String
        Dim ret As String = ""
        Try
            Dim Decode As New System.IO.MemoryStream
            Dim rc2 As New RC2CryptoServiceProvider
            rc2.Key = Encoding.ASCII.GetBytes(Key)
            Dim iv() As Byte = {11, 12, 33, 50, 78, 25, 72, 84}
            rc2.IV = iv
            Dim Myde As ICryptoTransform = rc2.CreateDecryptor
            Dim enPass() As Byte = Convert.FromBase64String(Data)
            Dim Mycry As New CryptoStream(Decode, Myde, CryptoStreamMode.Write)
            Mycry.Write(enPass, 0, enPass.Length)
            Mycry.Close()
            ret = Encoding.ASCII.GetString(Decode.ToArray())
        Catch ex As Exception
            ret = ""
        End Try
        
        Return ret
    End Function

    Private Function CheckSerial(ByRef EnData As String) As Boolean
        'Check Key
        Dim ret As Boolean = False
        Dim frm As New frmInput_Serial
        EnData = ""
        frm.Label1.Text = getCPU_ID()
        frm.ShowDialog()
        If frm.RetStatus Then
            EnData = EncodeData(getCPU_ID, "bible6666")
            EnData = EncodeData(EnData, "Amagadon99")
            If frm.TextBox1.Text = EnData Then
                ret = True
            End If
        End If


        '----------------------------------
        Return ret
    End Function

#Region " Load Form "
    Private Sub Form2_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim chk As New CheckDLL.CheckData

        If chk.CheckSerial = CheckDLL.CheckData.eCheckSerial.Check Then
            sFileSerial = Application.ExecutablePath
            sFileSerial = sFileSerial.ToUpper.Replace(".EXE", ".SVR")
            If Not System.IO.File.Exists(sFileSerial) Then
                'not have file
                Dim EnData As String = "" ' Input Serial Data
                If CheckSerial(EnData) Then
                    Write_Serial_File(EnData)
                Else
                    Application.Exit()
                End If
            Else
                'Have Data File
                Dim EnData As String = Read_Serial_File()
                If EnData.Trim = "" Then
                    If CheckSerial(EnData) Then
                        Write_Serial_File(EnData)
                    Else
                        Application.Exit()
                    End If
                Else
                    EnData = DecodeData(EnData, "Amagadon99")
                    EnData = DecodeData(EnData, "bible6666")
                    If EnData <> getCPU_ID() Then
                        If CheckSerial(EnData) Then
                            Write_Serial_File(EnData)
                        Else
                            Application.Exit()
                        End If
                    End If
                End If

            End If
        End If
    
        With clsScoreControl.dtRed
            .Columns.Clear()
            .Columns.Add("JoyNo", System.Type.GetType("System.String"))
            .Columns.Add("Score", System.Type.GetType("System.String"))
            .Columns.Add("Times", System.Type.GetType("System.String"))
        End With

        With clsScoreControl.dtBlue
            .Columns.Clear()
            .Columns.Add("JoyNo", System.Type.GetType("System.String"))
            .Columns.Add("Score", System.Type.GetType("System.String"))
            .Columns.Add("Times", System.Type.GetType("System.String"))
        End With

        With grdData_Red
            .AutoGenerateColumns = False
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .ColumnAdd_TextBox("JoyNo", "J", "JoyNo", 20)
            .ColumnAdd_TextBox("Score", "I", "Score", 20)
            .ColumnAdd_TextBox("Time", "เวลา", "Times")
            .DataSource = clsScoreControl.dtRed
        End With

        With grdDataBlue
            .AutoGenerateColumns = False
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .ColumnAdd_TextBox("JoyNo", "J", "JoyNo", 20)
            .ColumnAdd_TextBox("Score", "I", "Score", 20)
            .ColumnAdd_TextBox("Time", "เวลา", "Times")
            .DataSource = clsScoreControl.dtBlue
        End With

        GetJoyStick()

        If MessageBox.Show("ต้องการรันแบบต่อ Data Base [ Yes : ต่อ , No : ไม่ต่อ ]", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
            clsScoreControl.ConnType = clsScoreControl.eDBType.NotConnection
        End If
        lblRed.Text = "0"
        Dim frm As New frmOutSize
        frm.Show()
        runTime = New DateTime(2012, 1, 2, 12, 0, 0)
        runTime = runTime.AddSeconds(clsScoreControl.min)
        KyeShiTime = New DateTime(2012, 1, 2, 12, clsScoreControl.KyeShi, 0)
        clsScoreControl.ClearAllScreen()

        clsScoreControl.SetDisableKyeshi()
        TimeSocket.Start()
        'clsScoreControl.sServer_IP
        Dim sPath As String = Application.StartupPath
        Dim FS As New System.IO.FileStream(sPath & "\config.ini", IO.FileMode.Open)
        Dim FR As New System.IO.StreamReader(FS)
        Dim line As String = ""
        Dim str() As String

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.ServerName = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.DBName = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.Login = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.Pass = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsScoreControl.sServer_IP = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsScoreControl.bSendWin = clsScoreControl.eSendWind.ServerFolder
            clsScoreControl.sSendFolder = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.ServerName2 = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.DBName2 = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.Login2 = str(1).Trim
        End If

        line = FR.ReadLine
        str = line.Split("=")
        If str.Length >= 2 Then
            clsSys.Pass2 = str(1).Trim
        End If

     

        FR.Close()
        FS.Close()
        If clsScoreControl.ConnType = clsScoreControl.eDBType.Connection Then
            

            Dim m_ConnStr1 As String = ""
            m_ConnStr1 = "Server=" & clsSys.ServerName & ";uid=" & clsSys.Login & ";pwd=" & clsSys.Pass & ";Database=" & clsSys.DBName
            'clsSys.conn.setConnString("Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" & sDBFile & ";Uid=;Pwd=;")
            clsSys.conn.setConnString(m_ConnStr1)
            If Not clsSys.conn.openDB() Then
                MessageBox.Show("ไม่สามารถต่อกับฐานข้อมูลได้ โปรดตรวจสอบ config.ini ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End If

            Dim m_ConnStr2 As String = ""
            m_ConnStr2 = "Server=" & clsSys.ServerName2 & ";uid=" & clsSys.Login2 & ";pwd=" & clsSys.Pass2 & ";Database=" & clsSys.DBName2
            clsSys.conn2.setConnString(m_ConnStr2)
            If Not clsSys.conn2.openDB() Then
                MessageBox.Show("ไม่สามารถต่อกับฐานข้อมูลที่ 2 ได้ โปรดตรวจสอบ config.ini ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End If

            Dim sql As String = ""
            Dim dtPosition As New DataTable
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
        Else
            btnSave.Visible = False
        End If

        isFirst = False
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblRed.Text = "0"
        clsScoreControl.SetKeyFor_1()
        clsScoreControl.frmControl = Me
        ShowFightType()
    End Sub

#End Region

    Private Sub ShowFightType()
        Select Case clsScoreControl.FightType
            Case clsScoreControl.eFightType.GapPoint
                lblType.Text = "GapPoint"
            Case clsScoreControl.eFightType.NoLimit
                lblType.Text = "NoLimit"
            Case clsScoreControl.eFightType.GepPointRound2
                lblType.Text = "GapPoint Round 2"
        End Select
    End Sub
#Region " Check Key Input Data "
    Private Sub Form1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp, grdData_Red.KeyUp, grdDataBlue.KeyUp, txtBlueId.KeyUp, txtRedId.KeyUp, cboPositionBlue.KeyUp, cboPositionRed.KeyUp, lblRedName.KeyUp, lblRedTeam.KeyUp, lblBlueName.KeyUp, lblBlueTeam.KeyUp
        'MessageBox.Show(e.KeyData)
        clsScoreControl.AddScoreByKeyData(e.KeyData)
        CheckVibrationTimeout_Joy()
        If e.KeyCode = Keys.Space Then
            Select Case clsScoreControl.GameStatus
                Case clsScoreControl.eGameStatus.Runing
                    clsScoreControl.GameStatus = clsScoreControl.eGameStatus.StopGame


                    TimerStop()
                Case clsScoreControl.eGameStatus.EndRound
                    If clsScoreControl.Round <= 3 Then
                        'รอบที่ 1-3 ปรกติ
                        clsScoreControl.ShowUpdateRound()
                        runTime = New DateTime(2012, 1, 2, 12, 0, 0) ' Set เวลาใหม่
                        runTime = runTime.AddSeconds(clsScoreControl.min)
                        clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Runing
                        clsScoreControl.dtBlue.Rows.Clear()
                        clsScoreControl.dtRed.Rows.Clear()
                        TimerStart()

                    ElseIf clsScoreControl.Round = 4 Then
                        'รอบพิเศษ
                        clsScoreControl.ShowUpdateRound()
                        clsScoreControl.ClearAllScreen() 'Clear ใบเหลือง ใบแดง กับ คะแนนทั้งหมด
                        runTime = New DateTime(2012, 1, 2, 12, 0, 0) ' Set เวลาใหม่
                        runTime = runTime.AddSeconds(clsScoreControl.min)
                        clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Runing
                        clsScoreControl.dtBlue.Rows.Clear()
                        clsScoreControl.dtRed.Rows.Clear()
                        TimerStart()

                    End If

                Case clsScoreControl.eGameStatus.Start
                    runTime = New DateTime(2012, 1, 2, 12, 0, 0) ' Set เวลาใหม่
                    runTime = runTime.AddSeconds(clsScoreControl.min)
                    clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Runing
                    clsScoreControl.dtBlue.Rows.Clear()
                    clsScoreControl.dtRed.Rows.Clear()
                    TimerStart()
                Case clsScoreControl.eGameStatus.EndGame
                Case clsScoreControl.eGameStatus.Kyeshi
                    SetKyeshiDisable() 'ปิด kyeshi
                    clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Runing
                    TimerStart()
                    clsScoreControl.SetDisableKyeshi()
                Case clsScoreControl.eGameStatus.Break
                Case Else
                    clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Runing
                    TimerStart()
            End Select
        ElseIf e.KeyCode = Keys.F1 Then
            clsScoreControl.AddRedScore()
        ElseIf e.KeyCode = Keys.F2 Then
            clsScoreControl.MinusRedScore()
        ElseIf e.KeyCode = Keys.F3 Then
            clsScoreControl.AddRed_Yel_Card()
        ElseIf e.KeyCode = Keys.F4 Then
            clsScoreControl.AddRed_Red_Card()
        ElseIf e.KeyCode = Keys.F5 Then
            clsScoreControl.AddBlueScore()
        ElseIf e.KeyCode = Keys.F6 Then
            clsScoreControl.MinusBlueScore()
        ElseIf e.KeyCode = Keys.F7 Then
            clsScoreControl.AddBlue_Yel_Card()
        ElseIf e.KeyCode = Keys.F8 Then
            clsScoreControl.AddBlue_Red_Card()
        ElseIf e.KeyCode = Keys.F9 Then
            If clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Runing Or clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Kyeshi Or clsScoreControl.GameStatus = clsScoreControl.eGameStatus.StopGame Then
                If KyeShiEnable = False Then
                    SetKyeshiEnable()
                Else
                    SetKyeshiDisable()
                End If
            End If

        End If
    End Sub

    Private Sub SetKyeshiDisable()
        KyeShiEnable = False
        timeKyeShi.Stop()
        KyeShiTime = New DateTime(2012, 1, 2, 12, clsScoreControl.KyeShi, 0)
        clsScoreControl.UpdateKyeShiTime(KyeShiTime)
        clsScoreControl.UpdateKyeShiTime(KyeShiTime)

    End Sub
    Private Sub SetKyeshiEnable()
        'Start KyeShi
        KyeShiEnable = True
        TimerStop()
        timeKyeShi.Start()
        clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Kyeshi
        KyeShiTime = New DateTime(2012, 1, 2, 12, clsScoreControl.KyeShi, 0)
        clsScoreControl.UpdateKyeShiTime(KyeShiTime)
        clsScoreControl.SetEnableKyeshi()
    End Sub
#End Region

#Region " Click Red Control "
    Private Sub btnRed_P_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRed_P.Click
        clsScoreControl.AddRedScore()

    End Sub

    Private Sub btnRed_M_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRed_M.Click
        clsScoreControl.MinusRedScore()

    End Sub

    Private Sub btnRed_K_P_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRed_K_P.Click
        clsScoreControl.AddRed_Yel_Card()
    End Sub

    Private Sub btnRed_K_M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRed_K_M.Click
        clsScoreControl.MinusRed_Yel()
    End Sub

    Private Sub btnRed_G_P_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRed_G_P.Click
        clsScoreControl.AddRed_Red_Card()
    End Sub

    Private Sub btnRed_G_M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRed_G_M.Click
        clsScoreControl.MinusRed_Red_Card()

    End Sub
#End Region

#Region " Click Blue Control "
    Private Sub btnBlue_P_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlue_P.Click
        clsScoreControl.AddBlueScore()
    End Sub

    Private Sub btnBlue_M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlue_M.Click
        clsScoreControl.MinusBlueScore()
    End Sub

    Private Sub btnblue_K_P_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlue_K_P.Click
        clsScoreControl.AddBlue_Yel_Card()
    End Sub

    Private Sub btnBlue_K_M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlue_K_M.Click
        clsScoreControl.MinusBlue_Yel_Card()
    End Sub

    Private Sub btnBlue_G_P_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlue_G_P.Click
        clsScoreControl.AddBlue_Red_Card()
    End Sub

    Private Sub btnBlue_G_M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlue_G_M.Click
        clsScoreControl.MinusBlue_Red_Card()
    End Sub
#End Region

#Region " Time Control "
    Private Sub CheckVibrationTimeout_Joy()
        If (VibrationCountDown_joy1 > 0) Then
            VibrationCountDown_joy1 -= 1
            If VibrationCountDown_joy1 <= 0 Then
                Dim payerIndex As PlayerIndex = PlayerIndex.One
                Dim payerIndex2 As PlayerIndex = PlayerIndex.Two
                Dim payerIndex3 As PlayerIndex = PlayerIndex.Three
                Dim payerIndex4 As PlayerIndex = PlayerIndex.Four

            End If
        End If

    End Sub
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Static i As Integer
        CheckVibrationTimeout_Joy()
        joystickPolling1()


        If i >= 1000 Then
            runTime = runTime.AddSeconds(-1)
            'lblTime.Text = Format(runTime, "mm:ss")
            'lblTime.Refresh()
            clsScoreControl.UpdateTime(runTime)
            If runTime.Minute = 0 And runTime.Second = 0 Then
                clsScoreControl.PlayEndSound(clsScoreControl.iSoundId)
                TimerStop()


                If clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Break Then
                    'clsScoreControl.PlayEndSound(clsScoreControl.iSoundId)
                    clsScoreControl.CheckEndRoundByTime()
                    'If clsScoreControl.GameStatus = clsScoreControl.eGameStatus.EndGame And clsScoreControl.WinPlayer = clsScoreControl.Player.None Then

                    'End If
                    RunMode = RunType.Run
                    System.Threading.Thread.Sleep(1000)
                    runTime = New DateTime(2012, 1, 2, 12, 0, 0)
                    runTime = runTime.AddSeconds(clsScoreControl.min)

                    clsScoreControl.ShowUpdateRound()
                    clsScoreControl.UpdateTime(runTime)
                ElseIf Not clsScoreControl.GameStatus = clsScoreControl.eGameStatus.EndGame Then
                    'clsScoreControl.PlayEndSound(clsScoreControl.iSoundId)
                    clsScoreControl.CheckEndRoundByTime()
                    System.Threading.Thread.Sleep(1000)
                    runTime = New DateTime(2012, 1, 2, 12, 0, 0)
                    runTime = runTime.AddSeconds(clsScoreControl.TimeBreak)
                    clsScoreControl.UpdateTime(runTime)
                    TimerStart()
                End If

            End If
            i = 0

        Else
            i += 100
        End If



        Select Case clsScoreControl.GameStatus
            Case clsScoreControl.eGameStatus.Runing, clsScoreControl.eGameStatus.Break
            Case Else
                TimerStop()
        End Select

    End Sub

    Private Sub timeKyeShi_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timeKyeShi.Tick
        Static i As Integer
        If i >= 1000 Then
            KyeShiTime = KyeShiTime.AddSeconds(-1)
            lblKyeShi.Text = Format(KyeShiTime, "mm:ss")
            clsScoreControl.UpdateKyeShiTime(KyeShiTime)
            i = 0
        Else
            i += 100
        End If

        If KyeShiTime.Minute = 0 And KyeShiTime.Second = 0 Then
            timeKyeShi.Stop()
            KyeShiTime = New DateTime(2012, 1, 2, 12, clsScoreControl.KyeShi, 0)
            clsScoreControl.UpdateKyeShiTime(KyeShiTime)
            TimerStart()
            clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Runing
        End If
    End Sub

    Private Sub TimeSocket_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimeSocket.Tick
        If isUpdateScore Then
            SendScore()
            isUpdateScore = False
        End If
    End Sub

#End Region

    Private Sub btnSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSet.Click
        Using frm As New frmControlSet
            frm.cboField.SelectedValue = clsScoreControl.FieldId
            frm.ShowDialog()
            If frm.okStatus = True Then
                isUpdateScore = True
                If frm.rdbXBox.Checked Then
                    JoyType = eJoyType.xBox
                Else
                    JoyType = eJoyType.PS
                End If
                'Check Send Data To Server
                If frm.rdbSendDb.Checked Then
                    clsScoreControl.bSendWin = clsScoreControl.eSendWind.Database
                    clsScoreControl.sSendFolder = ""
                ElseIf frm.rdbSendFile.Checked Then
                    clsScoreControl.bSendWin = clsScoreControl.eSendWind.ServerFolder
                    clsScoreControl.sSendFolder = frm.txtSendFile.Text.Trim
                End If
                runTime = New DateTime(2012, 1, 2, 12, 0, 0)
                If frm.txtNowTime.Text.Trim <> "" AndAlso IsNumeric(frm.txtNowTime.Text.Trim) Then
                    runTime = runTime.AddSeconds(CDbl(frm.txtNowTime.Text.Trim))
                    clsScoreControl.GameStatus = clsScoreControl.eGameStatus.StopGame
                Else
                    runTime = runTime.AddSeconds(clsScoreControl.min)
                    clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Start
                End If

                clsScoreControl.ClearAllScreen()

                'If frm.txtIP.Text.Trim <> "" Then
                '    'Connectin To Server
                '    IP_Server = frm.txtIP.Text.Trim
                '    clsScoreControl.sServer_IP = frm.txtIP.Text.Trim
                '    Try
                '        clientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                '        Dim ipAddress As IPAddress = ipAddress.Parse(clsScoreControl.sServer_IP) '"192.168.0.80")
                '        Dim ipEndPoint As IPEndPoint = New IPEndPoint(ipAddress, 25)

                '        clientSocket.BeginConnect(ipEndPoint, New AsyncCallback(AddressOf OnConnect), Nothing)

                '        System.Threading.Thread.Sleep(1000)
                '    Catch ex As Exception

                '    End Try
                'End If
                If frm.txtNowRound.Text.Trim <> "" Then
                    clsScoreControl.Round = frm.txtNowRound.Text.Trim

                End If
                If frm.txtNowTime.Text.Trim <> "" Then
                    runTime = New DateTime(2012, 1, 2, 12, 0, 0)
                    runTime = runTime.AddSeconds(frm.txtNowTime.Text.Trim)
                    clsScoreControl.UpdateTime(runTime)
                End If
                clsScoreControl.ShowUpdateRound()
                ShowFightType()
            End If
        End Using
        Try
            If Not IsNothing(clientSocket) AndAlso clientSocket.Connected Then
                SendHeader()
            End If
        Catch ex As Exception

        End Try

    End Sub

#Region " Socket Control "
    Private Sub SendScore()
        Try
            Dim msg As String = ""
            Dim Round As String = "" '
            Dim str() As String = lblRound.Text.Split(":")
            If Str.Length = 2 Then
                Round = Str(1)
            End If
            sHeader = SetSendDataLength("0:" & Round & ":" & lblField.Text.Trim)
            sDetail = SetSendDataLength("1:" & lblRed.Text.Trim & ":" & lblBlue.Text.Trim)
            Send(msg, clientSocket)
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub SendHeader()
        Try
            Dim msg As String = ""
            Dim str() As String = lblRound.Text.Split(":")
            Dim Round As String = "" '
            If str.Length = 2 Then
                Round = str(1)
            End If

            sHeader = SetSendDataLength("0:" & Round & ":" & lblField.Text.Trim)
            'sDetail = SetSendDataLength("1:0:0")
            sDetail = SetSendDataLength("1:" & lblRed.Text.Trim & ":" & lblBlue.Text.Trim)
            Send(msg, clientSocket)
        Catch ex As Exception

        End Try

    End Sub

    Private Function SetSendDataLength(ByVal msg As String) As String
        Dim ret As String = ""
        ret = msg
        For i As Integer = ret.Length To 20
            ret &= " "
        Next
        Return ret
    End Function

    Private Sub Send(ByVal msg As String, ByVal client As Socket)
       
        If clsScoreControl.sServer_IP.Trim = "" Then
            Exit Sub
        End If


        'Dim NewFile As String = GetNewFile(clsScoreControl.sServer_IP)
        Dim NewFile As String = GetNewFile("")
        Dim TargetFile As String = clsScoreControl.sServer_IP.Trim & NewFile
        Dim SortFile As String = Application.StartupPath & "\Data\" & NewFile
        Dim SortOkFile As String = Application.StartupPath & "\Data\OK\" & NewFile
        Dim SortNGFile As String = Application.StartupPath & "\Data\NG\" & NewFile
       
        Dim sta As Boolean = True
        Try

            Dim Fs As New System.IO.FileStream(SortFile, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.Write)
            Dim Sw As New System.IO.StreamWriter(Fs)
            Sw.WriteLine(sHeader.Trim)
            Sw.WriteLine(sDetail.Trim)
            Sw.Close()
            Fs.Close()

            System.IO.File.Copy(SortFile, TargetFile)
        Catch ex As Exception
            'Save File Error
            sta = False
        End Try

        If sta = True Then
            System.IO.File.Move(SortFile, SortOkFile)
        Else
            System.IO.File.Move(SortFile, SortNGFile)
        End If

        
    End Sub

    'Private Sub OnSend(ByVal ar As IAsyncResult)
    '    Dim client As Socket = ar.AsyncState
    '    Dim a As Integer
    '    Try

    '        a = client.EndSend(ar)

    '    Catch ex As Exception

    '    End Try

    'End Sub


    'Private Sub OnConnect(ByVal ar As IAsyncResult)
    '    Try
    '        clientSocket.EndConnect(ar)
    '        clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, _
    '                                  New AsyncCallback(AddressOf OnRecieve), clientSocket)
    '    Catch ex As Exception
    '        MessageBox.Show("Cann't Connection To Server")
    '    End Try

    'End Sub

    'Private Sub OnRecieve(ByVal ar As IAsyncResult)
    '    Try
    '        Dim client As Socket = ar.AsyncState
    '        client.EndReceive(ar)
    '        Dim bytesRec As Byte() = byteData
    '        Msg_From_Server = System.Text.ASCIIEncoding.ASCII.GetString(bytesRec)
    '        Read(Msg_From_Server)
    '        clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, _
    '                                  New AsyncCallback(AddressOf OnRecieve), clientSocket)
    '    Catch ex As Exception

    '    End Try

    'End Sub

    Delegate Sub _Read(ByVal msg As String)

    Private Sub Read(ByVal msg As String)
        If InvokeRequired Then
            Invoke(New _Read(AddressOf Read), msg)
            Exit Sub
        End If

    End Sub

#End Region


    Private Sub lblBlue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblBlue.TextChanged, lblRed.TextChanged
        isUpdateScore = True
        'SendScore()

    End Sub

    Private Sub lblRound_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblRound.TextChanged
        SendHeader()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        clsScoreControl.GameStatus = clsScoreControl.eGameStatus.Start
        clsScoreControl.Round = 1
        clsScoreControl.ShowUpdateRound()

        If clsScoreControl.ConnType = clsScoreControl.eDBType.Connection Then
            Dim dt As New DataTable
            'Update Value
            clsScoreControl.FieldSeq = GetNextRound(clsScoreControl.FieldId, clsScoreControl.FieldSeq) ' System.Math.Round(clsScoreControl.FieldSeq + 0.5, 0, MidpointRounding.AwayFromZero)
            dt = GetAthleteData(clsScoreControl.FieldId, clsScoreControl.FieldSeq)
            If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
                Dim RedId As Integer = -1
                Dim BlueId As Integer = -1
                Dim RedName As String = ""
                Dim BlueName As String = ""
                Dim RedTeam As String = ""
                Dim BlueTeam As String = ""

                If Not IsDBNull(dt.Rows(0)("RedID")) Then
                    RedId = dt.Rows(0)("RedID")
                End If
                If Not IsDBNull(dt.Rows(0)("BlueID")) Then
                    BlueId = dt.Rows(0)("BlueID")
                End If

                If Not IsDBNull(dt.Rows(0)("Red")) Then
                    RedName = dt.Rows(0)("Red")
                End If
                If Not IsDBNull(dt.Rows(0)("Blue")) Then
                    BlueName = dt.Rows(0)("Blue")
                End If

                If Not IsDBNull(dt.Rows(0)("RedTeam")) Then
                    RedTeam = dt.Rows(0)("RedTeam")
                End If
                If Not IsDBNull(dt.Rows(0)("BlueTeam")) Then
                    BlueTeam = dt.Rows(0)("BlueTeam")
                End If

                clsScoreControl.iRedId = RedId
                clsScoreControl.sRedName = RedName
                clsScoreControl.sRedTeam = RedTeam
                clsScoreControl.iBlueId = BlueId
                clsScoreControl.sBlueName = BlueName
                clsScoreControl.sBlueTeam = BlueTeam
                txtRedId.Text = RedId
                txtBlueId.Text = BlueId
            Else
                clsScoreControl.iRedId = -1
                clsScoreControl.sRedName = ""
                clsScoreControl.sRedTeam = ""
                clsScoreControl.iRedId = -1
                clsScoreControl.sBlueName = ""
                clsScoreControl.sBlueTeam = ""
                txtRedId.Text = ""
                txtBlueId.Text = ""
            End If
        Else
            clsScoreControl.iRedId = -1
            clsScoreControl.sRedName = ""
            clsScoreControl.sRedTeam = ""
            clsScoreControl.iRedId = -1
            clsScoreControl.sBlueName = ""
            clsScoreControl.sBlueTeam = ""
            txtRedId.Text = ""
            txtBlueId.Text = ""
            clsScoreControl.FieldSeq = System.Math.Round(clsScoreControl.FieldSeq + 0.5, 0, MidpointRounding.AwayFromZero)
        End If

        'Clear Old Data

        clsScoreControl.WinPlayer = clsScoreControl.Player.None
        clsScoreControl.ClearScore()
        clsScoreControl.ClearAllScreen()
        SendHeader()
    End Sub

    Private Function GetNextRound(ByVal FieldId As Integer, ByVal OldRoundId As Double) As Double
        Dim ret As Double = 0
        Dim sql As String = ""
        Dim dt As New DataTable
        sql = " SELECT * FROM DataLine"
        sql &= " WHERE FieldId = " & FieldId & " AND ShowSeq > " & OldRoundId
        sql &= " ORDER BY ShowSeq"
        clsSys.conn.getData(sql, dt)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            ret = dr("ShowSeq")
        End If
        Return ret
    End Function

    Private Function GetAthleteData(ByVal FieldId As Integer, ByVal ShowSeq As Double) As DataTable
        Dim dt As New DataTable
        Dim sql As String = String.Empty
        sql = " select P1.Prefix_Name + ' ' + A1.Names  + ' ' + A1.Lastname as Blue,A1.AthleteID as BlueID , "
        sql &= "       P2.Prefix_Name + ' ' + A2.Names  + ' ' + A2.Lastname as Red,A2.AthleteID as RedID , "
        sql &= "       A1.Team as BlueTeam , A2.Team as RedTeam "
        sql &= " from DataLine D LEFT JOIN M_Athlete A1 ON D.Athlete_1 = A1.AthleteID"
        sql &= "           LEFT JOIN M_Prefix P1 ON A1.Prefix = P1.Prefix_ID"
        sql &= "           LEFT JOIN M_Athlete A2 ON D.Athlete_2 = A2.AthleteID"
        sql &= "           LEFT JOIN M_Prefix P2 ON A2.Prefix = P2.Prefix_ID"
        sql &= " Where D.FieldId = " & FieldId & " and D.ShowSeq = " & ShowSeq
        clsSys.conn.getData(sql, dt)
        Return dt
    End Function

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim frm As New frmSave
        frm.ShowDialog()
    End Sub

    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        joystickPolling2()
    End Sub

    Private Sub Timer3_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        joystickPolling3()
    End Sub

    Private Sub Timer4_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        joystickPolling4()
    End Sub

    Private Sub grdData_Red_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdData_Red.GotFocus
        Me.Focus()
    End Sub

    Private Sub grdDataBlue_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdDataBlue.GotFocus
        Me.Focus()
    End Sub

    Private Sub btnJoyUpdate_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnJoyUpdate.MouseDown
        Me.btnJoyUpdate.BackColor = Color.Aqua
    End Sub

    Private Sub btnJoyUpdate_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnJoyUpdate.MouseUp
        btnJoyUpdate.BackColor = Color.Black
    End Sub

    Private Sub btnJoyUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJoyUpdate.Click
        joystickDevice1 = Nothing
        joystickDevice2 = Nothing
        joystickDevice3 = Nothing
        joystickDevice4 = Nothing
        GetJoyStick()
    End Sub

    Private Sub Label4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Label4.MouseDown
        Label4.BackColor = Color.Yellow
    End Sub

    Private Sub Label4_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Label4.MouseUp
        Label4.BackColor = Color.DarkOrange
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click
        'If clsScoreControl.GameStatus <> clsScoreControl.eGameStatus.EndGame Then
        '    MessageBox.Show("ยังไม่จบการแข่งขัน")
        '    Exit Sub
        'End If
        Dim cls As New frmSave
        If clsScoreControl.sSendFolder.Trim = "" Then
            MessageBox.Show("กำหนด Path ผิดพลาด กรุณา setup ใหม่")
        Else
            Me.Cursor = Cursors.WaitCursor
            If cls.SaveToFile(clsScoreControl.FieldId, clsScoreControl.FieldSeq, cboPositionRed.SelectedValue, cboPositionBlue.SelectedValue, _
                          3, clsScoreControl.WinPlayer, clsScoreControl.iRedId, clsScoreControl.iBlueId) Then
                MessageBox.Show("ส่งข้อมูลเสร็จแล้ว")
            Else
                MessageBox.Show("การส่งข้อมูลไป Server ผิดพลาด กรุณาลองใหม่อีกครับ")
            End If
            Me.Cursor = Cursors.Arrow
        End If


    End Sub

    Private Sub txtBlueId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBlueId.KeyPress, txtRedId.KeyPress, lblRedName.KeyPress, lblRedTeam.KeyPress, lblBlueName.KeyPress, lblBlueTeam.KeyPress
        If e.KeyChar = " "c Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtBlueId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBlueId.TextChanged
        If isFirst Then Exit Sub
        isLoadDB = True
        lblBlueName.Text = ""
        lblBlueTeam.Text = ""

        clsScoreControl.iBlueId = -1
        clsScoreControl.sBlueName = ""
        clsScoreControl.sBlueTeam = ""

        If IsNumeric(txtBlueId.Text.Trim) Then
            Dim dt As New DataTable
            dt = GetAthleteDataById(CInt(txtBlueId.Text.Trim))
            clsScoreControl.iBlueId = CInt(txtBlueId.Text.Trim)
            If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
                lblBlueName.Text = dt.Rows(0)("AthleteName")
                lblBlueTeam.Text = dt.Rows(0)("Team")

                clsScoreControl.iBlueId = CInt(txtBlueId.Text.Trim)
                clsScoreControl.sBlueName = dt.Rows(0)("AthleteName")
                clsScoreControl.sBlueTeam = dt.Rows(0)("Team")
            End If
        End If
        clsScoreControl.SetName()
        isLoadDB = False
    End Sub

    Private Sub txtRedId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRedId.TextChanged
        If isFirst Then Exit Sub
        isLoadDB = True
        lblRedName.Text = ""
        lblRedTeam.Text = ""

        clsScoreControl.iRedId = -1
        clsScoreControl.sRedName = ""
        clsScoreControl.sRedTeam = ""
        If IsNumeric(txtRedId.Text.Trim) Then
            Dim dt As New DataTable
            dt = GetAthleteDataById(CInt(txtRedId.Text.Trim))
            clsScoreControl.iRedId = CInt(txtRedId.Text.Trim)
            If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
                lblRedName.Text = dt.Rows(0)("AthleteName")
                lblRedTeam.Text = dt.Rows(0)("Team")

                clsScoreControl.iRedId = CInt(txtRedId.Text.Trim)
                clsScoreControl.sRedName = dt.Rows(0)("AthleteName")
                clsScoreControl.sRedTeam = dt.Rows(0)("Team")
            End If
        End If
        clsScoreControl.SetName()
        isLoadDB = False
    End Sub

    Private Function GetAthleteDataById(ByVal pAthleteId As Integer) As DataTable
        Dim dt As New DataTable
        Dim sql As String = String.Empty
        sql = "  SELECT P1.Prefix_Name + ' ' + A1.Names  + ' ' + A1.Lastname as AthleteName,A1.Team as Team"
        sql &= " FROM M_Athlete A1 LEFT JOIN M_Prefix P1 ON A1.Prefix = P1.Prefix_ID"
        sql &= " WHERE A1.AthleteID = " & pAthleteId

        clsSys.conn.getData(sql, dt)
        Return dt
    End Function

    Private Sub lblRedName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblRedName.TextChanged
        If isFirst Then Exit Sub
        If isLoadDB Then Exit Sub


        clsScoreControl.sRedName = lblRedName.Text.Trim

        clsScoreControl.SetOutScreenOnly()
    End Sub

    Private Sub lblRedTeam_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblRedTeam.TextChanged
        If isFirst Then Exit Sub
        If isLoadDB Then Exit Sub

        If clsScoreControl.sRedTeam.Trim <> lblRedTeam.Text.Trim Then
            clsScoreControl.sRedTeam = lblRedTeam.Text.Trim
        End If


        clsScoreControl.SetOutScreenOnly()
    End Sub

    Private Sub lblBlueName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBlueName.TextChanged
        If isFirst Then Exit Sub
        If isLoadDB Then Exit Sub
        clsScoreControl.sBlueName = lblBlueName.Text.Trim

        clsScoreControl.SetOutScreenOnly()
    End Sub

    Private Sub lblBlueTeam_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBlueTeam.TextChanged
        If isFirst Then Exit Sub
        If isLoadDB Then Exit Sub
        clsScoreControl.sBlueTeam = lblBlueTeam.Text.Trim

        clsScoreControl.SetOutScreenOnly()
    End Sub


    Private Sub btnImage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImage1.Click
        Dim frm As New OpenFileDialog
        If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim InfoIcon As New Bitmap(frm.FileName)
            btnImage1.BackgroundImage = InfoIcon
            clsScoreControl.frmShow.lblRedFlag.BackgroundImage = InfoIcon
        End If

    End Sub

    Private Sub btnImage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImage2.Click
        Dim frm As New OpenFileDialog
        If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim InfoIcon As New Bitmap(frm.FileName)
            btnImage2.BackgroundImage = InfoIcon
            clsScoreControl.frmShow.lblBlueFlag.BackgroundImage = InfoIcon
        End If
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Dim sql As String = ""
        If cboPositionBlue.SelectedIndex >= 0 Then
            'Update Postion For Red
            sql = " UPDATE M_Athlete SET [ตำแหน่ง] = " & cboPositionBlue.SelectedValue & " , PrintSta = 0 "
            sql &= " WHERE AthleteID = " & txtBlueId.Text
            clsSys.conn2.runSQL(sql)
            MessageBox.Show("บันทึกสำเร็จ")
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Dim sql As String
        If Me.cboPositionRed.SelectedIndex >= 0 Then
            'Update Postion For Red
            sql = " UPDATE M_Athlete SET [ตำแหน่ง] = " & cboPositionRed.SelectedValue & " , PrintSta = 0 "
            sql &= " WHERE AthleteID = " & txtRedId.Text
            clsSys.conn2.runSQL(sql)
            MessageBox.Show("บันทึกสำเร็จ")
        End If
    End Sub
End Class