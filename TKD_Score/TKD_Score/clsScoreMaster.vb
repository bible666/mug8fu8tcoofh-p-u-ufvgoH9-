Public Class clsScoreMaster
    Private tSTime As DateTime = Now
    Private iKey1 As Integer = 0
    Private iKey2 As Integer = 0
    Private iKey3 As Integer = 0
    Private iKey4 As Integer = 0
    Private iKey5 As Integer = 0



#Region " Alt Key "
    Private iKey_Alt_1 As Integer
    Public Property Key_Alt_1() As Integer
        Get
            Return iKey_Alt_1
        End Get
        Set(ByVal value As Integer)
            iKey_Alt_1 = value
        End Set
    End Property

    Private iKey_Alt_2 As Integer
    Public Property Key_Alt_2() As Integer
        Get
            Return iKey_Alt_2
        End Get
        Set(ByVal value As Integer)
            iKey_Alt_2 = value
        End Set
    End Property

    Private iKey_Alt_3 As Integer
    Public Property Key_Alt_3() As Integer
        Get
            Return iKey_Alt_3
        End Get
        Set(ByVal value As Integer)
            iKey_Alt_3 = value
        End Set
    End Property

    Private iKey_Alt_4 As Integer
    Public Property Key_Alt_4() As Integer
        Get
            Return iKey_Alt_4
        End Get
        Set(ByVal value As Integer)
            iKey_Alt_4 = value
        End Set
    End Property

    Private iKey_Alt_5 As Integer
    Public Property Key_Alt_5() As Integer
        Get
            Return iKey_Alt_5
        End Get
        Set(ByVal value As Integer)
            iKey_Alt_5 = value
        End Set
    End Property
#End Region


#Region " Ctl Key "
    Private iKey_Ctl_1 As Integer
    Public Property Key_Ctl_1() As Integer
        Get
            Return iKey_Ctl_1
        End Get
        Set(ByVal value As Integer)
            iKey_Ctl_1 = value
        End Set
    End Property

    Private iKey_Ctl_2 As Integer
    Public Property Key_Ctl_2() As Integer
        Get
            Return iKey_Ctl_2
        End Get
        Set(ByVal value As Integer)
            iKey_Ctl_2 = value
        End Set
    End Property

    Private iKey_Ctl_3 As Integer
    Public Property Key_Ctl_3() As Integer
        Get
            Return iKey_Ctl_3
        End Get
        Set(ByVal value As Integer)
            iKey_Ctl_3 = value
        End Set
    End Property

    Private iKey_Ctl_4 As Integer
    Public Property Key_Ctl_4() As Integer
        Get
            Return iKey_Ctl_4
        End Get
        Set(ByVal value As Integer)
            iKey_Ctl_4 = value
        End Set
    End Property

    Private iKey_Ctl_5 As Integer
    Public Property Key_Ctl_5() As Integer
        Get
            Return iKey_Ctl_5
        End Get
        Set(ByVal value As Integer)
            iKey_Ctl_5 = value
        End Set
    End Property
#End Region

    Private iJude As Integer
    Public Property Jude() As Integer
        Get
            Return iJude
        End Get
        Set(ByVal value As Integer)
            iJude = value
        End Set
    End Property

    Private iAddValue As Integer
    Public Property AddValue() As Integer
        Get
            Return iAddValue
        End Get
        Set(ByVal value As Integer)
            iAddValue = value
        End Set
    End Property

    Public Function SetKeyData(ByVal iKeyData As Integer) As Integer
        Select Case iKeyData
            Case iKey_Alt_1, iKey_Alt_2, iKey_Alt_3, iKey_Alt_4, iKey_Alt_5
                Return AddScore(iKeyData)
            Case iKey_Ctl_1, iKey_Ctl_2, iKey_Ctl_3, iKey_Ctl_4, iKey_Ctl_5
                Return AddScore(iKeyData)
        End Select
        Return 0
    End Function

    Private Sub set_iKey(ByVal iKeyData As Integer)
        Select Case iKeyData
            Case iKey_Alt_1, iKey_Ctl_1
                iKey1 = 1
            Case iKey_Alt_2, iKey_Ctl_2
                iKey2 = 1
            Case iKey_Alt_3, iKey_Ctl_3
                iKey3 = 1
            Case iKey_Alt_4, iKey_Ctl_4
                iKey4 = 1
            Case iKey_Alt_5, iKey_Ctl_5
                iKey5 = 1
        End Select

    End Sub
    Public Function AddScoreByJoy(ByVal JoyId As Integer, ByVal JoyValue As Integer) As Integer
        If iJude <= 2 Then
            iKey1 = 0
            iKey2 = 0
            iKey3 = 0
            iKey4 = 0
            iKey5 = 0
            Return JoyValue
        End If

        If (iKey1 + iKey2 + iKey3 + iKey4 + iKey5) = 0 Then
            tSTime = Now

            Select Case JoyId
                Case 1
                    iKey1 = 1
                Case 2
                    iKey2 = 1
                Case 3
                    iKey3 = 1
                Case 4
                    iKey4 = 1
                Case 5
                    iKey5 = 1
            End Select
        Else
            Select Case JoyId
                Case 1
                    iKey1 = 1
                Case 2
                    iKey2 = 1
                Case 3
                    iKey3 = 1
                Case 4
                    iKey4 = 1
                Case 5
                    iKey5 = 1
            End Select
            Dim eDate As DateTime = Now

            If Format(eDate, "HHmmssfff") - Format(tSTime, "HHmmssfff") < 1000 Then
                If (iKey1 + iKey2 + iKey3 + iKey4 + iKey5) >= (iJude - 1) Then
                    iKey1 = 0
                    iKey2 = 0
                    iKey3 = 0
                    iKey4 = 0
                    iKey5 = 0
                    Return JoyValue
                End If
            Else
                'more 5
                tSTime = Now
                iKey1 = 0
                iKey2 = 0
                iKey3 = 0
                iKey4 = 0
                iKey5 = 0
                Select Case JoyId
                    Case 1
                        iKey1 = 1
                    Case 2
                        iKey2 = 1
                    Case 3
                        iKey3 = 1
                    Case 4
                        iKey4 = 1
                    Case 5
                        iKey5 = 1
                End Select
                Return 0
            End If
        End If


        Return 0
    End Function
    Private Function AddScore(ByVal iKeyData As Integer) As Integer
        If iJude <= 2 Then
            iKey1 = 0
            iKey2 = 0
            iKey3 = 0
            iKey4 = 0
            iKey5 = 0
            Return iAddValue
        End If

        If (iKey1 + iKey2 + iKey3 + iKey4 + iKey5) = 0 Then
            tSTime = Now
            set_iKey(iKeyData)
        Else
            set_iKey(iKeyData)
            Dim eDate As DateTime = Now
            'Debug.WriteLine(Format(eDate, "HHmmssfff") - Format(tSTime, "HHmmssfff"))

            If Format(eDate, "HHmmssfff") - Format(tSTime, "HHmmssfff") < 1000 Then
                If (iKey1 + iKey2 + iKey3 + iKey4 + iKey5) >= (iJude - 1) Then
                    iKey1 = 0
                    iKey2 = 0
                    iKey3 = 0
                    iKey4 = 0
                    iKey5 = 0
                    Return iAddValue
                End If
            Else
                'more 5
                tSTime = Now
                iKey1 = 0
                iKey2 = 0
                iKey3 = 0
                iKey4 = 0
                iKey5 = 0
                set_iKey(iKeyData)
                Return 0
            End If
        End If


        Return 0
    End Function
End Class
