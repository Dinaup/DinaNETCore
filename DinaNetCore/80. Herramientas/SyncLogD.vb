Public Class SyncLogD







    Public Shared Function IniciarSincronizacion(EtiquetaDatosSincronizar$) As SyncLogC


        Dim R As New SyncLogC
        R.Inicio = Date.UtcNow
        R.Etiqueta = EtiquetaDatosSincronizar



        Return R
    End Function






    Public Class SyncLogC


        Public Inicio As Date
        Public Etiqueta As String
        Public TotalResultados As Integer
        Public TotalProcesados As Integer

        Public Logs As New List(Of String)



        Public Sub CargarResumen(_TotalResultados%)
            Me.TotalResultados = _TotalResultados
            Logs.Add("------   Iniciando sincronización de '" & Etiqueta & "' (" & TotalResultados.ToString & " registros)   ------")
            Console.WriteLine(Logs(Logs.Count - 1))
            u_log_sw.Restart()
        End Sub




        Dim u_log_sw As New Stopwatch

        Public Sub FIN()

            Console.WriteLine("")
            Logs.Add("------   Fin Recambios   ------")
            Console.WriteLine(Logs.Last)
            Console.WriteLine("")
            Console.WriteLine("")
            Console.WriteLine("")


        End Sub
        Public Sub ReportarProcesados(_CantidadProcesados%)


            Me.TotalProcesados += _CantidadProcesados

            Dim R = ""

            Dim SW_TiempoSincronizacionMS = u_log_sw.ElapsedMilliseconds
            If _CantidadProcesados > 0 Then
                Dim itssec = (1000 / (SW_TiempoSincronizacionMS / _CantidadProcesados)).Redondear(2)
                R = "       +" & _CantidadProcesados.ToString & " | " & (Me.TotalProcesados / Me.TotalResultados * 100).Redondear(2) & "% | " & Me.TotalProcesados.ToString & " de " & Me.TotalResultados.ToString & " | " & itssec.ToString & " / its "
            Else
                R = "       " & Me.TotalProcesados.ToString & " de " & Me.TotalResultados
            End If




            u_log_sw.Restart()



            Logs.Add(R)
            Console.WriteLine(Logs(Logs.Count - 1))


        End Sub




        Sub New()
        End Sub
    End Class





End Class

