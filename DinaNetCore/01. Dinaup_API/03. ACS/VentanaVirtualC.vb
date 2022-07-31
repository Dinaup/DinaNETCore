Imports DinaNETCore.APID


Public MustInherit Class VentanaVirtualC



    Public MustOverride Sub Evento_Actualizar()






    Public ReadOnly Property EsListador As Boolean
        Get
            Return Token = TokenListador
        End Get
    End Property



    Public Token As String
    Public ReadOnly Property TokenListador$
        Get
            Return __VV__.TokenListador
        End Get
    End Property









    Public __VV__ As API_VVC


    Public Function Guardar() As API_VVC_GuardarC
        Return __VV__.Guardar()
    End Function

    Public Sub Cancelar()
        __VV__.Cancelar()
    End Sub
    Public Sub Update()
        __VV__.Update()
    End Sub


    Public Sub AgregarElementoALista()
        __VV__.AgregarElementoALista()
        Me.Evento_Actualizar()
    End Sub
    Public Sub EliminarElementoALista(Token$)
        __VV__.EliminarElementoALista(Token)
        Me.Evento_Actualizar()
    End Sub



#Region "Constructor"
#End Region


    Sub New(_HttpRespuestaForm As HTTPRespuestaAPIC_FormualarioC, _Token$)
        If __VV__ Is Nothing Then __VV__ = New API_VVC
        __VV__.Actualizar(_HttpRespuestaForm)
        Me.Token = _Token
    End Sub


    Sub New(___VV__ As API_VVC, _Token$)
        Me.__VV__ = ___VV__
        Me.Token = _Token
    End Sub





#Region "SET-Valor"
#End Region
    Public Sub SetValue(_Field$, _Value$)
        __VV__.SetValue(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Boolean(_Field$, _Value As Boolean)
        __VV__.SetValue_Boolean(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_String(_Field$, _Value As String)
        __VV__.SetValue_String(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Guid(_Field$, _Value As Guid)
        __VV__.SetValue_Guid(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Int(_Field$, _Value As Integer)
        __VV__.SetValue_Int(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Integer(_Field$, _Value As Integer)
        __VV__.SetValue_Integer(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Decimal(_Field$, _Value As Decimal)
        __VV__.SetValue_Decimal(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_DateTime(_Field$, _Value As Date)
        __VV__.SetValue_DateTime(Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Date(_Field$, _Value As Date)
        __VV__.SetValue_Date(Token, _Field, _Value)
    End Sub


#Region "GET-Valor"
#End Region



    Public Function GetText(_Field$) As String
        Return __VV__.Get_Text(Token, _Field)
    End Function


    Public Function GetValue(_Field$) As String
        Return __VV__.GetValue(Token, _Field)
    End Function
    Public Function GetValue_String(_Field$) As String
        Return __VV__.GetValue_String(Token, _Field)
    End Function
    Public Function GetValue_Guid(_Field$) As Guid
        Return __VV__.GetValue_Guid(Token, _Field)
    End Function
    Public Function GetValue_Int(_Field$) As Integer
        Return __VV__.GetValue_Int(Token, _Field)
    End Function
    Public Function GetValue_Integer(_Field$) As Integer
        Return __VV__.GetValue_Integer(Token, _Field)
    End Function
    Public Function GetValue_Boolean(_Field$) As Boolean
        Return __VV__.GetValue_Boolean(Token, _Field)
    End Function
    Public Function GetValue_Decimal(_Field$) As Decimal
        Return __VV__.GetValue_Decimal(Token, _Field)
    End Function
    Public Function GetValue_DateTime(_Field$) As Date?
        Return __VV__.GetValue_DateTime_UTC(Token, _Field)
    End Function
    Public Function GetValue_Date(_Field$) As Date?
        Return __VV__.GetValue_Date_Local(Token, _Field)
    End Function


End Class




Public Class API_VVC_GuardarC


    Public Ok As Boolean
    Public Descripcion$
    Public Respuesta As HTTPRespuestaAPIC_Formualario_GuardarC

    Sub New()

    End Sub

End Class



Public Class API_VVC



    Public ReadOnly Property TokenListador$
        Get
            Return Formulario_Listador.Token
        End Get
    End Property




    Public mHTTPForm As HTTPRespuestaAPIC_FormualarioC
    Public Property HTTPForm As HTTPRespuestaAPIC_FormualarioC
        Get
            Return mHTTPForm
        End Get
        Set(value As HTTPRespuestaAPIC_FormualarioC)
            mHTTPForm = value
            Reindexar()
        End Set
    End Property




#Region " ----  Formularios  ----  "
#End Region



    Sub Reindexar()



        Dim TodosLosFormularios As New List(Of DinaupAPI_Formulario_RegistroC)
        Dim FormulariosLista As New List(Of DinaupAPI_Formulario_RegistroC)


        If HTTPForm IsNot Nothing AndAlso
                HTTPForm.R_Formulario IsNot Nothing AndAlso
                HTTPForm.R_Formulario.Ventana IsNot Nothing Then

            TodosLosFormularios.AddRange(HTTPForm.R_Formulario.Ventana)
            FormulariosLista.AddRange(HTTPForm.R_Formulario.Ventana)
            FormulariosLista.RemoveAt(0)

            Me.Formulario_Listador = TodosLosFormularios(0)
            Me.Formularios_Todos = TodosLosFormularios
            Me.Formularios_Lista = FormulariosLista


        End If

    End Sub


    Public Formulario_Listador As DinaupAPI_Formulario_RegistroC
    Public Formularios_Lista As List(Of DinaupAPI_Formulario_RegistroC)
    Public Formularios_Todos As List(Of DinaupAPI_Formulario_RegistroC)


    Sub Actualizar(_Form As HTTPRespuestaAPIC_FormualarioC)

        HTTPForm = _Form


    End Sub





#Region " ----  Update  ----  "
#End Region




    Public Sub Cancelar()




        'API_FormulariosD .Formulario_Cancelar()


    End Sub

    Public Sub Update()


        Dim DatosModificados = RecibirParametrosURL(True)
        If DatosModificados <> "" Then

            Dim R = HTTPForm.DinapSesion.ConexionServidor.Funcion_Formulario_Ping(HTTPForm.DinapSesion, Me.HTTPForm.R_Formulario.Token, DatosModificados)
            Actualizar(R)

        End If


    End Sub


    Public Function Guardar() As API_VVC_GuardarC


        Dim DatosModificados = RecibirParametrosURL(True)
        Dim R = HTTPForm.DinapSesion.ConexionServidor.Funcion_Formulario_Guardar(HTTPForm.DinapSesion, Me.HTTPForm.R_Formulario.Token, DatosModificados)
        Dim Estado = R.RespuestaGenerica.Descripcion
        Dim codigo = R.RespuestaGenerica.Codigo
        Dim Retornar As New API_VVC_GuardarC
        Retornar.Respuesta = R
        Retornar.Ok = True
        If Estado <> "" AndAlso codigo <> 0 Then
            Retornar.Descripcion = Estado
            Retornar.Ok = False
        End If
        Return Retornar

    End Function


    Public Sub AgregarElementoALista()
        Dim DatosModificados = RecibirParametrosURL(True)
        Dim R = HTTPForm.DinapSesion.ConexionServidor.Funcion_Formulario_NuevoElemento(HTTPForm.DinapSesion, Me.HTTPForm.R_Formulario.Token, DatosModificados)
        Actualizar(R)
    End Sub


    Public Sub EliminarElementoALista(Token$)
        Dim DatosModificados = RecibirParametrosURL(True)
        Dim R = HTTPForm.DinapSesion.ConexionServidor.Funcion_Formulario_EliminarElemento(HTTPForm.DinapSesion, Me.HTTPForm.R_Formulario.Token, DatosModificados, Token)
        Actualizar(R)
    End Sub




#Region " ----  Valores  ----  "
#End Region

    Public Function RecibirParametrosURL(Optional SoloModificados As Boolean = False) As String
        Dim R As New List(Of String)
        Dim Formularios_Referencia = Formularios_Todos
        If Formularios_Referencia IsNot Nothing Then
            Dim I As Integer = 0
            For Each FormActual In Formularios_Todos
                For Each PestanaActual In FormActual.Pestanas
                    For Each ControlActual In PestanaActual.Controles
                        Dim ValorModificado = ""



                        If BufferDeCambios.TryGetValue(FormActual.Token.ToString & "|" & ControlActual.ID, ValorModificado) Then
                            R.Add(ControlActual.Token & "=" & ValorModificado.AdaptarURL)
                        Else
                            If SoloModificados Then Continue For
                            R.Add(ControlActual.Token & "=" & ControlActual.ValorActual.AdaptarURL)
                        End If

                    Next

                Next
                I += 1
            Next
        End If


        Return String.Join("&", R)
    End Function



    Public BufferDeCambios As New Dic(Of String, String)


    Private Function mBuscarCampo(_Token$, _Field$) As DinaupAPI_Formulario_ControlC


        Dim Formularios_Referencia = Formularios_Todos

        Dim Retornar As DinaupAPI_Formulario_RegistroC = Nothing

        For Each Actual In Formularios_Referencia
            If Actual.Token = _Token Then
                Retornar = Actual
                Exit For
            End If
        Next


        If Retornar Is Nothing Then Return Nothing
        If Retornar.Pestanas Is Nothing Then Return Nothing
        For Each PestanaActual In Retornar.Pestanas
            If PestanaActual.Controles.TieneDatos Then
                For Each ControlActual In PestanaActual.Controles
                    If ControlActual.ID = _Field Then
                        Return ControlActual
                    End If
                Next
            End If
        Next
        Return Nothing
    End Function


    Private Sub mSetValue(_Token$, _Field$, _Value$)
        Dim mKey = _Token & "|" & _Field
        Dim ControlX = mBuscarCampo(_Token, _Field)
        If ControlX Is Nothing Then Exit Sub
        If ControlX.ValorActual = _Value Then
            BufferDeCambios.Remove(mKey)
        Else
            'If ControlX.RolCampo = RolCampoE.Pass Then
            '    BufferDeCambios.HacerMagia(mKey) = "sha1:" & RecibirSHA1(_Value)
            'Else
            BufferDeCambios.HacerMagia(mKey) = _Value
            'End If
        End If
    End Sub

    Private Function mGetValue(_Token$, _Field$) As String
        Dim mKey = _Token & "|" & _Field
        Dim ControlX = mBuscarCampo(_Token, _Field)
        Dim R As String = ""
        If BufferDeCambios.TryGetValue(mKey, R) = False Then
            Return ControlX.ValorActual
        Else
            Return R
        End If
    End Function

    Public Function Get_Text(_Token$, _Field$) As String
        Dim mKey = _Token & "|" & _Field
        Dim ControlX = mBuscarCampo(_Token, _Field)
        Return ControlX.valor_txt
    End Function



#Region " ----  Set ---- "
#End Region

    Public Sub SetValue(_Token$, _Field$, _Value$)
        mSetValue(_Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Boolean(_Token$, _Field$, _Value As Boolean)
        mSetValue(_Token, _Field, _Value.STR)
    End Sub
    Public Sub SetValue_String(_Token$, _Field$, _Value As String)
        mSetValue(_Token, _Field, _Value)
    End Sub
    Public Sub SetValue_Guid(_Token$, _Field$, _Value As Guid)
        mSetValue(_Token, _Field, _Value.STR)
    End Sub
    Public Sub SetValue_Int(_Token$, _Field$, _Value As Integer)
        mSetValue(_Token, _Field, _Value.STR)
    End Sub
    Public Sub SetValue_Integer(_Token$, _Field$, _Value As Integer)
        mSetValue(_Token, _Field, _Value.STR)
    End Sub
    Public Sub SetValue_Decimal(_Token$, _Field$, _Value As Decimal)
        mSetValue(_Token, _Field, _Value.AdaptarMySQL)
    End Sub
    Public Sub SetValue_DateTime(_Token$, _Field$, _Value As Date)
        If _Value.Kind <> DateTimeKind.Utc Then Throw New Exception("Se esperaba fecha UTC (Revise DateTime.Kind). Código de error (E-5395)")
        mSetValue(_Token, _Field, _Value.AdaptarMySQL)
    End Sub
    Public Sub SetValue_Date(_Token$, _Field$, _Value As Date)
        If _Value.Kind <> DateTimeKind.Local Then Throw New Exception("Se esperaba fecha Local (Revise DateTime.Kind). Código de error (E-5392)")
        mSetValue(_Token, _Field, _Value.AdaptarMySQL)
    End Sub



#Region " ----  Get ---- "
#End Region

    Public Function GetValue(_Token$, _Field$) As String
        Dim mValor = mGetValue(_Token, _Field)
        Return mValor
    End Function
    Public Function GetValue_String(_Token$, _Field$) As String
        Dim mValor = mGetValue(_Token, _Field)
        Return mValor
    End Function
    Public Function GetValue_Guid(_Token$, _Field$) As Guid
        Dim mValor = mGetValue(_Token, _Field)
        Return mValor.ToGuid
    End Function
    Public Function GetValue_Int(_Token$, _Field$) As Integer
        Dim mValor = mGetValue(_Token, _Field)
        Return mValor.INT(0)
    End Function

    Public Function GetValue_Integer(_Token$, _Field$) As Integer
        Dim mValor = mGetValue(_Token, _Field)
        Return mValor.INT(0)
    End Function

    Public Function GetValue_Boolean(_Token$, _Field$) As Boolean
        Dim mValor = mGetValue(_Token, _Field)
        Return mValor = "1"
    End Function


    Public Function GetValue_Decimal(_Token$, _Field$) As Decimal
        Dim mValor = mGetValue(_Token, _Field)
        Return mValor.DEC(0)
    End Function

    Public Function GetValue_DateTime_UTC(_Token$, _Field$) As Date?
        Dim mValor = mGetValue(_Token, _Field).ToDateDesdeMySQLNulable_UTC
        If mValor Is Nothing Then Return Nothing
        If mValor.Value = Date.MinValue Then Return Nothing
        Return mValor
    End Function

    Public Function GetValue_Date_Local(_Token$, _Field$) As Date?
        Dim mValor = mGetValue(_Token, _Field).ToDateDesdeMySQLNulable_Local
        If mValor Is Nothing Then Return Nothing
        If mValor.Value = Date.MinValue Then Return Nothing
        Return mValor
    End Function







#Region "Constructor"
#End Region

    Sub New()
    End Sub

End Class
