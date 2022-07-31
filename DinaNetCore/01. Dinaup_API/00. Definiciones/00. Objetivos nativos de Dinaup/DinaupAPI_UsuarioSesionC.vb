

Partial Public Class APID

    Public Class DinaupAPI_UsuarioSesionC


        Public UltimaActividad As Date

        Public SesionIniciada As Boolean
        Public SesionID As Guid




        Public Usuarios As Tuple(Of Guid, String)
        Public Empresa As Tuple(Of Guid, String)
        Public Ubicacion As Tuple(Of Guid, String)

        Public SesionIP As String
        Public CambiarPass As Boolean
        Public SesionUserAgent As String

        Public Paneles As New List(Of DinaupAPI_PanelC)

        Public JSON As String



        Sub New(JObject As Newtonsoft.Json.Linq.JToken)

            JSON = JObject.ToString
            UltimaActividad = Date.UtcNow
            SesionIniciada = JObject("sesioniniciada").BOOL
            SesionID = JObject("sesionid").ToGUID


            Usuarios = New Tuple(Of Guid, String)(JObject("usuarioid").ToGUID, JObject("usuarionombre").STR)
            Empresa = New Tuple(Of Guid, String)(JObject("empresaid").ToGUID, JObject("empresanombre").STR)
            Ubicacion = New Tuple(Of Guid, String)(JObject("ubicacionid").ToGUID, JObject("ubicacionnombre").STR)


            SesionIP = JObject("sesionip").STR
            CambiarPass = JObject("cambiarpass").BOOL
            SesionUserAgent = JObject("sesionnavegador").STR

            If JObject("paneles") IsNot Nothing Then
                For Each Actual In JObject("paneles")
                    Paneles.Add(New DinaupAPI_PanelC(Actual))
                Next
            End If


        End Sub






        Sub New()

        End Sub



    End Class
End Class
