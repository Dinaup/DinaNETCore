Imports DinaNetCore.ExtensionesM
Imports Npgsql


Public MustInherit Class Base_DinaModeloC




    Public Sub Construir(_Dic As Dic(Of String, String))
        Construir_DesdeDiccionario(_Dic)
    End Sub


    Public MustOverride Sub Construir_DesdeDiccionario(_Dic As Dic(Of String, String))


    Public MustOverride Function Convertir_ADiccionar() As Dic(Of String, String)




    Sub New()
    End Sub


End Class

Partial Public Class BasesDeDatoD
    Partial Public Class PostgreSQLD
        Partial Public Class PG_ServidorC




            Public Event TablaModificada(Tabla$, SQlEjecutado$, DicValores As Dic(Of String, String))

            Private Sub Gatillo_TablaModificada(Tabla$, SQlEjecutado$, DicValores As Dic(Of String, String))
                On Error Resume Next
                RaiseEvent TablaModificada(Tabla$, SQlEjecutado$, DicValores)
            End Sub





#Region "Consulta"
#End Region


            Public Overloads Function ConsultaSQLDato(SQL$) As String
                SyncLock InstanciaConexion

                    Dim temp = ConsultaSQLPostgreSQL(SQL, InstanciaConexion, "", False)
                    If temp IsNot Nothing AndAlso temp.LaFilaArray_PSN IsNot Nothing AndAlso temp.LaFilaArray_PSN.Length = 1 Then
                        Return temp.LaFilaArray_PSN(0)
                    Else
                        Return Nothing
                    End If
                End SyncLock
            End Function


            Public Overloads Function ConsultaDatoSQL(SQL$) As String
                Return ConsultaSQLDato(SQL)
            End Function





            Public Iterator Function ConsultaSQLPostgreSQL_PorBloques_Obj(Of t)(Descripcion$, _SQLContar$, _SQLDatos$, _CantidadBloque%) As IEnumerable(Of List(Of t))

                Dim Bloques = ConsultaSQLPostgreSQL_PorBloques_Dic(Descripcion$, _SQLContar$, _SQLDatos$, _CantidadBloque%)
                For Each BloqueActual In Bloques
                    Dim Buffer As New List(Of t)
                    For Each ObjActualDIC In BloqueActual
                        Dim Objeto = Activator.CreateInstance(Of t)
                        TryCast(Objeto, Base_DinaModeloC).Construir(ObjActualDIC)
                        Buffer.Add(Objeto)
                    Next
                    Yield Buffer
                Next

            End Function


            Public Iterator Function ConsultaSQLPostgreSQL_PorBloques_Dic(Descripcion$, _SQLContar$, _SQLDatos$, _CantidadBloque%) As IEnumerable(Of List(Of Dic(Of String, String)))




                Dim Buffer As New List(Of Dic(Of String, String))


                Dim Reporte_Log = DinaNetCore.SyncLogD.IniciarSincronizacion(Descripcion)
                Dim DatosSQL = ConsultaSQLPostgreSQL_PorBloques(_SQLContar, _SQLDatos, _CantidadBloque)
                Reporte_Log.CargarResumen(DatosSQL.Lectura_TotalResultados)

                While DatosSQL.LeerFila
                    Buffer.Add(DatosSQL.ToDic)
                    If Buffer.Count >= _CantidadBloque Then
                        Yield Buffer

                        If Reporte_Log IsNot Nothing Then
                            Reporte_Log.ReportarProcesados(Buffer.Count)
                        End If

                        Buffer = New List(Of Dic(Of String, String))
                    End If



                End While

                If Buffer.Count > 0 Then
                    Yield Buffer
                    If Reporte_Log IsNot Nothing Then
                        Reporte_Log.ReportarProcesados(Buffer.Count)
                    End If
                End If

                Reporte_Log.FIN()



            End Function






            Public Overloads Function ConsultaSQLPostgreSQL_PorBloques(SQL_Contar$, SQL_Datos$, ResultadosPorBloque%, Optional Inicio% = 0) As PG_ResultadoSQL_BloquesC
                Dim X As New PG_ResultadoSQL_BloquesC
                X.Lectura_SQL_Datos = SQL_Datos
                X.Lectura_SQL_Contar = SQL_Contar
                X.Lectura_TotalResultados = ConsultaSQLDato(SQL_Contar).INT(0)
                X.Lectura_TResultadosPorBloque = ResultadosPorBloque
                X.Lectura_PosicionActual = Inicio
                X.Lectura_PosicionInicial = Inicio
                X.Conexion = Me
                Return X
            End Function


            Public Overloads Function ConsultaSQLPostgreSQL(SQL$, RecibirColumnas As Boolean, Optional OptimizarMemoria As Boolean = False) As PG_ResultadoSQLC
                SyncLock Me.InstanciaConexion
                    Return ConsultaSQLPostgreSQL(SQL, Me.InstanciaConexion, "", RecibirColumnas, False, OptimizarMemoria)
                End SyncLock
            End Function


            Public Overloads Function ConsultaSQLPostgreSQL_Copy(SQL$) As PG_ResultadoSQLC
                SyncLock Me.InstanciaConexion
                    Return ConsultaSQLPostgreSQL(SQL, Me.InstanciaConexion, "", False, True)
                End SyncLock
            End Function



#Region "EjecutarSQL"
#End Region

            Public Function EjecutarSQL%(SQL$)
                SyncLock Me.InstanciaConexion
                    Return EjecutarSQLPostgreSQL%(SQL, Me.InstanciaConexion, Nothing)
                End SyncLock
            End Function
            Public Function EjecutarSQL%(SQL$, ByRef return_notice As List(Of PostgresNotice))
                SyncLock Me.InstanciaConexion
                    Return EjecutarSQLPostgreSQL%(SQL, Me.InstanciaConexion, return_notice)
                End SyncLock
            End Function


            Public Function EjecutarSQL_InsertSiNoExiste(dicX As Dic(Of String, String), TablaSQL As String) As Boolean

                If AutoFecha(TablaSQL) Then dicX.AddSiNoExiste("fecha", Date.UtcNow.AdaptarMySQL)
                If AutoFechaM(TablaSQL) Then dicX.AddSiNoExiste("fecham", Date.UtcNow.AdaptarMySQL)



                Dim Valores As New List(Of String)


                For Each Actual In dicX.Values.ToList
                    If Actual Is Nothing Then
                        Valores.Add("null")
                    Else
                        Valores.Add("'" & Actual.AdaptarMySQL & "'")
                    End If
                Next

                Dim ElSQL = "insert into " & TablaSQL & "(" & String.Join(",", dicX.Keys) & ")  values(" & Valores.STRJoin(",") & ") ON CONFLICT DO NOTHING"
                Dim Afectados = EjecutarSQL(ElSQL)

                If Afectados <> 0 Then
                    Gatillo_TablaModificada(TablaSQL, ElSQL, dicX)
                End If


                Return Afectados <> 0

            End Function



            Public Function CrearInsertSQL(dicX As Dic(Of String, String), TablaSQL As String) As String
                If AutoFecha(TablaSQL) Then dicX.AddSiNoExiste("fecha", Date.UtcNow.AdaptarMySQL)
                If AutoFechaM(TablaSQL) Then dicX.AddSiNoExiste("fecham", Date.UtcNow.AdaptarMySQL)
                Dim Valores As New List(Of String)
                For Each Actual In dicX.Values.ToList
                    If Actual Is Nothing Then
                        Valores.Add("null")
                    ElseIf Actual.Contains("to_tsvector(") Then
                        Valores.Add(Actual)
                    Else
                        Valores.Add("'" & Actual.AdaptarMySQL & "'")
                    End If
                Next
                Dim ElSQL = "insert into " & TablaSQL & "(" & String.Join(",", dicX.Keys) & ")  values(" & Valores.STRJoin(",") & ")"
                Return (ElSQL)
            End Function


            Public Sub EjecutarSQL_InsertDIC(SQL_DicDatos As Dic(Of String, String), SQL_Tabla As String)
                Dim SQL_Insert = CrearInsertSQL(SQL_DicDatos, SQL_Tabla)
                EjecutarSQL(SQL_Insert)
                Gatillo_TablaModificada(SQL_Tabla, SQL_Insert, SQL_DicDatos)
            End Sub



            Public Function CrearSQL_Update(dicX As Dic(Of String, String), TablaSQL As String, CampoID$, ValorID$, Optional ImpedirAsignarFechamAtuoamticamente As Boolean = False) As String


                Dim ElSQL = "update " & TablaSQL & " set "
                Dim XAutoFechaM = AutoFechaM(TablaSQL)
                Dim XAutoFecha = AutoFecha(TablaSQL)


                Dim ElWhere As New List(Of String)



                Dim Separador = ""
                For Each Actual In dicX

                    If Actual.Key = CampoID Then Continue For
                    If XAutoFechaM AndAlso Actual.Key = "fecham" Then Continue For
                    If XAutoFecha AndAlso Actual.Key = "fecha" Then Continue For


                    If ImpedirAsignarFechamAtuoamticamente AndAlso Actual.Key = "fecham" Then Continue For
                    If ImpedirAsignarFechamAtuoamticamente AndAlso Actual.Key = "fecha" Then Continue For

                    If Actual.Value Is Nothing Then

                        ElSQL &= Separador & Actual.Key & " = null "
                        ElWhere.Add(Actual.Key & " is not null ")

                    ElseIf Actual.Value.Contains("to_tsvector(") Then

                        ElSQL &= Separador & Actual.Key & " = " & Actual.Value & " "
                        ElWhere.Add(Actual.Key & " <> " & Actual.Value & "")

                    Else
                        ElSQL &= Separador & Actual.Key & " = '" & Actual.Value.AdaptarMySQL & "' "
                        ElWhere.Add("(" & Actual.Key & " is null or " & Actual.Key & " <> '" & Actual.Value.AdaptarMySQL & "')")

                    End If

                    Separador = ", "

                Next




                If XAutoFechaM AndAlso ImpedirAsignarFechamAtuoamticamente = False Then
                    ElSQL &= Separador & " fecham = '" & Date.UtcNow.AdaptarMySQL & "' "
                End If







                ElSQL &= " where " & CampoID & "='" & ValorID.AdaptarMySQL & "' and (" & ElWhere.STRJoin(" or ") & ")"

                Return ElSQL


            End Function





            Public Sub EjecutarSQL_Update(dicX As Dic(Of String, String), TablaSQL As String, CampoID$, ValorID$)

                Dim ElSQL = CrearSQL_Update(dicX, TablaSQL, CampoID, ValorID)
                Dim Afectado = EjecutarSQL(ElSQL)
                If Afectado > 0 Then
                    Gatillo_TablaModificada(TablaSQL, ElSQL, dicX)
                End If

            End Sub







            Public Sub EjecutarSQL_InsertOUpdate(dicX As Dic(Of String, String), TablaSQL As String, CampoID$, Optional CamposActualizar() As String = Nothing)
                EjecutarSQL_InsertOUpdate(New List(Of Dic(Of String, String))({dicX}), TablaSQL, CampoID$, CamposActualizar)
            End Sub



            Public Sub EjecutarSQL_InsertOUpdate(dicX As List(Of Dic(Of String, String)), TablaSQL As String, CampoID$, Optional CamposActualizar() As String = Nothing)








                If dicX.TieneDatos Then
                    For Each DicDatosActual In dicX
                        If DicDatosActual.HacerMagia("id") = "" Then
                            Throw New Exception("Se esperaba un valor ID.")
                        End If
                    Next
                End If





                If CamposActualizar Is Nothing Then
                    CamposActualizar = dicX.First.Keys.ToArray
                End If


                Dim AutoMatico_FechaM = AutoFechaM(TablaSQL)
                Dim AutoMatico_Fecha = AutoFecha(TablaSQL)

                Dim FechaActual As Date = Date.UtcNow
                Dim Campos_CRC As New List(Of String)(dicX.First.Keys.ToArray)
                Dim Campos_Updatear = CamposActualizar.ToList


                Campos_Updatear.Remove(CampoID)

                If AutoMatico_Fecha Then

                    Campos_Updatear.Remove("fecha")
                    Campos_CRC.Remove("fecha")
                    For Each DicActual In dicX
                        DicActual.HacerMagia("fecha") = FechaActual.AdaptarMySQL
                    Next

                End If

                If AutoMatico_FechaM Then
                    Campos_CRC.Remove("fecham")
                    Campos_Updatear.Remove("fecham")
                    For Each DicActual In dicX
                        DicActual.HacerMagia("fecham") = FechaActual.AdaptarMySQL
                    Next
                End If





                For Each DicActual In dicX


                    Dim SQL_Update$ = CrearSQL_Update(DicActual, TablaSQL, CampoID, DicActual(CampoID), True)
                    Dim SQL_Insert$ = CrearInsertSQL(DicActual, TablaSQL)

                    If ConsultaDatoSQL("select 1 from " & TablaSQL & " where " & CampoID & "='" & DicActual(CampoID).AdaptarMySQL & "' limit 1") = "1" Then

                        EjecutarSQL(SQL_Update)
                    Else
                        EjecutarSQL(SQL_Insert$)
                    End If


                Next





            End Sub




            '            Public Sub EjecutarSQL_InsertOUpdate(dicX As List(Of Dic(Of String, String)), TablaSQL As String, CampoID$, Optional CamposActualizar() As String = Nothing)








            '                If dicX.TieneDatos Then
            '                    For Each DicDatosActual In dicX
            '                        If DicDatosActual.HacerMagia("id") = "" Then
            '                            Throw New Exception("Se esperaba un valor ID.")
            '                        End If
            '                    Next
            '                End If





            '                If CamposActualizar Is Nothing Then
            '                    CamposActualizar = dicX.First.Keys.ToArray
            '                End If


            '                Dim AutoMatico_FechaM = AutoFechaM(TablaSQL)
            '                Dim AutoMatico_Fecha = AutoFecha(TablaSQL)

            '                Dim FechaActual As Date = Date.UtcNow
            '                Dim Campos_CRC As New List(Of String)(dicX.First.Keys.ToArray)
            '                Dim Campos_Updatear = CamposActualizar.ToList


            '                Campos_Updatear.Remove(CampoID)

            '                If AutoMatico_Fecha Then

            '                    Campos_Updatear.Remove("fecha")
            '                    Campos_CRC.Remove("fecha")
            '                    For Each DicActual In dicX
            '                        DicActual.HacerMagia("fecha") = FechaActual.AdaptarMySQL
            '                    Next

            '                End If

            '                If AutoMatico_FechaM Then
            '                    Campos_CRC.Remove("fecham")
            '                    Campos_Updatear.Remove("fecham")
            '                    For Each DicActual In dicX
            '                        DicActual.HacerMagia("fecham") = FechaActual.AdaptarMySQL
            '                    Next
            '                End If







            '                Dim HanAfectadoCambios As Boolean = False





            '                Dim return_notice As List(Of PostgresNotice) = Nothing



            '                Dim ElSQL As New List(Of String)


            '                ElSQL.Add("DO $$
            '    DECLARE
            '        afectado int;
            '        afectadotemp int;
            'BEGIN
            'afectado=0;
            'afectadotemp=0;
            '")



            '                For Each DicActual In dicX






            '                    Dim SQL_Update$ = CrearSQL_Update(DicActual, TablaSQL, CampoID, DicActual(CampoID), True)
            '                    Dim SQL_Insert$ = CrearInsertSQL(DicActual, TablaSQL)
            '                    Dim SQL_ActualizarFechaM$ = ""

            '                    If AutoMatico_FechaM Then

            '                        SQL_ActualizarFechaM = "
            'IF (afectadotemp > 0) THEN
            '    update " & TablaSQL & " set fecham = '" & FechaActual.AdaptarMySQL & "' where " & CampoID & " = '" & DicActual(CampoID).AdaptarMySQL & "'; 
            'END IF;
            '"
            '                    End If





            '                    ElSQL.Add("
            '      IF  (select 1 from " & TablaSQL & " where " & CampoID & "='" & DicActual(CampoID).AdaptarMySQL & "' limit 1) = 1 THEN
            '            " & SQL_Update & ";
            '            get diagnostics afectadotemp = row_count;
            '           " & SQL_ActualizarFechaM & "
            '      ELSE
            '            " & SQL_Insert & ";
            '            get diagnostics afectadotemp = row_count;

            '      END IF;

            '            afectado = afectado + afectadotemp;
            '            ")

            '                Next

            '                ElSQL.Add(" RAISE INFO 'Afectados-%',  afectado::text; END $$;")




            '                EjecutarSQL(ElSQL.STRJoin(), return_notice)


            '                If return_notice IsNot Nothing Then
            '                    For Each Actual In return_notice
            '                        If Actual.MessageText <> "" Then
            '                            If Actual.MessageText.StartsWith("Afectados-") Then
            '                                If Actual.MessageText.StartsWith("Afectados-0") = False Then
            '                                    HanAfectadoCambios = True
            '                                End If
            '                            End If
            '                        End If
            '                    Next
            '                End If






            '                If HanAfectadoCambios Then
            '                    Gatillo_TablaModificada(TablaSQL, ElSQL.STRJoin(), Nothing)

            '                End If


            '            End Sub

        End Class
    End Class
End Class
