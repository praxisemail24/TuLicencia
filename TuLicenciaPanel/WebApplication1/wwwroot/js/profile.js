var Profile = () => {
    const init = () => {
        $('#rol').val(userAuth.nivel).trigger('change');

        $('#btnUpdateProfile').on('click', update);
        $('#btnChangePwd').on('click', changePwd);
    }

    const update = async() => {
        var signed = null;
        if ($('#signed').get(0).files.length > 0) {
            signed = await fileToBase64($('#signed').get(0).files[0]);
        }

        $.ajax({
            type: 'PUT',
            url: `${baseApiUrlEndPoint}/administrador`,
            contentType: 'application/json',
            data: JSON.stringify({
                adm_id: $('#profile-id').val(),
                adm_user: $('#usr').val(),
                adm_nombres: $('#fullname').val(),
                adm_nivel: $('#rol').val(),
                adm_email: $('#email').val(),
                FirmaBytes: signed,
                adm_est: 1,
            }),
            success(response) {
                if (response.success) {
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            }
        })
    }

    const changePwd = ()  => {
        $.ajax({
            type: 'POST',
            url: `${baseApiUrlEndPoint}/administrador/change_password`,
            contentType: 'application/json',
            data: JSON.stringify({
                id: userAuth.id,
                password: $('#pwd').val(),
                repeatPassword: $('#repeat-pwd').val(),
            }),
            success(response) {
                if (response.success) {
                    toastr.success(response.message);
                    $('#pwd').val('');
                    $('#repeat-pwd').val('');
                } else {
                    toastr.error(response.message);
                }
            }
        });
    }

    return {
        init,
    }
}

Profile().init()