$(document).ready(function () {

    // Función para verificar si ambas condiciones son verdaderas
    //function verificarCondiciones() {
    //    var formularioRevisado = $('input[name="radioRevision"]:checked').val() === '1';
    //    var multasRevisadas = $('input[name="radioRevisionmulta"]:checked').val() === '1';

    //    if (formularioRevisado && multasRevisadas) {
    //        // Activar la sección de asignación
    //        $('#btnConfirmarCaso').prop('disabled', false);
    //        $('#selecRadicador').prop('disabled', false);
    //    } else {
    //        // Desactivar la sección de asignación
    //        $('#btnConfirmarCaso').prop('disabled', true);
    //        $('#selecRadicador').prop('disabled', true);
    //    }
    //}

    //// Bloquear el botón de asignación y el combo de radicadores por defecto
    //$('#btnConfirmarCaso').prop('disabled', true);
    //$('#selecRadicador').prop('disabled', true);

    //// Verificar condiciones al cargar la página
    //verificarCondiciones();

    //// Escuchar cambios en los radio buttons
    //$('input[name="radioRevision"], input[name="radioRevisionmulta"]').change(function () {
    //    verificarCondiciones();
    //});

    var pgId = 0;

    const urlParams = new URLSearchParams(window.location.search);
    const status = urlParams.get('status');
    const estadoCasoSection = $('.card:has(#inlineRadio1)');
    const element = $('#formulario-id'); // Reemplaza con el ID real del elemento donde quieres mostrar el texto
    switch (status) {
        case '0':
            element.text('NEW CASES');
            break;
        case '1':
            element.text('REVIEWED CASES');
            estadoCasoSection.hide();
            break;
        case '2':
            element.text('PROCESSED CASES');
            break;
        case '3':
            element.text('CLOSED CASES');
            break;
        default:
            element.text(''); // Dejar vacío si no se reconoce el estado
            break;
    }

    $('#divMotivoSuspension').hide();
    //$('input[name="btnradioSuspendido"]').change(function () {
    //    if ($(this).val() === 'Sí') {
    //        $('#divMotivoSuspension').show();
    //    } else if ($(this).val() === 'No') {
    //        $('#divMotivoSuspension').hide();
    //    }
    //});
    $('#btnSelectSuspension').change(function () {
        if ($(this).is(':checked')) {
            $('#divMotivoSuspension').show();
        } else {
            $('#divMotivoSuspension').hide();
        }
    });

    $('#frm_paisProcede').change(function () {
        var selectedOption = $(this).val();

        console.log('Opción seleccionada:', selectedOption);
        if (selectedOption == "Puerto Rico") {
            $('#ddlPueblos3').next('.select2-container').show();
            $('#ddlEstados1').next('.select2-container').hide();
        }
        else {
            $('#ddlEstados1').next('.select2-container').show();
            $('#ddlPueblos3').next('.select2-container').hide();
        }
    });


    //GET PUEBLOS
    jQuery.ajax({
        url: baseApiUrlEndPoint + '/Pueblos',
        type: "GET",
        data: null,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            data.items.forEach((t, i) => {
                $('#ddlPueblos1').append("<option value='" + t.pl_id + "' style='height: 30px;'> " + t.pl_nombre + "</option>");
                $('#ddlPueblos2').append("<option value=" + t.pl_id + " > " + t.pl_nombre + "</option>");
                $('#ddlPueblos3').append("<option value=" + t.pl_id + " > " + t.pl_nombre + "</option>")

            });
        },
        error: function (error) {
            console.log(error)
        },
        // beforeSend: function () {
        // },
    });
    $('#ddlPueblos1').select2({
        theme: "bootstrap-5",
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: $(this).data('placeholder'),
        closeOnSelect: false,
    });
    $('#ddlPueblos2').select2({
        theme: "bootstrap-5",
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: $(this).data('placeholder'),
        closeOnSelect: false,
    });
    $('#ddlPueblos3').select2({
        theme: "bootstrap-5",
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: $(this).data('placeholder'),
        closeOnSelect: false,
    });
    jQuery.ajax({
        url: baseApiUrlEndPoint + '/Estados',
        type: "GET",
        data: null,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            data.items.forEach((t, i) => {
                $('#ddlEstados1').append("<option value='" + t.e_nombre + "' style='height: 30px;'> " + t.e_nombre + "</option>");
            });
        },
        error: function (error) {
            console.log(error)
        },
        // beforeSend: function () {
        // },
    });
    $('#ddlEstados1').select2({
        theme: "bootstrap-5",
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: $(this).data('placeholder'),
        closeOnSelect: false,
    });

    $('#ddlEstados1').next('.select2-container').hide();
    $('#ddlPueblos3').next('.select2-container').hide();
    var cl_id = $('#cl_id').val();
    var tr_id = $('#tr_id').val();
     var frm_id = $('#frm_id').val();
    // Obtenemos datos del Form  cl_id  tr_id  GET
    obtenerDatos();
   

    $('#btnActualizar').click(function () {
        ActualizarFormulario();
    });

    $('#frmCategoria').on('change', function () {
        var cat = $('#frmCategoria').val();
        $('#frmTipoVehiculo').val('');
        if (cat === 'Conductor') {
            $('#frmTipoVehiculo').attr('disabled', 'disabled');
        } else {
            $('#frmTipoVehiculo').removeAttr('disabled');
        }
    });

    $('#divImagen').on('click', 'a.btn-editar', function (e) {
        console.log(e);
        e.stopPropagation()
        e.preventDefault();
        var file = $(this).siblings('.custom-file-input');
        //$('#divImagen .btn-eliminar').not(this).prop('disabled', true);
        file.click();
    });

    $('#divImagen').on('change', '.custom-file-input', function (e) {
        console.log(e);
        var id = $(this).siblings('.btn-editar').attr('id');
        var ar_id_pos = $(this).attr('id');
        var file = e.target.files[0];
        agregarArchivo(id, ar_id_pos, file);
    });

    $('#divImagen').on('click', '.btn-preview', function (e) {
        console.log($(this));
        e.stopPropagation()
        e.preventDefault();
        var archivoId = $(this).data('id'); // Obtener el ID del archivo desde el atributo data
        var url = $(this).data('url');
        var esPDF = url.endsWith('.pdf');

        $('#exampleModal').modal('show');

        if (esPDF) {
            // Si es un PDF, mostrarlo en un iframe
            $('#imgURL').replaceWith('<iframe id="imgURL" src="' + url + '" style="width: 100%; height: 400px;" frameborder="0"></iframe>');
        } else {
            // Si es una imagen, mostrarla en un img
            $('#imgURL').replaceWith('<img id="imgURL" src="' + url + '" class="img-fluid">');
        }
    });

    $('#divImagen').on('click', 'a.btn-eliminar', function (e) {
        e.stopPropagation();
        e.preventDefault();
        var ar_id = $(this).attr('id');
        Swal.fire({
            title: '¿Estás seguro de eliminar este archivo?',
            text: "No podrás revertir este proceso!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                jQuery.ajax({
                    url: baseApiUrlEndPoint + '/Archivo/' + ar_id,
                    type: "DELETE",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        console.log('Solicitud DELETE exitosa:', data);
                        if (data.success) {
                            obtenerArchivo(pgId);
                            Swal.fire({
                                title: "Eliminado!",
                                text: data.message,
                                icon: "success",
                                timer: 1000
                            });
                        }
                    },
                });
            }
        });
    });


    function obtenerDatos() {
        // Obtenemos datos del Form  cl_id  tr_id  GET
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/RenovLic/obtenerDatosForm/' + cl_id + '/' + tr_id + '/' + frm_id,
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log('DATA ITEM:::::::::::::::',data.item);
                var item = data.item;
                if (!item)
                    return;

                function formatearFechaHora(fechaHoraFormateada) {
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


                if (item.fechaConvictoBebida != null && typeof item.fechaConvictoBebida !== 'undefined') {
                    var fechaConvictoBFormateada = item.fechaConvictoBebida.slice(0, 10);
                    $("#frmFechaConvictoBebida").val(fechaConvictoBFormateada);
                } else {
                    $("#frmFechaConvictoBebida").val("");
                }

                if (item.fechaConvictoNarcotico != null && typeof item.fechaConvictoNarcotico !== 'undefined') {
                    var fechaConvictoNFormateada = item.fechaConvictoNarcotico.slice(0, 10);
                    $("#frmFechaConvictoNarcotico").val(fechaConvictoNFormateada);
                } else {
                    $("#frmFechaConvictoNarcotico").val("");
                }

                if (item.fechaExpiracion != null && typeof item.fechaExpiracion !== 'undefined') {
                    var fechaExpiracion = item.fechaExpiracion.slice(0, 10);
                    $("#frm_fechaExpiracion").val(fechaExpiracion);
                } else {
                    $("#frm_fechaExpiracion").val("");
                }

                //$('#frm_fechaExpiracion').text(item.fechaExpiracion ? dateFormat(item.fechaExpiracion.substring(0, 10), 'MM/DD/YYYY') : '');
                //$('input[name="btnradioSuspendido"][value="' + item.licenciaSuspendida + '"]').prop('checked', true).trigger('change');
                //$('input[name="btnradioRecluido"][value="' + item.recluido + '"]').prop('checked', true).trigger('change');
                //$('input[name="btnradioBebida"][value="' + item.convictoBebida + '"]').prop('checked', true).trigger('change');
                //$('input[name="btnradioNarcotico"][value="' + item.convictoNarcotico + '"]').prop('checked', true).trigger('change');
                //$('input[name="btnradioAsume"][value="' + item.obligacionAlimentaria + '"]').prop('checked', true).trigger('change');
                //$('input[name="btnradioAcca"][value="' + item.deudaAcca + '"]').prop('checked', true).trigger('change');
                //$('input[name="radioRevision"][value="' + item.estadoRevision + '"]').prop('checked', true);


                $('#btnSelectSuspension').prop('checked', item.licenciaSuspendida === 'Sí').trigger('change');
                $('#btnfrmRecluido').prop('checked', item.recluido === 'Sí').trigger('change');
                $('#btnfrmConvictoBebida').prop('checked', item.convictoBebida === 'Sí').trigger('change');
                $('#btnfrmConvictoNarcotico').prop('checked', item.convictoNarcotico === 'Sí').trigger('change');
                $('#btnfrmObligacionAlimentaria').prop('checked', item.obligacionAlimentaria === 'Sí').trigger('change');
                $('#btnfrmDeudaAcca').prop('checked', item.deudaAcca === 'Sí').trigger('change');
                $('input[name="radioRevision"][value="' + item.estadoRevision + '"]').prop('checked', true);

                $('#frm_id').val(item.frmID);
                $("#frmNombre").text(nombreCompleto);
                $("#frmCorreo").text(item.cl_cliente.cl_correo);
                $("#frmTelefono").text(item.cl_cliente.cl_numeroTelefono);

                $("#frmCodigoPago").text(item.cl_pago.pg_codigo);
                $("#frmTipoTramite").text(item.cl_cliente.cl_correo);
                $("#frmFecha").text(fechaHoraFormateada);
                $("#frmTipoLicencia").val(item.tipoLicencia);
                $("#frmNumeroLicencia").val(item.numeroLicencia);
                $("#frmCategoria").val(item.categoria);
                $("#frmIdentificacion").val(item.identificacion);
                $("#frmNumeroIdentificacion").val(item.numeroIdentificacion);
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
                $("#ddlPueblos1").val(item.pueblo).trigger('change');
                $("#frmBarrio").val(item.barrio);
                $("#frmApartado").val(item.apartado);
                $("#ddlPueblos2").val(item.pueblo2).trigger('change');
                $("#frmMotivoSuspension").val(item.motivoSuspension);
                $("#frmCodigoPostal").val(item.codigoPostal);
                $("#frmCodigoPostal2").val(item.codigoPostal2);

                $("#frmTipoVehiculo").val(item.tipoVehiculo);
                $("#frm_paisProcede").val(item.paisProcede);
                $("#frm_estadoProcede").val(item.estadoProcede);
                var EstadoProcede = null;
                if (item.paisProcede == "Puerto Rico") {
                    $('#ddlPueblos3').next('.select2-container').show();
                    $("#ddlPueblos3").val(item.estadoProcede).trigger('change');
                    $('#ddlPueblos3').append("<option value='" + item.estadoProcede + "' style='height: 30px;' selected> " + item.estadoProcede + "</option>");
                } else {
                    $('#ddlEstados1').next('.select2-container').show();
                    $("#ddlEstados1").val(item.estadoProcede).trigger('change');
                    $('#ddlEstados1').append("<option value='" + item.estadoProcede + "' style='height: 30px;' selected> " + item.estadoProcede + "</option>");
                }
            
                //$("#frm_fechaExpiracion").val(item.fechaExpiracion);
                $("#frm_nombrePadre").val(item.nombrePadre);
                $("#frm_nombreMadre").val(item.nombreMadre);
                $("#frm_numeroLicenciaPR").val(item.numeroLicenciaPR);

                $('#nro-licencia').text(item.numeroLicencia);
                $('#nro-ssn').text(item.cl_cliente.cl_numeroSeguro);
                $('#fecha-nac').text(item.cl_cliente.cl_fechaNacimiento ? dateFormat(item.cl_cliente.cl_fechaNacimiento.substring(0, 10), 'MM/DD/YYYY') : '');

                //$('#doctorAsignado').text(item.doctorAsignado);
                $('#estadoFormulario').text(item.estadoFormulario === '1' ? '✔' : '✖');
                $('#estadoMultas').text(item.estadoMultas === '1' ? '✔' : '✖');
                //$('#estadoEvaluacion').text(item.estadoEvaluacion);

                //let estadoEvaluacion = item.estadoEvaluacion.toLowerCase();
                //let labelClass = '';
                //let estadoTexto = '';

                //switch (estadoEvaluacion) {
                //    case 'pendiente':
                //        labelClass = 'badge bg-warning-transparent';
                //        estadoTexto = 'Pendiente';
                //        break;
                //    case 'asignado':
                //        labelClass = 'badge bg-info-transparent';
                //        estadoTexto = 'Asignado';
                //        break;
                //    case 'aprobado':
                //        labelClass = 'badge bg-success-transparent';
                //        estadoTexto = 'Aprobado';
                //        break;
                //    case 'denegado':
                //        labelClass = 'badge bg-danger-transparent';
                //        estadoTexto = 'Denegado';
                //        break;
                //    case 'unreachable':
                //        labelClass = 'badge bg-primary-transparent';
                //        estadoTexto = 'Unreachable';
                //        break;
                //    default:
                //        labelClass = 'badge bg-secondary-transparent';
                //        estadoTexto = 'Desconocido';
                //        break;
                //}

                //$('#estadoEvaluacion').attr('class', labelClass).text(estadoTexto);

                $('input[name="radioRevision"][value="' + item.estadoFormulario + '"]').prop('checked', true);
                $('input[name="radioRevisionmulta"][value="' + item.estadoMultas + '"]').prop('checked', true);

                var formularioRevisado = $('input[name="radioRevision"]:checked').val() === '1';
                var multasRevisadas = $('input[name="radioRevisionmulta"]:checked').val() === '1';

                if (formularioRevisado && multasRevisadas) {
                    $('#btnGuardarRevision').prop('disabled', true);
                    $('input[name="radioRevision"], input[name="radioRevisionmulta"]').prop('disabled', true);
                }
                else {
                    $('#btnGuardarRevision').prop('disabled', false);
                    $('input[name="radioRevision"], input[name="radioRevisionmulta"]').prop('disabled', false);
                }

                //if (estadoEvaluacion === "aprobado" || pendiente === "denegado") {
                //    $('#selecDoctor').prop('disabled', true);
                //    $('#btnAsignarDoctor').prop('disabled', true);
                //}
                //else {
                //    $('#selecDoctor').prop('disabled', true);
                //    $('#btnAsignarDoctor').prop('disabled', true);
                //}

                pgId = item.cl_pago.pg_id;

                $('#btnImprimirForm').attr('data-print', baseApiUrlEndPoint + '/pdf/consolidado/' + tr_id + '/' + frm_id);

                obtenerArchivo(pgId);
            },
            error: function (error) {
                console.log(error)
            },
        });
    }
    function ActualizarFormulario() {
        var FrmID = $('#frm_id').val();
        var TipoLicencia = $('#frmTipoLicencia').val();
        var Categoria = $('#frmCategoria').val();
        var TipoVehiculo = $('#frmTipoVehiculo').val();
        var PaisProcede = $('#frm_paisProcede').val();

        var EstadoProcede = null;
        if (PaisProcede == "Puerto Rico") {
            EstadoProcede = $('#ddlPueblos3').val();
            EstadoProcede = $('#ddlPueblos3').select2('data')[0].text;
        } else {
            EstadoProcede = $('#ddlEstados1').val();
            EstadoProcede = $('#ddlEstados1').select2('data')[0].text;
        }
        var NumeroLicencia = $('#frmNumeroLicencia').val();
        var FechaExpiracion = $('#frm_fechaExpiracion').val() ? $('#frm_fechaExpiracion').val().trim() : null;
        var Identificacion = $('#frmIdentificacion').val();
        var NumeroIdentificacion = $('#frmNumeroIdentificacion').val();
        var StatusLegal = $('#frmStatusLegal').val();
        var Genero = $('#frmGenero').val();
        var Donante = $('#frmDonante').val();
        var TipoSangre = $('#frmTipoSangre').val();
        var Talla = $('#frmTalla').val();
        var Peso = $('#frmPeso').val();
        var Tez = $('#frmTez').val();
        var ColorPelo = $('#frmColorPelo').val();
        var ColorOjo = $('#frmColorOjo').val();
        var NombrePadre = $('#frm_nombreMadre').val();
        var NombreMadre = $('#frm_nombrePadre').val();
        var Direccion = $('#frmDireccion').val();
        var NumeroDireccion = $('#frmNumeroDireccion').val();
        var Pueblo = $('#ddlPueblos1').val();
        var CodigoPostal = $('#frmCodigoPostal').val();
        var Barrio = $('#frmBarrio').val();
        var Apartado = $('#frmApartado').val();
        var Pueblo2 = $('#ddlPueblos2').val();
        var CodigoPostal2 = $('#frmCodigoPostal2').val();
        //var LicenciaSuspendida = $('input[name="btnradioSuspendido"]:checked').val();
        var MotivoSuspension = $('#frmMotivoSuspension').val();
        var NumeroLicenciaPR = $('#frm_numeroLicenciaPR').val();
        //var Recluido = $('input[name="btnradioRecluido"]:checked').val();
        //var ConvictoBebida = $('input[name="btnradioBebida"]:checked').val();
        var FechaConvictoBebida = $('#frmFechaConvictoBebida').val() != '' ? $('#frmFechaConvictoBebida').val().trim() : null;
        //var ConvictoNarcotico = $('input[name="btnradioNarcotico"]:checked').val();
        var FechaConvictoNarcotico = $('#frmFechaConvictoNarcotico').val() ? $('#frmFechaConvictoNarcotico').val().trim() : null;
        //var ObligacionAlimentaria = $('input[name="btnradioAsume"]:checked').val();
        //var DeudaAcca = $('input[name="btnradioAcca"]:checked').val();
        var EstadoRevision = $('input[name="radioRevision"]:checked').val();
        var EstadoProceso = 0;


        var LicenciaSuspendida = $('#btnSelectSuspension').is(':checked') ? 'Sí' : 'No';
        var Recluido = $('#btnfrmRecluido').is(':checked') ? 'Sí' : 'No';
        var ConvictoBebida = $('#btnfrmConvictoBebida').is(':checked') ? 'Sí' : 'No';
        var ConvictoNarcotico = $('#btnfrmConvictoNarcotico').is(':checked') ? 'Sí' : 'No';
        var ObligacionAlimentaria = $('#btnfrmObligacionAlimentaria').is(':checked') ? 'Sí' : 'No';
        var DeudaAcca = $('#btnfrmDeudaAcca').is(':checked') ? 'Sí' : 'No';

        //Adicional
        var VehiculoPesado = '';
        var NumeroLicencia2 = '';
        var Numero = '';

        var EstadoMultas = $('input[name="radioRevisionmulta"]:checked').val();
        if (LicenciaSuspendida === 'No') {
            MotivoSuspension = '';
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
            Pueblo: Pueblo.trim(),
            CodigoPostal: CodigoPostal,
            Barrio: Barrio,
            Apartado: Apartado,
            Pueblo2: Pueblo2.trim(),
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
            TipoVehiculo: TipoVehiculo,
            PaisProcede: PaisProcede,
            EstadoProcede: EstadoProcede.trim(),
            NumeroLicencia2: NumeroLicencia2,
            FechaExpiracion: FechaExpiracion,
            NumeroIdentificacion: NumeroIdentificacion,
            NombrePadre: NombrePadre,
            NombreMadre: NombreMadre,
            NumeroLicenciaPR: NumeroLicenciaPR,
            EstadoRevision: EstadoRevision,
            EstadoProceso: EstadoProceso,
            EstadoMultas: EstadoMultas,
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
                        timer: 2000
                    });
                    $('#exampleModal').modal('hide');
                    obtenerDatos();
                }
            },
            error: function (error) {
                console.log(error);
            }
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
                console.log("Data", data)
                //success == false;
                ImagesManager($('#tr_id').val()).render(data.items);
                /*var descripcion = "";
                var item = null;
                if (data.items.length === 0) {
                    descripcion = validarNombre(1);
                    llenarhtml(null, descripcion, 0, 1);
                    descripcion = validarNombre(2);
                    llenarhtml(null, descripcion, 0, 2);
                    descripcion = validarNombre(3);
                    llenarhtml(null, descripcion, 0, 3);
                    descripcion = validarNombre(4);
                    llenarhtml(null, descripcion, 0, 4);
                    descripcion = validarNombre(10);
                    llenarhtml(null, descripcion, 0, 10);
                } else {
                    item = data.items.find(t => t.ar_pos == 1);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        descripcion = validarNombre(1);
                        llenarhtml(null, descripcion, 0, 1);
                    }
                    item = data.items.find(t => t.ar_pos == 2);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        descripcion = validarNombre(2);
                        llenarhtml(null, descripcion, 0, 2);
                    }

                    item = data.items.find(t => t.ar_pos == 6);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        llenarhtml(null, validarNombre(6), 0, 6);
                    }

                    item = data.items.find(t => t.ar_pos == 3);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        llenarhtml(null, validarNombre(3), 0, 3);
                    }

                    item = data.items.find(t => t.ar_pos == 5);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        llenarhtml(null, validarNombre(5), 0, 5);
                    }


                    item = data.items.find(t => t.ar_pos == 4);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        llenarhtml(null, validarNombre(4), 0, 4);
                    }

                     item = data.items.find(t => t.ar_pos == 10);
                     if (item) {
                         descripcion = validarNombre(item.ar_pos);
                         llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                     }
                     else {
                         llenarhtml(null, validarNombre(10), 0, 10);
                     }
                    item = data.items.find(t => t.ar_pos == 20);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        llenarhtml(null, validarNombre(20), 0, 20);
                    }

                    item = data.items.find(t => t.ar_pos == 30);
                    if (item) {
                        descripcion = validarNombre(item.ar_pos);
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        llenarhtml(null, validarNombre(30), 0, 30);
                    }
                }*/

            },
        });
    }

    function agregarArchivo(ar_id, ar_pos, file) {
        var formData = new FormData();
        formData.append('file', file);
        var tipo = "";
        if (ar_id == 0) {
            tipo = "POST";
        } else
            tipo = "PUT";
        var objArchivo = {
            ar_id: ar_id,
            ar_nombre: null,
            ar_pos: ar_pos,
            cl_cliente: { cl_id: cl_id },
            tr_tramite: { tr_id: tr_id },
            frm_id: $('#frm_id').val(),
            ar_estado: 1,
            ar_fecha: new Date(),
            pg_id: pgId
        }
        formData.append('item', JSON.stringify(objArchivo))
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Archivo/Upload',
            type: tipo,
            data: formData,
            processData: false,
            contentType: false,
            mimeType: "multipart/form-data",
            success: function (data) {
                obtenerArchivo(pgId);
            },
            error: function (error) {
                console.log(error)
            },
        });
    }

    function validarNombre(pos) {
        var descripcion = "";
        if (pos == 1)
            descripcion = "Tarjeta de Seguro Social";
        else if (pos == 2)
            descripcion = "Fecha de nacimiento y Presencia Legal";
        else if (pos == 3)
            descripcion = "Foto con ID - Anverso";
        else if (pos == 4)
            descripcion = "Recibo de agua o luz";
        else if (pos == 5)
            descripcion = "Foto con ID - Reverso";
        else if (pos == 6)
            descripcion = "Record Choferil de su estado de procedencia";
         else if (pos == 10)
            descripcion = "Foto Selfie";
        else if (pos == 20)
            descripcion = "Certificado de Autorización";
        else if (pos == 30)
            descripcion = "Firma";
        return descripcion;
    }

    function llenarhtml(url, name, id, pos) {
        if (url == null)
            url = "https://tulicenciapr.com/upload/0default1.png";
        // if (url == null && pos == 1)
        //     url = "";
        var htmldelete = "";

        var tipo = 'img';
        var tipo1 = '';
        if (pos === 20) {
            tipo = 'iframe';
            tipo1 = '</iframe>';
        }

        console.log(id);
        if (id > 0)
            htmldelete = '<a href="javascript:void(0);" data-id="' + id + '" id="' + id + '" class="btn-eliminar btn btn-wave btn-danger-light btn-icon btn-sm contact-delete"><i class="ri-delete-bin-line"></i></a>';
        var html = '<div class="col-sm-12 col-md-4 col-lg-3 col-xl-2">' +
            '<div class="card custom-card hrm-main-card primary team-member-card">' +
            '<div class="teammember-cover-image"> ' +
            '<' + tipo + ' src="' + url + '" class="card-img-top" style="object-fit: cover; min-height: 200px;" alt="..."' + (pos === 20 ? ' style="width: 100%; height: 200px;"' : '') + '>' + tipo1 + '</div>' +
            '<div class="card-body p-0">' +
            '<div class="d-flex flex-wrap align-item-center mt-sm-0 mt-5 justify-content-between border-bottom border-block-end-dashed p-3">' + '</div>' +
            '<div class="team-member-stats d-sm-flex justify-content-evenly">' +
            '<div class="text-center p-3 my-auto"  style="height: 71px;">' +
            '<p class="fw-bold mb-0">' + name + '</p>' + '</div>' + '</div>' + '</div>' +
            '<div class="card-footer border-block-start-dashed text-center">' +
            '<div class="btn-list">' +
            '<div class="btn-list">' +
            '<input type="file" class="custom-file-input" id="' + pos + '" style="display: none;"> ' +
            '<a href="javascript:void(0);" data-id="' + id + '" id="btn-preview-' + id + '" data-url="' + url + '" class="btn-preview btn btn-wave btn-primary-light btn-icon btn-sm"><i class="ri-eye-line"></i></a>' +
            '<a href="javascript:void(0);" data-id="' + id + '" id="' + id + '" class="btn-editar btn btn-wave btn-info-light btn-icon btn-sm"><i class="ri-upload-line"></i></a>' +
            htmldelete +
            '</div></div></div></div></div>';
        $('#divImagen').append(html);
    }


    //validar btn PDF
    function validarPDF() {
        console.log("ingresa a validar")
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Archivo/' + $('#frm_id').val(),
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var rutapdfValid = data.items.find(t => t.ar_pos == 6);
                if (rutapdfValid) {
                    $("#btnGrabarPDF").hide();
                    console.log("t.ar_pos == 6")
                }
                else $("#btnGrabarPDF").show();
            },
            error: function (error) {
                console.log(error)
            },
        });
    }


    //PDF

    $('#btnVerPDF').on('click', function () {
        validarPDF();
        var trId = $('#tr_id').val();
        var frmId = $('#frm_id').val();
        var url = baseApiUrlEndPoint + '/pdf/gen-consolidado/' + trId + '/' + frmId+'/1';
        var urlVisor = urlPdfViewer + '?url=' + url;
        $('#ihtml').attr('src', urlVisor);
        $('#btnDescargarPDF').attr('href', baseApiUrlEndPoint + '/pdf/consolidado/' + trId + '/' + frmId);
        $('#modalPdf').modal('show');
    });

    $('#btnGrabarPDF').on('click', function (e) {
        e.stopPropagation();
        e.preventDefault();
        var cl_id = $('#cl_id').val();
        var tr_id = $('#tr_id').val();
        console.log("cl_id enviaaa", cl_id);
        var rutaHTML = 'https://tulicenciapr.com/admin/Home/VerFormPDFReciprocidad/' + cl_id + '/' + tr_id;

        var apiUrl = encodeURIComponent(rutaHTML);
        var objArchivo = {
            ar_id: 0,
            ar_nombre: apiUrl,
            ar_pos: 6,
            cl_cliente: { cl_id: cl_id },
            tr_tramite: { tr_id: tr_id },
            frm_id: $('#frm_id').val(),
            ar_estado: 1,
            ar_fecha: new Date(),
        }
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Archivo/caso1Upload',
            type: 'POST',
            data: JSON.stringify(objArchivo),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    Swal.fire({
                        title: "Guardo en BD!",
                        text: "",
                        icon: "success",
                        timer: 2000
                    });
                }
            },
            error: function (error) {
                console.log(error)
            },
        });
    });


    //REVISION
    $('#btnGuardarRevision').click(function () {
        ActualizarFormulario();
    });

    // ASIGNACION
    jQuery.ajax({
        url: baseApiUrlEndPoint + '/Administrador/Radicadores',
        type: "GET",
        data: null,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            data.items.forEach((tr, i) => {
                $('#selecRadicador').append("<option value='" + tr.adm_id + "'style='height: 30px;'> " + tr.adm_nombres + "</option>");
            });
        },
        error: function (error) {
            console.log(error)
        },
    });

    $('#btnConfirmarCaso').click(function () {
        ConfirmarCaso();
    });

    function ConfirmarCaso() {

        var formularioRevisado = $('input[name="radioRevision"]:checked').val() === '1';
        var multasRevisadas = $('input[name="radioRevisionmulta"]:checked').val() === '1';
        /*var estadoEvaluacion = $('#estadoEvaluacion').text().trim(); // Obtener el estado de evaluación*/

        if (!formularioRevisado || !multasRevisadas ) {
            Swal.fire({
                title: 'Asignación no permitida',
                text: 'Debe haber aprobado tanto la revisión de formularios como de multas.',
                icon: 'warning',
                confirmButtonText: 'OK'
            });
            return;
        }

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¿Quieres confirmar esta asignación?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, confirmar!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {

            if (result.isConfirmed) {
                var tr_id = $('#tr_id').val();
                var frm_id = $('#frm_id').val();
                var adm_id1 = 1;                            //  1 REEMPLAZA POR ID DE LA PERSONA LOGEADA
                var adm_id2 = $('#selecRadicador').val();
                var asig_fecha = new Date();
                var asig_Activo = true;

                var objConfirmar = {
                    asig_id: 0,
                    tr_id: { tr_id: tr_id },
                    frm_id: { frmID: frm_id },
                    adm_id1: { adm_id: 1 },                //  1 REEMPLAZA POR ID DE LA PERSONA LOGEADA
                    adm_id2: { adm_id: adm_id2 },
                    asig_fecha: asig_fecha,
                    asig_Activo: asig_Activo,
                };

                jQuery.ajax({
                    url: baseApiUrlEndPoint + '/Asignacion/',
                    type: 'POST',
                    data: JSON.stringify(objConfirmar),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        console.log(data);
                        if (data.success) {
                            jQuery.ajax({
                                url: baseApiUrlEndPoint + '/RenovLic/cambioEstadoForm/' + tr_id + '/' + frm_id + '/' + 1,
                                type: 'PUT',
                                data: null,
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (data2) {
                                    console.log(data2);
                                    if (data2.success) {
                                        Swal.fire({
                                            title: "Confirmado!",
                                            text: data.message,
                                            icon: "success",
                                            timer: 2000
                                        });
                                        setTimeout(function () {
                                            //window.location.href = "https://localhost:7274";
                                            window.location.href = "https://tulicenciapr.com/admin/Home";
                                        }, 1500);
                                    }
                                },
                                error: function (error) {
                                    console.log(error);
                                }
                            });
                        }
                    },
                });
            }
        });
    }



});