Partial Public Class HerramientasD
    Public Class LimpiezaDeBackupsD


        '# Los que se hacen en día actual no se tocan
        '@ CuantosPorAño = Cantidad por año excluido el último año
        '@ CuantosPorMes = Cantidad por año excluido el último mes
        '@ CuantosPorSemana = Cantidad por año excluido la última semana
        '@ CuantosPorDia = Cantidad por año excluido ayer y las actuales que se van haciendo hoy
        '@ CuantosAyer% = Cantidad que mantener ayer (dia completo)
        Private Shared Futuros As New Dictionary(Of String, DateTime)
        Private Shared Bloqueador As New Object



        Public Shared Function RecibirFuturos() As Dic(Of String, DateTime)
            Dim FuturosTemp As New Dic(Of String, DateTime)
            SyncLock Futuros
                For Each F In Futuros
                    FuturosTemp.Add(F.Key, F.Value)
                Next
            End SyncLock
            Return FuturosTemp
        End Function



        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Public Shared Sub LimpiarBackups(Ruta$, Optional CuantosAyer% = 6, Optional CuantosPorDia_SinContarAyerYHoy% = 2, Optional CuantosPorSemana_SinContarLaUltimaSemana% = 7, Optional CuantosPorMes_SinContarElUltimoMes% = 4, Optional CuantosPorAño_SinContarElUltimoAño% = 12) ', UTCNowParaTest As DateTime)



            '@ Valores por defecto:
            '@ 1 por mes después de un año y "eternamente"
            '@ 1 por semana después de un mes y durante 1 año
            '@ 1 por día después de una semana y durante 1 mes
            '@ 2 por día sin contar ayer y hoy
            '@ 6 ayer
            '@ todos los que se hagan hoy

            SyncLock Bloqueador



                Dim UtcNow = DateTime.UtcNow




                UtcNow = UtcNow.AddDays(-1)



                UtcNow = New DateTime(UtcNow.Year, UtcNow.Month, UtcNow.Day, 23, 59, 59)



                Dim Años As New Dictionary(Of String, DateTime)
                Dim Meses As New Dictionary(Of String, DateTime)
                Dim Semanas As New Dictionary(Of String, DateTime)
                Dim Dias As New Dictionary(Of String, DateTime)
                Dim _24H As New Dictionary(Of String, DateTime)
                Dim DiaActual As New Dictionary(Of String, DateTime)



                SyncLock Futuros
                    Futuros.Clear()
                End SyncLock




                For Each Archivo In IO.Directory.GetFiles(Ruta)
                    If Archivo.EndsWith(".dback") = False Then Continue For

                    Dim Fecha = IO.File.GetLastWriteTimeUtc(Archivo)



                    If Fecha < UtcNow.AddDays(-336) Then
                        Años.Add(Archivo, Fecha)
                    ElseIf Fecha < UtcNow.AddDays(-28) Then
                        Meses.Add(Archivo, Fecha)
                    ElseIf Fecha < UtcNow.AddDays(-7) Then
                        Semanas.Add(Archivo, Fecha)
                    ElseIf Fecha < UtcNow.AddDays(-1) Then
                        Dias.Add(Archivo, Fecha)
                    ElseIf Fecha < UtcNow Then
                        _24H.Add(Archivo, Fecha)
                    ElseIf Fecha < UtcNow.AddDays(+1) Then
                        DiaActual.Add(Archivo, Fecha)
                    Else
                        SyncLock Futuros
                            Futuros.Add(Archivo, Fecha)
                        End SyncLock
                    End If


                Next



                For Actual = -336 * 2 To -336 * 30 Step -336



                    Dim RangoActual As New Dictionary(Of String, DateTime)
                    For Each Elem In Años
                        If Elem.Value > UtcNow.AddDays(Actual) Then
                            RangoActual.Add(Elem.Key, Elem.Value)
                        End If
                    Next
                    For Each Elem In RangoActual
                        Años.Remove(Elem.Key)
                    Next
                    EliminarExcedente(RangoActual, UtcNow.AddDays(Actual), UtcNow.AddDays(Actual + 336), CuantosPorAño_SinContarElUltimoAño%)



                Next



                For Actual = -28 * 2 To -28 * 12 Step -28



                    Dim RangoActual As New Dictionary(Of String, DateTime)
                    For Each Elem In Meses
                        If Elem.Value > UtcNow.AddDays(Actual) Then
                            RangoActual.Add(Elem.Key, Elem.Value)
                        End If
                    Next
                    For Each Elem In RangoActual
                        Meses.Remove(Elem.Key)
                    Next
                    EliminarExcedente(RangoActual, UtcNow.AddDays(Actual), UtcNow.AddDays(Actual + 28), CuantosPorMes_SinContarElUltimoMes%)



                Next



                For Actual = -7 * 2 To -28 Step -7



                    Dim RangoActual As New Dictionary(Of String, DateTime)
                    For Each Elem In Semanas
                        If Elem.Value > UtcNow.AddDays(Actual) Then
                            RangoActual.Add(Elem.Key, Elem.Value)
                        End If
                    Next
                    For Each Elem In RangoActual
                        Semanas.Remove(Elem.Key)
                    Next
                    EliminarExcedente(RangoActual, UtcNow.AddDays(Actual), UtcNow.AddDays(Actual + 7), CuantosPorSemana_SinContarLaUltimaSemana%)



                Next



                For Actual = -1 * 2 To -7 Step -1



                    Dim RangoActual As New Dictionary(Of String, DateTime)
                    For Each Elem In Dias
                        If Elem.Value > UtcNow.AddDays(Actual) Then
                            RangoActual.Add(Elem.Key, Elem.Value)
                        End If
                    Next
                    For Each Elem In RangoActual
                        Dias.Remove(Elem.Key)
                    Next
                    EliminarExcedente(RangoActual, UtcNow.AddDays(Actual), UtcNow.AddDays(Actual + 1), CuantosPorDia_SinContarAyerYHoy%)



                Next



                If True Then



                    EliminarExcedente(_24H, UtcNow.AddDays(-1), UtcNow, CuantosAyer%)



                End If



            End SyncLock



        End Sub



        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Private Shared Sub EliminarExcedente(Archivos3 As Dictionary(Of String, Date), Inicio As Date, Fin As Date, Cantidad%)



            Dim Archivos As New Dictionary(Of String, DateTime)
            Dim Archivos2 = Archivos3.OrderByDescending(Function(e) e.Value)



            For Each Archivo In Archivos2
                Archivos.Add(Archivo.Key, Archivo.Value)
            Next




            If Archivos.Count > Cantidad Then



                '@ Divido el rango en cuantos deben haber, e intento que haya uno por cada porción para que sea homogéneo
                '@ Voy de porciones antiguas a nuevas, para preservar las nuevas ante las antiguas inexistentes en su porción
                '@ la idea es que si por ejemplo son 12 en 24 horas, se quede 1 copia cada 2 horas, pero priorizando las recientes si hay horas que no tienen o tienen demasiadas.



                Dim CuantosSobran% = Archivos.Count - Cantidad
                Dim Minutos = (Fin - Inicio).TotalMinutes
                Dim MinutosPorcion = Minutos / Cantidad




                For Actual = 1 To Cantidad



                    Dim Eliminas As New Dictionary(Of String, DateTime)
                    Dim Encontrado As Boolean = False
                    For Each Archivo In Archivos
                        If Archivo.Value < Inicio.AddMinutes(MinutosPorcion * Actual) Then



                            If Encontrado = False Then
                                Encontrado = True
                                Eliminas.Add(Archivo.Key, Archivo.Value)
                                'File.Move(Archivo.Key, Archivo.Key & "_" & Cantidad)
                            Else
                                If CuantosSobran > 0 Then
                                    Kill(Archivo.Key)
                                    Eliminas.Add(Archivo.Key, Archivo.Value)
                                    CuantosSobran -= 1
                                Else
                                    Exit Sub
                                End If



                            End If



                        End If
                    Next



                    For Each Archivo In Eliminas
                        Archivos.Remove(Archivo.Key)
                    Next



                Next



            End If





        End Sub


    End Class
End Class
