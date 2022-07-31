

Partial Public Class APID

    Public Class DinaupAPI_ImportacionProcesableC
        Implements ToJSoneableI


        Public dinaup_listador_galeria As List(Of JSONBuildC)
        Public dinaup_listador_archivos As List(Of JSONBuildC)
        Public dinaup_listador_comentarios As List(Of JSONBuildC)

        Public dinaup_listador_datos As New Dic(Of String, String)
        Public dinaup_lista_datos As New List(Of Dic(Of String, String))



        <Obsolete>
        Public Sub AgregarAnotacion(Anotacion As APID.UtilidadesD.Anotacion_ArchivoDatoC, EnGaleria As Boolean, EnArchivos As Boolean, EnComentarios As Boolean)
            AgregarAnotacion(Anotacion.ToJSON, EnGaleria, EnArchivos, EnComentarios)
        End Sub

        <Obsolete>
        Public Sub AgregarAnotacion(Anotacion As APID.UtilidadesD.Anotacion_ArchivoDesdeURLC, EnGaleria As Boolean, EnArchivos As Boolean, EnComentarios As Boolean)
            AgregarAnotacion(Anotacion.ToJSON, EnGaleria, EnArchivos, EnComentarios)
        End Sub

        <Obsolete>
        Public Sub AgregarAnotacion(Anotacion As APID.UtilidadesD.Anotacion_TextoC, EnGaleria As Boolean, EnArchivos As Boolean, EnComentarios As Boolean)
            AgregarAnotacion(Anotacion.ToJSON, EnGaleria, EnArchivos, EnComentarios)
        End Sub
        <Obsolete>
        Public Sub AgregarAnotacion(Anotacion As APID.UtilidadesD.Anotacion_ArchivoIDC, EnGaleria As Boolean, EnArchivos As Boolean, EnComentarios As Boolean)
            AgregarAnotacion(Anotacion.ToJSON, EnGaleria, EnArchivos, EnComentarios)
        End Sub


        <Obsolete>
        Sub AgregarAnotacion(Anotacion As JSONBuildC, EnGaleria As Boolean, EnArchivos As Boolean, EnComentarios As Boolean)
            If EnGaleria AndAlso dinaup_listador_galeria Is Nothing Then dinaup_listador_galeria = New List(Of JSONBuildC)
            If EnArchivos AndAlso dinaup_listador_archivos Is Nothing Then dinaup_listador_archivos = New List(Of JSONBuildC)
            If EnComentarios AndAlso dinaup_listador_comentarios Is Nothing Then dinaup_listador_comentarios = New List(Of JSONBuildC)
            If EnGaleria Then dinaup_listador_galeria.Add(Anotacion)
            If EnArchivos Then dinaup_listador_archivos.Add(Anotacion)
            If EnComentarios Then dinaup_listador_comentarios.Add(Anotacion)
        End Sub



        Public Function ToJSoneableI_ToJSON() As JSONBuildC Implements ToJSoneableI.ToJSON



            Dim ListadorDIC As New JSONBuildC
            ListadorDIC.Add("{")
            For Each Actual In dinaup_listador_datos
                ListadorDIC.Add(Actual.Key, Actual.Value)
            Next
            ListadorDIC.Add("}")

            Dim Lista As New List(Of JSONBuildC)
            If dinaup_lista_datos.TieneDatos Then
                For Each Actual In dinaup_lista_datos
                    Dim ElementoNuevo As New JSONBuildC
                    ElementoNuevo.Add("{")
                    For Each VListaActual In Actual
                        ElementoNuevo.Add(VListaActual.Key, VListaActual.Value)
                    Next
                    ElementoNuevo.Add("}")
                    Lista.Add(ElementoNuevo)
                Next
            End If

            Dim R As New JSONBuildC
            R.Add("{")
            R.Add("dinaup_listador_datos", ListadorDIC)
            R.Add("dinaup_lista_datos", Lista)
            R.Add("dinaup_listador_galeria", dinaup_listador_galeria)
            R.Add("dinaup_listador_archivos", dinaup_listador_archivos)
            R.Add("dinaup_listador_comentarios", dinaup_listador_comentarios)
            R.Add("}")
            Return R

        End Function
        Sub New()
        End Sub
        Sub New(_ID As String)
            dinaup_listador_datos.Add("id", _ID)
        End Sub

    End Class
End Class
