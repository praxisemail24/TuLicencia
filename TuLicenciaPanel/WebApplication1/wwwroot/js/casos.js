document.addEventListener('DOMContentLoaded', () => {
    var estadoSelect = document.getElementById('frm_estado');
    var estadoProcesoContainer = document.getElementById('frm_estadoProceso_container');

    estadoSelect.addEventListener('change', function () {
        if (estadoSelect.value === '2') {
            estadoProcesoContainer.style.display = 'block';
        } else {
            estadoProcesoContainer.style.display = 'none';
        }
    });
});

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
            

    llenarTabla();
    $('#btnExampleModal').on('click', function () {
        cleanForm();
        $('#exampleModal').modal('show');
    })
    $('#btnAgregar').on('click', function () {
        agregarCliente();
    });

    // BUSQUEDA POR CAMPOS
    $('#btnBuscar').click(function () {
        llenarTabla();
    });

    $('#btnLimpiar').click(function () {   // LIMPIAR
        $('#frm_tipoTramite').val('0');
        $('#frm_estado').val('99');
        $('#frm_estadoProceso').val('99');
        $('#buscarCodigo').val('');
        $('#buscarNombre').val('');
        $('#buscarPrimerApellido').val('');
        $('#buscarSegundoApellido').val('');
        $('#buscarCorreo').val('');
    });


    $('#btnCancelar').click(function () {   // Cancelar                
        $('#btnLimpiar').trigger("click");
        llenarTabla();
    });


    function llenarTabla() {  //GET REGISTROS
        console.log("ingresoa llenartabla");
        var item = {
            estado: $('#frm_estado').val(),
            estadoProceso: $('#frm_estadoProceso').val(),
            cl_tramite: { tr_id: $('#frm_tipoTramite').val() },
            cl_pago: { pg_fecha: '10/10/2023', pg_codigo: $('#buscarCodigo').val(), }, cl_cliente: {
                cl_nombre: $('#buscarNombre').val(),
                cl_primerApellido: $('#buscarPrimerApellido').val(),
                cl_segundoApellido: $('#buscarSegundoApellido').val(),
                cl_correo: $('#buscarCorreo').val(),
            }
        };

        $('#tablaReporte').bootstrapTable('destroy');
        $('#tablaReporte').bootstrapTable({
            url: baseApiUrlEndPoint + '/RenovLic/buscadorRegistroPanel',
            method: 'post',
            pagination: true,
            sidePagination: 'server',
            pageSize: 10,
            queryParams: function (params) {
                return {
                    offset: params.offset, limit: params.limit,
                    //search: params.search, // 
                    //sort: params.sort, //
                    //order: params.order, // 
                };
            },
            ajax: function (request) {
                let formData = new FormData();
                formData.append('item', JSON.stringify(item));
                formData.append('paginator', request.data);
                $.ajax({
                    type: 'POST',
                    url: request.url,
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (res) {
                        request.success({
                            total: res.extra,
                            rows: res.items
                        });

                        $('th:nth-child(10)', '#tablaReporte').hide();
                        $('td:nth-child(10)', '#tablaReporte').hide();
                        $('th:nth-child(11)', '#tablaReporte').hide();
                        $('td:nth-child(11)', '#tablaReporte').hide();
                    },
                    beforeSend: (xhr) => {
                        xhr.setRequestHeader('Authorization', 'Bearer ' + jwtToken);
                    },
                });
            },
            responseHandler: function (res) {
                return {
                    total: res.total,
                    rows: res.rows
                };
            },
            columns: [
                { field: 'frmID', title: 'ID' },
                { field: 'cl_cliente.cl_nombreCompleto', title: 'NOMBRE CLIENTE', formatter: cellCustomer },
                //{ field: 'cl_cliente.cl_correo', title: 'Correo' },
                //{ field: 'cl_cliente.cl_numeroTelefono', title: 'Teléfono' },
                { field: 'nombreTramite', title: 'TIPO TRÁMITE' },
                { field: 'cl_pago.pg_fecha', title: 'FECHA PAGO', formatter: (value) => renderCellDateTime(value) },
                { field: 'cl_pago.pg_codigo', title: 'CÓDIGO PAGO' },
                { field: 'estado', title: 'ESTADO', formatter: formatoEstado, cellStyle: asignarClaseEstado },
                { field: 'estadoProceso', title: 'ESTADO PROCESO', formatter: formatoEstadoProceso, cellStyle: asignarClaseEstadoProceso },
                { field: 'porcAvance', title: 'AVANCE %', formatter: (value) => tmplProgressBar(value) },
                { field: 'acciones', title: 'ACCIONES', formatter: cellOptions },
                { field: 'cl_tramite.tr_id', title: 'tramiteID' },
                { field: 'cl_cliente.cl_id', title: 'clienteID' }
            ],
        });
    }

    function cellOptions(value, row, index) {
        var tmpl = '<div class="btn-group my-1">';
        tmpl += '<button class="btn btn-light btn-sm" type="button">Opciones</button>';
        tmpl += '<button type="button" class="btn btn-sm btn-light dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false"><span class="visually-hidden">Toggle Dropdown</span></button>';
        tmpl += '<ul class="dropdown-menu">';
        tmpl += '<li><a class="dropdown-item btnVerPDF" href="javascript:verPdf(' + row.cl_tramite.tr_id + ', ' + row.frmID +');"><i class="la la-file-pdf"></i><span class="ps-2">PDF</span></a></li>';
        tmpl += '<li><a class="dropdown-item btnVerHistorial" href="javascript: void(0)"><i class="la la-list"></i><span class="ps-2">Historial</span></a></li>';
        tmpl += '</ul>';
        tmpl += '</div>';
        return tmpl;
    }

    function cellCustomer(value, row, index) {
        var tmpl = '<div class="d-flex flex-column" style="min-width: 230px;">';
        tmpl += '<div class="text-uppercase">' + row.cl_cliente.cl_nombreCompleto +'</div>';
        tmpl += '<div class="d-flex flex-row align-items-center"><i class="la la-envelope text-warning" style="font-size: 1.1rem;"></i><span class="ps-2">' + row.cl_cliente.cl_correo +'</span></div>';
        tmpl += '<div class="d-flex flex-row align-items-center"><i class="la la-phone text-info" style="font-size: 1.1rem;"></i><span class="ps-2">' + row.cl_cliente.cl_numeroTelefono +'</span></div>';
        tmpl += '</div>';
        return tmpl;
    }

    // EXCEL
    $('#btnExport').on('click', function () {
        exportarExcel();
    });

    function exportarExcel() {
        var frm_tipoTramite = $('#frm_tipoTramite').val();
        var frm_estado = $('#frm_estado').val();
        var frm_estadoProceso = $('#frm_estadoProceso').val();
        var buscarCodigo = $('#buscarCodigo').val();
        var buscarNombre = $('#buscarNombre').val();
        var buscarPrimerApellido = $('#buscarPrimerApellido').val();
        var buscarSegundoApellido = $('#buscarSegundoApellido').val();
        var buscarCorreo = $('#buscarCorreo').val();

        var formData = new FormData();
        formData.append('item', JSON.stringify({
            cl_tramite: { tr_id: $('#frm_tipoTramite').val() },
            cl_cliente: {
                cl_nombre: buscarNombre,
                cl_primerApellido: buscarPrimerApellido,
                cl_segundoApellido: buscarSegundoApellido,
                cl_correo: buscarCorreo
            },
            cl_pago: {
                pg_codigo: buscarCodigo
            },
            Estado: frm_estado,
            EstadoProceso: frm_estadoProceso
        }));

        $.ajax({
            url: baseApiUrlEndPoint + '/RenovLic/buscadorRegistroPanelExcel',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            xhrFields: {
                responseType: 'blob'
            },
            success: function (response, status, xhr) {
                var filename = "";
                var disposition = xhr.getResponseHeader('Content-Disposition');
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    var matches = filenameRegex.exec(disposition);
                    if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                }

                var link = document.createElement('a');
                var url = window.URL.createObjectURL(response);
                link.href = url;
                link.download = filename;
                document.body.append(link);
                link.click();
                window.URL.revokeObjectURL(url);
                link.remove();
            },
            error: function (xhr, status, error) {
                console.error('Error al exportar a Excel:', error);
            }
        });
    }

    let globalPDFBytes = null;

    async function crearPDF(frm_id, estado) {
        var posData;
        if (estado == 'NEWCASE') {
            posData = 6;
        } else if (estado == 'REVIEWCASE') {
            posData = 7;
        } else if (estado == 'PROCESSCASE') {
            posData = 7;
        } else if (estado == 'CLOSEDCASE') {
            posData = 9;
        } else {
        }
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Archivo/' + frm_id,
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log("Datacreaaaa", data);
                const imagenes = [];
                if (data.items.length == 0) {
                    Swal.fire({
                        title: "",
                        text: 'No existen archivo PDF',
                        icon: "info",
                        timer: 1800
                    });
                    return;
                }
                var rutapdf = data.items.find(t => t.ar_pos == posData)?.ar_nombre;
                if (!rutapdf) {
                    Swal.fire({
                        title: "",
                        text: 'No se pudo encontrar el archivo PDF',
                        icon: "info",
                        timer: 1800
                    });
                    return;
                }
                downloadPDF2([], 0, rutapdf, 0, 0);
            },
            error: function (error) {
                console.log(error)
            },
            // beforeSend: function () {
            // },
        });
    }

    async function downloadPDF2(imagenes, tramite, rutapdf, cl_id, tr_id) {
        const { jsPDF } = window.jspdf;
        var thiss = this;
        let doc = new jsPDF({
            orientation: "portrait",
            unit: "mm",
            format: "a4",
        });

        const { PDFDocument } = window.PDFLib;
        const pdfDoc = await PDFDocument.create();


        const coverPageBytes = await doc.output('arraybuffer');
        const coverPdfDoc = await PDFDocument.load(coverPageBytes);
        const [coverPage] = await pdfDoc.copyPages(coverPdfDoc, [0]);
        //pdfDoc.addPage(coverPage);

        // Cargar y fusionar PDF existente
        const existingPdfBytes = await fetch(rutapdf).then((res) => res.arrayBuffer());
        const existingPdf = await PDFDocument.load(existingPdfBytes);
        const importedPages = await pdfDoc.copyPages(existingPdf, existingPdf.getPageIndices());
        // Agregar todas las páginas del PDF existente al nuevo documento
        importedPages.forEach((importedPage) => {
            pdfDoc.addPage(importedPage);
        });
        doc = new jsPDF({
            orientation: "portrait",
            unit: "mm",
            format: "a4"
        });
        let yPosition = 40;
        var lengthPDF = imagenes.length;
        for (let i = 0; i < imagenes.length; i++) {
            await new Promise((resolve) => {
                let img = new Image();
                img.onload = function () {
                    var name = '';
                    doc.setFontSize(12);
                    doc.text("Nombre: " + name, 105, 30, { align: "center" });
                    doc.addImage(this, "JPEG", 10, yPosition, 180, 110);
                    console.log('i', i);
                    if (i < lengthPDF - 1)
                        doc.addPage();
                    resolve();
                };
                img.src = imagenes[i].ar_nombre;
            });
        }

        // Combinar PDF creado con jsPDF
        const jspdfBytes = await doc.output("arraybuffer");
        const jspdfPdfDoc = await PDFDocument.load(jspdfBytes);
        const jspdfPages = await pdfDoc.copyPages(jspdfPdfDoc, jspdfPdfDoc.getPageIndices());

        jspdfPages.forEach((page) =>
            pdfDoc.addPage(page)
        );

        globalPDFBytes = await pdfDoc.save();
        const file = new Blob([globalPDFBytes], { type: "application/pdf" });
        const fileURL = URL.createObjectURL(file);
        document.getElementById("pdfPreview").src = fileURL;
        new bootstrap.Modal(document.getElementById("previewModal")).show();
    }

    function download(data, filename, type) {
        const file = new Blob([data], { type: type });
        if (window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(file, filename);
        } else {
            const a = document.createElement("a"),
                url = URL.createObjectURL(file);
            a.href = url;
            a.download = filename;
            document.body.appendChild(a);
            a.click();
            setTimeout(() => {
                document.body.removeChild(a);
                URL.revokeObjectURL(url);
            }, 0);
        }
    }

    $('#tablaReporte').on('click', '.btnVerHistorial', function (e) {
        var frm_id = $(this).closest('tr').find('td:first').text();  // ese si esta bien
        var tr_id = $(this).closest('tr').find('td:eq(9)').text();
        var cl_id = $(this).closest('tr').find('td:eq(10)').text();
        console.log("ingresoa tablaReporte cl", cl_id);
        console.log("ingresoa tablaReporte tr", tr_id);
        console.log("ingresoa tablaReporte frm", frm_id);
        obtenerAsignacion(frm_id, tr_id);
        obtenerLineaTiempo(frm_id, tr_id);
        //obtenerHistorial(cl_id, tr_id);

        var nombreTramite = '';
        var nombreTramite2 = '';

        if (tr_id === '1') {
            nombreTramite = 'Renovación de Licencia';
            nombreTramite2 = 'Renovación de Licencia';

        } else if (tr_id === '3') {
            nombreTramite = 'Duplicado de Licencia';
            nombreTramite2 = 'Duplicado de Licencia';

        } else if (tr_id === '4') {
            nombreTramite = 'Licencia de Reciprocidad';
            nombreTramite2 = 'Licencia de Reciprocidad';

        }
        $('#nombreTramite2').text(nombreTramite);
        $('#nombreTramite3').text(nombreTramite2);

    });

    // function actualizarNombreCliente(nombreCompleto) {
    //     $('.nombreCliente').text(nombreCompleto);
    // }

    function obtenerHistorial(cl_id, tr_id) {
        console.log("ingresoa obtenerHistorial", cl_id, tr_id);
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/RenovLic/obtenerDatosForm/' + cl_id + '/' + tr_id,
            //url: 'https://api.tulicenciapr.com/api/RenovLic/obtenerDatosForm/60/1',
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log("Datacreaaaa", data);
                console.log("Datacreaaaa", data.item);

                //CLIENTE
                $('#fechaCliente').text(formatearFecha(data.item.cl_cliente.cl_fechaNacimiento));
                $('#nombreCliente').text(data.item.cl_cliente.cl_nombre + ' ' + data.item.cl_cliente.cl_primerApellido);

                //PAGO
                $('#codigoPago').text(data.item.cl_pago.pg_codigo);
                $('#fechaPago').text(formatearFecha(data.item.cl_pago.pg_fecha));

                //FORM
                $('#fechaForm').text(formatearFecha(data.item.fecha));

                // $('#modalHistorial .modal-body').html('<pre>' + JSON.stringify(data, null, 2) + '</pre>');
                $('#modalHistorial').modal('show');
            },
        });
    }

    function formatearFecha(fecha) {
        var fechaFormateada = new Date(fecha);
        return (fechaFormateada.getMonth() + 1).toString().padStart(2, '0') + '/' + fechaFormateada.getDate().toString().padStart(2, '0') + '/' + fechaFormateada.getFullYear();
    }

    function obtenerAsignacion(frm_id, tr_id) {
        console.log("ingresoa obtenerHistorial");
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Asignacion/ObtenerAsignacion/' + frm_id + '/' + tr_id,
            //url: 'https://api.tulicenciapr.com/api/Asignacion/ObtenerAsignacion/168/1',
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: (xhr) => {
                xhr.setRequestHeader('Authorization', 'Bearer ' + jwtToken);
            },
            success: function (data) {
                console.log("Datacreaaaa", data);

                if (data.items.length > 0) {
                    var primerCaso = data.items[0];
                    if (primerCaso !== null && primerCaso !== undefined) {
                        $('#fechaReviewd').text(formatearFecha(primerCaso.asig_fecha));
                        $('#fechaReviewd2').text(formatearFecha(primerCaso.asig_fecha));

                        $('#nombre1').text(primerCaso.adm_id1.adm_nombres);
                        $('#nombre11').text(primerCaso.adm_id1.adm_nombres);
                    }

                    var segundoCaso = data.items[1];
                    if (segundoCaso !== null && segundoCaso !== undefined) {
                        $('#fechaProcess').text(formatearFecha(segundoCaso.asig_fecha));
                        $('#nombre2').text(segundoCaso.adm_id2.adm_nombres);
                        $('#nombre22').text(segundoCaso.adm_id2.adm_nombres);
                    }

                    var tercerCaso = data.items[2];
                    if (tercerCaso !== null && tercerCaso !== undefined) {
                        $('#fechaClosed').text(formatearFecha(tercerCaso.asig_fecha));
                        $('#nombre3').text(tercerCaso.adm_id2.adm_nombres);
                    }
                }
            },
        });
    }

    function obtenerLineaTiempo(frm_id, tr_id) {
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Asignacion/ObtenerLineaTiempo/' + frm_id + '/' + tr_id,
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: (xhr) => {
                xhr.setRequestHeader('Authorization', 'Bearer ' + jwtToken);
            },
            success: function (response) {
                console.dir(response);
                var tmpl = function (fecha, titulo, descripcion) {
                    var template = '<li><div class="timeline-time text-end"><span class="float-end badge bg-success-transparent text-muted timeline-badge fs-12">' + formatearFecha(fecha) + '</span></div>';
                    template += '<div class="timeline-icon"><a href="javascript:void(0);"></a></div><div class="timeline-body"><div class="d-flex align-items-top timeline-main-content flex-wrap mt-0">';
                    template += '<div class="flex-fill"><div class="d-flex align-items-center"><div class="mt-sm-0 mt-2"><p class="mb-0 fs-14 fw-semibold">' + titulo + '</p><p class="mb-0 text-muted">' + descripcion + '</p></div><div class="ms-auto"></div></div></div></div></div></li>';
                    return template;
                }

                if (response.success) {
                    $('img', '#modalHistorial').addClass('d-none');
                    var linesTimeHtml = '';
                    var maxLen = response.data.length;
                    for (var i = 0; i < (maxLen - 1); i++) {
                        var item = response.data[i];
                        linesTimeHtml += tmpl(item.fecha, item.titulo, item.descripcion);
                    }
                    $('#lineatiempo').html(linesTimeHtml);
                    if (maxLen == 9) {
                        $('img', '#modalHistorial').attr('src', response.data[8].descripcion);
                        $('img', '#modalHistorial').removeClass('d-none');
                    }
                }

                $('#modalHistorial').modal('show');
            },
        });
    }
})


// ESTADO   FORMATO
function formatoEstado(value, row, index) {
    switch (value) {
        case 0:
            return 'NEWCASE';
        case 1:
            return 'REVIEWCASE';
        case 2:
            return 'PROCESSCASE';
        case 3:
            return 'CLOSEDCASE';
        default:
            return 'Unknown';
    }
}

function asignarClaseEstado(value, row, index) {
    switch (value) {
        case 0:
            return { classes: 'estado0' };
;
        case 1:
            return { classes: 'estado1' };
        case 2:
            return { classes: 'estado2' };
        case 3:
            return { classes: 'estado2' };
        default:
            return { classes: '' };
    }
}

// ESTADO  PROCESO FORMATO
function formatoEstadoProceso(value, row, index) {
    switch (value) {
        case 0:
            return 'PENDIENTE';
        case 1:
            return 'APROBADO';
        case 2:
            return 'DENEGADO';
        default:
            return 'Unknown';
    }
}

function asignarClaseEstadoProceso(value, row, index) {
    console.log('estado_proceso', value)
    switch (value) {
        case 0:
            return { classes: 'text-warning' };
        case 1:
            return { classes: 'text-success' };
        case 2:
            return { classes: 'text-danger' };
        default:
            return { classes: '' };
    }
}

function verPdf(trId, frmId) {
    var url = baseApiUrlEndPoint + '/pdf/consolidado/' + trId + '/' + frmId;
    var urlVisor = urlPdfViewer + '?url=' + url;

    $('iframe', '#previewModal').attr('src', urlVisor);
    $('#btnDescargarPDF').attr('href', url);
    $('#previewModal').modal('show');
}