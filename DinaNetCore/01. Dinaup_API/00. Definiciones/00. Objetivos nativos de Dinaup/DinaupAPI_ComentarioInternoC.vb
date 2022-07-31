

Partial Public Class APID

    Public Class DinaupAPI_VariablesListaC

        Public Variables As New Dic(Of String, DinaupAPI_VariableC)

        Sub New(JObject As Newtonsoft.Json.Linq.JToken)
            For Each Actual In JObject
                If Actual.Type = Newtonsoft.Json.Linq.JTokenType.Null Then Continue For
                Dim VActual = New DinaupAPI_VariableC(Actual)
                Variables.Add(VActual.KeyWord, VActual)
            Next
        End Sub

    End Class

End Class
