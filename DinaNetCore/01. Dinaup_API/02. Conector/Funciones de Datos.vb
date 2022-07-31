Partial Public Class APID



    Partial Public Class ServidorDinaup_ConectorC








        Public Function Funcion_Datos_Recibir(_SeccionID As Guid, IncluirLista As Boolean, _Parametros As SeccionConsultaParametrosC) As HTTPRespuestaAPIC_Recibir_DatosC
            Return Task.Run(Function()
                                Return Funcion_Datos_Recibir_ASync(_SeccionID, IncluirLista, _Parametros)
                            End Function).Result
        End Function





        Public Async Function Funcion_Datos_Recibir_ASync(_SeccionID As Guid, IncluirLista As Boolean, _Parametros As SeccionConsultaParametrosC) As Task(Of HTTPRespuestaAPIC_Recibir_DatosC)







            If _Parametros Is Nothing Then Throw New Exception(". Código de error (E-2613)")
            If _Parametros.DinaupSesion Is Nothing Then Throw New Exception(". Código de error (E-2558)")
            If _Parametros.DinaupSesion.ConexionServidor Is Nothing Then Throw New Exception(". Código de error (E-2559)")
            'If _Parametros.DinaupSesion.DatosSesion Is Nothing Then Throw New Exception(". Código de error (E-2560)")

            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_seccionid", _SeccionID.STR)
            DAtos.Add("dinaup_busqueda_campo", _Parametros.CampoBusqueda)
            DAtos.Add("dinaup_sinlista", (Not IncluirLista).STR)
            DAtos.Add("dinaup_incluir_comentarios", _Parametros.dinaup_incluir_comentarios.STR)
            DAtos.Add("dinaup_incluir_archivos", _Parametros.dinaup_incluir_archivos.STR)
            DAtos.Add("dinaup_incluir_galeria", _Parametros.dinaup_incluir_galeria.STR)
            DAtos.Add("dinaup_incluir_archivosid_galeria", _Parametros.dinaup_incluir_archivosid_galeria.STR)




            For i = 0 To _Parametros.ValorBusqueda.Length - 1
                DAtos.Add("v" & i.ToString, _Parametros.ValorBusqueda(i))
            Next

            Dim HTTPResponse = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(_Parametros.DinaupSesion, "datos", DAtos, _Parametros.DinaupSesion.UserAgent, _Parametros.DinaupSesion.IP)
            Dim X = New HTTPRespuestaAPIC_Recibir_DatosC(HTTPResponse)
            Return X
        End Function

    End Class



    Public Class HTTPRespuestaAPIC_Recibir_DatoC

        Public Listador As New Dic(Of String, String)
        Public Lista As New List(Of Dic(Of String, String))

        Public Listador_Archivos As List(Of DinaupAPI_AnotacionRegistroC)
        Public Listador_Comentarios As List(Of DinaupAPI_AnotacionRegistroC)
        Public Listador_Galeria As List(Of DinaupAPI_AnotacionRegistroC)
        Public Listador_ArchivosIDEnGaleria As List(Of String)

        Public ReadOnly Property Item1 As Dic(Of String, String)
            Get
                Return Listador
            End Get
        End Property



        Public ReadOnly Property Item2 As List(Of Dic(Of String, String))
            Get
                Return Lista
            End Get
        End Property



        Sub New(_Listador As Dic(Of String, String), _Lista As List(Of Dic(Of String, String)))
            Listador = _Listador
            Lista = _Lista
        End Sub
    End Class
    Public Class HTTPRespuestaAPIC_Recibir_DatosC


        Public DinapSesion As DinaupSesionC
        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)
        'Public DetallesSesion As APID.DinaupAPI_UsuarioSesionC
        Public RespuestaGenerica As HTTP_RespuestaAPIC



        Public Datos As New Dic(Of String, HTTPRespuestaAPIC_Recibir_DatoC)


        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            Me.avisosok = Obj.avisosok
            Me.avisoserror = Obj.avisoserror
            Me.DinapSesion = Obj.DinapSesion
            'Me.DetallesSesion = Obj.DinapSesion
            Me.RespuestaGenerica = Obj



            If Obj.Obj_Respuesta IsNot Nothing AndAlso Obj.Obj_Respuesta("registros").NoEsNulo Then

                For Each RegistroActual In Obj.Obj_Respuesta("registros")

                    Dim XListador = RegistroActual("listador")
                    Dim XLista = RegistroActual("lista")


                    Dim listador_archivos = RegistroActual("listador_archivos")
                    Dim listador_comentarios = RegistroActual("listador_comentarios")
                    Dim listador_galeria = RegistroActual("listador_galeria")
                    Dim listador_archivosidengaleria = RegistroActual("listador_archivosidengaleria")

                    Dim Lista_ArchivosIDEnGaleria As New List(Of String)



                    Dim Lista_archivos As New List(Of DinaupAPI_AnotacionRegistroC)
                    Dim Lista_comentarios As New List(Of DinaupAPI_AnotacionRegistroC)
                    Dim Lista_galeria As New List(Of DinaupAPI_AnotacionRegistroC)

                    Dim ListadorDic As New Dic(Of String, String)
                    Dim ListaDic As New List(Of Dic(Of String, String))

                    For Each Actual As Newtonsoft.Json.Linq.JProperty In XListador
                        ListadorDic.Add(Actual.Name, Actual.Value.STR)
                    Next

                    If listador_archivosidengaleria.NoEsNulo Then
                        For Each Actual In listador_archivosidengaleria
                            Lista_ArchivosIDEnGaleria.Add(Actual.STR)
                        Next
                    End If
                    If XLista.NoEsNulo Then
                        For Each ListaActual As Newtonsoft.Json.Linq.JToken In XLista
                            Dim RegistroDic As New Dic(Of String, String)
                            For Each ValorListaActual As Newtonsoft.Json.Linq.JProperty In ListaActual
                                RegistroDic.Add(ValorListaActual.Name, ValorListaActual.Value.STR)
                            Next
                            ListaDic.Add(RegistroDic)
                        Next
                    End If




                    If listador_archivos.NoEsNulo Then
                        For Each Actual In listador_archivos
                            Lista_archivos.Add(New DinaupAPI_AnotacionRegistroC(Actual))
                        Next
                    End If
                    If listador_comentarios.NoEsNulo Then
                        For Each Actual In listador_comentarios
                            Lista_comentarios.Add(New DinaupAPI_AnotacionRegistroC(Actual))
                        Next
                    End If

                    If listador_galeria.NoEsNulo Then
                        For Each Actual In listador_galeria
                            Lista_galeria.Add(New DinaupAPI_AnotacionRegistroC(Actual))
                        Next
                    End If

                    Dim Nuevo = New HTTPRespuestaAPIC_Recibir_DatoC(ListadorDic, ListaDic)
                    Nuevo.Listador_Archivos = Lista_archivos
                    Nuevo.Listador_Comentarios = Lista_comentarios
                    Nuevo.Listador_Galeria = Lista_galeria
                    Nuevo.Listador_ArchivosIDEnGaleria = Lista_ArchivosIDEnGaleria
                    Datos.Add(ListadorDic("id"), Nuevo)

                Next

            End If


        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub

        Sub New(_MotivoNo As String)
            avisoserror = New List(Of String)
            avisoserror.Add(_MotivoNo)
        End Sub



    End Class




End Class



Public Class SeccionConsultaParametrosC



        Public DinaupSesion As DinaNETCore.APID.DinaupSesionC
        Public CampoBusqueda$
        Public ValorBusqueda As String()

        Public dinaup_incluir_comentarios As Boolean = False
        Public dinaup_incluir_archivos As Boolean = False
        Public dinaup_incluir_galeria As Boolean = False
        Public dinaup_incluir_archivosid_galeria As Boolean = False






    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="_Conexion"></param>
    ''' <param name="ExpresionBusqueda">Ejemplo {"id", "=" , "GUID"}</param>
    Sub New(_Conexion As DinaNetCore.APID.ServidorDinaup_ConectorC, ExpresionBusqueda() As String)
        Me.DinaupSesion = New APID.DinaupSesionC(_Conexion, "?", "?")
        Me.CampoBusqueda = "*"
        Me.ValorBusqueda = ExpresionBusqueda
    End Sub

End Class

