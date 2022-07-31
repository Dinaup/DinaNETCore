Imports System.ComponentModel.DataAnnotations




Partial Public Class ModelosD



    Public Class RecuperarContrasena_ModeloC

        <Required(ErrorMessage:="Se requiere indicar en el campo de identificación el usuario o el correo electrónico.")>
        <System.ComponentModel.DataAnnotations.StringLength(50)>
        <System.ComponentModel.DataAnnotations.Display(Name:="Identificador", Description:="Correo electrónico o nombre de usuario.")>
        Public Property Identificador As String
        Public Property Codigo As String
        Public Property EnviarEmail As Boolean = True


        Public Property Contexto_EmpresaID As Guid
        Public Property Contexto_UbicacionID As Guid


        Public Property Return_CodigoGenerado As Boolean
        Public Property Return_EmailEnviado As Boolean = False
        Public Property Return_Codigo As String



        Public Property EnviarEmail_Asunto As String = "Contraseña temporal"
        Public Property EnviarEmail_Contenido As String = "Puede inciciar sesión utilizando la siguiente contraseña temporal: {codigo} <br> Recuerde que esta contraseña es válida durante 20 minutos."






        Public Function Motivo_RellenadoIncorrecto() As String


            If String.IsNullOrEmpty(Identificador) Then Return "Debe indicar un correo electrónico."
            If Identificador.EsEmail = False Then Return "El correo indicado no tiene formato válido."
            If Identificador.Length < 3 Then Return "El correo es demasiado corto."
            If Identificador.Length > 50 Then Return "El correo es demasiado largo."

            Return ""


        End Function




        Public Respuesta_AvisosOk As List(Of String)
        Public Respuesta_AvisoError As List(Of String)




        Sub New()
        End Sub

    End Class




End Class