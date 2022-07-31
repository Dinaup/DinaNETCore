

Partial Public Class APID


    Public Class DinaupAPI_Informe_DatosC

        Public PuedeAcceder As Boolean
        Public PuedeAgregar As List(Of DinaupAPI_SeccionC)
        Public ColumnasEtiquetas As List(Of String)
        Public Columnas As List(Of DinaupAPI_CampoC)


        Public TotalPaginas As Integer
        Public ModoComprimido As Boolean
        Public Titulo$
        Public SeccionID$
        Public Seccion As DinaupAPI_SeccionC
        Public Cantidad%
        Public ResultadosPorPagina%
        Public Pagina%
        Public Totalresultados%
        Public Busqueda$
        Public Avisos As New List(Of String)

        Public UbicacionDatos As APID.DinaupAPI_UbicacionC
        Public EmpresaDatos As APID.DinaupAPI_EmpresaC

        Public Filas As Newtonsoft.Json.Linq.JToken


        Public Archivos As Dic(Of String, DinaupAPI_ArchivoC)


        Public Iterator Function FilasEnDic() As IEnumerable(Of Dic(Of String, String))



            For Each Actual As Newtonsoft.Json.Linq.JToken In Filas

                Dim Retornar As New Dic(Of String, String)

                For Each ColActual In Columnas
                    Retornar.Add(ColActual.KeyWord, Actual(ColActual.KeyWord).ToString)
                Next


                Yield Retornar

            Next

        End Function


        Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)


            ModoComprimido = JsonObj("modominimo").BOOL
            Titulo = JsonObj("titulo").STR
            SeccionID = JsonObj("seccionid").STR
            If JsonObj("seccion") IsNot Nothing AndAlso JsonObj("seccion").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Seccion = New DinaupAPI_SeccionC(JsonObj("seccion"))
            End If
            If JsonObj("avisos") IsNot Nothing AndAlso JsonObj("avisos").Type <> Newtonsoft.Json.Linq.JTokenType.Null AndAlso JsonObj("avisos").Count > 0 Then
                For Each Actual In JsonObj("avisos")
                    Me.Avisos.Add(Actual.ToString)
                Next
            End If
            Cantidad = JsonObj("cantidad").INT
            ResultadosPorPagina = JsonObj("rpp").INT
            Pagina = JsonObj("pagina").INT
            Totalresultados = JsonObj("totalresultados").INT
            Busqueda = JsonObj("busqueda").STR
            TotalPaginas = JsonObj("totalpaginas").INT
            PuedeAcceder = JsonObj("puedeacceder").BOOL


            If JsonObj("ubicacion") IsNot Nothing AndAlso JsonObj("ubicacion").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Me.UbicacionDatos = New DinaupAPI_UbicacionC(JsonObj("ubicacion"))
            End If

            If JsonObj("empresa") IsNot Nothing AndAlso JsonObj("empresa").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                Me.EmpresaDatos = New DinaupAPI_EmpresaC(JsonObj("empresa"))
            End If



            If JsonObj("archivos") IsNot Nothing AndAlso JsonObj("archivos").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then


                Me.Archivos = New Dic(Of String, DinaupAPI_ArchivoC)
                For Each Actual In JsonObj("archivos")
                    Dim ArchivoXR = New DinaupAPI_ArchivoC(Actual)
                    Me.Archivos.Add(ArchivoXR.ArchivoID.ToString(), ArchivoXR)
                Next
            End If


            Filas = JsonObj("elementos")

            Columnas = New List(Of DinaupAPI_CampoC)
            ColumnasEtiquetas = New List(Of String)

            For Each Actual In JsonObj("columnas")
                ColumnasEtiquetas.Add(Actual.ToString)
                Columnas.Add(New DinaupAPI_CampoC(JsonObj("campos")(Actual.ToString)))
            Next




        End Sub


    End Class



End Class

