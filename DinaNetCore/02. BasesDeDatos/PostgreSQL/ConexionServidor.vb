Option Strict Off
Imports System.Data
Imports Npgsql


Partial Public Class BasesDeDatoD


    Partial Public Class PostgreSQLD

        Public Class PG_ServidorC




            Public Sub Dispose()

                If InstanciaConexion IsNot Nothing Then
                    InstanciaConexion.Dispose()
                    InstanciaConexion = Nothing
                End If

            End Sub



            Public Function Duplicarcionexion() As PG_ServidorC
                Dim R As New PG_ServidorC
                R.TablasYCampos = Me.TablasYCampos
                R.Conectar(Me.PG_Servidor, Me.PG_Puerto, Me.PG_Usuario, Me.PG_Pass, Me.PG_BaseDeDatos, Me.AutoAsignar_Fecha, Me.AutoAsignar_FechaM)
                Return R
            End Function





            Public PG_Servidor$
            Public PG_Puerto$
            Public PG_Usuario$
            Public PG_Pass$
            Public PG_BaseDeDatos$

            Public TablasYCampos As New Dic(Of String, List(Of String))
            Public AutoAsignar_Fecha As Boolean
            Public AutoAsignar_FechaM As Boolean

            Public InstanciaConexion As PG_ConexionC
            Public LockX As New Object
            Private UltimoUso As Date
            Private Iniciado As Boolean
            Public UnError$ = ""



            Public ReadOnly Property Conectado As Boolean
                Get
                    Dim sref = InstanciaConexion
                    Return sref IsNot Nothing AndAlso sref.PostgreSQL.State = Data.ConnectionState.Open
                End Get
            End Property






            Public Sub Conectar(DBip$, DBpuerto%, DBusu$, DBpass$, dbname$, Optional _AutoAsignar_Fecha As Boolean = False, Optional _AutoAsignar_FechaM As Boolean = False)
                EstablecerCultureAlThreadActual()
                Iniciar()
                Conectarx(DBip, DBpuerto, DBusu, DBpass, dbname, _AutoAsignar_Fecha, _AutoAsignar_FechaM)
            End Sub


            Private Sub Conectarx(DBip$, DBpuerto%, DBusu$, DBpass$, dbname$, Optional _AutoAsignar_Fecha As Boolean = False, Optional _AutoAsignar_FechaM As Boolean = False)


                If PG_Servidor <> DBip Then Iniciado = False
                If PG_Puerto <> DBpuerto Then Iniciado = False
                If PG_Usuario <> DBusu Then Iniciado = False
                If PG_Pass <> DBpass Then Iniciado = False
                If PG_BaseDeDatos <> dbname Then Iniciado = False
                If AutoAsignar_Fecha <> _AutoAsignar_Fecha Then Iniciado = False
                If AutoAsignar_FechaM <> _AutoAsignar_FechaM Then Iniciado = False


                PG_Servidor = DBip
                PG_Puerto = DBpuerto
                PG_Usuario = DBusu
                PG_Pass = DBpass
                PG_BaseDeDatos = dbname
                AutoAsignar_Fecha = _AutoAsignar_Fecha
                AutoAsignar_FechaM = _AutoAsignar_FechaM



                If Iniciado = False Then
                    SyncLock LockX
                        If Iniciado = False Then

                            Try

                                Dim PostgreSQL = New Npgsql.NpgsqlConnection("Server=" & DBip & ";Port=" & DBpuerto & ";Userid=" & DBusu & ";password=" & DBpass & ";Pooling=true;MinPoolSize=1;SslMode=Disable;Database=" & dbname & ";MaxPoolSize=1000;")
                                PostgreSQL.Open()
                                Dim CreandoInstanciaConexion = New PG_ConexionC(Me.PG_Pass) With {.PostgreSQL = PostgreSQL}
                                SyncLock CreandoInstanciaConexion
                                    InstanciaConexion = CreandoInstanciaConexion
                                    UltimoUso = Date.UtcNow
                                    EjecutarSQL("SET standard_conforming_strings = off;")
                                    ActualizarCAcheDecolumnasYTablas()
                                    UnError = ""
                                    Iniciado = True
                                End SyncLock


                            Catch ef As Npgsql.PostgresException

                                If ef.Code = "3D000" Then

                                    Try
                                        Dim PostgreSQL = New Npgsql.NpgsqlConnection("Server=" & DBip & ";Port=" & DBpuerto & ";Userid=" & DBusu & ";password=" & DBpass & ";Pooling=true;MinPoolSize=1;SslMode=Disable;Database=postgres;MaxPoolSize=1000;")
                                        PostgreSQL.Open()
                                        Dim CreandoInstanciaConexion = New PG_ConexionC(Me.PG_Pass) With {.PostgreSQL = PostgreSQL}
                                        EjecutarSQLPostgreSQL("create database " & dbname & ";", CreandoInstanciaConexion, Nothing)
                                    Catch ex As Exception
                                        UnError = ex.Message
                                        Iniciado = False
                                        InstanciaConexion = Nothing
                                    End Try

                                Else
                                    UnError = ef.Message
                                    Iniciado = False
                                    InstanciaConexion = Nothing
                                End If

                            Catch ex As Exception
                                UnError = ex.Message
                                Iniciado = False
                                InstanciaConexion = Nothing
                            End Try

                        End If
                    End SyncLock
                End If


            End Sub


            Public Sub ActualizarDatosDeConexion(DBip$, DBpuerto%, DBusu$, DBpass$, dbname$, Optional _AutoAsignar_Fecha As Boolean = False, Optional _AutoAsignar_FechaM As Boolean = False)
                Conectarx(DBip, DBpuerto, DBusu, DBpass, dbname, _AutoAsignar_Fecha, _AutoAsignar_FechaM)
            End Sub



            Public Function RevisarDatosDeConexionValidos(DBip$, DBpuerto%, DBusu$, DBpass$, dbname$) As String

                Try

                    Dim PostgreSQL = New Npgsql.NpgsqlConnection("Server=" & DBip & ";Port=" & DBpuerto & ";Userid=" & DBusu & ";password=" & DBpass & ";Pooling=true;MinPoolSize=1;SslMode=Disable;Database=" & dbname & ";MaxPoolSize=1000;")
                    PostgreSQL.Open()

                    Dim Comando = New Npgsql.NpgsqlCommand
                    Comando.Connection = PostgreSQL
                    Comando.CommandType = Data.CommandType.Text
                    Comando.CommandTimeout = 60 * 30
                    Comando.CommandText = "SET standard_conforming_strings = off;"
                    Comando.ExecuteNonQuery()
                    Comando.Dispose()

                    Try
                        PostgreSQL.Close()
                        PostgreSQL.Dispose()
                    Catch
                    End Try
                    Return ""
                Catch ex As Exception
                    Return ex.Message
                End Try
            End Function


#Region "Cache - Tablas y Campos"
#End Region

            Public Sub ActualizarCAcheDecolumnasYTablas()
                Dim ColumnasYTablas = ConsultaSQLPostgreSQL("SELECT table_name,column_name  FROM information_schema.columns where table_schema='public';", False).LaFilaArray_PSN
                Dim Creando As New Dic(Of String, List(Of String))
                If ColumnasYTablas.TieneDatos Then
                    For i = 0 To ColumnasYTablas.Length - 1 Step 2
                        Creando.AddMagico(ColumnasYTablas(i + 0), ColumnasYTablas(i + 1))
                    Next
                End If
                TablasYCampos = Creando
            End Sub




#Region "Auto - Fecha"
#End Region


            Private Function AutoFecha(TablaSQL$) As Boolean
                If AutoAsignar_Fecha = False Then Return False
                Dim CamposX = TablasYCampos.HacerMagia(TablaSQL)
                Return CamposX IsNot Nothing AndAlso CamposX.Contains("fecha")
            End Function

            Private Function AutoFechaM(TablaSQL$) As Boolean
                If AutoAsignar_FechaM = False Then Return False
                Dim CamposX = TablasYCampos.HacerMagia(TablaSQL)
                Return CamposX IsNot Nothing AndAlso CamposX.Contains("fecham")
            End Function






#Region "Nativo"
#End Region



            Private Shared Sub Reconectar(Conexion As PG_ConexionC)
                Dim ExcepcionProducida As Exception = Nothing

                For i As Integer = 1 To 600

                    Try

                        Dim CadenaDeConexion = "password=" & Conexion.Pass & ";" & Conexion.PostgreSQL.ConnectionString

                        Conexion.Dispose()
                        Conexion.PostgreSQL = New NpgsqlConnection(CadenaDeConexion)
                        Conexion.PostgreSQL.Open()

                        Dim Comando = New Npgsql.NpgsqlCommand
                        Comando.Connection = Conexion.PostgreSQL
                        Comando.CommandType = Data.CommandType.Text
                        Comando.CommandTimeout = 60 * 30
                        Comando.CommandText = "SET standard_conforming_strings = off;"
                        Comando.ExecuteNonQuery()
                        Comando.Dispose()
                        Exit Sub
                    Catch ex As Exception
                        ExcepcionProducida = ex
                        System.Threading.Thread.Sleep(1000)
                    End Try

                Next

                If ExcepcionProducida IsNot Nothing Then
                    Throw ExcepcionProducida
                End If

            End Sub

            Private Shared Function EjecutarSQLPostgreSQL(ByVal SQL$, Conexion As PG_ConexionC, Optional ByRef return_notice As List(Of PostgresNotice) = Nothing) As Integer

                Dim ExcepcionProducida As Exception = Nothing

                For I As Integer = 1 To 20


                    Dim Comando As Npgsql.NpgsqlCommand = Nothing
                    Try
                        Comando = New Npgsql.NpgsqlCommand
                        Comando.Connection = Conexion.PostgreSQL
                        Comando.CommandType = Data.CommandType.Text
                        Comando.CommandTimeout = 60 * 30
                        Comando.CommandText = SQL
                        Dim FilasAfectadas = Comando.ExecuteNonQuery()
                        return_notice = Conexion.Notices
                        Comando.Dispose()
                        Return FilasAfectadas
                    Catch ex As Exception

                        If ExcepcionProducida Is Nothing Then
                            ExcepcionProducida = ex
                        End If


                        If Comando IsNot Nothing Then
                            Try : Comando.Dispose() : Catch : End Try
                        End If

                        If Conexion.Conectado = False AndAlso Conexion.ReconectarSiSePierdeLaConexion Then
                            Reconectar(Conexion)
                            Continue For
                        Else
                            Throw New Exception(ex.Message)
                            Return 0
                        End If

                    Finally

                        If Comando IsNot Nothing Then
                            Try : Comando.Dispose() : Catch : End Try
                        End If

                        If Conexion IsNot Nothing Then
                            Conexion.Notices = Nothing
                        End If
                    End Try

                Next

                Throw ExcepcionProducida
            End Function



            Private Overloads Shared Function ConsultaSQLPostgreSQL(ByVal SQL$, Conexion As PG_ConexionC, Informacion$, RecibirColumnas As Boolean, Optional UsarCopy As Boolean = False, Optional OptimizarMemoria As Boolean = False) As PG_ResultadoSQLC

                Dim ExcepcionProducida As Exception = Nothing


                For I As Integer = 1 To 20



                    Dim PGCopyDatos As Tuple(Of Boolean, String()) = Nothing


                    Dim Resultado As PG_ResultadoSQLC = New PG_ResultadoSQLC
                    Resultado.SQL = SQL

                    Dim SelectCache$ = ""


                    Dim ElReader As Npgsql.NpgsqlDataReader = Nothing
                    Dim Comando As Npgsql.NpgsqlCommand

                    Try




                        If RecibirColumnas Then

                            Comando = New Npgsql.NpgsqlCommand
                            Comando.Connection = Conexion.PostgreSQL
                            Comando.CommandType = Data.CommandType.Text
                            Comando.CommandTimeout = 60 * 30
                            Comando.CommandText = SQL

                            ElReader = Comando.ExecuteReader(CommandBehavior.KeyInfo)
                            ExtraerColumnasPostgreSQL(Resultado, Conexion, ElReader, SelectCache)

                            Resultado.TotalResultados = 0

                            If ElReader.HasRows Then
                                ExtraerResultadosDataReaderPostgreSQLAntiguo(Resultado, ElReader, False, OptimizarMemoria)
                            End If

                            Try
                                ElReader.Close()
                                ElReader.Dispose()
                                ElReader = Nothing
                            Catch
                            End Try

                        ElseIf UsarCopy Then


                            PGCopyDatos = SacarDatosCopy(Conexion, SQL, Resultado)
                            Resultado.LaFilaArray_PSN = ProcesarDatos(PGCopyDatos)
                            Return Resultado

                        Else

                            Comando = New Npgsql.NpgsqlCommand
                            Comando.Connection = Conexion.PostgreSQL
                            Comando.CommandType = Data.CommandType.Text
                            Comando.CommandTimeout = 60 * 30
                            Comando.CommandText = SQL

                            Dim Adaptador As New Npgsql.NpgsqlDataAdapter
                            Adaptador.SelectCommand = Comando

                            Dim ElDataSet As DataSet = New DataSet()
                            Adaptador.Fill(ElDataSet)
                            Dim Tabla = ElDataSet.Tables(0)

                            ExtraerResultadosDataReaderPostgreSQLNuevo(Resultado, Tabla, False, False, OptimizarMemoria)

                            ElDataSet.Dispose()
                            Adaptador.Dispose()
                            Tabla.Dispose()

                        End If


                        Try
                            If Comando IsNot Nothing Then
                                Comando.Dispose()
                                Comando = Nothing
                            End If
                        Catch
                        End Try


                        Return Resultado







                    Catch ex As Exception

                        If ExcepcionProducida Is Nothing Then
                            ExcepcionProducida = ex
                        End If



                        Try
                            If Comando IsNot Nothing Then
                                Comando.Dispose()
                                Comando = Nothing
                            End If
                        Catch
                        End Try


                        If Conexion.Conectado = False AndAlso Conexion.ReconectarSiSePierdeLaConexion Then
                            Reconectar(Conexion)
                            Continue For
                        Else
                            Throw New Exception(ex.Message)
                            Resultado.ErrorNumero = -1
                            Resultado.ErrorSQL = ex.Message
                            Return Resultado
                        End If

                    Finally


                        If Comando IsNot Nothing Then
                            Try
                                Comando.Dispose()
                                Comando = Nothing
                            Catch
                            End Try

                        End If

                        If ElReader IsNot Nothing Then
                            Try
                                ElReader.Close()
                                ElReader.Dispose()
                            Catch
                            End Try
                        End If
                        If Resultado.ErrorSQL <> "" Or Resultado.ErrorNumero <> 0 Then
                            Throw New Exception("*** ERROR PG ***" & vbCrLf & Resultado.ErrorSQL)
                        End If

                    End Try

                Next

                Throw ExcepcionProducida

            End Function



            Friend Shared Function ProcesarDatos(DatosX As Tuple(Of Boolean, String())) As String()

                If DatosX Is Nothing Then
                    Return Nothing
                End If


                Dim ArrayDeDatosCeldas() As String = DatosX.Item2
                Dim RequiereEscapar As Boolean = DatosX.Item1

                If RequiereEscapar = False Then
                    Return ArrayDeDatosCeldas
                Else
                    Dim pgdesescapado As New pgdesescapadorC(ArrayDeDatosCeldas)
                    pgdesescapado.Procesar()
                    ArrayDeDatosCeldas = pgdesescapado.retornar
                End If



                Return ArrayDeDatosCeldas

            End Function


            Friend Shared Function SacarDatosCopy(PostgreSQL As PG_ConexionC, SQL$, Resultado As PG_ResultadoSQLC) As Tuple(Of Boolean, String())

                Dim ArrayDeDatosCeldas() As String = Nothing
                Dim RequiereEscapar As Boolean
                Using reader As Npgsql.NpgsqlCopyTextReader = CType(PostgreSQL.PostgreSQL.BeginTextExport("COPY  (" & SQL & ") TO STDOUT"), Npgsql.NpgsqlCopyTextReader)

                    Dim TextoRead = reader.ReadToEnd()

                    If TextoRead = "" Then
                        Return Nothing
                    End If

                    ParsearCopyPG(ArrayDeDatosCeldas, RequiereEscapar, TextoRead)
                    Resultado.TotalColumnas = Mid(TextoRead, 1, TextoRead.IndexOf(vbLf)).Split(vbTab(0)).Length
                    Resultado.TotalResultados = CInt(ArrayDeDatosCeldas.Length / Resultado.TotalColumnas)

                End Using

                Return New Tuple(Of Boolean, String())(RequiereEscapar, ArrayDeDatosCeldas)
            End Function



            Friend Shared Sub ParsearCopyPG(ByRef ArrayDeDatosCeldas() As String, ByRef RequiereEscapar As Boolean, TextoRead As String)


                Dim textox = TextoRead.QueNoTermineEn(vbLf(0)).Replace(vbLf(0), vbTab)
                Dim TextoLimpio$

                If textox.Contains("\\") = False Then


                    TextoLimpio = textox.Replace("\r\n", vbCrLf)
                    TextoLimpio = TextoLimpio.Replace("\N", "")

                Else

                    TextoLimpio = textox

                    If textox.Contains("\\r\n") Then
                        TextoLimpio = TextoLimpio.Replace("\r\n", vbCrLf).Replace("\" & vbCrLf, "\\r\n")
                    Else
                        TextoLimpio = TextoLimpio.Replace("\r\n", vbCrLf)
                    End If



                    TextoLimpio = TextoLimpio.Replace(vbTab & "\N", vbTab)

                End If

                If TextoLimpio.StartsWith("\N") Then
                    TextoLimpio = TextoLimpio.EliminarInicio(2)
                End If

                ArrayDeDatosCeldas = TextoLimpio.Split(vbTab(0))
                RequiereEscapar = TextoLimpio.Contains("\"c)

            End Sub

#Region "Helper"
#End Region




            Private Shared Sub ExtraerColumnasPostgreSQL(Resultado As PG_ResultadoSQLC, Conexion As PG_ConexionC, ElReader As Npgsql.NpgsqlDataReader, SelectCache As String)



                Dim CargarColumnasDesdexsql As Boolean = True
                If SelectCache <> "" Then
                    If Conexion.CacheColumnas.ContainsKey(SelectCache) Then
                        Resultado.Columnas = Conexion.CacheColumnas(SelectCache)
                        Resultado.ColumnasID = Conexion.CacheColumnasID(SelectCache)
                        CargarColumnasDesdexsql = False
                    End If
                End If

                'Recibo desde  
                If CargarColumnasDesdexsql Then


                    Resultado.Columnas = New Dictionary(Of String, PG_ColumnaC)
                    Resultado.ColumnasID = New Dictionary(Of Integer, PG_ColumnaC)

                    Dim TablaInfo As DataTable = ElReader.GetSchemaTable()





                    Dim ColumnaActual = 0
                    For Each myField As DataRow In TablaInfo.Rows

                        If myField("IsHidden").ToString = "False" Then

                            Dim Columna As PG_ColumnaC = New PG_ColumnaC
                            Columna.ID = ColumnaActual + 1
                            Columna.Nombre = myField("ColumnName").ToString
                            Columna.NombreOriginal = myField("BaseColumnName").ToString
                            Columna.Tabla = myField("BaseTableName").ToString
                            Columna.TablaOriginal = myField("BaseTableName").ToString 'TODO No existe el alias

                            If myField("DataType").ToString = "System.String" Then 'TODO Convertir tipos de datos  ColumnaInfo.DataType 
                                Columna.Tipo = PG_TipoCampoE.Tipo_VARCHAR
                            ElseIf myField("DataType").ToString = "System.Int32" Then
                                Columna.Tipo = PG_TipoCampoE.Tipo_INTEGER
                            ElseIf myField("DataType").ToString = "System.Int16" Then
                                Columna.Tipo = PG_TipoCampoE.Tipo_TINYINT
                            ElseIf myField("DataType").ToString = "System.Byte" Then
                                Columna.Tipo = PG_TipoCampoE.Tipo_TINYINT
                            ElseIf myField("DataType").ToString = "System.DateTime" Then
                                Columna.Tipo = PG_TipoCampoE.Tipo_DATETIME
                            ElseIf myField("DataType").ToString = "System.Decimal" Then
                                Columna.Tipo = PG_TipoCampoE.Tipo_DOUBLE
                            ElseIf myField("DataType").ToString = "System.Byte[]" Then

#If DEBUG Then
                                MsgBox("Hay que cambiar el tipo por varchar(max)")
#End If

                            ElseIf myField("DataType").ToString = "System.Int64" Then
                                Columna.Tipo = PG_TipoCampoE.Tipo_BIGINT
                                'TODO Poner punto interrupción aquí para saber cuales son unsigned

                            ElseIf myField("DataType").ToString = "System.TimeSpan" Then
                                Columna.Tipo = PG_TipoCampoE.Tipo_TIME




                            Else

                                Dim ElTipoNoSoportado = myField("DataType").ToString

                                Columna.Tipo = PG_TipoCampoE.Tipo_VARCHAR
#If DEBUG Then
                                ' MsgBox("<SoloDebug> Tipo no soportado, agregar")
#End If
                            End If

                            Columna.Tama = CInt(myField("ColumnSize").ToString)
                            Columna.Unico = myField("IsUnique").ToString = "True"
                            Columna.AceptaNulos = myField("AllowDBNull").ToString = "True"
#If DEBUG Then
                            If Resultado.Columnas.ContainsKey(Columna.Nombre) Then
                                MsgBox("Ya se ha agregado una columna con el nombre: " & Columna.Nombre)
                            End If
#End If

                            Resultado.Columnas.Add(Columna.Nombre, Columna)
                            Resultado.ColumnasID.Add(ColumnaActual + 1, Columna)

                            ColumnaActual += 1

                            '  Else
                            '  Dim k = 1
                        End If

                    Next

                    Resultado.TotalColumnas = ColumnaActual

                    TablaInfo.Dispose()


                    'Guardo a Caché
                    If True Then
                        If SelectCache <> "" Then
                            Conexion.CacheColumnas.Add(SelectCache, Resultado.Columnas)
                            Conexion.CacheColumnasID.Add(SelectCache, Resultado.ColumnasID)
                        End If
                    End If


                End If
            End Sub




            Private Shared Sub ExtraerResultadosDataReaderPostgreSQLNuevo(Resultado As PG_ResultadoSQLC, LaTabla As DataTable, AutoTrimear As Boolean, fechasntimestamp As Boolean, Optional OptimizarMemoria As Boolean = False)


                Resultado.TotalResultados = LaTabla.Rows.Count
                Resultado.TotalColumnas = LaTabla.Columns.Count


                If Resultado.TotalResultados = 0 Then
                    Resultado.LaFilaArray_PSN = Nothing
                    Exit Sub
                Else
                    ReDim Resultado.LaFilaArray_PSN(Resultado.TotalResultados * Resultado.TotalColumnas - 1)
                End If



                Dim Tipos(Resultado.TotalColumnas - 1) As Integer
                For ColumnaActualTemp = 0 To Resultado.TotalColumnas - 1
                    Dim Actual = LaTabla.Columns(ColumnaActualTemp).DataType.ToString
                    If Actual.ContainsIgnoreCase("datetime") Then
                        Tipos(ColumnaActualTemp) = 2
                    ElseIf Actual.ContainsIgnoreCase("date") Then
                        Tipos(ColumnaActualTemp) = 1
                    ElseIf Actual.ContainsIgnoreCase("bool") Then
                        Tipos(ColumnaActualTemp) = 3
                    ElseIf Actual.ContainsIgnoreCase("numeric") Or Actual.ContainsIgnoreCase("decimal") Then
                        Tipos(ColumnaActualTemp) = 4
                    Else
                        Tipos(ColumnaActualTemp) = 0
                    End If
                Next


                Dim LosSTR As New Dic(Of String, String)



                Dim ActualArray%
                Dim FilaActual As Integer = 0
                Dim ColumnaActual As Integer = 0
                Do While (FilaActual < Resultado.TotalResultados)
                    ColumnaActual = 0
                    Do While (ColumnaActual < Resultado.TotalColumnas)

                        If Tipos(ColumnaActual) = 2 Then

                            If TypeOf LaTabla.Rows(FilaActual)(ColumnaActual) Is DBNull Then
                                Resultado.LaFilaArray_PSN(ActualArray) = ""
                            Else
                                Resultado.LaFilaArray_PSN(ActualArray) = DirectCast(LaTabla.Rows(FilaActual)(ColumnaActual), DateTime).ToString("yyyy-MM-dd HH:mm:ss")
                            End If

                        ElseIf Tipos(ColumnaActual) = 1 Then

                            If TypeOf LaTabla.Rows(FilaActual)(ColumnaActual) Is DBNull Then
                                Resultado.LaFilaArray_PSN(ActualArray) = ""
                            Else
                                Dim Fecha = LaTabla.Rows(FilaActual)(ColumnaActual)
                                Resultado.LaFilaArray_PSN(ActualArray) = (New Date(Fecha.Year, Fecha.Month, Fecha.Day)).ToString("yyyy-MM-dd HH:mm:ss")
                            End If

                        ElseIf Tipos(ColumnaActual) = 3 Then

                            If TypeOf LaTabla.Rows(FilaActual)(ColumnaActual) Is DBNull Then
                                Resultado.LaFilaArray_PSN(ActualArray) = "0"
                            Else
                                If LaTabla.Rows(FilaActual)(ColumnaActual) = True Then
                                    Resultado.LaFilaArray_PSN(ActualArray) = "1"
                                Else
                                    Resultado.LaFilaArray_PSN(ActualArray) = "0"
                                End If
                            End If


                        ElseIf Tipos(ColumnaActual) = 4 Then

                            If TypeOf LaTabla.Rows(FilaActual)(ColumnaActual) Is DBNull Then
                                Resultado.LaFilaArray_PSN(ActualArray) = ""
                            Else

                                Dim ValorNumericoX = LaTabla.Rows(FilaActual)(ColumnaActual).ToString.Replace(",", ".")

                                'If ValorNumericoX.Contains(".") Then
                                '    ValorNumericoX = ValorNumericoX.TrimEnd("0").TrimEnd(".")
                                'End If

                                Resultado.LaFilaArray_PSN(ActualArray) = ValorNumericoX

                            End If


                        Else
                            Resultado.LaFilaArray_PSN(ActualArray) = LaTabla.Rows(FilaActual)(ColumnaActual).ToString
                        End If


                        If OptimizarMemoria Then

                            If LosSTR.ContainsKey(Resultado.LaFilaArray_PSN(ActualArray)) = False Then
                                LosSTR.Add(Resultado.LaFilaArray_PSN(ActualArray), Resultado.LaFilaArray_PSN(ActualArray))
                            End If
                            Resultado.LaFilaArray_PSN(ActualArray) = LosSTR(Resultado.LaFilaArray_PSN(ActualArray))


                        End If

                        ActualArray += 1
                        ColumnaActual += 1
                    Loop
                    FilaActual += 1
                Loop













            End Sub





            Private Shared Sub ExtraerResultadosDataReaderPostgreSQLAntiguo(Resultado As PG_ResultadoSQLC, ElReader As Npgsql.NpgsqlDataReader, AutoTrimear As Boolean, OptimizarMemoria As Boolean)


                If Resultado.TotalColumnas = 0 Then
                    Resultado.TotalColumnas = ElReader.FieldCount
                End If

                Dim Resultados As New List(Of String)(Resultado.TotalColumnas + 1)




                Dim Tipos(Resultado.TotalColumnas - 1) As Integer


                Dim LosSTR As New Dic(Of String, String)

                For ColumnaActual = 0 To Resultado.TotalColumnas - 1
                    Dim Actual = ElReader.GetFieldType(ColumnaActual).ToString
                    If Actual.ContainsIgnoreCase("datetime") Then
                        Tipos(ColumnaActual) = 2
                    ElseIf Actual.ContainsIgnoreCase("date") Then
                        Tipos(ColumnaActual) = 1
                    ElseIf Actual.ContainsIgnoreCase("bool") Then
                        Tipos(ColumnaActual) = 3
                    ElseIf Actual.ContainsIgnoreCase("numeric") Or Actual.ContainsIgnoreCase("decimal") Then
                        Tipos(ColumnaActual) = 4
                    Else
                        Tipos(ColumnaActual) = 0
                    End If

                Next



                Do While ElReader.Read()
                    Resultado.TotalResultados += 1
                    For ColumnaActual% = 0 To Resultado.TotalColumnas - 1

                        If Tipos(ColumnaActual) = 2 Then

                            If ElReader.IsDBNull(ColumnaActual) Then
                                Resultados.Add("")
                            Else
                                Dim kk = ElReader.GetDateTime(ColumnaActual)
                                Resultados.Add(kk.AdaptarMySQL)
                            End If
                        ElseIf Tipos(ColumnaActual) = 4 Then

                            If ElReader.IsDBNull(ColumnaActual) Then
                                Resultados.Add("")
                            Else

                                Dim ValorNumericoX = ElReader.GetValue(ColumnaActual).ToString.Replace(",", ".")
                                'If ValorNumericoX.Contains(".") Then
                                '    ValorNumericoX = ValorNumericoX.TrimEnd("0").TrimEnd(".")
                                'End If
                                Resultados.Add(ValorNumericoX)

                            End If

                        ElseIf Tipos(ColumnaActual) = 1 Then
                            If ElReader.IsDBNull(ColumnaActual) Then
                                Resultados.Add("")
                            Else
                                Dim kk = ElReader.GetDate(ColumnaActual)
                                Resultados.Add((New Date(kk.Year, kk.Month, kk.Day)).AdaptarMySQL_Date)
                            End If
                        ElseIf Tipos(ColumnaActual) = 3 Then
                            Dim kk = ElReader.GetValue(ColumnaActual)

                            Resultados.Add(kk.ToString.BOOL.STR)
                        Else
                            Dim kk = ElReader.GetValue(ColumnaActual)
                            Resultados.Add(kk.ToString)
                        End If



                        If OptimizarMemoria Then


                            If LosSTR.ContainsKey(Resultados(Resultados.Count - 1)) = False Then
                                LosSTR.Add(Resultados(Resultados.Count - 1), Resultados(Resultados.Count - 1))
                            End If
                            Resultados(Resultados.Count - 1) = LosSTR(Resultados(Resultados.Count - 1))

                        End If

                    Next
                Loop

                If Resultado.TotalResultados > 0 Then
                    Resultado.LaFilaArray_PSN = Resultados.ToArray
                End If

            End Sub









        End Class



    End Class
End Class




Friend Class pgdesescapadorC

    Friend lista() As String
    Friend retornar() As String
    Friend CantidadColumnas%

    Shared cb As Char = Convert.ToChar(8)
    Shared cf As Char = Convert.ToChar(12)
    Shared cn As Char = Convert.ToChar(10)
    Shared cr As Char = Convert.ToChar(13)
    Shared ct As Char = Convert.ToChar(9)
    Shared cv As Char = Convert.ToChar(11)
    Shared strb As String = Convert.ToChar(8)
    Shared strf As String = Convert.ToChar(12)
    Shared strn As String = Convert.ToChar(10)
    Shared strr As String = Convert.ToChar(13)
    Shared strt As String = Convert.ToChar(9)
    Shared strv As String = Convert.ToChar(11)


    Friend StepActual As Integer


    Friend Sub Procesar()

        retornar = lista



        Dim Rangos As New List(Of Integer)

        If lista.Length < 100 Then
            StepActual = 10
            For i = 0 To lista.Length - 1 Step 10
                Rangos.Add(i)
            Next
        ElseIf lista.Length < 1000 Then
            StepActual = 100
            For i = 0 To lista.Length - 1 Step 100
                Rangos.Add(i)
            Next
        ElseIf lista.Length < 10000 Then
            StepActual = 1000
            For i = 0 To lista.Length - 1 Step 1000
                Rangos.Add(i)
            Next
        ElseIf lista.Length < 100000 Then
            StepActual = 5000
            For i = 0 To lista.Length - 1 Step 5000
                Rangos.Add(i)
            Next
        Else
            StepActual = CInt(lista.Length / 10)
            For i = 0 To lista.Length - 1 Step StepActual
                Rangos.Add(i)
            Next
        End If

        For Each Actual In Rangos
            QuitarEscapePostgreCopy(Actual)
        Next

    End Sub




    Private Sub QuitarEscapePostgreCopy(_LineaDesde As Integer)

        Dim retornar = Me.retornar
        Dim CeldaDesde = _LineaDesde
        Dim CeldaHasta = Math.Min(_LineaDesde + (StepActual - 1), lista.Length - 1)

        For I_Celda = CeldaDesde To CeldaHasta



            Dim LineaTieneBarra = retornar(I_Celda).IndexOf("\"c) <> -1
            If LineaTieneBarra = False Then
                Continue For
            End If



            If retornar(I_Celda).Contains("\\") = False Then
                '! Sin invalidadores invalidades \\ : Puedo reempalzar felizmente 
                If retornar(I_Celda).IndexOf("\b") <> -1 Then retornar(I_Celda) = retornar(I_Celda).Replace("\b", strb)
                If retornar(I_Celda).IndexOf("\f") <> -1 Then retornar(I_Celda) = retornar(I_Celda).Replace("\f", strf)
                If retornar(I_Celda).IndexOf("\n") <> -1 Then retornar(I_Celda) = retornar(I_Celda).Replace("\n", strn)
                If retornar(I_Celda).IndexOf("\r") <> -1 Then retornar(I_Celda) = retornar(I_Celda).Replace("\r", strr)
                If retornar(I_Celda).IndexOf("\t") <> -1 Then retornar(I_Celda) = retornar(I_Celda).Replace("\t", strt)
                If retornar(I_Celda).IndexOf("\v") <> -1 Then retornar(I_Celda) = retornar(I_Celda).Replace("\v", strv)
                Continue For
            End If


            Dim InvalidadorActivo As Boolean = False

            Dim CreandoSTR(retornar(I_Celda).Length) As Char
            Dim CreandoSTRcoutn = 0


            For ix = 0 To retornar(I_Celda).Length - 1


                Select Case retornar(I_Celda)(ix)
                    Case "\"c

                        If InvalidadorActivo Then
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else

                            InvalidadorActivo = True
                        End If

                    Case "b"c

                        If InvalidadorActivo Then
                            CreandoSTR(CreandoSTRcoutn) = cb
                            CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                        End If


                    Case "f"c


                        If InvalidadorActivo Then
                            CreandoSTR(CreandoSTRcoutn) = cf
                            CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                        End If

                    Case "N"c

                        If InvalidadorActivo Then
                            '@ empty 
                            'CreandoSTR(CreandoSTRcoutn) = String.em
                            'CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                        End If

                    Case "n"c

                        If InvalidadorActivo Then
                            CreandoSTR(CreandoSTRcoutn) = cn
                            CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                        End If

                    Case "r"c


                        If InvalidadorActivo Then
                            CreandoSTR(CreandoSTRcoutn) = cr
                            CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                        End If

                    Case "t"c


                        If InvalidadorActivo Then
                            CreandoSTR(CreandoSTRcoutn) = ct
                            CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                        End If


                    Case "v"c


                        If InvalidadorActivo Then
                            CreandoSTR(CreandoSTRcoutn) = cv
                            CreandoSTRcoutn += 1
                            InvalidadorActivo = False
                        Else
                            CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                            CreandoSTRcoutn += 1
                        End If

                    Case Else

                        CreandoSTR(CreandoSTRcoutn) = retornar(I_Celda)(ix)
                        CreandoSTRcoutn += 1

                End Select


            Next

            retornar(I_Celda) = New String(CreandoSTR, 0, CreandoSTRcoutn)




        Next


    End Sub


    Sub New(_lista As String())
        Me.lista = _lista
    End Sub
End Class

