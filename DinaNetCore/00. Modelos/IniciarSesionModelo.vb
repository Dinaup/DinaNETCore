Imports System.ComponentModel.DataAnnotations




Partial Public Class ModelosD



    Public Class IniciarSesion_ModeloC

        <Required(ErrorMessage:="Se requiere indicar en el campo de identificación el usuario o el correo electrónico.")>
        <System.ComponentModel.DataAnnotations.StringLength(50)>
        <System.ComponentModel.DataAnnotations.Display(Name:="Identificador", Description:="Correo electrónico o nombre de usuario.")>
        Public Property Identificador As String

        <Required(ErrorMessage:="Se requiere indicar la contraseña.")>
        <System.ComponentModel.DataAnnotations.StringLength(50)>
        <System.ComponentModel.DataAnnotations.Display(Name:="Contraseña")>
        Public Property Pass As String

        Public Property EmpresaID As String
        Public Property UbicacionID As String




        Public Function Motivo_RellenadoIncorrecto() As String


            If String.IsNullOrEmpty(Identificador) Then Return "Debe indicar un correo electrónico."
            If Identificador.EsEmail = False Then Return "El correo indicado no tiene formato válido."
            If Identificador.Length < 3 Then Return "El correo es demasiado corto."
            If Identificador.Length > 50 Then Return "El correo es demasiado largo."

            If String.IsNullOrEmpty(Pass) Then Return "Debe rellenar el campo de contraseña."

            If Pass.Length < 4 Then Return "La contraseña es demasiado corta."
            If Pass.Length > 40 Then Return "La contraseña es demasiado larga."
            Return ""


        End Function




        Public Respuesta_AvisosOk As List(Of String)
        Public Respuesta_AvisoError As List(Of String)
        Public Respuesta_UbicacionesASeleccionar As Dic(Of Guid, String)
        Public Respuesta_EmpresaASeleccionar As Dic(Of Guid, String)
        Public Respuesta_DetallesDeSesion As APID.DinaupAPI_UsuarioSesionC




        Sub New()
        End Sub
    End Class




End Class