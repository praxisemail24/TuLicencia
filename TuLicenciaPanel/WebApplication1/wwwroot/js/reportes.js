function mergeDeep(current, updates) {
    for (key of Object.keys(updates)) {
        if (!current.hasOwnProperty(key) || typeof updates[key] !== 'object') current[key] = updates[key];
        else mergeDeep(current[key], updates[key]);
    }
    return current;
}

var rptTable = function () {
    this.table = new Object();
    this.chart = new Object();
    this.defaultsTable = {
        destroy: true,
        processing: true,
        serverSide: true,
        responsive: true,
        ajax: {
            type: 'POST',
            dataType: 'json',
        },
        language: {
            url: 'https://tulicenciapr.com/admin/es-MX.json',
        },
        searching: false,
    };
    this.defaultsChart = {
        type: 'bar',
        options: {
            /*scales: {
                y: {
                    min: 0,
                    max: 100,
                    ticks: {
                        stepSize: 10,
                    },
                }
            }*/
            plugins: {
                legend: {
                    position: 'bottom',
                }
            }
        }
    };
}

rptTable.prototype = {
    initTable(selector, configTable = {}) {
        if (this.table[selector]) {
            this.table[selector].destroy();
        }

        var configTable = mergeDeep({...this.defaultsTable}, configTable);
        this.table[selector] = $(selector).DataTable(configTable);
    },
    refreshTable(selector = '#reporte') {
        this.table[selector].ajax.reload()
    },
    concatUrl(uri) {
        return baseApiUrlEndPoint + uri;
    },
    initChart(selector, config) {
        if (this.chart[selector]) {
            this.chart[selector].destroy();
        }

        var ctx = document.querySelector(selector);
        var configChart = mergeDeep({ ...this.defaultsChart }, config);
        this.chart[selector] = new Chart(ctx, configChart);
    },
    rptGeneral(selector) {
        var rpt = this;
        rpt.initTable(selector, {
            ajax: {
                url: this.concatUrl('/ReporteCaso/general'),
                data: (params) => {
                    var newParams = {
                        ...params,
                        AdditionalValues: {
                            TipoTramite: $('#tipo').val(),
                            EstadoTipo: $('#estado').val(),
                            EstadoProceso: $('#estado_proceso').val(),
                            Nombres: $('#nombres').val(),
                            ApellidoMaterno: $('#apellido_paterno').val(),
                            ApellidoPaterno: $('#apellido_materno').val(),
                            Correo: $('#correo').val(),
                        }
                    };
                    return newParams;
                }
            },
            columns: [
                { data: 'id', name: 'ID' },
                { data: 'tipoTramite', name: 'TIPO TRÁMITE' },
                { data: 'nombreCliente', name: 'NOMBRE CLIENTE', },
                { data: 'correo', name: 'CORREO' },
                { data: 'telefono', name: 'TELÉFONO' },
                { data: 'pagoCodigo', name: 'CODIGO PAGO' },
                {
                    data: 'pagoFecha', name: 'FECHA PAGO',
                    render: renderCellDateTime,
                },
                {
                    data: 'estado',
                    name: 'ESTADO',
                    render: (data, type, row, meta) => {
                        var type = data.trim().toLowerCase().replace(" ", "-");
                        var style = type === 'new-cases' ? 'bg-primary-transparent' : '';
                        style = type === 'closed-cases' ? 'bg-success-transparent' : style;
                        style = type === 'reviewed-cases' ? 'bg-warning-transparent' : style;
                        style = type === 'processed-cases' ? 'bg-info-transparent' : style;
                        return '<div class="d-flex align-items-center justify-content-center"><span class="badge ' + style + ' font-semibold">' + data + '<span></div>';
                    }
                },
                {
                    data: 'estadoProceso',
                    name: 'ESTADO PROCESO',
                    render: (data, type, row, meta) => {
                        var color = data === 0 ? 'bg-success-transparent' : 'bg-warning-transparent';
                        return '<span class="badge ' + color + '">' + (data === 0 ? 'PENDIENTE' : 'ACEPTADO') + '</div>';
                    }
                },
            ],
        })

        $('#btnbuscar').on('click', () => {
            rpt.refreshTable(selector);
        });

        $('#btnexportar').on('click', () => {
            var query = $.param({
                TipoTramite: $('#tipo').val(),
                EstadoTipo: $('#estado').val(),
                EstadoProceso: $('#estado_proceso').val(),
                Nombres: $('#nombres').val(),
                ApellidoMaterno: $('#apellido_paterno').val(),
                ApellidoPaterno: $('#apellido_materno').val(),
                Correo: $('#correo').val(),
            });
            window.open(rpt.concatUrl('/ReporteCaso/general/exportar?' + query), '_blank');
        });
    },
    rptAsyncPeriodo(params) {
        $.ajax({
            url: this.concatUrl('/ReporteCaso/periodo'),
            dataType: 'json',
            contentType: 'application/json',
            type: 'POST',
            data: JSON.stringify(params),
            cache: false,
            success: (response) => {
                if (response.success) {
                    this.initChart('#summary-bar', response.data.chartBar);
                    $('#table-summary').DataTable({
                        destroy: true,
                        serverSide: false,
                        processing: false,
                        paging: false,
                        searching: false,
                        data: response.data.summary,
                        columns: [
                            {
                                data: 'key', render: (data, type, row, meta) => {
                                    console.dir(row)
                                    return '<span style="color: ' + row.color + ';">' + data + '</span>';
                                },
                            },
                            { data: 'value' },
                        ],
                        order: {
                            name: 'index',
                            dir: 'asc'
                        }
                    });
                    $('#info-sin-procesar').text(response.data.sinIniciar);
                }
            }
        });
    },
    rptPeriodo(selector) {
        var rpt = this;

        var _paramsQuery = (params = {}) => {
            var estadoTipo = $('#estado').val();
            var estadoProceso = $('#estado_proceso').val();
            estadoTipo = estadoTipo === "" ? null : estadoTipo;
            estadoProceso = estadoProceso === "" ? null : estadoProceso;
            return {
                ...params,
                TipoReporte: $('#tipo_reporte').val(),
                Anio: $('#anio').val(),
                Mes: $('#mes').val(),
                TipoTramite: $('#tipo').val(),
                FechaInicio: $('#fecha_inicio').val(),
                FechaTermino: $('#fecha_termino').val(),
            };
        }

        this.rptAsyncPeriodo(_paramsQuery());

        $('#tipo_reporte').on('change', (e) => {
            var value = parseInt($(e.target).val());
            $('#fecha_inicio').val(null);
            $('#fecha_termino').val(null);

            $('#mes').attr('disabled', 'disabled');
            $('#anio').attr('disabled', 'disabled');
            $('#fecha_inicio').attr('disabled', 'disabled');
            $('#fecha_termino').attr('disabled', 'disabled');
            if (value === 0) {
                $('#fecha_inicio').removeAttr('disabled');
            }
            if (value === 1) {
                $('#mes').removeAttr('disabled');
                $('#anio').removeAttr('disabled');
            }
            if (value === 4) {
                $('#fecha_inicio').removeAttr('disabled');
                $('#fecha_termino').removeAttr('disabled');
            }
        });

        $('#btnbuscar').on('click', () => {
            rpt.rptAsyncPeriodo(_paramsQuery());
        });

        $('#btnexportar').hide();
        $('#btnexportar').on('click', () => {
            var query = $.param(_paramsQuery());
            window.open(rpt.concatUrl('/ReporteCaso/periodo/exportar?' + query), '_blank');
        });
    },
    rptVentas() {
        var rpt = this;

        var _paramsQuery = (params = {}) => {
            return {
                ...params,
                TipoReporte: $('#tipo_reporte').val(),
                TipoTramite: $('#tipo').val(),
                Anio: $('#anio').val(),
                Mes: $('#mes').val(),
                FechaInicio: $('#fecha_inicio').val(),
                FechaTermino: $('#fecha_termino').val(),
            }
        }

        rpt.rptVentasChart(_paramsQuery());

        rpt.initTable('#reporte', {
            ajax: {
                url: rpt.concatUrl('/ReporteVenta/detalle'),
                type: 'POST',
                data: (params) => {
                    return {
                        ...params,
                        AdditionalValues: _paramsQuery(),
                    }
                }
            },
            columns: [
                { data: 'tipoTramite', name: 'TIPO TRÁMITE' },
                {
                    data: 'id', name: 'NOMBRE CLIENTE',
                    render: (data, type, row, meta) => {
                        return '<div class="d-flex flex-column"><div class="text-uppercase">' + row.nombreCliente + '</div><div class="d-flex flex-row align-items-center"><i class="la la-envelope text-warning"></i><span class="ms-2">' + row.correo +'</span></div><div class="d-flex flex-row align-items-center"><i class="la la-phone text-info"></i><span class="ms-2">'+row.telefono+'</span></div></div>';
                    }
                },
                { data: 'pagoCodigo', name: 'CODIGO PAGO' },
                {
                    data: 'pagoFecha', name: 'FECHA PAGO',
                    render: renderCellDateTime,
                },
                {
                    data: 'estado',
                    name: 'ESTADO',
                    render: (data, type, row, meta) => {
                        var text = data === 1 ? 'PROCESADO' : '';
                        var style = data === 1 ? 'bg-success-transparent' : '';
                        var style = data === 0 ? 'bg-danger-transparent' : style;
                        //style = type === 'closed-cases' ? 'bg-success-transparent' : style;
                        //style = type === 'reviewed-cases' ? 'bg-warning-transparent' : style;
                        //style = typ === 'processed-cases' ? 'bg-info-transparent' : style;
                        return '<div class="d-flex align-items-center justify-content-center"><span class="badge ' + style + ' font-semibold">' + text + '<span></div>';
                    }
                },
                {
                    data: 'metodo',
                    name: 'METODO',
                    render: (data, type, row, meta) => {
                        var color = data === 1 ? 'bg-success-transparent' : 'bg-warning-transparent';
                        var text = data === 1 ? 'TARJETA' : '';
                        return '<span class="badge ' + color + '">' + text + '</div>';
                    }
                },
                {
                    data: 'precio', name: 'MONTO',
                    render: (data, type, row, meta) => {
                        return '$ ' + parseFloat(data).toFixed(2);
                    }
                },
                {
                    data: 'nota', name: 'NOTA',
                },
                {
                    data: 'iniciado', name: 'INICIADO',
                    render: (data, type, row, meta) => {
                        var color = data === 'Sí' ? 'bg-success-transparent' : 'bg-danger-transparent';
                        return '<span class="badge ' + color + '">' + data + '</div>';
                    }
                },
            ]
        });

        $('#tipo_reporte').on('change', (e) => {
            var value = parseInt($(e.target).val());
            $('#fecha_inicio').val(null);
            $('#fecha_termino').val(null);
            $('#tipo').val(0);
            $('input.form-control').val(null);
            $('#anio').val((new Date()).getFullYear());

            $('#mes').attr('disabled', 'disabled');
            $('#anio').attr('disabled', 'disabled');
            $('#fecha_inicio').attr('disabled', 'disabled');
            $('#fecha_termino').attr('disabled', 'disabled');

            if (value === 0) {
                $('#fecha_inicio').removeAttr('disabled');
            }
            if (value === 1) {
                $('#mes').removeAttr('disabled');
                $('#anio').removeAttr('disabled');
            }
            if (value === 4) {
                $('#fecha_inicio').removeAttr('disabled');
                $('#fecha_termino').removeAttr('disabled');
            }
        });

        $('#btnbuscar').on('click', () => {
            var tipo = parseInt($('#tipo_reporte').val());
            var error = false;
            if (tipo == 4) {
                var date = new Date($('#fecha_termino').val());
                if (date > (new Date())) {
                    toastr.error('Seleccione una fecha válida.');
                    error = true;
                }
            }

            if (error === false) {
                rpt.rptVentasChart(_paramsQuery());
                rpt.refreshTable('#reporte');
            }
        });

        $('#btnexportar').on('click', () => {
            var tipo = parseInt($('#tipo_reporte').val());
            var error = false;
            if (tipo == 4) {
                var date = new Date($('#fecha_termino').val());
                if (date > (new Date())) {
                    toastr.error('Seleccione una fecha válida.');
                    error = true;
                }
            }

            if (error === false) {
                var query = $.param(_paramsQuery());
                window.open(rpt.concatUrl('/ReporteVenta/exportar?' + query), '_blank');
            }
        });
    },
    rptVentasChart(params) {
        $.ajax({
            url: this.concatUrl('/ReporteVenta/general'),
            data: JSON.stringify(params),
            dataType: 'json',
            contentType: 'application/json',
            type: 'POST',
            cache: false,
            success: (response) => {
                if (response.success) {
                    $('#total-cantidad').text(response.data.total);
                    $('#total-monto').text('$ ' + parseFloat(response.data.monto).toFixed(2));
                    this.initChart('#summary-bar-amount', response.data.amountBar);
                    this.initChart('#summary-bar-quantity', response.data.quantityBar);
                    $('#titulo-reporte').text(response.data.quantityBar.options.plugins.title.text);
                }
            }
        });
    },
    rptTiempo() {
        var rpt = this;

        var _paramsQuery = (params = {}) => {
            return {
                ...params,
                TipoReporte: $('#tipo_reporte').val(),
                TipoTramite: $('#tipo').val(),
                Anio: $('#anio').val(),
                Mes: $('#mes').val(),
                FechaInicio: $('#fecha_inicio').val(),
                FechaTermino: $('#fecha_termino').val(),
                TipoEstado: $('#estado').val(),
                NroDias: $('#nrodias').val(),
            }
        }

        this.initTable('#table-report', {
            ajax: {
                url: this.concatUrl('/ReporteCaso/tiempos'),
                type: 'POST',
                data: (params) => {
                    return {
                        ...params,
                        AdditionalValues: _paramsQuery(),
                    }
                }
            },
            columns: [
                { data: 'tipoTramite', name: 'TIPO TRÁMITE' },
                { data: 'nombreCliente', name: 'NOMBRE CLIENTE' },
                { data: 'correo', name: 'CORREO' },
                { data: 'telefono', name: 'TELÉFONO' },
                { data: 'pagoCodigo', name: 'CODIGO PAGO' },
                {
                    data: 'pagoFecha', name: 'FECHA PAGO',
                    render: renderCellDateTime,
                },
                {
                    data: 'estado',
                    name: 'ESTADO',
                    render: (data, type, row, meta) => {
                        var type = data.trim().toLowerCase().replace(" ", "-");
                        var style = type === 'new-cases' ? 'bg-primary-transparent' : '';
                        style = type === 'closed-cases' ? 'bg-success-transparent' : style;
                        style = type === 'reviewed-cases' ? 'bg-warning-transparent' : style;
                        style = type === 'processed-cases' ? 'bg-info-transparent' : style;
                        return '<div class="d-flex align-items-center justify-content-center"><span class="badge ' + style + ' font-semibold">' + data + '<span></div>';
                    }
                },
                {
                    data: 'estadoProceso',
                    name: 'ESTADO PROCESO',
                    render: (data, type, row, meta) => {
                        var color = data === 'ACEPTADO' ? 'bg-success-transparent' : 'bg-warning-transparent';
                        return '<span class="badge ' + color + '">' + data + '</div>';
                    }
                },
                { data: 'dias', name: 'DÍAS' },
            ],
            rowCallback: (row, data, index) => {
                var dias = data.dias;
                if (dias <= 7) {
                    $('td', row).addClass('bg-success-transparent');
                } else if (dias >= 8 && dias <= 15) {
                    $('td', row).addClass('bg-warning-transparent');
                } else {
                    $('td', row).addClass('bg-danger-transparent');
                }
            }
        });

        $('#tipo_reporte').on('change', (e) => {
            var value = parseInt($(e.target).val());
            $('#fecha_inicio').val(null);
            $('#fecha_termino').val(null);
            $('#tipo').val(null);
            $('#nrodias').val(null);
            $('#estado').val(null);
            $('#estado_proceso').val(null);
            $('input.form-control').val(null);
            $('#anio').val((new Date()).getFullYear());

            $('#mes').attr('disabled', 'disabled');
            $('#anio').attr('disabled', 'disabled');
            $('#fecha_inicio').attr('disabled', 'disabled');
            $('#fecha_termino').attr('disabled', 'disabled');

            if (value === 0) {
                $('#fecha_inicio').removeAttr('disabled');
            }
            if (value === 1) {
                $('#mes').removeAttr('disabled');
                $('#anio').removeAttr('disabled');
            }
            if (value === 4) {
                $('#fecha_inicio').removeAttr('disabled');
                $('#fecha_termino').removeAttr('disabled');
            }
        });

        $('#btnbuscar').on('click', () => {
            $('#table-report').DataTable().ajax.reload();
        });

        $('#btnexportar').on('click', () => {
            var query = $.param(_paramsQuery());
            window.open(rpt.concatUrl('/ReporteCaso/tiempos/exportar?' + query), '_blank');
        });
    }
}

var reporte = new rptTable()