Public Class RateLimitsD





    Public Class RateLimitsC



        Public ReadOnly PorCadaSegundos As Integer
        Public ReadOnly Cantidad_PorIP As Integer
        Public ReadOnly Cantidad_Total As Integer
        Public ProximoReseteo As Date



        Private Log_PorIP As New Dic(Of String, Integer)
        Private Log_Total%


        Public Function Autorizar(IP$) As String


            If ProximoReseteo < Date.UtcNow Then
                ProximoReseteo = Date.UtcNow.AddSeconds(PorCadaSegundos)
                Log_Total = 0
                Log_PorIP.Clear()
            End If


            If Cantidad_Total < Log_Total Then
                Return "Hemos desactivado esta opción temporalmente, inténtelo de nuevo más tarde."
            End If


            Dim CantidadAccionesIPActual = 0

            SyncLock Log_PorIP
                Log_PorIP.HacerMagia(IP) += 1
                CantidadAccionesIPActual = Log_PorIP.HacerMagia(IP)
                Log_Total += 1
            End SyncLock

            If Cantidad_PorIP < CantidadAccionesIPActual Then
                Return "Hemos desactivado esta opción temporalmente, inténtelo de nuevo más tarde."
            Else
                Return ""
            End If

        End Function



        Sub New(_CantidadPorIP%, _CantidadTotal%, _PorCadaSegundos%)
            Me.ProximoReseteo = Date.UtcNow.AddSeconds(_PorCadaSegundos)
            Me.PorCadaSegundos = _PorCadaSegundos
            Me.Cantidad_PorIP = _CantidadPorIP
            Me.Cantidad_Total = _CantidadTotal
        End Sub


    End Class








End Class

