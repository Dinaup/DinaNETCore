Partial Public Class APID

    Public Class DinaupAPI_IdentificacionRegistroC
        Implements IdentificadorRegistro


        Public Property ID As Guid Implements IdentificadorRegistro.ID
        Public Property Titulo As String Implements IdentificadorRegistro.Legible
        Public Property SeccionID As Guid Implements IdentificadorRegistro.SeccionID

        Public Seccion_Titulo$
        Public IconoID As Guid


        Public Overrides Function ToString() As String
            Return Titulo
        End Function

        Sub New(_Id As Guid, _Texto As String)
            Me.ID = _Id
            Me.Titulo = _Texto
        End Sub


        Sub New(_DatoBase As Base_DatoC)
            If _DatoBase IsNot Nothing Then
                Me.ID = _DatoBase.Base__ID
                Me.Titulo = _DatoBase.Base__Legible
                Me.IconoID = _DatoBase.Base__ImagenID
                Me.SeccionID = _DatoBase.Base__SeccionID
            End If
        End Sub


        Sub New(_DatoBase As IdentificadorRegistro)
            If _DatoBase IsNot Nothing Then
                Me.ID = _DatoBase.ID
                Me.Titulo = _DatoBase.Legible
                Me.SeccionID = _DatoBase.SeccionID
            End If
        End Sub

    End Class

End Class