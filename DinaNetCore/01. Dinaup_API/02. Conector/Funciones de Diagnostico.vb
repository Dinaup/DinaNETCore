Partial Public Class APID



    Partial Public Class ServidorDinaup_ConectorC



        Public Function Funcion_Sesion_RecibirDetalles_Instalacion(UserUserAgent$, UserIP$) As HTTPRespuestaAPIC_RespuestaInstalacionC

            Dim DAtos As New Specialized.NameValueCollection
            'DAtos.Add("dinaup_sesion", SesionID.ToString)
            Dim R = Me.Http_EjecutarFuncionAPI_JSON(Nothing, "instalacion", DAtos, UserUserAgent, UserIP)
            Dim Retornar As New HTTPRespuestaAPIC_RespuestaInstalacionC(R)
            Return Retornar
        End Function




    End Class




    Public Class HTTPRespuestaAPIC_RespuestaInstalacionC


        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)



        Public RespuestaGenerica As HTTP_RespuestaAPIC


        Public ReadOnly Property Codigo%
            Get
                Return RespuestaGenerica.Codigo
            End Get
        End Property

        Public ReadOnly Property Ok As Boolean
            Get
                Return RespuestaGenerica.Ok
            End Get
        End Property

        Public ReadOnly Property Descripcion$
            Get
                Return RespuestaGenerica.Descripcion
            End Get
        End Property


        Public archivos As String
        Public archivospublicos As String

        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            Me.RespuestaGenerica = Obj
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror

            If Obj.Obj_Respuesta.NoEsNulo Then
                archivos = Obj.Obj_Respuesta("archivos").STR
                archivospublicos = Obj.Obj_Respuesta("archivospublicos").STR
            End If

        End Sub





        Sub New(Obj As HTTP_RespuestaAPIC)
            Iniciar(Obj)
        End Sub

    End Class





End Class
