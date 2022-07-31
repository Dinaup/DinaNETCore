Partial Public Module ExtensionesM

 
    Public Sub EstablecerCultureAlThreadActual()
        EstablecerCultureAlThread(System.Threading.Thread.CurrentThread)
    End Sub

    Public Sub EstablecerCultureAlThread(th As Threading.Thread)

        Dim nfi = New Globalization.CultureInfo("es-ES", True)

        Dim nfix = nfi.NumberFormat
        nfix.CurrencyDecimalSeparator = "."
        nfix.NumberDecimalSeparator = "."
        nfix.PercentDecimalSeparator = "."
        nfix.NumberGroupSeparator = ","
        nfix.CurrencyGroupSeparator = ","
        nfix.PercentGroupSeparator = ","

        Dim CL As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
        CL.NumberFormat = nfix
        th.CurrentUICulture = CL
        th.CurrentCulture = CL

    End Sub

End Module
