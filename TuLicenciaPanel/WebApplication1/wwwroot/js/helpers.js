const configDataTables = () => {
    if (window['DataTable']) {
        Object.assign(DataTable.defaults, {
            destroy: true,
            processing: true,
            serverSide: true,
            responsive: true,
            ajax: {
                type: 'POST',
                dataType: 'json',
                //contentType: 'application/json',
            },
            language: {
                url: 'https://tulicenciapr.com/admin/es-MX.json',
            },
            searching: false,
        });
    }
}

const dateFormat = (date, format = 'datetime') => {
    if (date === null || date === undefined) {
        return '';
    }

    switch (format) {
        case 'date':
            format = 'DD/MM/YYYY';
            break;
        case 'time':
            format = 'hh:mm:ss a';
            break;
        case 'datetime':
            format = 'DD/MM/YYYY hh:mm:ss a';
            break;
        default:
            break;
    }

    return moment(date).format(format);
}

const renderCellDateTime = (data, type, row, meta) => {
    return '<div class="d-flex flex-column"><span>' + dateFormat(data, 'date') + '</span><small>' + dateFormat(data, 'time') + '</small></div>';
}

const renderCellCustomer = (fullName, email, phone) => {
    var html = '<div class="d-flex flex-column">';
    html += '<div class="d-flex flex-row align-items-center">' + fullName + '</div>'
    html += '<div class="d-flex flex-row align-items-center"><i class="la la-envelope text-warning"></i><span class="ms-2">'+email+'</span></div>'
    html += '<div class="d-flex flex-row align-items-center"><i class="la la-phone text-info"></i><span class="ms-2">'+phone+'</span></div>'
    html += '</div>';
    return html;
}

const renderCellStatus = (status, doctor) => {
    let estadoClass = '';
    let estadoFontSize = 'font-size: medium;'; // Tamaño de fuente para el estado
    let doctorFontSize = 'font-size: x-small;'; // Tamaño de fuente para el doctor

    switch (status) {
        case 'Pendiente':
            estadoClass = 'badge bg-warning-transparent';
            break;
        case 'Asignado':
            estadoClass = 'badge bg-info-transparent';
            break;
        case 'Aprobado':
            estadoClass = 'badge bg-success-transparent';
            break;
        case 'Denegado':
            estadoClass = 'badge bg-danger-transparent';
            break;
        case 'Unreachable':
            estadoClass = 'badge bg-primary-transparent';
            break;


        case '-':
            estadoClass = '';
            break;
        default:
            estadoClass = 'bg-secondary text-white p-2';
            break;
    }

    var html = '<div>';
    html += `<div class="${estadoClass}">${status}</div>`;

    if (doctor && doctor.trim() !== '') {
        html += `<div class="d-flex flex-row align-items-center" style="${doctorFontSize}"><i class="la la-user-md text-info"></i><span class="ms-2">${doctor}</span></div>`;
    }

    html += '</div>';
    return html;
};

//const renderCellStatus = (status, doctor) => {
//    let estadoClass = '';

//    switch (status) {
//        case 'Pendiente':
//            estadoClass = 'badge bg-warning-transparent';
//            break;
//        case 'Asignado':
//            estadoClass = 'badge bg-info-transparent';
//            break;
//        case 'Aprobado':
//            estadoClass = 'badge bg-success-transparent';
//            break;
//        case 'Denegado':
//            estadoClass = 'badge bg-danger-transparent';
//            break;
//        case 'Unreacheble':
//            estadoClass = 'badge bg-primary-transparent';
//            break;
//        case '-':
//            estadoClass = '';
//            break;
//        default:
//            estadoClass = 'bg-secondary text-white p-2';
//            break;
//    }

//    var html = '<div class="d-flex flex-column">';
//    html += `<div class="d-flex flex-row align-items-center ${estadoClass}" >${status}</div>`;
//    html += `<div class="d-flex flex-row align-items-center"><i class="la la-user-md text-info"></i><span class="ms-2">${doctor}</span></div>`;
//    html += '</div>';
//    return html;
//};

const tmplProgressBar = (percent = 50) => {
    return '<div class="pt-5"><div class="progress progress-lg mb-5 custom-progress-4 progress-animate" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"><div class="progress-bar" style="width: '+percent+'%"><div class="progress-bar-value">'+percent+'%</div></div></div></div>';
}

const loaderJS = () => {

}

const onPrint = (e) => {
    try {
        const tagName = e.target.tagName;
        const printJs = document.getElementById('printJS');
        if (printJs) {
            printJs.remove();
        }

        var url = (tagName === 'SPAN' || tagName === 'I') ? e.target.parentElement.getAttribute('data-print') : e.target.getAttribute('data-print');
        if (url) {
            printJS({ printable: url, type: 'pdf', showModal: true, modalMessage: 'Obteniendo documento...' })
        }
    } catch (e) {
        toastr.error(`Error al intentar obtener archivo: ${e}`);
    }
}

const checkEvaluationStatus = (frmId, trId) => {
    $.ajax({
        type: 'POST',
        url: baseApiUrlEndPoint + '/Evaluacion/CheckStatus/' + frmId + '/' + trId,
        success: (response) => {
            console.log('RESPONSE:::::::::::',response);

            if (!response.success) {
                $('.alert', '#sectionEvaluation').addClass('alert-danger').text('Trámite sin evaluación.');
                $('.btn-primary', '#sectionEvaluation').addClass('d-none');
            } else {
                $('.btn-primary', '#sectionEvaluation').removeClass('d-none');
                if (response.status === 'pending') {
                    $('.alert', '#sectionEvaluation').addClass('alert-warning').text('La evaluación ha sido generada.');
                } else if (response.status === 'proccess') {
                    $('.alert', '#sectionEvaluation').addClass('alert-info').text('La evaluación se encuentra pendiente.');
                } else {
                    $('.alert', '#sectionEvaluation').addClass('alert-success').text('La evaluación ha sido completada.');
                }
                $('iframe', '#evaluationStatusModal').attr('src', urlPdfViewer + '?url=' + baseApiUrlEndPoint + response.file);
            }
        }
    })
}

const ImagesManager = (trId, selector = '#divImagen') => {
    var fileNames = {
        "1": {
            "1": "Tarjeta de Seguro Social",
            "2": "Foto ID (Expirada o por expirar) - Frontal",
            "5": "Foto ID(Expirada o por expirar) – Posterior",
            "3": "Recibo de Luz o Estado Bancario",
            "4": "Certificado de Nacimiento o Prueba de Residencia",
            /*"6": "Copia de Plantilla o W2",*/
            "10": "Foto Selfie",
            "20": "Certificación de Authorización",
            "50": "Evaluación Médica",
            "30": "Firma",
        },
        "3": {
            "1": "Tarjeta de Seguro Social",
            "2": "Foto ID - Anverso",
            "5": "Foto ID - Reverso",
            "3": "Recibo de Luz o Estado Bancario",
            "4": "Declaración Jurada ante Notario Público",
            "6": "Certificado de Nacimiento o Prueba de Residencia",
            "10": "Foto Selfie",
            "20": "Certificación de Authorización",
            "50": "Evaluación Médica",
            "30": "Firma",
        },
        "4": {
            "1": "Tarjeta de Seguro Social",
            "2": "Certificado de Nacimiento",
            "6": "Record Choferil de su estado de procedencia",
            "3": "Licencia vigente de su país – Frontal",
            "5": "Licencia vigente de su país -  Posterior",
            "4": "Recibo de Agua o Estado Bancario",
            "10": "Foto Selfie",
            "20": "Certificación de Authorización",
            "30": "Firma",
            "50": "Evaluación"
        },
        "5": {
            "1": "Copia del licencia del vehículo",
            "2": "Copia de licencia de conducir de vendedor",
            "3": "Copia de licencia de conducir del comprador",
            "4": "Título de propiedad (Opcional)",
            "5": "Contrato de compraventa (Bill of Sale)",
            "6": "Declaración jurada (Opcional)",
        }
    }

    const getFileGroup = () => {
        return (fileNames[trId] && typeof fileNames === 'object') ? fileNames[trId] : {}
    }

    const getFileName = (id) => {
        id = id.toString();
        var _fileNames = getFileGroup(trId);
        return _fileNames[id] ? _fileNames[id] : "";
    }

    const upload = (ar_id, ar_pos, file, pg_id) => {
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
            pg_id,
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
            },
        });
    }

    const fillHtml = (url, name, id, pos) => {
        var isDefault = false;
        if (url == null) {
            isDefault = true;
            url = "https://tulicenciapr.com/upload/0default1.png";
        }

        var tipo = 'img';
        
        if ((pos === 20 || pos === 50 || (pos == 5 && parseInt(trId) === 5) || url.endsWith('.pdf')) && isDefault === false) {
            tipo = 'iframe';
        }

        var card = $('<div class="col-sm-12 col-md-4 col-lg-3 col-xl-2"></div>');
        var cardContainer = $(`<div class="card custom-card hrm-main-card primary team-member-card" id="card-file-${pos}"></div>`);
        var cardHeader = $('<div class="teammember-cover-image"></div>');
        var cardBody = $('<div class="card-body p-0"></div>');
        var cardFooter = $('<div class="card-footer border-block-start-dashed text-center"></div>');

        var filePreview = $(`<${tipo} class="card-img-top" alt="..." src="${url}" style="object-fit: contain; height: 200px;" />`);
        var fileTitle = $(`<div class="team-member-stats d-sm-flex justify-content-evenly"><div class= "text-center p-3 my-auto" style="height: 87px;"><p class="fw-bold mb-0">${name}</p></div></div>`);
        var fileOptions = $('<div class="btn-list"></div>');

        var input = $(`<input type="file" accept="image/*;application/pdf" class="custom-file-input" data-pos="${pos}" data-id="${id}" id="${pos}" style="display: none;">`);
        var btnView = $(`<a href="javascript:void(0);" data-type="${tipo}" data-pos="${pos}" data-id="${id}" id="btn-preview-${id}" data-url="${url}" class="btn-preview btn btn-wave btn-primary-light btn-icon btn-sm"><i class="ri-eye-line"></i></a>`);
        var btnUpload = $(`<a href="javascript:void(0);" data-type="${tipo}" data-pos="${pos}" data-id="${id}" id="${id}" class="btn-editar btn btn-wave btn-info-light btn-icon btn-sm"><i class="ri-upload-line"></i></a>`);

        fileOptions.append(input);
        if (id > 0) {
            fileOptions.append(btnView);
        }
        fileOptions.append(btnUpload);
        if (id > 0) {
            var btnDelete = $(`<a href="javascript:void(0);" data-type="${tipo}" data-pos="${pos}" data-id="${id}" id="${id}" class="btn-eliminar btn btn-wave btn-danger-light btn-icon btn-sm contact-delete"><i class="ri-delete-bin-line"></i></a>`);
            fileOptions.append(btnDelete);
        }

        cardHeader.append(filePreview);
        cardBody.append(fileTitle);
        cardFooter.append(fileOptions);

        cardContainer.append(cardHeader);
        cardContainer.append(cardBody);
        cardContainer.append(cardFooter);
        card.append(cardContainer);

        $(selector).addClass('row');
        $(selector).append(card);
    }

    const findFile = (files, id) => {
        id = parseInt(id);
        files = files.filter(f => f.ar_pos === id);
        return files.length === 0 ? { ar_id: 0, ar_nombre: null, ar_pos: id,  } : files[0];
    }

    const renderImg = (files, id) => {
        id = parseInt(id);
        var file = findFile(files, id);
        var url = typeof file.ar_url === 'string' ? file.ar_url : file.ar_nombre; 
        var name = typeof file.ar_url === 'string' ? file.ar_nombre : getFileName(id); 
        fillHtml(url, name, file.ar_id, id);
    }

    const render = (files) => {
        $(selector).empty();
        if (trId === "1") {
            renderImg(files, 1);
            renderImg(files, 2);
            renderImg(files, 5);
            renderImg(files, 3);
            renderImg(files, 4);
          /*  renderImg(files, 6);*/
            renderImg(files, 10);
            renderImg(files, 20);
            renderImg(files, 50);
            renderImg(files, 30);
        }
        if (trId === "3") {
            renderImg(files, 1);
            renderImg(files, 2);
            renderImg(files, 5);
            renderImg(files, 3);
            renderImg(files, 4);
            renderImg(files, 6);
            renderImg(files, 10);
            renderImg(files, 20);
            renderImg(files, 30);
            renderImg(files, 50);
        }
        if (trId === "4") {
            renderImg(files, 1);
            renderImg(files, 2);
            renderImg(files, 6);
            renderImg(files, 3);
            renderImg(files, 5);
            renderImg(files, 4);
            renderImg(files, 10);
            renderImg(files, 30);
            renderImg(files, 20);
        }
        if (trId === "5") {
            renderImg(files, 1);
            renderImg(files, 2);
            renderImg(files, 3);
            renderImg(files, 4);
            renderImg(files, 5);
            renderImg(files, 6);
        }
    }

    return {
        getFileName,
        fillHtml,
        findFile,
        renderImg,
        render,
        upload,
    }
}

const fileToBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = reject;
});

const Notifications = () => {
    const templateItem = (item) => {
        var html = '<li class="dropdown-item"><div class="d-flex align-items-start">';
        html += '<div class="pe-2"><span class="avatar avatar-md bg-success-transparent avatar-rounded"><i class="ti ti-clock fs-18"></i></span></div>';
        html += '<div class="flex-grow-1 d-flex align-items-center justify-content-between"><div>';
        html += '<p class="mb-0 fw-semibold"><a href="javascript: void(0);">'+item.title+' <span class="text-success">#'+item.id+'</span></a></p>';
        html += '<span class="text-muted fw-normal fs-12 header-notification-text">'+item.description+'</span></div></div></div></li>';

        return html;
    }

    const loadAsync = () => {
        $('#notification-icon-badge').val(0);
        $.ajax({
            url: baseApiUrlEndPoint + '/Notification/' + userAuth.id + '/tramites',
            type: 'POST',
            beforeSend: (xhr) => {
                xhr.setRequestHeader('Authorization', 'Bearer ' + jwtToken);
            },
            success: (response) => {
                if (response.success) {
                    $('#notification-icon-badge').text(response.data.length);
                    response.data.forEach((item) => {
                        $('#header-notification-scroll').append($(templateItem(item)));
                    });
                    $('#notifiation-data').text(response.data.length + ' trámites');
                } else {
                    toastr.error(response.message)
                }
            }
        })
    }

    loadAsync();
}

Notifications();

$('#frmCategoria').on('change', (e) => {
    var value = $(e.target).val();

    if (value === 'Chofer') {
        $('#frmVehiculoPesado').removeAttr('disabled');
    } else {
        $('#frmVehiculoPesado').attr('disabled', 'disabled');
    }
});

$(document).ready(() => {
    setTimeout(() => {
        var value = $('#frmCategoria').val();

        if (value === 'Chofer') {
            $('#frmVehiculoPesado').removeAttr('disabled');
        } else {
            $('#frmVehiculoPesado').attr('disabled', 'disabled');
        }
    }, 3000);
});
