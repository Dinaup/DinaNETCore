

Partial Public Class APID

    Public Class DinaupAPI_UbicacionC

        Public ID As Guid
        Public Name As String
        Public NombrePublico As String
        Public Telefono As String
        Public CodigoPostal As String
        Public Direccion As String
        Public ServidorID As Guid
        Public Eliminado As Boolean



        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)
            Me.ID = JsonObj("id").STR.ToGuid
            Me.Name = JsonObj("nombre").STR
            Me.NombrePublico = JsonObj("nombrepublico").STR
            Me.Telefono = JsonObj("telefono").STR
            Me.CodigoPostal = JsonObj("codigopostal").STR
            Me.Direccion = JsonObj("direccion").STR
            Me.ServidorID = JsonObj("servidorid").ToGUID
            Me.Eliminado = JsonObj("eliminado").BOOL
        End Sub

    End Class
End Class
