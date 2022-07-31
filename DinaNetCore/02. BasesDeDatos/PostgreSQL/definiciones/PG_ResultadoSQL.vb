

Partial Public Class BasesDeDatoD
    Partial Public Class PostgreSQLD

        Public Class PG_ResultadoSQL_BloquesC

            Public Conexion As PG_ServidorC

            Public Lectura_SQL_Contar$
            Public Lectura_SQL_Datos As String
            Public Lectura_TResultadosPorBloque%
            Public Lectura_TotalResultados%
            Public Lectura_PosicionInicial%
            Public Lectura_PosicionActual%


            Public Reporte_FInicio As Date


            Public ReadOnly Property Estadisticas_TotalProcesados As Integer
                Get
                    Return Lectura_PosicionActual + If(Bloques_BloqueLeyendo_Actualmente IsNot Nothing, Bloques_BloqueLeyendo_Actualmente.FilaActual, 0) - Lectura_PosicionInicial
                End Get
            End Property
            Public ReadOnly Property Estadisticas_Posicion As Integer
                Get
                    Return Lectura_PosicionActual + If(Bloques_BloqueLeyendo_Actualmente IsNot Nothing, Bloques_BloqueLeyendo_Actualmente.FilaActual, 0)
                End Get
            End Property

            Public ReadOnly Property Estadisticas_Porcentaje As Decimal
                Get
                    Return ((Estadisticas_Posicion / Lectura_TotalResultados%) * 100).Redondear(2)
                End Get
            End Property





            Public ReadOnly Property Estadisticas_Velocidad As String
                Get
                    If Reporte_FInicio = Date.MinValue Then Return ""
                    Dim Diferencia = (Date.UtcNow - Reporte_FInicio).TotalSeconds
                    Return (Estadisticas_TotalProcesados / Diferencia).Redondear(2) & " i/s"
                End Get
            End Property











#Region "Leer"

            Public Function UltimoElemento() As Boolean
                If Bloques_Cola_PendientesDeConsultar.Count = 0 Then
                    If Bloques_BloqueLeyendo_Actualmente IsNot Nothing Then
                        Return Bloques_BloqueLeyendo_Actualmente.UltimoElemento
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
            End Function
            Public Function LeerFila() As Boolean
                If Bloques_Cola_PendientesDeConsultar Is Nothing Then
                    Bloques_Iniciar()
                End If
                If Bloques_BloqueLeyendo_Actualmente IsNot Nothing AndAlso Bloques_BloqueLeyendo_Actualmente.LeerFila Then
                    Lectura_PosicionActual += 1
                    Return True
                ElseIf Bloques_MoverAlSiguiente() = False Then
                    Return False
                Else
                    Return Me.LeerFila
                End If
            End Function


            Default Public Property Item(ByVal Columna$) As String
                Get
                    Return Bloques_BloqueLeyendo_Actualmente(Columna)
                End Get
                Set(Value As String)
                    Bloques_BloqueLeyendo_Actualmente(Columna) = Value
                End Set
            End Property


            Public Function ToDic() As Dic(Of String, String)
                Dim R As New Dic(Of String, String)
                For Each Actual In Bloques_BloqueLeyendo_Actualmente.Columnas
                    R.AddSiNoExiste(Actual.Key, Me(Actual.Key))
                Next
                Return R
            End Function






            Dim u_log_fecha As Date
            Dim u_log_Procesados As Integer
            Dim u_log_sw As New Stopwatch
            Public Function Log() As String

                Dim R = ""
                If u_log_fecha = Date.MinValue Then
                    R = Me.Estadisticas_TotalProcesados.ToString & " de " & Me.Lectura_TotalResultados
                Else

                    Dim Dif = u_log_sw.ElapsedMilliseconds
                    Dim Dif_count = Me.Lectura_PosicionActual - u_log_Procesados
                    If Dif_count > 0 Then


                        Dim itssec = (Dif / Dif_count * 1000).Redondear(2)


                        R = "+" & Dif_count.ToString & " | " & Me.Estadisticas_Porcentaje & "% | " & Me.Estadisticas_TotalProcesados.ToString & " de " & Me.Lectura_TotalResultados & " | " & itssec.ToString & " / its "

                    Else
                        R = Me.Estadisticas_TotalProcesados.ToString & " de " & Me.Lectura_TotalResultados
                    End If

                End If




                u_log_fecha = Date.UtcNow
                u_log_Procesados = Me.Lectura_PosicionActual
                u_log_sw.Restart()

                Return R
            End Function





#End Region



#Region "Sistema de bloques"
#End Region


            '! Sistema de bloques
            ', Para tablas con muchos datos, voy haciendo selects de tamaño moderado
            ', Bloque 1:  limit 0 a 1000
            ', Bloque 2:  limit 1001 a 2000
            ', Bloque 3:  limit 2001 a 3000
            ', Bloque 4:  ...
            ', Cada bloque entra en un Task como lambda y lo "Consulto" mientras se van recorriendo.



            Private Bloques_Cola_PendientesDeConsultar As Queue(Of Task(Of PG_ResultadoSQLC))
            Private Bloques_BloqueLeyendo_Actualmente As PG_ResultadoSQLC

            Private Function Bloques_MoverAlSiguiente() As Boolean

                '! Obtengo Bloque a leer
                Dim SiguienteBloqueALeer = If(Bloques_Cola_PendientesDeConsultar.TieneDatos, Bloques_Cola_PendientesDeConsultar.Dequeue, Nothing)
                If SiguienteBloqueALeer IsNot Nothing Then
                    Bloques_BloqueLeyendo_Actualmente = SiguienteBloqueALeer.Result   ' esto hace .Wait()
                Else
                    Return False
                End If

                '! Obtengo Bloque a Consultar 
                If Bloques_Cola_PendientesDeConsultar.TieneDatos Then
                    Bloques_Cola_PendientesDeConsultar.Peek().Start()
                End If

                Return True
            End Function

            Private Sub Bloques_Iniciar()

                Bloques_Cola_PendientesDeConsultar = New Queue(Of Task(Of PG_ResultadoSQLC))

                For i = Lectura_PosicionActual To Lectura_TotalResultados Step Lectura_TResultadosPorBloque
                    Dim ElSQL = Lectura_SQL_Datos & " limit " & Lectura_TResultadosPorBloque & " offset " & i.ToString
                    Bloques_Cola_PendientesDeConsultar.Enqueue(New Task(Of PG_ResultadoSQLC)(Function() Conexion.ConsultaSQLPostgreSQL(ElSQL, True)))
                Next

                If Bloques_Cola_PendientesDeConsultar IsNot Nothing AndAlso Bloques_Cola_PendientesDeConsultar.Count > 0 Then
                    Bloques_Cola_PendientesDeConsultar.Peek().Start()
                End If

            End Sub


#Region "Constructor"
#End Region










            Sub New()
            End Sub

        End Class



        Public Class PG_ResultadoSQLC



                Public SQL$
                Public ErrorSQL$
                Public ErrorNumero%
                Public TotalResultados As Integer
                Public TotalColumnas As Integer
                Public Columnas As Dictionary(Of String, PG_ColumnaC)
                Public ColumnasID As Dictionary(Of Integer, PG_ColumnaC)
                Public FilaActual% = 0
                Public LaFilaArray_PSN$()



                Public ResultadosPorPagina% = -1
                Public PaginaActual% = -1
                Public TotalPaginas% = -1

                Public TotalResultadosIncluidosDistintasPaginas% = -1

#Region "Lector"

                Public Sub Reset()
                    FilaActual = 0
                End Sub





            Public Function Porcentaje_Base100() As Integer
                    If FilaActual = 0 Then Return 0
                    If TotalResultados = 0 Then Return 0
                    Return CInt(FilaActual / TotalResultados * 100)
                End Function
                Public Function LeerFila() As Boolean
                    If FilaActual < TotalResultados Then
                        FilaActual += 1
                        Return True
                    Else
                        Return False
                    End If
                End Function
                Public Function UltimoElemento() As Boolean
                    If FilaActual = TotalResultados Then
                        Return True
                    Else
                        Return False
                    End If
                End Function

#End Region



                Default Public Property Item(ByVal Columna$) As String
                    Get

                        If Columnas.ContainsKey(Columna) Then
                            If FilaActual = 0 Then LeerFila()
                            Return RecibirValorFila(FilaActual - 1, Columnas(Columna).ID - 1)
                        Else
                            Dim Cols$ = ""
                            For Each Col In Columnas.Values
                                Cols &= " - [" & Col.Nombre & "]" & vbCrLf
                            Next
                            Throw New Exception(vbCrLf & "No existe la columna: [" & Columna & "]" & vbCrLf & vbCrLf & "Columnas:" & vbCrLf & Cols & vbCrLf & "SQL: " & SQL)
                        End If

                    End Get
                    Set(Value As String)
                        RecibirValorFila(FilaActual - 1, Columnas(Columna).ID - 1) = Value
                    End Set
                End Property

                Default Public ReadOnly Property Item(ByVal Columna%) As String
                    Get
                        Return RecibirValorFila(FilaActual - 1, Columna - 1)
                    End Get
                End Property

                Public Property RecibirValorFila(Lafila%, LaCol%) As String
                    Get
                        If Lafila = 0 Then
                            Return LaFilaArray_PSN(LaCol)
                        Else
                            Return LaFilaArray_PSN((TotalColumnas * Lafila) + LaCol)
                        End If
                    End Get
                    Set(value As String)
                        If Lafila = 0 Then
                            LaFilaArray_PSN(LaCol) = value
                        Else
                            LaFilaArray_PSN((TotalColumnas * Lafila) + LaCol) = value
                        End If
                    End Set
                End Property

                Public Function ExisteColumna(ByVal Columna$) As Boolean
                    If Columnas.ContainsKey(Columna) Then Return True Else Return False
                End Function

                Public Function RecibirDatoPorColuman(Col As String) As String
                    Return Me(Col)
                End Function

            End Class

        End Class
    End Class
