
Partial Public Class APID

    Public Class DinaupAPI_EmpresaC


        Public ID As Guid
        Public Name As String
        Public Archivada As Boolean
        Public IdentificadorDeAcceso As String
        Public ColorMarca As String
        Public Eliminado As Boolean

        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)
            ID = JsonObj("id").ToGUID
            Name = JsonObj("nombre").STR
            Archivada = JsonObj("archivada").BOOL
            IdentificadorDeAcceso = JsonObj("archivada").STR
            ColorMarca = JsonObj("color").STR
            Eliminado = JsonObj("eliminado").BOOL
        End Sub

    End Class

End Class
