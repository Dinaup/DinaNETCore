

Partial Public Class APID





    Public Class DinaupAPI_EnumeradorDinamico_ValoresC

        Public Titulo$
        Public Valores As New Dic(Of Integer, String)
        Sub New(JObject As Newtonsoft.Json.Linq.JToken)
            Titulo = JObject("titulo").STR
            For Each Actual In JObject("valores")
                Valores.Add(Actual("valor").STR.INT(0), Actual("titulo").STR)
            Next
        End Sub

    End Class

End Class
