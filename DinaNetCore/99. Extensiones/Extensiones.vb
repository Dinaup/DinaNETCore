Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices

Public Module ExtensionesM


    Private EnCulture As Globalization.CultureInfo = Globalization.CultureInfo.GetCultureInfo("en-en")
    Private ESCulture As Globalization.CultureInfo = Globalization.CultureInfo.GetCultureInfo("es-es")




    Public Function CrearContraseña(ByVal Tamano As Integer) As String
        Dim _caracteresPermitidos As String = "ÁÉÍÓÚáéíóú¨`äëüïöÄËÏÖÜ[]ªÆÿýèå½\Ýíñʑʤʡʟʪʧʟʞʛ|/-*ÇçabcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789-*$-+?_&=¡#@!%{}/"
        Dim NumerosAleatorios As New Random()
        Dim chars(Tamano - 1) As Char
        Dim tcuenta As Integer = _caracteresPermitidos.Length
        For i As Integer = 0 To Tamano - 1
            chars(i) = _caracteresPermitidos.Chars(CInt(Fix((_caracteresPermitidos.Length) * NumerosAleatorios.NextDouble())))
        Next i
        Return New String(chars)
    End Function


    Public Function CrearArrayDeBytesAleatorio(ByVal Tamano As Integer) As Byte()


        Dim R(Tamano - 1) As Byte
        Dim rnd = New Random()
        rnd.NextBytes(R)
        Return R

    End Function




    <Extension>
    Public Function Leer_Date(ElSTR As Dic(Of String, String), key$) As Date
        Return ElSTR.HacerMagia(key).ToDateDesdeMySQL_Local
    End Function



    <Extension>
    Public Function Leer_Time(ElSTR As Dic(Of String, String), key$) As DateTime
        Dim Nulable = ElSTR.Leer_Time_Nulable(key)
        If Nulable Is Nothing Then
            Dim retornar = Date.MinValue
            If retornar.Second > 0 OrElse retornar.Hour > 0 OrElse retornar.Minute > 0 Then
                retornar = retornar.AddDays(1)
                retornar = New Date(retornar.Year, retornar.Month, retornar.Day, 0, 0, 0)
            End If
            Return retornar
        Else
            Return Nulable.Value
        End If
    End Function


    <Extension>
    Public Function Leer_DateTime(ElSTR As Dic(Of String, String), key$) As Date
        Return ElSTR.HacerMagia(key).ToDateDesdeMySQL_utc
    End Function

    <Extension>
    Public Function Leer_String(ElSTR As Dic(Of String, String), key$) As String
        Return ElSTR.HacerMagia(key)
    End Function

    <Extension>
    Public Function Leer_Guid(ElSTR As Dic(Of String, String), key$) As Guid
        Return ElSTR.HacerMagia(key).ToGuid
    End Function

    <Extension>
    Public Function Leer_Integer(ElSTR As Dic(Of String, String), key$) As Integer
        Return ElSTR.HacerMagia(key).INT(0)
    End Function

    <Extension>
    Public Function Leer_Time_Nulable(ElSTR As Dic(Of String, String), key$) As DateTime?

        Dim r = ElSTR.HacerMagia(key)
        If r <> "" Then r = r.Trim
        If r = "" Then Return Nothing
        If r.Contains(":") = False Then Return Nothing
        If r.Contains(" ") Then r = r.RecibirUltimoTrozo(" "c)

        Dim Sep = r.Trim.Split(":"c)
        If Sep.Length = 3 OrElse Sep.Length = 2 Then

            Dim retornar = Date.MinValue
            If retornar.Second > 0 OrElse retornar.Hour > 0 OrElse retornar.Minute > 0 Then
                retornar = retornar.AddDays(1)
                retornar = New Date(retornar.Year, retornar.Month, retornar.Day, 0, 0, 0)
            End If

            Dim vHoras = Sep(0).INT(0)
            Dim vMinutos = Sep.HacerMagiaArray(1, "0").INT(0)
            Dim vSegundos = Sep.HacerMagiaArray(2, "0").INT(0)

            retornar = retornar.AddHours(vHoras)
            retornar = retornar.AddMinutes(vMinutos)
            retornar = retornar.AddSeconds(vSegundos)
            Return retornar

        End If


        Return Date.MinValue
    End Function

    <Extension>
    Public Function Leer_DateTime_Nulable(ElSTR As Dic(Of String, String), key$) As DateTime?
        Return ElSTR.HacerMagia(key).ToDateDesdeMySQLNulable_UTC
    End Function
    <Extension>
    Public Function Leer_Date_Nulable(ElSTR As Dic(Of String, String), key$) As DateTime?
        Return ElSTR.HacerMagia(key).ToDateDesdeMySQLNulable_Local
    End Function

    <Extension>
    Public Function Leer_Boolean(ElSTR As Dic(Of String, String), key$) As Boolean
        Return ElSTR.HacerMagia(key) = "1"
    End Function



    <Extension>
    Public Function Leer_Decimal(ElSTR As Dic(Of String, String), key$) As Decimal
        Return ElSTR.HacerMagia(key).DEC(0)
    End Function





    <Extension()>
    Public Function AdaptarURL(aString As String) As String
        If aString = "" Then Return ""
        Return Uri.EscapeUriString(aString)
    End Function



    <Extension()>
    Public Function AdaptarNombreArchivoWindows(aString As String) As String
        If aString = "" Then Return ""
        If aString.Contains(":") Then aString = aString.Replace(":", ".")
        For Each Actualff In IO.Path.GetInvalidFileNameChars
            aString = aString.Replace(Actualff.ToString, "")
        Next
        Return aString
    End Function


    <Extension>
    Public Function ToGuid(ElSTR As String) As System.Guid
        If ElSTR Is Nothing OrElse ElSTR.Length <> 36 OrElse ElSTR.IndexOf("-"c) = -1 Then
            Return Guid.Empty
        Else
            Dim r As Guid
            Guid.TryParse(ElSTR, r)
            Return r
        End If
    End Function



    Public Function NombreDeArchivoEsImagen(archivo As String) As Boolean
        If archivo = "" Then Return False
        archivo = archivo.ToLower
        If archivo.EndsWith(".gif") Then Return True
        If archivo.EndsWith(".apng") Then Return True
        If archivo.EndsWith(".png") Then Return True
        If archivo.EndsWith(".jpg") Then Return True
        If archivo.EndsWith(".jpeg") Then Return True
        If archivo.EndsWith(".svg") Then Return True
        Return False
    End Function
    Public Function GetMimeType(extension As String) As String
        If extension = "" Then Return "application/octet-stream"
        If extension.Contains(".") Then extension = extension.RecibirUltimoTrozo("."c)

        Select Case extension.ToLower()
            Case "323" : Return "text/h323"
            Case "3g2" : Return "video/3gpp2"
            Case "3gp" : Return "video/3gpp"
            Case "3gp2" : Return "video/3gpp2"
            Case "3gpp" : Return "video/3gpp"
            Case "7z" : Return "application/x-7z-compressed"
            Case "aa" : Return "audio/audible"
            Case "aac" : Return "audio/aac"
            Case "aaf" : Return "application/octet-stream"
            Case "aax" : Return "audio/vnd.audible.aax"
            Case "ac3" : Return "audio/ac3"
            Case "aca" : Return "application/octet-stream"
            Case "accda" : Return "application/msaccess.addin"
            Case "accdb" : Return "application/msaccess"
            Case "accdc" : Return "application/msaccess.cab"
            Case "accde" : Return "application/msaccess"
            Case "accdr" : Return "application/msaccess.runtime"
            Case "accdt" : Return "application/msaccess"
            Case "accdw" : Return "application/msaccess.webapplication"
            Case "accft" : Return "application/msaccess.ftemplate"
            Case "acx" : Return "application/internet-property-stream"
            Case "addin" : Return "text/xml"
            Case "ade" : Return "application/msaccess"
            Case "adobebridge" : Return "application/x-bridge-url"
            Case "adp" : Return "application/msaccess"
            Case "adt" : Return "audio/vnd.dlna.adts"
            Case "adts" : Return "audio/aac"
            Case "afm" : Return "application/octet-stream"
            Case "ai" : Return "application/postscript"
            Case "aif" : Return "audio/x-aiff"
            Case "aifc" : Return "audio/aiff"
            Case "aiff" : Return "audio/aiff"
            Case "air" : Return "application/vnd.adobe.air-application-installer-package+zip"
            Case "amc" : Return "application/x-mpeg"
            Case "application" : Return "application/x-ms-application"
            Case "asa" : Return "application/xml"
            Case "asax" : Return "application/xml"
            Case "ascx" : Return "application/xml"
            Case "asd" : Return "application/octet-stream"
            Case "asf" : Return "video/x-ms-asf"
            Case "ashx" : Return "application/xml"
            Case "asi" : Return "application/octet-stream"
            Case "asm" : Return "text/plain"
            Case "asmx" : Return "application/xml"
            Case "aspx" : Return "application/xml"
            Case "asr" : Return "video/x-ms-asf"
            Case "asx" : Return "video/x-ms-asf"
            Case "atom" : Return "application/atom+xml"
            Case "au" : Return "audio/basic"
            Case "avi" : Return "video/x-msvideo"
            Case "axs" : Return "application/olescript"
            Case "bas" : Return "text/plain"
            Case "bcpio" : Return "application/x-bcpio"
            Case "bin" : Return "application/octet-stream"
            Case "c" : Return "text/plain"
            Case "cab" : Return "application/octet-stream"
            Case "caf" : Return "audio/x-caf"
            Case "calx" : Return "application/vnd.ms-office.calx"
            Case "cat" : Return "application/vnd.ms-pki.seccat"
            Case "cc" : Return "text/plain"
            Case "cd" : Return "text/plain"
            Case "cdda" : Return "audio/aiff"
            Case "cdf" : Return "application/x-cdf"
            Case "cer" : Return "application/x-x509-ca-cert"
            Case "chm" : Return "application/octet-stream"
            Case "class" : Return "application/x-java-applet"
            Case "clp" : Return "application/x-msclip"
            Case "art" : Return "image/x-jg"
            Case "bmp" : Return "image/bmp"
            Case "cmx" : Return "image/x-cmx"
            Case "cod" : Return "image/cis-cod"
            Case "cnf" : Return "text/plain"
            Case "config" : Return "application/xml"
            Case "contact" : Return "text/x-ms-contact"
            Case "coverage" : Return "application/xml"
            Case "cpio" : Return "application/x-cpio"
            Case "cpp" : Return "text/plain"
            Case "crd" : Return "application/x-mscardfile"
            Case "crl" : Return "application/pkix-crl"
            Case "crt" : Return "application/x-x509-ca-cert"
            Case "cs" : Return "text/plain"
            Case "csdproj" : Return "text/plain"
            Case "csh" : Return "application/x-csh"
            Case "csproj" : Return "text/plain"
            Case "css" : Return "text/css"
            Case "csv" : Return "text/csv"
            Case "cur" : Return "application/octet-stream"
            Case "cxx" : Return "text/plain"
            Case "dat" : Return "application/octet-stream"
            Case "datasource" : Return "application/xml"
            Case "dbproj" : Return "text/plain"
            Case "dcr" : Return "application/x-director"
            Case "def" : Return "text/plain"
            Case "deploy" : Return "application/octet-stream"
            Case "der" : Return "application/x-x509-ca-cert"
            Case "dgml" : Return "application/xml"
            Case "dib" : Return "image/bmp"
            Case "dif" : Return "video/x-dv"
            Case "dir" : Return "application/x-director"
            Case "disco" : Return "text/xml"
            Case "dll" : Return "application/x-msdownload"
            Case "dll.config" : Return "text/xml"
            Case "dlm" : Return "text/dlm"
            Case "doc" : Return "application/msword"
            Case "docm" : Return "application/vnd.ms-word.document.macroenabled.12"
            Case "docx" : Return "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            Case "dot" : Return "application/msword"
            Case "dotm" : Return "application/vnd.ms-word.template.macroenabled.12"
            Case "dotx" : Return "application/vnd.openxmlformats-officedocument.wordprocessingml.template"
            Case "dsp" : Return "application/octet-stream"
            Case "dsw" : Return "text/plain"
            Case "dtd" : Return "text/xml"
            Case "dtsconfig" : Return "text/xml"
            Case "dv" : Return "video/x-dv"
            Case "dvi" : Return "application/x-dvi"
            Case "dwf" : Return "drawing/x-dwf"
            Case "dwp" : Return "application/octet-stream"
            Case "dxr" : Return "application/x-director"
            Case "eml" : Return "message/rfc822"
            Case "emz" : Return "application/octet-stream"
            Case "eot" : Return "application/octet-stream"
            Case "eps" : Return "application/postscript"
            Case "etl" : Return "application/etl"
            Case "etx" : Return "text/x-setext"
            Case "evy" : Return "application/envoy"
            Case "exe" : Return "application/octet-stream"
            Case "exe.config" : Return "text/xml"
            Case "fdf" : Return "application/vnd.fdf"
            Case "fif" : Return "application/fractals"
            Case "filters" : Return "application/xml"
            Case "fla" : Return "application/octet-stream"
            Case "flr" : Return "x-world/x-vrml"
            Case "flv" : Return "video/x-flv"
            Case "fsscript" : Return "application/fsharp-script"
            Case "fsx" : Return "application/fsharp-script"
            Case "generictest" : Return "application/xml"
            Case "gif" : Return "image/gif"
            Case "group" : Return "text/x-ms-group"
            Case "gsm" : Return "audio/x-gsm"
            Case "gtar" : Return "application/x-gtar"
            Case "gz" : Return "application/x-gzip"
            Case "h" : Return "text/plain"
            Case "hdf" : Return "application/x-hdf"
            Case "hdml" : Return "text/x-hdml"
            Case "hhc" : Return "application/x-oleobject"
            Case "hhk" : Return "application/octet-stream"
            Case "hhp" : Return "application/octet-stream"
            Case "hlp" : Return "application/winhlp"
            Case "hpp" : Return "text/plain"
            Case "hqx" : Return "application/mac-binhex40"
            Case "hta" : Return "application/hta"
            Case "htc" : Return "text/x-component"
            Case "htm" : Return "text/html"
            Case "html" : Return "text/html"
            Case "htt" : Return "text/webviewhtml"
            Case "hxa" : Return "application/xml"
            Case "hxc" : Return "application/xml"
            Case "hxd" : Return "application/octet-stream"
            Case "hxe" : Return "application/xml"
            Case "hxf" : Return "application/xml"
            Case "hxh" : Return "application/octet-stream"
            Case "hxi" : Return "application/octet-stream"
            Case "hxk" : Return "application/xml"
            Case "hxq" : Return "application/octet-stream"
            Case "hxr" : Return "application/octet-stream"
            Case "hxs" : Return "application/octet-stream"
            Case "hxt" : Return "text/html"
            Case "hxv" : Return "application/xml"
            Case "hxw" : Return "application/octet-stream"
            Case "hxx" : Return "text/plain"
            Case "i" : Return "text/plain"
            Case "ico" : Return "image/x-icon"
            Case "ics" : Return "application/octet-stream"
            Case "idl" : Return "text/plain"
            Case "ief" : Return "image/ief"
            Case "iii" : Return "application/x-iphone"
            Case "inc" : Return "text/plain"
            Case "inf" : Return "application/octet-stream"
            Case "inl" : Return "text/plain"
            Case "ins" : Return "application/x-internet-signup"
            Case "ipa" : Return "application/x-itunes-ipa"
            Case "ipg" : Return "application/x-itunes-ipg"
            Case "ipproj" : Return "text/plain"
            Case "ipsw" : Return "application/x-itunes-ipsw"
            Case "iqy" : Return "text/x-ms-iqy"
            Case "isp" : Return "application/x-internet-signup"
            Case "ite" : Return "application/x-itunes-ite"
            Case "itlp" : Return "application/x-itunes-itlp"
            Case "itms" : Return "application/x-itunes-itms"
            Case "itpc" : Return "application/x-itunes-itpc"
            Case "ivf" : Return "video/x-ivf"
            Case "jar" : Return "application/java-archive"
            Case "java" : Return "application/octet-stream"
            Case "jck" : Return "application/liquidmotion"
            Case "jcz" : Return "application/liquidmotion"
            Case "jfif" : Return "image/pjpeg"
            Case "jnlp" : Return "application/x-java-jnlp-file"
            Case "jpb" : Return "application/octet-stream"
            Case "jpe" : Return "image/jpeg"
            Case "jpeg" : Return "image/jpeg"
            Case "jpg" : Return "image/jpeg"
            Case "js" : Return "application/x-javascript"
            Case "jsx" : Return "text/jscript"
            Case "jsxbin" : Return "text/plain"
            Case "latex" : Return "application/x-latex"
            Case "library-ms" : Return "application/windows-library+xml"
            Case "lit" : Return "application/x-ms-reader"
            Case "loadtest" : Return "application/xml"
            Case "lpk" : Return "application/octet-stream"
            Case "lsf" : Return "video/x-la-asf"
            Case "lst" : Return "text/plain"
            Case "lsx" : Return "video/x-la-asf"
            Case "lzh" : Return "application/octet-stream"
            Case "m13" : Return "application/x-msmediaview"
            Case "m14" : Return "application/x-msmediaview"
            Case "m1v" : Return "video/mpeg"
            Case "m2t" : Return "video/vnd.dlna.mpeg-tts"
            Case "m2ts" : Return "video/vnd.dlna.mpeg-tts"
            Case "m2v" : Return "video/mpeg"
            Case "m3u" : Return "audio/x-mpegurl"
            Case "m3u8" : Return "audio/x-mpegurl"
            Case "m4a" : Return "audio/m4a"
            Case "m4b" : Return "audio/m4b"
            Case "m4p" : Return "audio/m4p"
            Case "m4r" : Return "audio/x-m4r"
            Case "m4v" : Return "video/x-m4v"
            Case "mac" : Return "image/x-macpaint"
            Case "mak" : Return "text/plain"
            Case "man" : Return "application/x-troff-man"
            Case "manifest" : Return "application/x-ms-manifest"
            Case "map" : Return "text/plain"
            Case "master" : Return "application/xml"
            Case "mda" : Return "application/msaccess"
            Case "mdb" : Return "application/x-msaccess"
            Case "mde" : Return "application/msaccess"
            Case "mdp" : Return "application/octet-stream"
            Case "me" : Return "application/x-troff-me"
            Case "mfp" : Return "application/x-shockwave-flash"
            Case "mht" : Return "message/rfc822"
            Case "mhtml" : Return "message/rfc822"
            Case "mid" : Return "audio/mid"
            Case "midi" : Return "audio/mid"
            Case "mix" : Return "application/octet-stream"
            Case "mk" : Return "text/plain"
            Case "mmf" : Return "application/x-smaf"
            Case "mno" : Return "text/xml"
            Case "mny" : Return "application/x-msmoney"
            Case "mod" : Return "video/mpeg"
            Case "mov" : Return "video/quicktime"
            Case "movie" : Return "video/x-sgi-movie"
            Case "mp2" : Return "video/mpeg"
            Case "mp2v" : Return "video/mpeg"
            Case "mp3" : Return "audio/mpeg"
            Case "mp4" : Return "video/mp4"
            Case "mp4v" : Return "video/mp4"
            Case "mpa" : Return "video/mpeg"
            Case "mpe" : Return "video/mpeg"
            Case "mpeg" : Return "video/mpeg"
            Case "mpf" : Return "application/vnd.ms-mediapackage"
            Case "mpg" : Return "video/mpeg"
            Case "mpp" : Return "application/vnd.ms-project"
            Case "mpv2" : Return "video/mpeg"
            Case "mqv" : Return "video/quicktime"
            Case "ms" : Return "application/x-troff-ms"
            Case "msi" : Return "application/octet-stream"
            Case "mso" : Return "application/octet-stream"
            Case "mts" : Return "video/vnd.dlna.mpeg-tts"
            Case "mtx" : Return "application/xml"
            Case "mvb" : Return "application/x-msmediaview"
            Case "mvc" : Return "application/x-miva-compiled"
            Case "mxp" : Return "application/x-mmxp"
            Case "nc" : Return "application/x-netcdf"
            Case "nsc" : Return "video/x-ms-asf"
            Case "nws" : Return "message/rfc822"
            Case "ocx" : Return "application/octet-stream"
            Case "oda" : Return "application/oda"
            Case "odc" : Return "text/x-ms-odc"
            Case "odh" : Return "text/plain"
            Case "odl" : Return "text/plain"
            Case "odp" : Return "application/vnd.oasis.opendocument.presentation"
            Case "ods" : Return "application/oleobject"
            Case "odt" : Return "application/vnd.oasis.opendocument.text"
            Case "one" : Return "application/onenote"
            Case "onea" : Return "application/onenote"
            Case "onepkg" : Return "application/onenote"
            Case "onetmp" : Return "application/onenote"
            Case "onetoc" : Return "application/onenote"
            Case "onetoc2" : Return "application/onenote"
            Case "orderedtest" : Return "application/xml"
            Case "osdx" : Return "application/opensearchdescription+xml"
            Case "p10" : Return "application/pkcs10"
            Case "p12" : Return "application/x-pkcs12"
            Case "p7b" : Return "application/x-pkcs7-certificates"
            Case "p7c" : Return "application/pkcs7-mime"
            Case "p7m" : Return "application/pkcs7-mime"
            Case "p7r" : Return "application/x-pkcs7-certreqresp"
            Case "p7s" : Return "application/pkcs7-signature"
            Case "pbm" : Return "image/x-portable-bitmap"
            Case "pcast" : Return "application/x-podcast"
            Case "pct" : Return "image/pict"
            Case "pcx" : Return "application/octet-stream"
            Case "pcz" : Return "application/octet-stream"
            Case "pdf" : Return "application/pdf"
            Case "pfb" : Return "application/octet-stream"
            Case "pfm" : Return "application/octet-stream"
            Case "pfx" : Return "application/x-pkcs12"
            Case "pgm" : Return "image/x-portable-graymap"
            Case "pic" : Return "image/pict"
            Case "pict" : Return "image/pict"
            Case "pkgdef" : Return "text/plain"
            Case "pkgundef" : Return "text/plain"
            Case "pko" : Return "application/vnd.ms-pki.pko"
            Case "pls" : Return "audio/scpls"
            Case "pma" : Return "application/x-perfmon"
            Case "pmc" : Return "application/x-perfmon"
            Case "pml" : Return "application/x-perfmon"
            Case "pmr" : Return "application/x-perfmon"
            Case "pmw" : Return "application/x-perfmon"
            Case "png" : Return "image/png"
            Case "pnm" : Return "image/x-portable-anymap"
            Case "pnt" : Return "image/x-macpaint"
            Case "pntg" : Return "image/x-macpaint"
            Case "pnz" : Return "image/png"
            Case "pot" : Return "application/vnd.ms-powerpoint"
            Case "potm" : Return "application/vnd.ms-powerpoint.template.macroenabled.12"
            Case "potx" : Return "application/vnd.openxmlformats-officedocument.presentationml.template"
            Case "ppa" : Return "application/vnd.ms-powerpoint"
            Case "ppam" : Return "application/vnd.ms-powerpoint.addin.macroenabled.12"
            Case "ppm" : Return "image/x-portable-pixmap"
            Case "pps" : Return "application/vnd.ms-powerpoint"
            Case "ppsm" : Return "application/vnd.ms-powerpoint.slideshow.macroenabled.12"
            Case "ppsx" : Return "application/vnd.openxmlformats-officedocument.presentationml.slideshow"
            Case "ppt" : Return "application/vnd.ms-powerpoint"
            Case "pptm" : Return "application/vnd.ms-powerpoint.presentation.macroenabled.12"
            Case "pptx" : Return "application/vnd.openxmlformats-officedocument.presentationml.presentation"
            Case "prf" : Return "application/pics-rules"
            Case "prm" : Return "application/octet-stream"
            Case "prx" : Return "application/octet-stream"
            Case "ps" : Return "application/postscript"
            Case "psc1" : Return "application/powershell"
            Case "psd" : Return "application/octet-stream"
            Case "psess" : Return "application/xml"
            Case "psm" : Return "application/octet-stream"
            Case "psp" : Return "application/octet-stream"
            Case "pub" : Return "application/x-mspublisher"
            Case "pwz" : Return "application/vnd.ms-powerpoint"
            Case "qht" : Return "text/x-html-insertion"
            Case "qhtm" : Return "text/x-html-insertion"
            Case "qt" : Return "video/quicktime"
            Case "qti" : Return "image/x-quicktime"
            Case "qtif" : Return "image/x-quicktime"
            Case "qtl" : Return "application/x-quicktimeplayer"
            Case "qxd" : Return "application/octet-stream"
            Case "ra" : Return "audio/x-pn-realaudio"
            Case "ram" : Return "audio/x-pn-realaudio"
            Case "rar" : Return "application/octet-stream"
            Case "ras" : Return "image/x-cmu-raster"
            Case "rat" : Return "application/rat-file"
            Case "rc" : Return "text/plain"
            Case "rc2" : Return "text/plain"
            Case "rct" : Return "text/plain"
            Case "rdlc" : Return "application/xml"
            Case "resx" : Return "application/xml"
            Case "rf" : Return "image/vnd.rn-realflash"
            Case "rgb" : Return "image/x-rgb"
            Case "rgs" : Return "text/plain"
            Case "rm" : Return "application/vnd.rn-realmedia"
            Case "rmi" : Return "audio/mid"
            Case "rmp" : Return "application/vnd.rn-rn_music_package"
            Case "roff" : Return "application/x-troff"
            Case "rpm" : Return "audio/x-pn-realaudio-plugin"
            Case "rqy" : Return "text/x-ms-rqy"
            Case "rtf" : Return "application/rtf"
            Case "rtx" : Return "text/richtext"
            Case "ruleset" : Return "application/xml"
            Case "s" : Return "text/plain"
            Case "safariextz" : Return "application/x-safari-safariextz"
            Case "scd" : Return "application/x-msschedule"
            Case "sct" : Return "text/scriptlet"
            Case "sd2" : Return "audio/x-sd2"
            Case "sdp" : Return "application/sdp"
            Case "sea" : Return "application/octet-stream"
            Case "searchconnector-ms" : Return "application/windows-search-connector+xml"
            Case "setpay" : Return "application/set-payment-initiation"
            Case "setreg" : Return "application/set-registration-initiation"
            Case "settings" : Return "application/xml"
            Case "sgimb" : Return "application/x-sgimb"
            Case "sgml" : Return "text/sgml"
            Case "sh" : Return "application/x-sh"
            Case "shar" : Return "application/x-shar"
            Case "shtml" : Return "text/html"
            Case "sit" : Return "application/x-stuffit"
            Case "sitemap" : Return "application/xml"
            Case "skin" : Return "application/xml"
            Case "sldm" : Return "application/vnd.ms-powerpoint.slide.macroenabled.12"
            Case "sldx" : Return "application/vnd.openxmlformats-officedocument.presentationml.slide"
            Case "slk" : Return "application/vnd.ms-excel"
            Case "sln" : Return "text/plain"
            Case "slupkg-ms" : Return "application/x-ms-license"
            Case "smd" : Return "audio/x-smd"
            Case "smi" : Return "application/octet-stream"
            Case "smx" : Return "audio/x-smd"
            Case "smz" : Return "audio/x-smd"
            Case "snd" : Return "audio/basic"
            Case "snippet" : Return "application/xml"
            Case "snp" : Return "application/octet-stream"
            Case "sol" : Return "text/plain"
            Case "sor" : Return "text/plain"
            Case "spc" : Return "application/x-pkcs7-certificates"
            Case "spl" : Return "application/futuresplash"
            Case "src" : Return "application/x-wais-source"
            Case "srf" : Return "text/plain"
            Case "ssisdeploymentmanifest" : Return "text/xml"
            Case "ssm" : Return "application/streamingmedia"
            Case "sst" : Return "application/vnd.ms-pki.certstore"
            Case "stl" : Return "application/vnd.ms-pki.stl"
            Case "sv4cpio" : Return "application/x-sv4cpio"
            Case "sv4crc" : Return "application/x-sv4crc"
            Case "svc" : Return "application/xml"
            Case "swf" : Return "application/x-shockwave-flash"
            Case "t" : Return "application/x-troff"
            Case "tar" : Return "application/x-tar"
            Case "tcl" : Return "application/x-tcl"
            Case "testrunconfig" : Return "application/xml"
            Case "testsettings" : Return "application/xml"
            Case "tex" : Return "application/x-tex"
            Case "texi" : Return "application/x-texinfo"
            Case "texinfo" : Return "application/x-texinfo"
            Case "tgz" : Return "application/x-compressed"
            Case "thmx" : Return "application/vnd.ms-officetheme"
            Case "thn" : Return "application/octet-stream"
            Case "tif" : Return "image/tiff"
            Case "tiff" : Return "image/tiff"
            Case "tlh" : Return "text/plain"
            Case "tli" : Return "text/plain"
            Case "toc" : Return "application/octet-stream"
            Case "tr" : Return "application/x-troff"
            Case "trm" : Return "application/x-msterminal"
            Case "trx" : Return "application/xml"
            Case "ts" : Return "video/vnd.dlna.mpeg-tts"
            Case "tsv" : Return "text/tab-separated-values"
            Case "ttf" : Return "application/octet-stream"
            Case "tts" : Return "video/vnd.dlna.mpeg-tts"
            Case "txt" : Return "text/plain"
            Case "u32" : Return "application/octet-stream"
            Case "uls" : Return "text/iuls"
            Case "user" : Return "text/plain"
            Case "ustar" : Return "application/x-ustar"
            Case "vb" : Return "text/plain"
            Case "vbdproj" : Return "text/plain"
            Case "vbk" : Return "video/mpeg"
            Case "vbproj" : Return "text/plain"
            Case "vbs" : Return "text/vbscript"
            Case "vcf" : Return "text/x-vcard"
            Case "vcproj" : Return "application/xml"
            Case "vcs" : Return "text/plain"
            Case "vcxproj" : Return "application/xml"
            Case "vddproj" : Return "text/plain"
            Case "vdp" : Return "text/plain"
            Case "vdproj" : Return "text/plain"
            Case "vdx" : Return "application/vnd.ms-visio.viewer"
            Case "vml" : Return "text/xml"
            Case "vscontent" : Return "application/xml"
            Case "vsct" : Return "text/xml"
            Case "vsd" : Return "application/vnd.visio"
            Case "vsi" : Return "application/ms-vsi"
            Case "vsix" : Return "application/vsix"
            Case "vsixlangpack" : Return "text/xml"
            Case "vsixmanifest" : Return "text/xml"
            Case "vsmdi" : Return "application/xml"
            Case "vspscc" : Return "text/plain"
            Case "vss" : Return "application/vnd.visio"
            Case "vsscc" : Return "text/plain"
            Case "vssettings" : Return "text/xml"
            Case "vssscc" : Return "text/plain"
            Case "vst" : Return "application/vnd.visio"
            Case "vstemplate" : Return "text/xml"
            Case "vsto" : Return "application/x-ms-vsto"
            Case "vsw" : Return "application/vnd.visio"
            Case "vsx" : Return "application/vnd.visio"
            Case "vtx" : Return "application/vnd.visio"
            Case "wav" : Return "audio/wav"
            Case "wave" : Return "audio/wav"
            Case "wax" : Return "audio/x-ms-wax"
            Case "wbk" : Return "application/msword"
            Case "wbmp" : Return "image/vnd.wap.wbmp"
            Case "wcm" : Return "application/vnd.ms-works"
            Case "wdb" : Return "application/vnd.ms-works"
            Case "wdp" : Return "image/vnd.ms-photo"
            Case "webarchive" : Return "application/x-safari-webarchive"
            Case "webtest" : Return "application/xml"
            Case "wiq" : Return "application/xml"
            Case "wiz" : Return "application/msword"
            Case "wks" : Return "application/vnd.ms-works"
            Case "wlmp" : Return "application/wlmoviemaker"
            Case "wlpginstall" : Return "application/x-wlpg-detect"
            Case "wlpginstall3" : Return "application/x-wlpg3-detect"
            Case "wm" : Return "video/x-ms-wm"
            Case "wma" : Return "audio/x-ms-wma"
            Case "wmd" : Return "application/x-ms-wmd"
            Case "wmf" : Return "application/x-msmetafile"
            Case "wml" : Return "text/vnd.wap.wml"
            Case "wmlc" : Return "application/vnd.wap.wmlc"
            Case "wmls" : Return "text/vnd.wap.wmlscript"
            Case "wmlsc" : Return "application/vnd.wap.wmlscriptc"
            Case "wmp" : Return "video/x-ms-wmp"
            Case "wmv" : Return "video/x-ms-wmv"
            Case "wmx" : Return "video/x-ms-wmx"
            Case "wmz" : Return "application/x-ms-wmz"
            Case "wpl" : Return "application/vnd.ms-wpl"
            Case "wps" : Return "application/vnd.ms-works"
            Case "wri" : Return "application/x-mswrite"
            Case "wrl" : Return "x-world/x-vrml"
            Case "wrz" : Return "x-world/x-vrml"
            Case "wsc" : Return "text/scriptlet"
            Case "wsdl" : Return "text/xml"
            Case "wvx" : Return "video/x-ms-wvx"
            Case "x" : Return "application/directx"
            Case "xaf" : Return "x-world/x-vrml"
            Case "xaml" : Return "application/xaml+xml"
            Case "xap" : Return "application/x-silverlight-app"
            Case "xbap" : Return "application/x-ms-xbap"
            Case "xbm" : Return "image/x-xbitmap"
            Case "xdr" : Return "text/plain"
            Case "xht" : Return "application/xhtml+xml"
            Case "xhtml" : Return "application/xhtml+xml"
            Case "xla" : Return "application/vnd.ms-excel"
            Case "xlam" : Return "application/vnd.ms-excel.addin.macroenabled.12"
            Case "xlc" : Return "application/vnd.ms-excel"
            Case "xld" : Return "application/vnd.ms-excel"
            Case "xlk" : Return "application/vnd.ms-excel"
            Case "xll" : Return "application/vnd.ms-excel"
            Case "xlm" : Return "application/vnd.ms-excel"
            Case "xls" : Return "application/vnd.ms-excel"
            Case "xlsb" : Return "application/vnd.ms-excel.sheet.binary.macroenabled.12"
            Case "xlsm" : Return "application/vnd.ms-excel.sheet.macroenabled.12"
            Case "xlsx" : Return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Case "xlt" : Return "application/vnd.ms-excel"
            Case "xltm" : Return "application/vnd.ms-excel.template.macroenabled.12"
            Case "xltx" : Return "application/vnd.openxmlformats-officedocument.spreadsheetml.template"
            Case "xlw" : Return "application/vnd.ms-excel"
            Case "xml" : Return "text/xml"
            Case "xmta" : Return "application/xml"
            Case "xof" : Return "x-world/x-vrml"
            Case "xoml" : Return "text/plain"
            Case "xpm" : Return "image/x-xpixmap"
            Case "xps" : Return "application/vnd.ms-xpsdocument"
            Case "xrm-ms" : Return "text/xml"
            Case "xsc" : Return "application/xml"
            Case "xsd" : Return "text/xml"
            Case "xsf" : Return "text/xml"
            Case "xsl" : Return "text/xml"
            Case "xslt" : Return "text/xml"
            Case "xsn" : Return "application/octet-stream"
            Case "xss" : Return "application/xml"
            Case "xtp" : Return "application/octet-stream"
            Case "xwd" : Return "image/x-xwindowdump"
            Case "z" : Return "application/x-compress"
            Case "zip" : Return "application/x-zip-compressed"
            Case Else
                Return "application/octet-stream"
        End Select
    End Function


    Public Function RecibirSHA1(ByRef ByteArrays() As Byte) As String
        If ByteArrays Is Nothing Then Return ""


        Dim sha1Obj As New System.Security.Cryptography.SHA1CryptoServiceProvider
        Dim bytesToHash() As Byte = sha1Obj.ComputeHash(ByteArrays)
        Dim Sha1Fin As String = ""
        For Each b As Byte In bytesToHash
            Sha1Fin += b.ToString("x2")
        Next
        sha1Obj.Dispose()

        Return Sha1Fin
    End Function



    Public Function RecibirSHA1_Archivo(ByRef Archivo As String) As String
        If Archivo = "" Then Return ""
        Using x = New System.IO.FileStream(Archivo, FileMode.Open)
            Dim sha1Obj As New Security.Cryptography.SHA1CryptoServiceProvider
            Dim bytesToHash() As Byte = sha1Obj.ComputeHash(x)
            Dim Sha1Fin As String = ""
            For Each b As Byte In bytesToHash
                Sha1Fin += b.ToString("x2")
            Next
            sha1Obj.Dispose()
            Return Sha1Fin
        End Using
    End Function
    Public Function RecibirSHA1(ByRef texto As String) As String
        If texto = "" Then Return ""

        Dim sha1Obj As New Security.Cryptography.SHA1CryptoServiceProvider
        Dim bytesToHash() As Byte = sha1Obj.ComputeHash(System.Text.Encoding.UTF8.GetBytes(texto))
        Dim Sha1Fin As String = ""
        For Each b As Byte In bytesToHash
            Sha1Fin += b.ToString("x2")
        Next
        sha1Obj.Dispose()
        Return Sha1Fin
    End Function

    Public Function RecibirSHA1(ByRef STREAM As Stream) As String
        If STREAM Is Nothing Then Return ""
        Dim sha1Obj As New Security.Cryptography.SHA1CryptoServiceProvider
        Dim bytesToHash() As Byte = sha1Obj.ComputeHash(STREAM)
        Dim Sha1Fin As String = ""
        For Each b As Byte In bytesToHash
            Sha1Fin += b.ToString("x2")
        Next
        sha1Obj.Dispose()
        Return Sha1Fin
    End Function




#Region "Conversiones"

#End Region

    '!+ .LNG()
    <Extension()>
    Public Function LNG(o As String) As Long
        Return CType(o, Long)
    End Function
    <Extension()>
    Public Function LNG(o As String, SiFalle As Long) As Long
        Dim Retornar As Long
        If Long.TryParse(o, Globalization.NumberStyles.Number, EnCulture, Retornar) = False Then
            Retornar = SiFalle
        End If
        Return Retornar
    End Function

    '!+ .INT()
    <Extension()>
    Public Function INT(o As Boolean) As Integer
        Return If(o, 1, 0)
    End Function

    <Extension()>
    Public Function INT(o As IntPtr) As Integer
        Return CInt(o)
    End Function

    <Extension()>
    Public Function INT(o As Object) As Integer
        Return CInt(o)
    End Function

    <Extension()>
    Public Function DEC(o As String) As Decimal
        Return Decimal.Parse(o, Globalization.NumberStyles.Number, EnCulture)
    End Function

    <Extension()>
    Public Function DEC(o As String, SiFalla As Decimal) As Decimal
        Dim Retornar As Decimal
        If Decimal.TryParse(o, Globalization.NumberStyles.Number, EnCulture, Retornar) = False Then
            Retornar = SiFalla
        End If
        Return Retornar
    End Function


    <Extension()>
    Public Function INT(o As Char, Defecto As Integer) As Integer
        If o = "0"c Then Return 0
        If o = "1"c Then Return 1
        If o = "2"c Then Return 2
        If o = "3"c Then Return 3
        If o = "4"c Then Return 4
        If o = "5"c Then Return 5
        If o = "6"c Then Return 6
        If o = "7"c Then Return 7
        If o = "8"c Then Return 8
        If o = "9"c Then Return 9
        Return Defecto
    End Function


    <Extension()>
    Public Function INT(o As String, ExcepcionSiFalla As String) As Integer
        Dim R As Integer
        If Integer.TryParse(o, R) Then
            Return R
        End If
        Throw New Exception(ExcepcionSiFalla)
    End Function

    <Extension()>
    Public Function INT(o As String) As Integer
        Return CInt(o)
    End Function

    <Extension()>
    Public Function INT(o As String, Minimo As Integer, Maximo As Integer, SiFalla As Integer) As Integer

        Dim Actual As Integer
        If Integer.TryParse(o, Actual) Then
            If Actual < Minimo OrElse Actual > Maximo Then
                Return SiFalla
            Else
                Return Actual
            End If
        End If

        Return SiFalla
    End Function

    <Extension()>
    Public Function INT(o As String, SiPeta As Integer) As Integer
        Dim Retornar As Integer = 0
        If Integer.TryParse(o, Retornar) Then
            Return Retornar
        Else
            Return SiPeta
        End If
    End Function

    <Extension()>
    Public Function INT64(o As String, SiPeta As Long) As Long
        Dim Retornar As Long
        If Long.TryParse(o, Retornar) Then
            Return Retornar
        Else
            Return SiPeta
        End If
    End Function

    <Extension()>
    Public Function INT64(o As String) As Long
        Return Long.Parse(o)
    End Function



    <Extension()>
    Public Function STR(BytesArray() As Byte) As String
        If BytesArray Is Nothing Then Return ""
        Return System.Text.Encoding.UTF8.GetString(BytesArray)
    End Function

    <Extension()>
    Public Function ToArrayBytes(Texto As String) As Byte()
        Return System.Text.Encoding.UTF8.GetBytes(Texto)
    End Function




    <Extension()>
    Public Function UppercaseFirst(s As String) As String
        If String.IsNullOrEmpty(s) Then Return String.Empty
        Return Char.ToUpper(s(0)) & s.Substring(1)
    End Function




    '!+ .BOOL()
    <Extension()>
    Public Function BOOL(o As String) As Boolean
        If o = "" Then Return False
        If o.Equals("1") Then Return True
        If o.Equals("0") Then Return False
        If o.Length = 2 Then Return o.EqualsIgnoreCase("si") OrElse o.EqualsIgnoreCase("sí") OrElse o.EqualsIgnoreCase("on") 'OrElse
        If o.Length.Equals(3) Then Return o.EqualsIgnoreCase("yes")
        If o.Length.Equals(4) Then Return o.EqualsIgnoreCase("true")
        Return False
    End Function


    <Extension()>
    Public Function STR(o As Boolean) As String
        Return If(o, "1", "0")
    End Function

    <Extension()>
    Public Function STR(o As Decimal?) As String
        If o Is Nothing Then Return ""
        Return o.Value.STR
    End Function

    <Extension()>
    Public Function STR(o As Integer?) As String
        If o Is Nothing Then Return ""
        Return o.ToString
    End Function

    <Extension()>
    Public Function STR(o As Date?) As String
        Return If(o Is Nothing, "", o.Value.STR)
    End Function




    <Extension()>
    Public Function ToDateTime_Local(o As String) As Date
        Return o.ToDateDesdeMySQL_Local
    End Function


    <Extension()>
    Public Function ToDateTime_UTC(o As String) As Date
        Return o.ToDateDesdeMySQL_utc
    End Function

    <Extension()>
    Public Function ToDate(o As String) As Date
        Return o.ToDateDesdeMySQL_Local.Date
    End Function




    <Extension()>
    Public Function ToDateTime_Local(o As Newtonsoft.Json.Linq.JToken) As Date?
        If o Is Nothing Then Return Nothing
        Return o.STR.ToDateDesdeMySQLNulable_Local
    End Function


    <Extension()>
    Public Function ToDateTime_UTC(o As Newtonsoft.Json.Linq.JToken) As Date?
        If o Is Nothing Then Return Nothing
        Return o.STR.ToDateDesdeMySQLNulable_UTC
    End Function

    <Extension()>
    Public Function ToDate(o As Newtonsoft.Json.Linq.JToken) As Date?
        If o Is Nothing Then Return Nothing
        Dim R = o.STR.ToDateDesdeMySQLNulable_Local
        If R Is Nothing Then Return Nothing
        Return R.Value.Date
    End Function







    <Extension()>
    Public Function STR(o As Decimal) As String
        Return o.ToString("0.####################", EnCulture)
    End Function


    <Extension()>
    Public Function DEC(o As Newtonsoft.Json.Linq.JToken) As Decimal
        If o Is Nothing Then Return 0
        Return o.STR.DEC(0)
    End Function

    <Extension()>
    Public Function BOOL(o As Newtonsoft.Json.Linq.JToken) As Boolean
        If o Is Nothing Then Return False
        Return CType(o, Boolean)
    End Function



    <Extension()>
    Public Function ToGUID(o As Newtonsoft.Json.Linq.JToken) As System.Guid
        If o Is Nothing Then Return Guid.Empty
        Return o.STR.ToGuid
    End Function

    <Extension()>
    Public Function STR(o As Newtonsoft.Json.Linq.JToken) As String
        If o IsNot Nothing Then Return o.ToString
        Return ""
    End Function
    <Extension()>
    Public Function INT(o As Newtonsoft.Json.Linq.JToken) As Integer
        If o IsNot Nothing Then Return o.ToString.INT
        Return 0
    End Function

    <Extension()>
    Public Function INT(o As Newtonsoft.Json.Linq.JToken, V%) As Integer
        If o IsNot Nothing Then Return o.ToString.INT(V)
        Return V
    End Function


    <Extension()>
    Public Function NoEsNulo(o As Newtonsoft.Json.Linq.JToken) As Boolean
        Return o IsNot Nothing AndAlso o.HasValues
    End Function



    <Extension()>
    Public Function STR(o As Guid) As String
        Return If(o = Guid.Empty, "", o.ToString)
    End Function

    <Extension()>
    Public Function STR(o As Object) As String
        Return If(o Is Nothing, "", o.ToString)
    End Function

    <Extension()>
    Public Function STR(o As String) As String
        Return If(o Is Nothing, "", o)
    End Function

    <Extension()>
    Public Function STR(o As Integer) As String
        Return o.ToString
    End Function


    <Extension()>
    Public Function STR(stream As Stream) As String

        Dim Retornar As String

        Dim pos = stream.Position
        stream.Position = 0

        Using reader As New StreamReader(stream)
            Retornar = reader.ReadToEnd()
        End Using

        stream.Position = pos

        Return Retornar
    End Function

    <Extension()>
    Public Function STR(o As Date) As String
        Return o.AdaptarMySQL
    End Function



    <Extension()>
    Public Function CLong(o As String) As Long
        Return CType(o, Long)
    End Function



    <Extension()>
    Public Function CLong(o As Integer) As Long
        Return CType(o, Long)
    End Function

    <Extension()>
    Public Function CLong(o As Decimal) As Long
        Return CType(o, Long)
    End Function
    <Extension()>
    Public Function CLong(o As Single) As Long
        Return CType(o, Long)
    End Function

    <Extension()>
    Public Function CLong(o As Double) As Long
        Return CType(o, Long)
    End Function


#Region "SQL"

#End Region







    <Extension()>
    Public Function AdaptarHTML(aString As String) As String
        Return If(aString = "", "", System.Net.WebUtility.HtmlEncode(aString))
    End Function





    <Extension()>
    Public Function AdaptarMySQL(aString As Integer) As String
        Return aString.ToString
    End Function

    <Extension()>
    Public Function AdaptarMySQL(aString As Decimal) As String
        Return aString.STR.Replace(",", ".")
    End Function

    <Extension()>
    Public Function AdaptarMySQL(aString As Single) As String
        Return aString.STR.Replace(",", ".")
    End Function

    <Extension()>
    Public Function AdaptarMySQL(aString As Boolean) As String
        Return If(aString, "1", "0")
    End Function


    <Extension()>
    Public Function AdaptarMySQL_Date(o As Date) As String
        Return o.ToString("yyyy-MM-dd")
    End Function



    <Extension()>
    Public Function AdaptarMySQL_Date_Nulable(o As Date?) As String
        If o Is Nothing Then Return ""
        Return o.Value.ToString("yyyy-MM-dd")
    End Function

    <Extension()>
    Public Function AdaptarMySQL_DateTime_Nulable(o As DateTime?) As String
        If o Is Nothing Then Return ""
        Return o.Value.AdaptarMySQL
    End Function

    <Extension()>
    Public Function AdaptarMySQL_Time_Nulable(o As DateTime?) As String
        If o Is Nothing Then Return ""
        Return o.Value.AdaptarMySQL_HoraConSegundos
    End Function
    <Extension()>
    Public Function AdaptarMySQL_Time(o As DateTime?) As String
        If o Is Nothing Then Return ""
        Return o.Value.AdaptarMySQL_HoraConSegundos
    End Function


    <Extension()>
    Public Function AdaptarMySQL_String(o As String) As String
        Return o.AdaptarMySQL
    End Function

    <Extension()>
    Public Function AdaptarMySQL_Boolean(o As Boolean) As String
        Return o.AdaptarMySQL
    End Function



    <Extension()>
    Public Function AdaptarMySQL_Integer(o As Integer) As String
        Return o.AdaptarMySQL
    End Function



    <Extension()>
    Public Function AdaptarMySQL_DateTime(o As DateTime) As String
        Return o.AdaptarMySQL
    End Function


    <Extension()>
    Public Function AdaptarMySQL_Decimal(o As Decimal) As String
        Return o.AdaptarMySQL
    End Function



    <Extension()>
    Public Function AdaptarMySQL_Guid(o As Guid) As String
        Return o.AdaptarMySQL
    End Function




    <Extension()>
    Public Function AdaptarMySQL(o As Long) As String
        Return o.ToString
    End Function

    <Extension()>
    Public Function AdaptarMySQL(o As Guid) As String
        Return o.STR
    End Function

    <Extension()>
    Public Function AdaptarMySQL(aString As Date?) As String
        If aString Is Nothing Then
            Return ""
        Else
            Return aString.Value.AdaptarMySQL
        End If
    End Function

    <Extension()>
    Public Function AdaptarMySQL(aString As String) As String
        If aString = "" Then
            Return ""
        ElseIf aString.Contains("\"c) Or aString.Contains("'"c) Then
            Return aString.Replace("\", "\\").Replace("'", "\'")
        Else
            Return aString
        End If
    End Function

    <Extension()>
    Public Function AdaptarMySQL(o As Date) As String
        Return o.ToString("yyyy-MM-dd HH:mm:ss")
    End Function

    <Extension()>
    Public Function AdaptarNombreArchivo_DateTime(o As Date) As String
        Return o.Day & "." & (o.Month) & "." & o.Year & " " & o.Hour & "." & o.Minute
    End Function


    <Extension()>
    Public Function AdaptarMySQL_SinSegundos(o As Date) As String
        Return o.ToString("yyyy-MM-dd HH:mm") & ":00"
    End Function



#Region "JSON"


    <Extension()>
    Public Function AdaptarJSON(aString As String) As String
        Return If(aString = "", "", System.Web.HttpUtility.JavaScriptStringEncode(aString))
    End Function


    <Extension()>
    Public Function AdaptarJSON(aString As Guid) As String
        Return If(aString = Guid.Empty, "", System.Web.HttpUtility.JavaScriptStringEncode(aString.ToString))
    End Function


    <Extension()>
    Public Function AdaptarJSON_PN(aString As String()) As String
        If aString Is Nothing Then Return "[]"
        Dim R As New List(Of String)
        For i = 0 To aString.Length - 1
            R.Add(System.Web.HttpUtility.JavaScriptStringEncode(aString(i), True))
        Next
        Return "[" & String.Join(",", R) & "]"
    End Function

    <Extension()>
    Public Function AdaptarJSON_PN(p As Decimal()) As String
        If p Is Nothing Then Return "[]"
        Return "[" & String.Join(",", p) & "]"
    End Function

    <Extension()>
    Public Function AdaptarJSON_PN(p As Integer()) As String
        If p Is Nothing Then Return "[]"
        Return "[" & String.Join(",", p) & "]"
    End Function


    <Extension()>
    Public Function AdaptarJSON_PN(aString As List(Of String)) As String
        If aString Is Nothing Then Return "[]"
        Dim R As New List(Of String)
        For i = 0 To aString.Count - 1
            R.Add(System.Web.HttpUtility.JavaScriptStringEncode(aString(i), True))
        Next
        Return "[" & String.Join(",", R) & "]"
    End Function


    <Extension()>
    Public Function AdaptarJSON_PN(aString As List(Of Boolean)) As String
        If aString Is Nothing Then Return "[]"
        Dim R As New List(Of String)
        For i = 0 To aString.Count - 1
            R.Add(aString(i).AdaptarJSON)
        Next
        Return "[" & String.Join(",", R) & "]"
    End Function
    <Extension()>
    Public Function AdaptarJSON_PN(aString As List(Of JSONBuildC)) As String
        If aString Is Nothing Then Return "[]"
        Dim R As New List(Of String)
        For i = 0 To aString.Count - 1
            R.Add(aString(i).ToJson)
        Next
        Return "[" & String.Join(",", R) & "]"
    End Function




    <Extension()>
    Public Function AdaptarJSON_PN(aString As List(Of Integer)) As String
        If aString Is Nothing Then Return "[]"
        Dim R As New List(Of String)
        For i = 0 To aString.Count - 1
            R.Add(aString(i).AdaptarJSON)
        Next
        Return "[" & String.Join(",", R) & "]"
    End Function



    <Extension()>
    Public Function AdaptarJSON_PN(aString As List(Of Decimal)) As String
        If aString Is Nothing Then Return "[]"
        Dim R As New List(Of String)
        For i = 0 To aString.Count - 1
            R.Add(aString(i).AdaptarJSON)
        Next
        Return "[" & String.Join(",", R) & "]"
    End Function





    <Extension()>
    Public Function AdaptarJSON(aString As Boolean) As String
        Return If(aString, "true", "false")
    End Function




    <Extension()>
    Public Function AdaptarJSON(aString As Integer) As String
        Return aString.ToString
    End Function

    <Extension()>
    Public Function AdaptarJSON(aString As Decimal) As String
        Return aString.ToString.Replace(",", ".")
    End Function



    <Extension()>
    Public Function AdaptarJSON_Comillear(aString As String) As String
        If aString Is Nothing Then aString = ""
        Return System.Web.HttpUtility.JavaScriptStringEncode(aString, True)
    End Function


#End Region







    <Extension()>
    Public Function Vacia(o As Date) As Boolean
        Return o = Date.MinValue
    End Function



    <Extension()>
    Public Function Rellenada(o As Date) As Boolean
        Return o <> Date.MinValue
    End Function

    <Extension()>
    Public Function AdaptarMySQL_ConSegundos_test(o As Date) As String
        Return o.ToString("yyyy-MM-dd HH:mm:ss")
    End Function


    <Extension()>
    Public Function AdaptarMySQL_Fecha(o As Date) As String
        Return o.ToString("yyyy-MM-dd")
    End Function

    <Extension()>
    Public Function AdaptarMySQL_HoraConSegundos(o As Date) As String
        Return o.ToString("HH:mm:ss")
    End Function

    <Extension()>
    Public Function ToTimeMYSQL(o As Date) As String
        Return o.ToString("HH:mm:ss")
    End Function

    <Extension()>
    Public Function AdaptarMySQL_HoraSinSegundos(o As Date) As String
        Return o.ToString("HH:mm") & ":00"
    End Function


    Public Function GarantizarNombreDeArchivoNoRepetido(rutaArchivo As String) As String
        If File.Exists(rutaArchivo) = False Then
            Return rutaArchivo
        End If
        Dim Ruta$ = System.IO.Path.GetDirectoryName(rutaArchivo)
        Dim Archivo$ = System.IO.Path.GetFileNameWithoutExtension(rutaArchivo)
        Dim Extension$ = System.IO.Path.GetExtension(rutaArchivo)
        For i = 2 To 1000000
            Dim RutaActual = System.IO.Path.Combine(Ruta, Archivo & " (" & i.ToString & ")" & Extension)
            If File.Exists(RutaActual) = False Then
                Return RutaActual
            End If
        Next
        Return rutaArchivo
    End Function




    <Extension()>
    Public Function Gzip_Compress(data As Byte()) As Byte()
        Using compressedStream = New MemoryStream()
            Using zipStream = New GZipStream(compressedStream, CompressionMode.Compress)
                zipStream.Write(data, 0, data.Length)
                zipStream.Close()
                Return compressedStream.ToArray()
            End Using
        End Using
    End Function


    <Extension()>
    Public Function Gzip_Decompress(data As Byte()) As Byte()

        Using compressedStream = New MemoryStream(data)
            Using zipStream = New GZipStream(compressedStream, CompressionMode.Decompress)
                Using resultStream = New MemoryStream()

                    zipStream.CopyTo(resultStream)
                    Return resultStream.ToArray()

                End Using
            End Using
        End Using

    End Function



    <Extension()>
    Public Function MidAvanzado(Texto As String, Comiezo%, CaracteresQueSeVanAMostrar%, Optional TextoFinalCadenaCortada$ = "...") As String
        If Texto = "" Then Return ""
        If Comiezo = 1 AndAlso CaracteresQueSeVanAMostrar >= Texto.Length Then
            Return Texto
        End If
        Dim TextoParaDevolver$ = Mid(Texto, Comiezo, CaracteresQueSeVanAMostrar)
        If TextoParaDevolver.Length <> (Texto.Length - (Comiezo - 1)) Then TextoParaDevolver &= TextoFinalCadenaCortada
        Return TextoParaDevolver
    End Function








#Region "DAte"





    Public Function Datetime_TryParse(FechaX As String, ByRef datex As Date) As Boolean
        Dim XX = FechaX.ToDateDesdeMySQLNulable
        If XX Is Nothing Then
            Return False
        Else
            datex = XX.Value
            Return True
        End If
    End Function

    <Extension()>
    Public Function ToDateX(o As String) As DateTime
        Return CDate(o)
    End Function





    Public Function PonerCerosAFechas(FechaX As String) As String



        If FechaX = "" Then


            Return FechaX
        End If


        If FechaX(FechaX.Length - 1).Equals("z"c) OrElse FechaX(FechaX.Length - 1).Equals("Z"c) Then
            FechaX = FechaX.Replace("z", "").Replace("Z", "").Replace("t", " ").Replace("T", " ")
        End If


        If (FechaX.Length = 8 AndAlso FechaX(1).Equals("/"c) AndAlso FechaX(3).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) OrElse       '2/2/1234   8
               (FechaX.Length = 9 AndAlso FechaX(1).Equals("/"c) AndAlso FechaX(4).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) OrElse   '2/12/1234  9
               (FechaX.Length = 9 AndAlso FechaX(2).Equals("/"c) AndAlso FechaX(4).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) OrElse   '21/2/1234  9
               (FechaX.Length = 10 AndAlso FechaX(2).Equals("/"c) AndAlso FechaX(5).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) OrElse  '21/22/1234  10
               (FechaX.Length = 6 AndAlso FechaX(1).Equals("/"c) AndAlso FechaX(3).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) OrElse   '2/2/12   6   
               (FechaX.Length = 7 AndAlso FechaX(1).Equals("/"c) AndAlso FechaX(4).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) OrElse   '2/12/12  7   
               (FechaX.Length = 7 AndAlso FechaX(2).Equals("/"c) AndAlso FechaX(4).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) OrElse   '21/2/12  7    
               (FechaX.Length = 8 AndAlso FechaX(2).Equals("/"c) AndAlso FechaX(5).Equals("/"c) AndAlso FechaX.CountLetras("/"c) = 2 AndAlso FechaX.Replace("/", "").EsNumerico) Then     '21/22/12  8

            Dim Dia = FechaX.RecibirPrimerTrozo("/"c).PadLeft(2, "0"c)
            Dim Mes = FechaX.RecibirPrimerTrozo("/", 1, 1).PadLeft(2, "0"c)
            Dim Ano = FechaX.RecibirUltimoTrozo("/"c).PadLeft(3, "0"c)
            If Ano.Length < 4 Then Ano = "2" & Ano
            Return Ano & "-" & Mes & "-" & Dia

        ElseIf (FechaX.Length = 8 AndAlso FechaX(1).Equals("-"c) AndAlso FechaX(3).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) OrElse         '2-2-1234   8
               (FechaX.Length = 9 AndAlso FechaX(1).Equals("-"c) AndAlso FechaX(4).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) OrElse     '2-12-1234  9
               (FechaX.Length = 9 AndAlso FechaX(2).Equals("-"c) AndAlso FechaX(4).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) OrElse     '21-2-1234  9
               (FechaX.Length = 10 AndAlso FechaX(2).Equals("-"c) AndAlso FechaX(5).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) OrElse    '21-22-1234  10
               (FechaX.Length = 6 AndAlso FechaX(1).Equals("-"c) AndAlso FechaX(3).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) OrElse     '2-2-12   6   
               (FechaX.Length = 7 AndAlso FechaX(1).Equals("-"c) AndAlso FechaX(4).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) OrElse     '2-12-12  7   
               (FechaX.Length = 7 AndAlso FechaX(2).Equals("-"c) AndAlso FechaX(4).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) OrElse     '21-2-12  7    
               (FechaX.Length = 8 AndAlso FechaX(2).Equals("-"c) AndAlso FechaX(5).Equals("-"c) AndAlso FechaX.CountLetras("-"c) = 2 AndAlso FechaX.Replace("-", "").EsNumerico) Then       '21-22-12  8


            Dim Dia = FechaX.RecibirPrimerTrozo("-"c).PadLeft(2, "0"c)
            Dim Mes = FechaX.RecibirPrimerTrozo("-", 1, 1).PadLeft(2, "0"c)
            Dim Ano = FechaX.RecibirUltimoTrozo("-"c).PadLeft(3, "0"c)
            If Ano.Length < 4 Then Ano = "2" & Ano
            Return Ano & "-" & Mes & "-" & Dia


        ElseIf FechaX.Length > 8 Then

            If FechaX.Length < 7 Then
                Return FechaX
            End If

            If FechaX(6) = "-" Then
                FechaX = FechaX.Substring(0, 5) & "0" & FechaX.Substring(5, FechaX.Length - 5)
            End If

            If FechaX.Length = 9 Then

                FechaX = FechaX.Substring(0, 8) & "0" & FechaX.Substring(FechaX.Length - 1, 1)
                Return FechaX

            Else

                If FechaX.Length < 10 Then
                    Return FechaX
                End If
                If FechaX(9) = " " Then
                    FechaX = FechaX.Substring(0, 8) & "0" & FechaX.Substring(8, FechaX.Length - 8)
                End If

            End If

            If FechaX.Length < 13 Then
                Return FechaX
            End If

            If FechaX(12) = ":" Then
                FechaX = FechaX.Substring(0, 11) & "0" & FechaX.Substring(11, FechaX.Length - 11)
            End If


            If FechaX.Length < 15 Then
                Return FechaX
            ElseIf FechaX.Length = 15 Then
                FechaX = FechaX.Substring(0, 14) & "0" & FechaX.Substring(14, FechaX.Length - 14)
                Return FechaX
            Else
                If FechaX(15) = ":" Then
                    FechaX = FechaX.Substring(0, 14) & "0" & FechaX.Substring(14, FechaX.Length - 14)
                End If
            End If

            If FechaX.Length < 18 Then
                Return FechaX
            ElseIf FechaX.Length = 18 Then
                FechaX = FechaX.Substring(0, 17) & "0" & FechaX.Substring(FechaX.Length - 1, 1)
                Return FechaX
            End If

        End If

        Return FechaX
    End Function


    <Extension()>
    Public Function ToDateDesdeMySQLNulable(elstr As String, Optional kindx As DateTimeKind = DateTimeKind.Unspecified) As Date?
        If elstr Is Nothing Then Return Nothing
        elstr = PonerCerosAFechas(elstr)
        If elstr.Length = 19 Then

            Dim Ano%, Mes%, Dia%, Hora%, Minuto%, Segundo%

            If Integer.TryParse(elstr.Substring(0, 4), Ano) Then
                If Integer.TryParse(elstr.Substring(5, 2), Mes) Then
                    If Integer.TryParse(elstr.Substring(8, 2), Dia) Then
                        If Integer.TryParse(elstr.Substring(11, 2), Hora) Then
                            If Integer.TryParse(elstr.Substring(14, 2), Minuto) Then
                                If Integer.TryParse(elstr.Substring(17, 2), Segundo) Then
                                    If Ano > 1 AndAlso Ano < 8000 AndAlso Mes >= 1 AndAlso Mes <= 12 AndAlso Dia >= 1 AndAlso Dia <= DateTime.DaysInMonth(Ano, Mes) Then
                                        Return New Date(Ano, Mes, Dia, Hora, Minuto, Segundo, kindx)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If





        ElseIf elstr.Length = 10 AndAlso elstr(4).Equals("-"c) AndAlso elstr(7).Equals("-"c) Then  ' 2001-02-03


            Dim Ano%, Mes%, Dia%

            If Integer.TryParse(elstr.Substring(0, 4), Ano) Then
                If Integer.TryParse(elstr.Substring(5, 2), Mes) Then
                    If Integer.TryParse(elstr.Substring(8, 2), Dia) Then
                        If Ano > 1 AndAlso Ano < 8000 AndAlso Mes >= 1 AndAlso Mes <= 12 AndAlso Dia >= 1 AndAlso Dia <= DateTime.DaysInMonth(Ano, Mes) Then
                            Return New Date(Ano, Mes, Dia, 0, 0, 0, kindx)
                        End If
                    End If
                End If
            End If

        ElseIf elstr.Length = 10 Then

            Dim Ano%, Mes%, Dia%

            If Integer.TryParse(elstr.Substring(0, 4), Ano) Then
                If Integer.TryParse(elstr.Substring(5, 2), Mes) Then
                    If Integer.TryParse(elstr.Substring(8, 2), Dia) Then
                        If Ano > 1 AndAlso Ano < 8000 AndAlso Mes >= 1 AndAlso Mes <= 12 AndAlso Dia >= 1 AndAlso Dia <= DateTime.DaysInMonth(Ano, Mes) Then
                            Return New Date(Ano, Mes, Dia, 0, 0, 0, kindx)
                        End If
                    End If
                End If
            End If

        ElseIf elstr.Length = 16 Then


            Dim Ano%, Mes%, Dia%, Hora%, Minuto%

            If Integer.TryParse(elstr.Substring(0, 4), Ano) Then
                If Integer.TryParse(elstr.Substring(5, 2), Mes) Then
                    If Integer.TryParse(elstr.Substring(8, 2), Dia) Then
                        If Integer.TryParse(elstr.Substring(11, 2), Hora) Then
                            If Integer.TryParse(elstr.Substring(14, 2), Minuto) Then
                                If Ano > 1 AndAlso Ano < 8000 AndAlso Mes >= 1 AndAlso Mes <= 12 AndAlso Dia >= 1 AndAlso Dia <= DateTime.DaysInMonth(Ano, Mes) Then
                                    Return New Date(Ano, Mes, Dia, Hora, Minuto, 0, kindx)
                                End If
                            End If
                        End If
                    End If
                End If
            End If


        ElseIf elstr.Length < 8 Then '12:12:12

            If elstr.IndexOf(":"c) <> -1 Then
                Try
                    Return New Date(CDate(elstr).Ticks, kindx)
                Catch
                End Try
            End If

        End If






        Return Nothing
    End Function

    <Extension()>
    Public Function ToDateDesdeMySQL(elstr As String, kind As DateTimeKind) As Date
        If elstr = "" Then Return New Date(DateTime.MinValue.Ticks, kind)
        If elstr Is Nothing Then Return New Date(DateTime.MinValue.Ticks, kind)
        elstr = PonerCerosAFechas(elstr)
        If elstr.Length = 19 Then
            Return New Date(Integer.Parse(elstr.Substring(0, 4)), Integer.Parse(elstr.Substring(5, 2)), Integer.Parse(elstr.Substring(8, 2)), Integer.Parse(elstr.Substring(11, 2)), Integer.Parse(elstr.Substring(14, 2)), Integer.Parse(elstr.Substring(17, 2)), kind)
        ElseIf elstr.Length = 10 Then
            Return New Date(Integer.Parse(elstr.Substring(0, 4)), Integer.Parse(elstr.Substring(5, 2)), Integer.Parse(elstr.Substring(8, 2)), 0, 0, 0, kind)
        Else
            Dim x = CDate(elstr)
            Return New Date(x.Ticks, kind)
        End If
    End Function



    Public mToDateDesdeMySQLCache As Dic(Of String, Date)


    <Extension()>
    Public Function ToDateDesdeMySQL(elstr As String) As Date
        If elstr = "" Then Return DateTime.MinValue
        If elstr Is Nothing Then Return DateTime.MinValue







        Dim R As Date

        If mToDateDesdeMySQLCache IsNot Nothing AndAlso mToDateDesdeMySQLCache.TryGetValue(elstr, R) Then
            Return R
        End If

        elstr = PonerCerosAFechas(elstr)
        If elstr.Length = 19 Then
            R = New Date(Integer.Parse(elstr.Substring(0, 4)), Integer.Parse(elstr.Substring(5, 2)), Integer.Parse(elstr.Substring(8, 2)), Integer.Parse(elstr.Substring(11, 2)), Integer.Parse(elstr.Substring(14, 2)), Integer.Parse(elstr.Substring(17, 2)))
        ElseIf elstr.Length = 10 Then
            R = New Date(Integer.Parse(elstr.Substring(0, 4)), Integer.Parse(elstr.Substring(5, 2)), Integer.Parse(elstr.Substring(8, 2)), 0, 0, 0)
        Else
            R = CDate(elstr)
        End If


        Return R
    End Function


    <Extension()>
    Public Function ToDateDesdeMySQL_utc(elstr As String) As Date
        Return elstr.ToDateDesdeMySQL(DateTimeKind.Utc)
    End Function


    <Extension()>
    Public Function ToDateDesdeMySQL_Local(elstr As String) As Date
        Return elstr.ToDateDesdeMySQL(DateTimeKind.Local)
    End Function


    <Extension()>
    Public Function ToDateDesdeMySQLNulable_Local(elstr As String) As Date?
        Return elstr.ToDateDesdeMySQLNulable(DateTimeKind.Local)
    End Function


    <Extension()>
    Public Function ToDateDesdeMySQLNulable_UTC(elstr As String) As Date?
        Return elstr.ToDateDesdeMySQLNulable(DateTimeKind.Utc)
    End Function

    <Extension()>
    Public Function ToLocalTime_PN(elstr As Date?) As Date?
        If elstr Is Nothing Then Return Nothing
        Return elstr.Value.ToLocalTime
    End Function

    ''' <summary>
    ''' Intenta convertir un string a un tipo datetime, si resulta imposible retornará valor nulo
    ''' </summary>
    <Extension()>
    Public Function ToDateNulable(o As String, kind As DateTimeKind) As DateTime?
        If o = "" OrElse o.Length < 4 Then
            Return Nothing
        ElseIf o.Length = 10 Then
            '  "xx-xx-xxxx" =  10 Caráceteres
            '@ Faltan la hora
            o &= " 00:00:00"
        ElseIf o.Length = 16 Then
            '@ Faltan los  segundos
            '"xx-xx-xxxx xx:xx". = 16 Caracteres
            o &= ":00"
        End If

        Dim T As Date
        If Datetime_TryParse(o, T) Then
            If T.Kind <> kind Then
                Return New Date(T.Ticks, kind)
            Else
                Return T
            End If
        Else
            Return Nothing
        End If
    End Function


    ''' <summary>
    ''' Intenta convertir un string a un tipo datetime, si resulta imposible retornará valor nulo
    ''' </summary>
    <Extension()>
    Public Function ToDateNulable(o As String) As DateTime?

        If o = "" OrElse o.Length < 4 Then
            Return Nothing
        ElseIf o.Length = 10 Then
            '  "xx-xx-xxxx" =  10 Caráceteres
            '@ Faltan la hora
            o = o & " 00:00:00"
        ElseIf o.Length = 16 Then
            '@ Faltan los  segundos
            '"xx-xx-xxxx xx:xx". = 16 Caracteres
            o = o & ":00"
        End If

        Dim T As Date
        If Datetime_TryParse(o, T) Then
            Return T
        Else
            Return Nothing
        End If
    End Function

    <Extension()>
    Public Function ToDateNulable_UTC_a_Local(o As String) As Date?
        If o = "" OrElse o.Length < 4 Then
            Return Nothing
        ElseIf o.Length = 10 Then
            '  "xx-xx-xxxx" =  10 Caráceteres
            '@ Faltan la hora
            o = o & " 00:00:00"
        ElseIf o.Length = 16 Then
            '@ Faltan los  segundos
            '"xx-xx-xxxx xx:xx". = 16 Caracteres
            o = o & ":00"
        End If

        Dim T As Date
        If Datetime_TryParse(o, T) Then
            Return (New Date(T.Kind, DateTimeKind.Utc)).ToLocalTime
        Else
            Return Nothing
        End If
    End Function

#End Region




#Region "LikeM"
    <Extension()>
    Public Function LikeMIgnoreCase(o As String, ParamArray Opciones() As String) As Boolean
        If Opciones Is Nothing OrElse Opciones.Length = 0 Then
            Return False
        End If
        For i As Integer = 0 To Opciones.Length - 1
            If Opciones(i).Length = o.Length AndAlso Opciones(i).Equals(o, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function


    <Extension()>
    Public Function LikeM(o As Integer, Opcion1 As Integer) As Boolean
        Return o = Opcion1
    End Function




    <Extension()>
    Public Function LikeM(o As String, ByRef Opcion1 As String, ByRef Opcion2 As String) As Boolean
        Return o = Opcion1 OrElse o = Opcion2
    End Function

    <Extension()>
    Public Function LikeM(o As Integer, ByRef Opcion1 As Integer, ByRef Opcion2 As Integer) As Boolean
        Return o = Opcion1 OrElse o = Opcion2
    End Function

    <Extension()>
    Public Function LikeM(o As Integer, ByRef Opcion1 As Integer, ByRef Opcion2 As Integer, ByRef Opcion3 As Integer) As Boolean
        Return o = Opcion1 OrElse o = Opcion2 OrElse o = Opcion3
    End Function

    <Extension()>
    Public Function LikeM(o As Integer, Opcion1 As Integer, Opcion2 As Integer, Opcion3 As Integer, Opcion4 As Integer) As Boolean
        Return o = Opcion1 OrElse o = Opcion2 OrElse o = Opcion3 OrElse o = Opcion4
    End Function

    <Extension()>
    Public Function LikeM(o As Integer, Opcion1 As Integer, Opcion2 As Integer, Opcion3 As Integer, Opcion4 As Integer, Opcion5 As Integer) As Boolean
        Return o = Opcion1 OrElse o = Opcion2 OrElse o = Opcion3 OrElse o = Opcion4 OrElse o = Opcion5
    End Function


    <Extension()>
    Public Function LikeM(o As Decimal, Opcion1 As Decimal) As Boolean
        Return o = Opcion1
    End Function

    <Extension()>
    Public Function LikeM(o As Decimal, Opcion1 As Decimal, Opcion2 As Decimal) As Boolean
        Return o = Opcion1 OrElse o = Opcion2
    End Function

    <Extension()>
    Public Function LikeM(o As Decimal, Opcion1 As Decimal, Opcion2 As Decimal, Opcion3 As Decimal) As Boolean
        Return o = Opcion1 OrElse o = Opcion2 OrElse o = Opcion3
    End Function

    <Extension()>
    Public Function LikeM(o As Decimal, Opcion1 As Decimal, Opcion2 As Decimal, Opcion3 As Decimal, Opcion4 As Decimal) As Boolean
        Return o = Opcion1 OrElse o = Opcion2 OrElse o = Opcion3 OrElse o = Opcion4
    End Function

    <Extension()>
    Public Function LikeM(o As Decimal, Opcion1 As Decimal, Opcion2 As Decimal, Opcion3 As Decimal, Opcion4 As Decimal, Opcion5 As Decimal) As Boolean
        Return o = Opcion1 OrElse o = Opcion2 OrElse o = Opcion3 OrElse o = Opcion4 OrElse o = Opcion5
    End Function

    <Extension()>
    Public Function LikeM(o As Decimal, Opciones() As Decimal) As Boolean
        If Opciones Is Nothing OrElse Opciones.Length = 0 Then Return False
        For i As Integer = 0 To Opciones.Length - 1
            If Opciones(i) = o Then Return True
        Next
        Return False
    End Function

    <Extension()>
    Public Function LikeM(o As Char, Char1 As Char, Char2 As Char) As Boolean
        Return o.Equals(Char1) OrElse o.Equals(Char2)
    End Function

    <Extension()>
    Public Function LikeM(o As Char, Char1 As Char, Char2 As Char, Char3 As Char) As Boolean
        Return o.Equals(Char1) OrElse o.Equals(Char2) OrElse o.Equals(Char3)
    End Function

    <Extension()>
    Public Function LikeM(o As Char, Char1 As Char, Char2 As Char, Char3 As Char, Char4 As Char) As Boolean
        Return o.Equals(Char1) OrElse o.Equals(Char2) OrElse o.Equals(Char3) OrElse o.Equals(Char4)
    End Function

    <Extension()>
    Public Function LikeM(o As Char, ParamArray Opciones() As Char) As Boolean
        If Opciones Is Nothing OrElse Opciones.Length = 0 Then Return False
        For i As Integer = 0 To Opciones.Length - 1
            If Opciones(i) = o Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

    Public Interface ToJSoneableI


        Function ToJSON() As JSONBuildC



    End Interface



    Public Class JSONBuildC


        Public Function TieneDatos() As Boolean
            Return Lista.Count > 0
        End Function

        Public Function ToJson() As String
            Return String.Join("", Lista)
        End Function


        Public Sub Clear()
            Lista.Clear()
            Proximocomillaizquierda = False
        End Sub


        Private Lista As List(Of String)
        Private Proximocomillaizquierda As Boolean = False



        Private Sub mAdd_Key_Value(keysegura$, valorseguro$)
            Lista.Add(If(Proximocomillaizquierda, ",", "") & keysegura & ": " & valorseguro)
            Proximocomillaizquierda = True
        End Sub




        Public Sub Add_Key_ValueJSON(KeySinComillasNiEscapa$, ValorJSON$)
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValorJSON)
        End Sub





        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa$)
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), System.Web.HttpUtility.JavaScriptStringEncode(ValueSinComillasNiEscapa, True))
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As Guid)
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), System.Web.HttpUtility.JavaScriptStringEncode(ValueSinComillasNiEscapa.STR, True))
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As Integer)
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON)
        End Sub

        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As Decimal)
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON)
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As Boolean)
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON)
        End Sub

        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As List(Of String))
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As String())
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub

        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As Integer())
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub

        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As Decimal())
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As List(Of Integer))
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub

        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As List(Of Decimal))
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As List(Of Boolean))
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As JSONBuildC)
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), If(ValueSinComillasNiEscapa Is Nothing, "null", ValueSinComillasNiEscapa.ToJson))
        End Sub

        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As List(Of JSONBuildC))
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.AdaptarJSON_PN)
        End Sub



        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As List(Of ToJSoneableI))
            Dim LSTX As New List(Of JSONBuildC)
            If ValueSinComillasNiEscapa IsNot Nothing Then
                For Each Actual In ValueSinComillasNiEscapa
                    LSTX.Add(Actual.ToJSON)
                Next
            End If
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), LSTX.AdaptarJSON_PN)
        End Sub


        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As IEnumerable(Of ToJSoneableI))
            Dim LSTX As New List(Of JSONBuildC)
            If ValueSinComillasNiEscapa IsNot Nothing Then
                For Each Actual In ValueSinComillasNiEscapa
                    LSTX.Add(Actual.ToJSON)
                Next
            End If
            mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), LSTX.AdaptarJSON_PN)
        End Sub





        Public Sub Add(KeySinComillasNiEscapa$, ValueSinComillasNiEscapa As ToJSoneableI)
            If ValueSinComillasNiEscapa IsNot Nothing Then
                mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), ValueSinComillasNiEscapa.ToJSON.ToJson)
            Else
                mAdd_Key_Value(System.Web.HttpUtility.JavaScriptStringEncode(KeySinComillasNiEscapa, True), "null")
            End If
        End Sub


        Public Sub Add(ValorDirecto$)
            Lista.Add(ValorDirecto)
            If ValorDirecto = "}" Then
                Proximocomillaizquierda = True
            ElseIf ValorDirecto = "{" Then
                Proximocomillaizquierda = False
            ElseIf ValorDirecto = "," Then
                Proximocomillaizquierda = False
            Else
                RecalcularProximoComillaIZquierda()
            End If
        End Sub


        Private Sub RecalcularProximoComillaIZquierda()
            If Lista.Count = 0 Then
                Proximocomillaizquierda = False
            Else
                Dim UltimoValor = Lista(Lista.Count - 1).Trim
                Dim UltimoCaracter = If(UltimoValor.Length > 0, UltimoValor(UltimoValor.Length - 1).ToString, "")
                If UltimoValor = "" Then
                    Lista.RemoveAt(Lista.Count - 1)
                    RecalcularProximoComillaIZquierda()
                ElseIf UltimoCaracter.LikeM("{", ",", "[") Then
                    Proximocomillaizquierda = False
                ElseIf UltimoCaracter.LikeM("]", "}") Then
                    Proximocomillaizquierda = True
                ElseIf UltimoValor.StartsWith("""") AndAlso UltimoValor.Contains(":") Then 'KeyValue 
                    Proximocomillaizquierda = True
                Else
                    'Master_Interruptor()
                End If

            End If
        End Sub


        Sub New()
            Lista = New List(Of String)(20)
        End Sub
        Sub New(capacidadBase%)
            Lista = New List(Of String)(capacidadBase)
        End Sub
    End Class


End Module
