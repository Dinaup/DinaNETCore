Imports System.IO

Partial Public Class HerramientasD
    Public Class CopiaDeArchivosD



        '@ Aviso: Pueden quedar archivos temporales si se interrumpe el proceso de forma forzada
        '@ Si ya existe un archivo con el mismo nombre, no lo reemplaza



        Private Shared BloqueoCopiando As New Dic(Of String, String)
        Private Shared Errores As New Dic(Of String, String)



        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Public Shared Sub LimpiarErrores()
            SyncLock Errores
                Errores.Clear()
            End SyncLock
        End Sub



        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Public Shared Function RecibirEstados() As Dic(Of String, String)
            Dim BloqueoCopiandoTemp As Dic(Of String, String)
            SyncLock BloqueoCopiando
                BloqueoCopiandoTemp = BloqueoCopiando.ClonarDIC
            End SyncLock
            Return BloqueoCopiandoTemp
        End Function



        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Public Shared Function RecibirErrores() As Dic(Of String, String)
            Dim ErroresTemp As Dic(Of String, String)
            SyncLock Errores
                ErroresTemp = Errores.ClonarDIC
            End SyncLock
            Return ErroresTemp
        End Function



        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Public Shared Function CopiarCarpeta(Orgen$, Destino$) As String



            If Directory.Exists(Orgen$) = False Then
                Return "No existe la carpeta origen."
            End If



            If Directory.Exists(Destino$) = False Then
                Return "No existe la carpeta destino."
            End If



            SyncLock BloqueoCopiando



                If BloqueoCopiando.ContainsKey(Orgen$) Then
                    Return "Ya se está copiando la carpeta origen."
                Else
                    BloqueoCopiando.Add(Orgen, "Iniciando...")
                End If



            End SyncLock



            Dim Th As New System.Threading.Thread(Sub()
                                                      ThreadCopiador(Orgen$, Destino$)
                                                  End Sub)
            Th.Name = "Thread Copiador de archivos: " & Orgen$ & " -> " & Destino$
            Th.Start()



            Return ""



        End Function


        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Private Shared Sub CopiarCarpetaInterno(Orgen$, Destino$, CuentaTotal%, ByRef CuentaActual%)



            Dim Directorios As String()



            Try
                Directorios = Directory.GetDirectories(Orgen)
            Catch ex As Exception
                SyncLock Errores
                    If Errores.ContainsKey(Orgen) = False Then
                        Errores.Add(Orgen, ex.Message)
                    End If
                End SyncLock
                Exit Sub
            End Try



            For Each Dire In Directorios

                Dim NombreDirectorio As String = Path.GetFileName(Dire)
                Dim UnError As Boolean = False
                Try
                    If Not Directory.Exists(Path.Combine(Destino, NombreDirectorio)) Then
                        Directory.CreateDirectory(Path.Combine(Destino, NombreDirectorio))
                    End If
                Catch ex As Exception
                    UnError = True
                    SyncLock Errores
                        If Errores.ContainsKey(NombreDirectorio) = False Then
                            Errores.Add(NombreDirectorio, ex.Message)
                        End If
                    End SyncLock
                End Try

                If UnError = False Then
                    CopiarCarpetaInterno(Dire, Path.Combine(Destino, NombreDirectorio), CuentaTotal%, CuentaActual%)
                End If

            Next



            For Each Archivo In Directory.GetFiles(Orgen)



                Try
                    If File.Exists(Path.Combine(Destino, Path.GetFileName(Archivo))) = False Then
                        Dim ArchivoTemp = Path.Combine(Destino, "temp_" & Guid.NewGuid.ToString & "_" & Path.GetFileName(Archivo))
                        File.Copy(Archivo, ArchivoTemp)
                        File.Move(ArchivoTemp, Path.Combine(Destino, Path.GetFileName(Archivo)))
                    End If



                    SyncLock BloqueoCopiando



                        If BloqueoCopiando.ContainsKey(Orgen) Then
                            CuentaActual += 1
                            BloqueoCopiando(Orgen) = CuentaActual & " / " & CuentaTotal & " [" & Archivo & "]"
                        End If



                    End SyncLock



                Catch ex As Exception
                    SyncLock Errores
                        If Errores.ContainsKey(Archivo) = False Then
                            Errores.Add(Archivo, ex.Message)
                        End If
                    End SyncLock
                End Try



            Next



        End Sub




        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Private Shared Function ContarCarpetaInterno(Orgen$, Destino$) As Integer


            Dim Cuenta = 0
            Dim Directorios As String()

            Try
                Directorios = Directory.GetDirectories(Orgen)
            Catch ex As Exception
                Return Cuenta
            End Try



            For Each Archivo In Directory.GetFiles(Orgen)
                Try
                    If File.Exists(Path.Combine(Destino, Path.GetFileName(Archivo))) = False Then
                        Cuenta += 1
                    End If
                Catch
                End Try
            Next
            Return Cuenta

        End Function




        <Reflection.ObfuscationAttribute(Exclude:=False, Feature:="-ctrl flow")>
        Private Shared Sub ThreadCopiador(Orgen$, Destino$)

            Try
                Dim CuentaTotal = ContarCarpetaInterno(Orgen, Destino)
                Dim CuentaActual%
                CopiarCarpetaInterno(Orgen, Destino, CuentaTotal, CuentaActual)
            Catch
                '@ Los errores ya se gestionan dentro
            Finally
                SyncLock BloqueoCopiando
                    If BloqueoCopiando.ContainsKey(Orgen) Then
                        BloqueoCopiando.Remove(Orgen)
                    End If
                End SyncLock
            End Try



        End Sub





    End Class
End Class