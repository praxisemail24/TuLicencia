$(document).ready(function () {
    var pgId = 0;

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

    // Tipo tramite
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

    var selectEstadoProceso = document.getElementById('selectEstadoProceso');
    var btnEnvioClosed = document.getElementById('btnEnvioClosed');

    //selectEstadoProceso.addEventListener('change', function () {
    //    if (selectEstadoProceso.value === '1' || selectEstadoProceso.value === '2' || selectEstadoProceso.value === '3') {
    //        btnEnvioClosed.disabled = false;
    //    } else {
    //        btnEnvioClosed.disabled = true;
    //    }
    //});
    btnEnvioClosed.disabled = true;

    var motivoAnulacionContainer = document.getElementById('motivoAnulacionContainer');
    //var bodyArchivos = document.getElementById('bodyArchivos');

    selectEstadoProceso.addEventListener('change', function () {
        $('#observacionSubsanarContainer').css({ display: 'none' });
        if (selectEstadoProceso.value === '3') {
            motivoAnulacionContainer.style.display = 'block';
            bodyArchivos.style.display = 'none';
        }
        else if (selectEstadoProceso.value === '2') {
            motivoAnulacionContainer.style.display = 'none';
            bodyArchivos.style.display = 'none';
            $('#observacionSubsanarContainer').css({ display: 'block' });
        }        
        else {
            motivoAnulacionContainer.style.display = 'none';
            bodyArchivos.style.display = 'block';
        }
    });

    // DATOS DEL CASO
    jQuery.ajax({
        url: baseApiUrlEndPoint + '/RenovLic/obtenerDatosForm/' + cl_id + '/' + tr_id + '/' + frm_id,
        type: "GET",
        //data: null,
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        success: function (data) {
            console.log(data.item);
            var item = data.item;
            $("#frmNombre").text(item.cl_cliente.cl_nombre);
            $("#frmPrimerApellido").text(item.cl_cliente.cl_primerApellido);
            $("#frmSegundoApellido").text(item.cl_cliente.cl_segundoApellido);
            $("#frmCorreo").text(item.cl_cliente.cl_correo);
            $("#frmTelefono").text(item.cl_cliente.cl_numeroTelefono);


            $("#frmcodigoPago").text(item.cl_pago.pg_codigo);
            $("#selectEstadoProceso").val(item.estadoProceso);
            var fechaPago = new Date(item.cl_pago.pg_fecha);
            var formatoFechaPago = ("0" + (fechaPago.getMonth() + 1)).slice(-2) + "/" + ("0" + fechaPago.getDate()).slice(-2) + "/" + fechaPago.getFullYear();
            $("#frmFechaPago").text(formatoFechaPago);
            console.log("item.cl_pago.pg_fecha", item.cl_pago.pg_fecha);

            pgId = item.cl_pago.pg_id;

            obtenerArchivo(pgId);
        },
    });

    // DATOS DEL ASIGNADO
    jQuery.ajax({
        url: baseApiUrlEndPoint + '/Asignacion/ObtenerAsignacion/' + frm_id + '/' + tr_id,
        type: "GET",
        //data: null,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            console.log('ObtenrAsignacion', data);

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

            $("#statusactual").text(primerCaso.statusactual);
            $("#motivoAnulacion").val(primerCaso.motivoAnulacion);
            $("#observacionSubsanar").val(primerCaso.motivoAnulacion);

            $('#adm_id1').val(primerCaso.adm_id1.adm_id);
            $('#adm_id2').val(primerCaso.adm_id2.adm_id);


            if ($("#statusactual").text() !== 'Pendiente') { // Uso de text() como función
                btnEnvioClosed.disabled = false;
            } else {
                btnEnvioClosed.disabled = true;
            }


            var bodyArchivos1 = document.getElementById('bodyArchivos');


            var motivoAnulacionContainer = document.getElementById('motivoAnulacionContainer');
            $('#observacionSubsanarContainer').css({ display: 'none' });                    
           
            if ($("#statusactual").text() === 'Denegado/Final') {       
                bodyArchivos1.style.display = 'none';
                motivoAnulacionContainer.style.display = 'block';
            } else if ($("#statusactual").text() === 'Denegado/Subsanable') {
                   
                bodyArchivos1.style.display = 'none';
                motivoAnulacionContainer.style.display = 'none';
                $('#observacionSubsanarContainer').css({ display: 'block' });
            } else {
                bodyArchivos1.style.display = 'block';
                motivoAnulacionContainer.style.display = 'none';
            }
           
        },
    });

    // SELECCION DEL PROCESO
    $('#btGuardarSelect').click(function () {
        GuardarSelectProceso();
    });

    function GuardarSelectProceso() {
        var estadoProcess = $('.form-select').val();
        var motivo = null;

        if (estadoProcess === '2') {
            motivo = $('#observacionSubsanar').val();
        }

        if (estadoProcess === '3') {
            motivo = $('#motivoAnulacion').val();
        }

        var data = {
            estadoProcess: estadoProcess,
            motivo,
        };

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
                jQuery.ajax({
                    url: baseApiUrlEndPoint + '/RenovLic/cambioEstadoProcessForm/' + tr_id + '/' + frm_id + '/' + estadoProcess,
                    type: 'PUT',
                    data: JSON.stringify(data),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data2) {
                        console.log(data2);
                        if (data2.success) {
                            Swal.fire({
                                title: "Confirmado!",
                                text: data2.message,
                                icon: "success",
                                timer: 1500
                            }).then(() => {
                                location.reload(); // Recargar la página después de confirmar
                            });
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        Swal.fire({
                            title: "Error!",
                            text: "Hubo un error al intentar actualizar el estado del proceso.",
                            icon: "error"
                        });
                    }
                });
            }
        });
    }


    // SUBIR LICENCIA

    $('#divImagen').on('click', 'a.btn-editar', function (e) {
        console.log("clic en el botón editar");
        console.log(e);
        e.stopPropagation()
        e.preventDefault();
        var file = $(this).siblings('.custom-file-input');
        file.click();
    });

    $('#divImagen').on('change', '.custom-file-input', function (e) {
        console.log(e);
        var id = $(this).siblings('.btn-editar').attr('id');
        var ar_id_pos = $(this).attr('id');
        var file = e.target.files[0];
        agregarArchivo(id, ar_id_pos, file);
    });

    $('#divImagen').on('click', 'a.btn-eliminar', function (e) {
        console.log("clic en el botón eliminar");

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
                                timer: 1300
                            });
                        }
                    },
                    error: function (error) {
                        console.error('Error al realizar la solicitud DELETE:', error);
                    }
                });
            }
        });
    });

    $('#divImagen').on('click', '.btn-preview', function (e) {
        console.log($(this));
        e.stopPropagation()
        e.preventDefault();
        var archivoId = $(this).data('id');
        var url = $(this).data('url');

        $('#exampleModal').modal('show');
        $('#imgURL').attr('src', url);
    });

    function obtenerArchivo(frm_id) {
        if (frm_id <= 0) {
            return;
        } 


        $('#divImagen').empty();
        console.log("clic frm_id", frm_id);
        jQuery.ajax({
            url: baseApiUrlEndPoint + '/Archivo/' + frm_id,
            type: "GET",
            data: null,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log("Data de Obtener ARchivo", data)
                var descripcion = "";
                var item = null;
                if (data.items.length === 0) {
                    descripcion = 'Licencia';
                    llenarhtml(null, descripcion, 0, 8);
                } else {
                    item = data.items.find(t => t.ar_pos == 8);
                    if (item) {
                        descripcion = 'Licencia';
                        llenarhtml(item.ar_nombre, descripcion, item.ar_id, item.ar_pos);
                    }
                    else {
                        descripcion = 'Licencia';
                        llenarhtml(null, descripcion, 0, 8);
                    }
                }
            },
            error: function (error) {
                console.log(error)
            },
            // beforeSend: function () {
            // },
        });
    }

    function agregarArchivo(ar_id, ar_pos, file) {
        console.log("agregarArchivo", ar_id);

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
            ar_pos: 8,
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
                if (data.success) {
                    Swal.fire({
                        title: "Se agrego archivo!",
                        text: data.message,
                        icon: "success",
                        timer: 2000
                    });
                }
                obtenerArchivo(pgId);
            },
            error: function (error) {
                console.log(error)
            },
            // beforeSend: function () {
            // },
        });
    }

    function llenarhtml(url, name, id, pos) {
        if (url == null)
            url = "https://tulicenciapr.com/upload/0default1.png";
        // if (url == null && pos == 1)
        //     url = "";
        var htmldelete = "";
        console.log(id);
        if (id > 0)
            htmldelete = '<a href="javascript:void(0);" data-id="' + id + '" id="' + id + '" class="btn-eliminar btn btn-wave btn-danger-light btn-icon contact-delete"><i class="ri-delete-bin-line"></i></a>';
        var html = '<div class="col-xxl-2 col-xl-4 col-lg-3 col-md-3 col-sm-12">' +
            '<div class="card custom-card hrm-main-card primary team-member-card">' +
            '<div class="teammember-cover-image"> ' +
            '<img src="' + url + '" class="card-img-top" alt="..."> ' + '</div>' +
            '<div class="card-body p-0">' +
            '<div class="d-flex flex-wrap align-item-center mt-sm-0 mt-5 justify-content-between border-bottom border-block-end-dashed p-3">' + '</div>' +
            '<div class="team-member-stats d-sm-flex justify-content-evenly">' +
            '<div class="text-center p-3 my-auto"  style="height: 71px;">' +
            '<p class="fw-bold mb-0">' + name + '</p>' + '</div>' + '</div>' + '</div>' +
            '<div class="card-footer border-block-start-dashed text-center">' +
            '<div class="btn-list">' +
            '<div class="btn-list">' +
            '<input type="file" class="custom-file-input" id="' + pos + '" style="display: none;"> ' +
            '<a href="javascript:void(0);" data-id="' + id + '" id="btn-preview-' + id + '" data-url="' + url + '" class="btn-preview btn btn-wave btn-primary-light btn-icon"><i class="ri-eye-line"></i></a>' +
            '<a href="javascript:void(0);" data-id="' + id + '" id="' + id + '" class="btn-editar btn btn-wave btn-info-light btn-icon"><i class="ri-pencil-line"></i></a>' +
            htmldelete +
            '</div></div></div></div></div>';
        $('#divImagen').append(html);
    }


    /// MODAL PDF

    $('#btnVerPDF').click(function () {
        var frmId = $('#frm_id').val()
        var trId = $('#tr_id').val()
        var url = baseApiUrlEndPoint + '/pdf/consolidado/' + trId + '/' + frmId;
        var urlVisor = urlPdfViewer + '?url=' + baseApiUrlEndPoint + '/pdf/consolidado/' + trId + '/' + frmId;
        $('#pdfPreview').attr('src', urlVisor);
        $('#btnDescargarPDF').attr('href', url);
        $('#previewModal').modal('show');     
    });

    let globalPDFBytes = null;
    function downloadPDF() {
        download(globalPDFBytes, "documento-final2.pdf", "application/pdf");
        bootstrap.Modal.getInstance(document.getElementById("previewModal")).hide();
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

    //document.getElementById("btnDescargarPDF").addEventListener("click", downloadPDF);
    //document.getElementById("btnGuardarBD").addEventListener("click", grabarCasoPDF);

    function grabarCasoPDF() {
        const pdfBlob = new Blob([globalPDFBytes], { type: 'application/pdf' });
        const pdfFile = new File([pdfBlob], 'documento-final.pdf', {
            type: 'application/pdf',
            lastModified: new Date()
        });
        var formData = new FormData();
        formData.append('file', pdfFile); // Cambiado de 'pdf' a 'file' para coincidir con la expectativa común del servidor

        var objArchivo = {
            ar_id: 0,
            ar_pos: 9,
            cl_cliente: { cl_id: cl_id },
            tr_tramite: { tr_id: tr_id },
            frm_id: frm_id,
            ar_estado: 1,
            ar_fecha: new Date(),
        };
        formData.append('item', JSON.stringify(objArchivo));

        $.ajax({
            url: baseApiUrlEndPoint + '/Archivo/Upload',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            mimeType: "multipart/form-data",
            success: function (data) {
                Swal.fire({
                    title: "Se agrego archivo a BD",
                    text: data.message,
                    icon: "success",
                    timer: 2000
                });
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    // ASIGNAR A CLOSED

    $('#btnEnvioClosed').click(function () {
        ConfirmarCaso();
    });

    function ConfirmarCaso() {
        Swal.fire({
            title: '¿Estás seguro?',
            text: "¿Quieres finalizar este caso?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, finalizar!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {

            if (result.isConfirmed) {
                var tr_id = $('#tr_id').val();
                var frm_id = $('#frm_id').val();
                var adm_id1 = $('#adm_id2').val();
                var adm_id2 = $('#adm_id2').val();
                var asig_fecha = new Date();
                var asig_Activo = true;

                var objConfirmar = {
                    asig_id: 0,
                    tr_id: { tr_id: tr_id },
                    frm_id: { frmID: frm_id },
                    adm_id1: { adm_id: adm_id1 },
                    adm_id2: { adm_id: adm_id2 },
                    asig_fecha: asig_fecha,
                    asig_Activo: asig_Activo,
                };

                var motivoAnulacion = $('#motivoAnulacion').val();

                var data = {
                    motivo: ($("#statusactual").text() === 'Denegado/Final') ? motivoAnulacion : null
                };

                jQuery.ajax({
                    url: baseApiUrlEndPoint + '/Asignacion/',
                    type: 'POST',
                    data: JSON.stringify(objConfirmar),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.success) {
                            jQuery.ajax({
                                url: baseApiUrlEndPoint + '/RenovLic/cambioEstadoForm/' + tr_id + '/' + frm_id + '/' + 3,
                                type: 'PUT',
                                data: JSON.stringify(data),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (data2) {
                                    console.log(data2);
                                    if (data2.success) {
                                        Swal.fire({
                                            title: "Confirmado!",
                                            text: data2.message,
                                            icon: "success",
                                            timer: 2000
                                        });
                                        setTimeout(function () {
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
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
});