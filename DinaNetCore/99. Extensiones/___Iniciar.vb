Imports System.Reflection

Partial Public Module ExtensionesM





    Private Function AssemblyResolve(ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly
        If args.Name.StartsWith("Npgsql") Then Return Assembly.Load(My.Resources.Npgsql)
        If args.Name.StartsWith("MySql.Data") Then Return Assembly.Load(My.Resources.MySql_Data)
        If args.Name.StartsWith("Newtonsoft.Json") Then Return Assembly.Load(My.Resources.Newtonsoft_Json)
        If args.Name.StartsWith("Newtonsoft.Json.Bson") Then Return Assembly.Load(My.Resources.Newtonsoft_Json_Bson)
        Return Nothing
    End Function
    Public Sub Iniciar()
        If Iniciado Then Exit Sub
        Iniciado = True
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf AssemblyResolve
    End Sub

    Public Iniciado As Boolean = False




End Module
