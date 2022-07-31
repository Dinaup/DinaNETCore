Partial Public Class APID







    Partial Public Class ServidorDinaup_ConectorC




        Public Async Function Funcion_Informes_Consultar(Sesion As DinaupSesionC, Parametros As Funcion_Informe_Consultar_ParametrosC) As Task(Of HTTPRespuestaAPIC_InformeC)

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_listar_informeid", Parametros.InformeID.STR)
            DAtos.Add("dinaup_listar_pagina", Parametros.PaginaActual.ToString)
            DAtos.Add("dinaup_listar_rrpp", Parametros.ResultadosPorPagina.ToString)
            DAtos.Add("dinaup_listar_busqueda", Parametros.Busqueda)
            DAtos.Add("dinaup_listar_modominimo", Parametros.ModoMinimo.STR)
            DAtos.Add("dinaup_listar_vista", Parametros.ViewConfig)


            DAtos.Add("tokenctrl", Parametros.TokenCTRL)
            DAtos.Add("secciondestino", Parametros.SeccionDestinoID)
            DAtos.Add("dinaup_utc_diff", Parametros.DiferenciaUTC.ToString)

            If Parametros.SeparadorDecimales = "." Then
                DAtos.Add("dinaup_sep_decimal", ".")
            Else
                DAtos.Add("dinaup_sep_decimal", ",")
            End If


            Dim R = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(Sesion, "informe", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim Retornar As New HTTPRespuestaAPIC_InformeC(R)
            Return Retornar
        End Function







    End Class






    Public Class HTTPRespuestaAPIC_InformeC



        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)
        Public RespuestaGenerica As HTTP_RespuestaAPIC
        Public Informe As DinaupAPI_InformeC
        Public Listado As DinaupAPI_Informe_DatosC



        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror
            If Obj.Obj_Respuesta IsNot Nothing AndAlso Obj.Ok Then
                If Obj.Obj_Respuesta IsNot Nothing AndAlso Obj.Obj_Respuesta("informe").HasValues Then
                    Informe = New DinaupAPI_InformeC(Obj.Obj_Respuesta("informe"))
                End If
                If Obj.Obj_Respuesta IsNot Nothing AndAlso Obj.Obj_Respuesta("listado").HasValues Then
                    Listado = New DinaupAPI_Informe_DatosC(Obj.Obj_Respuesta("listado"))
                End If

            End If
        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub

    End Class


    Public Class Funcion_Informe_Consultar_ParametrosC


        Public InformeID As String
        Public PaginaActual% = 1
        Public ResultadosPorPagina% = 300
        Public Buffer_Orden As New Dic(Of String, Boolean)
        Public Buffer_Respuestas As New Dic(Of String, String)
        Public Buffer_Filtro As New List(Of String)
        Public Busqueda As String = ""
        Public ModoMinimo As Boolean = False






        Public TokenCTRL$
        Public SeccionDestinoID$
        Public DiferenciaUTC As Integer
        Public SeparadorDecimales As String = ","




        Public ViewConfig_Manual$


        Sub New(_param As Funcion_Informe_Consultar_ParametrosC)

            'Me.InformeID = InformeID
            Me.InformeID = _param.InformeID
            Me.PaginaActual = _param.PaginaActual%
            Me.ResultadosPorPagina = _param.ResultadosPorPagina

            Me.Busqueda = _param.Busqueda
            Me.ModoMinimo = _param.ModoMinimo
            Me.TokenCTRL = _param.TokenCTRL$
            Me.SeccionDestinoID = _param.SeccionDestinoID$
            Me.DiferenciaUTC = _param.DiferenciaUTC
            Me.SeparadorDecimales = _param.SeparadorDecimales
            Me.ViewConfig_Manual$ = _param.ViewConfig_Manual

            If _param.Buffer_Orden IsNot Nothing Then Me.Buffer_Orden = New Dic(Of String, Boolean)(_param.Buffer_Orden)
            If _param.Buffer_Respuestas IsNot Nothing Then Me.Buffer_Respuestas = New Dic(Of String, String)(_param.Buffer_Respuestas)
            If _param.Buffer_Filtro IsNot Nothing Then Me.Buffer_Filtro = New List(Of String)(_param.Buffer_Filtro)

        End Sub





        Public ReadOnly Property ViewConfig$
            Get

                If ViewConfig_Manual <> "" Then Return ViewConfig_Manual

                Dim R As New List(Of String)

                If Buffer_Respuestas.TieneDatos Then
                    For Each Actual In Buffer_Respuestas
                        R.Add(("dupvar785_" & Actual.Key).AdaptarURL & "=" & Actual.Value.AdaptarURL)
                    Next
                End If



                If Buffer_Filtro.TieneDatos Then
                    For i As Integer = 0 To Buffer_Filtro.Count - 1 Step 3
                        R.Add(("q_" & Buffer_Filtro(i)).AdaptarURL & "=" & (Buffer_Filtro(i + 1) + Buffer_Filtro(i + 2)).AdaptarURL)
                    Next
                End If



                If Buffer_Orden.TieneDatos Then
                    For Each Actual In Buffer_Orden
                        R.Add(("o_" & Actual.Key).AdaptarURL & "=" & If(Actual.Value, "1", "-1"))
                    Next
                End If


                Return String.Join("&", R)

            End Get
        End Property




        Public Sub Agregar_Respuesta(_Key$, _Value$)
            If _Key = "" Then Throw New Exception(". Código de error (E-5297)")
            If _Value Is Nothing Then _Value = ""
            Buffer_Respuestas.HacerMagia(_Key) = _Value
        End Sub
        Public Sub Agregar_Filtro(_Key$, _Operator$, _Value$)
            If _Key = "" Then Throw New Exception(". Código de error (E-5299)")
            If _Operator = "" Then Throw New Exception(". Código de error (E-5300)")

            Select Case _Operator
                Case "=", "<", ">", "<>", ">=", "<="
                Case Else
                    Throw New Exception("The operator '" & _Operator & "' is not valid. Código de error (E-5301)")
            End Select

            Me.Buffer_Filtro.Add(_Key)
            Me.Buffer_Filtro.Add(_Operator)
            Me.Buffer_Filtro.Add(_Value)

        End Sub




        Public Sub Agregar_Ordenar(_Key$, _desc As Boolean)
            If _Key = "" Then Throw New Exception(". Código de error (E-5298)")
            Buffer_Orden.HacerMagia(_Key) = _desc
        End Sub


        Sub New(_InformeID As String)
            Me.InformeID = _InformeID
        End Sub

    End Class

End Class
