Imports System.Media

Public Class clsPlaySound
    Private wavPlayer As SoundPlayer = New SoundPlayer()
    Private Path_Sound As String = Application.StartupPath

    Public Sub Play_Sound_RoundEnd()
        wavPlayer.SoundLocation = Path_Sound & "\Sound\roundend.wav"
        AddHandler wavPlayer.LoadCompleted, AddressOf wavPlayer_LoadCompleted
        wavPlayer.LoadAsync()

    End Sub

    Public Sub Play_Sound_RoundEndNum(ByVal i As Integer)
        wavPlayer.SoundLocation = Path_Sound & "\Sound\roundend_" & i & ".wav"
        AddHandler wavPlayer.LoadCompleted, AddressOf wavPlayer_LoadCompleted
        wavPlayer.LoadAsync()
    End Sub

    Public Sub Play_Sound_Alert_RoundEndNum(ByVal i As Integer)
        wavPlayer.SoundLocation = Path_Sound & "\Sound\alert_" & i & ".wav"
        AddHandler wavPlayer.LoadCompleted, AddressOf wavPlayer_LoadCompleted
        wavPlayer.LoadAsync()
    End Sub

    Public Sub Play_Sound_Plus4()
        wavPlayer.SoundLocation = Path_Sound & "\Sound\plus_4.wav"
        AddHandler wavPlayer.LoadCompleted, AddressOf wavPlayer_LoadCompleted
        wavPlayer.LoadAsync()
    End Sub

    Private Sub wavPlayer_LoadCompleted(ByVal sender As Object, ByVal e As System.EventArgs)
        CType(sender, System.Media.SoundPlayer).Play()

    End Sub
End Class
