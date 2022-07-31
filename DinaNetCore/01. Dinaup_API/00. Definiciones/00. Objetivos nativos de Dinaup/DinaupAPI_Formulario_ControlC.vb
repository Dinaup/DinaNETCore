
Partial Public Class APID

    Public Class DinaupAPI_Formulario_ControlC


        Public _JObject As Newtonsoft.Json.Linq.JToken



        Public ID$
        Public CampoID$
        Public Token$
        Public Etiqueta$
        Public EtiquetaIzquierda As Boolean
        Public EtiquetaAncho%


        Public EsValorUTC As Boolean
        Public EsValorMultilinea As Boolean
        Public ElValorEstaModificado As Boolean

        Public IconoID As String

        Public X As Integer
        Public Y As Integer
        Public Alto As Integer
        Public Ancho As Integer

        Public Pestana$
        Public Formato As Integer
        Public FormatoNumerico As Integer
        Public EsEDU As Boolean
        Public Informacion As String



        Public Obligatorio As Boolean
        Public Bloqueado As Boolean
        Public Visible As Boolean
        Public RolCampo As Integer
        Public EsArchivo As Boolean
        Public Tipo%
        Public InputType As String
        Public InputStep As String
        Public ValorActual As String
        Public Aviso_ValorIncorrecto As String
        Public Aviso_Bloqueo As String


        Public valor_txt As String

        Public Overrides Function ToString() As String
            On Error Resume Next
            Return "API_Formulario_ControlC: " & Pestana & " > " & Etiqueta
        End Function


        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)

            _JObject = JsonObj

            ID = _JObject("id").STR
            CampoID = _JObject("idcampo").STR
            Token = _JObject("token").STR
            Etiqueta = _JObject("etiqueta").STR
            If _JObject("etizq") IsNot Nothing Then EtiquetaIzquierda = _JObject("etizq").BOOL
            If _JObject("etancho") IsNot Nothing Then EtiquetaAncho = _JObject("etancho").INT

            EsValorUTC = _JObject("esutc").BOOL
            EsValorMultilinea = _JObject("esmultilinea").BOOL
            ElValorEstaModificado = _JObject("modificado").BOOL

            IconoID = _JObject("icono").STR

            X = _JObject("x").INT
            Y = _JObject("y").INT
            Alto = _JObject("alto").INT
            Ancho = _JObject("ancho").INT


            Pestana = _JObject("pestana").STR
            Tipo = _JObject("tipo").INT
            Informacion = _JObject("informacion").STR
            Visible = _JObject("visible").BOOL



            If _JObject("formato") IsNot Nothing Then Formato = _JObject("formato").INT
            If _JObject("formatonum") IsNot Nothing Then FormatoNumerico = _JObject("formatonum").INT
            If _JObject("esedu") IsNot Nothing Then EsEDU = _JObject("esedu").BOOL
            If _JObject("obligatorio") IsNot Nothing Then Obligatorio = _JObject("obligatorio").BOOL
            If _JObject("bloqueado") IsNot Nothing Then Bloqueado = _JObject("bloqueado").BOOL
            If _JObject("rolcampo") IsNot Nothing Then RolCampo = _JObject("rolcampo").INT()
            If _JObject("esarchivo") IsNot Nothing Then EsArchivo = _JObject("esarchivo").BOOL
            If _JObject("inputtype") IsNot Nothing Then InputType = _JObject("inputtype").STR
            If _JObject("inputsetp") IsNot Nothing Then InputStep = _JObject("inputsetp").STR
            If _JObject("valor") IsNot Nothing Then ValorActual = _JObject("valor").STR
            If _JObject("estado_valorincorrecto") IsNot Nothing Then Aviso_ValorIncorrecto = _JObject("estado_valorincorrecto").STR
            If _JObject("estado_bloqueo") IsNot Nothing Then Aviso_Bloqueo = _JObject("estado_bloqueo").STR

            If _JObject("valor_txt") IsNot Nothing Then valor_txt = _JObject("valor_txt").STR


        End Sub

    End Class

End Class
