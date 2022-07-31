

Partial Public Class BasesDeDatoD
    Partial Public Class PostgreSQLD

        Public Class PG_ColumnaC

            Public Nombre$
            Public NombreOriginal$
            Public Tabla$
            Public TablaOriginal$
            Public ID%
            Public Tipo As PG_TipoCampoE
            Public Tama%
            Public Unico As Boolean = False
            Public AceptaNulos As Boolean

        End Class




        <Serializable>
        Public Enum PG_TipoCampoE
            Tipo_DECIMAL
            Tipo_TINYINT
            Tipo_SMALLINT
            Tipo_INTEGER
            Tipo_FLOAt
            Tipo_DOUBLE
            Tipo_NULL
            Tipo_TIMESTAMP
            Tipo_BIGINT
            Tipo_INTEGER24
            Tipo_DATE
            Tipo_TIME
            Tipo_DATETIME
            Tipo_YEAR
            Tipo_NEWDATE
            Tipo_ENUM = 247
            Tipo_SET = 248
            Tipo_TINY_BLOB = 249
            Tipo_MEDIUM_BLOB = 250
            Tipo_LONG_BLOB = 251
            Tipo_BLOB = 252
            Tipo_VARCHAR = 253
            Tipo_STRING = 254
            Tipo_NO_COMPATIBLE = 1196
        End Enum

    End Class
End Class
