Imports System.Data
Imports Npgsql

Partial Public Class BasesDeDatoD
    Partial Public Class PostgreSQLD






        Public Class PG_ConexionC


            Public ReconectarSiSePierdeLaConexion As Boolean = True


            Public ReadOnly Property Conectado As Boolean
                Get
                    Dim sref = PostgreSQL
                    Return sref IsNot Nothing AndAlso sref.State = ConnectionState.Open
                End Get
            End Property




            Public Pass$



            Public Notices As List(Of PostgresNotice)
            Public WithEvents PostgreSQL As Npgsql.NpgsqlConnection
            Public CacheColumnas As Dictionary(Of String, Dictionary(Of String, PG_ColumnaC)) = New Dictionary(Of String, Dictionary(Of String, PG_ColumnaC))
            Public CacheColumnasID As Dictionary(Of String, Dictionary(Of Integer, PG_ColumnaC)) = New Dictionary(Of String, Dictionary(Of Integer, PG_ColumnaC))

            Sub Dispose()
                On Error Resume Next


                If PostgreSQL IsNot Nothing Then
                    PostgreSQL.Close()
                    PostgreSQL.Dispose()
                    PostgreSQL = Nothing
                End If
            End Sub



            Private Sub PostgreSQL_Notice(sender As Object, e As NpgsqlNoticeEventArgs) Handles PostgreSQL.Notice
                If e IsNot Nothing AndAlso e.Notice IsNot Nothing Then
                    If e.Notice.Severity = "WARNING" Then Exit Sub
                    If Notices Is Nothing Then Notices = New List(Of PostgresNotice)
                    Notices.Add(e.Notice)
                End If
            End Sub



            Sub New(_pass$)
                Me.Pass = _pass
            End Sub



        End Class


    End Class
End Class
