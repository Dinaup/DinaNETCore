




Partial Public Class APID

    Partial Public Class ServidorDinaup_ConectorC






        Public Function Funcion_Archivos_Lista(Sesion As APID.DinaupSesionC, Busqueda As RecibirArchivosC) As HTTPRespuestaAPIC_ListaArchivoC


            Dim DAtos As New Specialized.NameValueCollection

            If Busqueda.Filtrar_SubidosPorAPI Then DAtos.Add("dinaup_archivo_soloapi", "1")
            If Busqueda.Filtrar_Extensiones.TieneDatos Then DAtos.Add("dinaup_archivo_extensiones", Busqueda.Filtrar_Extensiones.STRJoin(":"))
            If Busqueda.Filtrar_Nombres.TieneDatos Then
                For Each Actual In Busqueda.Filtrar_Nombres
                    If Actual.Contains(":") Then
                        Throw New Exception("No se permite el caracter : en el nombre de archivo a buscar. Código de error (E-2051)")
                    End If
                Next
                DAtos.Add("dinaup_archivo_nombres", Busqueda.Filtrar_Nombres.STRJoin(":"))
            End If

            If Busqueda.Filtrar_CRCs.TieneDatos Then
                For Each Actual In Busqueda.Filtrar_CRCs
                    If Actual.Contains(":") Then
                        Throw New Exception("No se permite el caracter : en el crc de archivo a buscar. Código de error (E-2052)")
                    End If
                Next
                DAtos.Add("dinaup_archivo_crcs", Busqueda.Filtrar_CRCs.STRJoin(":"))
            End If

            Dim R = Me.Http_EjecutarFuncionAPI_Bytes(Sesion, "listararchivo", DAtos, Sesion.UserAgent, Sesion.IP)
            Return New HTTPRespuestaAPIC_ListaArchivoC(New HTTP_RespuestaAPIC(System.Text.Encoding.UTF8.GetString(R.Contenido), Sesion))
        End Function









        Public Function Funcion_Archivos_Consultar(Sesion As APID.DinaupSesionC, archivo$, Tamano As TamanoE) As HTTPRespuestaAPIC_ArchivoC
            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_archivo", archivo)
            DAtos.Add("dinaup_archivo_tamano", CInt(Tamano).ToString)
            Dim R = Me.Http_EjecutarFuncionAPI_Bytes(Sesion, "archivo", DAtos, Sesion.UserAgent, Sesion.IP)
            Return New HTTPRespuestaAPIC_ArchivoC(New HTTP_RespuestaAPIC(System.Text.Encoding.UTF8.GetString(R.Contenido), Sesion))
        End Function


        'Public Function Funcion_Archivos_Subir_URL(URL$, NombreArchivo$) As HTTPRespuestaAPIC_ArchivoC
        '    Dim Sesion = New APID.DinaupSesionC
        '    Sesion.UserAgent = "?"
        '    Sesion.IP = "?"
        '    Sesion.ConexionServidor = Me
        '    Dim DAtos As New Specialized.NameValueCollection
        '    DAtos.Add("archivo", URL)
        '    DAtos.Add("archivonombre", NombreArchivo)
        '    Dim R = Me.Http_EjecutarFuncionAPI_Bytes(Sesion, "subirarchivo", DAtos, Sesion.UserAgent, Sesion.IP)
        '    Return New HTTPRespuestaAPIC_ArchivoC(New HTTP_RespuestaAPIC(System.Text.Encoding.UTF8.GetString(R.Contenido), Sesion))
        'End Function







        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Sesion"></param>
        ''' <param name="Direcciones">
        ''' El sistema intenterá recuperar el archivo de varias direcciones (por orden)
        ''' También es útil si se cambia el dominio o la dirección. Ya que el sistema detectará si alguna de las direcciones ya ha sido importada.
        ''' </param>
        ''' <param name="NombreArchivo$"></param>
        ''' <returns></returns>
        Public Function Funcion_Archivos_Subir_URL(Sesion As APID.DinaupSesionC, Direcciones() As String, NombreArchivo$) As HTTPRespuestaAPIC_ArchivoC


            If Direcciones.TieneDatos = False Then Throw New Exception("La URL no es válida.")



            For i = 0 To Direcciones.Length - 1


                Dim RutaActual = Direcciones(i)

                If RutaActual.Length < 4 Then
                    Throw New Exception("La ruta es demasiado corta. Código de error (E-2852)")
                End If


                If RutaActual.StartsWithIgnoreCase("http") Then
                    ', Ruta Válida, una dirección HTTP.  
                    Continue For
                End If

                If RutaActual.StartsWithIgnoreCase("c:\Dinaup\API_ArchivosSubidos\") Then
                    Continue For
                End If


                Throw New Exception("La ruta indicada no es válida. Código de error (E-2938).")



            Next



            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("archivo", Direcciones.STRJoin("{[¿~?]}"))
            DAtos.Add("archivonombre", NombreArchivo)
            Dim R = Me.Http_EjecutarFuncionAPI_Bytes(Sesion, "subirarchivo", DAtos, Sesion.UserAgent, Sesion.IP)
            Return New HTTPRespuestaAPIC_ArchivoC(New HTTP_RespuestaAPIC(System.Text.Encoding.UTF8.GetString(R.Contenido), Sesion))
        End Function
        Public Function Funcion_Archivos_Subir_URL(Sesion As APID.DinaupSesionC, URL$, NombreArchivo$) As HTTPRespuestaAPIC_ArchivoC

            If URL.StartsWith("http") = False Then Throw New Exception("La URL no es válida.")

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("archivo", URL)
            DAtos.Add("archivonombre", NombreArchivo)
            Dim R = Me.Http_EjecutarFuncionAPI_Bytes(Sesion, "subirarchivo", DAtos, Sesion.UserAgent, Sesion.IP)
            Return New HTTPRespuestaAPIC_ArchivoC(New HTTP_RespuestaAPIC(System.Text.Encoding.UTF8.GetString(R.Contenido), Sesion))
        End Function





        Public Function Funcion_Archivos_Subir(Sesion As APID.DinaupSesionC, Archivos As List(Of String)) As HTTPRespuestaAPIC_ArchivoC


            If Archivos.TieneDatos = False Then Throw New Exception("No se han detectado archivos a subir.")



            Dim ArchivosSubiendo As New List(Of SubirArchivoHTTPC)

            For Each Actual In Archivos
                ArchivosSubiendo.Add(New SubirArchivoHTTPC(ArchivosSubiendo.Count + 1, Actual))
            Next



            Dim DAtos As New Specialized.NameValueCollection
            Dim R = Me.Http_EjecutarFuncionAPI_SubirArchivoEnDisco(Sesion, "subirarchivo", DAtos, ArchivosSubiendo, Sesion.UserAgent, Sesion.IP)

            For Each Actual In ArchivosSubiendo
                Try
                    Actual.Destruir()
                Catch : End Try
            Next

            Return New HTTPRespuestaAPIC_ArchivoC(New HTTP_RespuestaAPIC(System.Text.Encoding.UTF8.GetString(R.Contenido), Sesion))
        End Function





        ''' <param name="RutaDeArchivo$">Ejemplo "MisArchivos/MiFoto.png""</param>
        Public Function Funcion_Archivos_SubirDesde_FTP(Sesion As APID.DinaupSesionC, IPOURL$, Puerto%, Usuario$, Contrasena$, RutaDeArchivo$) As HTTPRespuestaAPIC_ArchivoC
            If Ftp.ExisteFichero(IPOURL, Puerto, Usuario, Contrasena, RutaDeArchivo) = False Then Throw New Exception("El archivo indicado no existe en el servidor FPT.")
            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("ftp_ip", IPOURL)
            DAtos.Add("ftp_puerto", Puerto.ToString)
            DAtos.Add("ftp_usuario", Usuario$)
            DAtos.Add("ftp_contrasena", Contrasena)
            DAtos.Add("ftp_archivoruta", RutaDeArchivo)
            Dim R = Me.Http_EjecutarFuncionAPI_Bytes(Sesion, "subirarchivo", DAtos, Sesion.UserAgent, Sesion.IP)
            Return New HTTPRespuestaAPIC_ArchivoC(New HTTP_RespuestaAPIC(System.Text.Encoding.UTF8.GetString(R.Contenido), Sesion))
        End Function




    End Class



    Public Class DinaupAPI_ArchivoC


        Public ArchivoID As Guid
        Public Nombre$
        Public Archivo$
        Public Ruta$
        Public Extension$
        Public Alto%
        Public Ancho%
        Public CantidadDeComentarios%
        Public CRC$
        Public EsPublico As Boolean
        Public TamanoEnBytes As Integer
        Public CompatibleConVistaPrevia As Boolean
        Public Mime As String


        Sub New(obj As Newtonsoft.Json.Linq.JToken)
            ArchivoID = obj("id").STR.ToGuid
            Nombre = obj("nombre").STR
            Archivo = obj("archivo").STR
            Mime = obj("mime").STR
            Ruta = obj("ruta").STR
            Extension = obj("extension").STR
            Alto = obj("alto").STR.INT(0)
            Ancho = obj("ancho").STR.INT(0)
            CantidadDeComentarios = obj("comentarios").STR.INT(0)
            CRC = obj("crc").STR
            EsPublico = obj("publico").STR.BOOL
            TamanoEnBytes = obj("tamano").STR.INT(0)
            CompatibleConVistaPrevia = obj("vistaprevia").STR.BOOL
        End Sub


    End Class





    Public Class HTTPRespuestaAPIC_ListaArchivoC

        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)
        Public RespuestaGenerica As HTTP_RespuestaAPIC




        Public Informe As DinaupAPI_InformeC
        Public Listado As DinaupAPI_Informe_DatosC


        Public Archivo As New List(Of DinaupAPI_ArchivoC)


        Public ReadOnly Property Codigo%
            Get
                Return RespuestaGenerica.Codigo
            End Get
        End Property

        Public ReadOnly Property Ok As Boolean
            Get
                Return RespuestaGenerica.Ok
            End Get
        End Property

        Public ReadOnly Property Descripcion$
            Get
                Return RespuestaGenerica.Descripcion
            End Get
        End Property


        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror

            If Obj.Obj_Respuesta IsNot Nothing Then
                For Each Actual In Obj.Obj_Respuesta
                    Archivo.Add(New DinaupAPI_ArchivoC(Actual))
                Next
            End If

        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub
    End Class
    Public Class HTTPRespuestaAPIC_ArchivoC



        Public Informe As DinaupAPI_InformeC
        Public Listado As DinaupAPI_Informe_DatosC
        Public Archivo As New Dic(Of String, DinaupAPI_ArchivoC)
        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)
        Public RespuestaGenerica As HTTP_RespuestaAPIC


        Public ReadOnly Property Codigo%
            Get
                Return RespuestaGenerica.Codigo
            End Get
        End Property

        Public ReadOnly Property Ok As Boolean
            Get
                Return RespuestaGenerica.Ok
            End Get
        End Property

        Public ReadOnly Property Descripcion$
            Get
                Return RespuestaGenerica.Descripcion
            End Get
        End Property



        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror

            If Obj.Obj_Respuesta IsNot Nothing Then

                Archivo = New Dic(Of String, DinaupAPI_ArchivoC)


                If True Then

                    Dim ArchivoActual = Obj.Obj_Respuesta("archivo")
                    If ArchivoActual IsNot Nothing Then
                        Archivo.Add("archivo", New DinaupAPI_ArchivoC(ArchivoActual))
                    End If
                End If

                For i = 1 To 100
                    Dim ArchivoActual = Obj.Obj_Respuesta("archivo_" & i.ToString)
                    If ArchivoActual IsNot Nothing Then
                        Archivo.Add("archivo_" & i.ToString, New DinaupAPI_ArchivoC(ArchivoActual))
                    End If
                Next

            End If

        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub

    End Class



    Public Class RecibirArchivosC


        Public Filtrar_SubidosPorAPI As Boolean
        Public Filtrar_Extensiones As New List(Of String)
        ''' <summary>Extensión incluida. </summary>
        Public Filtrar_Nombres As New List(Of String)
        Public Filtrar_CRCs As New List(Of String)



        Sub New()

        End Sub

    End Class


    Public Enum TamanoE
        NoRecibir = 0
        Original = 1
        Alto100px = 2
        Alto300px = 3
        Alto32px = 4
    End Enum

End Class