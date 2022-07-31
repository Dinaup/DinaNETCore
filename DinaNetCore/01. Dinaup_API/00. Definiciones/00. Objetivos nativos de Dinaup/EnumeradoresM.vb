Imports System.ComponentModel

Public Enum TipoCamposSQLServerE
    <Description("Indefinido")> Indefinido = 0
    <Description("Sí/No")> Bool = 1
    <Description("Entero")> Entero = 2
    <Description("Decimal")> Doble = 3
    <Description("Texto")> Texto = 5
    <Description("Fecha Y Hora")> FechaYHora = 6
    <Description("Fecha")> Fecha = 7
    <Description("Hora")> Hora = 8
    <Description("Relación")> Guid = 9
    <Description("Imagen")> ImgBase64Obsoleto = 10
    <Description("Especial")> Boton = 11
End Enum






Public Enum RolCampoE

    <Description("Genérico")> Ordinario = 0
    <Description("Web")> Web = 1
    <Description("Teléfono")> Telefono = 2
    <Description("Skype")> Skype = 3
    <Description("Email")> CorreoElectronico = 4
    <Description("Moneda")> Moneda = 5
    <Description("Porcentaje")> Porcentaje = 6
    <Description("Icono")> Icono = 7
    <Description("Archivo")> Archivo = 8
    <Description("Multilinea (512)")> TextoLargo = 9
    <Description("Contraseña")> Pass = 10
    <Description("Precisión Avanzada")> Preciso = 11
    <Description("Referencia Pistola")> ReferenciaPistola = 12
    <Description("Texto 60")> Texto60 = 13
    <Description("Horas")> Horas = 15
    <Description("Texto 8.000")> Varchar8000 = 16
    <Description("Imagen")> Imagen = 17
    <Description("Icono Color")> Icono_Color = 19
    <Description("Con segundos")> ConSegundos = 21
    <Description("Sin segundos")> SinSegundos = 22
    <Description("Color")> ColorHex = 29
    <Description("Año")> Año = 25
    <Description("Minutos")> Minutos = 14
    <Description("Segundos")> Segundos = 33
    <Description("Milisegundos")> Milisegundos = 34
    <Description("Bytes")> Byesx = 26
    <Description("Megabytes")> Megabytesx = 27
    <Description("Gigabytes")> Gigabytex = 28
    <Description("MEDIUMTEXT 16MB")> Texto_MEDIUMTEXT = 47 ' @@@ creo que es para tabla de historicos, en principio no usar, esta el varchar 8k
    <Description("Multilinea (2048)")> TextoLargo2048 = 50
    <Description("Multilinea (4096)")> TextoLargo4096 = 51
    <Description("Código Subcuenta")> CodigoSubcuenta = 54
    <Description("Código Cuenta contable")> CodigoCuenta = 59
    <Description("HTML")> HTML = 65
    <Description("HTML (Editor)")> HTML_Editor = 140
    <Description("Fecha Principal")> FechaPrincipal = 69
    <Description("Código de cuenta (CCC)")> CodigoDeCuentaBancaria = 76
    <Description("NIF/CIF")> NifCif = 79
    <Description("Territorialidad - Pais Texto")> Territorialidad_Pais_Texto = 83
    <Description("Territorialidad - Provincia Texto")> Territorialidad_Provincia_Texto = 84
    <Description("Territorialidad - Cod. País")> Territorialidad_CodigoPais = 85
    <Description("Territorialidad - Cod. Provincia")> Territorialidad_CodProvincia = 86
    <Description("Tipo NIF/CIF")> TipoNIFCif = 87
    <Description("Clave operación intracomunitaria")> ClaveOperacionIntraComunitaria = 89
    <Description("KeyWord")> keyword = 100
    <Description("Regular manual Fecha Principal")> FechaPrincipalRegulador = 103
    <Description("Código de cuenta IBAN")> CodigoDeCuentaIBAN = 119
    <Description("CCC SeguridadSocial")> CCC_SeguridadSocial = 122



    '! Campo de texto que almacena una URL
    <Description("Asistente URL")> AsistenteURL = 149

    '@ [[[[[  ESPECIALES ]]]]]
    <Description("Datos serializados")> DatosSerializados = 77
    <Description("Condiciones CM")> CondicionesCM = 30



    '@ [[[[[  OBSOLETOS   ]]]]]

    <Description("obsoleto")> _obsoleto1 = 18
    <Description("obsoleto")> _obsoleto2 = 20








    <Description("Prioridad")> Enum_Prioridad = 49
    <Description("Tipo parámetro de nómina")> Enum_TipoParametroNominaE = 55
    <Description("Tipo JSON")> Enum_AreaAutomaticaFuncionesE = 52
    '@ [[[[[  ENUM   ]]]]]
    <Description("Enum - S.C.D Valor")> Enum_SynCheck_ValorDiagnostico = 35
    <Description("Enum - S.C.D Carencia")> Enum_SynCheck_CarenciaDiagnostico = 36
    <Description("Enum - S.C.D Registro")> Enum_SynCheck_RegistroDiagnostico = 37
    <Description("Enum - Chat Tipo Mensaje")> Enum_TipoMensajeChat = 38
    <Description("Enum - R.A Acción papelera")> Enum_AccionEliminarRestaurar = 39
    <Description("Enum - Evento Agenda")> Enum_TipoEventoNotificacion = 31
    <Description("Enum - Mes")> Enum_Mes = 24
    <Description("Enum - Día de la semana")> Enum_DiaDeSemana = 23
    <Description("Enum - Criterio evaluación Agenda")> Enum_ModoEvaluacionAgenda = 32
    <Description("Enum - Tipo de realización de turno")> Enum_TipoRealizacionDeTurnoE = 46
    <Description("Enum - A. Control de presencia")> Enum_ControlPresenciaE = 40
    <Description("Enum - A. Log acceso")> Enum_LogAccesoE = 41
    <Description("Enum - R.A Principal")> Enum_RAActionE = 42
    <Description("Enum - R.A Archivo")> Enum_RAArchivoEventoE = 43
    <Description("Enum - Notificación Periodo")> CFGIntervalosPeriodosNotis = 44
    <Description("Enum - Genero")> Enum_GeneroE = 45
    <Description("Enum - Tramitación")> Enum_TransaccionTramitacionE = 48
    <Description("Enum - Naturaleza de cuenta")> Enum_NaturalezaCuentaContableE = 53
    <Description("Enum - Asiento")> Enum_TipoDeAsientoE = 56
    <Description("Enum - Criterio Bloqueo REST")> Enum_CriterioBloqueoRestE = 57
    <Description("Enum - Nivel Cuenta Contable")> Enum_NivelCuentaContable = 58
    <Description("Enum - Operación balance")> Enum_Balance_ElementoOperacionE = 60
    <Description("Enum - Llamada Entrante/Saliente")> Enum_LlamadaEntranteSalienteE = 61
    <Description("Enum - Correo Entrante/Saliente")> Enum_CorreoEntranteSalienteE = 62
    <Description("Enum - Contacto Comercial")> Enum_ComercialSoporteE = 63
    <Description("Enum - Estado de trámite")> Enum_EstadoTramiteE = 64
    <Description("Enum - Tipo de crédito")> Enum_TipoDeDeudaE = 66
    <Description("Enum - Tipo de pago de crédito")> Enum_TipoPagoDecreditoE = 67
    <Description("Enum - Algoritmo cálculo de intereses")> Enum_AlgoritmoCalculoInteresE = 68
    <Description("Enum - Tipo de documento")> Enum_Balance_TipoDeDocumentoE = 70
    <Description("Enum - Formato")> Enum_Formato = 71
    <Description("Enum - Estado de ejecucción (Pausable)")> Enum_EstadoEjecuccion_PausableE = 72
    <Description("Enum - Estado de ejecucción (No Pausable)")> Enum_EstadoEjecuccionE = 73
    <Description("Enum - Estado Acción programada")> Enum_EstadoAccionProgramadaE = 74
    <Description("Enum - Tipo de turno")> Enum_TipoDeTurnoE = 75
    <Description("Enum - Intervalo Contable")> Enum_IntervaloContableE = 78
    <Description("Enum - Intervalo Contable (Periodos)")> Enum_IntervaloContableE_PeriodoE = 80
    <Description("Enum - Territorialidad")> Enum_Territorialidad = 81
    <Description("Enum - Tipo Movimiento")> Enum_TipoMovimientoE = 88
    <Description("Enum - Tipo de Devengo")> Enum_TipoDeDevengo = 90
    <Description("Enum - Inclusión/Exclusión")> Enum_InclusionExclusion = 91
    <Description("Enum - TipoIdentificacion Fiscal")> Enum_TipoIdentificacionFiscalE = 92
    <Description("Enum - Tipo servicio de correo")> Enum_TipoEmailE = 93
    <Description("Enum - Régimen de IVA")> Enum_RegimenDeIvaE = 94
    <Description("Enum - Tipo Actividad")> Enum_TipoActividadE = 95
    <Description("Enum - Regime IRPF")> Enum_RegimenIRPFE = 96
    <Description("Enum - Inmovilizado_NaturalezaE")> Enum_Inmovilizado_NaturalezaE = 97
    <Description("Enum - Inmovilizado_EstadoAdquisicionE")> Enum_Inmovilizado_EstadoAdquisicionE = 98
    <Description("Enum - Enum_provisionesSuplidoE")> Enum_AprovisionamientoSuplidoE = 99
    <Description("Enum - obsoletoNomina_IT_TipoDeEnfermedadE")> Enum_Nomina_IT_TipoDeEnfermedadE = 101
    <Description("Enum - obsoletoNomina_IT_TipoDeAbonoE")> Enum_Nomina_IT_TipoDeAbonoE = 102
    <Description("Enum - TipoBackUpE")> Enum_TipoBackUpE = 104
    <Description("Enum - TipoDeConvenio")> Enum_TipoDeConvenioE = 106
    <Description("Enum - TipoUnidadFamiliar")> Enum_TipoMiembroUnidadFamiliarE = 107
    <Description("Enum - TipoDiscapacidad")> Enum_TipoDiscapacidadE = 108
    <Description("Enum - TipoDeRetencionE")> Enum_TipoDeRetencionE = 109
    <Description("Enum - TipoLimiteCotizacionE")> Enum_TipoLimiteCotizacionE = 110
    <Description("Enum - CuandoCobrarNominaE")> Enum_CuandoCobrarNominaE = 111
    <Description("Enum - Enum_AportacionRetencionE")> Enum_AportacionRetencionE = 112
    <Description("Enum - Enum_TipoDePeriodoDetrabajosE")> Enum_TipoDePeriodoDetrabajosE = 113
    <Description("Enum - Enum_IT_TipoAbonoE")> Enum_IT_TipoAbonoE = 114
    <Description("Enum - Enum_IT_TipoE")> Enum_IT_TipoE = 115
    <Description("Enum - Enum_IT_ModoCalculoPorcentajeE")> Enum_IT_ModoCalculoPorcentajeE = 116
    <Description("Enum - Enum_GestionNumeros0E")> Enum_GestionNumeros0E = 117
    <Description("Enum - Enum_ModoCalculoDeDevengoE")> Enum_ModoCalculoDeDevengoE = 118
    <Description("Enum - Enum_DiaNoLaboralTipo")> Enum_DiaNoLaboralTipoE = 120
    <Description("Enum - Enum_ModoCalculoAntiguedadE")> Enum_ModoCalculoAntiguedadE = 121
    <Description("Enum - ModoCalculoDeHorasContrato")> Enum_ModoCalculoDeHorasContratoE = 123
    <Description("Enum - ModoCotizacionExcesoE")> Enum_ModoCotizacionExcesoE = 124
    <Description("Enum - TipoDeAusenciaLaboralE")> Enum_TipoDeAusenciaLaboralE = 125
    <Description("Enum - NaturalezaContratoEmpleadoE")> Enum_NaturalezaContratoEmpleadoE = 126


    <Description("Enum - TipoDeRetribucionE")> Enum_TipoDeRetribucionE = 127
    <Description("Enum - Cotización ausencia")> Enum_CotizacionAusenciaE = 128
    <Description("Enum - Tipo de Nómina")> Enum_TipoDeNominaE = 129
    <Description("Enum - Tipo de ausencia laboral")> Enum_TipoDescuentoAusenciaLaboralE = 130
    <Description("Enum - Modo seleccion de condiciones CCME")> Enum_ModoSeleccionCondicionControlCME = 131
    <Description("Enum - Modo computación ausencias")> Enum_ModoComputacionAusenciasE = 132
    <Description("Enum - Modo agrupación ausencias en nómina")> Enum_ModoAgrupacionAusenciaEnNominaE = 133
    <Description("Enum - Modo agrupación atrasos en nómina")> Enum_ModoAgrupacionAtrasosEnNominaE = 134
    <Description("Enum - Modo agrupación finiquitos en nómina")> Enum_ModoAgrupacionFiniquitosEnNominaE = 135

    <Description("Enum - Calculadora Laboral Tipo de Dato")> Enum_CalculadoraLaboral20_TipoDeDatoE = 136
    <Description("Enum - Calculadora Laboral Base de Datos")> Enum_CalculadoraLaboral20_BaseDeDatosE = 137
    <Description("Enum - Género con Mixto")> Enum_GeneroIncluyeMixtoE = 138
    <Description("Enum - Tipos de periodos")> Enum_TipoPeriodoE = 139
    <Description("Enum - Estado y Proceso")> Enum_EstadoYProcesoE = 141
    <Description("Enum - Asistencia a Reunion")> Enum_AsistenciaReunionE = 142
    <Description("Enum - Respuesta aprobación de solicitud")> Enum_RespuestaDeAprobacionDeSolicitudE = 143
    <Description("Enum - Estado punto a tratar")> Enum_EstadoPuntoATratarReunionE = 145






    <Description("Enum - Estado transporte")> Enum_EstadoTransporteE = 146
    <Description("Enum - Estado division de recurso de la empresa")> Enum_EstadoDeDivisionDeRecursoDeLaEmpresaE = 147
    <Description("Enum - Disponibilidad de recurso")> Enum_DisponibilidadRecursoDeLaEmpresaE = 148
    <Description("Cronometro")> Cronometro = 150

    <Description("Enum - AccesoArchivoE")> Enum_AccesoArchivoE = 151
    <Description("Enum - EstilosTextoE")> Enum_EstilosTextoE = 152

    <Description("Enum - NaturalezaDeProceso")> Enum_NaturalezaDeProcesoE = 153
    <Description("Enum - ModoEjecuccionProceso")> Enum_ModoEjecuccionProcesoE = 154
    <Description("Enum - TipoDeProceso")> Enum_TipoDeProcesoE = 155
    <Description("Enum - TipoReporteFalloSugerenciaPregunta")> Enum_TipoReporteFalloSugerenciaPreguntaE = 156



    '<Description("API - ID (Externa)")> IDExterna = 157
    '<Description("API - Versión (Externa)")> VersionExterna = 158
    <Description("Enum - Rol Equipo Dinaup")> Enum_RolEquipoDinaup = 157
    <Description("Enum - Estado Version")> Enum_EsadoVersion = 158
End Enum