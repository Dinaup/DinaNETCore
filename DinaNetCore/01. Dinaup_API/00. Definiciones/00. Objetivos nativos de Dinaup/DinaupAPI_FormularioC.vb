

Partial Public Class APID



    Public Class DinaupAPI_FormularioC


        Public _JObject As Newtonsoft.Json.Linq.JToken

        Public Token$
        Public Titulo$
        Public Icono$

        Public Ventana As New List(Of DinaupAPI_Formulario_RegistroC)



        Public Overrides Function ToString() As String
            Return "DinaupAPI_FormularioC: " & Titulo
        End Function


        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)
            _JObject = JsonObj("formularios")
            Token = JsonObj("token").STR
            Titulo = JsonObj("titulo").STR
            Icono = JsonObj("icono").STR
            For Each Actual In _JObject
                Ventana.Add(New DinaupAPI_Formulario_RegistroC(Actual))
            Next
        End Sub


    End Class














End Class
