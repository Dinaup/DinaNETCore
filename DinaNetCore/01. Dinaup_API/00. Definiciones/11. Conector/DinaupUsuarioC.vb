Partial Public Class APID


    Public Class DinaupSesionC






        Public ConexionServidor As ServidorDinaup_ConectorC
        Public DatosSesion As DinaupAPI_UsuarioSesionC


        Public SesionHTTP_PN As HTTPRespuestaAPIC_IniciarSesionC
        Public UserAgent$ = "?"
        Public IP$ = "?"


        Public ReadOnly Property SesionIniciada As Boolean
            Get
                Return DatosSesion IsNot Nothing
            End Get
        End Property


        Sub New(_Server As ServidorDinaup_ConectorC, _UserAgen$, _IP$)
            Me.ConexionServidor = _Server
            Me.UserAgent = _UserAgen
            Me.IP = _IP
        End Sub

        Sub New()
        End Sub

    End Class

End Class
