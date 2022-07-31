Public Interface IDatoCacheable
    ReadOnly Property ID As Guid
    ReadOnly Property FechaIA As Date
End Interface








Public Class CacheableC(Of T)


    Public DicPorID As New DinaNETCore.ExtensionesM.DicCD(Of System.Guid, T)
    Public MaxFecha As DateTime
    Public RutaArchivoCache$




    Public Function RecibirPorID(id As String) As T
        Return RecibirPorID(id.ToGuid)
    End Function
    Public Function RecibirPorID(id As Guid) As T
        If id = Guid.Empty Then Return Nothing
        Return DicPorID.TryHacerMagia(id)
    End Function

    Public Function RecibirPorIDS_SeparadasPorComas(IDsPorComas As String) As Dic(Of Guid, T)


        Dim r = New Dic(Of Guid, T)

        If IDsPorComas = "" Then
            Return r
        ElseIf IDsPorComas.EsGUID Then
            Dim Obj = DicPorID.TryHacerMagia(IDsPorComas.ToGuid)
            If Obj IsNot Nothing Then
                r.Add(IDsPorComas.ToGuid, Obj)
            End If
        ElseIf IDsPorComas.Contains(","c) Then
            Dim Sep = IDsPorComas.Split(","c)
            For i = 0 To Sep.Length - 1
                Dim IDActual = Sep(i).Trim.ToGuid
                If IDActual <> Guid.Empty Then
                    Dim Obj = DicPorID.TryHacerMagia(IDActual)
                    If Obj IsNot Nothing Then
                        r.Add(IDActual, Obj)
                    End If
                End If
            Next
        End If


        Return r

    End Function






    Public Sub Actualizar(Dic As DinaNETCore.ExtensionesM.Dic(Of System.Guid, T), _Maxfecha As DateTime)
        Me.DicPorID.AddOrUpdateDic(Dic)
        Dim FechaCambiada = Me.MaxFecha <> _Maxfecha
        Me.MaxFecha = _Maxfecha
        If Dic.Count > 10 AndAlso FechaCambiada Then
            Guarda_Cache()
        End If
    End Sub



    Public Sub Iniciar(_RutaArchivoCache$)



        Me.RutaArchivoCache = _RutaArchivoCache
        Cargar_Cache()


    End Sub

    Private Sub ComprobarExistenciaDeCarpeta()
        Try
            Dim Carpeta = System.IO.Path.GetDirectoryName(RutaArchivoCache)
            If Carpeta <> "" Then
                If IO.Directory.Exists(Carpeta) = False Then
                    IO.Directory.CreateDirectory(Carpeta)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Guarda_Cache()


        If RutaArchivoCache = "" Then Exit Sub
        ComprobarExistenciaDeCarpeta()
        Try
            If DicPorID.Count < 1000 Then
                ', Se solicitará una vez al iniciar y luego se irá actualizando. 
                ', Pero no veo necesidad de guardarlo en disco
                Exit Sub
            End If
            Dim ListaDeDatos = m_Serializar(Tuple.Create(MaxFecha, DicPorID))
            System.IO.File.WriteAllBytes(Me.RutaArchivoCache, ListaDeDatos)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Cargar_Cache()


        If RutaArchivoCache = "" Then Exit Sub

        ComprobarExistenciaDeCarpeta()



        If System.IO.File.Exists(RutaArchivoCache) Then
            Try
                Dim ListaDeDatosDeserializados = m_DesSerializar(Of Tuple(Of Date, DinaNETCore.ExtensionesM.DicCD(Of System.Guid, T)))(RutaArchivoCache)
                If ListaDeDatosDeserializados IsNot Nothing Then
                    If ListaDeDatosDeserializados.Item1 <> Date.MinValue AndAlso ListaDeDatosDeserializados.Item2.Count > 0 Then
                        DicPorID = ListaDeDatosDeserializados.Item2
                        MaxFecha = ListaDeDatosDeserializados.Item1
                    End If
                End If
            Catch ex As Exception
            End Try
        End If


    End Sub

    Sub New()
    End Sub



    Public Function m_DesSerializar(Of xt)(ArchivoCache As String) As xt
        On Error Resume Next
        If ArchivoCache = "" Then Return Nothing
        If IO.File.Exists(ArchivoCache) = False Then Return Nothing
        Dim deserializedStuff As xt
        Using memStream As New IO.FileStream(ArchivoCache, IO.FileMode.Open)
            If memStream.Length < 3 Then Return Nothing
            memStream.Position = 0
            deserializedStuff = ProtoBuf.Serializer.Deserialize(Of xt)(memStream)
        End Using
        Return deserializedStuff
    End Function


    Public Function m_DesSerializar(Of xt)(Datos As Byte()) As xt
        On Error Resume Next
        If Datos Is Nothing OrElse Datos.Length = 0 Then Return Nothing
        Dim deserializedStuff As xt
        Using memStream As New IO.MemoryStream(Datos)
            memStream.Position = 0
            deserializedStuff = ProtoBuf.Serializer.Deserialize(Of xt)(memStream)
        End Using
        Return deserializedStuff
    End Function

    Public Function m_Serializar(Of xt)(LaClase As xt) As Byte()
        Dim buffer As Byte()
        Using memStream As New IO.MemoryStream
            ProtoBuf.Serializer.Serialize(memStream, LaClase)
            buffer = memStream.ToArray
        End Using
        Return buffer
    End Function




End Class
