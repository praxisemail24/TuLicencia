String.prototype.lpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = padString + str;
    return str;
}

var MultaConfig = function () {
    this.loadedInfo = false;
}

MultaConfig.prototype = {
    getTramiteId() {
        return $('#tr_id').val();
    },
    getFormularioId() {
        return $('#frm_id').val();
    },
    getClienteId() {
        return $('#cl_id').val();
    },
    httpAsync(trId, frmId, clId, resolve, reject) {
        $.ajax({
            url: baseApiUrlEndPoint + '/Multa/Info/' + trId + '/' + frmId + '/' + clId,
            success(response) {
                resolve(response);
            },
            error(xhr) {
                reject(xhr);
            }
        });
    },
    asyncInfo() {
        return new Promise((resolve, reject) => {
            var trId = parseInt(this.getTramiteId());
            var frmId = parseInt(this.getFormularioId());
            var clId = parseInt(this.getClienteId());

            if (trId > 0 && trId !== NaN && frmId > 0 && frmId !== NaN && clId > 0 && clId !== NaN) {
                this.httpAsync(trId, frmId, clId, resolve, reject);
            } else {
                setTimeout(() => {
                    this.httpAsync(trId, frmId, clId, resolve, reject);
                }, 3000);
            }
        })
    },
}
var ModalMulta = function (origen) {
    this.origen = origen;
    this.$containerEl = $('#modal-' + origen);
    this.$modalPayment = $('#modal-payment');
    this.$config = new MultaConfig()
    this.multas = [];
    this.certificados = [];
    this.cuotas = [];
    this.total = 0;
    this.pago = null;

    if (this.$containerEl.length > 0) {
        setTimeout(() => {
            this.initialize()
        }, 1500)
    }
}

ModalMulta.prototype = {
    initialize() {
        console.log('Modal iniciado ' + this.origen + '...');

        $('input[name="rbtnmultas"]', this.$containerEl).on('click', (e) => {
            var val = parseInt(e.target.value);
            if (val === 1) {
                //$('.form-multas', this.$containerEl).removeClass('d-none');
                $('button[data-action="subir"]', this.$containerEl).removeClass('d-none');
                $('.form-pagos', this.$containerEl).removeClass('d-none');

                $('.form-pagos-no', this.$containerEl).removeClass('d-none');
                $('.form-pagos-yes', this.$containerEl).removeClass('d-none');
                $('#multas-list-cesco').collapse('show');
            } else {
                //$('.form-multas', this.$containerEl).addClass('d-none');
                $('button[data-action="subir"]', this.$containerEl).addClass('d-none');
                $('.form-pagos', this.$containerEl).addClass('d-none');

                $('.form-pagos-no', this.$containerEl).addClass('d-none');
                $('.form-pagos-yes', this.$containerEl).addClass('d-none');
            }
            $('button[data-action="submit"]', this.$containerEl).removeClass('d-none');
        });

        $('input[name="rbtnpagos"]', this.$containerEl).on('click', (e) => {
            var val = parseInt(e.target.value);
            if (val === 1) {
                $('.form-pagos-yes', this.$containerEl).removeClass('d-none');
                $('.form-pagos-no', this.$containerEl).addClass('d-none');
                $('button[data-action="submit"]', this.$containerEl).removeClass('d-none');
            } else {
                $('.form-pagos-yes', this.$containerEl).addClass('d-none');
                $('.form-pagos-no', this.$containerEl).removeClass('d-none');
                $('button[data-action="submit"]', this.$containerEl).addClass('d-none');
            }
        });

        if (this.origen === 'cesco') {
            $('#select-cuotas', this.$containerEl).on('change', (e) => {
                //$('input[name="rbtnpagocuota"]', this.$containerEl).on('click', (e) => {
                var val = parseInt(e.target.value);

                var tbody = $('.cuotas table tbody', this.$containerEl);
                tbody.empty();
                //$('.cuotas', this.$containerEl).empty();

                //var value = this.total / val;

                var primeraCuota = this.total * 0.2;
                // Calcular el resto del total menos la primera cuota
                var restoTotal = this.total - primeraCuota;
                // Calcular el monto de las cuotas restantes de forma proporcional
                var cuotaProporcional = restoTotal / (val - 1);

                for (var i = 0; i < val; i++) {
                
                    if (this.cuotas[i] === undefined) {
                        var date = new Date();
                        var newDate = new Date(date.setMonth(date.getMonth() + i));
                        this.cuotas.push({
                            fecha: newDate,
                            nroCuota: i + 1,
                            monto: i === 0 ? parseFloat(primeraCuota) : parseFloat(cuotaProporcional),
                            multaPagoId: 0,
                        });
                    } else {
                        this.cuotas[i].monto = i === 0 ? parseFloat(primeraCuota) : parseFloat(cuotaProporcional);
                    }

                    var row = $('<tr />');
                    var cuotaCell = $('<td />').text( (i + 1));
                    var montoCell = $('<td />').text(this.cuotas[i].monto.toFixed(2));
                    row.append(cuotaCell);
                    row.append(montoCell);
                    tbody.append(row);

                }
            });

            $('input[name="rbtnpagotipo"]', this.$containerEl).on('click', (e) => {
                var tipo = parseInt(e.target.value);
                $('.pago-total', this.$containerEl).addClass('d-none');
                $('.pago-parcial', this.$containerEl).addClass('d-none');

                if (tipo === 1) {
                    $('.pago-total', this.$containerEl).removeClass('d-none');
                } else if (tipo === 2) {
                    $('.pago-parcial', this.$containerEl).removeClass('d-none');
                }
            });
        }
        $('button[data-action="finish"]', this.$containerEl).on('click', () => this.$containerEl.modal('hide'));
        $('button[data-action="agregar"]', this.$containerEl).on('click', (e) => this.addPenaltyFee(e));
        $('button[data-action="pay-gateway"]', this.$containerEl).on('click', (e) => this.createModalPayment(e));
        $('button[data-action="subir"]', this.$containerEl).on('click', (e) => this.uploadFile(e));
        $('button[data-action="pagar"]', this.$containerEl).on('click', (e) => this.payed(e));
        $('button[data-action="plan-pago"]', this.$containerEl).on('click', (e) => {
            this.planPayed(e).then(() => {
                this.$containerEl.modal('hide');
            })
        });
        $('button[data-action="correo"]', this.$containerEl).on('click', (e) => {
            this.SendMail(e.target.getAttribute('data-mail-type'));
        });

        this.$containerEl.on('shown.bs.modal', () => {
            this.loadInfo();
        });

        this.$modalPayment.on('hide.bs.modal', () => {
            this.loadInfo();
        });

        this.loadInfo();
    },
    loadInfo() {
        return new Promise((resolve, reject) => {
            this.$config.asyncInfo().then((info) => {
                this.setInfo(info);

                resolve(info);
            }).catch(error => {
                reject(error);
            })
        }).then((info) => {
            // Añadir info como parámetro aquí
            // Aquí se ejecuta después de cargar la información y establecer los datos

            // Seleccionar botón de radio de tipo de pago parcial si es necesario
            if (info.tipoPago === "Parcial" && this.origen === "cesco") {
                $("#rbtnpagoparcial").prop('checked', true);
                $('.pago-total', this.$containerEl).addClass('d-none');
                $('.pago-parcial', this.$containerEl).removeClass('d-none');
            }

            // Seleccionar botón de radio de pago Cesco si ya ha sido pagado
            if (info.pagoCesco > 0 && this.origen === "cesco") {
                $("#rbtnpago-si1").prop('checked', true);
                $('.form-pagos-yes', this.$containerEl).removeClass('d-none');
                $('.form-pagos-no', this.$containerEl).addClass('d-none');
                $('button[data-action="submit"]', this.$containerEl).removeClass('d-none');

                $("#pago1").css({ display: 'none' })
                $("#pago1si").css({ display: 'none' })
                $("#pago1no").css({ display: 'none' });

            }

            // Seleccionar botón de radio de pago Auto Express si ya ha sido pagado
            if (info.pagoAutoExpress > 0 && this.origen !== "cesco") {
                $("#rbtnpago-si2").prop('checked', true);
                $('.form-pagos-yes', this.$containerEl).removeClass('d-none');
                $('.form-pagos-no', this.$containerEl).addClass('d-none');
                $('button[data-action="submit"]', this.$containerEl).removeClass('d-none');

                $("#pago2").css({ display: 'none' })
                $("#pago2si").css({ display: 'none' })
                $("#pago2no").css({ display: 'none' });

            }
        });
    },
    setInfo(obj) {
        $('.nombre span', this.$containerEl).text(obj.nombreCliente);
        $('.nro-licencia span', this.$containerEl).text(obj.nroLicencia);
        $('.nro-ssn span', this.$containerEl).text(obj.nroSSN);
        $('.fecha-nac span', this.$containerEl).text(obj.fechaNac);
        $('.correo span', this.$containerEl).text(obj.correo);
        //$('#estadocesco', this.$containerEl).text(obj.PagoCesco);
        //$('#estadoautoexpressn', this.$containerEl).text(obj.PagoAutoExpress);

        $("#estadocesco").text(obj.pagoCesco > 0 ? "Pago Cesco Completado" : "Pago Cesco Pendiente");
        $("#estadoautoexpress").text(obj.pagoAutoExpress > 0 ? "Pago Auto Express Completado" : "Pago Auto Express Pendiente");

        if (obj.tipoPago === "Parcial" && this.origen === "cesco") {
            // Seleccionamos el botón de radio
            $("#rbtnpagoparcial").prop('checked', true);
            $('.pago-parcial', this.$containerEl).removeClass('d-none');
        } 

        if (obj.pagoCesco > 0 && this.origen === "cesco") {
            // Seleccionamos el botón de radio
            $("#rbtnpago-si1").prop('checked', true);
            $('.form-pagos-yes', this.$containerEl).removeClass('d-none');
            $('.form-pagos-no', this.$containerEl).addClass('d-none');
            $('button[data-action="submit"]', this.$containerEl).removeClass('d-none');
           }       

        if (obj.pagoAutoExpress > 0 && this.origen !== "cesco") {
            // Seleccionamos el botón de radio
            $("#rbtnpago-si2").prop('ckecked', true);
            $('.form-pagos-yes', this.$containerEl).removeClass('d-none');
            $('.form-pagos-no', this.$containerEl).addClass('d-none');
            $('button[data-action="submit"]', this.$containerEl).removeClass('d-none');
        } 

        this.multas = obj.multas.filter(x => x.origen === this.origen);
        this.certificados = obj.archivos.filter(x => x.origen === this.origen);
        this.cuotas = obj.pago === null ? [] : obj.pago.cuotas;
        this.pago = { Id: 0 };

        if (obj.pago !== null) {
            this.pago = obj.pago
        }

        if (this.multas.length > 0) {
            $('input[name="rbtnmultas"][value="1"]', this.$containerEl).trigger('click');
        } else {
            $('input[name="rbtnmultas"][value="0"]', this.$containerEl).trigger('click');
        }

        if (this.cuotas.length > 0) {
            $('input[name="rbtnpagos"][value="1"]', this.$containerEl).trigger('click');

            if (this.cuotas.conPagoTotal) {
                $('input[name="rbtnpagotipo"][value="1"]', this.$containerEl).trigger('click');
            } else {
                $('input[name="rbtnpagotipo"][value="2"]', this.$containerEl).trigger('click');
            }

            //setTimeout(() => {
            //    $('input[name="rbtnpagocuota"][value="' + this.cuotas.length + '"]', this.$containerEl).trigger('click');
            //}, 700)
            setTimeout(() => {
                $('#select-cuotas', this.$containerEl).val(this.cuotas.length).trigger('change');
            }, 700)


        } else {
            $('input[name="rbtnpagotipo"][value="1"]', this.$containerEl).trigger('click');
            $('input[name="rbtnpagos"][value="0"]', this.$containerEl).trigger('click');
        }

        $('#tb-multas').bootstrapTable('destroy').bootstrapTable({
            height: 250,
            data: obj.multas,
            columns: [
                [
                    {
                        title: 'DESCRIPCIÓN',
                        field: 'descripcion',
                        checkbox: false,
                        rowspan: 1,
                        align: 'left',
                        valign: 'middle'
                    },
                    {
                        title: 'ORIGEN',
                        field: 'origen',
                        checkbox: false,
                        rowspan: 1,
                        align: 'left',
                        valign: 'middle'
                    },
                    {
                        title: 'MONTO',
                        field: 'monto',
                        checkbox: false,
                        rowspan: 1,
                        align: 'left',
                        valign: 'middle'
                    },
                    {
                        title: '',
                        field: 'id',
                        checkbox: false,
                        rowspan: 1,
                        align: 'left',
                        valign: 'middle',
                        formatter: () => '',
                    },
                ]
            ]
        });

        this.renderListMultas();
        this.renderListCertificados();
    },
    renderListMultas() {
        //var container = $('.form-multas-lista ul', this.$containerEl);
        var container = $('.form-multas-lista table tbody', this.$containerEl);
        container.empty();
        console.log(this.multas);

        this.total = 0;
        if (this.multas.length > 0) {
            container.closest('.form-multas-lista').removeClass('d-none');

            for (var i = 0; i < this.multas.length; i++) {

                var row = $('<tr />');
                var descripcionCell = $('<td />').text(this.multas[i].descripcion);
                var montoCell = $('<td />').text(this.multas[i].monto);
                var accionesCell = $('<td />');
                var idmulta = this.multas[i].id;
                var deleteButton = $('<button />', {
                    class: 'btn btn-sm btn-danger',
                    click: (e) => {
                        e.preventDefault();
                        this.deleteMulta(idmulta);
                    } // Asegúrate de usar la función deleteMulta del contexto de ModalMulta
                });
                var deleteIcon = $('<i />', {
                    class: 'si-trash' // Clase del icono de tacho de basura
                });
                deleteButton.append(deleteIcon);
                accionesCell.append(deleteButton);
                row.append(descripcionCell);
                row.append(montoCell);
                row.append(accionesCell);
                container.append(row);

                this.total += parseFloat(this.multas[i].monto);
            }
        } else {
            container.closest('.form-multas-lista').addClass('d-none');
        }

        $('.total', this.$containerEl).empty();
        var strong = $('<strong />');
        strong.text('Total');
        var span = $('<span />', { class: 'ms-2 total-value', id: 'total-value' });
    

        span.text(this.total);
        $('.total', this.$containerEl).addClass('mb-3');
        $('.total', this.$containerEl).append(strong);
        $('.total', this.$containerEl).append(span);


        $('.total2', this.$containerEl).empty();
        var strong = $('<strong />');
        strong.text('Total');
        var span = $('<span />', { class: 'ms-2', id: 'total-value2' });


        span.text(this.total2);
        $('.total2', this.$containerEl).addClass('mb-3');
        $('.total2', this.$containerEl).append(strong);
        $('.total2', this.$containerEl).append(span);

        $('#select-cuotas', this.$containerEl).val('3').trigger('change');
        this.calculateTotalParcial();
        //$('input[name="rbtnpagocuota"][value="3"]', this.$containerEl).trigger('click');
    },

    updateTotalParcial() {
        $('.total-parcial', this.$containerEl).empty();
        var strong = $('<strong />').text('Pago Parcial: ');
        var span = $('<span />', { class: 'ms-2 total-parcial-value', id: 'total-parcial-value' }).text(this.totalParcial.toFixed(2));
        $('.total-parcial', this.$containerEl).append(strong).append(span);
    }
    , calculateTotalParcial() {
        // Aquí debes definir cómo calcular el total parcial.
        // Ejemplo simple (ajusta según tu lógica):

        this.totalParcial = this.total * 0.2; // Cambia según tu lógica
        this.updateTotalParcial();
    }
,

    deleteMulta(id) {

        console.log('ID::::::::::::', id);

        if (document.getElementById("estadocesco").innerHTML === "Pago Cesco Completado" && this.origen === "cesco") {
            toastr.error("Multa ya pagada, no se puede borrar");
            return;
        }

        if (document.getElementById("estadoautoexpress").innerHTML === "Pago Auto Express Completado" && this.origen === "autoexpress") {
            toastr.error("Multa ya pagada, no se puede borrar");
            return;
        }

        var cr = confirm("Desea eliminar multa?");

        if (cr) {
            var multa = {
                id: id,
                tramiteId: "0", // this.$config.getTramiteId(),
                formularioId: "0", // this.$config.getFormularioId(),
                clienteId: "0", // this.$config.getClienteId(),
                origen: "string", // this.origen,
                descripcion: "string",
                monto: 0,
                estado: "string", // 'eliminada',
                autorId: 0, // userAuth.id,
                fechaCreacion: "2024-07-20T14:01:28.034Z",
                ultimaActualizacion: "2024-07-20T14:01:28.034Z"
            };

            console.log("Datos de la multa que se enviarán:", multa);


            $.ajax({
                url: baseApiUrlEndPoint + '/Multa/StoreBorra', // URL del endpoint para eliminar
                type: 'POST', // Tipo de solicitud
                contentType: 'application/json',
                data: JSON.stringify(multa), // Enviar la estructura similar a Store
            }).then(response => {
                if (response.success) {
                    toastr.success(response.message);

                    // Eliminar la multa del array de multas local
                    this.multas = this.multas.filter(multa => multa.id !== id);

                    // Renderizar la lista de multas actualizada
                    this.renderListMultas();
                } else {
                    toastr.error(response.message);
                }
            }).fail(jqXHR => {
                toastr.error('Error en la solicitud: ' + jqXHR.statusText);
            });
        }
    },
    renderListCertificados() {
        var container = $('.form-multas-certificados ul', this.$containerEl);
        container.empty();
        if (this.certificados.length > 0) {
            container.closest('.form-multas-certificados').removeClass('d-none');

            for (var i = 0; i < this.certificados.length; i++) {
                var node = $('<li />', { class: 'list-group-item d-flex flex-row' });
                var link = $('<a />', { style: 'width: 100%', href: this.certificados[i].url, target: '_blank' });
                link.text(this.certificados[i].nombreArchivo);
                node.append(link);
                container.append(node);
            }
        } else {
            container.closest('.form-multas-certificados').addClass('d-none');
        }
    },
    addPenaltyFee(e) {
        var descripcion = $('input[data-input="descripcion"]', this.$containerEl).val();
        var monto = $('input[data-input="monto"]', this.$containerEl).val();

        if (!descripcion || !monto) {
            toastr.error('Se requiere que ingrese descripción y monto.');
            return;
        }

        var multa = {
            id: 0,
            tramiteId: this.$config.getTramiteId(),
            formularioId: this.$config.getFormularioId(),
            clienteId: this.$config.getClienteId(),
            origen: this.origen,
            descripcion,
            monto,
            estado: 'pendiente',
            autorId: userAuth.id,
            fechaCreacion: null,
            ultimaActualizacion: null,
        };

        $.ajax({
            url: baseApiUrlEndPoint + '/Multa/Store',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(multa),
        }).then(response => {
            if (response.success) {
                toastr.success(response.message);

                $('input[data-input="descripcion"]', this.$containerEl).val('');
                $('input[data-input="monto"]', this.$containerEl).val('');

                this.multas.push(multa);
                this.renderListMultas();
            } else {
                toastr.error(response.message);
            }
        });
    },
    uploadFile(e) {
        var $el = $('input[data-input="certificado"]', this.$containerEl).get(0);
        if ($el) {
            if ($el.files.length === 0) {
                toastr.error('Se requiere que seleccione un archivo.');
                return;
            }

            var formData = new FormData();
            formData.append("tramiteId", this.$config.getTramiteId());
            formData.append("formularioId", this.$config.getFormularioId());
            formData.append("autorId", userAuth.id);
            formData.append('certificado', $el.files[0]);
            formData.append('origen', this.origen);

            $.ajax({
                url: baseApiUrlEndPoint + '/Multa/SubirCertificado/' + this.$config.getTramiteId() + '/' + this.$config.getFormularioId(),
                type: 'POST',
                dataType: "json",
                data: formData,
                cache: false,
                contentType: false,
                processData: false
            }).then((response) => {
                if (response.success) {
                    toastr.success(response.message);

                    $el.value = '';

                    this.loadInfo();
                } else {
                    toastr.error(response.message);
                }
            });
        }
    },
    payed(e) {

    },
    planPayed(e) {
        var pago = {
            ...this.pago,
            origen: this.origen,
            formularioId: this.$config.getFormularioId(),
            tramiteId: this.$config.getTramiteId(),
            clienteId: this.$config.getClienteId(),
            total: this.total,
            autorId: userAuth.id,
            cantidadCuotas: this.cuotas.length,
            cuotas: this.cuotas,
            tipo: "parcial",
            pagado: parseInt($('input[name="rbtnpagos"]').val()) === 1,
            CodigoPago: '',
        }

        console.dir(pago);

        return new Promise((resolve, reject) => {
            Swal.fire({
                title: "¿Desea guardar el plan de pagos?",
                showDenyButton: false,
                showCancelButton: true,
                confirmButtonText: "Si",
                cancelButtonText: "Cancelar",
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: baseApiUrlEndPoint + '/Multa/PlanPago',
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json',
                        data: JSON.stringify(pago),
                    }).then((response) => {
                        if (response.success) {
                            toastr.success(response.message);

                            resolve();
                        } else {
                            toastr.error(response.message);
                        }
                    }).catch((error) => {
                        reject(error);
                    });
                }
            });
        });
    },
    finish(e) {
        var is = this
        return new Promise((resolve) => {
            console.dir(is.origen)
            if (is.origen === 'cesco') {
                $.ajax({
                    url: baseApiUrlEndPoint + '/Multa/StoreCuota',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(this.cuotas),
                }).then((response) => {
                    if (response.success) {
                        toastr.success(response.message);

                        resolve();
                    } else {
                        toastr.error(response.message);
                    }
                });
            } else {
                resolve();
            }
        });
    },
    SendMail(correo) {
        return new Promise((resolve) => {
            if (correo === null) {
                correo = 0
            }
            $.ajax({
                url: baseApiUrlEndPoint + '/Multa/EnviarCorreo',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Correo: correo,
                    TramiteId: this.$config.getTramiteId(),
                    FormularioId: this.$config.getFormularioId(),
                    ClienteId: this.$config.getClienteId(),
                    Origen: this.origen,
                }),
            }).then((response) => {
                if (response.success) {
                    toastr.success(response.message);

                    resolve();
                } else {
                    toastr.error(response.message);
                }
            })
        })
    },
    createModalPayment(e) {
        var tipo = $(e.target).attr('data-pay-type');
        var total = $('.total-value', this.$containerEl).text();
        var pagar = tipo === 'total' ? $('.total-value', this.$containerEl).text() : $('.total-parcial-value', this.$containerEl).text();

        total = parseFloat(total);
        pagar = parseFloat(pagar);

        var installments = "1";

        if (tipo === 'parcial') {
            installments = this.cuotas.map((c) => {
                return {
                    id: c.multaPagoId,
                    index: c.nroCuota,
                    date: c.fecha,
                    amount: c.monto,
                };
            });
        }

        var p = {
            penaltyType: tipo,
            penaltyOrigin: this.origen,
            frmId: this.$config.getFormularioId(),
            clId: this.$config.getClienteId(),
            trId: this.$config.getTramiteId(),
            name: `Pago de Multas ${this.origen.toUpperCase()}`,
            description: tipo === 'total' ? `Pago total de multas.` : `Se ha pagado ${pagar.toFixed(2)} USD de ${total.toFixed(2)} USD.`,
            amount: pagar,
            installments,
        }

        console.log('PreparedPayment', p);

        StripePay().embed($('.modal-body', this.$modalPayment).get(0), p);
        $(this.$modalPayment).modal('show', { backdrop: 'static', keyboard: false });
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new ModalMulta('cesco');
    new ModalMulta('autoexpress');
});