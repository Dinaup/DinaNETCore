Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Extensions.Primitives
Imports System.IO



Public Class RespuestaSubidaC

    Public UnError$
    Public ArchivosSubidos As New List(Of DinaNetCore.APID.FirmaURLArchivoSubidoC)


    Sub New(Respuesta As String)
        If Respuesta.Contains("{") = False Then
            Me.UnError = Respuesta
        Else
            Try
                Dim ElObj As Newtonsoft.Json.Linq.JToken = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(Respuesta), Newtonsoft.Json.Linq.JToken)
                If ElObj IsNot Nothing AndAlso ElObj("archivos").NoEsNulo Then
                    For Each Actual In ElObj("archivos")
                        Me.ArchivosSubidos.Add(New APID.FirmaURLArchivoSubidoC(Actual))
                    Next
                End If
            Catch ex As Exception
                Me.UnError = ex.Message
            End Try
        End If
    End Sub

End Class



Public Class ControladorDeArchivosC
    Inherits Microsoft.AspNetCore.Mvc.Controller













    Public FirmaDeArchivos As NET_Servicio_FirmaURLC

    Public Sub New(_ServicioDeFirma As NET_Servicio_FirmaURLC)
        Me.FirmaDeArchivos = _ServicioDeFirma
    End Sub



    <Microsoft.AspNetCore.Mvc.HttpPost("dinaup/subirarchivos/subir")>
    Public Function POST_SubirArchivo() As IActionResult


        Try

            Dim files = Request.Form.Files.ToArray


            Dim Param_Ext As StringValues
            Dim Param_Max As StringValues
            Dim Param_Firma As StringValues
            Dim Param_Num As StringValues
            Request.Query.TryGetValue("ext", Param_Ext)
            Request.Query.TryGetValue("max", Param_Max)
            Request.Query.TryGetValue("signature", Param_Firma)
            Request.Query.TryGetValue("signature", Param_Firma)
            Request.Query.TryGetValue("num", Param_Num)


            If Param_Ext.Count <> 1 Then Return StatusCode(500, "Parámetro ext inválido")
            If Param_Max.Count <> 1 Then Return StatusCode(500, "Parámetro max inválido")
            If Param_Firma.Count <> 1 Then Return StatusCode(500, "Parámetro signature inválido")
            If Param_Num.Count <> 1 Then Return StatusCode(500, "Parámetro números inválido")
            If files.Length = 0 Then Return StatusCode(500, "No se ha detectado ningún archivo.")


            Dim Valor_Ext As String = Param_Ext(0)
            Dim Valor_Max As Decimal = Param_Max(0).DEC(0)
            Dim Valor_Firma As String = Param_Firma(0)
            Dim Valor_NumeroMaximoDeArchivos As Integer = Param_Num(0).INT(0)

            If FirmaDeArchivos.FirmarURL_SubirArchivo_EsValida(Valor_Max, Valor_Ext, Valor_NumeroMaximoDeArchivos, Valor_Firma) = False Then
                Return StatusCode(500, "No está autorizado.")
            End If

            If Valor_NumeroMaximoDeArchivos < files.Length Then
                Return StatusCode(500, "No puede enviar tantos archivos.")
            End If


            Dim ListaDeArchivos = New List(Of DinaNetCore.APID.FirmaURLArchivoSubidoC)

            For Each ArchivoActual In files


                Dim Actual_ArchivoID = Guid.NewGuid().ToString()
                Dim Actual_RutaEnDisco = FirmaDeArchivos.BuscarArchivo_Subido(Actual_ArchivoID)
                If Directory.Exists(Path.GetDirectoryName(Actual_RutaEnDisco)) = False Then Directory.CreateDirectory(Path.GetDirectoryName(Actual_RutaEnDisco))

                Using fileStream As Stream = New FileStream(Actual_RutaEnDisco, FileMode.Create)
                    ArchivoActual.CopyTo(fileStream)
                End Using

                Dim URLArchivo = FirmaDeArchivos.FirmarURL_LeerArchivoSubido_Firmar(Actual_ArchivoID)
                Dim AchivoDetalles = New DinaNetCore.APID.FirmaURLArchivoSubidoC(Actual_ArchivoID.ToGuid(), URLArchivo, ArchivoActual.FileName)
                ListaDeArchivos.Add(AchivoDetalles)

            Next

            Dim RetornarArchivos = New JSONBuildC()
            RetornarArchivos.Add("{")
            RetornarArchivos.Add("archivos", ListaDeArchivos)
            RetornarArchivos.Add("}")
            Return StatusCode(200, RetornarArchivos.ToJson())
        Catch ex As Exception
            Return StatusCode(500, ex.Message)
        End Try
    End Function



    <HttpGet("dinaup/archivos/{id}")>
    Public Function GET_LeerArchivo(ID As String) As IActionResult

        Try

            Dim request_tama = If((Request.Query("tama").Count = 1), Request.Query("tama")(0), "")
            Dim Conexion = DinaNetCore.ASP_NETD.PaginaD.DinaupServer
            Dim NombreDeArchivo = ID
            Dim Archivo_Publico = FirmaDeArchivos.BuscarArchivo_Publico(NombreDeArchivo)
            Dim Archivo_Publico_Tamano = FirmaDeArchivos.BuscarArchivo_Publico(Archivo_Publico, request_tama)
            Dim Ruta_Archivo_Privado = FirmaDeArchivos.BuscarArchivo_Privado(NombreDeArchivo)
            Dim Ruta_Archivo_Privado_Tamano = FirmaDeArchivos.BuscarArchivo_Privado(Ruta_Archivo_Privado, request_tama)

            If Archivo_Publico_Tamano <> Archivo_Publico AndAlso System.IO.File.Exists(Archivo_Publico_Tamano) Then
                Response.Headers.Add("Cache-Control", "max-age=31536000")
                Return New PhysicalFileResult(Archivo_Publico_Tamano, DinaNetCore.ExtensionesM.GetMimeType(Archivo_Publico_Tamano)) With {.FileDownloadName = System.IO.Path.GetFileName(Archivo_Publico_Tamano)}
            ElseIf System.IO.File.Exists(Archivo_Publico) Then
                Response.Headers.Add("Cache-Control", "max-age=31536000")
                Return New PhysicalFileResult(Archivo_Publico, DinaNetCore.ExtensionesM.GetMimeType(Archivo_Publico)) With {.FileDownloadName = System.IO.Path.GetFileName(Archivo_Publico)}
            End If

            If NombreDeArchivo.Length > 10 AndAlso Request.Query.ContainsKey("expire") AndAlso Request.Query.ContainsKey("signature") Then

                If System.IO.File.Exists(Ruta_Archivo_Privado) Then
                    Dim request_Expire = Request.Query("expire")(0)
                    Dim request_Signature = Request.Query("signature")(0)
                    Dim request_file = Request.Query("file")(0)
                    Dim request_mime = Request.Query("mime")(0)
                    Dim Firma_HMAC = DinaNetCore.ExtensionesM.CalcularHMAC(FirmaDeArchivos.HashKey, NombreDeArchivo & "[/]" & request_Expire & "[/]" + request_file & "[/]" + request_mime)

                    If request_Signature = Firma_HMAC Then

                        If Ruta_Archivo_Privado_Tamano <> Ruta_Archivo_Privado AndAlso System.IO.File.Exists(Ruta_Archivo_Privado_Tamano) Then
                            Response.Headers.Add("Content-Type", request_mime)
                            Response.Headers.Add("Content-Disposition", "attachment; filename=""" & request_file & """")
                            Response.Headers.Add("Cache-Control", "max-age=31536000")
                            Return New PhysicalFileResult(Ruta_Archivo_Privado_Tamano, request_mime) With {.FileDownloadName = request_file}
                        End If

                        If System.IO.File.Exists(Ruta_Archivo_Privado) Then
                            Response.Headers.Add("Content-Type", request_mime)
                            Response.Headers.Add("Content-Disposition", "attachment; filename=""" & request_file & """")
                            Response.Headers.Add("Cache-Control", "max-age=31536000")
                            Return New PhysicalFileResult(Ruta_Archivo_Privado, request_mime) With {
                                .FileDownloadName = request_file
                            }
                        End If
                    End If
                End If
            End If

        Catch
        End Try

        Return StatusCode(403, "Archivo no encontrado.")
    End Function

    <HttpGet("dinaup/archivossubidos/{id}")>
    Public Function RecibirArchivoSubido(ID As String) As IActionResult
        Try

            If ID.Length > 10 AndAlso Request.Query.ContainsKey("expire") AndAlso Request.Query.ContainsKey("signature") Then
                Dim Conexion = DinaNetCore.ASP_NETD.PaginaD.DinaupServer
                Dim ArchivoSubido = FirmaDeArchivos.BuscarArchivo_Subido(ID)

                If System.IO.File.Exists(ArchivoSubido) Then
                    Dim request_Expire = Request.Query("expire")(0)
                    Dim request_Signature = Request.Query("signature")(0)

                    If FirmaDeArchivos.FirmarURL_LeerArchivoSubido_EsValida(ID, request_Expire, request_Signature) Then
                        Return New PhysicalFileResult(ArchivoSubido, "application/octet-stream")
                    End If
                End If
            End If

        Catch
        End Try

        Return StatusCode(403, "Archivo no encontrado.")
    End Function
End Class
