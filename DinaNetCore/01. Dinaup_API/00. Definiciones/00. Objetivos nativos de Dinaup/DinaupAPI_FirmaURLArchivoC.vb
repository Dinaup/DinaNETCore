

Partial Public Class APID


    Public Class FirmaURLArchivoSubidoC
        Implements ToJSoneableI

        Public ArchivoID As Guid
        Public URLFirmada As String
        Public NombreOriginalArchivo As String


        Sub New(JObject As Newtonsoft.Json.Linq.JToken)
            Me.ArchivoID = JObject("id").STR.ToGuid
            Me.URLFirmada = JObject("url").STR
            Me.NombreOriginalArchivo = JObject("nombre").STR
        End Sub
        Sub New(_JSON As String)
            If _JSON <> "" Then
                Dim ElObj As Newtonsoft.Json.Linq.JToken = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(_JSON), Newtonsoft.Json.Linq.JToken)
                Me.ArchivoID = ElObj("id").STR.ToGuid
                Me.URLFirmada = ElObj("url").STR
                Me.NombreOriginalArchivo = ElObj("nombre").STR
            End If
        End Sub

        Sub New(_Id As Guid, _url As String, _name As String)
            Me.ArchivoID = _Id
            Me.URLFirmada = _url
            Me.NombreOriginalArchivo = _name
        End Sub



        Public Function ToJSON() As JSONBuildC Implements ToJSoneableI.ToJSON
            Dim R As New JSONBuildC
            R.Add("{")
            R.Add("id", ArchivoID)
            R.Add("url", URLFirmada)
            R.Add("nombre", NombreOriginalArchivo)
            R.Add("}")
            Return R
        End Function



    End Class

End Class
