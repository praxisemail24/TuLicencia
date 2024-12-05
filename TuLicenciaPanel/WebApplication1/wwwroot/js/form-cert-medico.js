$(document).ready(function () {
    var cl_id = $('#cl_id').val();
    var tr_id = $('#tr_id').val();

    ///

    jQuery.ajax({
        url: baseApiUrlEndPoint + '/RenovLic/obtenerDatosForm/' + cl_id + '/' + tr_id,
        type: "GET",
        data: null,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            console.log(data);
            console.log(data.item);
            var item = data.item;

            function formatearFechaHora(fechaHoraFormateada) {
                if (fechaHoraFormateada == null) {
                    return null;
                }

                var partes = fechaHoraFormateada.split('T');
                var fechaParte = partes[0];
                var horaParte = partes[1].substring(0, 8);
                var fechaFormateada = fechaParte;
                var horaFormateada = horaParte;
                return fechaFormateada + ' / ' + horaFormateada;
            }
            var fechaHora = item.fecha;
            var fechaHoraFormateada = formatearFechaHora(fechaHora);

            var nombreCompleto = item.cl_cliente.cl_nombre + " " + item.cl_cliente.cl_primerApellido + " " + item.cl_cliente.cl_segundoApellido;

            var fechaConvictoBFormateada = formatearFechaHora(item.fechaConvictoBebida);
            var fechaConvictoNFormateada = formatearFechaHora(item.fechaConvictoNarcotico);


            $("#frmFechaConvictoBebida").val(fechaConvictoBFormateada);
            $("#frmFechaConvictoNarcotico").val(fechaConvictoNFormateada);


            $('#frm_id').val(item.frmID);
            $("#frmNombre").text(nombreCompleto);
            $("#frmCorreo").text(item.cl_cliente.cl_correo);
            $("#frmCodigoPago").text(item.cl_pago.pg_codigo);
            $("#frmTipoTramite").text(item.cl_cliente.cl_correo);
            $("#frmFecha").text(fechaHoraFormateada);

            $("#frmTipoLicencia").val(item.tipoLicencia);
            $("#frmNumeroLicencia").val(item.numeroLicencia);
            $("#frmCategoria").val(item.categoria);
            $("#frmVehiculoPesado").val(item.vehiculoPesado);
            $("#frmIdentificacion").val(item.identificacion);
            $("#frmNumero").val(item.numero);
            $("#frmStatusLegal").val(item.statusLegal);
            $("#frmGenero").val(item.genero);
            $("#frmDonante").val(item.donante);
            $("#frmTipoSangre").val(item.tipoSangre);
            $("#frmTalla").val(item.talla);
            $("#frmPeso").val(item.peso);
            $("#frmTez").val(item.tez);
            $("#frmColorPelo").val(item.colorPelo);
            $("#frmColorOjo").val(item.colorOjo);
            $("#frmDireccion").val(item.direccion);
            $("#frmNumeroDireccion").val(item.numeroDireccion);
            $("#frmPueblo").val(item.pueblo);
            $("#frmBarrio").val(item.barrio);
            $("#frmApartado").val(item.apartado);
            $("#frmPueblo2").val(item.pueblo2);
            $("#frmLicenciaSuspendida").val(item.licenciaSuspendida);
            $("#frmMotivoSuspension").val(item.motivoSuspension);
            $("#frmRecluido").val(item.recluido);
            $("#frmConvictoBebida").val(item.convictoBebida);
            //$("#frmFechaConvictoBebida").val(item.fechaConvictoBebida);
            $("#frmConvictoNarcotico").val(item.convictoNarcotico);
            //$("#frmFechaConvictoNarcotico").val(item.fechaConvictoNarcotico);
            $("#frmObligacionAlimentaria").val(item.obligacionAlimentaria);
            $("#frmDeudaAcca").val(item.deudaAcca);
            $("#frmCodigoPostal").val(item.codigoPostal);
            $("#frmCodigoPostal2").val(item.codigoPostal2);


            obtenerArchivo(item.frmID);
        },
        error: function (error) {
            console.log(error)
        },
    });

    $('#btnActualizar').click(function () {
        ActualizarFormulario();
    });

    $('#divImagen').on('click', '.btn-eliminar', function (e) {
        console.log(e);
        var id = e.currentTarget.id;

    });

    function ActualizarFormulario() {
        var FrmID = $('#frm_id').val();
        var TipoLicencia = $('#frmTipoLicencia').val();
        var NumeroLicencia = $('#frmNumeroLicencia').val();
        var Categoria = $('#frmCategoria').val();
        var VehiculoPesado = $('#frmVehiculoPesado').val();
        var Identificacion = $('#frmIdentificacion').val();
        var Numero = $('#frmNumero').val();
        var StatusLegal = $('#frmStatusLegal').val();
        var Genero = $('#frmGenero').val();
        var Donante = $('#frmDonante').val();
        var TipoSangre = $('#frmTipoSangre').val();
        var Talla = $('#frmTalla').val();
        var Peso = $('#frmPeso').val();
        var Tez = $('#frmTez').val();
        var ColorPelo = $('#frmColorPelo').val();
        var ColorOjo = $('#frmColorOjo').val();
        var Direccion = $('#frmDireccion').val();
        var NumeroDireccion = $('#frmNumeroDireccion').val();
        //var Pueblo = $('#ddlPueblos1').val().toString();
        var Pueblo = $('#frmPueblo').val().toString();
        var CodigoPostal = $('#frmCodigoPostal').val();
        var Barrio = $('#frmBarrio').val();
        var Apartado = $('#frmApartado').val();
        var Pueblo2 = $('#frmPueblo2').val();
        var CodigoPostal2 = $('#frmCodigoPostal2').val();
        var LicenciaSuspendida = $('#frmLicenciaSuspendida').val();
        var MotivoSuspension = $('#frmMotivoSuspension').val();
        var Recluido = $('#frmRecluido').val();
        var ConvictoBebida = $('#frmConvictoBebida').val();
        var FechaConvictoBebida = $('#frmFechaConvictoBebida').val();
        var ConvictoNarcotico = $('#frmConvictoNarcotico').val();
        var FechaConvictoNarcotico = $('#frmFechaConvictoNarcotico').val();
        var ObligacionAlimentaria = $('#frmObligacionAlimentaria').val();
        var DeudaAcca = $('#frmDeudaAcca').val();

        if (FechaConvictoBebida === "") {
            FechaConvictoBebida = null;
        }
        if (FechaConvictoNarcotico === "") {
            FechaConvictoNarcotico = null;
        }

        var ObjFormulario = {
            FrmID: FrmID,
            tr_id: tr_id,
            Estado: 0,
            NumeroLicencia: NumeroLicencia,
            TipoLicencia: TipoLicencia,
            Categoria: Categoria,
            VehiculoPesado: VehiculoPesado,
            Identificacion: Identificacion,
            Numero: Numero,
            StatusLegal: StatusLegal,
            Genero: Genero,
            Donante: Donante,
            TipoSangre: TipoSangre,
            Talla: Talla,
            Peso: Peso,
            Tez: Tez,
            ColorPelo: ColorPelo,
            ColorOjo: ColorOjo,
            Direccion: Direccion,
            NumeroDireccion: NumeroDireccion,
            Pueblo: Pueblo,
            CodigoPostal: CodigoPostal,
            Barrio: Barrio,
            Apartado: Apartado,
            Pueblo2: Pueblo2,
            CodigoPostal2: CodigoPostal2,
            LicenciaSuspendida: LicenciaSuspendida,
            MotivoSuspension: MotivoSuspension,
            Recluido: Recluido,
            ConvictoBebida: ConvictoBebida,
            FechaConvictoBebida: FechaConvictoBebida,
            ConvictoNarcotico: ConvictoNarcotico,
            FechaConvictoNarcotico: FechaConvictoNarcotico,
            ObligacionAlimentaria: ObligacionAlimentaria,
            DeudaAcca: DeudaAcca,
        };
        console.log("obj", ObjFormulario)
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/RenovLic/actualizarDatosForm/',
            type: 'PUT',
            data: JSON.stringify(ObjFormulario),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log(data);
                if (data.success) {
                    Swal.fire({
                        title: "Actualizado!",
                        text: data.message,
                        icon: "success",
                        timer: 2500
                    });
                    // llenarTabla(); actualizar la tabla????? como
                    $('#exampleModal').modal('hide');
                } else {
                    toastr.error(data.message);
                }
            },
        });

    }

    function obtenerArchivo(frmId) {
        $('#divImagen').empty();
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Archivo/' + frmId,
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                data.items.forEach((t, i) => {
                    var descripcion = "foto " + (i + 1);
                    var html = '<div class="col-xxl-2 col-xl-4 col-lg-3 col-md-3 col-sm-12">' +
                        '<div class="card custom-card hrm-main-card primary team-member-card">' +
                        '<div class="teammember-cover-image"> ' +
                        '<img src="' + t.ar_nombre + '" class="card-img-top" alt="..."> ' +
                        '</div>' +
                        '<div class="card-body p-0">' +
                        '<div class="d-flex flex-wrap align-item-center mt-sm-0 mt-5 justify-content-between border-bottom border-block-end-dashed p-3">' +
                        '</div>' +
                        '<div class="team-member-stats d-sm-flex justify-content-evenly">' +
                        '<div class="text-center p-3 my-auto">' +
                        '<p class="fw-bold mb-0">' + descripcion + '</p>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '<div class="card-footer border-block-start-dashed text-center">' +
                        '<div class="btn-list">' +
                        '<div class="btn-list">' +
                        '<a href="job-details.html" class="btn btn-icon btn-sm btn-primary-light btn-wave waves-effect waves-light btn-lg"><i class="ri-eye-line"></i></a>' +
                        // '<a href="javascript:void(0);" class="btn btn-icon btn-sm btn-info-light btn-wave waves-effect waves-light btn-lg"><i class="ri-edit-line"></i></a>' +
                        '<a href ="javascript:void(0);" id="' + t.ar_id + '" class="btn-eliminar btn btn-icon btn-sm btn-danger-light btn-wave waves-effect waves-light btn-lg"><i class="ri-delete-bin-line"></i></a>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                    $('#divImagen').append(html);
                });
            },
            error: function (error) {
                console.log(error)
            },
        });
    }
});