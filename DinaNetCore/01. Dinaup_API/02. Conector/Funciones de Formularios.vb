Partial Public Class APID



    Partial Public Class ServidorDinaup_ConectorC



        Public Function Funcion_Formulario_Abrir(Sesion As DinaupSesionC, _SeccionID$, _CampoBusqueda$, _ValorBusqueda$) As HTTPRespuestaAPIC_FormualarioC
            If Sesion Is Nothing Then Throw New Exception(". Código de error (E-2387)")
            If Sesion.ConexionServidor Is Nothing Then Throw New Exception(". Código de error (E-2310)")
            If Sesion.DatosSesion Is Nothing Then Throw New Exception(". Código de error (E-2306)")

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_comando", "abrir")
            DAtos.Add("dinaup_seccionid", _SeccionID)
            DAtos.Add("dinaup_registro_campo", _CampoBusqueda)
            DAtos.Add("dinaup_registro_valor", _ValorBusqueda)

            Dim HTTPResponse = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "formulario", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_FormualarioC(HTTPResponse)
            Return X

        End Function

        Public Function Funcion_Formulario_Abrir(Sesion As DinaupSesionC, _SeccionID As Guid, _CampoBusqueda$, _ValorBusqueda$) As HTTPRespuestaAPIC_FormualarioC

            If Sesion Is Nothing Then Throw New Exception(". Código de error (E-2203)")
            If Sesion.ConexionServidor Is Nothing Then Throw New Exception(". Código de error (E-2229)")
            If Sesion.DatosSesion Is Nothing Then Throw New Exception(". Código de error (E-2395)")

            Return Funcion_Formulario_Abrir(Sesion, _SeccionID.STR, _CampoBusqueda, _ValorBusqueda)
        End Function

        Public Function Funcion_Formulario_Ping(Sesion As DinaupSesionC, Form_Token$, Form_Values$) As HTTPRespuestaAPIC_FormualarioC

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_comando", "sincronizar")
            DAtos.Add("dinaup_form_token", Form_Token)
            DAtos.Add("dinaup_form_valores", Form_Values)
            Dim HTTPResponse = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "formulario", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_FormualarioC(HTTPResponse)
            Return X

        End Function

        Public Function Funcion_Formulario_NuevoElemento(Sesion As DinaupSesionC, Form_Token$, Form_Values$) As HTTPRespuestaAPIC_FormualarioC


            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_comando", "nuevoelemento")
            DAtos.Add("dinaup_form_valores", Form_Values)
            DAtos.Add("dinaup_form_token", Form_Token)
            Dim HTTPResponse = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "formulario", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_FormualarioC(HTTPResponse)
            Return X

        End Function

        Public Function Funcion_Formulario_EliminarElemento(Sesion As DinaupSesionC, Form_Token$, Form_Values$, Token$) As HTTPRespuestaAPIC_FormualarioC


            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_comando", "eliminarelemento")
            DAtos.Add("dinaup_form_valores", Form_Values)
            DAtos.Add("dinaup_form_token", Form_Token)
            DAtos.Add("dinaup_lista_eliminartoken", Token.ToString)
            Dim HTTPResponse = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "formulario", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_FormualarioC(HTTPResponse)
            Return X

        End Function

        Public Function Funcion_Formulario_Guardar(Sesion As DinaupSesionC, Form_Token$, Form_Values$) As HTTPRespuestaAPIC_Formualario_GuardarC

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_comando", "guardar")
            DAtos.Add("dinaup_form_valores", Form_Values)
            DAtos.Add("dinaup_form_token", Form_Token)

            Dim HTTPResponse = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "formulario", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_Formualario_GuardarC(HTTPResponse)
            Return X

        End Function



        Public Function Funcion_Formulario_Cancelar(Sesion As DinaupSesionC, Form_Token$) As HTTPRespuestaAPIC_Formualario_CancelarC

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_comando", "cancelar")
            DAtos.Add("dinaup_form_token", Form_Token)
            Dim HTTPResponse = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "formulario", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_Formualario_CancelarC(HTTPResponse)
            Return X

        End Function
    End Class


    Public Class HTTPRespuestaAPIC_FormualarioC






        Public DinapSesion As DinaupSesionC



        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)

        Public RespuestaGenerica As HTTP_RespuestaAPIC



        Public R_URL$
        Public R_Nombre$
        Public R_Estado%


        Public R_Formulario As DinaupAPI_FormularioC
        Public R_FormularioDatoSTR As String
        Public R_FormularioJSON As Newtonsoft.Json.Linq.JToken



        Public Overrides Function ToString() As String
            Return "HTTPRespuestaAPIC_FormualarioC"
        End Function



        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror
            DinapSesion = Obj.DinapSesion

            Dim ObjForm = Obj.Obj_Respuesta("formulario")
            If ObjForm IsNot Nothing Then

                Me.R_Formulario = New DinaupAPI_FormularioC(ObjForm)
                Me.R_FormularioDatoSTR = ObjForm("formularios").ToString
                Me.R_Estado = ObjForm("estado").INT
                Me.R_Nombre = ObjForm("nombre").STR
                Me.R_URL = ObjForm("url").STR

                Try

                    Me.R_FormularioJSON = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(R_FormularioDatoSTR), Newtonsoft.Json.Linq.JToken)

                Catch : End Try


            End If


            'Me.ServerConnection = Obj.DinapSesion
            'Me.DetallesSesion = Obj.DinapSesion

            'Dim X As Newtonsoft.Json.Linq.JToken = Obj.Obj_Respuesta("formulario")




        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub






    End Class




    Public Class HTTPRespuestaAPIC_Formualario_GuardarC


        Public DinapSesion As DinaupSesionC
        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)
        Public DetallesSesion As APID.DinaupAPI_UsuarioSesionC
        Public RespuestaGenerica As HTTP_RespuestaAPIC

        Public procesados%
        Public ignorados_sinvalorclave%
        Public ignorados_claverepetida%
        Public optimizados%
        Public nuevos%
        Public actualizaciones%

        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            Me.avisosok = Obj.avisosok
            Me.avisoserror = Obj.avisoserror
            Me.DinapSesion = Obj.DinapSesion
            'Me.DetallesSesion = Obj.DinapSesion
            Me.RespuestaGenerica = Obj



            If Obj.Obj_Respuesta IsNot Nothing Then
                procesados = Obj.Obj_Respuesta("procesados").STR.INT(0)
                ignorados_sinvalorclave = Obj.Obj_Respuesta("ignorados_sinvalorclave").STR.INT(0)
                ignorados_claverepetida = Obj.Obj_Respuesta("ignorados_claverepetida").STR.INT(0)
                optimizados = Obj.Obj_Respuesta("optimizados").STR.INT(0)
                nuevos = Obj.Obj_Respuesta("nuevos").STR.INT(0)
                actualizaciones = Obj.Obj_Respuesta("actualizaciones").STR.INT(0)
            Else
                Dim kkkk = 2
            End If

        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub



    End Class

    Public Class HTTPRespuestaAPIC_Formualario_CancelarC

        Public DinapSesion As DinaupSesionC
        'Public ServerConnection As ServidorDinaup_ConectorC
        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)
        'Public DetallesSesion As APID.DinaupAPI_UsuarioSesionC
        Public RespuestaGenerica As HTTP_RespuestaAPIC



        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            Me.avisosok = Obj.avisosok
            Me.avisoserror = Obj.avisoserror
            Me.DinapSesion = Obj.DinapSesion
            'Me.ServerConnection = Obj.ServerConnection
            'Me.DetallesSesion = Obj.DinapSesion
            Me.RespuestaGenerica = Obj
        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub

    End Class




End Class

