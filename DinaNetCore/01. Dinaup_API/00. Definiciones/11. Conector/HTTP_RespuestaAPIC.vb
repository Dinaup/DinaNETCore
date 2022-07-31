Partial Public Class APID

    Public Class HTTP_RespuestaAPIC


        'Public ServerConnection As ServidorDinaup_ConectorC
        Public DinapSesion As APID.DinaupSesionC

        Public avisosok As New List(Of String)
        Public avisoserror As New List(Of String)
        Public Codigo%
        Public Ok As Boolean
        Public Descripcion$
        Public FuncionAPI$


        Public Obj_Estado As Newtonsoft.Json.Linq.JToken
        Public Obj_Respuesta As Newtonsoft.Json.Linq.JToken
        Public Obj_Original As Newtonsoft.Json.Linq.JToken
        Public JSONOriginal As String



        Sub New(_MotivoError$)
            Me.avisoserror.Add(_MotivoError)
        End Sub

        Sub New(_JSON$, _DinapSesion As DinaupSesionC)
            Me.DinapSesion = _DinapSesion
            'Me.Sesion = _Sesion
            Me.JSONOriginal = _JSON

            If _JSON = "" Then
                Me.Codigo = -1
                Me.Ok = False
                Me.Descripcion = "No se ha podido procesar."
                Exit Sub
            End If

            Try

                Dim ElObj As Newtonsoft.Json.Linq.JToken = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(_JSON), Newtonsoft.Json.Linq.JToken)
                Obj_Original = ElObj
                Obj_Estado = ElObj("estado")
                Obj_Respuesta = ElObj("respuesta")
                Me.Ok = Obj_Estado("estado").STR = "OK"

                If Obj_Estado("avisosok") IsNot Nothing Then
                    For Each Actual In Obj_Estado("avisosok")
                        If Actual Is Nothing Then Continue For
                        Me.avisosok.Add(Actual.ToString)
                    Next
                End If


                If Obj_Estado("avisoserror") IsNot Nothing Then
                    For Each Actual In Obj_Estado("avisoserror")
                        If Actual Is Nothing Then Continue For
                        Me.avisoserror.Add(Actual.ToString)
                    Next
                End If

                Me.Codigo = Obj_Estado("codigo").ToString.INT(0)
                Me.Descripcion = Obj_Estado("descripcion").ToString
                Me.FuncionAPI = Obj_Estado("funcionapi").ToString

            Catch

                Me.Ok = False
                Me.Descripcion = "No se ha podido procesar la solicitud"
                Me.Codigo = -1

            End Try


        End Sub

    End Class









End Class