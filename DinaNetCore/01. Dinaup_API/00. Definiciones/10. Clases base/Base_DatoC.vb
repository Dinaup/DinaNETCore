Partial Public Class APID


    Public Interface IdentificadorRegistro
        Property ID As Guid
        Property Legible As String
        Property SeccionID As Guid
    End Interface


    Public MustInherit Class Base_DatoC
        Implements IdentificadorRegistro



        MustOverride Function ToDic() As Dic(Of String, String)
        MustOverride Sub CargarDatos(_Datos As Dic(Of String, String))

        Public Dinaup_Anotaciones_ComentariosInternos As List(Of DinaupAPI_AnotacionRegistroC)
        Public Dinaup_Anotaciones_ArchivosInternos As List(Of DinaupAPI_AnotacionRegistroC)


        Public Dinaup_Anotaciones_GaleriaPublica As List(Of DinaupAPI_AnotacionRegistroC)


        ''' <summary> 
        ''' Esta información puede ser redudante. 
        ''' Es la misma que aparecería en Dinaup_Anotaciones_GaleriaPublica pero únicamente las IDS.  
        ''' Esta funcionalidad existe porque es más rapido procesar únicamente IDs. 
        ''' (Para ello se requiere tener cacheado los datos de los archivos.)
        ''' </summary>
        Public Dinaup_Anotaciones_GaleriaPublica_ArchivosIDs As List(Of String)









        Public __Guardando As Boolean
        Public __AvisosOk As List(Of String)
        Public __AvisosError As List(Of String)






        Public Property Base__ID As Guid Implements IdentificadorRegistro.ID
        Public Property Base__Legible As String Implements IdentificadorRegistro.Legible
        Public Property Base__ImagenID As Guid
        Public Property Base__vEliminado As Boolean
        Public Property Base__SeccionID As Guid Implements IdentificadorRegistro.SeccionID



        Public Overrides Function ToString() As String
            Return Base__Legible
        End Function

        Public Sub CargaInterna(obj As HTTPRespuestaAPIC_Recibir_DatoC)


            Me.Base__ID = obj.Listador.HacerMagia("id").ToGuid
            Me.Base__Legible = obj.Listador.HacerMagia("nombre")
            Me.Base__ImagenID = obj.Listador.HacerMagia("__previa").ToGuid
            Me.Base__vEliminado = (obj.Listador.HacerMagia("eliminado") = "1")
            Me.Base__SeccionID = (obj.Listador.HacerMagia("plantillapid")).ToGuid



            Me.Dinaup_Anotaciones_ComentariosInternos = obj.Listador_Comentarios
            Me.Dinaup_Anotaciones_ArchivosInternos = obj.Listador_Archivos
            Me.Dinaup_Anotaciones_GaleriaPublica = obj.Listador_Galeria
            Me.Dinaup_Anotaciones_GaleriaPublica_ArchivosIDs = obj.Listador_ArchivosIDEnGaleria


        End Sub


    End Class
End Class
