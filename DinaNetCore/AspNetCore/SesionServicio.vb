Imports System.Web
Imports DinaNETCore.APID

Partial Public Class ASP_NETD


    Public Class SesionServicio






        Public Event SesionCambiada()


        Sub New()

        End Sub





        Public Sub CerrarSesion(Optional coockiex As ICookie = Nothing)


            Me.DinaupSesionIDCoockie = ""
            Me.Detectarsesion()


        End Sub


        Public Async Function IniciarSesion(param As DinaNETCore.ModelosD.IniciarSesion_ModeloC, Optional coockiex As ICookie = Nothing) As Task(Of HTTPRespuestaAPIC_IniciarSesionC)




            Try
                Dim RespuestaIniciarsesion = Await PaginaD.DinaupServer.Funcion_Sesion_IniciarSesion(param.Identificador, param.Pass, param.UbicacionID, param.EmpresaID, Me.Request_UserAgent, Me.Request_UserIP)


                If RespuestaIniciarsesion.Respuesta = HTTPRespuestaAPIC_IniciarSesionC.InicioSesionRespuestaE.Ok Then


                ElseIf RespuestaIniciarsesion.Respuesta = HTTPRespuestaAPIC_IniciarSesionC.InicioSesionRespuestaE.SeleccionarContexto Then

                    If RespuestaIniciarsesion.IniciarSesion_Ubicaciones.Count = 1 Then
                        param.UbicacionID = RespuestaIniciarsesion.IniciarSesion_Ubicaciones.Keys(0).STR
                    End If

                    If RespuestaIniciarsesion.IniciarSesion_Empresas.Count = 1 Then
                        param.EmpresaID = RespuestaIniciarsesion.IniciarSesion_Empresas.Keys(0).STR
                    End If


                    If param.EmpresaID <> "" AndAlso param.UbicacionID <> "" Then
                        RespuestaIniciarsesion = Await PaginaD.DinaupServer.Funcion_Sesion_IniciarSesion(param.Identificador, param.Pass, param.UbicacionID, param.EmpresaID, Me.Request_UserAgent, Me.Request_UserIP)
                    End If

                End If



                param.Respuesta_UbicacionesASeleccionar = RespuestaIniciarsesion.IniciarSesion_Ubicaciones
                param.Respuesta_EmpresaASeleccionar = RespuestaIniciarsesion.IniciarSesion_Empresas
                param.Respuesta_AvisosOk = RespuestaIniciarsesion.avisosok
                param.Respuesta_AvisoError = RespuestaIniciarsesion.avisoserror
                param.Respuesta_DetallesDeSesion = RespuestaIniciarsesion.DetallesSesion


                If param.Respuesta_DetallesDeSesion IsNot Nothing Then
                    Me.DinaupSesionIDCoockie = param.Respuesta_DetallesDeSesion.SesionID.STR
                    coockiex.SetValue("sesionid", Me.DinaupSesionIDCoockie)
                    Me.Detectarsesion()
                Else
                    Me.DinaupSesionIDCoockie = ""
                    coockiex.SetValue("sesionid", "")
                End If
                Return RespuestaIniciarsesion

            Catch ex As Exception
                param.Respuesta_AvisoError = New List(Of String)
                param.Respuesta_AvisoError.Add(ex.Message)
            End Try

        End Function











        '! #########################
        '! #        REQUEST        #
        '! #########################

        Public Request_UserIP$
        Public Request_UserAgent$

        'Public RawURL$






        '! #########################
        '! #         API           #
        '! #########################
        Public Avisos_Ok As New List(Of String)
        Public Avisos_Errores As New List(Of String)
        'Public VUE_Data As New List(Of String)
        Public ReadOnly Property ConexionDisponibleConElServidorDinaup As Boolean
            Get
                Return PaginaD.DinaupServer.ConexionDisponible
            End Get
        End Property






        '! #########################
        '! #       SESIÓN          #
        '! #########################

        'Public SesionUsuario As APID.DinaupAPI_UsuarioSesionC
        Public DinaupUsuario As DinaupSesionC
        Public SesionIniciada As Boolean
        Public CambiarPass As Boolean




        ''! #########################
        ''! #        DEBUG          #
        ''! #########################
        'Public Debug_SW As New System.Diagnostics.Stopwatch











        ''! #########################
        ''! #        Eventos        #
        ''! #########################
        '', Este código pertenece a la clase base, es decir, para poder usar la clase, debe herdarse.
        '', Cuando se herede, la clase heredará estos 2 métodos.
        ''! Pagina_AntesDeIniciar:
        '', Este evento se producirá antes de que la clase base intente conectar con el API y Verificar la sesión. 
        ''! Pagina_Iniciada:  
        '', Este evento se producirá una vez se haya iniciado y comprobado todo el sistema API y de Sesiones. 

        'MustOverride Sub Pagina_CargarConfiguracion()
        'MustOverride Sub Pagina_AntesDeIniciar()
        'MustOverride Sub Pagina_Iniciada()


        Public Iniciado As Boolean



        Public Sub Iniciar(_IP$, _Navegador$, _SesionID$)
            Me.Request_UserIP = _IP
            Me.Request_UserAgent = _Navegador
            Me.DinaupSesionIDCoockie = _SesionID
            Iniciado = True
            Task.Run(Function()
                         Return Detectarsesion()
                     End Function).Wait()
        End Sub


        Public Async Function Detectarsesion() As Task


            ', Se comprueba la sesión de usuario  
            Me.DinaupUsuario = Await PaginaD.DinaupServer.Funcion_Sesion_RecibirDetalles_CompatibleConCache(Me.DinaupSesionIDCoockie, Me.Request_UserIP, Me.Request_UserAgent)
            Me.SesionIniciada = Me.DinaupUsuario IsNot Nothing AndAlso Me.DinaupUsuario.SesionIniciada
            Me.CambiarPass = Me.DinaupUsuario IsNot Nothing AndAlso Me.DinaupUsuario.DatosSesion IsNot Nothing AndAlso Me.DinaupUsuario.DatosSesion.CambiarPass





            If Me.SesionIniciada = False Then
                Me.DinaupUsuario = New DinaupSesionC(PaginaD.DinaupServer, Me.Request_UserIP, Me.Request_UserAgent)
                If DinaupSesionIDCoockie <> "" Then
                    DinaupSesionIDCoockie = ""
                End If
            End If

            If Me.DinaupUsuario Is Nothing Then
                Me.DinaupUsuario = New DinaupSesionC(PaginaD.DinaupServer, Me.Request_UserIP, Me.Request_UserAgent)
            End If


            Try
                RaiseEvent SesionCambiada()
            Catch
            End Try

            Await Task.Yield
        End Function


        Public DinaupSesionIDCoockie$



        Public Sub Iniciarx()


            PaginaD.Iniciar()


            Dim x = PaginaD.DinaupServer.Funcion_Sesion_RecibirDetalles_CompatibleConCache(DinaupSesionIDCoockie, Me.Request_UserIP, Me.Request_UserAgent)
            x.Wait()

            ', Se comprueba la sesión de usuario  
            Me.DinaupUsuario = x.Result


            ', Si no se ha detectado la sesión, se elimina de las coockies para que no se vuelvan a comprobar. 
            If Me.SesionIniciada = False Then
                DinaupUsuario = Nothing
                If DinaupSesionIDCoockie <> "" Then
                    DinaupSesionIDCoockie = ""
                End If
            End If


        End Sub























    End Class
End Class
