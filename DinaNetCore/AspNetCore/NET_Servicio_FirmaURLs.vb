Imports DinaNETCore.APID

Public Class NET_Servicio_FirmaURLC



    Public HashKey As Byte()
    Public RutaArchivosPublicos As String = ""
    Public RutaPrivados As String = ""
    Public RutaSubiendo As String = ""





#Region "Firma"
#End Region







    Public Function FirmarURL_SubirArchivo(Optional TamanoMaximoMegas As Decimal = 20, Optional Extensiones As String = "*", Optional NumeroMaximoDeArchivos As Integer = 1) As String
        Dim TokenFirma = String.Join("[/]", New String() {Extensiones, TamanoMaximoMegas.AdaptarMySQL(), NumeroMaximoDeArchivos.ToString()})
        Dim Firma_HMAC As String = CalcularHMAC(HashKey, TokenFirma)
        Return String.Concat("/dinaup/subirarchivos/subir/?ext=", Extensiones.AdaptarURL(), "&max=", TamanoMaximoMegas.AdaptarMySQL().AdaptarURL(), "&num=", NumeroMaximoDeArchivos.ToString(), "&signature=", Firma_HMAC.AdaptarURL())
    End Function

    Public Function FirmarURL_SubirArchivo_EsValida(TamanoMaximoMegas As Decimal, Extensiones As String, NumeroMaximoDeArchivos As Integer, Firma As String) As Boolean
        Dim TokenFirma = String.Join("[/]", New String() {Extensiones, TamanoMaximoMegas.AdaptarMySQL(), NumeroMaximoDeArchivos.ToString()})
        Dim Firma_HMAC As String = CalcularHMAC(HashKey, TokenFirma)
        Return (Firma_HMAC = Firma)
    End Function

    Public Function FirmarURL_Archivo(ID As String, NombreDeArchivo As String, Optional Tamano As TamanoE = TamanoE.Original) As String
        Return FirmarURL_Archivo(ID.ToGuid, NombreDeArchivo, Tamano)
    End Function
    Public Function FirmarURL_Archivo(ID As Guid, NombreDeArchivo As String, Optional Tamano As TamanoE = TamanoE.Original) As String
        If ID = Guid.Empty Then Return ""

        Dim Firma_NombreDeArchivo As String = ID.STR
        Dim Ruta_Archivo_Publico = BuscarArchivo_Publico(Firma_NombreDeArchivo)
        Dim Ruta_Archivo_Privado = BuscarArchivo_Privado(Firma_NombreDeArchivo)
        Dim Mime As String = GetMimeType(NombreDeArchivo)

        If System.IO.File.Exists(Ruta_Archivo_Publico) Then
            Return String.Concat("/dinaup/archivos/", Firma_NombreDeArchivo, "?mime=", Mime, "&file=", NombreDeArchivo, "&tama=", (CInt(Tamano)).ToString())
        End If

        If System.IO.File.Exists(Ruta_Archivo_Privado) Then
            Dim Firma_FechaDeExpiracion As String = DateTime.UtcNow.AddMinutes(20).Ticks.ToString()
            Dim TokenFirma = Firma_NombreDeArchivo & "[/]" & Firma_FechaDeExpiracion & "[/]" & NombreDeArchivo & "[/]" & Mime
            Dim Firma_HMAC As String = CalcularHMAC(HashKey, TokenFirma)
            Return String.Concat("/dinaup/archivos/", Firma_NombreDeArchivo.AdaptarURL(), "?expire=", Firma_FechaDeExpiracion.AdaptarURL(), "&mime=", Mime.AdaptarURL(), "&file=", NombreDeArchivo.AdaptarURL(), "&signature=", Firma_HMAC.AdaptarURL(), "&tama=", (CInt(Tamano)).ToString())
        End If

        Return ""
    End Function

    Public Function FirmarURL_LeerArchivoSubido_Firmar(ID As String) As String
        If ID.EsGUID = False Then Return ""
        Dim Ruta_Archivo_Subido = BuscarArchivo_Subido(ID)
        If System.IO.File.Exists(Ruta_Archivo_Subido) Then
            Dim Firma_FechaDeExpiracion As String = DateTime.UtcNow.AddHours(10).Ticks.ToString()
            Dim TokenFirma = ID & "[/]" & Firma_FechaDeExpiracion
            Dim Firma_HMAC As String = DinaNETCore.ExtensionesM.CalcularHMAC(HashKey, TokenFirma)
            Return String.Concat("/dinaup/archivossubidos/", ID, "?expire=", Firma_FechaDeExpiracion.AdaptarURL(), "&signature=", Firma_HMAC.AdaptarURL())
        End If
        Return ""
    End Function

    Public Function FirmarURL_LeerArchivoSubido_EsValida(ID As String, Expire As String, Firma As String) As Boolean
        If ID.EsGUID = False Then Return False
        Dim LongExpire As Long
        Dim TokenFirma = String.Join("[/]", New String() {ID, Expire})
        Dim Firma_HMAC As String = CalcularHMAC(HashKey, TokenFirma)
        If Firma_HMAC <> Firma Then Return False
        If Long.TryParse(Expire, LongExpire) = False Then Return False
        Dim Fecha = New DateTime(LongExpire, DateTimeKind.Utc)
        If Fecha < DateTime.UtcNow Then Return False
        Return True
    End Function

    Public Function FirmarURL_Archivo(Archivo As DinaupAPI_ArchivoC, Optional Tamano As TamanoE = TamanoE.Original) As String
        Return FirmarURL_Archivo(Archivo.ArchivoID.ToString(), Archivo.Nombre, Tamano)
    End Function










#Region "Busqueda Archivos"
#End Region


    Public Function BuscarArchivo_Privado(Archivo As DinaupAPI_ArchivoC) As String
        Dim ID = Archivo.ArchivoID.ToString()
        If RutaPrivados <> "" Then Return System.IO.Path.Combine(RutaPrivados, ID(0).ToString(), ID(1).ToString(), ID(2).ToString(), ID(3).ToString(), ID & ".dat")
        Return ""
    End Function

    Public Function BuscarArchivo_Publico(Archivo As DinaupAPI_ArchivoC) As String
        Dim ID = Archivo.ArchivoID.ToString()
        If RutaArchivosPublicos <> "" Then Return System.IO.Path.Combine(RutaArchivosPublicos, ID(0).ToString(), ID(1).ToString() + ID(2).ToString(), ID & ".dat")
        Return ""
    End Function

    Public Function BuscarArchivo_Privado(ID As String) As String
        If RutaPrivados <> "" Then Return System.IO.Path.Combine(RutaPrivados, ID(0).ToString(), ID(1).ToString(), ID(2).ToString(), ID(3).ToString(), ID & ".dat")
        Return ""
    End Function

    Public Function BuscarArchivo_Subido(ID As String) As String
        If RutaSubiendo <> "" Then Return System.IO.Path.Combine(RutaSubiendo, ID(0).ToString(), ID(1).ToString(), ID(2).ToString(), ID(3).ToString(), ID & ".dat")
        Return ""
    End Function

    Public Function BuscarArchivo_Publico(ID As String) As String
        If RutaArchivosPublicos <> "" Then Return System.IO.Path.Combine(RutaArchivosPublicos, ID(0).ToString(), ID(1).ToString() & ID(2).ToString(), ID & ".dat")
        Return ""
    End Function

    Public Function BuscarArchivo_Privado(Archivo As DinaupAPI_ArchivoC, Tamano As String) As String
        Dim RutaX = BuscarArchivo_Privado(Archivo)
        If RutaX <> "" Then Return DinaNETCore.ExtensionesM.AplicarTamanoARuta(RutaX, Tamano)
        Return ""
    End Function

    Public Function BuscarArchivo_Publico(Archivo As DinaupAPI_ArchivoC, Tamano As String) As String
        Dim RutaX = BuscarArchivo_Publico(Archivo)
        If RutaX <> "" Then Return DinaNETCore.ExtensionesM.AplicarTamanoARuta(RutaX, Tamano)
        Return ""
    End Function

    Public Function BuscarArchivo_Privado(ID As String, Tamano As String) As String
        Dim RutaX = BuscarArchivo_Privado(ID)
        If RutaX <> "" Then Return DinaNETCore.ExtensionesM.AplicarTamanoARuta(RutaX, Tamano)
        Return ""
    End Function

    Public Function BuscarArchivo_Publico(ID As String, Tamano As String) As String
        Dim RutaX = BuscarArchivo_Publico(ID)
        If RutaX <> "" Then Return DinaNETCore.ExtensionesM.AplicarTamanoARuta(RutaX, Tamano)
        Return ""
    End Function


    Sub New(_RutaArchivosPublicos$, _RutaPrivados$, _RutaSubiendo$, Optional _ClaveFirmar As Byte() = Nothing)
        Me.RutaArchivosPublicos = _RutaArchivosPublicos
        Me.RutaPrivados = _RutaPrivados
        Me.RutaSubiendo = _RutaSubiendo
        If _ClaveFirmar Is Nothing OrElse _ClaveFirmar.Length < 3 Then _ClaveFirmar = DinaNetCore.ExtensionesM.CrearArrayDeBytesAleatorio(64)
        Me.HashKey = _ClaveFirmar
    End Sub

End Class
