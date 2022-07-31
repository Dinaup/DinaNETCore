Partial Public Class APID




    Partial Public Class ServidorDinaup_ConectorC





        Public Function Funcion_ComentarioInterno_Galeria_Agregar(Sesion As DinaupSesionC, SeccionID As Guid, RegistroID As String, ArchivoAdjuntoID As Guid,
                                                                  Texto As String) As HTTPRespuestaAPIC_AgregarComentarioInternoC

            Return mFuncion_ComentarioInterno_ArchivosInternos_Agregar(Sesion, SeccionID, RegistroID, ArchivoAdjuntoID, Texto, False, False, True)
        End Function


        Public Function Funcion_ComentarioInterno_Comentario_Agregar(Sesion As DinaupSesionC, SeccionID As Guid, RegistroID As String, ArchivoAdjuntoID As Guid,
                                                                     Texto As String) As HTTPRespuestaAPIC_AgregarComentarioInternoC

            Return mFuncion_ComentarioInterno_ArchivosInternos_Agregar(Sesion, SeccionID, RegistroID, ArchivoAdjuntoID, Texto, False, True, False)
        End Function



        Public Function Funcion_ComentarioInterno_ArchivosInternos_Agregar(Sesion As DinaupSesionC, SeccionID As Guid, RegistroID As String, ArchivoAdjuntoID As Guid,
                                                                           Texto As String) As HTTPRespuestaAPIC_AgregarComentarioInternoC
            Return mFuncion_ComentarioInterno_ArchivosInternos_Agregar(Sesion, SeccionID, RegistroID, ArchivoAdjuntoID, Texto, True, False, False)
        End Function



        Private Function mFuncion_ComentarioInterno_ArchivosInternos_Agregar(Sesion As DinaupSesionC,
                                                                             SeccionID As Guid,
                                                                             RegistroID As String,
                                                                             ArchivoAdjuntoID As Guid,
                                                                             Texto As String,
                                                                             EnArchivos As Boolean,
                                                                             EnComentarios As Boolean,
                                                                             EnGaleria As Boolean) As HTTPRespuestaAPIC_AgregarComentarioInternoC


            Dim param As New Specialized.NameValueCollection
            param.Add("dinaup_texto", Texto)
            param.Add("dinaup_seccionid", SeccionID.STR)
            param.Add("dinaup_registroid", RegistroID.STR)
            param.Add("dinaup_archivoid", ArchivoAdjuntoID.STR)
            param.Add("dinaup_engaleria", EnGaleria.STR)
            param.Add("dinaup_encomentario", EnComentarios.STR)
            param.Add("dinaup_enarchivo", EnArchivos.STR)


            Dim R = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "agregarcomentario", param, Sesion.UserAgent, Sesion.IP)
            Dim X As New HTTPRespuestaAPIC_AgregarComentarioInternoC(R)
            Return X

        End Function


    End Class





    Public Class HTTPRespuestaAPIC_AgregarComentarioInternoC





        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)
        Public RespuestaGenerica As HTTP_RespuestaAPIC




        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror
        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub



    End Class






End Class