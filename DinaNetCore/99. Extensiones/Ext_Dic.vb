
Imports STR = System.String
Imports INT = System.Int32
Imports BOOL = System.Boolean
Imports System.Runtime.CompilerServices

Public Module ExtensionesM


    Public Class DicCD(Of TKey, TValue)
        Inherits Concurrent.ConcurrentDictionary(Of TKey, TValue)






        Public Function Leer(laKey As TKey) As TValue
            Return Me.TryHacerMagia(laKey)
        End Function

        Public Sub AddOrUpdateDic(DicX As Dic(Of TKey, TValue))
            If DicX.TieneDatos Then
                For Each Actual In DicX
                    Me.TryHacerMagia(Actual.Key) = Actual.Value
                Next
            End If
        End Sub


        Friend Function RecibirKeys() As List(Of TKey)
            Return Me.Keys.ToList
        End Function



        Public Sub TryAdd(Key As TKey, Value As TValue)
            Me.AddOrUpdate(Key, Value, Function(x, y) Value)
        End Sub



        Public Sub Remove(Key As TKey)
            Dim Retornar As TValue
            If Key IsNot Nothing AndAlso MyBase.TryGetValue(Key, Retornar) Then
                MyBase.TryRemove(Key, Retornar)
            End If
        End Sub


        Public Overloads Property TryHacerMagia(ByVal Key As TKey) As TValue
            Get
                Dim Retornar As TValue
                If Key IsNot Nothing AndAlso MyBase.TryGetValue(Key, Retornar) Then
                    Return Retornar
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal Value As TValue)
                Me.AddOrUpdate(Key, Value, Function(Clave, ValorAntiguo) Value)
            End Set
        End Property


        Public Overloads Property TryHacerMagia(ByVal Key As TKey, ByVal Defecto As TValue) As TValue
            Get
                Dim Retornar As TValue
                If Key IsNot Nothing AndAlso MyBase.TryGetValue(Key, Retornar) Then
                    Return Retornar
                Else
                    Return Defecto
                End If
            End Get
            Set(ByVal Value As TValue)
                Me.AddOrUpdate(Key, Value, Function(Clave, ValorAntiguo) Value)
            End Set
        End Property


        Friend Function ToDic() As Dic(Of TKey, TValue)
            Return New Dic(Of TKey, TValue)(Me)
        End Function


        Sub New(dix As System.Collections.Generic.Dictionary(Of TKey, TValue))
            MyBase.New(dix)
        End Sub

        Sub New()
        End Sub
    End Class





    Public Class Dic(Of TKey, TValue)
        Inherits System.Collections.Generic.Dictionary(Of TKey, TValue)

        Public Congelar_Bloquear_Cache As Boolean = False



        Private Memactual As Tuple(Of TKey, TValue)



        ReadOnly Property GetM(ElKey As TKey) As TValue
            Get
                Dim r As Tuple(Of TKey, TValue) = Memactual
                If r IsNot Nothing AndAlso r.Item1.Equals(ElKey) Then
                    Return r.Item2
                ElseIf MyBase.ContainsKey(ElKey) = False Then
                    Throw New Exception("Key " & ElKey.ToString & " no encontrada...")
                End If
                r = New Tuple(Of TKey, TValue)(ElKey, MyBase.Item(ElKey))
                Memactual = r
                Return r.Item2
            End Get
        End Property

        Friend Function TryGetValueX(ClaveX As TKey, ByRef ValorX As TValue) As BOOL
            Dim temp As TValue = ValorX
            If MyBase.TryGetValue(ClaveX, temp) Then
                ValorX = temp
                Return True
            Else
                Return False
            End If
        End Function



        Default Shadows Property Item(ElKey As TKey) As TValue
            Get
                Dim r = Memactual
                If r IsNot Nothing AndAlso r.Item1.Equals(ElKey) Then
                    Return r.Item2
                ElseIf MyBase.ContainsKey(ElKey) = False Then
                    Throw New Exception("Key " & ElKey.ToString & " no encontrada...")
                End If
                Return MyBase.Item(ElKey)
            End Get
            Set(value As TValue)
                MyBase.Item(ElKey) = value
                Memactual = Nothing
            End Set
        End Property



        Public Overloads Function Remove(ByVal ElKey As TKey) As Boolean
            Dim r = Memactual
            If r IsNot Nothing AndAlso r.Item1.Equals(ElKey) Then
                Memactual = Nothing
            End If
            Return MyBase.Remove(ElKey)
        End Function


        Public Overloads Sub Add(ByVal ElKey As TKey, Val As TValue)
            If MyBase.ContainsKey(ElKey) Then
                Throw New Exception("Key " & ElKey.ToString & " duplicada.")
            End If
            MyBase.Add(ElKey, Val)
        End Sub





        Public Function AddInline(ByVal Key As TKey, Val As TValue) As Dic(Of TKey, TValue)
            Me.Add(Key, Val)
            Return Me
        End Function

        Public Function AddInline(ByVal OtroDic As Dic(Of TKey, TValue)) As Dic(Of TKey, TValue)
            For Each keyValuePair As KeyValuePair(Of TKey, TValue) In OtroDic
                Me.Add(keyValuePair.Key, keyValuePair.Value)
            Next
            Return Me
        End Function


        Public Function ClonarDIC() As Dic(Of TKey, TValue)
            Return New Dic(Of TKey, TValue)(Me)
        End Function









        Public Overloads Property HacerMagia(ByVal Key As TKey) As TValue
            Get
                Dim Retornar As TValue
                If Key IsNot Nothing AndAlso MyBase.TryGetValue(Key, Retornar) Then
                    Return Retornar
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal Value As TValue)

                If Congelar_Bloquear_Cache Then
                    Throw New Exception("Diccionario es solo lectura")
                ElseIf MyBase.ContainsKey(Key) Then
                    MyBase.Item(Key) = Value
                    Memactual = Nothing
                Else
                    MyBase.Add(Key, Value)
                End If

            End Set
        End Property


        Public Overloads Property HacerMagia(ByVal Key As TKey, ByVal predeterminado As TValue) As TValue
            Get

                Dim Retornar As TValue
                If Key IsNot Nothing AndAlso MyBase.TryGetValue(Key, Retornar) Then
                    Return Retornar
                Else
                    Return predeterminado
                End If

            End Get
            Set(ByVal Value As TValue)

                If Congelar_Bloquear_Cache Then
                    Throw New Exception("Diccionario es solo lectura")
                ElseIf MyBase.ContainsKey(Key) Then
                    MyBase.Item(Key) = Value
                    Memactual = Nothing
                Else
                    MyBase.Add(Key, Value)
                End If

            End Set
        End Property



        Overloads Sub Clear_SiHayMasDE(MasDe%)
            If Me.Count > MasDe Then
                Me.Clear()
            End If
        End Sub


        Overloads Sub Clear()
            If Congelar_Bloquear_Cache Then
                Throw New Exception("Diccionario es solo lectura")
            Else
                MyBase.Clear()
            End If
            Memactual = Nothing
        End Sub


        Public Sub Agregar(ByVal Key As TKey, ByVal Value As TValue)

            If Congelar_Bloquear_Cache Then
                Throw New Exception("Diccionario es solo lectura")
            End If
            If Not Me.ContainsKey(Key) Then Me.Add(Key, Value)
        End Sub


        Public Function Eliminar(ByVal Key As TKey) As Boolean

            If Congelar_Bloquear_Cache Then
                Throw New Exception("Diccionario es solo lectura")
            End If
            If Me.ContainsKey(Key) Then
                Me.Remove(Key)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function DuplicarDic() As Dic(Of TKey, TValue)
            Dim Retornar As New Dic(Of TKey, TValue)(Me)
            Return Retornar
        End Function


        Public Function CK(Key As TKey) As Boolean
            Return Me.ContainsKey(Key)
        End Function



        Sub New(ByVal Capacidad As Integer)
            MyBase.New(Capacidad + 10)
        End Sub
        Sub New(Diccionario As IDictionary(Of TKey, TValue))
            MyBase.New(Diccionario)
        End Sub


        Sub New(x As IEnumerable(Of IGrouping(Of TKey, TValue)))


        End Sub

        Sub New()
        End Sub
    End Class




    <Extension()>
    Public Function ToDic(Of TKey, TValue)(x As IEnumerable(Of IGrouping(Of TKey, TValue))) As Dic(Of TKey, List(Of TValue))
        Dim R As New Dic(Of TKey, List(Of TValue))
        For Each actual In x
            R.Add(actual.Key, actual.ToList)
        Next
        Return R
    End Function




    <Extension()>
    Public Function AddMagico(Of t, v, r, f)(Coleccion As Dic(Of t, Dic(Of v, Dic(Of f, r))), Key1 As t, Key2 As v, Key3 As f, Valor As r) As Boolean


        Dim BaseKey1 As Dic(Of v, Dic(Of f, r))
        Dim BaseKey2 As Dic(Of f, r)

        If Coleccion.ContainsKey(Key1) = False Then
            BaseKey1 = New Dic(Of v, Dic(Of f, r))
            BaseKey2 = New Dic(Of f, r)
            BaseKey1.Add(Key2, BaseKey2)
            BaseKey2.Add(Key3, Valor)
            Coleccion.Add(Key1, BaseKey1)
            Return True
        Else


            BaseKey1 = Coleccion(Key1)


            If BaseKey1.ContainsKey(Key2) = False Then

                Dim NuevoDic2 As New Dic(Of f, r)
                NuevoDic2.Add(Key3, Valor)
                BaseKey1.Add(Key2, NuevoDic2)
                Return True

            Else

                Dim dic2 = BaseKey1(Key2)

                If dic2.ContainsKey(Key3) = False Then
                    dic2.Add(Key3, Valor)
                    Return True
                End If

            End If



        End If



        Return False
    End Function
    <Extension()>
    Public Function AddMagico(Of t, v, f)(Coleccion As Dic(Of t, Dic(Of v, List(Of f))), Key1 As t, Key2 As v, Valor As f, PermitirValoresRepetidos As BOOL) As BOOL


        Dim ContenedorDecontenedor As Dic(Of v, List(Of f))
        Dim CotenedorDeLista As List(Of f)



        If Coleccion.ContainsKey(Key1) = False Then
            ContenedorDecontenedor = New Dic(Of v, List(Of f))
            CotenedorDeLista = New List(Of f)
            ContenedorDecontenedor.Add(Key2, CotenedorDeLista)
            CotenedorDeLista.Add(Valor)
            Coleccion.Add(Key1, ContenedorDecontenedor)
            Return True
        Else

            ContenedorDecontenedor = Coleccion(Key1)
            If ContenedorDecontenedor.ContainsKey(Key2) = False Then
                CotenedorDeLista = New List(Of f)
                CotenedorDeLista.Add(Valor)
                ContenedorDecontenedor.Add(Key2, CotenedorDeLista)
                Return True
            Else

                Dim ListaDevalores = ContenedorDecontenedor(Key2)
                If PermitirValoresRepetidos OrElse ListaDevalores.Contains(Valor) = False Then
                    ListaDevalores.Add(Valor)
                    Return True
                End If

            End If

        End If


        Return False
    End Function


    <Extension()>
    Public Sub AddMagico(Of t, v)(Coleccion As Dic(Of t, List(Of v)), ByRef Key1 As t, ByRef Valor As v)
        Dim Temp As List(Of v) = Nothing
        If Coleccion.TryGetValueX(Key1, Temp) Then
            Temp.Add(Valor)
        Else
            Temp = New List(Of v)
            Temp.Add(Valor)
            Coleccion.Add(Key1, Temp)
        End If
    End Sub


    <Extension()>
    Public Function AddMagico(Coleccion As Dic(Of Integer, HashSet(Of Integer)), ByRef Key1 As Integer, ByRef Valor As Integer) As BOOL
        Dim Temp As HashSet(Of Integer) = Nothing
        If Coleccion.TryGetValueX(Key1, Temp) Then
            Return Temp.Add(Valor)
        Else
            Temp = New HashSet(Of Integer)
            Temp.Add(Valor)
            Coleccion.Add(Key1, Temp)
            Return True
        End If
    End Function

    <Extension()>
    Public Sub AddMagico(Coleccion As Dic(Of STR, HashSet(Of Guid)), ByRef Key1 As STR, ByRef Valor As Guid)
        Dim Temp As HashSet(Of Guid) = Nothing
        If Coleccion.TryGetValueX(Key1, Temp) Then
            Temp.Add(Valor)
        Else
            Temp = New HashSet(Of Guid)
            Temp.Add(Valor)
            Coleccion.Add(Key1, Temp)
        End If
    End Sub


    <Extension()>
    Public Function AddMagico(Coleccion As Dic(Of Guid, HashSet(Of Guid)), ByRef Key1 As Guid, ByRef Valor As Guid) As BOOL
        Dim Temp As HashSet(Of Guid) = Nothing
        If Coleccion.TryGetValueX(Key1, Temp) Then
            Return Temp.Add(Valor)
        Else
            Temp = New HashSet(Of Guid)
            Temp.Add(Valor)
            Coleccion.Add(Key1, Temp)
            Return True
        End If
    End Function

    <Extension()>
    Public Sub AddMagico(Coleccion As Dic(Of STR, HashSet(Of STR)), ByRef Key1 As STR, ByRef Valor As List(Of STR))
        Dim Temp As HashSet(Of STR) = Nothing
        If Coleccion.TryGetValueX(Key1, Temp) Then
            Temp.AddRange(Valor)
        Else
            Coleccion.Add(Key1, New HashSet(Of STR)(Valor))
        End If
    End Sub


    <Extension()>
    Public Function AddMagico(Coleccion As Dic(Of STR, HashSet(Of STR)), ByRef Key1 As STR, ByRef Valor As STR) As BOOL
        Dim Temp As HashSet(Of STR) = Nothing
        If Coleccion.TryGetValueX(Key1, Temp) Then
            Return Temp.Add(Valor)
        Else
            Coleccion.Add(Key1, New HashSet(Of STR)({Valor}))
            Return True
        End If
    End Function



    <Extension()>
    Public Sub AddMagico(Coleccion As Dic(Of STR, List(Of STR)), ByRef Key1 As STR, ByRef Valor As STR)
        Dim Temp As List(Of String) = Nothing
        If Coleccion.TryGetValueX(Key1, Temp) Then
            Temp.Add(Valor)
        Else
            Temp = New List(Of String)
            Temp.Add(Valor)
            Coleccion.Add(Key1, Temp)
        End If
    End Sub
    '

    <Extension()>
    Public Function AddMagico(Coleccion As Dic(Of String, Dic(Of String, HashSet(Of STR))), Key1 As STR, Key2 As STR, Valor As String) As BOOL


        If Coleccion.ContainsKey(Key1) = False Then
            Dim ContenedorDecontenedor As Dic(Of String, HashSet(Of STR))
            Dim CotenedorDeLista As HashSet(Of STR)
            ContenedorDecontenedor = New Dic(Of String, HashSet(Of STR))
            CotenedorDeLista = New HashSet(Of STR)
            ContenedorDecontenedor.Add(Key2, CotenedorDeLista)
            CotenedorDeLista.Add(Valor)
            Coleccion.Add(Key1, ContenedorDecontenedor)
            Return True
        Else
            If Coleccion(Key1).ContainsKey(Key2) = False Then
                Coleccion(Key1).Add(Key2, New HashSet(Of STR)({Valor}))
                Return True
            Else
                Return Coleccion(Key1)(Key2).Add(Valor)
            End If

        End If
        Return False
    End Function


    <Extension()>
    Public Function AddMagico_PN(ByRef Coleccion_PN As Dic(Of String, Dic(Of String, HashSet(Of STR))), Key1 As STR, Key2 As STR, Valor As String) As BOOL



        If Coleccion_PN Is Nothing Then
            Coleccion_PN = New Dic(Of String, Dic(Of String, HashSet(Of String)))
        End If


        Dim Nivel2 As Dic(Of String, HashSet(Of String))
        Dim Nivel3 As HashSet(Of STR)

        If Coleccion_PN.TryGetValueX(Key1, Nivel2) = False Then
            Nivel2 = New Dic(Of String, HashSet(Of String))
            Coleccion_PN.Add(Key1, Nivel2)
        End If


        If Nivel2.TryGetValueX(Key2, Nivel3) = False Then
            Nivel3 = New HashSet(Of String)
            Nivel2.Add(Key2, Nivel3)
        End If


        Nivel3.Add(Valor)


        Return False


    End Function


End Module