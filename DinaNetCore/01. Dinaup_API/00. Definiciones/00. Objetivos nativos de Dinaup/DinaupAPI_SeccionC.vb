
Public Class DinaupAPI_SeccionC


    Public ID As Guid
    Public Titulo As String
    Public IconoID As String
    Public EtiquetaSingular As String
    Public EtiquetaPlural As String
    Public EtiquetaEsFemenina As Boolean
    Public ContieneLista As Boolean
    Public EsLista As Boolean
    Public EsBase As Boolean
    Public PuedeAgrgar As Boolean


    Sub New(JObject As Newtonsoft.Json.Linq.JToken)


        ID = JObject("id").ToString.ToGuid
        Titulo = JObject("titulo").ToString
        IconoID = JObject("iconoid").ToString
        EtiquetaSingular = JObject("etiquetasingular").ToString
        EtiquetaPlural = JObject("etiquetaplural").ToString
        EtiquetaEsFemenina = CType(JObject("etiquetaesfemenino"), Boolean)
        ContieneLista = CType(JObject("contienelista"), Boolean)
        EsLista = CType(JObject("eslista"), Boolean)
        EsBase = CType(JObject("esbase"), Boolean)
        PuedeAgrgar = CType(JObject("puedeagregar"), Boolean)

    End Sub


End Class
