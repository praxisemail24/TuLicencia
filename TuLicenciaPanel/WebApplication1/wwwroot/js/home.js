function HomePage() {
    this.storageStatusKey = 'statusCacheActive';
    var status = localStorage.getItem(this.storageStatusKey);

    if (status !== null && localStorage.getItem('authNivelId') !== null && localStorage.getItem('authUserId') !== null) {
        var nivelId = localStorage.getItem('authNivelId');
        var userId = parseInt(localStorage.getItem('authUserId'));

        //if (userAuth.id !== userId && userAuth.nivel !== nivelId) {
        //    status = userAuth.nivel == '1' ? 0 : 1;
        //}

        status = userAuth.nivel == '1' ? 0 : 1;
    } else {
        localStorage.setItem('authNivelId', userAuth.nivel);
        localStorage.setItem('authUserId', userAuth.id);
        status = userAuth.nivel == '1' ? 0 : 1;
    }

    this.loadStatus(status);
};

HomePage.prototype = {
    loadTable(e) {
        this.loadStatus($(e.currentTarget).data('status'));
    },
    loadStatus(status) {
        status = parseInt(status);
        localStorage.setItem(this.storageStatusKey, status);

        console.log('Load Case', status);

        configDataTables();

        $('.category-link').removeClass('active');
        $(`button[data-status="${status}"]`).toggleClass('active');

        if (status === 0 || status === 1 || status === 2) {
            $('#filtros').show();
        } else {
            $('#filtros').hide();
        }

        $('#tb-cases__title').removeClass('bg-primary-transparent');
        $('#tb-cases__title').removeClass('bg-secondary-transparent');
        $('#tb-cases__title').removeClass('bg-warning-transparent');
        $('#tb-cases__title').removeClass('bg-success-transparent');

        switch (status) {
            case 0:
                $('#tb-cases__title').text('NEW CASES');
                $('#tb-cases__title').addClass('bg-primary-transparent');
                //element.innerText = 'NEW CASES';
                break;
            case 1:
                $('#tb-cases__title').text('REVIEWED CASES');
                $('#tb-cases__title').addClass('bg-secondary-transparent');
                //element.innerText = 'REVIEWED CASES';
                break;
            case 2:
                $('#tb-cases__title').text('PROCESSED CASES');
                $('#tb-cases__title').addClass('bg-warning-transparent');
                //element.innerText = 'PROCESSED CASES';
                break;
            case 3:
                $('#tb-cases__title').text('CLOSED CASES');
                $('#tb-cases__title').addClass('bg-success-transparent');
                //element.innerText = 'CLOSED CASES';
                break;
            default:
                //element.innerText = '';
                break;

        }

        $('#tb-cases').DataTable({
            ajax: {
                url: baseApiUrlEndPoint + '/Home/dashboard/tramites',
                type: 'POST',
                data: (params) => {
                    var nParams = {
                        ...params,
                        AdditionalValues: {
                            AdminId: userAuth.id,
                            TipoTramite: parseInt($('#tipo-tramite').val()),
                            Estado: parseInt(status),
                            NombreTramite: $('#nombre-tramite').val(),
                            CodigoPago: $('#codigo-pago').val(),
                            EstadoProceso: $('#EstadoProceso').val(),
                        }
                    }

                    return nParams;
                },
            },
            columns: [
                {
                    data: 'pagoCodigo',
                    name: 'Código Pago',
                    render: (data, type, row, meta) => {
                        if (row.estadoProceso === 2) {
                            return '<span class="fw-semibold text-warning">' + row.pagoCodigo +'</span>';
                        } else {
                            return '<span class="fw-semibold text-primary">' + row.pagoCodigo +'</span>';
                        }
                    }
                },
                { data: 'tipoTramite', name: 'Código Pago' },
                { data: 'nombreTramite', name: 'Código Pago' },
                {
                    data: 'nombreCliente', render: (data, type, row, meta) => {
                        return renderCellCustomer(data, row.correo, row.telefono);
                    }
                },
                {
                    data: 'pagoFecha',
                    render: renderCellDateTime,
                },

                {
                    data: 'statusEvaluacion',
                    render: (data, type, row, meta) => renderCellStatus(data, row.doctor) // Aquí se usa la nueva función
                },
                {
                    data: 'id', render: (data, type, row, meta) => {
                        var html = '';
                        if (status === 0) {
                            var url = '';
                            switch (row.trId) {
                                case 3:
                                    url = 'Home/VerFormDupliLicNew/' + row.clienteId + '/' + row.trId + '/' + row.id +'?status=0';
                                    break;
                                case 4:
                                    url = 'Home/VerFormLicReciprocidadNew/' + row.clienteId + '/' + row.trId + '/' + row.id +'?status=0';
                                    break;
                                case 5:
                                    url = 'Home/EditarTraspaso/'+row.id + '?status=0';
                                    break;
                                default:
                                    url = 'Home/VerFormulario/' + row.clienteId + '/' + row.trId + '/' + row.id+'?status=0';
                            }
                            html += '<a class="btn btn-icon btn-sm btn-info-light btn-wave waves-effect waves-light me-2 " href="' + url + '"> <i class="ri-edit-box-line"></i></a>';
                           /* html += '<a class="btn btn-icon btn-sm btn-success-light btn-wave waves-effect waves-light me-2 " href="Home/FormCertificadoMed/' + row.clienteId + '/' + row.trId+'"> <i class="ri-edit-2-line"></i></a>';*/
                        }

                        if (status === 1) {
                            var url = '';
                            switch (row.trId) {
                                case 3:
                                    url = 'Home/VerFormDupliLicReview/' + row.clienteId + '/' + row.trId + '/' + row.id + '?status=1';
                                    break;
                                case 4:
                                    url = 'Home/VerFormLicReciprocidadReview/' + row.clienteId + '/' + row.trId + '/' + row.id + '?status=1';
                                    break;
                                case 5:
                                    url = 'Home/EditarTraspaso/' + row.id + '?status=1';
                                    break;
                                default:
                                    url = 'Home/FormRenovLicReview/' + row.clienteId + '/' + row.trId + '/' + row.id + '?status=1';
                            }
                            html += '<a class="btn btn-icon btn-sm btn-info-light btn-wave waves-effect waves-light me-2 " href="' + url + '"> <i class="ri-edit-2-line"></i> </a>';
                        }

                        if (status === 2) {
                            var url = 'Home/ProcessForm/' + row.clienteId + '/' + row.trId + '/' + row.id + '?status=2';

                            if (row.trId === 5) {
                                url = 'Home/VerTraspaso/' + row.id + '?status=2';
                            }

                            html += '<a class="btn-preview btn btn-wave btn-primary-light btn-icon btn-sm" href="' + url + '"><i class="ri-eye-line"></i> </a>';
                        }

                        if (status === 3) {
                            var url = 'Home/ClosedForm/' + row.clienteId + '/' + row.trId + '/' + row.id + '?status=3';

                            if (row.trId === 5) {
                                url = 'Home/VerTraspaso/' + row.id + '?status=3';
                            }

                            html += '<a class="btn btn-icon btn-sm btn-info-light btn-wave waves-effect waves-light me-2 " href="' + url + '"> <i class="ri-edit-box-line"></i> </a>';
                        }

                        return html;
                    },
                }
            ],
            createdRow: function (row, data, dataIndex) {
                if (data.estadoProceso === 2) {
                   
                }
            }
        });
    },
    refresh() {
        $('#tb-cases').DataTable().ajax.reload();
    },
    resetFilters() {
        $('#codigo-pago').val('');
        $('#tipo-tramite').val(0);
        $('#nombre-tramite').val('');
        $('#EstadoProceso').val('99');
    }
};


function cargarDetalleCaso(tipoCaso) {
    const element = document.getElementById('formulario-id'); // Cambia 'element-id' por el ID de tu elemento

    // Reseteamos el texto
    element.innerText = '';

    switch (tipoCaso) {
        case 'new':
            element.innerText = 'NEW CASES';
            break;
        case 'inProcess':
            element.innerText = 'IN PROCESS';
            break;
        case 'processed':
            element.innerText = 'PROCESSED CASES';
            break;
        case 'closed':
            element.innerText = 'CLOSED CASES';
            break;
        default:
            element.innerText = '';
            break;
    }
}


document.addEventListener('DOMContentLoaded', () => {
    window['Home'] = new HomePage();
});