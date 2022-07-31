Imports System.ComponentModel.DataAnnotations




Partial Public Class ModelosD


    Public Class CrearCuenta_Activar_ModelC


        Public Property CuentaTemporalID As Guid
        Public Property CodigoDeActivacion As String
        Public Property Retornar_EntidadID As String
        Public Property Retornar_Identificador As String



        Public Function Motivo_RellenadoIncorrecto() As String


            If CuentaTemporalID = Guid.Empty Then
                Return "El proceso de activación ya no es válido, por favor, inícielo nuevamente."
            End If

            If String.IsNullOrEmpty(CodigoDeActivacion) Then
                Return "Se requiere rellenar el campo código."
            End If

            Return ""


        End Function




        Sub New()

        End Sub

    End Class


    Public Class CrearCuenta_ModelC





        <Required(ErrorMessage:="Se requiere indicar en el campo de nombre su nombre o el de su empresa..")>
        <System.ComponentModel.DataAnnotations.StringLength(50)>
        <System.ComponentModel.DataAnnotations.Display(Name:="Nombre", Description:="Su nombre.")>
        Public Property Usuario_Nombre As String



        <Required(ErrorMessage:="Se requiere indicar en el campo de identificación el usuario o el correo electrónico.")>
        <System.ComponentModel.DataAnnotations.StringLength(50)>
        <System.ComponentModel.DataAnnotations.Display(Name:="Identificador", Description:="Correo electrónico o nombre de usuario.")>
        Public Property Usuario_Identificador As String

        <Required(ErrorMessage:="Se requiere indicar la contraseña.")>
        <System.ComponentModel.DataAnnotations.Display(Name:="Contraseña")>
        Public Property Usuario_Contrasena As String


        <Required(ErrorMessage:="Se requiere indicar repetir contraseña.")>
        <System.ComponentModel.DataAnnotations.Display(Name:="Contraseña")>
        Public Property Usuario_RepetirContrasena As String


        Public Property Contexto_EmpresaID As Guid
        Public Property Contexto_UbicacionID As Guid



        Public Function Motivo_RellenadoIncorrecto() As String

            If String.IsNullOrEmpty(Usuario_Nombre) Then Return "Se requiere rellenar el campo nombre."
            If Usuario_Nombre.Length < 3 Then Return "El nombre es demasiado corto."
            If Usuario_Nombre.Length > 50 Then Return "El nombre es demasiado largo."

            If String.IsNullOrEmpty(Usuario_Identificador) Then Return "Debe indicar un correo electrónico."
            If Usuario_Identificador.EsEmail = False Then Return "El correo indicado no tiene formato válido."
            If Usuario_Identificador.Length < 3 Then Return "El correo es demasiado corto."
            If Usuario_Identificador.Length > 50 Then Return "El correo es demasiado largo."

            If String.IsNullOrEmpty(Usuario_Contrasena) Then Return "Debe rellenar el campo de contraseña."
            If String.IsNullOrEmpty(Usuario_RepetirContrasena) Then Return "Debe rellenar el campo de repetir contraseña."

            If Usuario_Contrasena.Length < 4 Then Return "La contraseña es demasiado corta."
            If Usuario_Contrasena.Length > 40 Then Return "La contraseña es demasiado larga."

            If Usuario_Contrasena <> Usuario_RepetirContrasena Then Return "contraseña y repetir contraseña deben ser iguales."


            Return ""


        End Function





        Public Property EmailValidacion_Enviar As Boolean = True
        Public Property EmailValidacion_Asunto As String
        Public Property EmailValidacion_Contenido As String






        Public Property Retornar_SeHaEnviadoUnEmailCorrectamente As Boolean
        Public Property Retornar_RegistroID_CuentaTemporal As Guid
        Public Property Retornar_CodigoDeValidacion As String
        Public Property Retornar_CodigoDeValidacion_Requerido As Boolean





        Sub New()
        End Sub
    End Class




End Class