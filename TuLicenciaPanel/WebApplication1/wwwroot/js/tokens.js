var TokenManager = function () {
    configDataTables();
}

TokenManager.prototype = {
    copy(text) {
        navigator.clipboard.writeText(text).then((r) => {
            toastr.info('Copiado!');
        });
    },
    loadTable() {
        $('#tb-tokens').DataTable({
            destroy: true,
            paging: true,
            processing: true,
            serverSide: true,
            responsive: true,
            language: {
                url: '/es-MX.json',
            },
            searching: true,
            ajax: {
                url: baseApiUrlEndPoint + '/Token/',
                type: 'POST',
                dataType: 'json',
            },
            columns: [
                { data: 'userName', name: 'Usuario' },
                {
                    data: 'accessToken', name: 'Token',
                    render: (data, type, row, meta) => {
                        return '<div class="d-flex align-items-center" style="flex-direction: row;"><code style="margin-right: 5px;">' + data.substring(0, 20) + '...</code><button class="btn btn-sm btn-info" onclick="apiTokens.copy(\'' + data + '\')" type="button"><i classs="ri-clipboard-fill"></i><span>Copiar</span></button></div>';
                    }
                },
                { data: 'expiredAt', name: 'F. Vencimiento', render: renderCellDateTime },
                { data: 'revokedAt', name: 'F. Revocación', render: renderCellDateTime },
                {
                    data: 'useOrigin', name: 'Usuario', render: (data, type, row, meta) => {
                        switch (data) {
                            case 1:
                                return 'Administrador';
                            case 2:
                                return 'Operador';
                            case 3:
                                return 'Radicador';
                            case 4:
                                return 'Cliente';
                            default:
                                return 'Desconocido';
                        }
                    }
                },
                {
                    data: 'id',
                    name: 'Id',
                    render: (data, type, row, meta) => {
                        return row.revokedAt != null ? '' : '<button onclick="apiTokens.Revoke(' + data + ')" type="button" class="btn btn-sm btn-danger hint hint--bounce hint--top" data-hint="Revocar"><i class="ri-delete-row"></i></button>';
                    }
                },
            ]
        });
    },
    refreshTable() {
        $('#tb-tokens').DataTable().ajax.reload();
    },
    New() {
        $('#username').val('');
        $('#userId').val('');
        $('#rol').val('0');
        $('#expiredAt').val(null);
        $('#modalNewToken').modal('show');
    },
    Generate() {
        var rol = $('#rol').val();
        $.ajax({
            type: 'POST',
            url: baseApiUrlEndPoint + '/Token/Create/' + rol,
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify({
                UserName: $('#username').val(),
                ExpiredAt: $('#expiredAt').val(),
                UserId: $('#userId').val(),
            }),
            success: (response) => {
                if (response.success) {
                    toastr.success('Se ha generado correctamente el token.');
                    apiTokens.refreshTable();
                    $('#modalNewToken').modal('hide');
                } else {
                    toastr.error(response.message);
                }
            }
        })
    },
    Revoke(id) {
        Swal.fire({
            title: "Revocar token",
            text: "¿Desea revocar el token?",
            showCancelButton: true,
            confirmButtonText: "Revocar",
            cancelButtonText: "Cancelar",
        }).then(r => {
            if (r.isConfirmed) {
                $.ajax({
                    type: 'DELETE',
                    url: baseApiUrlEndPoint + '/Token/Revoked/' + id,
                    contentType: 'application/json',
                    dataType: 'json',
                    success: (response) => {
                        if (response.success) {
                            apiTokens.refreshTable();
                        }
                    }
                })
            }
        })
    }
}

var apiTokens = new TokenManager();