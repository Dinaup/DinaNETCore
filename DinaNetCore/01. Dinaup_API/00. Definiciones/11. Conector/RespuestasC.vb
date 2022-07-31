Partial Public Class APID



    Public Class RespuestasC


        Public Respuestas As New Dic(Of String, String)


        Public Sub Add(_Clave$, _Valor$)
            Respuestas.Add(_Clave, _Valor)
        End Sub

        Public Sub Add(_Clave$, _Valor As Guid)
            Respuestas.Add(_Clave, _Valor.STR)
        End Sub

        Public Sub Add(_Clave$, _Valor As Decimal)
            Respuestas.Add(_Clave, _Valor.STR)
        End Sub
        Public Sub Add(_Clave$, _Valor As Integer)
            Respuestas.Add(_Clave, _Valor.STR)
        End Sub
        Public Sub Add(_Clave$, _Valor As Boolean)
            Respuestas.Add(_Clave, _Valor.STR)
        End Sub

    End Class

End Class