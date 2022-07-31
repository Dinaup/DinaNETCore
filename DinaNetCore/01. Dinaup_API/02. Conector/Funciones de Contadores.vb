Partial Public Class APID




    Partial Public Class ServidorDinaup_ConectorC






        Public Function Funcion_Contador_Ping(Sesion As DinaupSesionC, CampoID As Guid, RegistroID As Guid) As HTTPRespuestaAPIC_ContadorC
            Dim param As New Specialized.NameValueCollection
            param.Add("cmd", "-1")
            param.Add("registroid", RegistroID.STR)
            param.Add("campoid", CampoID.STR)
            Dim R = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "contador", param, Sesion.UserAgent, Sesion.IP)
            Dim X As New HTTPRespuestaAPIC_ContadorC(R)
            Return X
        End Function

        Public Function Funcion_Contador_Detener(Sesion As DinaupSesionC, CampoID As Guid, RegistroID As Guid) As HTTPRespuestaAPIC_ContadorC
            Dim param As New Specialized.NameValueCollection
            param.Add("cmd", "0")
            param.Add("registroid", RegistroID.STR)
            param.Add("campoid", CampoID.STR)
            Dim R = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "contador", param, Sesion.UserAgent, Sesion.IP)
            Dim X As New HTTPRespuestaAPIC_ContadorC(R)
            Return X
        End Function

        Public Function Funcion_Contador_Iniciar(Sesion As DinaupSesionC, CampoID As Guid, RegistroID As Guid) As HTTPRespuestaAPIC_ContadorC
            Dim param As New Specialized.NameValueCollection
            param.Add("cmd", "1")
            param.Add("registroid", RegistroID.STR)
            param.Add("campoid", CampoID.STR)
            Dim R = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "contador", param, Sesion.UserAgent, Sesion.IP)
            Dim X As New HTTPRespuestaAPIC_ContadorC(R)
            Return X
        End Function




    End Class





    Public Class HTTPRespuestaAPIC_ContadorC





        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)

        Public RespuestaGenerica As HTTP_RespuestaAPIC




        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror

        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub



    End Class






End Class