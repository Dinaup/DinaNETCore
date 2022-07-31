Imports Microsoft.VisualBasic
Imports DinaNETCore
Imports DinaNETCore.APID
Imports System
Imports System.Timers
Imports System.Web






Public Class ASP_NETD
    Public Class PaginaD



        Public Shared SistemaIniciado As Boolean
        Public Shared WithEvents Sistema_Timer As New System.Timers.Timer
        Public Shared DinaupServer As DinaNETCore.APID.ServidorDinaup_ConectorC





        Public Shared Sub Iniciar(_Configuracion_APIKey$, _Configuracion_ServidorIPODominion$, _Configuracion_Puerto$)
            DinaupServer = New ServidorDinaup_ConectorC
            DinaupServer.Iniciar(_Configuracion_APIKey$, _Configuracion_ServidorIPODominion$, _Configuracion_Puerto$)
            Iniciar()
        End Sub

        Public Shared Sub Iniciar()
            If SistemaIniciado Then Exit Sub
            SyncLock Sistema_Timer
                If SistemaIniciado Then Exit Sub
                SistemaIniciado = True
                Sistema_Timer.Interval = 2000
                Sistema_Timer.Enabled = True
                Sistema_Timer.AutoReset = True
            End SyncLock
        End Sub

        Private Shared Sub Timer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles Sistema_Timer.Elapsed
            On Error Resume Next
            Sistema_Timer.Enabled = False
            RecalcularConexionAPI()
            Sistema_Timer.Enabled = True
        End Sub






        Public Shared Sub RecalcularConexionAPI()

            If DinaupServer Is Nothing Then Exit Sub


            Dim SegundosDesdeUltimaConfirmacion = Date.UtcNow - DinaupServer.ConexionDisponible_FechaConfirmacion


            If DinaupServer.ConexionDisponible Then
                '! Si la conexión  está disponible la compruebo cada 30 segundos
                If SegundosDesdeUltimaConfirmacion.TotalSeconds < 30 Then
                    Exit Sub
                End If
            Else
                '! Si la conexión no está disponible la compruebo cada 3 segundos
                If SegundosDesdeUltimaConfirmacion.TotalSeconds < 3 Then
                    Exit Sub
                End If
            End If


            DinaupServer.PING()

        End Sub



    End Class
End Class
