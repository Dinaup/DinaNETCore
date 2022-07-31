Imports System.Collections.Specialized
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Text

Partial Public Class APID

    Public Class ServidorDinaup_ConectorC




        Public ConexionDisponible_FechaConfirmacion As Date






        Public Configuracion_APIKey As String
        Public Configuracion_ServidorIPODominion As String
        Public Configuracion_Puerto As String


        Public s3_Archivos_EndPointURL$ = ""
        Public s3_AuthenticationRegion As String = "de"
        Public s3_Archivos_access_key_id$ = ""
        Public s3_Archivos_secret_access_key$ = ""



        Public s3_Archivos_SignatureVersion$ = "4"
        Public s3_UseHttp As Boolean = True





        Public DescripcionDeErrorDeConexion$ = "Pendiente de iniciar"




        ''' <summary>
        ''' Indica que el servidor está listo para recibir peticiones.
        ''' Bas de datos iniciada, licencia válida....
        ''' </summary>
        Public ConexionDisponible As Boolean

        ''' <summary>
        ''' Indica si la dirección del servidor es correcta y el servidor responde.
        ''' Que sea válidao no significa que esté disponible.
        ''' Un servidor puede estar iniciándose, licencia desactivada, iniciando base de datos...
        ''' En esos casos el servidor responde, pero siempre responde un mensaje de bloqueo.
        ''' </summary>
        Public ConexionValida As Boolean


        Public ReadOnly Property URLAPI$
            Get
                Return "http://" & Configuracion_ServidorIPODominion & ":" & Configuracion_Puerto
            End Get
        End Property



        Public Function PING() As String
            Try
                Using WC As New Net.WebClient
                    WC.Encoding = System.Text.Encoding.UTF8
                    Dim X = WC.DownloadString(URLAPI & "/?dinaup_ping=1")


                    If X = "" Then

                        ConfirmarConexion(False, True)

                    ElseIf X <> "" AndAlso X.Contains("{") Then

                        Dim MotivoNoValida = "?"
                        Try
                            Dim respuesta As New APID.HTTP_RespuestaAPIC(X, Nothing)
                            If respuesta.Codigo = 201 Then
                                MotivoNoValida = respuesta.Descripcion
                            End If
                        Catch
                        End Try
                        ConfirmarConexion(False, True, MotivoNoValida)

                    ElseIf X.Trim.StartsWith("PONG") Then
                        ConfirmarConexion(X.Trim.StartsWith("PONG"), True)
                    Else
                        ConfirmarConexion(False, True)
                    End If


                    Return X
                End Using
            Catch ex As Exception
                ConfirmarConexion(False, False)
            End Try
            Return Nothing
        End Function

        Public Sub ConfirmarConexion(_Disponible As Boolean, _Valida As Boolean, Optional _MotivoNoValida As String = "")
            DescripcionDeErrorDeConexion = _MotivoNoValida
            ConexionDisponible = _Disponible
            ConexionValida = _Valida
            ConexionDisponible_FechaConfirmacion = Date.UtcNow
        End Sub








        Public Sub Iniciar(_Configuracion_APIKey$, _Configuracion_ServidorIPODominion$, _Configuracion_Puerto$)
            Me.Configuracion_APIKey = _Configuracion_APIKey
            Me.Configuracion_ServidorIPODominion = _Configuracion_ServidorIPODominion
            Me.Configuracion_Puerto = _Configuracion_Puerto
            Me.PING()
        End Sub



        Public Sub Iniciar(Optional _RutaArchivoDeConfiguracion$ = "c:\Dinaup\automatizaciones\config.ini")



            If IO.File.Exists(_RutaArchivoDeConfiguracion) = False Then
                DescripcionDeErrorDeConexion = "No se ha detectado el archivo de configuración. Código de error (E-4685)"
                Exit Sub
            End If


            Dim DinaApiEncontrado As Boolean
            For Each Contenido In IO.File.ReadAllLines(_RutaArchivoDeConfiguracion)
                If Contenido = "[dinaupapi]" Then
                    DinaApiEncontrado = True
                ElseIf Contenido.StartsWith("[") Then
                    DinaApiEncontrado = False
                End If

                If DinaApiEncontrado AndAlso Contenido.Contains("=") Then
                    Dim Sep = Contenido.Split("="c)
                    If Sep.Length = 2 Then
                        Select Case Sep(0).Trim.ToLower
                            Case "port" : Configuracion_Puerto = Sep(1)
                            Case "host" : Configuracion_ServidorIPODominion = Sep(1)
                            Case "apikey" : Configuracion_APIKey = Sep(1)
                            Case "s3_access_key_id" : s3_Archivos_access_key_id = Sep(1)
                            Case "s3_secret_access_key" : s3_Archivos_secret_access_key = Sep(1)
                            Case "s3_endpoint" : s3_Archivos_EndPointURL = Sep(1)
                            Case "s3_region" : s3_AuthenticationRegion = Sep(1)
                        End Select
                    End If
                End If
            Next
            Me.PING()
        End Sub








        Public Function Http_EjecutarFuncionAPI(AutoLoginUser As String, Funcion$, Parametros As System.Collections.Specialized.NameValueCollection, IP$, UserAgen$) As HTTP_RespuestaAPIC
            If Parametros Is Nothing Then Parametros = New Specialized.NameValueCollection
            Parametros.Remove("dinaup_sesion")
            Parametros.Remove("dinaup_usuario")
            Parametros.Add("dinaup_sesion", Configuracion_APIKey)
            Parametros.Add("dinaup_usuario", AutoLoginUser)
            Return Http_EjecutarFuncionAPI_JSON(Nothing, Funcion, Parametros, IP, UserAgen)
        End Function
        Public Async Function Http_EjecutarFuncionAPI_JSON_Asyn(UsuarioSesion As DinaupSesionC, Funcion$, Parametros As System.Collections.Specialized.NameValueCollection, UserAgen$, IP$) As Task(Of HTTP_RespuestaAPIC)






            If Me.ConexionValida = False Then
                Return New HTTP_RespuestaAPIC("La conexión no es válida.")
            ElseIf Me.ConexionDisponible = False Then
                Return New HTTP_RespuestaAPIC("La conexión no está disponible: " & DescripcionDeErrorDeConexion)
            End If





            If UsuarioSesion IsNot Nothing AndAlso UsuarioSesion.DatosSesion IsNot Nothing Then

                Parametros.Add("dinaup_sesion", UsuarioSesion.DatosSesion.SesionID.ToString)
                Parametros.Add("dinaup_ubicacion", UsuarioSesion.DatosSesion.Ubicacion.Item1.ToString)
                Parametros.Add("dinaup_empresa", UsuarioSesion.DatosSesion.Empresa.Item1.ToString)

            End If



            Parametros.Add("dinaup_funcionapi", Funcion)
            Parametros.Add("dinaup_apikey", Configuracion_APIKey)
            Parametros.Add("dinaup_usuarioip", IP)
            Parametros.Add("dinaup_usuarionavegador", UserAgen)


            Using WC As New Net.WebClient
                WC.Encoding = System.Text.Encoding.UTF8

                WC.Headers.Add("Accept-Encoding", "gzip")

                Dim Respuesta As String = ""

                Try

                    Dim Datos = Await WC.UploadValuesTaskAsync(URLAPI, Parametros)

                    Dim LaRespuestaEstaComprimida As Boolean = False
                    For Each Actual In WC.ResponseHeaders
                        If Actual IsNot Nothing AndAlso Actual.ToString.EqualsIgnoreCase("content-encoding") Then
                            LaRespuestaEstaComprimida = WC.ResponseHeaders(Actual.ToString).Contains("gzip")
                        End If
                    Next
                    If LaRespuestaEstaComprimida Then
                        Datos = Datos.Gzip_Decompress()
                    End If

                    Respuesta = WC.Encoding.GetString(Datos)
                    Me.ConfirmarConexion(True, True)
                Catch
                End Try

                If Respuesta = "" Then
                    Return New HTTP_RespuestaAPIC("", UsuarioSesion)
                Else
                    Return New HTTP_RespuestaAPIC(Respuesta, UsuarioSesion)
                End If
            End Using


        End Function
        Public Function Http_EjecutarFuncionAPI_JSON(UsuarioSesion As DinaupSesionC, Funcion$, Parametros As System.Collections.Specialized.NameValueCollection, UserAgen$, IP$) As HTTP_RespuestaAPIC
            Return Task.Run(Function()
                                Return Http_EjecutarFuncionAPI_JSON_Asyn(UsuarioSesion, Funcion, Parametros, UserAgen, IP)
                            End Function).Result
        End Function







        Private Function Http_EjecutarFuncionAPI_SubirArchivoEnDisco(DinaupSesion As APID.DinaupSesionC, Funcion$,
                                                           Datos_POST_Parametros As System.Collections.Specialized.NameValueCollection,
                                                           ArchivosASubir As List(Of SubirArchivoHTTPC),
                                                           UserAgen$, IP$) As HTTP_RespuestaBytesC





            If DinaupSesion IsNot Nothing AndAlso DinaupSesion.DatosSesion IsNot Nothing Then
                Datos_POST_Parametros.Add("dinaup_sesion", DinaupSesion.DatosSesion.SesionID.ToString)
                Datos_POST_Parametros.Add("dinaup_ubicacion", DinaupSesion.DatosSesion.Ubicacion.Item1.ToString)
                Datos_POST_Parametros.Add("dinaup_empresa", DinaupSesion.DatosSesion.Empresa.Item1.ToString)
            End If
            Datos_POST_Parametros.Add("dinaup_funcionapi", Funcion)
            Datos_POST_Parametros.Add("dinaup_apikey", Configuracion_APIKey)
            Datos_POST_Parametros.Add("dinaup_usuarioip", IP)
            Datos_POST_Parametros.Add("dinaup_usuarionavegador", UserAgen)



            Dim Retornar As New HTTP_RespuestaBytesC



            Dim request = WebRequest.Create(URLAPI)
            request.Method = "POST"
            Dim boundary = "---------------------------" & DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo)
            request.ContentType = "multipart/form-data; boundary=" & boundary
            boundary = "--" & boundary

            Using requestStream = request.GetRequestStream()

                For Each PostKeyActual As String In Datos_POST_Parametros.Keys
                    Dim buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine)
                    requestStream.Write(buffer, 0, buffer.Length)
                    buffer = Encoding.ASCII.GetBytes(String.Format("Content-Disposition: form-data; name=""{0}""{1}{1}", PostKeyActual, Environment.NewLine))
                    requestStream.Write(buffer, 0, buffer.Length)
                    buffer = Encoding.UTF8.GetBytes(Datos_POST_Parametros(PostKeyActual) + Environment.NewLine)
                    requestStream.Write(buffer, 0, buffer.Length)
                Next

                For Each ArchivoActual In ArchivosASubir
                    Dim buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine)
                    requestStream.Write(buffer, 0, buffer.Length)
                    buffer = Encoding.UTF8.GetBytes(String.Format("Content-Disposition: form-data; name=""{0}""; filename=""{1}""{2}", ArchivoActual.Name, ArchivoActual.Filename, Environment.NewLine))
                    requestStream.Write(buffer, 0, buffer.Length)
                    buffer = Encoding.ASCII.GetBytes(String.Format("Content-Type: {0}{1}{1}", ArchivoActual.ContentType, Environment.NewLine))
                    requestStream.Write(buffer, 0, buffer.Length)
                    ArchivoActual.Stream.CopyTo(requestStream)
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine)
                    requestStream.Write(buffer, 0, buffer.Length)
                Next

                Dim boundaryBuffer = Encoding.ASCII.GetBytes(boundary & "--")
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length)
            End Using


            Using response = request.GetResponse()
                Retornar.Headers = response.Headers
                Using responseStream = response.GetResponseStream()
                    Using stream = New MemoryStream()
                        responseStream.CopyTo(stream)
                        Retornar.Contenido = stream.ToArray()
                    End Using
                End Using
            End Using


            Return Retornar
        End Function



        Public Class SubirArchivoHTTPC



            Public Sub New(ID%, actual$)
                ContentType = "application/octet-stream"
                Me.Name = "archivo_" & ID.ToString
                Me.Filename = IO.Path.GetFileName(actual)
                Me.ContentType = GetMimeType(actual)
                Me.Stream = New IO.FileStream(actual, FileMode.Open)
            End Sub

            Public Name As String
            Public Filename As String
            Public ContentType As String
            Public Stream As IO.FileStream

            Sub Destruir()
                On Error Resume Next
                If Stream IsNot Nothing Then
                    Stream.Close()
                    Stream.Dispose()
                    Stream = Nothing
                End If
            End Sub
        End Class


        Public Function Http_EjecutarFuncionAPI_Bytes(DinaupSesion As APID.DinaupSesionC, Funcion$, Parametros As System.Collections.Specialized.NameValueCollection, UserAgen$, IP$) As HTTP_RespuestaBytesC


            If DinaupSesion IsNot Nothing AndAlso DinaupSesion.DatosSesion IsNot Nothing Then
                Parametros.Add("dinaup_sesion", DinaupSesion.DatosSesion.SesionID.ToString)
                Parametros.Add("dinaup_ubicacion", DinaupSesion.DatosSesion.Ubicacion.Item1.ToString)
                Parametros.Add("dinaup_empresa", DinaupSesion.DatosSesion.Empresa.Item1.ToString)
            End If
            Parametros.Add("dinaup_funcionapi", Funcion)
            Parametros.Add("dinaup_apikey", Configuracion_APIKey)
            Parametros.Add("dinaup_usuarioip", IP)
            Parametros.Add("dinaup_usuarionavegador", UserAgen)


            Dim Retornar As New HTTP_RespuestaBytesC

            Dim R As Byte()
            Using WC As New Net.WebClient
                WC.Encoding = System.Text.Encoding.UTF8
                Try
                    R = WC.UploadValues(URLAPI, Parametros)
                    Me.ConfirmarConexion(True, True)
                    Retornar.Contenido = R
                    Retornar.Headers = WC.ResponseHeaders
                Catch
                End Try
            End Using

            Return Retornar
        End Function






#Region "Inicio"
#End Region


        Sub New()
        End Sub

        Sub New(_RutaArchivoDeConfiguracion$)
            Iniciar(_RutaArchivoDeConfiguracion)
        End Sub

        Sub New(_Configuracion_APIKey$, _Configuracion_ServidorIPODominion$, _Configuracion_Puerto$)
            Iniciar(_Configuracion_APIKey, _Configuracion_ServidorIPODominion, _Configuracion_Puerto)
        End Sub

    End Class


End Class

