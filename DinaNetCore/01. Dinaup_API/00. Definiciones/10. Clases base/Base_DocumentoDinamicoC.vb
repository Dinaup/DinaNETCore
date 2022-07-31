


Partial Public Class APID
    Public MustInherit Class Base_DocumentoDinamicoC





        Public Sesion As DinaupSesionC
        Public ID As Guid
        Public Titulo$
        Public Parametros As Funcion_DocumentoDinamico_Consultar_ParametrosC
        Public Respuesta As HTTPRespuestaAPIC_DocumentoDinamicoC


        'Public MustOverride Sub CargarRespuesta()



        Public Sub Consultar(_Sesion As DinaupSesionC)
            Me.Sesion = _Sesion
            Respuesta = Sesion.ConexionServidor.Funcion_DocumentoDinamico_Consultar(Sesion, Parametros)
        End Sub






        Public Sub Agregar_Respuesta(_Key$, _Value As Boolean)
            Parametros.Agregar_Respuesta(_Key, _Value.STR)
        End Sub
        Public Sub Agregar_Respuesta(_Key$, _Value As Decimal)
            Parametros.Agregar_Respuesta(_Key, _Value.AdaptarJSON)
        End Sub

        Public Sub Agregar_Respuesta(_Key$, _Value As Integer)
            Parametros.Agregar_Respuesta(_Key, _Value.STR)
        End Sub

        Public Sub Agregar_Respuesta(_Key$, _Value As Guid)
            Parametros.Agregar_Respuesta(_Key, _Value.STR)
        End Sub
        Public Sub Agregar_Respuesta(_Key$, _Value$)
            Parametros.Agregar_Respuesta(_Key, _Value)
        End Sub

    End Class

End Class
