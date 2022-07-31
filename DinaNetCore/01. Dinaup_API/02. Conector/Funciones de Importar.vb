Partial Public Class APID



    Partial Public Class ServidorDinaup_ConectorC













        Public Function Funcion_Importar_Ejecutar(Sesion As DinaupSesionC, SeccionID As Guid, DatosImportando As DinaupAPI_ImportacionProcesableC, Optional CampoClaveListador$ = "id", Optional CampoClaveLista$ = "id") As HTTPRespuestaAPIC_Formualario_GuardarC
            Return Funcion_Importar_Ejecutar(Sesion, SeccionID, New List(Of DinaupAPI_ImportacionProcesableC)({DatosImportando}), CampoClaveListador, CampoClaveLista)
        End Function


        Public Async Function Funcion_Importar_Ejecutar_Async(Sesion As DinaupSesionC, SeccionID As Guid, DatosImportando As DinaupAPI_ImportacionProcesableC, Optional CampoClaveListador$ = "id", Optional CampoClaveLista$ = "id") As Task(Of HTTPRespuestaAPIC_Formualario_GuardarC)
            Return Await Funcion_Importar_Ejecutar_Async(Sesion, SeccionID, New List(Of DinaupAPI_ImportacionProcesableC)({DatosImportando}), CampoClaveListador, CampoClaveLista)
        End Function




        Public Function Funcion_Importar_Ejecutar(Sesion As DinaupSesionC, SeccionID As Guid, DatosImportando As List(Of DinaupAPI_ImportacionProcesableC), Optional CampoClaveListador$ = "id", Optional CampoClaveLista$ = "id") As HTTPRespuestaAPIC_Formualario_GuardarC


            Return Task.Run(Function()
                                Return Funcion_Importar_Ejecutar_Async(Sesion, SeccionID, DatosImportando, CampoClaveListador, CampoClaveLista)
                            End Function).Result

        End Function


        Public Async Function Funcion_Importar_Ejecutar_Async(Sesion As DinaupSesionC,
                                                              SeccionID As Guid,
                                                              DatosImportando As List(Of DinaupAPI_ImportacionProcesableC),
                                                              Optional CampoClaveListador$ = "id", Optional CampoClaveLista$ = "id") As Task(Of HTTPRespuestaAPIC_Formualario_GuardarC)


            Dim JSonFilas As New List(Of JSONBuildC)
            For Each Actual In DatosImportando
                JSonFilas.Add(Actual.ToJSoneableI_ToJSON)
            Next

            Dim CreandoDAtos As New JSONBuildC
            CreandoDAtos.Add("{")
            CreandoDAtos.Add("datos", JSonFilas)
            CreandoDAtos.Add("}")

            Dim ParamX As New Specialized.NameValueCollection
            ParamX.Add("dinaup_importar_datos", CreandoDAtos.ToJson)
            ParamX.Add("dinaup_importar_ejecutarscripts", "1")
            ParamX.Add("dinaup_importar_listador_campoclave", CampoClaveListador)
            ParamX.Add("dinaup_importar_lista_campoclave", CampoClaveLista)
            ParamX.Add("dinaup_importar_seccionid", SeccionID.STR)

            Dim HTTPResponse = Await Me.Http_EjecutarFuncionAPI_JSON_Asyn(Sesion, "importar", ParamX, Sesion.UserAgent, Sesion.IP)
            Dim X = New HTTPRespuestaAPIC_Formualario_GuardarC(HTTPResponse)



            Return X

        End Function

    End Class





End Class

