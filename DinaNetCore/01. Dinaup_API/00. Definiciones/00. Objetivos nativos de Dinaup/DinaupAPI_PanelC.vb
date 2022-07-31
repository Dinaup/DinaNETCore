

Partial Public Class APID


    Public Class DinaupAPI_PanelC

        Public Nombre$
        Public Nivel$
        Public Icono$
        Public ID$


        Public Function URL_Icono() As String

            Return Icono

        End Function

        Sub New(_obj As Newtonsoft.Json.Linq.JToken)
            Nombre = _obj("nombre").STR
            Nivel = _obj("nivel").STR
            Icono = _obj("icono").STR
            ID = _obj("id").STR
        End Sub
    End Class

End Class
