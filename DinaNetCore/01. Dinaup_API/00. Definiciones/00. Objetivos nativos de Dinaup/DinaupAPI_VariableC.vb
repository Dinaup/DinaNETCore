

Partial Public Class APID


    Public Class DinaupAPI_VariableC



        Public EtiquetaB$
        Public Informacion$
        Public KeyWord$
        Public Formato As TipoCamposSQLServerE
        Public EnPreguntasMostrarSelectorDeFechaDinamica$
        Public RolCampo As RolCampoE


        Public SeccionRelacionada As DinaupAPI_SeccionC
        Public DesplegableInforme As DinaupAPI_InformeC


        Public Rango As Boolean
        'Public inputtype As String
        Public Enumeracion As DinaupAPI_EnumeradorDinamico_ValoresC




        Sub New(JObject As Newtonsoft.Json.Linq.JToken)

            Me.EtiquetaB = JObject("titulo").ToString
            Me.Informacion$ = JObject("descripcion").ToString
            Me.KeyWord$ = JObject("keyword").ToString
            Me.Formato = CType(JObject("formato").INT, TipoCamposSQLServerE)
            Me.RolCampo = CType(JObject("rol").INT, RolCampoE)

            If JObject("seccionrelacionada") IsNot Nothing AndAlso JObject("seccionrelacionada").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Me.SeccionRelacionada = New DinaupAPI_SeccionC(JObject("seccionrelacionada"))
            End If


            If JObject("desplegableinforme") IsNot Nothing AndAlso JObject("desplegableinforme").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Me.DesplegableInforme = New DinaupAPI_InformeC(JObject("desplegableinforme"))
            End If



            Me.Rango = JObject("rango").ToString.BOOL


            If JObject("enumeracion") IsNot Nothing AndAlso JObject("enumeracion").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Me.Enumeracion = New DinaupAPI_EnumeradorDinamico_ValoresC(JObject("enumeracion"))
            End If


        End Sub

    End Class


End Class
