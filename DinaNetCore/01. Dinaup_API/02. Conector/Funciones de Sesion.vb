Partial Public Class APID



    Partial Public Class ServidorDinaup_ConectorC





#Region "Cache"


        Private cache_sesiones As New Dic(Of Guid, APID.DinaupAPI_UsuarioSesionC)
        Private Sub cache_sesion_eliminar(SesionID As Guid)
            SyncLock Me.cache_sesiones
                Me.cache_sesiones.Remove(SesionID)
            End SyncLock
        End Sub
        Private Sub cache_sesion_actualizar(sesion As APID.DinaupAPI_UsuarioSesionC)
            If sesion Is Nothing Then Exit Sub
            If sesion.SesionID = Guid.Empty Then Exit Sub
            SyncLock Me.cache_sesiones
                ', Si hay muchas sesiones cacheadas, limpio el diccionario de las mas antiguas
                If Me.cache_sesiones.Count > 50000 Then
                    Try
                        Dim AConservar = Me.cache_sesiones.Values.OrderByDescending(Function(e) e.UltimaActividad).Take(10000).ToList
                        Me.cache_sesiones.Clear()
                        For Each Actual In AConservar
                            Me.cache_sesiones.Add(Actual.SesionID, Actual)
                        Next
                    Catch
                    End Try
                End If
                Me.cache_sesiones.HacerMagia(sesion.SesionID) = sesion
            End SyncLock
        End Sub



#End Region








        Public Async Function Funcion_Sesion_RecibirDetalles_CompatibleConCache(_SesionID As String, UserUserAgent$, UserIP$) As Task(Of DinaupSesionC)
            If _SesionID.EsGUID = False Then Return Nothing
            Dim SesionID = _SesionID.ToGuid
            If SesionID = Guid.Empty Then Return Nothing
            Dim R = Await Me.Funcion_Sesion_RecibirDetalles_CompatibleConCache(SesionID, UserUserAgent$, UserIP$)
            Dim Temp As New DinaupSesionC(Me, UserUserAgent, UserIP)
            Temp.DatosSesion = R
            Return Temp
        End Function


        ''' <summary>
        ''' Si está en cache devuelve el valor cacheado, si no lo está, entonces realiza petición al servidor.
        ''' </summary>
        Private Async Function Funcion_Sesion_RecibirDetalles_CompatibleConCache(SesionID As Guid, UserUserAgent$, UserIP$) As Task(Of DinaupAPI_UsuarioSesionC)

            If SesionID = Guid.Empty Then Return Nothing

            Dim Retornar As DinaupAPI_UsuarioSesionC

            ',  Se intenta obtener desde Cache 
            SyncLock Me.cache_sesiones
                Retornar = Me.cache_sesiones.HacerMagia(SesionID)
            End SyncLock


            ', Si no está en caché se le pide al servidor
            If Retornar Is Nothing Then
                Dim Detalles = Await Me.Funcion_Sesion_RecibirDetalles_Consulta(SesionID, UserUserAgent$, UserIP$)
                If Detalles IsNot Nothing Then
                    Retornar = Detalles.DetallesSesion
                    If Retornar IsNot Nothing AndAlso Retornar.SesionIniciada = False Then
                        Return Retornar
                    End If
                End If
            End If

            If Retornar IsNot Nothing Then
                Retornar.UltimaActividad = Date.UtcNow
            End If
            Return Retornar
        End Function






        ''' <summary>
        ''' Se recomienda llamar a la función compatible con caché.
        ''' </summary>
        Public Async Function Funcion_Sesion_RecibirDetalles_Consulta(SesionID As Guid, UserUserAgent$, UserIP$) As Task(Of HTTPRespuestaAPIC_SesionC)

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_sesion", SesionID.ToString)
            Dim R = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(Nothing, "sesion", DAtos, UserUserAgent, UserIP)
            Dim Retornar As New HTTPRespuestaAPIC_SesionC(R)
            If Retornar IsNot Nothing AndAlso Retornar.DetallesSesion IsNot Nothing Then
                Me.cache_sesion_actualizar(Retornar.DetallesSesion)
            End If
            Return Retornar
        End Function





        Public Async Function Funcion_Sesion_CambiarPassConCodigo(Codigo$, NuevaPass$, UserUserAgent$, UserIP$) As Task(Of HTTPRespuestaAPIC_CambiarPassConCodigoC)
            Dim Parametros As New Specialized.NameValueCollection
            Parametros.Add("dinaup_codigo", Codigo)
            Parametros.Add("dinaup_pass", "sha1:" & RecibirSHA1(NuevaPass))
            Dim R = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(Nothing, "cambiarpass", Parametros, UserUserAgent, UserIP)
            Dim Retornar As New HTTPRespuestaAPIC_CambiarPassConCodigoC(R)
            Return Retornar
        End Function


        Public Async Function Funcion_Sesion_CrearCuenta_Activar(Modelo As ModelosD.CrearCuenta_Activar_ModelC, UserUserAgent$, UserIP$) As Task(Of HTTPRespuestaAPIC_ActivarCuentaC)

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_cuentatemporalid", Modelo.CuentaTemporalID.STR)
            DAtos.Add("dinaup_codigo_activacion", Modelo.CodigoDeActivacion)
            Dim RespuestaX = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(New DinaupSesionC(Me, UserUserAgent, UserIP), "activarcuenta", DAtos, UserUserAgent, UserIP)
            Dim R = New HTTPRespuestaAPIC_ActivarCuentaC(RespuestaX)
            Return R


        End Function

        Public Async Function Funcion_Sesion_CrearCuenta(Modelo As ModelosD.CrearCuenta_ModelC, UserUserAgent$, UserIP$) As Task(Of HTTPRespuestaAPIC_CrearCuentaC)



            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_nombre", Modelo.Usuario_Nombre)
            DAtos.Add("dinaup_correo", Modelo.Usuario_Identificador)
            DAtos.Add("dinaup_empresa", Modelo.Contexto_EmpresaID.STR)
            DAtos.Add("dinaup_ubicacion", Modelo.Contexto_UbicacionID.STR)
            DAtos.Add("dinaup_pass", "sha1:" & RecibirSHA1(Modelo.Usuario_Contrasena))
            DAtos.Add("dinaup_mail_asunto", Modelo.EmailValidacion_Asunto)
            DAtos.Add("dinaup_mail_contenido", Modelo.EmailValidacion_Contenido)
            DAtos.Add("dinaup_mail_enviar", Modelo.EmailValidacion_Enviar.STR)

            Dim RespuestaX = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(New DinaupSesionC(Me, UserUserAgent, UserIP), "crearcuenta", DAtos, UserUserAgent, UserIP)
            Dim R = New HTTPRespuestaAPIC_CrearCuentaC(RespuestaX)
            Modelo.Retornar_SeHaEnviadoUnEmailCorrectamente = R.emailenviado.BOOL
            Modelo.Retornar_RegistroID_CuentaTemporal = R.cuentatemporalid.ToGuid
            Modelo.Retornar_CodigoDeValidacion = R.codigovalidacion
            Modelo.Retornar_CodigoDeValidacion_Requerido = R.codigovalidacion <> ""
            Return R

        End Function

        Public Async Function Funcion_Sesion_RecuperarContrasena(Modelo As ModelosD.RecuperarContrasena_ModeloC, UserUserAgent$, UserIP$) As Task(Of HTTPRespuestaAPIC_RecuperarPassC)

            If Modelo.EnviarEmail_Contenido.Contains("{codigo}") = False Then
                Throw New Exception("El contenido debe tener {codigo}.")
            End If

            Dim Parametros As New Specialized.NameValueCollection
            Parametros.Add("dinaup_correo", Modelo.Identificador)
            Parametros.Add("dinaup_empresa", Modelo.Contexto_EmpresaID.STR)
            Parametros.Add("dinaup_ubicacion", Modelo.Contexto_UbicacionID.STR)
            Parametros.Add("dinaup_mail_asunto", Modelo.EnviarEmail_Asunto)
            Parametros.Add("dinaup_mail_contenido", Modelo.EnviarEmail_Contenido)
            Parametros.Add("dinaup_mail_enviar", Modelo.EnviarEmail.STR)

            Dim R = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(Nothing, "recuperarpass", Parametros, UserUserAgent, UserIP)
            Dim Retornar As New HTTPRespuestaAPIC_RecuperarPassC(R)

            Modelo.Return_EmailEnviado = Retornar.Return_EmailEnviado
            Modelo.Return_Codigo = Retornar.Return_CodigoID
            Modelo.Return_CodigoGenerado = Retornar.Return_CodigoID <> ""

            Return Retornar

        End Function



        Public Async Function Funcion_Sesion_IniciarSesion(UsuarioLogin$, PassLogin$, UbicacionID$, EmpresaID$, UserUserAgent$, UserIP$) As Task(Of HTTPRespuestaAPIC_IniciarSesionC)

            Dim Parametros As New Specialized.NameValueCollection
            Parametros.Add("dinaup_usuario", UsuarioLogin)
            Parametros.Add("dinaup_contasena", RecibirSHA1(PassLogin))
            Parametros.Add("dinaup_ubicacion", UbicacionID)
            Parametros.Add("dinaup_empresa", EmpresaID)
            Dim RespuestaX = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(New DinaupSesionC(Me, UserUserAgent, UserIP), "iniciarsesion", Parametros, UserUserAgent, UserIP)

            Dim R = New HTTPRespuestaAPIC_IniciarSesionC(RespuestaX)
            If R.Respuesta = HTTPRespuestaAPIC_IniciarSesionC.InicioSesionRespuestaE.Ok Then
                If R IsNot Nothing AndAlso R.DetallesSesion IsNot Nothing Then
                    Me.cache_sesion_actualizar(R.DetallesSesion)
                End If
            End If
            Return R
        End Function



        Public Async Function Funcion_Sesion_CerrarSesion(SesionID As Guid, UserUserAgent$, UserIP$) As Task(Of HTTP_RespuestaAPIC)
            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_sesion", SesionID.ToString)
            Dim R = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(Nothing, "cerrarsesion", DAtos, UserUserAgent$, UserIP$)
            If R.Ok Then
                Me.cache_sesion_eliminar(SesionID)
            End If
            Return R
        End Function


        Public Async Function Funcion_Sesion_CambiarContrasena(SesionID As Guid, Pass1$, Pass2$, UserUserAgent$, UserIP$) As Task(Of HTTPRespuestaAPIC_SesionC)

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_sesion", SesionID.ToString)
            DAtos.Add("cambiarpass", "1")
            DAtos.Add("pass1", RecibirSHA1(Pass1))
            DAtos.Add("pass2", RecibirSHA1(Pass2))

            Dim R = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(Nothing, "sesion", DAtos, UserUserAgent$, UserIP$)
            Dim Retornar As New HTTPRespuestaAPIC_SesionC(R)

            If Retornar.DetallesSesion IsNot Nothing Then
                Me.cache_sesion_actualizar(Retornar.DetallesSesion)
            End If

            Return Retornar
        End Function


    End Class




    Public Class HTTPRespuestaAPIC_RecuperarPassC


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


        Public Return_CodigoID As String
        Public Return_RecuperadorID As String
        Public Return_EmailEnviado As Boolean

        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            Me.RespuestaGenerica = Obj
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror

            If Obj.Obj_Respuesta.NoEsNulo Then
                Return_CodigoID = Obj.Obj_Respuesta("codigo").STR
                Return_RecuperadorID = Obj.Obj_Respuesta("recuperacionid").STR
                Return_EmailEnviado = Return_CodigoID <> ""
            End If

        End Sub





        Sub New(Obj As HTTP_RespuestaAPIC)
            Iniciar(Obj)
        End Sub

    End Class



    Public Class HTTPRespuestaAPIC_ActivarCuentaC

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





        Public entidadid As String
        Public identificador As String

        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            Me.RespuestaGenerica = Obj
            Me.avisosok = Obj.avisosok
            Me.avisoserror = Obj.avisoserror


            If Obj.Obj_Respuesta.NoEsNulo Then
                entidadid = Obj.Obj_Respuesta("entidadid").STR
                identificador = Obj.Obj_Respuesta("identificador").STR
            End If

        End Sub


        Sub New(Obj As HTTP_RespuestaAPIC)
            Iniciar(Obj)
        End Sub


    End Class

    Public Class HTTPRespuestaAPIC_CrearCuentaC

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



        Public emailenviado As String
        Public codigovalidacion As String
        Public cuentatemporalid As String

        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            Me.RespuestaGenerica = Obj
            Me.avisosok = Obj.avisosok
            Me.avisoserror = Obj.avisoserror


            If Obj.Obj_Respuesta.NoEsNulo Then
                emailenviado = Obj.Obj_Respuesta("emailenviado").STR
                codigovalidacion = Obj.Obj_Respuesta("codigovalidacion").STR
                cuentatemporalid = Obj.Obj_Respuesta("cuentatemporalid").STR
            End If



        End Sub


        Sub New(Obj As HTTP_RespuestaAPIC)
            Iniciar(Obj)
        End Sub


    End Class
    Public Class HTTPRespuestaAPIC_CambiarPassConCodigoC

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
            Me.RespuestaGenerica = Obj
            Me.avisosok = Obj.avisosok
            Me.avisoserror = Obj.avisoserror

        End Sub


        Sub New(Obj As HTTP_RespuestaAPIC)
            Iniciar(Obj)
        End Sub


    End Class
    Public Class HTTPRespuestaAPIC_IniciarSesionC





            Public avisosok As List(Of String)
            Public avisoserror As List(Of String)
            Public Respuesta As InicioSesionRespuestaE
            Public IniciarSesion_Ubicaciones As New Dic(Of Guid, String)
            Public IniciarSesion_Empresas As New Dic(Of Guid, String)


            Public DetallesSesion As APID.DinaupAPI_UsuarioSesionC
            Public DinaupSesion As APID.DinaupSesionC


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

                Me.RespuestaGenerica = Obj

                avisosok = Obj.avisosok
                avisoserror = Obj.avisoserror

                If Obj.Codigo = 611 Then


                    Respuesta = InicioSesionRespuestaE.SeleccionarContexto

                    Dim Obj_Respuesta = Obj.Obj_Respuesta

                    If Obj_Respuesta IsNot Nothing Then


                        For Each Actual In Obj_Respuesta("ubicaciones")
                            IniciarSesion_Ubicaciones.Add(Actual("id").ToString.ToGuid, Actual("nombre").ToString)
                        Next


                        For Each Actual In Obj_Respuesta("empresas")
                            IniciarSesion_Empresas.Add(Actual("id").ToString.ToGuid, Actual("nombre").ToString)
                        Next


                    End If

                ElseIf Obj.Ok Then


                    DetallesSesion = New APID.DinaupAPI_UsuarioSesionC(Obj.Obj_Respuesta)
                    DinaupSesion = New DinaupSesionC(Obj.DinapSesion.ConexionServidor, DetallesSesion.SesionUserAgent, DetallesSesion.SesionIP)
                    DinaupSesion.DatosSesion = DetallesSesion
                    Respuesta = InicioSesionRespuestaE.Ok

                Else

                    Respuesta = InicioSesionRespuestaE.UnError

                    If avisoserror Is Nothing Then
                        avisoserror = New List(Of String)
                    End If
                    avisoserror.Add(Obj.Descripcion)

                End If


            End Sub





            Public Enum InicioSesionRespuestaE
                Indefinido = 0
                SeleccionarContexto = 1
                Ok = 2
                UnError = 3
            End Enum

            Sub New(Obj As HTTP_RespuestaAPIC)
                Iniciar(Obj)
            End Sub

        End Class


        Public Class HTTPRespuestaAPIC_SesionC





        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)

        Public DetallesSesion As APID.DinaupAPI_UsuarioSesionC
        Public OriginalResponse As HTTP_RespuestaAPIC


        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror
            If Obj.Obj_Respuesta IsNot Nothing Then
                DetallesSesion = New APID.DinaupAPI_UsuarioSesionC(Obj.Obj_Respuesta)
            End If
        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            OriginalResponse = Obj
            Iniciar(Obj)
        End Sub

    End Class


End Class
