

Partial Public Class APID






    Public Class DinaupAPI_CampoC




        Public Es_ClavePrimaria_ID As Boolean
        Public Es_IndicaidorDeEliminado As Boolean





        Public KeyWord$



        Public Etiqueta$
        Public Formato As TipoCamposSQLServerE
        Public RolCampo As RolCampoE
        Public Obligatorio As Boolean


        Public AceptaSegundos As Boolean
        Public AceptaCero As Boolean
        Public AceptaPositivos As Boolean
        Public AceptaNegativos As Boolean

        Public SeccionRelacionadaID As String
        Public SecionRelacionadaInstancia As DinaupAPI_SeccionC
        Public CantidadDecimales%

        Public MotivoBloqueo As String
        Public EsMultilinea As Boolean
        Public EsEDU As Boolean


        Public Oculta As Boolean
        Public Visible As Boolean


        Public EsUTC As Boolean


        Public Rec As System.Drawing.Rectangle

        Public SoloValoresPredefinido As Boolean
        Public Predefinidos_Valores As List(Of Integer)
        Public Predefinidos_Textos As List(Of String)
        Public Predefinidos_Iconos As List(Of String)

        Public Filtro As DinaupAPI_VariableC



        Public NetType As System.Type


        Public Function RecibirLegible(Valor$) As String
            If Predefinidos_Textos.TieneDatos Then
                Return Predefinidos_Textos.HacerMagiaArray(Valor.INT(0))
            End If
            Return Valor
        End Function
        Public Function RecibirIcono(Valor$) As String
            If Predefinidos_Iconos.TieneDatos Then
                Return Predefinidos_Iconos.HacerMagiaArray(Valor.INT(0))
            End If
            Return Valor
        End Function



        Public ItemsDesplegable As List(Of DesplegableItemC)


        Sub New(JsonObj As String)
            Dim ElObj As Newtonsoft.Json.Linq.JToken = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(JsonObj), Newtonsoft.Json.Linq.JToken)
            iniciar(ElObj)
        End Sub
        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)
            iniciar(JsonObj)
        End Sub
        Sub iniciar(JsonObj As Newtonsoft.Json.Linq.JToken)


            Me.Es_ClavePrimaria_ID = JsonObj("esid").BOOL
            Me.Es_IndicaidorDeEliminado = JsonObj("eseliminado").BOOL
            Me.KeyWord = JsonObj("keyword").STR
            Me.Etiqueta = JsonObj("etiqueta").STR
            Me.Oculta = JsonObj("oculta").BOOL
            Me.Visible = Not Me.Oculta

            Me.Formato = CType(JsonObj("formato").INT, TipoCamposSQLServerE)
            Me.EsEDU = JsonObj("porubicacion").BOOL
            Me.SeccionRelacionadaID = JsonObj("seccionrelacionadaid").STR
            Me.RolCampo = CType(JsonObj("rol").INT, RolCampoE)


            Me.CantidadDecimales = JsonObj("decimales").INT()
            Me.EsMultilinea = JsonObj("multilinea").BOOL
            Me.Obligatorio = JsonObj("obligatorio").BOOL
            Me.MotivoBloqueo = JsonObj("motivobloqueo").STR
            Me.EsUTC = JsonObj("esutc").BOOL
            Me.AceptaSegundos = JsonObj("aceptasegundos").BOOL
            Me.AceptaCero = JsonObj("aceptacero").BOOL
            Me.AceptaPositivos = JsonObj("aceptapositivos").BOOL
            Me.AceptaNegativos = JsonObj("aceptanegativos").BOOL
            Me.SoloValoresPredefinido = JsonObj("solovalorespredefinidos").BOOL




            If JsonObj("predefinidos_valores") IsNot Nothing AndAlso JsonObj("predefinidos_valores").Count > 0 Then
                    Me.Predefinidos_Valores = New List(Of Integer)
                    For Each Actual In JsonObj("predefinidos_valores")
                        Me.Predefinidos_Valores.Add(Actual.INT)
                    Next
                End If



                If JsonObj("predefinidos_textos") IsNot Nothing AndAlso JsonObj("predefinidos_textos").Count > 0 Then
                    Me.Predefinidos_Textos = New List(Of String)
                    For Each Actual In JsonObj("predefinidos_textos")
                        Me.Predefinidos_Textos.Add(Actual.STR)
                    Next
                End If


            If JsonObj("predefinidos_iconos") IsNot Nothing AndAlso JsonObj("predefinidos_iconos").Count > 0 Then
                Me.Predefinidos_Iconos = New List(Of String)
                For Each Actual In JsonObj("predefinidos_iconos")
                    Me.Predefinidos_Iconos.Add(Actual.ToString)
                Next
            End If




            If Me.Predefinidos_Valores.TieneDatos Then

                Dim CreandoItemsDespItemsDesplegable As New List(Of DesplegableItemC)
                For i = 0 To Me.Predefinidos_Valores.Count - 1
                    Dim X = New DesplegableItemC
                    X.Texto = Me.Predefinidos_Textos(i).STR
                    X.ID = Me.Predefinidos_Valores(i).STR.INT(0)
                    If Predefinidos_Iconos.TieneDatos Then
                        X.Icono = Predefinidos_Iconos.HacerMagiaArray(i)
                    End If

                    If X.ID = 0 AndAlso X.Texto.EqualsIgnoreCase("indefinido") Then
                        X.Texto = ""
                    End If
                    CreandoItemsDespItemsDesplegable.Add(X)
                Next
                Me.ItemsDesplegable = CreandoItemsDespItemsDesplegable

            End If



            If JsonObj("seccionrelacionada") IsNot Nothing AndAlso JsonObj("seccionrelacionada").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Me.SecionRelacionadaInstancia = New DinaupAPI_SeccionC(JsonObj("seccionrelacionada"))
            End If


            Filtro = New DinaupAPI_VariableC(JsonObj("filtro"))

            Select Case Me.Formato
                Case TipoCamposSQLServerE.Indefinido
                    Me.NetType = GetType(String)
                Case TipoCamposSQLServerE.Bool
                    Me.NetType = GetType(Boolean)
                Case TipoCamposSQLServerE.Entero
                    Me.NetType = GetType(Integer)
                Case TipoCamposSQLServerE.Doble
                    Me.NetType = GetType(Decimal)
                Case TipoCamposSQLServerE.Texto
                    Me.NetType = GetType(String)
                Case TipoCamposSQLServerE.FechaYHora
                    Me.NetType = GetType(DateTime)
                Case TipoCamposSQLServerE.Fecha
                    Me.NetType = GetType(Date)
                Case TipoCamposSQLServerE.Hora
                    Me.NetType = GetType(System.TimeSpan)
                Case TipoCamposSQLServerE.Guid
                    Me.NetType = GetType(System.Guid)
                Case TipoCamposSQLServerE.ImgBase64Obsoleto
                    Me.NetType = GetType(String)
                Case TipoCamposSQLServerE.Boton
                    Me.NetType = GetType(String)
            End Select

        End Sub


    End Class

End Class
Public Class DesplegableItemC

    Public ID As Integer
    Public Texto$
    Public Icono$


    Sub New()
    End Sub

End Class
