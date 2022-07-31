Imports System.Runtime.CompilerServices

Partial Public Module ExtensionesM







    Public Sub Swap(Of t)(ByRef uno As t, ByRef dos As t)
        Dim temp As t = uno
        uno = dos
        dos = temp
    End Sub
    <Extension()>
    Public Sub SubirItem(Of t)(Lista As List(Of t), Item As t, Optional Cantidad As Integer = 1)
        Dim IndiceOrigen = Lista.IndexOf(Item)
        Dim IndiceDestino = IndiceOrigen - Cantidad
        If Lista.Count > IndiceDestino AndAlso IndiceDestino >= 0 Then
            Swap(Lista(IndiceOrigen), Lista(IndiceDestino))
        End If
    End Sub


    <Extension()>
    Public Sub BajarItem(Of t)(Lista As List(Of t), Item As t, Optional Cantidad As Integer = 1)
        Dim IndiceOrigen = Lista.IndexOf(Item)
        Dim IndiceDestino = IndiceOrigen + Cantidad
        If Lista.Count > IndiceDestino AndAlso IndiceDestino >= 0 Then
            Swap(Lista(IndiceOrigen), Lista(IndiceDestino))
        End If
    End Sub

    <Extension()>
    Public Function TryDequeue(Of t)(Lista As Queue(Of t), ByRef return_valor As t) As Boolean
        If Lista.Count > 0 Then
            return_valor = Lista.Dequeue
            Return True
        Else
            Return False
        End If
    End Function


    <Extension()>
    Public Function TryDequeue(Lista As Queue(Of String), ByRef return_valor As String) As Boolean
        If Lista.Count > 0 Then
            return_valor = Lista.Dequeue
            Return True
        Else
            Return False
        End If
    End Function




#Region "Hacer magia Array"


    <Extension()>
    Public Function HacerMagiaArray(ElArray As List(Of String), i As Integer) As String
        If i < 0 Then Return Nothing
        If ElArray Is Nothing Then Return Nothing
        If ElArray.Count = 0 Then Return Nothing
        If ElArray.Count <= i Then Return Nothing
        Return ElArray(i)
    End Function


    <Extension()>
    Public Function HacerMagiaArray(ElArray As List(Of Guid), i As Integer) As Guid
        If i < 0 Then Return Guid.Empty
        If ElArray Is Nothing Then Return Guid.Empty
        If ElArray.Count = 0 Then Return Guid.Empty
        If ElArray.Count <= i Then Return Guid.Empty
        Return ElArray(i)
    End Function


    <Extension()>
    Public Function HacerMagiaArray(ElArray As Guid(), i As Integer) As Guid
        If i < 0 Then Return Guid.Empty
        If ElArray Is Nothing Then Return Guid.Empty
        If ElArray.Length = 0 Then Return Guid.Empty
        If ElArray.Length <= i Then Return Guid.Empty
        Return ElArray(i)
    End Function


    <Extension()>
    Public Function HacerMagiaArray(ElArray As List(Of String), i As Integer, Defecto As String) As String
        If i < 0 Then Return Defecto
        If ElArray.Count = 0 Then Return Defecto
        If ElArray.Count <= i Then Return Defecto
        Return ElArray(i)
    End Function

    <Extension()>
    Public Function HacerMagiaArray(ElArray() As String, i As Integer, ValorDefecto As String) As String
        If i < 0 Then Return ValorDefecto
        If ElArray Is Nothing Then Return ValorDefecto
        If ElArray.Length = 0 Then Return ValorDefecto
        If ElArray.Length <= i Then Return ValorDefecto
        Return ElArray(i)
    End Function

    <Extension()>
    Public Function HacerMagiaArray(ElArray() As String, i As Integer) As String
        If i < 0 Then Return Nothing
        If ElArray Is Nothing Then Return Nothing
        If ElArray.Length = 0 Then Return Nothing
        If ElArray.Length <= i Then Return Nothing
        Return ElArray(i)
    End Function

    <Extension()>
    Public Function HacerMagiaArray(ElArray() As Dic(Of String, String), i As Integer) As Dic(Of String, String)
        If i < 0 Then Return Nothing
        If ElArray Is Nothing Then Return Nothing
        If ElArray.Length = 0 Then Return Nothing
        If ElArray.Length <= i Then Return Nothing
        Return ElArray(i)
    End Function

    <Extension()>
    Public Function HacerMagiaArray_Generico(Of t)(ElArray() As t, i As Integer) As t
        If i < 0 Then Return Nothing
        If ElArray Is Nothing Then Return Nothing
        If ElArray.Length = 0 Then Return Nothing
        If ElArray.Length <= i Then Return Nothing
        Return ElArray(i)
    End Function


    <Extension()>
    Public Function HacerMagiaArray(ElArray() As Integer, i As Integer) As Integer
        If i < 0 Then Return Nothing
        If ElArray Is Nothing Then Return Nothing
        If ElArray.Length = 0 Then Return Nothing
        If ElArray.Length <= i Then Return Nothing
        Return ElArray(i)
    End Function


    <Extension()>
    Public Function HacerMagiaArray(Of t)(ElArray As List(Of t), i As Integer) As t
        If i < 0 Then Return Nothing
        If ElArray Is Nothing Then Return Nothing
        If ElArray.Count = 0 Then Return Nothing
        If ElArray.Count <= i Then Return Nothing
        Return ElArray(i)
    End Function



#End Region


    <Extension()>
    Public Function AceptaIndice(ElArray() As Integer, IndiceBase0 As Integer) As Boolean
        If IndiceBase0 < 0 Then Return False
        If ElArray Is Nothing Then Return False
        If ElArray.Length = 0 Then Return False
        If ElArray.Length <= IndiceBase0 Then Return False
        Return True
    End Function



    <Extension()>
    Public Function AceptaIndice(Of t)(ElArray() As t, IndiceBase0 As Integer) As Boolean
        If IndiceBase0 < 0 Then Return False
        If ElArray Is Nothing Then Return False
        If ElArray.Length = 0 Then Return False
        If ElArray.Length <= IndiceBase0 Then Return False
        Return True
    End Function


    <Extension()>
    Public Function AceptaIndice(Of t)(ElArray As IEnumerable(Of t), IndiceBase0 As Integer) As Boolean
        If IndiceBase0 < 0 Then Return False
        If ElArray Is Nothing Then Return False
        If ElArray.Count = 0 Then Return False
        If ElArray.Count <= IndiceBase0 Then Return False
        Return True
    End Function


    <Extension()>
    Public Function AceptaIndice(ElArray As String(), IndiceBase0 As Integer) As Boolean
        If IndiceBase0 < 0 Then Return False
        If ElArray Is Nothing Then Return False
        If ElArray.Length = 0 Then Return False
        If ElArray.Length <= IndiceBase0 Then Return False
        Return True
    End Function


    <Extension()>
    Public Function AceptaIndice(Of T)(ElArray As List(Of T), IndiceBase0 As Integer) As Boolean
        If IndiceBase0 < 0 Then Return False
        If ElArray Is Nothing Then Return False
        If ElArray.Count = 0 Then Return False
        If ElArray.Count <= IndiceBase0 Then Return False
        Return True
    End Function


    <Extension()>
    Public Function DividirEnPaquetes20(Of t)(LaLista As List(Of t), EnPaquetesDE%) As List(Of List(Of t))
        Dim Retornar As New List(Of List(Of t))
        If LaLista Is Nothing Then
            Return Retornar
        ElseIf LaLista.Count <= EnPaquetesDE Then
            Retornar.Add(LaLista)
            Return Retornar
        Else
            Dim BufferActualX As New List(Of t)(EnPaquetesDE%)
            For I As Integer = 1 To LaLista.Count
                BufferActualX.Add(LaLista(I - 1))
                If I.EsMultiploDe(EnPaquetesDE) Then
                    Retornar.Add(BufferActualX)
                    BufferActualX = New List(Of t)
                End If
            Next
            If BufferActualX.Count > 0 Then
                Retornar.Add(BufferActualX)
            End If
        End If
        Return Retornar
    End Function






#Region "Tiene Datos"
    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(o As HashSet(Of String)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function

    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(Of t)(o As HashSet(Of t)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function

    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(o As Dic(Of String, Integer)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function

    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(o As Dic(Of String, String())) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function
    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(o As Dic(Of Guid, String)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function

    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(Of t, v)(o As Dic(Of t, v)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function


    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(o As Dic(Of String, HashSet(Of String))) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function


    <Extension()>
    Public Function TieneDatos(o As Byte()) As Boolean
        Return o IsNot Nothing AndAlso o.Length > 0
    End Function


    <Extension()>
    Public Function TieneDatos(o As List(Of System.Guid)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function

    <Extension()>
    Public Function TieneDatos(temp As ICollection) As Boolean
        Return temp IsNot Nothing AndAlso temp.Count > 0
    End Function

    <Extension()>
    Public Function TieneDatos(o As List(Of String)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function


 

    <Extension()>
    Public Function TieneDatos(o As String()) As Boolean
        Return o IsNot Nothing AndAlso o.Length > 0
    End Function
    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(o As Dic(Of String, String)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function
    <Extension()> <DebuggerNonUserCode>
    Public Function TieneDatos(ByRef o As Dic(Of String, Boolean)) As Boolean
        Return o IsNot Nothing AndAlso o.Count > 0
    End Function

#End Region
#Region "Listas"


    <Extension()>
    Public Sub Add(o As List(Of String), valor1 As String, valor2 As String)
        o.Add(valor1)
        o.Add(valor2)
    End Sub

    <Extension()>
    Public Sub Add(o As List(Of String), valor1 As String, valor2 As String(), valor3 As String)
        o.Add(valor1)
        o.AddRange(valor2)
        o.Add(valor3)
    End Sub

    <Extension()>
    Public Sub Add(o As List(Of String), valor1 As String, valor2 As String, valor3 As String)
        o.Add(valor1)
        o.Add(valor2)
        o.Add(valor3)
    End Sub

    <Extension()>
    Public Sub Add(o As List(Of String), ParamArray valor1() As String)
        o.AddRange(valor1)
    End Sub

    <Extension()>
    Public Sub ADD_A_PN(ByRef O As List(Of Guid), Valor As Guid)
        If O Is Nothing Then O = New List(Of Guid)
        O.Add(Valor)
    End Sub

    <Extension()>
    Public Sub ADD_A_PN(Of t)(ByRef O As List(Of t), Valor As t)
        If O Is Nothing Then O = New List(Of t)
        O.Add(Valor)
    End Sub

    <Extension()>
    Public Sub ADD_A_PN(ByRef O As List(Of String), Valor As String)
        If O Is Nothing Then
            O = New List(Of String)
        End If
        O.Add(Valor)
    End Sub

    <Extension()>
    Public Sub AddRange(O As HashSet(Of Date), Val As Date())
        O.UnionWith(Val)
    End Sub

    <Extension()>
    Public Sub AddRange(O As HashSet(Of Guid), val As HashSet(Of Guid))
        O.UnionWith(val)
    End Sub

    <Extension()>
    Public Sub AddRange(O As HashSet(Of Decimal), val As HashSet(Of Decimal))
        O.UnionWith(val)
    End Sub


    <Extension()>
    Public Sub AddRange(O As HashSet(Of Decimal), val As List(Of Decimal))
        O.UnionWith(val)
    End Sub




    <Extension()>
    Public Sub AddRange(O As HashSet(Of Decimal), val As Decimal())
        O.UnionWith(val)
    End Sub



















    <Extension()>
    Public Function AddRange(O As HashSet(Of Guid), val As Guid()) As Boolean
        Dim Antes = O.Count
        O.UnionWith(val)
        Return Antes <> O.Count
    End Function

    <Extension()>
    Public Function AddRange(O As HashSet(Of String), val As Dic(Of String, String).KeyCollection) As Boolean
        Dim Antes = O.Count
        O.UnionWith(val)
        Return Antes <> O.Count
    End Function

    <Extension()>
    Public Function AddRange(O As HashSet(Of String), val As IEnumerable(Of String)) As Boolean
        Dim Antes = O.Count
        O.UnionWith(val)
        Return Antes <> O.Count
    End Function

    <Extension()>
    Public Function AddRange(O As HashSet(Of String), Val As List(Of String)) As Boolean
        Dim Antes = O.Count
        O.UnionWith(Val)
        Return Antes <> O.Count
    End Function

    <Extension()>
    Public Function AddRange(O As HashSet(Of String), Val As HashSet(Of String)) As Boolean
        Dim Antes = O.Count
        O.UnionWith(Val)
        Return Antes <> O.Count
    End Function

    <Extension()>
    Public Function GarantizarQueNoHayanValuesRepetidos_KeyEntreParentesis(O As Dic(Of Integer, String)) As Dic(Of Integer, String)

        Dim CuentaValores As New Dic(Of String, Integer)

        For Each actual In O
            CuentaValores.HacerMagia(actual.Value) += 1
        Next


        Dim R As New Dic(Of Integer, String)
        For Each actual In O

            Dim NuevoValue_Esperado = actual.Value
            If CuentaValores(actual.Value) > 1 Then
                NuevoValue_Esperado = actual.Value & " (" & actual.Key & ")"
            End If



            Dim NuevoValue_Confirmado = NuevoValue_Esperado

            ', Agrego la key entre paréntesis y precisamente por eso 
            ', hago esta comprobación. Quizás "ya exista esa nuevo nopmbre"
            Dim Cuenta% = 0
            While R.Values.Contains(NuevoValue_Confirmado)
                Cuenta += 1
                NuevoValue_Confirmado = NuevoValue_Esperado & " (" & Cuenta.ToString & ")"
            End While

            R.Add(actual.Key, NuevoValue_Confirmado)

        Next

        Return R
    End Function


    <Extension()>
    Public Function GarantizarQueNoHayanValuesRepetidos_KeyEntreParentesis(O As Dic(Of String, String)) As Dic(Of String, String)

        Dim CuentaValores As New Dic(Of String, Integer)

        For Each actual In O
            CuentaValores.HacerMagia(actual.Value) += 1
        Next


        Dim R As New Dic(Of String, String)
        For Each actual In O

            Dim NuevoValue_Esperado = actual.Value
            If CuentaValores(actual.Value) > 1 Then
                NuevoValue_Esperado = actual.Value & " (" & actual.Key & ")"
            End If



            Dim NuevoValue_Confirmado = NuevoValue_Esperado

            ', Agrego la key entre paréntesis y precisamente por eso 
            ', hago esta comprobación. Quizás "ya exista esa nuevo nopmbre"
            Dim Cuenta% = 0
            While R.Values.Contains(NuevoValue_Confirmado)
                Cuenta += 1
                NuevoValue_Confirmado = NuevoValue_Esperado & " (" & Cuenta.ToString & ")"
            End While

            R.Add(actual.Key, NuevoValue_Confirmado)

        Next

        Return R
    End Function



    <Extension()>
    Public Sub AddRange(o As List(Of String), valor1 As String, valor2 As List(Of String), valor3 As String)
        o.Add(valor1)
        o.AddRange(valor2)
        o.Add(valor3)
    End Sub

    <Extension>
    Public Function AddSiNoExiste(Of tkey, tvalue)(eldic As Dic(Of tkey, tvalue), Key As tkey, Val As tvalue) As Boolean
        If eldic.ContainsKey(Key) = False Then
            eldic.Add(Key, Val)
            Return True
        Else
            Return False
        End If
    End Function
    <Extension()>
    Public Function AddSiNoExiste(O As Dic(Of String, Integer), Keey As String, Val As Integer) As Boolean
        If O.ContainsKey(Keey) = False Then
            O.Add(Keey, Val)
            Return True
        Else
            Return False
        End If
    End Function
    <Extension()>
    Public Function AddSiNoExiste(O As Dic(Of String, Long), Keey As String, Val As Long) As Boolean
        If O.ContainsKey(Keey) = False Then
            O.Add(Keey, Val)
            Return True
        Else
            Return False
        End If
    End Function
    <Extension()>
    Public Function AddSiNoExiste(O As Dic(Of String, String), Keey As String, Val As String) As Boolean
        If O.ContainsKey(Keey) = False Then
            O.Add(Keey, Val)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()>
    Public Function AddSiNoExiste(O As List(Of Guid), Valor As Guid) As Boolean
        If O.Contains(Valor) Then Return False
        O.Add(Valor)
        Return True
    End Function

    <Extension()>
    Public Function AddSiNoExiste(O As List(Of Integer), Valor As Integer) As Boolean
        If O.Contains(Valor) = False Then
            O.Add(Valor)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()>
    Public Function AddSiNoExiste(O As List(Of String), Valor As String) As Boolean
        If O.Contains(Valor) = False Then
            O.Add(Valor)
            Return True
        Else
            Return False
        End If
    End Function



    <Extension()>
    Public Function AddSiNoExiste(O As List(Of Decimal), Valor As Decimal) As Boolean
        If O.Contains(Valor) = False Then
            O.Add(Valor)
            Return True
        Else
            Return False
        End If
    End Function


    <Extension()>
    Public Sub ADDSiNoExiste_A_PN(ByRef O As List(Of Guid), Valor As Guid)
        If O Is Nothing Then
            O = New List(Of Guid)
            O.Add(Valor)
        ElseIf O.Contains(Valor) = False Then
            O.Add(Valor)
        End If
    End Sub

    <Extension()>
    Public Sub ADD_A_PN_SiNoEsvacio(ByRef O As List(Of String), Valor As String)
        If Valor <> "" Then
            If O Is Nothing Then
                O = New List(Of String)
            End If
            O.Add(Valor)
        End If
    End Sub
    <Extension()>
    Public Sub Add_Repetido(o As List(Of String), valor1 As String, Cantidad As Integer)
        For i As Integer = 1 To Cantidad
            o.Add(valor1)
        Next
    End Sub
    <Extension()>
    Public Sub AddRangeNoVacios(o As List(Of String), valor1 As List(Of String))
        If valor1 Is Nothing OrElse valor1.Count = 0 Then
            Exit Sub
        ElseIf valor1.Count = 1 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False Then o.Add(valor1(0))
        ElseIf valor1.Count = 2 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False Then o.Add(valor1(1))
        ElseIf valor1.Count = 3 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False Then o.Add(valor1(1))
            If String.IsNullOrWhiteSpace(valor1(2)) = False Then o.Add(valor1(2))
        Else
            For i As Integer = 0 To valor1.Count - 1
                If String.IsNullOrWhiteSpace(valor1(i)) = False Then
                    o.Add(valor1(i))
                End If
            Next
        End If
    End Sub
    <Extension()>
    Public Sub AddRangeNoVacios(o As List(Of String), valor1() As String)
        If valor1 Is Nothing OrElse valor1.Length = 0 Then
            Exit Sub
        ElseIf valor1.Length = 1 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False Then o.Add(valor1(0))
        ElseIf valor1.Length = 2 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False Then o.Add(valor1(1))
        ElseIf valor1.Length = 3 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False Then o.Add(valor1(1))
            If String.IsNullOrWhiteSpace(valor1(2)) = False Then o.Add(valor1(2))
        Else
            For i As Integer = 0 To valor1.Length - 1
                If String.IsNullOrWhiteSpace(valor1(i)) = False Then
                    o.Add(valor1(i))
                End If
            Next
        End If
    End Sub
    <Extension()>
    Public Sub AddRangeNoVaciosNoRepetidos(o As List(Of String), valor1() As String)
        If valor1 Is Nothing OrElse valor1.Length = 0 Then
            Exit Sub
        ElseIf valor1.Length = 1 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False AndAlso o.Contains(valor1(0)) = False Then o.Add(valor1(0))
        ElseIf valor1.Length = 2 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False AndAlso o.Contains(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False AndAlso o.Contains(valor1(1)) = False Then o.Add(valor1(1))
        ElseIf valor1.Length = 3 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False AndAlso o.Contains(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False AndAlso o.Contains(valor1(1)) = False Then o.Add(valor1(1))
            If String.IsNullOrWhiteSpace(valor1(2)) = False AndAlso o.Contains(valor1(2)) = False Then o.Add(valor1(2))
        Else
            For i As Integer = 0 To valor1.Length - 1
                If String.IsNullOrWhiteSpace(valor1(i)) = False AndAlso o.Contains(valor1(i)) = False Then
                    o.Add(valor1(i))
                End If
            Next
        End If
    End Sub


    <Extension()>
    Public Sub AddRangeNoVaciosNoRepetidos(o As List(Of String), valor1 As List(Of String))
        If valor1 Is Nothing OrElse valor1.Count = 0 Then
            Exit Sub
        ElseIf valor1.Count = 1 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False AndAlso o.Contains(valor1(0)) = False Then o.Add(valor1(0))
        ElseIf valor1.Count = 2 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False AndAlso o.Contains(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False AndAlso o.Contains(valor1(1)) = False Then o.Add(valor1(1))
        ElseIf valor1.Count = 3 Then
            If String.IsNullOrWhiteSpace(valor1(0)) = False AndAlso o.Contains(valor1(0)) = False Then o.Add(valor1(0))
            If String.IsNullOrWhiteSpace(valor1(1)) = False AndAlso o.Contains(valor1(1)) = False Then o.Add(valor1(1))
            If String.IsNullOrWhiteSpace(valor1(2)) = False AndAlso o.Contains(valor1(2)) = False Then o.Add(valor1(2))
        Else
            For i As Integer = 0 To valor1.Count - 1
                If String.IsNullOrWhiteSpace(valor1(i)) = False AndAlso o.Contains(valor1(i)) = False Then
                    o.Add(valor1(i))
                End If
            Next
        End If
    End Sub

    <Extension()>
    Public Sub AddNoVacio(o As List(Of String), valor1 As String)
        If String.IsNullOrWhiteSpace(valor1) = False Then
            o.Add(valor1)
        End If
    End Sub

#End Region

End Module
