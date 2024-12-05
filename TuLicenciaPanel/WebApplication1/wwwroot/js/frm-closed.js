$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const status = urlParams.get('status');
    const element = $('#formulario-id'); // Reemplaza con el ID real del elemento donde quieres mostrar el texto
    switch (status) {
        case '0':
            element.text('NEW CASES');
            break;
        case '1':
            element.text('REVIEWED CASES');
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


    var cl_id = $('#cl_id').val();
    var tr_id = $('#tr_id').val();
    var frm_id = $('#frm_id').val();
    var pgId = 0;

    var url = baseApiUrlEndPoint + '/pdf/consolidado/' + tr_id + '/' + frm_id;
    var urlVisor = urlPdfViewer + '?url=' + url;
    $('iframe', '#modal-visor').attr('src', urlVisor);
    $('.btn-pdf', '#modal-visor').attr('href', url);

    // TIPO TRAMITE
    var texto_tramite;
    if (tr_id === '1') {
        texto_tramite = 'Renovación de Licencia';
    } else if (tr_id === '3') {
        texto_tramite = 'Duplicado de Licencia';
    } else if (tr_id === '4') {
        texto_tramite = 'Licencia Reciprocidad';
    } else {
        texto_tramite = '';
    }
    $('#frmTramite').text(texto_tramite);


    // DATOS DEL CASO
    jQuery.ajax({
        url: 'https://api.tulicenciapr.com/api/RenovLic/obtenerDatosForm/' + cl_id + '/' + tr_id + '/' + frm_id,
        type: "GET",
        data: null,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var item = data.item;
            $("#frmNombre").text(item.cl_cliente.cl_nombre);
            $("#frmPrimerApellido").text(item.cl_cliente.cl_primerApellido);
            $("#frmSegundoApellido").text(item.cl_cliente.cl_segundoApellido);
            $("#frmCorreo").text(item.cl_cliente.cl_correo);
            $("#frmTelefono").text(item.cl_cliente.cl_numeroTelefono);

            $("#frmcodigoPago").text(item.cl_pago.pg_codigo);

            var fechaPago = new Date(item.cl_pago.pg_fecha);
            var formatoFechaPago = ("0" + (fechaPago.getMonth() + 1)).slice(-2) + "/" + ("0" + fechaPago.getDate()).slice(-2) + "/" + fechaPago.getFullYear();
            $("#frmFechaPago").text(formatoFechaPago);

            pgId = item.cl_pago.pg_id;
            obtenerArchivos();

            console.log("item.cl_pago.pg_fecha", item.cl_pago.pg_fecha);
        },
        error: function (error) {
            console.log(error)
        },
        // beforeSend: function () {
        // },
    });

    // DATOS DEL ASIGNADO
    jQuery.ajax({
        url: 'https://api.tulicenciapr.com/api/Asignacion/ObtenerAsignacion/' + frm_id + '/' + tr_id,
        type: "GET",
        data: null,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            // PRIMER FILA
            var primerCaso = data.items[0];
            var fechaPrimerCaso = new Date(primerCaso.asig_fecha);
            var formatoFechaPrimerCaso = ("0" + (fechaPrimerCaso.getMonth() + 1)).slice(-2) + "/" + ("0" + fechaPrimerCaso.getDate()).slice(-2) + "/" + fechaPrimerCaso.getFullYear();

            $("#asignadoFechaInicio0").text(" en: " + formatoFechaPrimerCaso);
            $("#asignadoPor0").text(primerCaso.adm_id1.adm_nombres);
            $("#asignadoa0").text(primerCaso.adm_id2.adm_nombres);

            //evaluado
            $("#asignadoPor").text(primerCaso.adm_id1.adm_nombres);
            $("#asignadoFechaInicio").text(" en: " + formatoFechaPrimerCaso);

            //  SEGUNDA FILA
            var segundoCaso = data.items[1];
            var fechaSegundoCaso = new Date(segundoCaso.asig_fecha);
            var formatoFechaSegundoCaso = ("0" + (fechaSegundoCaso.getMonth() + 1)).slice(-2) + "/" + ("0" + fechaSegundoCaso.getDate()).slice(-2) + "/" + fechaSegundoCaso.getFullYear();

            $("#fechaAsignado1").text(" en: " + formatoFechaSegundoCaso);
            $("#asignadoPor1").text(segundoCaso.adm_id2.adm_nombres);

            // TERCERA FILA
            var segundoCaso = data.items[2];
            var fechaSegundoCaso = new Date(segundoCaso.asig_fecha);
            var formatoFechaSegundoCaso = ("0" + (fechaSegundoCaso.getMonth() + 1)).slice(-2) + "/" + ("0" + fechaSegundoCaso.getDate()).slice(-2) + "/" + fechaSegundoCaso.getFullYear();

            $("#fechaAsignado2").text(" en: " + formatoFechaSegundoCaso);
            $("#asignadoPor2").text(segundoCaso.adm_id2.adm_nombres);
        },
        error: function (error) {
            console.log(error)
        },
        // beforeSend: function () {
        // },
    });


    function obtenerArchivos() {
        $.ajax({
            url: baseApiUrlEndPoint + '/Archivo/' + pgId,
            dataType: "json",
            success: (response) => {
                if (response) {
                    if (response.items) {
                        var files = response.items.filter(x => x.tr_tramite.tr_id === parseInt(tr_id) && x.ar_pos === 8);
                        if (files.length > 0) {
                            $('#img-certificado').attr('src', files[0].ar_nombre);
                        } else {
                            console.log('No se encontró ningún archivo con ar_pos 8.');
                        }
                    }
                }
            },
            error: (jqXHR, textStatus, errorThrown) => {
                console.log('Error en la solicitud AJAX:', textStatus, errorThrown);
            }
        });
    }
});