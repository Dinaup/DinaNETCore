Imports System.ComponentModel.DataAnnotations




Partial Public Class ModelosD



    Public Class CambiarContrasena_ModelC



        <Required(AllowEmptyStrings:=False, ErrorMessage:="Se requiere indicar la contraseña.")>
        Public Property Usuario_Contrasena As String

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Se requiere indicar repetir contraseña.")>
        Public Property Usuario_RepetirContrasena As String



        Public Function Motivo_RellenadoIncorrecto() As String

            If String.IsNullOrEmpty(Usuario_Contrasena) Then Return "Debe rellenar el campo de contraseña."
            If String.IsNullOrEmpty(Usuario_RepetirContrasena) Then Return "Debe rellenar el campo de repetir contraseña."

            If Usuario_Contrasena.Length < 4 Then Return "La contraseña es demasiado corta."
            If Usuario_Contrasena.Length > 40 Then Return "La contraseña es demasiado larga."

            If Usuario_Contrasena <> Usuario_RepetirContrasena Then Return "contraseña y repetir contraseña deben ser iguales."


            Return ""


        End Function
        Sub New()
        End Sub
    End Class




End Class