
Partial Public Class APID


    Public Class DinaupAPI_Formulario_PestanaC

        Public _JObject As Newtonsoft.Json.Linq.JToken
        Public JObject_Contenedores As Newtonsoft.Json.Linq.JToken
        Public JObject_Controles As Newtonsoft.Json.Linq.JToken
        Public JObject_Botones As Newtonsoft.Json.Linq.JToken

        Public TokenPestana$
        Public Pestana$
        Public Alto%
        Public Ancho%
        Public Principal As Boolean
        Public Contenedores As New List(Of DinaupAPI_Formulario_ControlC)
        Public Controles As New List(Of DinaupAPI_Formulario_ControlC)
        Public Botones As New List(Of DinaupAPI_Formulario_ControlC)


        Public Overrides Function ToString() As String
            On Error Resume Next
            If Pestana Is Nothing Then
                Return "DinaupAPI_Formulario_PestanaC: ¿?"
            Else
                Return "DinaupAPI_Formulario_PestanaC: " & Pestana
            End If
        End Function




        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)


            _JObject = JsonObj

            JObject_Contenedores = _JObject("contenedores")
            JObject_Controles = _JObject("controles")
            JObject_Botones = _JObject("botones")

            Tokenpestana = _JObject("tokenpestana").STR
            Pestana = _JObject("pestana").STR
            Alto = _JObject("alto").INT
            Ancho = _JObject("ancho").INT
            Principal = _JObject("principal").BOOL

            For Each Actual In JObject_Contenedores
                Contenedores.Add(New DinaupAPI_Formulario_ControlC(Actual))
            Next

            For Each Actual In JObject_Controles
                Controles.Add(New DinaupAPI_Formulario_ControlC(Actual))
            Next

            For Each Actual In JObject_Botones
                Botones.Add(New DinaupAPI_Formulario_ControlC(Actual))
            Next

        End Sub


    End Class


End Class
