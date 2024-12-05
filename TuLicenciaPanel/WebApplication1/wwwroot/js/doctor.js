$(document).ready(function () {


    var today = new Date();
    var oneMonthAgo = new Date();
    oneMonthAgo.setMonth(today.getMonth() - 1);

    var todayFormatted = today.toISOString().split('T')[0];
    var oneMonthAgoFormatted = oneMonthAgo.toISOString().split('T')[0];

    $('#fechaHasta').val(todayFormatted);
    $('#fechaDesde').val(oneMonthAgoFormatted);

    $('#filtrar').click(function () {
        const doctorId = $('#doctorId').val();
        const fechaDesde = $('#fechaDesde').val();
        const fechaHasta = $('#fechaHasta').val();
        const nombreTramite = $('#nombreTramite').val();
        const nombreCliente = $('#nombreCliente').val();
        const estado = $('#estado').val();

        // Prepara los datos para el filtro
        const formData = {
            doctorId: parseInt(doctorId),  // Asegúrate de que sea un número
            fechaDesde: fechaDesde,
            fechaHasta: fechaHasta,
            nombreTramite: nombreTramite,
            nombreCliente: nombreCliente,
            estado: estado
        };

        //$('#tablaCasos').DataTable().clear().destroy();

        $('#tablaCasos').DataTable({
            processing: true,
            serverSide: true,
            responsive: true,
            searching: false,
            ajax: {
                url: baseApiUrlEndPoint + '/Doctor/ObtenerCasosAsignadosTable',
                type: 'POST',
                data: (params) => {
                    var mParams = {
                        ...params,
                        length: 10,
                        AdditionalValues: {
                            ...formData,
                        }
                    };
                    return mParams;
                },
                headers: {
                    'Authorization': `Bearer ${jwtToken}`
                },
            },
            columns: [
                { data: 'rowIndex' },
                {
                    data: 'fecha',
                    render: renderCellDateTime
                },
                { data: 'codigo' },
                { data: 'nombreCliente' },
                { data: 'tipoTramite' },
                { data: 'nombreTramite' },
                {
                    data: 'estado',
                    render: function (data, type, row) {
                        let labelClass = '';
                        let estadoTexto = '';

                        switch (data.toLowerCase()) {
                            case 'pendiente':
                                labelClass = 'badge bg-warning-transparent';
                                estadoTexto = 'Pendiente';
                                break;
                            case 'asignado':
                                labelClass = 'badge bg-info-transparent';
                                estadoTexto = 'Asignado';
                                break;
                            case 'aprobado':
                                labelClass = 'badge bg-success-transparent';
                                estadoTexto = 'Aprobado';
                                break;
                            case 'denegado':
                                labelClass = 'badge bg-danger-transparent';
                                estadoTexto = 'Denegado';
                                break;
                            case 'unreachable':
                                labelClass = 'badge bg-primary-transparent';
                                estadoTexto = 'Unreachable';
                                break;
                            default:
                                labelClass = '';
                                estadoTexto = data;
                                break;
                        }

                        return `<span class="badge ${labelClass}">${estadoTexto}</span>`;
                    }
                },
                {
                    data: 'id',
                    render: function (data, type, row) {
                        var html = `<a aria-label="anchor" href="/admin/Home/Evaluacion/${row.id}/${row.tr_id}" class="btn btn-icon waves-effect waves-light btn-sm btn-danger">
                                            <i class="ri-medicine-bottle-line"></i>
                                        </a>`;

                        html += `<a style="margin-left: 4px;" aria-label="anchor" target="_blank" href="${baseApiUrlEndPoint}/pdf/gen-certificado-medico/${row.tr_id}/${row.id}/1" class="btn btn-icon waves-effect waves-light btn-sm btn-info">
                                            <i class="la la-file-pdf"></i>
                                        </a>`;

                        return html;
                    }
                }
            ],
            destroy: true,
        });
    });

    // Opcional: Ejecuta la función de filtro automáticamente al cargar la página
    $('#filtrar').trigger('click');
});


function abrirModal(id) {
    console.log('ID:::::::::::', id);
    $('#myModal').modal('show');
}
