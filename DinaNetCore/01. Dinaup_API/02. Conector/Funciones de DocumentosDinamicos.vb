Partial Public Class APID







    Partial Public Class ServidorDinaup_ConectorC



        Public Function Funcion_DocumentoDinamico_Consultar(Sesion As DinaupSesionC, Parametros As Funcion_DocumentoDinamico_Consultar_ParametrosC) As HTTPRespuestaAPIC_DocumentoDinamicoC


            Dim DAtos As New Specialized.NameValueCollection
            DAtos.Add("dinaup_dd_id", Parametros.DocDiamicoID.STR)
            DAtos.Add("dinaup_dd_vista", Parametros.SerializadoURL)
            DAtos.Add("dinaup_dd_descargar", Parametros.dinaup_dd_descargar.STR)
            DAtos.Add("dinaup_dd_modurl", Parametros.dinaup_dd_modurl.STR)
            DAtos.Add("dinaup_utc_diff", Parametros.dinaup_utc_diff.ToString)

            If Parametros.dinaup_sep_decimal = "." Then
                DAtos.Add("dinaup_sep_decimal", ".")
            Else
                DAtos.Add("dinaup_sep_decimal", ",")
            End If

            Dim R = Me.Http_EjecutarFuncionAPI_JSON(Sesion, "docdinamico", DAtos, Sesion.UserAgent, Sesion.IP)
            Dim Retornar As New HTTPRespuestaAPIC_DocumentoDinamicoC(R)
            Return Retornar
        End Function








    End Class






    Public Class Funcion_DocumentoDinamico_Consultar_ParametrosC


        Public DocDiamicoID As String
        Public Respuestas As New Dic(Of String, String)

        Public dinaup_dd_descargar As Boolean = True
        Public dinaup_dd_modurl As Boolean
        Public dinaup_utc_diff As Integer
        Public dinaup_sep_decimal As String


        Public SerializadoURL_Manual$
        Public ReadOnly Property SerializadoURL$
            Get
                If SerializadoURL_Manual <> "" Then
                    Return SerializadoURL_Manual
                End If

                Dim R As New List(Of String)
                If Respuestas.TieneDatos Then
                    For Each Actual In Respuestas
                        R.Add(("dupvar785_" & Actual.Key).AdaptarURL & "=" & Actual.Value.AdaptarURL)
                    Next
                End If
                Return String.Join("&", R)
            End Get
        End Property




        Public Sub Agregar_Respuesta(_Key$, _Value$)
            If _Key = "" Then Throw New Exception(". Código de error (E-5303)")
            If _Value Is Nothing Then _Value = ""
            Respuestas.HacerMagia(_Key) = _Value
        End Sub









        Sub New(_DocumentID As String)
            Me.DocDiamicoID = _DocumentID
        End Sub



        Sub New(_DocumentID As Guid)
            Me.DocDiamicoID = _DocumentID.STR
        End Sub

    End Class






    Public Class HTTPRespuestaAPIC_DocumentoDinamicoC





        Public avisosok As List(Of String)
        Public avisoserror As List(Of String)

        'Public DetallesSesion As APID.DinaupAPI_UsuarioSesionC
        Public RespuestaGenerica As HTTP_RespuestaAPIC





        Public R_URL$
        Public R_Nombre$
        Public R_Estado%


        Public R_Documento As DDocumentC


        Public R_DocumentoDatoSTR As String
        Public R_DocumentoJSON As Newtonsoft.Json.Linq.JToken




        Sub Iniciar(Obj As HTTP_RespuestaAPIC)
            avisosok = Obj.avisosok
            avisoserror = Obj.avisoserror


            Me.R_Documento = New DDocumentC(Obj.Obj_Respuesta("docdinamico"))
            Me.R_DocumentoDatoSTR = Obj.Obj_Respuesta("documento").STR
            Me.R_Estado = Obj.Obj_Respuesta("estado").INT
            Me.R_Nombre = Obj.Obj_Respuesta("nombre").STR
            Me.R_URL = Obj.Obj_Respuesta("url").STR


            Dim X As Newtonsoft.Json.Linq.JToken = Obj.Obj_Respuesta("documento")

            Try
                Me.R_DocumentoJSON = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(R_DocumentoDatoSTR), Newtonsoft.Json.Linq.JToken)
            Catch
            End Try




        End Sub



        Sub New(Obj As HTTP_RespuestaAPIC)
            RespuestaGenerica = Obj
            Iniciar(Obj)
        End Sub





        Public Class DDocumentC


            Public ID As Guid
            Public Nombre$
            Public Descripcion$
            Public Categoria$
            Public SubCategoria$
            Public TamanoMilimetros_Ancho As Decimal
            Public TamanoMilimetros_Alto As Decimal
            Public Margen_A As Decimal
            Public Margen_D As Decimal
            Public Margen_B As Decimal
            Public Margen_L As Decimal
            Public ValidoEmail As Boolean
            Public Es_Ticket As Boolean
            Public Es_Impresion As Boolean
            Public Es_RI As Boolean
            Public Es_Archivo As Boolean
            Public Es_ArchivoExtension$
            Public Es_Archivo_TextoPlano As Boolean
            Public Preguntas As DinaupAPI_VariablesListaC
            Public Seccion As DinaupAPI_SeccionC


            Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)




                ID = JsonObj("id").ToString.ToGuid
                Nombre = JsonObj("titulo").STR
                Descripcion = JsonObj("descripcion").STR
                Categoria = JsonObj("categoria").STR
                SubCategoria = JsonObj("subcategoria").STR
                TamanoMilimetros_Ancho = JsonObj("anchomm").DEC
                TamanoMilimetros_Alto = JsonObj("altomm").DEC
                Margen_A = JsonObj("margen_superiormm").DEC
                Margen_D = JsonObj("margen_derechamm").DEC
                Margen_B = JsonObj("margen_inferiormm").DEC
                Margen_L = JsonObj("margen_izquierdamm").DEC
                ValidoEmail = JsonObj("validoemail").BOOL
                Es_Ticket = JsonObj("esticket").BOOL
                Es_Impresion = JsonObj("esimpresion").BOOL
                Es_RI = JsonObj("esri").BOOL
                Es_Archivo = JsonObj("esarchivo").BOOL
                Es_ArchivoExtension = JsonObj("esarchivoextension").STR
                Es_Archivo_TextoPlano = JsonObj("estextoplano").BOOL



                If JsonObj("seccion") IsNot Nothing AndAlso JsonObj("seccion").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                    Seccion = New DinaupAPI_SeccionC(JsonObj("seccion"))
                End If



                If JsonObj("preguntas") IsNot Nothing AndAlso JsonObj("preguntas").Type <> Newtonsoft.Json.Linq.JTokenType.Null Then
                    If JsonObj("preguntas").Count > 0 Then
                        Preguntas = New DinaupAPI_VariablesListaC(JsonObj("preguntas"))
                    End If
                End If





            End Sub
        End Class


        Public Class DDDocument_DataC




            Sub New(JsonObj As Newtonsoft.Json.Linq.JToken)

            End Sub
        End Class


    End Class




End Class