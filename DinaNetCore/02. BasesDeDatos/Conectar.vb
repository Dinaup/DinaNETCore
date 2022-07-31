

Partial Public Class BasesDeDatoD


    Public Shared Function Conectar_PostgreSQL(DBip$, DBpuerto%, DBusu$, DBpass$, dbname$, Optional _AutoAsignar_Fecha As Boolean = False, Optional _AutoAsignar_FechaM As Boolean = False) As PostgreSQLD.PG_ServidorC
        Iniciar()
        Dim R As New PostgreSQLD.PG_ServidorC
        R.Conectar(DBip, DBpuerto, DBusu, DBpass, dbname, _AutoAsignar_Fecha, _AutoAsignar_FechaM)
        Return R
    End Function

    Public Shared Function Conectar_MySQL(DBip$, DBpuerto%, DBusu$, DBpass$, dbname$, Optional _AutoAsignar_Fecha As Boolean = False, Optional _AutoAsignar_FechaM As Boolean = False) As PostgreSQLD.PG_ServidorC
        Iniciar()
        Dim R As New PostgreSQLD.PG_ServidorC
        R.Conectar(DBip, DBpuerto, DBusu, DBpass, dbname, _AutoAsignar_Fecha, _AutoAsignar_FechaM)
        Return R
    End Function


End Class
