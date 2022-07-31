Partial Public Class APID




    Public MustInherit Class APID_InformeC



        Public Sesion As DinaupSesionC
        Public ID As Guid
        Public Titulo$
        Public Parametros As Funcion_Informe_Consultar_ParametrosC
        Public Respuesta As HTTPRespuestaAPIC_InformeC
        Public MustOverride Sub CargarRespuesta()
        Public Event DatosActualizados()
        Public UltimaConsulta As Date



        Public DatosDisponibles As Boolean






        Public Async Function ConsultarAsync(_Sesion As DinaNETCore.ASP_NETD.SesionServicio, Optional _Pagina% = -1, Optional _RPP% = -1) As Task
            Await mConsultarAsync(_Sesion.DinaupUsuario, _Pagina%, _RPP%)
        End Function


        Public Async Function ConsultarAsync(_Sesion As DinaupSesionC, Optional _Pagina% = -1, Optional _RPP% = -1) As Task
            Await mConsultarAsync(_Sesion, _Pagina%, _RPP%)
        End Function


        Public Async Function Consultar_SinSesion_Async(_ConexionConServidor As APID.ServidorDinaup_ConectorC, Optional _Pagina% = -1, Optional _RPP% = -1) As Task
            Dim ConexionConServidorsinAutenticar = New DinaupSesionC
            ConexionConServidorsinAutenticar.ConexionServidor = _ConexionConServidor
            Await mConsultarAsync(ConexionConServidorsinAutenticar, _Pagina%, _RPP%)
        End Function




        Private Async Function mConsultarAsync(_Sesion As DinaupSesionC, _Pagina%, _RPP%) As Task
            DatosDisponibles = False
            Me.Sesion = _Sesion
            If _Pagina > 0 Then Parametros.PaginaActual = _Pagina
            If _RPP > 0 Then Parametros.ResultadosPorPagina = _RPP

            If Me.Parametros_Predeterminados Is Nothing Then
                Me.GuardarConfiguracionActualComoPredeterminada()
            End If

            Respuesta = Await Sesion.ConexionServidor.Funcion_Informes_Consultar(Sesion, Parametros)
            UltimaConsulta = Date.UtcNow
            CargarRespuesta()
            DatosDisponibles = True
            RaiseEvent DatosActualizados()
        End Function


        Public Async Function ActualizarDatos_Async() As Task
            Await ConsultarAsync(Me.Sesion)
        End Function


        Public Async Function ActualizarSiHaTranscurridoMasDeXSegundos_Async(Segundos As Double) As Task

            If Me.UltimaConsulta = Date.MinValue Then
                Await ActualizarDatos_Async()
            ElseIf (Me.UltimaConsulta - Date.UtcNow).TotalSeconds > Segundos Then
                Await ActualizarDatos_Async()
            End If


        End Function
        Public Async Function Consultar_PaginaSiguiente_Async() As Task
            If ExistePaginaSiguiente = False Then Await Task.Yield
            Parametros.PaginaActual += 1
            Await ConsultarAsync(Me.Sesion)
        End Function


        Public Async Function Consultar_PaginaAnterior_Async() As Task
            If ExistePaginaAnterior = False Then Await Task.Yield
            Parametros.PaginaActual -= 1
            Await ConsultarAsync(Me.Sesion)
        End Function


        Public ReadOnly Property ExistePaginaSiguiente As Boolean
            Get
                Return Respuesta IsNot Nothing AndAlso
                Respuesta.Listado IsNot Nothing AndAlso
                Respuesta.Listado.Pagina < Respuesta.Listado.TotalPaginas
            End Get
        End Property

        Public ReadOnly Property ExistePaginaAnterior As Boolean
            Get
                Return Respuesta IsNot Nothing AndAlso
                Respuesta.Listado IsNot Nothing AndAlso
                Respuesta.Listado.Pagina > 1
            End Get
        End Property




        Public Parametros_Predeterminados As Funcion_Informe_Consultar_ParametrosC

        Public Sub GuardarConfiguracionActualComoPredeterminada()
            Parametros_Predeterminados = New Funcion_Informe_Consultar_ParametrosC(Me.Parametros)
        End Sub
        Public Sub CargarConfiguracionPredeterminada()
            Me.Parametros = Parametros_Predeterminados
        End Sub


        Public Sub Agregar_Respuesta(_Key$, _Value As Boolean)
            Parametros.Agregar_Respuesta(_Key, _Value.STR)
        End Sub
        Public Sub Agregar_Respuesta(_Key$, _Value As Decimal)
            Parametros.Agregar_Respuesta(_Key, _Value.AdaptarJSON)
        End Sub

        Public Sub Agregar_Respuesta(_Key$, _Value As Integer)
            Parametros.Agregar_Respuesta(_Key, _Value.STR)
        End Sub

        Public Sub Agregar_Respuesta(_Key$, _Value As Date)
            Parametros.Agregar_Respuesta(_Key, _Value.AdaptarMySQL)
        End Sub

        Public Sub Agregar_Respuesta(_Key$, _Value As Guid)
            Parametros.Agregar_Respuesta(_Key, _Value.STR)
        End Sub
        Public Sub Agregar_Respuesta(_Key$, _Value$)
            Parametros.Agregar_Respuesta(_Key, _Value)
        End Sub
        Public Sub Agregar_Filtro(_Key$, _Operator$, _Value$)
            Parametros.Agregar_Filtro(_Key, _Operator, _Value)
        End Sub
        Public Sub Agregar_Filtro(_Key$, _Operator$, _Fecha As Date)
            Parametros.Agregar_Filtro(_Key, _Operator, _Fecha.AdaptarMySQL)
        End Sub
        Public Sub Agregar_Ordenar(_Key$, _desc As Boolean)
            Parametros.Agregar_Ordenar(_Key, _desc)
        End Sub




    End Class
End Class
