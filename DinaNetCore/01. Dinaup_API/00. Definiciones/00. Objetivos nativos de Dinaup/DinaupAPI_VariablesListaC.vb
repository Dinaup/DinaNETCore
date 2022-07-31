

Partial Public Class APID

    Public Class DinaupAPI_AnotacionRegistroC

        'Public Variables As New Dic(Of String, DinaupAPI_VariableC)
        Public ID As Guid
        Public Texto$
        Public Autor As DinaupAPI_TextoPrincipalC
        Public fecha As Date
        Public adjuntoarchivos As New List(Of DinaupAPI_ArchivoC)
        Public adjuntoregistros As New List(Of DinaupAPI_TextoPrincipalC)
        Sub New(JObject As Newtonsoft.Json.Linq.JToken)
            'For Each Actual In JObject
            '    If Actual.Type = Newtonsoft.Json.Linq.JTokenType.Null Then Continue For
            '    Dim VActual = New DinaupAPI_VariableC(Actual)
            '    Variables.Add(VActual.KeyWord, VActual)
            'Next

            ID = JObject("id").ToString.ToGuid()
            Texto = JObject("texto").STR
            Autor = New DinaupAPI_TextoPrincipalC(JObject("autor"))
            fecha = JObject("fecha").STR.ToDateDesdeMySQL_utc

            If JObject("adjuntoarchivos").NoEsNulo Then
                For Each Actual In JObject("adjuntoarchivos")
                    adjuntoarchivos.Add(New DinaupAPI_ArchivoC(Actual))
                Next
            End If

            If JObject("adjuntoregistros") IsNot Nothing Then
                For Each Actual In JObject("adjuntoregistros")
                    adjuntoregistros.Add(New DinaupAPI_TextoPrincipalC(Actual))
                Next
            End If


        End Sub

    End Class

    Public Class DinaupAPI_TextoPrincipalC

        Public ID As Guid
        Public Nombre As String
        Public PreviaID As Guid
        Public SeccionID As Guid
        Public icono_RolCampo As RolCampoE
        Public icono_Valor As Integer

        Sub New(JObject As Newtonsoft.Json.Linq.JToken)
            Me.ID = JObject("id").STR.ToGuid
            Me.Nombre = JObject("nombre").STR
            Me.PreviaID = JObject("imagenid").STR.ToGuid
            Me.SeccionID = JObject("seccionid").STR.ToGuid
            Me.icono_RolCampo = CType(JObject("iconorol").INT(0), RolCampoE)
            Me.icono_Valor = JObject("iconovalor").INT(0)
        End Sub

    End Class


End Class
