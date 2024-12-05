//$(document).ready(() => {
//    // Evento click del botón buscar
//    $('#btnbuscar').click(() => {
//        const AdditionalValues = {
//            cl_nombre: $('#cliente').val() || "",
//            cl_correo: $('#correo').val() || "",
//            cl_numeroTelefono: $('#phone').val() || "",
//            pg_codigo: $('#codigopago').val() || "",
//            tr_id: $('#tipotra').val() || "",
//            fecha: $('#fecha').val() || "", // Si es "Fechas"
//            estado: $('#tipo').val() || ""
//        };

//        console.log("AdditionalValues:", AdditionalValues);

//        // Crear un objeto FormData y agregar los datos
//        const formData = new FormData();
//        for (const key in AdditionalValues) {
//            const value = AdditionalValues[key] == null ? "" : AdditionalValues[key];
//            formData.append(`AdditionalValues.${key}`, value);
//        }

//        // Depuración de formData
//        for (var pair of formData.entries()) {
//            console.log(pair[0] + ', ' + pair[1]);
//        }

//        // Realizar la solicitud AJAX
//        $.ajax({
//            url: baseApiUrlEndPoint + '/ReporteVenta/multa',
//            type: 'POST',
//            data: formData,
//            processData: false,
//            contentType: false,
//            headers: {
//                'Authorization': `Bearer ${jwtToken}`
//            },
//            success: (response) => {
//                // Limpiar la tabla existente
//                $('#reporte').DataTable().clear().destroy();
//                console.log(response);
//                // Inicializar DataTable con nuevos datos
//                $('#reporte').DataTable({
//                    data: response.data,
//                    columns: [
//                        { data: 'codigO_PAGO' },
//                        { data: 'nombreCliente' },
//                        { data: 'tr_nombre' },
//                        { data: 'pagado' },
//                        { data: 'cl_numeroTelefono' },
//                        { data: 'cl_correo' },

//                        { data: 'total' },
//                        { data: 'cantidaD_CUOTAS' },
//                        { data: 'origen' },
//                        { data: 'fechA_CREACION' },
//                    ]
//                });

//                // Mostrar el título del reporte
//                $('#titulo-reporte').text(`Resultados de Búsqueda - ${tipoReporte}`);
//            },
//            error: (xhr, status, error) => {
//                console.error('Error al buscar el reporte:', error);
//            }
//        });
//    });
//});


$(document).ready(() => {
    // Función para realizar la búsqueda
    const realizarBusqueda = () => {
        const AdditionalValues = {
            cl_nombre: $('#cliente').val() || "",
            cl_correo: $('#correo').val() || "",
            cl_numeroTelefono: $('#phone').val() || "",
            pg_codigo: $('#codigopago').val() || "",
            tr_id: $('#tipotra').val() || "",
            fecha: $('#fecha').val() || "",
            estado: $('#tipo').val() || ""
        };

        console.log("AdditionalValues:", AdditionalValues);

        const formData = new FormData();
        for (const key in AdditionalValues) {
            const value = AdditionalValues[key] == null ? "" : AdditionalValues[key];
            formData.append(`AdditionalValues.${key}`, value);
        }

        for (var pair of formData.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }

        $.ajax({
            url: baseApiUrlEndPoint + '/ReporteVenta/multa',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: (response) => {
                $('#reporte').DataTable().clear().destroy();
                console.log(response);
                $('#reporte').DataTable({
                    data: response.data,
                    columns: [
                        { data: 'codigO_PAGO' },
                        { data: 'nombreCliente' },
                        { data: 'tr_nombre' },
                        

                        {
                            data: 'pagado',
                            name: 'ESTADO',
                            render: (data, type, row, meta) => {
                                var text = data ;

                                var style = data === 'Pagado' ? 'bg-success-transparent' : '';
                                var style = data === 'Pendiente' ? 'bg-warning-transparent' : style;
                                var style = data === 'No Pago' ? 'bg-danger-transparent' : style;

                                //style = type === 'closed-cases' ? 'bg-success-transparent' : style;
                                //style = type === 'reviewed-cases' ? 'bg-warning-transparent' : style;
                                //style = typ === 'processed-cases' ? 'bg-info-transparent' : style;
                                return '<div class="d-flex align-items-center justify-content-center"><span class="badge ' + style + ' font-semibold">' + text + '<span></div>';
                            }
                        },

                        { data: 'cl_numeroTelefono' },
                        { data: 'cl_correo' },
                        { data: 'total' },
                        { data: 'cantidaD_CUOTAS' },
                        { data: 'origen' },
                        { data: 'fechA_CREACION' },
                        {
                            data: null,
                            name: 'INVOICE',
                            render: (data, type, row, meta) => {
                                if (row.pagado === "Pagado") {
                                    return `<center> <a href=Multa/VerPago?codigopago=${row.codigO_PAGO} class="btn btn-primary btn-sm"><i class="bi bi-receipt"></i></a></center>`;
                                } else {
                                    return `<span class="text-muted">No Pagado</span>`;
                                }
                            }
                        }
                    ], order: [[9, 'desc']],
                    responsive: true,
                    autoWidth: false,
                    scrollX: true

                });

                $('#titulo-reporte').text(`Resultados de Búsqueda - Multas`);
            },
            error: (xhr, status, error) => {
                console.error('Error al buscar el reporte:', error);
            }
        });
    };

    // Evento click del botón buscar
    $('#btnbuscar').click(() => {
        realizarBusqueda();
    });

    // Evento load para realizar la búsqueda
    $(window).on('load', () => {
        realizarBusqueda();
    });
});

