

Partial Public Class APID


    Public Class DinaupAPI_InformeC

        Public Title$
        Public ID As String
        Public Icon As String
        Public Seccion As DinaupAPI_SeccionC
        Public Categoria$
        Public SubCategoria$
        Public Aviso_Texto$
        Public Descripcion$
        Public Ocultar_SistemaDeAPginacion As Boolean
        Public OcultarRI_Informes As Boolean
        Public OcultarRI_Registros As Boolean
        Public InformeRelacionadoID As Guid
        Public InformeRelacionadoEtiqueta As String
        Public MostrarTodasLasEmpresas As Boolean
        Public MostrarTodasLasUbicaciones As Boolean
        Public Preguntas As APID.DinaupAPI_VariablesListaC


        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)




            ID = JsonObj("id").STR
            Title = JsonObj("titulo").STR
            Categoria = JsonObj("categoria").STR
            SubCategoria = JsonObj("subcategoria").STR
            Aviso_Texto = JsonObj("aviso_texto").STR
            Descripcion = JsonObj("descripcion").STR
            Ocultar_SistemaDeAPginacion = JsonObj("ocultarpaginas").BOOL
            OcultarRI_Informes = JsonObj("ocultarriinformes").BOOL
            OcultarRI_Registros = JsonObj("ocultarriregistros").BOOL
            InformeRelacionadoID = JsonObj("informerelacionadoid").ToGUID
            InformeRelacionadoEtiqueta = JsonObj("informerelacionadoetiqueta").STR
            MostrarTodasLasEmpresas = JsonObj("vertodaslasempresas").BOOL
            MostrarTodasLasUbicaciones = JsonObj("vertodaslasubicaciones").BOOL



            If JsonObj("seccion") IsNot Nothing AndAlso JsonObj("seccion").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Seccion = New DinaupAPI_SeccionC(JsonObj("seccion"))
            End If
            If JsonObj("icono") IsNot Nothing Then
                Icon = JsonObj("icono").STR
            End If
            If JsonObj("preguntas") IsNot Nothing AndAlso JsonObj("preguntas").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                If JsonObj("preguntas").Count > 0 Then
                    Preguntas = New DinaupAPI_VariablesListaC(JsonObj("preguntas"))
                End If
            End If

        End Sub

    End Class



End Class

