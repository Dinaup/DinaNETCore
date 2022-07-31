

Partial Public Class APID
    Public Class UtilidadesD



        Public Shared Function ValorRelacionado(CampoDestino$, ValorDestino$) As String
            Return "[" & CampoDestino & "=" & ValorDestino & "]"
        End Function


        Public Shared Sub AgregarGaleria(Datos As Dic(Of String, String), ArchivoURL As String, NombreArchivo As String)

            Dim Serializado = (New Anotacion_ArchivoDesdeURLC(ArchivoURL, NombreArchivo)).ToJSON.ToJson


            If Datos.ContainsKey("__galeria") Then
                Datos("__galeria") = Datos("__galeria") & "," & Serializado
            Else
                Datos.Add("__galeria", Serializado)
            End If

        End Sub



        Public Class Anotacion_TextoC
            Implements ToJSoneableI

            Public Mensaje As String


            Sub New(_texto$)
                Me.Mensaje = _texto
            End Sub

            Public Function ToJSON() As JSONBuildC Implements ToJSoneableI.ToJSON
                Dim r As New JSONBuildC
                r.Add("{")
                r.Add("modo", 0)
                r.Add("texto", Mensaje)
                r.Add("}")
                Return r
            End Function
        End Class


        Public Class Anotacion_ArchivoIDC
            Implements ToJSoneableI


            Public ArchivoID As Guid
            Public Mensaje As String

            Sub New(_ArchivoID As Guid, Optional _texto$ = "")
                Me.ArchivoID = _ArchivoID
                Me.Mensaje = _texto
            End Sub

            Public Function ToJSON() As JSONBuildC Implements ToJSoneableI.ToJSON
                Dim r As New JSONBuildC
                r.Add("{")
                r.Add("modo", 1)
                r.Add("texto", Mensaje)
                r.Add("archivoid", ArchivoID)
                r.Add("}")
                Return r
            End Function
        End Class




        Public Class Anotacion_ArchivoDesdeURLC
            Implements ToJSoneableI


            Public Archivo_URL As String
            Public Archivo_Nombre As String
            Public Mensaje As String


            Public Function ToJSON() As JSONBuildC Implements ToJSoneableI.ToJSON
                Dim r As New JSONBuildC
                r.Add("{")
                r.Add("modo", 2)
                r.Add("texto", Mensaje)
                r.Add("url", Archivo_URL)
                r.Add("nombre", Archivo_Nombre)
                r.Add("}")
                Return r
            End Function


            Sub New(_ArchivoURL As String, _NombreArchivo As String, Optional _texto$ = "")
                Me.Archivo_URL = _ArchivoURL
                Me.Archivo_Nombre = _NombreArchivo
                Me.Mensaje = _texto
            End Sub


        End Class


        Public Class Anotacion_ArchivoDatoC
            Implements ToJSoneableI


            Public Registro_SeccionID As Guid
            Public Registro_Dato As Guid
            Public Mensaje As String


            Public Function ToJSON() As JSONBuildC Implements ToJSoneableI.ToJSON
                Dim r As New JSONBuildC
                r.Add("{")
                r.Add("modo", 3)
                r.Add("texto", Mensaje)
                r.Add("seccionid", Registro_SeccionID)
                r.Add("datoid", Registro_Dato)
                r.Add("}")
                Return r
            End Function
            Sub New(_Registro_SeccionID As Guid, _Registro_Dato As Guid, Optional _texto$ = "")
                Me.Registro_SeccionID = _Registro_SeccionID
                Me.Registro_Dato = _Registro_Dato
                Me.Mensaje = _texto
            End Sub

        End Class


    End Class
End Class
