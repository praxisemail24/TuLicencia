using System.Data.SqlClient;
using SmartLicencia.Data;
using SmartLicencia.Models;
using SmartLicencia.Entity;
using DocumentFormat.OpenXml.Office.CustomUI;
using System.Collections.Generic;
using System.Globalization;

namespace SmartLicencia.Repository
{
    public class ReporteCasoRepository : AbstractConnection
    {
        public ReporteCasoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<ReporteTramiteCaso> ReporteGeneral(int tipoTramite, int? estadoTramite, int? estadoProceso,
            string? nombre, string? apePaterno, string? apeMaterno, string? correo)
        {
            try
            {

            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<ReporteTramiteCaso>>("__sp_EstadoTramiteReporte", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_tramite", tipoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado_tramite", estadoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado_proceso", estadoProceso));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@nombre_cli", nombre));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@ape_paterno_cli", apePaterno));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@ape_materno_cli", apeMaterno));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@correo_cli", correo));
                var reader = cmd.ExecuteReader();
                var lista = new List<ReporteTramiteCaso>();
                while (reader.Read())
                {
                    var row = new ReporteTramiteCaso();
                    row.Id = Convert.ToInt64(reader.GetValue(reader.GetOrdinal("id")));
                    row.TipoTramite = reader.GetString(reader.GetOrdinal("tipo"));
                    row.NombreCliente = reader.IsDBNull(reader.GetOrdinal("nombre_cliente")) ? "" : reader.GetString(reader.GetOrdinal("nombre_cliente"));
                    row.Correo = reader.IsDBNull(reader.GetOrdinal("correo")) ? "" : reader.GetString(reader.GetOrdinal("correo"));
                    row.Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? "" : reader.GetString(reader.GetOrdinal("telefono"));
                    row.PagoCodigo = reader.IsDBNull(reader.GetOrdinal("pago_codigo")) ? "" : reader.GetString(reader.GetOrdinal("pago_codigo"));
                    row.Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? "" : reader.GetString(reader.GetOrdinal("estado"));
                    row.estadoProceso = reader.IsDBNull(reader.GetOrdinal("estado_proceso")) ? 0 : reader.GetInt32(reader.GetOrdinal("estado_proceso"));
                    row.PagoCodigo = reader.IsDBNull(reader.GetOrdinal("pago_codigo")) ? "" : reader.GetString(reader.GetOrdinal("pago_codigo"));
                    row.PagoFecha = reader.IsDBNull(reader.GetOrdinal("pago_fecha")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("pago_fecha"));
                    row.PagoId = reader.IsDBNull(reader.GetOrdinal("pg_id")) ? null : reader.GetInt32(reader.GetOrdinal("pg_id"));
                    row.ClienteId = reader.IsDBNull(reader.GetOrdinal("cl_id")) ? null : reader.GetInt32(reader.GetOrdinal("cl_id"));
                    lista.Add(row);
                }
                reader.Dispose();

                return lista;
            });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                return Enumerable.Empty<ReporteTramiteCaso>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<ReporteTramiteCaso>();
            }
        }

        private decimal GetCantidadTramite(int tipoTramite, int estadoTramite)
        {
            return ExecProcedure<SqlConnection, SqlCommand, decimal>("__sp_TotalTramiteReporte", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_tramite", tipoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado_tramite", estadoTramite));
                object scalar = cmd.ExecuteScalar();

                if (scalar == null)
                    return 0m;

                return Convert.ToDecimal(scalar);
            });
        }

        private decimal GetCantidadTramiteSinIniciar()
        {
            return ExecProcedure<SqlConnection, SqlCommand, decimal>("__sp_TramitesSinIniciar", (cmd) =>
            {
                object scalar = cmd.ExecuteScalar();

                if (scalar == null)
                    return 0m;

                return Convert.ToDecimal(scalar);
            });
        }

        public ReporteChartPeriodo ChartTotalesPorPeriodo(int tipoTramite)
        {
            var datasets = new List<DataSetChart>();
            var totalNewCases = 0m;
            var totalPreviewedCases = 0m;
            var totalProcessedCases = 0m;
            var totalClosedCases = 0m;

            if (tipoTramite == 0)
            {
                totalNewCases += GetCantidadTramite(1, 0);
                totalPreviewedCases += GetCantidadTramite(1, 1);
                totalProcessedCases += GetCantidadTramite(1, 2);
                totalClosedCases += GetCantidadTramite(1, 3);

                totalNewCases += GetCantidadTramite(2, 0);
                totalPreviewedCases += GetCantidadTramite(2, 1);
                totalProcessedCases += GetCantidadTramite(2, 2);
                totalClosedCases += GetCantidadTramite(2, 3);

                totalNewCases += GetCantidadTramite(3, 0);
                totalPreviewedCases += GetCantidadTramite(3, 1);
                totalProcessedCases += GetCantidadTramite(3, 2);
                totalClosedCases += GetCantidadTramite(3, 3);

                totalNewCases += GetCantidadTramite(4, 0);
                totalPreviewedCases += GetCantidadTramite(4, 1);
                totalProcessedCases += GetCantidadTramite(4, 2);
                totalClosedCases += GetCantidadTramite(4, 3);

                totalNewCases += GetCantidadTramite(5, 0);
                totalPreviewedCases += GetCantidadTramite(5, 1);
                totalProcessedCases += GetCantidadTramite(5, 2);
                totalClosedCases += GetCantidadTramite(5, 3);
            } else
            {
                totalNewCases = GetCantidadTramite(tipoTramite, 0);
                totalPreviewedCases = GetCantidadTramite(tipoTramite, 1);
                totalProcessedCases = GetCantidadTramite(tipoTramite, 2);
                totalClosedCases = GetCantidadTramite(tipoTramite, 3);
            }

            datasets.Add(new DataSetChart { Label = "NEW CASES", Data = new decimal[] { totalNewCases }, BackgroundColor = "#CFC2F0", BorderColor = "#7E5CD7", });
            datasets.Add(new DataSetChart { Label = "REVIEWED CASES", Data = new decimal[] { totalPreviewedCases }, BackgroundColor = "#C0E2F3", BorderColor = "#58B4E0", });
            datasets.Add(new DataSetChart { Label = "PROCESSED CASES", Data = new decimal[] { totalProcessedCases }, BackgroundColor = "#F6E0B4", BorderColor = "#ECBA5E", });
            datasets.Add(new DataSetChart { Label = "CLOSED CASES", Data = new decimal[] { totalClosedCases }, BackgroundColor = "#C2E5D7", BorderColor = "#5DBC97", });

            var reporte = new ReporteChartPeriodo();
            reporte.SinIniciar = GetCantidadTramiteSinIniciar();
            reporte.ChartBar.Data.Labels = new string[] { "CANTIDAD" };
            reporte.ChartBar.Data.Datasets = datasets;
            reporte.Summary.Add(new { Index = 0, Key = "NEW CASES", Value = totalNewCases, Color = "#7E5CD7", });
            reporte.Summary.Add(new { Index = 1, Key = "REVIEWED CASES", Value = totalPreviewedCases, Color = "#58B4E0", });
            reporte.Summary.Add(new { Index = 2, Key = "PROCESSED CASES", Value = totalProcessedCases, Color = "#ECBA5E", });
            reporte.Summary.Add(new { Index = 3, Key = "CLOSED CASES", Value = totalClosedCases, Color = "#5DBC97", });
            reporte.Summary.Add(new { Index = 4, Key = "TOTAL", Value = (totalNewCases + totalPreviewedCases + totalProcessedCases + totalClosedCases), Color = "#303335", });
            return reporte;
        }

        private string GetTituloReporte(int? tipoReporte, int? mes, int? anio, DateTime? fechaInicio, DateTime? fechaTermino)
        {
            string title = string.Empty;

            switch (tipoReporte)
            {
                case 0:
                    title = string.Format("{0:dd/MM/yyyy}", fechaInicio);
                    break;
                case 1:
                    var listaMeses = new string[] { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };
                    title = string.Format("{0} DEL {1}", listaMeses.GetValue((mes ?? 1) - 1 ), anio);
                    break;
                case 2:
                    title = string.Format("ÚLTIMOS 15 DÍAS");
                    break;
                case 3:
                    title = string.Format("ÚLTIMA SEMANA");
                    break;
                case 4:
                    title = string.Format("{0:dd/MM/yyyy} AL {1:dd/MM/yyyy}", fechaInicio, fechaTermino);
                    break;
                default:
                    break;
            }

            return title;
        }

        public ReporteVentaGrupo __reporteVenta(int? tipoReporte, int tipoTramite, int? mes, int? anio, DateTime? fechaInicio, DateTime? fechaTermino, int pagoEstado = 1)
        {
            var lista = ExecProcedure<SqlConnection, SqlCommand, List<ReporteVenta>>("__sp_TotalReporteVenta", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_reporte", tipoReporte));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@mes", mes));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@anio", anio));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha_inicio", fechaInicio));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha_termino", fechaTermino));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_tramite", tipoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@pago_estado", pagoEstado));

                var reader = cmd.ExecuteReader();
                var lista = new List<ReporteVenta>();
                while (reader.Read())
                {
                    var row = new ReporteVenta();

                    //if (tipoTramite != 0 && tipo)
                    //    row.TipoTramite = reader.IsDBNull(reader.GetOrdinal("tipo_tramite")) ? "" : reader.GetString(reader.GetOrdinal("tipo_tramite"));

                    row.Grupo = reader.GetString(reader.GetOrdinal("grupo"));

                    row.Cantidad = reader.IsDBNull(reader.GetOrdinal("cantidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("cantidad"));
                    row.Monto = reader.IsDBNull(reader.GetOrdinal("monto")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("monto"));
                    lista.Add(row);
                }
                reader.Dispose();

                return lista;
            });

            string[] titles = new string[] { "RENOVACIÓN DE LICENCIA", "LICENCIA DE APRENDIZAJE", "DUPLICADO DE LICENCIA", "LICENCIA DE RECIPROCIDAD", "TRASPASO DE VEHÍCULOS" };
            string[] borderColors = new string[] { "#7E5CD7", "#58B4E0", "#ECBA5E", "#5DBC97", "#502FE1" };
            string[] bgColors = new string[] { "#CFC2F0", "#C0E2F3", "#F6E0B4", "#C2E5D7", "#524CE8" };

            int index = tipoTramite - 1;

            return new ReporteVentaGrupo
            {
                Cantidad = Convert.ToInt32(lista.Sum(x => x.Cantidad)),
                Monto = Convert.ToDecimal(lista.Sum(x => x.Monto)),
                Lista = lista,
                DataSetMonto = new DataSetChart { Label = titles[index], BackgroundColor = bgColors[index], BorderColor = borderColors[index], Fill = true, },
                DataSetCantidad = new DataSetChart { Label = titles[index], BackgroundColor = bgColors[index], BorderColor = borderColors[index], Fill = true, },
            };
        }

        public ReporteChartVenta ReporteVenta(int? tipoReporte, int tipoTramite, int? mes, int? anio, DateTime? fechaInicio, DateTime? fechaTermino, int pagoEstado = 1)
        {
            var datasetsMonto = new List<DataSetChart>();
            var datasetsCantidad = new List<DataSetChart>();
            var labels = new List<string>();
            var cantidadTotal = 0;
            var montoTotal = 0.0m;

            if (tipoReporte == 0)
            {
                labels = __generateMonthYearLabels(DateTime.Now, DateTime.Now.AddDays(1));
            }
            else if (tipoReporte == 1)
            {
                var startDate = new DateTime(anio ?? DateTime.Now.Year, mes ?? DateTime.Now.Month, 1);
                var endDate = (new DateTime(startDate.Year, startDate.Month, startDate.Day)).AddMonths(1);
                labels = __generateMonthYearLabels(startDate, endDate);
            } else if(tipoReporte == 2)
            {
                var endDate = DateTime.Now;
                var startDate = DateTime.Now.AddDays(-15);
                labels = __generateMonthYearLabels(startDate, endDate);
            } else if(tipoReporte == 3)
            {
                var endDate = DateTime.Now;
                var startDate = DateTime.Now.AddDays(-7);
                labels = __generateMonthYearLabels(startDate, endDate);
            } else
            {
                if (fechaInicio != null && fechaTermino != null)
                    labels = __generateMonthYearLabels(fechaInicio.Value, fechaTermino.Value);
            }

            if (tipoTramite != 0)
            {
                var rpt = __reporteVenta(tipoReporte, tipoTramite, mes, anio, fechaInicio, fechaTermino, pagoEstado);
                cantidadTotal = rpt.Cantidad;
                montoTotal = rpt.Monto;
                datasetsMonto.Add(rpt.DataSetMonto);
                datasetsCantidad.Add(rpt.DataSetCantidad);
            } else
            {
                var rl = __reporteVenta(tipoReporte, 1, mes, anio, fechaInicio, fechaTermino, pagoEstado);
                var la = __reporteVenta(tipoReporte, 2, mes, anio, fechaInicio, fechaTermino, pagoEstado);
                var dl = __reporteVenta(tipoReporte, 3, mes, anio, fechaInicio, fechaTermino, pagoEstado);
                var lr = __reporteVenta(tipoReporte, 4, mes, anio, fechaInicio, fechaTermino, pagoEstado);
                var tv = __reporteVenta(tipoReporte, 5, mes, anio, fechaInicio, fechaTermino, pagoEstado);

                var ds1Monto = new decimal[labels.Count];
                var ds2Monto = new decimal[labels.Count];
                var ds3Monto = new decimal[labels.Count];
                var ds4Monto = new decimal[labels.Count];
                var ds5Monto = new decimal[labels.Count];

                var ds1Cant = new decimal[labels.Count];
                var ds2Cant = new decimal[labels.Count];
                var ds3Cant = new decimal[labels.Count];
                var ds4Cant = new decimal[labels.Count];
                var ds5Cant = new decimal[labels.Count];

                for (int i = 0; i < labels.Count; i++)
                {
                    var ncMonto = rl.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (ncMonto != null)
                        ds1Monto[i] = Convert.ToDecimal(ncMonto.Monto);
                    else
                        ds1Monto[i] = 0;

                    var rcMonto = la.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (rcMonto != null)
                        ds2Monto[i] = Convert.ToDecimal(rcMonto.Monto);
                    else
                        ds2Monto[i] = 0;

                    var pcMonto = dl.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (pcMonto != null)
                        ds3Monto[i] = Convert.ToDecimal(pcMonto.Monto);
                    else
                        ds3Monto[i] = 0;

                    var ccMonto = lr.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (ccMonto != null)
                        ds4Monto[i] = Convert.ToDecimal(ccMonto.Monto);
                    else
                        ds4Monto[i] = 0;

                    var tvMonto = tv.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (tvMonto != null)
                        ds5Monto[i] = Convert.ToDecimal(tvMonto.Monto);
                    else
                        ds5Monto[i] = 0;

                    //

                    var ncCant = rl.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (ncCant != null)
                        ds1Cant[i] = Convert.ToDecimal(ncCant.Cantidad);
                    else
                        ds1Cant[i] = 0;

                    var rcCant = la.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (rcCant != null)
                        ds2Cant[i] = Convert.ToDecimal(rcCant.Cantidad);
                    else
                        ds2Cant[i] = 0;

                    var pcCant = dl.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (pcCant != null)
                        ds3Cant[i] = Convert.ToDecimal(pcCant.Cantidad);
                    else
                        ds3Cant[i] = 0;

                    var ccCant = lr.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (ccCant != null)
                        ds4Cant[i] = Convert.ToDecimal(ccCant.Cantidad);
                    else
                        ds4Cant[i] = 0;

                    var tvCant = tv.Lista.Where(x => x.Grupo == labels[i]).FirstOrDefault();
                    if (tvCant != null)
                        ds5Cant[i] = Convert.ToDecimal(tvCant.Cantidad);
                    else
                        ds5Cant[i] = 0;
                }

                rl.DataSetMonto.Data = ds1Monto;
                rl.DataSetCantidad.Data = ds1Cant;
                la.DataSetMonto.Data = ds2Monto;
                la.DataSetCantidad.Data = ds2Cant;
                dl.DataSetMonto.Data = ds3Monto;
                dl.DataSetCantidad.Data = ds3Cant;
                lr.DataSetMonto.Data = ds4Monto;
                lr.DataSetCantidad.Data = ds4Cant;
                tv.DataSetMonto.Data = ds5Monto;
                tv.DataSetCantidad.Data = ds5Cant;

                datasetsMonto.Add(rl.DataSetMonto);
                datasetsCantidad.Add(rl.DataSetCantidad);
                cantidadTotal += rl.Cantidad;
                montoTotal += rl.Monto;

                datasetsMonto.Add(la.DataSetMonto);
                datasetsCantidad.Add(la.DataSetCantidad);
                cantidadTotal += la.Cantidad;
                montoTotal += la.Monto;

                datasetsMonto.Add(dl.DataSetMonto);
                datasetsCantidad.Add(dl.DataSetCantidad);
                cantidadTotal += dl.Cantidad;
                montoTotal += dl.Monto;

                datasetsMonto.Add(lr.DataSetMonto);
                datasetsCantidad.Add(lr.DataSetCantidad);
                cantidadTotal += lr.Cantidad;
                montoTotal += lr.Monto;

                datasetsMonto.Add(tv.DataSetMonto);
                datasetsCantidad.Add(tv.DataSetCantidad);
                cantidadTotal += tv.Cantidad;
                montoTotal += tv.Monto;
            }

            var chart = new ReporteChartVenta();
            chart.AmountBar = new Chart
            {
                Type = "bar",
                Data = new ChartData
                {
                    Labels = labels,
                    Datasets = datasetsMonto,
                },
                Options = new
                {
                    plugins = new
                    {
                        title = new
                        {
                            display = true,
                            text = "REPORTE MONTO DE VENTAS - " + GetTituloReporte(tipoReporte, mes, anio, fechaInicio, fechaTermino),
                        }
                    }
                }
            };
            chart.QuantityBar = new Chart
            {
                Type = "bar",
                Data = new ChartData
                {
                    Labels = labels,
                    Datasets = datasetsCantidad,
                },
                Options = new
                {
                    plugins = new
                    {
                        title = new
                        {
                            display = true,
                            text = "REPORTE DE VENTAS - " + GetTituloReporte(tipoReporte, mes, anio, fechaInicio, fechaTermino),
                        }
                    }
                }
            };

            chart.Total = cantidadTotal;

            chart.Monto = montoTotal;

            return chart;
        }

        public IEnumerable<ReporteDetalleVenta> ReporteDetalleVenta(int? tipoReporte, int tipoTramite, int? mes, int? anio, DateTime? fechaInicio, DateTime? fechaTermino, int pagoEstado = 1)
        {
            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<ReporteDetalleVenta>>("__sp_ReporteDetalleVenta", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_reporte", tipoReporte));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@mes", mes));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@anio", anio));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha_inicio", fechaInicio));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha_termino", fechaTermino));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_tramite", tipoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@pago_estado", pagoEstado));

                var reader = cmd.ExecuteReader();
                var lista = new List<ReporteDetalleVenta>();
                while (reader.Read())
                {
                    var row = new ReporteDetalleVenta();
                    row.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    row.TipoTramite = reader.GetString(reader.GetOrdinal("tipo_tramite"));
                    row.NombreCliente = reader.IsDBNull(reader.GetOrdinal("nombre_cliente")) ? "" : reader.GetString(reader.GetOrdinal("nombre_cliente"));
                    row.Correo = reader.IsDBNull(reader.GetOrdinal("correo")) ? "" : reader.GetString(reader.GetOrdinal("correo"));
                    row.Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? "" : reader.GetString(reader.GetOrdinal("telefono"));
                    row.PagoCodigo = reader.IsDBNull(reader.GetOrdinal("pago_codigo")) ? "" : reader.GetString(reader.GetOrdinal("pago_codigo"));
                    row.Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? -1 : reader.GetInt32(reader.GetOrdinal("estado"));
                    row.Metodo = reader.IsDBNull(reader.GetOrdinal("metodo")) ? -1 : reader.GetInt32(reader.GetOrdinal("metodo"));
                    row.Nota = reader.IsDBNull(reader.GetOrdinal("nota")) ? "" : reader.GetString(reader.GetOrdinal("nota"));
                    row.PagoCodigo = reader.IsDBNull(reader.GetOrdinal("pago_codigo")) ? "" : reader.GetString(reader.GetOrdinal("pago_codigo"));
                    row.PagoFecha = reader.IsDBNull(reader.GetOrdinal("pago_fecha")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("pago_fecha"));
                    row.PagoId = reader.IsDBNull(reader.GetOrdinal("pg_id")) ? null : reader.GetInt32(reader.GetOrdinal("pg_id"));
                    row.ClienteId = reader.IsDBNull(reader.GetOrdinal("cl_id")) ? null : reader.GetInt32(reader.GetOrdinal("cl_id"));
                    row.Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0 : Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("precio")));
                    row.Iniciado = reader.IsDBNull(reader.GetOrdinal("iniciado")) ? "No" : reader.GetString(reader.GetOrdinal("iniciado"));
                    lista.Add(row);
                }
                reader.Close();
                return lista;
            });
        }


        public IEnumerable<ReporteTramiteCasoTiempo> ReporteTiempo(int tipoReporte, int? tipoTramite, int? mes, int? anio, DateTime? fechaInicio, DateTime? fechaTermino,
            int? estadoTramite, int? estadoProceso,
            string? nombre, string? apePaterno, string? apeMaterno, string? correo, int nroDias = 0)
        {
            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<ReporteTramiteCasoTiempo>>("__sp_EstadoTiempoReporte", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_reporte", tipoReporte));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@mes", mes));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@anio", anio));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha_inicio", fechaInicio));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha_termino", fechaTermino));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_tramite", tipoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado_tramite", estadoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado_proceso", estadoProceso));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@nombre_cli", nombre));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@ape_paterno_cli", apePaterno));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@ape_materno_cli", apeMaterno));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@correo_cli", correo));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@nro_dias", nroDias));
                var reader = cmd.ExecuteReader();
                var lista = new List<ReporteTramiteCasoTiempo>();
                while (reader.Read())
                {
                    var row = new ReporteTramiteCasoTiempo();
                    row.Id = reader.GetInt64(reader.GetOrdinal("id"));
                    row.TipoTramite = reader.GetString(reader.GetOrdinal("tipo"));
                    row.NombreCliente = reader.IsDBNull(reader.GetOrdinal("nombre_cliente")) ? "" : reader.GetString(reader.GetOrdinal("nombre_cliente"));
                    row.Correo = reader.IsDBNull(reader.GetOrdinal("correo")) ? "" : reader.GetString(reader.GetOrdinal("correo"));
                    row.Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? "" : reader.GetString(reader.GetOrdinal("telefono"));
                    row.PagoCodigo = reader.IsDBNull(reader.GetOrdinal("pago_codigo")) ? "" : reader.GetString(reader.GetOrdinal("pago_codigo"));
                    row.Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? "" : reader.GetString(reader.GetOrdinal("estado"));
                    row.EstadoProceso = reader.IsDBNull(reader.GetOrdinal("estado_proceso")) ? "" : reader.GetString(reader.GetOrdinal("estado_proceso"));
                    row.PagoCodigo = reader.IsDBNull(reader.GetOrdinal("pago_codigo")) ? "" : reader.GetString(reader.GetOrdinal("pago_codigo"));
                    row.PagoFecha = reader.IsDBNull(reader.GetOrdinal("pago_fecha")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("pago_fecha"));
                    row.PagoId = reader.IsDBNull(reader.GetOrdinal("pg_id")) ? null : reader.GetInt32(reader.GetOrdinal("pg_id"));
                    row.ClienteId = reader.IsDBNull(reader.GetOrdinal("cl_id")) ? null : reader.GetInt32(reader.GetOrdinal("cl_id"));
                    row.Dias = reader.IsDBNull(reader.GetOrdinal("dias")) ? 0 : reader.GetInt32(reader.GetOrdinal("dias"));
                    lista.Add(row);
                }
                reader.Dispose();

                return lista;
            });
        }


        private static List<string> __generateMonthYearLabels(DateTime startDate, DateTime endDate)
        {
            List<string> labels = new List<string>();

            if (startDate > endDate)
                throw new ArgumentException("La fecha de inicio debe ser menor o igual a la fecha de término");

            DateTime current = new DateTime(startDate.Year, startDate.Month, 1);
            var months = new string[] { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };

            while (current < endDate)
            {
                string monthName = months[current.Month - 1];
                string label = $"{monthName} {current.Year}";
                labels.Add(label);

                current = current.AddMonths(1);
            }

            return labels;
        }
        public string SanitizeParameter(object? value)
        {
            if (value == null || value.Equals(0) || value.ToString().Trim() == "0")
            {
                return "";
            }
            return value.ToString();
        }
        public IEnumerable<ReporteMulta>? ReporteMulta(string? cl_nombre, string? cl_correo, string? cl_numeroTelefono, string? pg_codigo, string? tr_id, string? fecha, string? estado)
        {
            try
            { 
                return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<ReporteMulta>>("spSearchMultas", (cmd) =>
                {
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@cl_nombre", SanitizeParameter(cl_nombre)));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@cl_correo", SanitizeParameter(cl_correo)));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@cl_numeroTelefono", SanitizeParameter(cl_numeroTelefono)));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@pg_codigo", SanitizeParameter(pg_codigo)));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@tr_id", SanitizeParameter(tr_id)));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@fecha", SanitizeParameter(fecha)));
                    cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado", SanitizeParameter(estado)));

                    var reader = cmd.ExecuteReader();
                    var lista = new List<ReporteMulta>();
                    while (reader.Read())
                    {
                        var row = new ReporteMulta();
                        row.ID = reader.GetString(reader.GetOrdinal("ID"));
                        row.cl_id = reader.GetString(reader.GetOrdinal("cl_id"));
                        row.nombreCliente = reader.GetString(reader.GetOrdinal("nombreCliente"));

                        row.cl_correo = reader.GetString(reader.GetOrdinal("cl_correo"));

                        row.cl_numeroTelefono = reader.GetString(reader.GetOrdinal("cl_numeroTelefono"));
                        row.tr_nombre = reader.GetString(reader.GetOrdinal("tr_nombre"));
                        row.ORIGEN = reader.GetString(reader.GetOrdinal("ORIGEN"));
                        row.TOTAL = reader.GetString(reader.GetOrdinal("TOTAL"));

                        row.PAGADO = reader.GetString(reader.GetOrdinal("PAGADO"));
                        row.TIPO = reader.GetString(reader.GetOrdinal("TIPO"));
                        row.FECHA_CREACION = reader.GetString(reader.GetOrdinal("FECHA_CREACION"));
                        row.CODIGO_PAGO = reader.GetString(reader.GetOrdinal("CODIGO_PAGO"));
                        row.CANTIDAD_CUOTAS = reader.GetString(reader.GetOrdinal("CANTIDAD_CUOTAS"));
                    
                    

                        lista.Add(row);
                    }
                    reader.Close();
                    return lista;
            });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                return Enumerable.Empty<ReporteMulta>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<ReporteMulta>();
            }
        }
    }
}
