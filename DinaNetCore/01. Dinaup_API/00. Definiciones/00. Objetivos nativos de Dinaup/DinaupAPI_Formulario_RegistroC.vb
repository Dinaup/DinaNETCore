
Public Class APID




    Public Class DinaupAPI_Formulario_RegistroC


        Public _JObject As Newtonsoft.Json.Linq.JToken
        Public JObject_Comentarios As Newtonsoft.Json.Linq.JToken
        Public JObject_Documentacion As Newtonsoft.Json.Linq.JToken
        Public JObject_Pestana As Newtonsoft.Json.Linq.JToken


        Public JObject_Seccion As Newtonsoft.Json.Linq.JToken
        Public JObject_SeccionLista As Newtonsoft.Json.Linq.JToken





        Public Token As String
        Public Tamano As Drawing.Size


        Public Seccion As DinaupAPI_SeccionC
        Public SeccionLista As DinaupAPI_SeccionC




        Public EsLista As Boolean
        Public Lista As Newtonsoft.Json.Linq.JToken


        Public Pestanas As New List(Of DinaupAPI_Formulario_PestanaC)


        Public Overrides Function ToString() As String
            On Error Resume Next
            If Seccion Is Nothing Then
                Return "DinaupAPI_Formulario_RegistroC: ¿?"
            Else
                Return "DinaupAPI_Formulario_RegistroC: " & Seccion.Titulo
            End If
        End Function



        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)

            _JObject = JsonObj
            JObject_Pestana = _JObject("pestanas")
            JObject_Comentarios = _JObject("comentarios")
            JObject_Documentacion = _JObject("documentacion")

            Token = _JObject("token").STR
            Tamano = New Drawing.Size(_JObject("ancho").ToString.INT(0), _JObject("alto").ToString.INT(0))
            JObject_Seccion = _JObject("seccion")
            JObject_SeccionLista = _JObject("seccionlista")
            EsLista = CType(_JObject("eslista"), Boolean)
            Lista = _JObject("lista")

            If JObject_Pestana IsNot Nothing Then
                For Each Actual In JObject_Pestana
                    Pestanas.Add(New DinaupAPI_Formulario_PestanaC(Actual))
                Next
            End If

            Seccion = New DinaupAPI_SeccionC(JObject_Seccion)

            If JObject_SeccionLista IsNot Nothing Then
                SeccionLista = New DinaupAPI_SeccionC(JObject_SeccionLista)
            End If

        End Sub


    End Class
End Class
