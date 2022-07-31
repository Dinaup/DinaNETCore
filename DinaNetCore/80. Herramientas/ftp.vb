
Imports System.Net.FtpWebRequest
Imports System.Net
Imports System.IO
Public Class Ftp


    Public Shared Function Descargar(_IP As String, _Puerto%, _User As String, _Contrasena As String, _RutaArchivo As String) As Byte()
        If _RutaArchivo.StartsWith("/") Then _RutaArchivo = _RutaArchivo.TrimStart("/"c)
        Using WC As New WebClient
            WC.Credentials = New NetworkCredential(_User, _Contrasena)
            Return WC.DownloadData("ftp://" & _IP & ":" & _Puerto & "/" & _RutaArchivo)
        End Using
    End Function




    Public Shared Function ExisteFichero(_IP As String, _Puerto%, _User As String, _Contrasena As String, _RutaArchivo As String) As Boolean
        If _RutaArchivo.StartsWith("/") Then _RutaArchivo = _RutaArchivo.TrimStart("/"c)


        Dim peticionFTP As FtpWebRequest
        peticionFTP = CType(WebRequest.Create(New Uri("ftp://" & _IP & ":" & _Puerto & "/" & _RutaArchivo)), FtpWebRequest)
        peticionFTP.Credentials = New NetworkCredential(_User, _Contrasena)
        peticionFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp
        peticionFTP.UsePassive = False

        Try
            Dim respuestaFTP As FtpWebResponse
            respuestaFTP = CType(peticionFTP.GetResponse(), FtpWebResponse)
            Return True
        Catch ex As Exception
            ' Si el objeto no existe, se producirá un error y al entrar por el Catch
            ' se devolverá falso
            Return False
        End Try
    End Function

End Class