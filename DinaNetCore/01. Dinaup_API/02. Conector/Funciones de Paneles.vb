Partial Public Class APID



    Partial Public Class ServidorDinaup_ConectorC




        Public Function Funcion_Panel_Cargar(Sesion As APID.DinaupSesionC, PanelID As String) As HTTPRespuestaAPIC_CargarPanelC
            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("id", PanelID)
            Dim R = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "cargarpanel", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_CargarPanelC(R)
            Return X
        End Function





    End Class





    Public Class HTTPRespuestaAPIC_CargarPanelC





        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)

        Public DetallesSesion As APID.DinaupAPI_UsuarioSesionC
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
