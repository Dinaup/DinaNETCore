
Imports System.Data
Imports MySql.Data.MySqlClient

Partial Public Class ConexionMySQLC





    Private CadenaDeConexion As String
    Private LaConexion As MySqlConnection



    Sub New(Servidor$, Puerto%, Usuario$, PassWord$, BaseDeDatos$)

        CadenaDeConexion = "server=" & Servidor & ";user=" & Usuario & ";database=" & BaseDeDatos & ";port=" & Puerto.ToString & ";password=" & PassWord & ";old guids=true;"
        LaConexion = New MySqlConnection(CadenaDeConexion)
        LaConexion.Open()

    End Sub

    Public Function ConsultarSQL(ElSQL$) As String()
        Dim Excaptruada As Exception
        SyncLock Me
            Try
                Return mConsultarSQL(ElSQL)
            Catch ex As Exception
                Excaptruada = ex
            End Try
        End SyncLock
        Throw Excaptruada
    End Function


    Private Function mConsultarSQL(ElSQL$) As String()

        Dim sql As String = ElSQL
        Dim cmd As MySqlCommand = New MySqlCommand(sql, LaConexion)
        Dim Retornar() As String

        Try


            Dim Adaptador As New MySqlDataAdapter
            Adaptador.SelectCommand = cmd
            Dim ElDataSet As DataSet = New DataSet()
            Adaptador.Fill(ElDataSet)
            Dim Tabla = ElDataSet.Tables(0)


            Dim TotalResultados = Tabla.Rows.Count
            Dim TotalColumnas = Tabla.Columns.Count



            If TotalResultados = 0 Then
                Return Nothing
            Else
                Retornar = New String(TotalResultados * TotalColumnas - 1) {}
            End If



            Dim ActualArray%
            Dim FilaActual As Integer = 0
            Dim ColumnaActual As Integer = 0
            Do While (FilaActual < TotalResultados)
                ColumnaActual = 0
                Do While (ColumnaActual < TotalColumnas)
                    Retornar(ActualArray) = Tabla.Rows(FilaActual)(ColumnaActual).ToString
                    ActualArray += 1
                    ColumnaActual += 1
                Loop
                FilaActual += 1
            Loop


        Finally
            cmd.Dispose()
        End Try

        Return Retornar
    End Function








End Class
