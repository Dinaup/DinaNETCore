Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic

Partial Public Module ExtensionesM




    <Extension()>
    Public Function EsEmail(ElString As String) As Boolean
        If ElString = "" Then Return False
        If ElString.Contains(" ") Then Return False
        If ElString.Contains("@") = False Then Return False


        Dim Sep = ElString.Split("@"c)
        If Sep Is Nothing Then Return False
        If Sep.Length <> 2 Then Return False


        If Sep(0) = "" Then Return False


        '! El dominio no puede estar vacio 
        If Sep(1) = "" Then Return False
        '! El dominio debe tener punto 
        If Sep(1).Contains(".") = False Then Return False


        Dim Caracteres = "äëïöüÄËÏÖÜáéíóúÁÉÍÓÚabcdefghijklmnopqrstuvwxyzñÑABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789&+-=?^_{|}.@"

        For i = 0 To ElString.Length - 1
            If Caracteres.Contains(ElString(i)) = False Then
                Return False
            End If
        Next


        Return True
    End Function
    <Extension()>
    Public Function LimpiarAcentos(ByVal aString As String, Optional ByVal PermitirnEñes As Boolean = True) As String
        If aString = "" Then Return ""

        Dim Retornar(aString.Length - 1) As Char

        For i = 0 To aString.Length - 1

            Select Case aString(i)
                Case "ñ"c : Retornar(i) = If(PermitirnEñes, "ñ"c, "n"c)
                Case "Ñ"c : Retornar(i) = If(PermitirnEñes, "Ñ"c, "N"c)
                Case "á"c : Retornar(i) = "a"c
                Case "é"c : Retornar(i) = "e"c
                Case "í"c : Retornar(i) = "i"c
                Case "ó"c : Retornar(i) = "o"c
                Case "ú"c : Retornar(i) = "u"c
                Case "ü"c : Retornar(i) = "u"c
                Case "ç"c : Retornar(i) = "ç"c
                Case "Á"c : Retornar(i) = "A"c
                Case "É"c : Retornar(i) = "E"c
                Case "Í"c : Retornar(i) = "I"c
                Case "Ó"c : Retornar(i) = "O"c
                Case "Ú"c : Retornar(i) = "U"c
                Case "Ü"c : Retornar(i) = "U"c
                Case "Ç"c : Retornar(i) = "Ç"c
                Case Else : Retornar(i) = aString(i)
            End Select

        Next

        Return New String(Retornar)
    End Function




    <Extension()>
    Public Function QueEmpiecePor(ByVal Texto As String, ByVal ComienzoCadena As String) As String
        If Texto = "" Then Return ComienzoCadena
        If Texto.StartsWith(ComienzoCadena) Then Return Texto
        Return ComienzoCadena & Texto
    End Function


    <Extension()>
    Public Function QueNoEmpieceEn(ByVal Texto As String, ByVal QueNoEmpiecePor As String) As String
        If Texto = "" Then Return ""
        If QueNoEmpiecePor = "" Then Return Texto

        If Texto.StartsWith(QueNoEmpiecePor) Then
            Return Mid(Texto, QueNoEmpiecePor.Length + 1).QueNoEmpieceEn(QueNoEmpiecePor)
        Else
            Return Texto
        End If
    End Function


    <Extension()>
    Public Function QueNoTermineEn_Ignorecase(Texto As String, FinCadena As String) As String
        If Texto = "" Then Return ""
        If FinCadena = "" Then Return Texto
        If Texto.EndsWithIgnoreCase(FinCadena) Then
            Return Texto.Remove(Texto.Length - FinCadena.Length, FinCadena.Length).QueNoTermineEn_Ignorecase(FinCadena)
        Else
            Return Texto
        End If
    End Function

    <Extension()>
    Public Function Desencomillear(ByVal str As String, Optional ByVal Comilla As String = "'") As String
        Return If(str.StartsWith(Comilla) AndAlso str.EndsWith(Comilla), Mid(str, 2, str.Length - 2), str)
    End Function



    <Extension()>
    Public Function Parsear$(ByRef Texto$, ByVal Inicio$, ByVal Fin$)
        If InStr(Texto, Inicio) = 0 Then
            Return ""
        End If

        Dim InicioI% = InStr(Texto, Inicio) + Inicio.Length

        Try
            Return (Trim(Mid(Texto, InicioI, InStr(InicioI, Texto, Fin) - InicioI)))
        Catch
            Return ""
        End Try
    End Function

    <Extension()>
    Public Function Trim_PN(Valor As String) As String
        Return If(Valor IsNot Nothing, Valor.Trim, "")
    End Function

    <Extension()>
    Public Function LimpiarCaracteresEspecialesSTR(ByVal aString As String, Optional ByVal PermitirEñes As Boolean = True, Optional ByVal SeparadorEspacios As String = " ", Optional DespuesDeEspacioMayuscula As Boolean = False) As String
        If aString = "" Then Return ""

        Dim Retornar(aString.Length - 1) As Char
        Dim UsadosIndices As Integer = 0


        Dim ProximaMayuscula As Boolean = False


        For I As Integer = 0 To aString.Length - 1

            If aString(I) = " "c Then

                If SeparadorEspacios <> "" Then
                    If UsadosIndices > 0 And Retornar(UsadosIndices) <> SeparadorEspacios Then
                        Retornar(UsadosIndices) = SeparadorEspacios(0)
                        UsadosIndices += 1
                    End If
                End If

                If DespuesDeEspacioMayuscula Then
                    ProximaMayuscula = True
                End If
            Else
                Select Case aString(I)
                    Case "Ñ"c : Retornar(UsadosIndices) = If(PermitirEñes, "Ñ"c, "N"c)
                        UsadosIndices += 1
                    Case "á"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "A"c, "a"c)
                        UsadosIndices += 1
                    Case "é"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "E"c, "e"c)
                        UsadosIndices += 1
                    Case "í"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "I"c, "i"c)
                        UsadosIndices += 1
                    Case "ó"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "O"c, "o"c)
                        UsadosIndices += 1
                    Case "ú"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "U"c, "u"c)
                        UsadosIndices += 1
                    Case "ü"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "U"c, "u"c)
                        UsadosIndices += 1
                    Case "ç"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "Ç"c, "ç"c)
                        UsadosIndices += 1
                    Case "Á"c : Retornar(UsadosIndices) = "A"c
                        UsadosIndices += 1
                    Case "É"c : Retornar(UsadosIndices) = "E"c
                        UsadosIndices += 1
                    Case "Í"c : Retornar(UsadosIndices) = "I"c
                        UsadosIndices += 1
                    Case "Ó"c : Retornar(UsadosIndices) = "O"c
                        UsadosIndices += 1
                    Case "Ú"c : Retornar(UsadosIndices) = "U"c
                        UsadosIndices += 1
                    Case "Ü"c : Retornar(UsadosIndices) = "U"c
                        UsadosIndices += 1
                    Case "Ç"c : Retornar(UsadosIndices) = "Ç"c
                        UsadosIndices += 1

                    Case "ñ"c : Retornar(UsadosIndices) = If(ProximaMayuscula, If(PermitirEñes, "Ñ"c, "N"c), If(PermitirEñes, "ñ"c, "n"c))
                        UsadosIndices += 1
                    Case "a"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "A"c, "a"c)
                        UsadosIndices += 1
                    Case "b"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "B"c, "b"c)
                        UsadosIndices += 1
                    Case "c"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "C"c, "c"c)
                        UsadosIndices += 1
                    Case "d"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "D"c, "d"c)
                        UsadosIndices += 1
                    Case "e"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "E"c, "e"c)
                        UsadosIndices += 1
                    Case "f"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "F"c, "f"c)
                        UsadosIndices += 1
                    Case "g"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "G"c, "g"c)
                        UsadosIndices += 1
                    Case "h"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "H"c, "h"c)
                        UsadosIndices += 1
                    Case "i"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "I"c, "i"c)
                        UsadosIndices += 1
                    Case "j"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "J"c, "j"c)
                        UsadosIndices += 1
                    Case "k"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "K"c, "k"c)
                        UsadosIndices += 1
                    Case "l"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "L"c, "l"c)
                        UsadosIndices += 1
                    Case "m"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "M"c, "m"c)
                        UsadosIndices += 1
                    Case "n"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "N"c, "n"c)
                        UsadosIndices += 1
                    Case "o"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "O"c, "o"c)
                        UsadosIndices += 1
                    Case "p"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "P"c, "p"c)
                        UsadosIndices += 1
                    Case "q"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "Q"c, "q"c)
                        UsadosIndices += 1
                    Case "r"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "R"c, "r"c)
                        UsadosIndices += 1
                    Case "s"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "S"c, "s"c)
                        UsadosIndices += 1
                    Case "t"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "T"c, "t"c)
                        UsadosIndices += 1
                    Case "u"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "U"c, "u"c)
                        UsadosIndices += 1
                    Case "v"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "V"c, "v"c)
                        UsadosIndices += 1
                    Case "w"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "W"c, "w"c)
                        UsadosIndices += 1
                    Case "x"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "X"c, "x"c)
                        UsadosIndices += 1
                    Case "y"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "Y"c, "y"c)
                        UsadosIndices += 1
                    Case "z"c : Retornar(UsadosIndices) = If(ProximaMayuscula, "Z"c, "z"c)
                        UsadosIndices += 1


                    Case "A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c, "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c, "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "0"c
                        Retornar(UsadosIndices) = aString(I)
                        UsadosIndices += 1

                    Case Else
                        Continue For

                End Select


                If ProximaMayuscula Then
                    ProximaMayuscula = False
                End If


            End If

        Next

        If UsadosIndices > 0 Then
            Return New String(Retornar, 0, UsadosIndices)
        Else
            Return ""
        End If
    End Function
#Region "Count"

    <Extension()>
    Public Function Count(ByVal o As String) As Integer
        Return o.Length
    End Function


    <Extension()>
    Public Function CountLetrasMasDe1(ByVal Texto As String, ByVal Caracter As Char) As Boolean
        If Texto = "" Then Return False
        Dim Pos1 As Integer = Texto.IndexOf(Caracter)
        Return Pos1 <> -1 AndAlso Texto.IndexOf(Caracter, Pos1 + 1) <> -1
    End Function

    <Extension()>
    Public Function CountLetras(ByVal Texto As String, ByVal Caracter As Char, Optional ByVal max As Integer = -1) As Integer
        If Texto = "" Then Return 0

        Dim total As Integer = 0

        If max = -1 Then

            For i As Integer = 0 To Texto.Length - 1
                If Texto(i) = Caracter Then
                    total += 1
                End If
            Next

        Else

            For i As Integer = 0 To Texto.Length - 1
                If Texto(i) = Caracter Then
                    total += 1
                    If total = max Then
                        Return total
                    End If
                End If
            Next

        End If

        Return total
    End Function

    <Extension()>
    Public Function CountLetrasfueraDeComillas(ByVal Texto As String, ByVal Caracter As Char, ByVal tipoComillas As Char) As Integer
        If Texto = "" Then Return 0
        Dim total As Integer = 0
        For i As Integer = 0 To Texto.Length - 1
            If Texto(i) = Caracter Then
                total += 1
            End If
        Next
        Return total
    End Function

#End Region

#Region "Contains - Starts - Ends"

    <Extension()>
    Public Function ContainsMagico(CadenaEnLaQueSecompara As String, aString As String) As Boolean
        If CadenaEnLaQueSecompara = "" Then Return (aString = "")

        ' LO HAGO MINISUCULAS
        CadenaEnLaQueSecompara = CadenaEnLaQueSecompara.ToLower.LimpiarAcentos
        aString = aString.ToLower.LimpiarAcentos

        ' RECORRO COMPARANDO
        Dim Cadena() As String = aString.ToLower.Split(" "c)
        For i As Integer = 0 To Cadena.Length - 1
            If Cadena(i) = "" Then Continue For
            If Not CadenaEnLaQueSecompara.Contains(Cadena(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()>
    Public Function ContainsMagicoRapido(CadenaEnLaQueSecompara As String, CAdenaBusqueda() As String) As Boolean

        If CadenaEnLaQueSecompara = "" Then
            If CAdenaBusqueda.Length = 0 OrElse CAdenaBusqueda.Length = 1 AndAlso CAdenaBusqueda(0) <> "" Then
                Return True
            Else
                Return False
            End If
        End If

        ' LO HAGO MINISUCULAS
        ' RECORRO COMPARANDO
        For i As Integer = 0 To CAdenaBusqueda.Length - 1
            If CAdenaBusqueda(i) = "" Then Continue For
            If Not CadenaEnLaQueSecompara.Contains(CAdenaBusqueda(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()>
    Public Function ContainsChar(ByRef Cadena$, ByRef O As Char) As Boolean
        Return Cadena$ IsNot Nothing AndAlso Cadena.IndexOf(O) <> -1
    End Function

    <Extension()>
    Public Function ContainsChar(ByRef Cadena$, ByRef O As Char()) As Boolean
        If Cadena = "" OrElse O Is Nothing OrElse O.Length = 0 Then Return False
        For i = 0 To O.Length - 1
            If Cadena.IndexOf(O(i)) <> -1 Then
                Return True
            End If
        Next
        Return False
    End Function

    <Extension()>
    Public Function ContainsIgnoreCase(Cadena$, Valor$) As Boolean
        Return Cadena.IndexOf(Valor, 0, comparisonType:=StringComparison.OrdinalIgnoreCase) > -1
    End Function


    <Extension()>
    Public Function ContieneAlgunNumero(Cadena$) As Boolean
        If Cadena = "" Then Return False
        For i = 0 To Cadena.Length - 1
            If Char.IsNumber(Cadena(i)) Then
                Return True
            End If
        Next
        Return False
    End Function







    <Extension()>
    Public Function EqualsIgnoreCase(Valor As String, Ref$) As Boolean

        If Valor IsNot Nothing AndAlso Ref IsNot Nothing Then
            If Valor.Length <> Ref.Length Then
                Return False
            Else
                Return Valor.Equals(Ref, StringComparison.OrdinalIgnoreCase)
            End If
        ElseIf Valor = "" AndAlso Ref = "" Then
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()>
    Public Function EndsWithIgnoreCase(Valor As String, Ref$) As Boolean
        Return Valor.ToLower.EndsWith(Ref.ToLower)
    End Function

    <Extension()>
    Public Function StartsWithIgnoreCase(Valor As String, TextoPorElCualDebeDeEmpezar$) As Boolean
        If Valor.Length < TextoPorElCualDebeDeEmpezar.Length Then
            Return False
        Else
            Return Valor.IndexOf(TextoPorElCualDebeDeEmpezar, 0, Valor.Length, StringComparison.OrdinalIgnoreCase) = 0
        End If
    End Function

#End Region

#Region "Replace"

    <Extension()>
    Public Function ReplaceIgnoreCase(Valor As String, ValorAnterior As String, NuevoValor As String) As String
        If Valor = "" Then Return ""
        If ValorAnterior = "" Then Return Valor

        Dim Temp As Integer = 0
        While True

            Temp = Valor.IndexOf(ValorAnterior, Temp, StringComparison.OrdinalIgnoreCase)

            If Temp = -1 Then
                Return Valor
            Else
                Valor = Valor.Remove(Temp, ValorAnterior.Length).Insert(Temp, NuevoValor)
            End If

            Temp += ValorAnterior.Length
            If Temp >= Valor.Length Then
                Return Valor
            End If
        End While

        Return Valor
    End Function

    <Extension()>
    Public Function ReplaceInicio(ByVal Texto As String, ByVal Esto As String, ByVal PorEsto As String) As String
        If Texto.StartsWith(Esto) Then
            Return PorEsto & Mid(Texto, Esto.Length + 1)
        Else
            Return Texto
        End If
    End Function

#End Region



    <Extension()>
    Public Function LimpiarEspacios_DoblesInicioYFin(ElSTr As String) As String
        If ElSTr = "" Then
            Return ""
        Else
            Dim R = ElSTr.Trim
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            If R.Contains("  ") = False Then Return R
            R = R.Replace("  ", " ")
            While R.Contains("  ")
                R = R.Replace("  ", " ")
            End While
            Return R
        End If
    End Function


#Region "Inicio y Fin"


    <Extension()>
    Public Function EliminarInicio(ByVal ElSTr As String, ByVal CaracteresQueSequierenEliminar As Integer) As String
        If ElSTr = "" Then
            Return ""
        ElseIf CaracteresQueSequierenEliminar > ElSTr.Length Then
            Return ""
        Else
            Return ElSTr.Substring(CaracteresQueSequierenEliminar, ElSTr.Length - CaracteresQueSequierenEliminar)
        End If
    End Function

    <Extension()>
    Public Function EliminarFinal(ByVal str As String, ByVal CaracteresQueSequierenEliminar As Integer) As String
        If str Is Nothing Then Return ""
        If CaracteresQueSequierenEliminar < str.Length Then
            Return str.Substring(0, str.Length - CaracteresQueSequierenEliminar)
        Else
            Return ""
        End If
    End Function

    <Extension()>
    Public Function EliminarUltimoCaracter(ByVal str As String) As String
        If str = "" Then
            Return ""
        Else
            Return str.Substring(0, str.Length - 1)
        End If
    End Function

    <Extension()>
    Public Function EliminarUltimoCaracter(ByVal str As String, ByVal Caracter As String) As String
        If str.EndsWith(Caracter) Then
            Return str.EliminarFinal(Caracter.Length)
        Else
            Return str
        End If
    End Function

#End Region

#Region "Trozos"

    <Extension()>
    Public Function RecibirPrimerTrozo(Cadena As String, Separador As String, Optional ARecibir% = 1, Optional AIgnorar% = 0) As String


        If Separador = "" Then Return Cadena
        If Cadena = "" Then Return Cadena
        If Separador.Length = 1 Then Return Cadena.RecibirPrimerTrozo(Separador(0), ARecibir, AIgnorar)
        If Cadena.IndexOf(Separador) = -1 Then Return Cadena

        Dim Desde As Integer = 0
        Dim Hasta As Integer = 0

        If AIgnorar > 0 Then

            Select Case AIgnorar
                Case 1
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                Case 2
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                Case 3
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                Case 4
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                Case 5
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                    If Desde = -1 Then Return ""
                Case Else
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    For i = 2 To AIgnorar
                        Desde = Cadena.IndexOf(Separador, Desde + Separador.Length)
                        If Desde = -1 Then Return ""
                    Next
            End Select

        End If

        If ARecibir% > 0 Then
            Select Case ARecibir%
                Case 1
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + Separador.Length, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 2
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + Separador.Length, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 3
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + Separador.Length, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 4
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + Separador.Length, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 5
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + Separador.Length, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case Else
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + Separador.Length, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    For i = 2 To ARecibir
                        Hasta = Cadena.IndexOf(Separador, Hasta + Separador.Length)
                        If Hasta = -1 Then GoTo RecibirTdososGo
                    Next
            End Select
        End If

        If False Then
RecibirTdososGo:
            ARecibir = 0
        End If

        If AIgnorar > 0 And ARecibir% > 0 Then
            Return Mid(Cadena, Desde + Separador.Length + 1, Hasta - Desde - Separador.Length)
        ElseIf AIgnorar > 0 Then
            Return Mid(Cadena, Desde + Separador.Length + 1)
        ElseIf ARecibir% > 0 Then
            Return Mid(Cadena, 1, Hasta)
        Else
            Return Cadena
        End If
    End Function


    <Extension()>
    Public Function RecibirPrimerTrozo(Cadena As String, Separador As Char) As String

        If Cadena IsNot Nothing Then

            Dim Pos = Cadena.IndexOf(Separador, 0)

            If Pos > -1 Then
                Return Mid(Cadena, 1, Pos)
            Else
                Return Cadena
            End If

        Else
            Return ""
        End If
    End Function


    <Extension()>
    Public Function RecibirPrimerTrozo(Cadena As String, Separador As Char, ARecibir%, Optional AIgnorar% = 0) As String
        If Cadena = "" Then Return ""
        If Cadena.IndexOf(Separador) = -1 Then Return Cadena

        Dim Desde As Integer = 0
        Dim Hasta As Integer = 0

        If AIgnorar > 0 Then

            Select Case AIgnorar
                Case 1
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                Case 2
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                Case 3
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                Case 4
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                Case 5
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                    Desde = Cadena.IndexOf(Separador, Desde + 1)
                    If Desde = -1 Then Return ""
                Case Else
                    Desde = Cadena.IndexOf(Separador, 0)
                    If Desde = -1 Then Return ""
                    For i = 2 To AIgnorar
                        Desde = Cadena.IndexOf(Separador, Desde + 1)
                        If Desde = -1 Then Return ""
                    Next
            End Select

        End If

        If ARecibir% > 0 Then
            Select Case ARecibir%
                Case 1
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + 1, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 2
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + 1, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 3
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + 1, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 4
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + 1, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case 5
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + 1, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                    If Hasta = -1 Then GoTo RecibirTdososGo
                Case Else
                    Hasta = Cadena.IndexOf(Separador, If(AIgnorar > 0, Desde + 1, 0))
                    If Hasta = -1 Then GoTo RecibirTdososGo
                    For i = 2 To ARecibir
                        Hasta = Cadena.IndexOf(Separador, Hasta + 1)
                        If Hasta = -1 Then GoTo RecibirTdososGo
                    Next
            End Select
        End If

        If False Then
RecibirTdososGo:
            ARecibir = 0
        End If

        If AIgnorar > 0 And ARecibir% > 0 Then
            Return Mid(Cadena, Desde + 2, Hasta - Desde - 1)
        ElseIf AIgnorar > 0 Then
            Return Mid(Cadena, Desde + 2)
        ElseIf ARecibir% > 0 Then
            Return Mid(Cadena, 1, Hasta)
        Else
            Return Cadena
        End If
    End Function

    <Extension()>
    Public Function RecibirUltimoTrozo(ByVal ElSTr As String, SeparadorDeTrozos As Char) As String
        If ElSTr = "" Then
            Return ""
        ElseIf ElSTr.IndexOf(SeparadorDeTrozos) <> -1 Then
            Return Mid(ElSTr, ElSTr.LastIndexOf(SeparadorDeTrozos) + 2)
        Else
            Return ElSTr
        End If
    End Function

    <Extension()>
    Public Function RecibirUltimoTrozo(ByVal ElSTr As String, SeparadorDeTrozos As String, Optional CAtnRecibir As Integer = 1, Optional CantidadTrozosAIgnorar As Integer = 0) As String
        If ElSTr = "" Then
            Return ""
        ElseIf ElSTr.Contains(SeparadorDeTrozos) = False Then
            If CAtnRecibir = 1 Then
                Return ElSTr
            Else
                Return ""
            End If
        ElseIf CAtnRecibir = 1 AndAlso CantidadTrozosAIgnorar = 0 Then
            Return Mid(ElSTr, InStrRev(ElSTr, SeparadorDeTrozos) + SeparadorDeTrozos.Length)
        Else

            CAtnRecibir += CantidadTrozosAIgnorar

            Dim Posicion As Integer = ElSTr.Length
            Dim CaracteresAIgnorar As Integer = 0

            For i As Integer = 1 To CAtnRecibir

                Posicion = InStrRev(ElSTr, SeparadorDeTrozos, Posicion - 1)

                If i <= CantidadTrozosAIgnorar Then
                    CaracteresAIgnorar = Posicion
                End If

                If Posicion < 0 Then
                    Return ""
                End If
            Next
            '

            If CaracteresAIgnorar > 0 Then
                'Dim Desde As Integer = Posicion + 1
                'Dim Hasta As Integer = ElSTr.Length - Desde - (ElSTr.Length - CaracteresAIgnorar)
                Return Mid(ElSTr, Posicion + 1, ElSTr.Length - (Posicion + 1) - (ElSTr.Length - CaracteresAIgnorar))
            Else
                Return Mid(ElSTr, Posicion + 1)
            End If

        End If
    End Function

    <Extension()>
    Public Function EliminarTrozoInicio(Cadena$, Separador$, Optional TrozosAEliminar% = 1) As String


        Dim Desde As Integer = 1
        Dim TamaCadena% = Cadena.Length

        If TrozosAEliminar > 0 Then
            While TrozosAEliminar > 0

                Dim kk = InStr(Desde, Cadena, Separador)
                If kk = 0 Then Return ""
                Desde = kk + Separador.Length

                If Desde = -1 Or Desde = 0 Or Desde > TamaCadena Then
                    Return ""
                End If

                TrozosAEliminar -= 1
            End While
        End If

        Return Mid(Cadena, Desde)
    End Function

    <Extension()>
    Public Function EliminarUltimoTrozo(ByVal ElSTr As String, SeparadorDeTrozos As String, Optional CantidadDeTrozosaEliminar As Integer = 1) As String
        If ElSTr.Contains(SeparadorDeTrozos) = False Then
            Return ""
        ElseIf CantidadDeTrozosaEliminar = 1 Then
            Return Mid(ElSTr, 1, InStrRev(ElSTr, SeparadorDeTrozos) - 1)
        Else

            For i = 1 To CantidadDeTrozosaEliminar
                ElSTr = ElSTr.EliminarUltimoTrozo(SeparadorDeTrozos)
                If ElSTr = "" Then Return ElSTr
            Next

            Return ElSTr
        End If
    End Function

#End Region
#Region "Que no Termine"


    <Extension()>
    Public Function QueNoTermineEn(Texto As String, FinCadena As String) As String
        If Texto = "" Then Return ""
        If FinCadena = "" Then Return Texto
        If Texto.EndsWith(FinCadena) Then
            Return Texto.Remove(Texto.Length - FinCadena.Length, FinCadena.Length).QueNoTermineEn(FinCadena)
        Else
            Return Texto
        End If
    End Function

    <Extension()>
    Public Function QueNoTermineEn(Texto As String, FinCadena As Char) As String
        If Texto = "" Then Return ""
        If Texto(Texto.Length - 1).Equals(FinCadena) Then
            Return Texto.Remove(Texto.Length - 1, 1).QueNoTermineEn(FinCadena)
        Else
            Return Texto
        End If
    End Function

#End Region


End Module
