Public Interface ICookie
    Function SetValue(key As String, value As String, Optional days As Integer? = Nothing) As Task
    Function GetValue(key As String, Optional def As String = "") As Task(Of String)
End Interface

