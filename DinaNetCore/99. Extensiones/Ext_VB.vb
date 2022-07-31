Imports System.IO
Imports System.Runtime.CompilerServices

Partial Public Module ExtensionesM








    Public Function AplicarTamanoARuta(RutaDeArchivoOriginal$, Tamano As String) As String
        If Tamano = "" Then Return RutaDeArchivoOriginal
        '! Son los únicos tamaños compatibles, para lo demás se entraga el archivo original
        '! la extensión .dat es la extensión con la que dinaup guarda TODOS los archivos.
        If RutaDeArchivoOriginal.EndsWith(".dat") AndAlso (Tamano = "2" OrElse Tamano = "3" OrElse Tamano = "4") Then

            Dim Extension$ = ""

            Select Case Tamano
                Case "2" ' Alto100px
                    Extension = "_01"
                Case "3" ' Alto300px
                    Extension = "_01"
                Case "4" ' Alto32px
                    Extension = "_03"
            End Select

            If Extension <> "" Then
                Return RutaDeArchivoOriginal.EliminarFinal(4) & Extension + ".dat"
            End If

        End If



        Return RutaDeArchivoOriginal
    End Function




    Public Function CalcularHMAC(Pass$, Datos As String) As String
        Dim key As Byte() = System.Text.Encoding.ASCII.GetBytes(Pass)
        Dim myhmacsha256 As System.Security.Cryptography.HMACSHA256 = New System.Security.Cryptography.HMACSHA256(key)
        Dim byteArray = System.Text.Encoding.ASCII.GetBytes(Datos)
        Dim result As String = BitConverter.ToString(myhmacsha256.ComputeHash(byteArray)).Replace("-", "").ToLower
        myhmacsha256.Dispose()
        Return result
    End Function

    Public Function CalcularHMAC(Pass As Byte(), Datos As String) As String
        Dim byteArray = System.Text.Encoding.ASCII.GetBytes(Datos)
        Dim myhmacsha256 As System.Security.Cryptography.HMACSHA256 = New System.Security.Cryptography.HMACSHA256(Pass)
        Dim result As String = BitConverter.ToString(myhmacsha256.ComputeHash(byteArray)).Replace("-", "").ToLower
        myhmacsha256.Dispose ()
        Return result
    End Function



    <Extension()>
    Public Function SerializarJSON(x As Object) As String
        Return Newtonsoft.Json.JsonConvert.SerializeObject(x, Newtonsoft.Json.Formatting.Indented)
    End Function




    <Extension()>
    Public Function EstaEjecutandose(th As System.Threading.Thread) As Boolean
        Return th IsNot Nothing AndAlso th.IsAlive
    End Function




    <Extension()>
    Public Sub AbortEnTimeOut(th As System.Threading.Thread, ms%)
        On Error Resume Next
        If th Is Nothing Then Exit Sub
        If th.IsAlive = False Then Exit Sub


        Dim ValorStep = If(ms < 20, 1, 10)
        For i As Integer = 1 To ms Step ValorStep
            If th.IsAlive = False Then Exit Sub
            System.Threading.Thread.Sleep(ValorStep)
        Next

        If th.IsAlive Then
            th.Abort()
        End If
    End Sub





#Region "ThRead"

#End Region


    <Extension()>
    Public Function EsElThreadActual(th As System.Threading.Thread) As Boolean
        Return th IsNot Nothing AndAlso th Is System.Threading.Thread.CurrentThread
    End Function





#Region "STRJoin"


    <Extension()>
    Public Function STRJoin(o As String(), Optional Separador As String = "") As String
        If o Is Nothing Then
            Return ""
        ElseIf o.Length = 1 Then
            Return o(0)
        ElseIf o.Length = 2 Then
            If Separador <> "" Then
                Return o(0) & Separador & o(1)
            Else
                Return o(0) & o(1)
            End If
        Else
            Return String.Join(Separador, o)
        End If
    End Function



    <Extension()>
    Public Function STRJoin(o As List(Of String), Optional Separador As String = "") As String
        If o Is Nothing Then
            Return ""
        ElseIf o.Count = 1 Then
            Return o(0)
        ElseIf o.Count = 2 Then
            If Separador <> "" Then
                Return o(0) & Separador & o(1)
            Else
                Return o(0) & o(1)
            End If
        Else
            Return String.Join(Separador, o)
        End If
    End Function

    <Extension()>
    Public Function STRJoin(o As List(Of Guid), Optional Separador As String = "") As String
        If o Is Nothing Then
            Return ""
        ElseIf o.Count = 1 Then
            Return o(0).STR
        ElseIf o.Count = 2 Then
            If Separador <> "" Then
                Return o(0).STR & Separador & o(1).STR
            Else
                Return o(0).STR & o(1).STR
            End If
        Else
            Return String.Join(Separador, o)
        End If
    End Function

#End Region



    <Extension()>
    Public Function LikeM(o As Integer, ParamArray Opciones() As Integer) As Boolean
        If Opciones Is Nothing OrElse Opciones.Length = 0 Then Return False
        For i As Integer = 0 To Opciones.Length - 1
            If Opciones(i) = o Then Return True
        Next
        Return False
    End Function

    <Extension()>
    Public Function LikeM(o As String, ParamArray Opciones() As String) As Boolean
        If Opciones Is Nothing OrElse Opciones.Length = 0 Then Return False
        For i As Integer = 0 To Opciones.Length - 1
            If Opciones(i) = o Then Return True
        Next
        Return False
    End Function


    Public Function NoEsNulo(x As Object) As Boolean
        Return x IsNot Nothing
    End Function



    Public Function Nulo(x As Object) As Boolean
        Return x Is Nothing
    End Function



    Public Function EsNulo(x As Object) As Boolean
        Return x Is Nothing
    End Function




    Public Class CapsulaC(Of t)

        Public Valor As t

        Sub New(_Valor As t)
            Valor = _Valor
        End Sub

    End Class







    Public Function Max(a As Date, a1 As Date) As Date
        Return If(a > a1, a, a1)
    End Function



    Public Function Min(a As Date, a1 As Date) As Date
        Return If(a < a1, a, a1)
    End Function




    Public Function Max(a As Integer, a1 As Integer) As Integer
        Return If(a > a1, a, a1)
    End Function




    Public Function Max(a As Decimal, a1 As Decimal) As Decimal
        If a > a1 Then Return a
        Return a1
    End Function

    Public Function Max(a As Long, a1 As Long) As Long
        If a > a1 Then Return a
        Return a1
    End Function



    Public Function Min(a As Integer, a1 As Integer) As Integer
        Return If(a < a1, a, a1)
    End Function



    <Extension()> <DebuggerNonUserCode>
    Public Function EsGUID(o As String) As Boolean
        Return Guid.TryParse(o, Nothing)
    End Function


    <Extension()>
    Public Function Mitad(o As Integer) As Integer
        Return CInt(o / 2)
    End Function
    <Extension()>
    Public Function EsNumerico(o As String) As Boolean
        If o = "" Then Return False
        If Char.IsNumber(o(o.Length - 1)) = False Then Return False
        Return Decimal.TryParse(o.Replace(",", "."), Nothing)
    End Function

    <Extension()>
    Public Function EsNumericoEntero(o As String, Optional VacioEs0 As Boolean = False) As Boolean
        If o = "" Then
            Return VacioEs0
        Else
            If Char.IsNumber(o(o.Length - 1)) = False Then Return False
            Return Integer.TryParse(o, Nothing)
        End If
    End Function




    <Extension()>
    Public Function EsMultiploDe(o As Integer, De%) As Boolean
        Return o <> 0 AndAlso CType(o / De, Decimal) = CType(CInt(o / De), Decimal)
    End Function


    <Extension()>
    Public Function Redondear(o As Double, MaximosDecimales As Integer) As Decimal
        Return Redondear(CDec(o), MaximosDecimales)
    End Function


    <Extension()>
    Public Function Redondear(o As Decimal, MaximosDecimales As Integer) As Decimal

        If o = 0D Then

            Return 0D

        Else

            Dim Temp As Decimal = Math.Round(o, MaximosDecimales, MidpointRounding.AwayFromZero)

            If Temp.ToString.EndsWith("0") AndAlso CDec(CType(Temp, Long)) <> Temp Then

                Dim Trimeado = Temp.ToString.Trim("0"c)

                If Trimeado(Trimeado.Length - 1) = "." Then
                    Return Trimeado.Trim("."c).DEC
                ElseIf Trimeado(Trimeado.Length - 1) = "," Then
                    Return Trimeado.Trim(","c).DEC
                Else
                    Return Trimeado.DEC
                End If

            Else

                Return Temp

            End If

        End If

    End Function

End Module
