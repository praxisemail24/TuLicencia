$(document).ready(function () {
    var id = $('#id').val();
    var tr_id = $('#tr_id').val();
    // Cargar datos de evaluación por ID
    $.ajax({
        url: baseApiUrlEndPoint + `/Doctor/GetEvaluacion/${id}/${tr_id}`,
        type: 'GET',
        headers: {
            'Authorization': `Bearer ${jwtToken}`
        },
        success: function (data) {
            if (data.success) {
                $('#nombreCliente').text(data.item.nombreCliente);
                $('#segundoNombreCliente').text(data.item.segundoNombreCliente);
                $('#apellidoPaternoCliente').text(data.item.apellidoPaternoCliente);
                $('#apellidoMaternoCliente').text(data.item.apellidoMaternoCliente);
                $('#seguroSocial').text(data.item.seguroSocial);
                $('#licenciaConducir').text(data.item.licenciaConducir);

                // Asignar valores a los campos del formulario
                $('#ojoDerechoSinLentes').val(data.item.ojoDerechoSinLentes);
                $('#ojoIzquierdoSinLentes').val(data.item.ojoIzquierdoSinLentes);
                $('#ojoDerechoConLentes').val(data.item.ojoDerechoConLentes);
                $('#ojoIzquierdoConLentes').val(data.item.ojoIzquierdoConLentes);
                $('#fcm_estado').val(data.item.estado); // Asegúrate de usar minúsculas

                var fechaEvaluacion = data.item.fechaEvaluacion.split('T')[0]; // Formatear la fecha
                $('#fcm_fecha').val(fechaEvaluacion);

                $('#fcm_nombreMedico').val(data.item.nombreMedico);
                $('#fcm_licenciaMedico').val(data.item.licenciaMedico);

                $('#fcm_condicion').val(data.item.condicion);
                $('#fcm_condi_con_lentes').val(data.item.condisionConLentes);
                $('#fcm_condi_sin_lentes').val(data.item.condisionSinLentes);
                $('#fcm_ambosOjos').val(data.item.ambosOjos);
                $('#fcm_espejuelos').val(data.item.espejuelos);
                $('#fcm_usaLentes').val(data.item.usaLentes);
                $('#fcm_observacion').val(data.item.observacion);
                $('#fcm_condicionOido').val(data.item.condicionOido);
                $('#fcm_condicionBrazo').val(data.item.condicionBrazo);
                $('#fcm_condicionPierna').val(data.item.condicionPierna);
                $('#fcm_condicionFisica').val(data.item.condicionFisica);
                $('#fcm_estadoInconciencia').val(data.item.estadoInconciencia);
                $('#fcm_padeceCorazon').val(data.item.padeceCorazon);
                $('#fcm_marcapaso').val(data.item.marcapaso);
                $('#fcm_protesis').val(data.item.protesis);
                $('#fcm_peso').val(data.item.peso);
                $('#fcm_colorOjo').val(data.item.colorOjo);
                $('#fcm_colorPelo').val(data.item.colorPelo);
                $('#estaturaPies').val(data.item.estaturaPies);
                $('#estaturaPulgadas').val(data.item.estaturaPulgadas);

            } else {
                console.error('Error al cargar los datos de evaluación:', data.message);
            }
        },
        error: function (error) {
            console.error('Error al realizar la solicitud:', error);
        }
    });

    // Enviar formulario para guardar la evaluación
    $('#formEvaluacion').submit(function (e) {
        e.preventDefault();

        var formData = {
            id: id,
            tr_id: tr_id,


            nombreCliente: $('#nombreCliente').text(),
            segundoNombreCliente: $('#segundoNombreCliente').text(),
            apellidoPaternoCliente: $('#apellidoPaternoCliente').text(),
            apellidoMaternoCliente: $('#apellidoMaternoCliente').text(),
            seguroSocial: $('#seguroSocial').text(),
            licenciaConducir: $('#licenciaConducir').text(),

            ojoDerechoSinLentes: $('#ojoDerechoSinLentes').val(),
            ojoIzquierdoSinLentes: $('#ojoIzquierdoSinLentes').val(),
            ojoDerechoConLentes: $('#ojoDerechoConLentes').val(),
            ojoIzquierdoConLentes: $('#ojoIzquierdoConLentes').val(),
            // ... otros campos
            Estado: $('#fcm_estado').val(),
            fechaEvaluacion: $('#fcm_fecha').val(),
            nombreMedico: $('#fcm_nombreMedico').val(),
            licenciaMedico: $('#fcm_licenciaMedico').val(),
            condicion: $('#fcm_condicion').val(),
            CondisionConLentes: $('#fcm_condi_con_lentes').val(),
            CondisionSinLentes: $('#fcm_condi_sin_lentes').val(),
            ambosOjos: $('#fcm_ambosOjos').val(),
            espejuelos: $('#fcm_espejuelos').val(),
            usaLentes: $('#fcm_usaLentes').val(),
            observacion: $('#fcm_observacion').val(),
            condicionOido: $('#fcm_condicionOido').val(),
            condicionBrazo: $('#fcm_condicionBrazo').val(),
            condicionPierna: $('#fcm_condicionPierna').val(),
            condicionFisica: $('#fcm_condicionFisica').val(),
            estadoInconciencia: $('#fcm_estadoInconciencia').val(),
            padeceCorazon: $('#fcm_padeceCorazon').val(),
            marcapaso: $('#fcm_marcapaso').val(),
            protesis: $('#fcm_protesis').val(),
            peso: $('#fcm_peso').val(),
            colorOjo: $('#fcm_colorOjo').val(),
            colorPelo: $('#fcm_colorPelo').val(),
            estaturaPies: $('#estaturaPies').val(),
            estaturaPulgadas: $('#estaturaPulgadas').val()
        };

        $.ajax({
            url: baseApiUrlEndPoint+ '/Doctor/GuardarEvaluacion',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            headers: {
                'Authorization': `Bearer ${jwtToken}`
            },
            success: function (response) {
                if (response.success) {
                    window.location.href = 'https://tulicenciapr.com/admin/Home/Doctor/'+ userAuth.id;
                    alert('Evaluación guardada correctamente');
                } else {
                    alert('Error al guardar la evaluación: ' + response.message);
                }
            },
            error: function (error) {
                console.error('Error al realizar la solicitud:', error);
                alert('Error al guardar la evaluación');
            }
        });
    });
});
