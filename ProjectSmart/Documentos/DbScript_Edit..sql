CREATE DATABASE TuLicenciaPR;

SELECT * FROM Estados;

SELECT * FROM Administrador;
SELECT * FROM Manejo_casos;
SELECT * FROM Archivos WHERE frm_id  =168;
SELECT * FROM Pueblos;
SELECT * FROM Pagos;
SELECT * FROM Cliente WHERE cl_correo = 'riverareynaangel@gmail.com';
SELECT * FROM Pagos WHERE cl_id =60 and tr_id=1;
SELECT * FROM Tramites;
SELECT * FROM TipoTramites;
SELECT * FROM Frm_RenovacionLicencia;
SELECT * FROM Frm_DuplicadoLicencia;
SELECT * FROM Frm_LicenciaReciprocidad;
SELECT * FROM Frm_CertificadoMedico;
SELECT * FROM Frm_RenovacionLicencia frl WHERE frl.cl_id = 60 AND frl.tr_id = 1;
SELECT * FROM Asignacion WHERE frm_id =168
;



   
   
ALTER  PROCEDURE spPanel_buscarAdministrador
    @adm_email VARCHAR(255) = NULL,    @adm_user VARCHAR(255) = NULL,
    @adm_nivel INT = NULL,    @adm_est INT = NULL
AS BEGIN
    SET NOCOUNT ON;
    SELECT 
    	adm_id,        adm_user,        adm_clv,        adm_nombres,
        adm_est,        adm_nivel,        adm_fech_reg,        adm_email
    FROM         TuLicencia.dbo.Administrador
    WHERE
        (@adm_email IS NULL OR adm_email LIKE '%' + @adm_email + '%') AND
        (@adm_user IS NULL OR adm_user LIKE '%' + @adm_user + '%') AND
        (case when @adm_nivel = 0 then adm_nivel else @adm_nivel end) = adm_nivel AND
        (case when @adm_est = 99 then adm_est else @adm_est end) = adm_est;
END
















ALTER PROCEDURE spPanel_ActualizarFormularioCliente
    @frm_id int,  @tr_id int,   @frm_estado int, @frm_tipoLicencia nvarchar(100),
    @frm_numeroLicencia NVARCHAR(25), @frm_categoria NVARCHAR(20), @frm_vehiculoPesado NVARCHAR(10),
	@frm_identificacion NVARCHAR(50), @frm_numero NVARCHAR(25), @frm_statusLegal NVARCHAR(255),
	@frm_genero NVARCHAR(10), @frm_donante NVARCHAR(2), @frm_tipoSangre NVARCHAR(20), @frm_talla NVARCHAR(20),
	@frm_peso NVARCHAR(20), @frm_tez NVARCHAR(20), @frm_colorPelo NVARCHAR(20), @frm_colorOjo NVARCHAR(20),
	@frm_direccion NVARCHAR(255), @frm_numeroDireccion NVARCHAR(10), @frm_pueblo NVARCHAR(100),
	@frm_codigoPostal INT, @frm_barrio NVARCHAR(255), @frm_apartado NVARCHAR(255), @frm_pueblo2 NVARCHAR(100),
	@frm_codigoPostal2 INT, @frm_licenciaSuspendida NVARCHAR(2), @frm_motivoSuspension NVARCHAR(50),
	@frm_recluido NVARCHAR(2), @frm_convictoBebida NVARCHAR(2), @frm_fechaConvictoBebida DATE, @frm_convictoNarcotico NVARCHAR(2),
	@frm_fechaConvictoNarcotico DATE, @frm_obligacionAlimentaria NVARCHAR(2), @frm_deudaAcca NVARCHAR(2),
	
	@frm_tipoVehiculo NVARCHAR(50), @frm_paisProcede NVARCHAR(50), @frm_estadoProcede NVARCHAR(50),
    @frm_numeroLicencia2 NVARCHAR(25), @frm_fechaExpiracion DATE, @frm_numeroIdentificacion NVARCHAR(50),
	@frm_nombrePadre NVARCHAR(255), @frm_nombreMadre NVARCHAR(255), @frm_numeroLicenciaPR NVARCHAR(25),
	@frm_estadoRevision int, @frm_estadoProceso int, 
    @Resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    IF @tr_id = 1
    BEGIN        
        UPDATE Frm_RenovacionLicencia  
        SET
        frl_estado = @frm_estado,  frl_tipoLicencia = @frm_tipoLicencia,            
        frl_numeroLicencia = @frm_numeroLicencia,  frl_categoria = @frm_categoria, frl_identificacion = @frm_identificacion,  frl_numero = @frm_numero,
        frl_statusLegal = @frm_statusLegal, frl_genero = @frm_genero, frl_donante = @frm_donante, frl_tipoSangre = @frm_tipoSangre,
		frl_talla = @frm_talla,  frl_peso = @frm_peso, frl_tez = @frm_tez, frl_colorPelo = @frm_colorPelo, frl_colorOjo = @frm_colorOjo,
        frl_direccion = @frm_direccion, frl_numeroDireccion = @frm_numeroDireccion, frl_pueblo = @frm_pueblo, frl_codigoPostal = @frm_codigoPostal,
        frl_barrio = @frm_barrio, frl_apartado = @frm_apartado, frl_pueblo2 = @frm_pueblo2, frl_codigoPostal2 = @frm_codigoPostal2,
        frl_licenciaSuspendida = @frm_licenciaSuspendida, frl_motivoSuspension = @frm_motivoSuspension, frl_recluido = @frm_recluido,
        frl_convictoBebida = @frm_convictoBebida, frl_fechaConvictoBebida = @frm_fechaConvictoBebida, frl_convictoNarcotico = @frm_convictoNarcotico,
        frl_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, frl_obligacionAlimentaria = @frm_obligacionAlimentaria,
        frl_deudaAcca = @frm_deudaAcca, frl_vehiculoPesado = @frm_vehiculoPesado,  frl_estadoProceso = @frm_estadoProceso, frl_estadoRevision= @frm_estadoRevision		
        WHERE
            frl_id = @frm_id;
    END
    ELSE IF @tr_id = 2
    BEGIN
        UPDATE Frm_LicenciaAprendizaje
        SET
            fla_estado = @frm_estado

        WHERE  fla_id = @frm_id;
    END
    ELSE IF @tr_id = 3
    BEGIN
        UPDATE Frm_DuplicadoLicencia
        SET
        fdl_estado = @frm_estado, fdl_tipoLicencia = @frm_tipoLicencia, fdl_numeroLicencia = @frm_numeroLicencia,  fdl_categoria = @frm_categoria,
		fdl_identificacion = @frm_identificacion,  fdl_numero = @frm_numero, fdl_statusLegal = @frm_statusLegal, fdl_genero = @frm_genero,
		fdl_donante = @frm_donante, fdl_tipoSangre = @frm_tipoSangre,
		fdl_talla = @frm_talla,  fdl_peso = @frm_peso, fdl_tez = @frm_tez, fdl_colorPelo = @frm_colorPelo, fdl_colorOjo = @frm_colorOjo,
        fdl_direccion = @frm_direccion, fdl_numeroDireccion = @frm_numeroDireccion, fdl_pueblo = @frm_pueblo, fdl_codigoPostal = @frm_codigoPostal,
        fdl_barrio = @frm_barrio, fdl_apartado = @frm_apartado, fdl_pueblo2 = @frm_pueblo2, fdl_codigoPostal2 = @frm_codigoPostal2,
        fdl_licenciaSuspendida = @frm_licenciaSuspendida, fdl_motivoSuspension = @frm_motivoSuspension, fdl_recluido = @frm_recluido,
        fdl_convictoBebida = @frm_convictoBebida, fdl_fechaConvictoBebida = @frm_fechaConvictoBebida, fdl_convictoNarcotico = @frm_convictoNarcotico,
        fdl_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, fdl_obligacionAlimentaria = @frm_obligacionAlimentaria,
        fdl_deudaAcca = @frm_deudaAcca, fdl_vehiculoPesado = @frm_vehiculoPesado,fdl_estadoProceso = @frm_estadoProceso, fdl_estadoRevision= @frm_estadoRevision	
        WHERE
            fdl_id = @frm_id;
    END
    ELSE IF @tr_id = 4
    BEGIN
        UPDATE Frm_LicenciaReciprocidad
        SET
            flr_estado = @frm_estado,  flr_tipoLicencia = @frm_tipoLicencia,            
        flr_numeroLicencia = @frm_numeroLicencia,  flr_categoria = @frm_categoria, flr_identificacion = @frm_identificacion,
        flr_statusLegal = @frm_statusLegal, flr_genero = @frm_genero, flr_donante = @frm_donante, flr_tipoSangre = @frm_tipoSangre,
		flr_talla = @frm_talla,  flr_peso = @frm_peso, flr_tez = @frm_tez, flr_colorPelo = @frm_colorPelo, flr_colorOjo = @frm_colorOjo,
        flr_direccion = @frm_direccion, flr_numeroDireccion = @frm_numeroDireccion, flr_pueblo = @frm_pueblo, flr_codigoPostal = @frm_codigoPostal,
        flr_barrio = @frm_barrio, flr_apartado = @frm_apartado, flr_pueblo2 = @frm_pueblo2, flr_codigoPostal2 = @frm_codigoPostal2,
        flr_licenciaSuspendida = @frm_licenciaSuspendida, flr_motivoSuspencion = @frm_motivoSuspension, flr_recluido = @frm_recluido,
        flr_convictoBebida = @frm_convictoBebida, flr_fechaConvictoBebida = @frm_fechaConvictoBebida, flr_convictoNarcotico = @frm_convictoNarcotico,
        flr_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, flr_obligacionAlimentaria = @frm_obligacionAlimentaria,
        flr_deudaAcca = @frm_deudaAcca, 
                
        flr_tipoVehiculo = @frm_tipoVehiculo, flr_paisProcede = @frm_paisProcede, flr_estadoProcede = @frm_estadoProcede,
        flr_numeroLicencia2 = @frm_numeroLicencia2, flr_fechaExpiracion = @frm_fechaExpiracion, flr_numeroIdentificacion = @frm_numeroIdentificacion,
	    flr_nombrePadre = @frm_nombrePadre, flr_nombreMadre = @frm_nombreMadre, flr_numeroLicenciaPR = @frm_numeroLicenciaPR,
	    flr_estadoProceso = @frm_estadoProceso, flr_estadoRevision= @frm_estadoRevision	
        WHERE
            flr_id = @frm_id;
           
    END
    ELSE
    BEGIN
        PRINT 'El valor de tr_id no es válido';
    END
    SET @Resultado = @@ROWCOUNT;
END;













ALTER PROCEDURE spPanel_BuscarRegistro1
    @cl_nombre nvarchar(255) = NULL,    @cl_primerApellido nvarchar(255) = NULL,    @cl_segundoApellido nvarchar(255) = NULL,
    @cl_correo nvarchar(255) = NULL,    @cl_numeroTelefono nvarchar(255) = NULL,    @tr_id int = NULL,
    @pg_fecha datetime = NULL,    @pg_codigo nvarchar(255) = NULL,    @frm_estado int = NULL,    
    @frm_estadoProceso int = NULL, @pstart int, @plimit int, @Resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;   
   SELECT @Resultado =count(*)
    FROM         TuLicencia.dbo.Cliente c
    INNER JOIN   TuLicencia.dbo.Pagos p ON c.cl_id = p.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_RenovacionLicencia frl ON  frl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON  fdl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON  flr.cl_id = c.cl_id
    WHERE  (LOWER(c.cl_nombre) LIKE '%' + LOWER(@cl_nombre) + '%')
       AND (LOWER(c.cl_primerApellido) LIKE '%' + LOWER(@cl_primerApellido) + '%')
       AND (LOWER(c.cl_segundoApellido) LIKE '%' + LOWER(@cl_segundoApellido) + '%')
       AND (LOWER(c.cl_correo) LIKE '%' + LOWER(@cl_correo) + '%')
       AND ( LOWER(p.pg_codigo) LIKE '%' + LOWER(@pg_codigo) + '%')
       AND (CASE WHEN @tr_id = 0 THEN p.tr_id else @tr_id END)  = p.tr_id
       AND (CASE WHEN @frm_estadoProceso = 99 THEN p.tr_id 
       			 when @tr_id = 1 THEN frl.frl_estadoProceso 
       			 when @tr_id = 3 THEN fdl.fdl_estadoProceso 
       			 when @tr_id = 4 THEN flr.flr_estadoProceso END) = 
       	   (CASE WHEN @tr_id = 1 THEN p.tr_id else  @frm_estadoProceso END)
       AND (CASE WHEN @frm_estado = 99 THEN p.tr_id 
       			 when @tr_id = 1 THEN frl.frl_estado 
       			 when @tr_id = 3 THEN fdl.fdl_estado
       			 when @tr_id = 4 THEN flr.flr_estado END) = 
       	   (CASE WHEN @frm_estado = 99 THEN p.tr_id else  @frm_estado END);  

    SELECT 
        c.cl_id,       CONCAT(c.cl_nombre, ' ', c.cl_primerApellido, ' ', c.cl_segundoApellido) AS nombreCliente, 
        c.cl_nombre,   c.cl_primerApellido,   c.cl_segundoApellido,  c.cl_correo,  c.cl_numeroTelefono,
        p.tr_id,       p.pg_fecha,         p.pg_codigo,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_id  WHEN p.tr_id = 3 THEN fdl.fdl_id  WHEN p.tr_id = 4 THEN flr.flr_id  ELSE NULL 
        END AS frm_id,
        CASE 
            WHEN p.tr_id = 1 THEN 'Renovacion de Licencia' WHEN p.tr_id = 3 THEN 'Duplicado de Licencia' WHEN p.tr_id = 4 THEN 'Licencia de Reciprocidad' ELSE NULL 
        END AS nombreTramite,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_estado WHEN p.tr_id = 3 THEN fdl.fdl_estado WHEN p.tr_id = 4 THEN flr.flr_estado ELSE NULL 
        END AS frm_estado,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_estadoProceso WHEN p.tr_id = 3 THEN fdl.fdl_estadoProceso WHEN p.tr_id = 4 THEN flr.flr_estadoProceso ELSE NULL 
        END AS frm_estadoProceso
    FROM         TuLicencia.dbo.Cliente c
    INNER JOIN   TuLicencia.dbo.Pagos p ON c.cl_id = p.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_RenovacionLicencia frl ON  frl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON  fdl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON  flr.cl_id = c.cl_id
    WHERE  (LOWER(c.cl_nombre) LIKE '%' + LOWER(@cl_nombre) + '%')
       AND (LOWER(c.cl_primerApellido) LIKE '%' + LOWER(@cl_primerApellido) + '%')
       AND (LOWER(c.cl_segundoApellido) LIKE '%' + LOWER(@cl_segundoApellido) + '%')
       AND (LOWER(c.cl_correo) LIKE '%' + LOWER(@cl_correo) + '%')
       AND ( LOWER(p.pg_codigo) LIKE '%' + LOWER(@pg_codigo) + '%')
       AND (CASE WHEN @tr_id = 0 THEN p.tr_id else @tr_id END)  = p.tr_id
       AND (CASE WHEN @frm_estadoProceso = 99 THEN p.tr_id 
       			 when @tr_id = 1 THEN frl.frl_estadoProceso 
       			 when @tr_id = 3 THEN fdl.fdl_estadoProceso 
       			 when @tr_id = 4 THEN flr.flr_estadoProceso END) = 
       	   (CASE WHEN @tr_id = 1 THEN p.tr_id else  @frm_estadoProceso END)
       AND (CASE WHEN @frm_estado = 99 THEN p.tr_id 
       			 when @tr_id = 1 THEN frl.frl_estado 
       			 when @tr_id = 3 THEN fdl.fdl_estado
       			 when @tr_id = 4 THEN flr.flr_estado END) = 
       	   (CASE WHEN @frm_estado = 99 THEN p.tr_id else  @frm_estado END)
       	   
    ORDER BY p.pg_fecha DESC 
    OFFSET @pstart ROWS
    FETCH NEXT @plimit ROWS ONLY;
END;






SELECT * from Pagos p 
SELECT * from Pagos p 


SELECT * from Pagos p 





SELECT
    p.pg_id,  p.tr_id, p.pg_fecha, p.pg_codigo, CONCAT(c.cl_nombre, ' ', c.cl_primerApellido, ' ', c.cl_segundoApellido) AS nombreCliente,
    COALESCE(frl.frl_id, fdl.fdl_id, flr.flr_id) AS frm_id,
    c.cl_id, c.cl_nombre,    c.cl_primerApellido,    c.cl_segundoApellido,
    c.cl_correo, c.cl_numeroTelefono, 
    COALESCE(frl.frl_fecha, fdl.fdl_fecha, flr.flr_fecha) AS frm_fecha,
    COALESCE(frl.frl_estado, fdl.fdl_estado, flr.flr_estado) AS frm_estado,
	COALESCE(frl.frl_estadoProceso, fdl.fdl_estadoProceso, flr.flr_estadoProceso) AS frm_estadoProceso,
	CASE 
        WHEN p.tr_id = 1 THEN 'Renovacion de Licencia' WHEN p.tr_id = 3 THEN 'Duplicado de Licencia' WHEN p.tr_id = 4 THEN 'Licencia de Reciprocidad' ELSE NULL 
       END AS nombreTramite
FROM
    TuLicencia.dbo.Pagos p
INNER JOIN
    TuLicencia.dbo.Cliente c ON p.cl_id = c.cl_id
LEFT JOIN
    TuLicencia.dbo.Frm_RenovacionLicencia frl ON p.pg_id = frl.pg_id
LEFT JOIN
    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON p.pg_id = fdl.pg_id
LEFT JOIN
    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON p.pg_id = flr.pg_id
WHERE
    COALESCE(frl.frl_id, fdl.fdl_id, flr.flr_id) IS NOT NULL;

DECLARE @pstart INT = 0;
DECLARE @plimit INT = 10;
DECLARE @Resultado INT;

EXEC spPanel_BuscarRegistro1 
    @tr_id = 0,
    @cl_nombre = '',
    @cl_primerApellido = '',
    @cl_segundoApellido = '',
    @cl_correo = '',
    @cl_numeroTelefono  = '',
    @pg_codigo ='',
    @frm_estado =99,
    @frm_estadoProceso =99,
    @pstart = @pstart,
    @plimit = @plimit,
    @Resultado = @Resultado OUTPUT;

SELECT @Resultado AS 'Total de registros encontrados';



ALTER PROCEDURE spPanel_BuscarRegistro1
    @cl_nombre nvarchar(255) = NULL,    @cl_primerApellido nvarchar(255) = NULL,    @cl_segundoApellido nvarchar(255) = NULL,
    @cl_correo nvarchar(255) = NULL,    @cl_numeroTelefono nvarchar(255) = NULL,    @tr_id int = NULL,
    @pg_fecha datetime = NULL,    @pg_codigo nvarchar(255) = NULL,    @frm_estado int = NULL,    
    @frm_estadoProceso int = NULL, @pstart int, @plimit int, @Resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;   
   SELECT @Resultado =count(*)
    FROM
    TuLicencia.dbo.Pagos p
INNER JOIN
    TuLicencia.dbo.Cliente c ON p.cl_id = c.cl_id
LEFT JOIN
    TuLicencia.dbo.Frm_RenovacionLicencia frl ON p.pg_id = frl.pg_id
LEFT JOIN
    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON p.pg_id = fdl.pg_id
LEFT JOIN
    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON p.pg_id = flr.pg_id
WHERE
    COALESCE(frl.frl_id, fdl.fdl_id, flr.flr_id) IS NOT NULL

    AND  (LOWER(c.cl_nombre) LIKE '%' + LOWER(@cl_nombre) + '%')
       AND (LOWER(c.cl_primerApellido) LIKE '%' + LOWER(@cl_primerApellido) + '%')
       AND (LOWER(c.cl_segundoApellido) LIKE '%' + LOWER(@cl_segundoApellido) + '%')
       AND (LOWER(c.cl_correo) LIKE '%' + LOWER(@cl_correo) + '%')
       AND ( LOWER(p.pg_codigo) LIKE '%' + LOWER(@pg_codigo) + '%')
       AND (CASE WHEN @tr_id = 0 THEN p.tr_id else @tr_id END)  = p.tr_id
       AND (CASE WHEN @frm_estadoProceso = 99 THEN p.tr_id 
       			 when @tr_id = 1 THEN frl.frl_estadoProceso 
       			 when @tr_id = 3 THEN fdl.fdl_estadoProceso 
       			 when @tr_id = 4 THEN flr.flr_estadoProceso END) = 
       	   (CASE WHEN @tr_id = 1 THEN p.tr_id else  @frm_estadoProceso END)
       AND (CASE WHEN @frm_estado = 99 THEN p.tr_id 
       			 when @tr_id = 1 THEN frl.frl_estado 
       			 when @tr_id = 3 THEN fdl.fdl_estado
       			 when @tr_id = 4 THEN flr.flr_estado END) = 
       	   (CASE WHEN @frm_estado = 99 THEN p.tr_id else  @frm_estado END)

    SELECT
    p.pg_id,  p.tr_id, p.pg_fecha, p.pg_codigo, CONCAT(c.cl_nombre, ' ', c.cl_primerApellido, ' ', c.cl_segundoApellido) AS nombreCliente,
    COALESCE(frl.frl_id, fdl.fdl_id, flr.flr_id) AS frm_id,
    c.cl_id, c.cl_nombre,    c.cl_primerApellido,    c.cl_segundoApellido, c.cl_correo, c.cl_numeroTelefono, 
    COALESCE(frl.frl_fecha, fdl.fdl_fecha, flr.flr_fecha) AS frm_fecha,
    COALESCE(frl.frl_estado, fdl.fdl_estado, flr.flr_estado) AS frm_estado,
	COALESCE(frl.frl_estadoProceso, fdl.fdl_estadoProceso, flr.flr_estadoProceso) AS frm_estadoProceso,
	CASE 
        WHEN p.tr_id = 1 THEN 'Renovacion de Licencia' WHEN p.tr_id = 3 THEN 'Duplicado de Licencia' WHEN p.tr_id = 4 THEN 'Licencia de Reciprocidad' ELSE NULL 
       END AS nombreTramite
FROM
    TuLicencia.dbo.Pagos p
INNER JOIN

































    TuLicencia.dbo.Cliente c ON p.cl_id = c.cl_id
LEFT JOIN
    TuLicencia.dbo.Frm_RenovacionLicencia frl ON p.pg_id = frl.pg_id
LEFT JOIN
    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON p.pg_id = fdl.pg_id
LEFT JOIN
    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON p.pg_id = flr.pg_id
WHERE
    COALESCE(frl.frl_id, fdl.fdl_id, flr.flr_id) IS NOT NULL
    and  (LOWER(c.cl_nombre) LIKE '%' + LOWER(@cl_nombre) + '%')
       AND (LOWER(c.cl_primerApellido) LIKE '%' + LOWER(@cl_primerApellido) + '%')
       AND (LOWER(c.cl_segundoApellido) LIKE '%' + LOWER(@cl_segundoApellido) + '%')
       AND (LOWER(c.cl_correo) LIKE '%' + LOWER(@cl_correo) + '%')
       AND ( LOWER(p.pg_codigo) LIKE '%' + LOWER(@pg_codigo) + '%')
       AND (CASE WHEN @tr_id = 0 THEN p.tr_id else @tr_id END)  = p.tr_id
       --AND (CASE WHEN @frm_estadoProceso = 99 THEN p.tr_id 
       --			 when @tr_id = 1 THEN frl.frl_estadoProceso 
       	--		 when @tr_id = 3 THEN fdl.fdl_estadoProceso 
       	--		 when @tr_id = 4 THEN flr.flr_estadoProceso END) = 
       	--   (CASE WHEN @tr_id = 1 THEN p.tr_id else  @frm_estadoProceso END)
       --AND (CASE WHEN @frm_estado = 99 THEN p.tr_id 
       	--		 when @tr_id = 1 THEN frl.frl_estado 
       	--		 when @tr_id = 3 THEN fdl.fdl_estado
       	--		 when @tr_id = 4 THEN flr.flr_estado END) = 
       	--   (CASE WHEN @frm_estado = 99 THEN p.tr_id else  @frm_estado END)
       	   
    ORDER BY p.pg_fecha DESC 
    OFFSET @pstart ROWS
    FETCH NEXT @plimit ROWS ONLY;
END;





   












EXEC spPanel_BuscarRegistro1 
    @cl_nombre = '', 
    @cl_primerApellido = '', 
    @cl_segundoApellido = 'chamorro', 
    @cl_correo = '', 
    @cl_numeroTelefono = '', 
    @tr_id = NULL, 
    @pg_fecha = NULL, 
    @pg_codigo = '', 
    @frm_estado = null, 
    @frm_estadoProceso = NULL,
    @pstart = @PageNumber,
    @plimit = @PageSize,
   @Resultado = 0;

ALTER PROCEDURE spPanel_BuscarRegistro1
    @cl_nombre nvarchar(255) = NULL,    @cl_primerApellido nvarchar(255) = NULL,    @cl_segundoApellido nvarchar(255) = NULL,
    @cl_correo nvarchar(255) = NULL,    @cl_numeroTelefono nvarchar(255) = NULL,    @tr_id int = NULL,
    @pg_fecha datetime = NULL,    @pg_codigo nvarchar(255) = NULL,    @frm_estado int = NULL,    
    @frm_estadoProceso int = NULL, @pstart int, @plimit int, @Resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
   
   SELECT @Resultado =count(*)
    FROM         TuLicencia.dbo.Cliente c
    INNER JOIN   TuLicencia.dbo.Pagos p ON c.cl_id = p.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_RenovacionLicencia frl ON  frl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON  fdl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON  flr.cl_id = c.cl_id
    WHERE  (LOWER(c.cl_nombre) LIKE '%' + LOWER(@cl_nombre) + '%')
       AND (LOWER(c.cl_primerApellido) LIKE '%' + LOWER(@cl_primerApellido) + '%')
       AND (LOWER(c.cl_segundoApellido) LIKE '%' + LOWER(@cl_segundoApellido) + '%')
       AND (LOWER(c.cl_correo) LIKE '%' + LOWER(@cl_correo) + '%')
      -- AND (TRIM(c.cl_numeroTelefono) = @cl_numeroTelefono) 
      -- AND (p.tr_id = @tr_id)
       --AND (CONVERT(date, p.pg_fecha) = CONVERT(date, @pg_fecha))
       AND ( LOWER(p.pg_codigo) LIKE '%' + LOWER(@pg_codigo) + '%');
   
    SELECT 
        c.cl_id,       CONCAT(c.cl_nombre, ' ', c.cl_primerApellido, ' ', c.cl_segundoApellido) AS nombreCliente, 
        c.cl_nombre,   c.cl_primerApellido,   c.cl_segundoApellido,  c.cl_correo,  c.cl_numeroTelefono,
        p.tr_id,       p.pg_fecha,         p.pg_codigo,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_id  WHEN p.tr_id = 3 THEN fdl.fdl_id  WHEN p.tr_id = 4 THEN flr.flr_id  ELSE NULL 
        END AS frm_id,
        CASE 
            WHEN p.tr_id = 1 THEN 'Renovacion de Licencia' WHEN p.tr_id = 3 THEN 'Duplicado de Licencia' WHEN p.tr_id = 4 THEN 'Licencia de Reciprocidad' ELSE NULL 
        END AS nombreTramite,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_estado WHEN p.tr_id = 3 THEN fdl.fdl_estado WHEN p.tr_id = 4 THEN flr.flr_estado ELSE NULL 
        END AS frm_estado,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_estadoProceso WHEN p.tr_id = 3 THEN fdl.fdl_estadoProceso WHEN p.tr_id = 4 THEN flr.flr_estadoProceso ELSE NULL 
        END AS frm_estadoProceso
    FROM         TuLicencia.dbo.Cliente c
    INNER JOIN   TuLicencia.dbo.Pagos p ON c.cl_id = p.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_RenovacionLicencia frl ON  frl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON  fdl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON  flr.cl_id = c.cl_id
    WHERE  (LOWER(c.cl_nombre) LIKE '%' + LOWER(@cl_nombre) + '%')
    
       AND (LOWER(c.cl_primerApellido) LIKE '%' + LOWER(@cl_primerApellido) + '%')
       AND (LOWER(c.cl_segundoApellido) LIKE '%' + LOWER(@cl_segundoApellido) + '%')
       AND (LOWER(c.cl_correo) LIKE '%' + LOWER(@cl_correo) + '%')
      -- AND (TRIM(c.cl_numeroTelefono) = @cl_numeroTelefono) 
      -- AND (p.tr_id = @tr_id)
       --AND (CONVERT(date, p.pg_fecha) = CONVERT(date, @pg_fecha))
       AND ( LOWER(p.pg_codigo) LIKE '%' + LOWER(@pg_codigo) + '%')
      -- AND ( ( (p.tr_id = 1 AND frl.frl_estado = @frm_estado) OR (p.tr_id = 3 AND fdl.fdl_estado = @frm_estado) OR (p.tr_id = 4 AND flr.flr_estado = @frm_estado)))
      -- AND (@frm_estadoProceso IS NULL OR ((p.tr_id = 1 AND frl.frl_estadoProceso = @frm_estadoProceso) OR (p.tr_id = 3 AND fdl.fdl_estadoProceso = @frm_estadoProceso) 
      --	OR  (p.tr_id = 4 AND flr.flr_estadoProceso = @frm_estadoProceso)))
    ORDER BY p.pg_fecha DESC 
    OFFSET @pstart ROWS
    FETCH NEXT @plimit ROWS ONLY;
END;




CREATE PROCEDURE spPanel_BuscarRegistroExcel1
    @cl_nombre nvarchar(255) = NULL,    @cl_primerApellido nvarchar(255) = NULL,    @cl_segundoApellido nvarchar(255) = NULL,
    @cl_correo nvarchar(255) = NULL,    @cl_numeroTelefono nvarchar(255) = NULL,    @tr_id int = NULL,
    @pg_fecha datetime = NULL,    @pg_codigo nvarchar(255) = NULL,    @frm_estado int = NULL,    
    @frm_estadoProceso int = NULL
AS BEGIN
    SET NOCOUNT ON;
   
    SELECT 
        c.cl_id,       CONCAT(c.cl_nombre, ' ', c.cl_primerApellido, ' ', c.cl_segundoApellido) AS nombreCliente, 
        c.cl_nombre,   c.cl_primerApellido,   c.cl_segundoApellido,  c.cl_correo,  c.cl_numeroTelefono,
        p.tr_id,       p.pg_fecha,         p.pg_codigo,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_id  WHEN p.tr_id = 3 THEN fdl.fdl_id  WHEN p.tr_id = 4 THEN flr.flr_id  ELSE NULL 
        END AS frm_id,
        CASE 
            WHEN p.tr_id = 1 THEN 'Renovacion de Licencia' WHEN p.tr_id = 3 THEN 'Duplicado de Licencia' WHEN p.tr_id = 4 THEN 'Licencia de Reciprocidad' ELSE NULL 
        END AS nombreTramite,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_estado WHEN p.tr_id = 3 THEN fdl.fdl_estado WHEN p.tr_id = 4 THEN flr.flr_estado ELSE NULL 
        END AS frm_estado,
        CASE 
            WHEN p.tr_id = 1 THEN frl.frl_estadoProceso WHEN p.tr_id = 3 THEN fdl.fdl_estadoProceso WHEN p.tr_id = 4 THEN flr.flr_estadoProceso ELSE NULL 
        END AS frm_estadoProceso
    FROM         TuLicencia.dbo.Cliente c
    INNER JOIN   TuLicencia.dbo.Pagos p ON c.cl_id = p.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_RenovacionLicencia frl ON  frl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_DuplicadoLicencia fdl ON  fdl.cl_id = c.cl_id
    LEFT JOIN    TuLicencia.dbo.Frm_LicenciaReciprocidad flr ON  flr.cl_id = c.cl_id
    WHERE  (LOWER(c.cl_nombre) LIKE '%' + LOWER(@cl_nombre) + '%')
    
       AND (LOWER(c.cl_primerApellido) LIKE '%' + LOWER(@cl_primerApellido) + '%')
       AND (LOWER(c.cl_segundoApellido) LIKE '%' + LOWER(@cl_segundoApellido) + '%')
       AND (LOWER(c.cl_correo) LIKE '%' + LOWER(@cl_correo) + '%')
      -- AND (TRIM(c.cl_numeroTelefono) = @cl_numeroTelefono) 
      -- AND (p.tr_id = @tr_id)
       --AND (CONVERT(date, p.pg_fecha) = CONVERT(date, @pg_fecha))
       AND ( LOWER(p.pg_codigo) LIKE '%' + LOWER(@pg_codigo) + '%')
      -- AND ( ( (p.tr_id = 1 AND frl.frl_estado = @frm_estado) OR (p.tr_id = 3 AND fdl.fdl_estado = @frm_estado) OR (p.tr_id = 4 AND flr.flr_estado = @frm_estado)))
      -- AND (@frm_estadoProceso IS NULL OR ((p.tr_id = 1 AND frl.frl_estadoProceso = @frm_estadoProceso) OR (p.tr_id = 3 AND fdl.fdl_estadoProceso = @frm_estadoProceso) 
      --	OR  (p.tr_id = 4 AND flr.flr_estadoProceso = @frm_estadoProceso)))
    ORDER BY p.pg_fecha DESC 
    
END;








-- Ejemplo de búsqueda por frm_estado
EXEC spPanel_BuscarRegistro1
    @frm_estado = 0; -- Suponiendo que 1 representa un estado específico

-- Ejemplo de búsqueda por frm_estadoProceso
EXEC spPanel_BuscarRegistro1 
    @frm_estadoProceso = 1; -- Suponiendo que 2 representa un estado de proceso específico

-- Ejemplo de búsqueda por ambos frm_estado y frm_estadoProceso
EXEC spPanel_BuscarRegistro1 
    @frm_estado = 2, 
    @frm_estadoProceso = 1; -- Suponiendo valores específicos para cada campo





-- Ejemplo de búsqueda por nombre y apellido
EXEC spPanel_BuscarRegistro1
--@cl_nombre = 'Stivo',
    @cl_primerApellido = 'Lop';

-- Ejemplo de búsqueda por correo electrónico
EXEC spPanel_BuscarRegistro 
    @cl_correo = 'juan@example.com';

-- Ejemplo de búsqueda por número de teléfono
EXEC spPanel_BuscarRegistro 
    @cl_numeroTelefono = '123456789';

-- Ejemplo de búsqueda por ID de trámite
EXEC spPanel_BuscarRegistro 
    @tr_id = 1;

-- Ejemplo de búsqueda por fecha de pago
EXEC spPanel_BuscarRegistro 
    @pg_fecha = '2024-05-22';

-- Ejemplo de búsqueda por código de pago
EXEC spPanel_BuscarRegistro 
    @pg_codigo = 'ABC123';




CREATE PROCEDURE spPanel_cambioEstadoProcesoForm
    @frl_id INT, @frl_estadoProceso INT
AS BEGIN
    UPDATE Frm_RenovacionLicencia SET frl_estadoProceso = @frl_estadoProceso WHERE frl_id = @frl_id;
END;



CREATE PROCEDURE spPanel_cambioEstadoProcesoForm
    @tr_id int, @frm_id int, @new_estado int, @Resultado INT OUTPUT
AS BEGIN
	SET NOCOUNT ON;
    IF @tr_id = 1
    BEGIN
        UPDATE Frm_RenovacionLicencia SET frl_estadoProceso = @new_estado WHERE frl_id = @frm_id;
    END
    ELSE IF @tr_id = 3
    BEGIN
        UPDATE Frm_DuplicadoLicencia SET fdl_estadoProceso = @new_estado WHERE fdl_id = @frm_id;
    END
    ELSE IF @tr_id = 4
    BEGIN
        UPDATE Frm_LicenciaReciprocidad SET flr_estadoProceso = @new_estado WHERE flr_id = @frm_id;
    END
    SET @Resultado = @@ROWCOUNT;
END;








ALTER PROCEDURE sp_RegistrarRenovLic
(@cl_id INT, @pg_id INT, @tr_id INT, @frl_fecha DATETIME, @frl_estado INT, @frl_tipoLicencia NVARCHAR(50),
@frl_numeroLicencia NVARCHAR(25), @frl_categoria NVARCHAR(20), @frl_vehiculoPesado NVARCHAR(10),
@frl_identificacion NVARCHAR(50), @frl_numero NVARCHAR(25), @frl_statusLegal NVARCHAR(255),
@frl_genero NVARCHAR(10), @frl_donante NVARCHAR(2), @frl_tipoSangre NVARCHAR(20), @frl_talla NVARCHAR(20),
@frl_peso NVARCHAR(20), @frl_tez NVARCHAR(20), @frl_colorPelo NVARCHAR(20), @frl_colorOjo NVARCHAR(20),
@frl_direccion NVARCHAR(255), @frl_numeroDireccion NVARCHAR(10), @frl_pueblo NVARCHAR(100),
@frl_codigoPostal INT, @frl_barrio NVARCHAR(255), @frl_apartado NVARCHAR(255), @frl_pueblo2 NVARCHAR(100),
@frl_codigoPostal2 INT, @frl_licenciaSuspendida NVARCHAR(2), @frl_motivoSuspension NVARCHAR(50),
@frl_recluido NVARCHAR(2), @frl_convictoBebida NVARCHAR(2), @frl_fechaConvictoBebida DATE, @frl_convictoNarcotico NVARCHAR(2),
@frl_fechaConvictoNarcotico DATE, @frl_obligacionAlimentaria NVARCHAR(2), @frl_deudaAcca NVARCHAR(2), @frl_estadoProceso INT, @frl_estadoRevision INT, @Resultado INT output)
AS BEGIN
SET NOCOUNT ON;
INSERT INTO Frm_RenovacionLicencia 
(cl_id, pg_id, tr_id, frl_fecha, frl_estado,frl_tipoLicencia, frl_numeroLicencia, frl_categoria, frl_vehiculoPesado, frl_identificacion,
frl_numero, frl_statusLegal, frl_genero, frl_donante, frl_tipoSangre, frl_talla, frl_peso, frl_tez, frl_colorPelo, frl_colorOjo,
frl_direccion, frl_numeroDireccion, frl_pueblo, frl_codigoPostal, frl_barrio, frl_apartado, frl_pueblo2, frl_codigoPostal2,
frl_licenciaSuspendida, frl_motivoSuspension, frl_recluido, frl_convictoBebida, frl_fechaConvictoBebida, frl_convictoNarcotico, frl_fechaConvictoNarcotico, 
frl_obligacionAlimentaria, frl_deudaAcca, frl_estadoProceso, frl_estadoRevision)
VALUES 
(@cl_id, @pg_id, @tr_id, @frl_fecha, @frl_estado, @frl_tipoLicencia, @frl_numeroLicencia, @frl_categoria, @frl_vehiculoPesado, @frl_identificacion,
@frl_numero, @frl_statusLegal, @frl_genero, @frl_donante, @frl_tipoSangre, @frl_talla, @frl_peso, @frl_tez, @frl_colorPelo, @frl_colorOjo,
@frl_direccion, @frl_numeroDireccion, @frl_pueblo, @frl_codigoPostal, @frl_barrio, @frl_apartado, @frl_pueblo2, @frl_codigoPostal2,
@frl_licenciaSuspendida, @frl_motivoSuspension, @frl_recluido, @frl_convictoBebida, @frl_fechaConvictoBebida, @frl_convictoNarcotico, @frl_fechaConvictoNarcotico,
@frl_obligacionAlimentaria, @frl_deudaAcca, 0,0);

UPDATE Cliente 
    SET cl_genero = @frl_genero,  cl_talla = @frl_talla, cl_peso = @frl_peso, cl_tez = @frl_tez, cl_colorPelo = @frl_colorPelo,
        cl_colorOjo = @frl_colorOjo, cl_codigoPostal = @frl_codigoPostal, cl_pueblo = @frl_pueblo
    WHERE cl_id = @cl_id;
    SET @Resultado = SCOPE_IDENTITY();
END

ALTER TABLE Frm_RenovacionLicencia
ADD frl_estadoRevision INT;
go

EXEC sp_ObtenerDatosAsignacion 168,1
CREATE PROCEDURE sp_ObtenerDatosAsignacion
    @frm_id INT, @tr_id INT
AS BEGIN
    SELECT * FROM Asignacion WHERE frm_id = @frm_id AND tr_id = @tr_id;
END;

ALTER PROCEDURE sp_ObtenerDatosAsignacion
    @frm_id INT, @tr_id INT
AS BEGIN
    SELECT 
        a.asig_id, a.frm_id, a.tr_id, a.adm_id1, a.adm_id2, adm1.adm_nombres AS adm_nombres1, 
        adm2.adm_nombres AS adm_nombres2, a.asig_fecha, a.asig_activo
    FROM Asignacion a
    INNER JOIN Administrador adm1 ON a.adm_id1 = adm1.adm_id
    INNER JOIN Administrador adm2 ON a.adm_id2 = adm2.adm_id
    WHERE a.frm_id = @frm_id AND a.tr_id = @tr_id;
END;






CREATE TABLE Asignacion (
    asig_id INT IDENTITY(1,1) NOT NULL,
    frm_id INT,
    tr_id INT,
    adm_id1 INT,
    adm_id2 INT,
    asig_fecha DATETIME,
    asig_activo BIT,
    CONSTRAINT pk_Asignacion PRIMARY KEY (asig_id),
	CONSTRAINT FK_Tramites FOREIGN KEY (tr_id) REFERENCES Tramites(tr_id),
	CONSTRAINT FK_Administrador1 FOREIGN KEY (adm_id1) REFERENCES Administrador(adm_id),
	CONSTRAINT FK_Administrador2 FOREIGN KEY (adm_id2) REFERENCES Administrador(adm_id)
);

EXEC sp_ObtenerRadicadores
CREATE PROCEDURE sp_ObtenerRadicadores
AS BEGIN
    SELECT * FROM Administrador WHERE adm_nivel = 3;
END;






ALTER PROCEDURE spPanel_ObtenerDatosCompletosCliente
    @cl_id int, @tr_id int
AS BEGIN
    SET NOCOUNT ON;

    IF @tr_id = 1
    BEGIN
        SELECT c.*, p.*, pg.*, frl_id as frm_id,  frl_estado as frm_estado,  frl_fecha as frm_fecha,
	      frl_tipoLicencia as frm_tipoLicencia,  frl_numeroLicencia as frm_numeroLicencia, frl_categoria as frm_categoria,
		  frl_vehiculoPesado as frm_vehiculoPesado, frl_identificacion as frm_identificacion, frl_numero as frm_numero,
		  frl_statusLegal as frm_statusLegal, frl_genero as frm_genero, frl_donante as frm_donante, frl_tipoSangre as frm_tipoSangre, frl_talla as frm_talla,
		  frl_peso as frm_peso, frl_tez as frm_tez, frl_colorPelo as frm_colorPelo, frl_colorOjo as frm_colorOjo, frl_direccion as frm_direccion,
		  frl_numeroDireccion as frm_numeroDireccion, frl_pueblo as frm_pueblo, frl_barrio as frm_barrio, frl_apartado as frm_apartado,
		  frl_pueblo2 as frm_pueblo2, frl_licenciaSuspendida as frm_licenciaSuspendida, frl_motivoSuspension as frm_motivoSuspension, frl_recluido as frm_recluido,
		  frl_convictoBebida as frm_convictoBebida, frl_fechaConvictoBebida as frm_fechaConvictoBebida, frl_convictoNarcotico as frm_convictoNarcotico,
		  frl_fechaConvictoNarcotico as frm_fechaConvictoNarcotico, frl_obligacionAlimentaria as frm_obligacionAlimentaria, frl_deudaAcca as frm_deudaAcca,
		  frl_serv_nec as frm_serv_nec, frl_codigoPostal2 as frm_codigoPostal2, frl_codigoPostal as frm_codigoPostal,'' as frm_tipoVehiculo, '' as frm_paisProcede,	  
		  '' as frm_estadoProcede, '' as frm_numeroLicencia2, '' as frm_fechaExpiracion,'' as frm_numeroIdentificacion, '' as frm_nombrePadre,
     	  '' as frm_nombreMadre, '' as frm_numeroLicenciaPR, frl_estadoProceso as frm_estadoProceso, frl_estadoRevision as frm_estadoRevision
        FROM Cliente c
        LEFT JOIN Pueblos p ON c.pl_id = p.pl_id
        LEFT JOIN Frm_RenovacionLicencia frl ON c.cl_id = frl.cl_id AND @tr_id = 1
        LEFT JOIN Pagos pg ON c.cl_id = pg.cl_id AND pg.tr_id = @tr_id
        WHERE c.cl_id = @cl_id;
    END
    ELSE IF @tr_id = 2
    BEGIN
        SELECT c.*, p.*, pg.*, fla.fla_id as frm_id, fla.fla_estado as frm_estado, fla.fla_fecha as frm_fecha
        FROM Cliente c
        LEFT JOIN Pueblos p ON c.pl_id = p.pl_id
        LEFT JOIN Frm_LicenciaAprendizaje fla ON c.cl_id = fla.cl_id AND @tr_id = 2
        LEFT JOIN Pagos pg ON c.cl_id = pg.cl_id AND pg.tr_id = @tr_id
        WHERE c.cl_id = @cl_id;
    END
    ELSE IF @tr_id = 3
    BEGIN
        SELECT c.*, p.*, pg.*, fdl_id as frm_id, fdl_estado as frm_estado, fdl_fecha as frm_fecha,
     fdl_tipoLicencia as frm_tipoLicencia, fdl_numeroLicencia as frm_numeroLicencia, fdl_categoria as frm_categoria,
     fdl_vehiculoPesado as frm_vehiculoPesado, fdl_identificacion as frm_identificacion, fdl_numero as frm_numero,
     fdl_statusLegal as frm_statusLegal,  fdl_genero as frm_genero, fdl_donante as frm_donante, fdl_tipoSangre as frm_tipoSangre,
     fdl_talla as frm_talla, fdl_peso as frm_peso, fdl_tez as frm_tez, fdl_colorPelo as frm_colorPelo, fdl_colorOjo as frm_colorOjo, fdl_direccion as frm_direccion,
	 fdl_numeroDireccion as frm_numeroDireccion, fdl_pueblo as frm_pueblo, fdl_barrio as frm_barrio, fdl_apartado as frm_apartado,
	 fdl_pueblo2 as frm_pueblo2, fdl_licenciaSuspendida as frm_licenciaSuspendida,  fdl_motivoSuspension as frm_motivoSuspension, fdl_recluido as frm_recluido,
     fdl_convictoBebida as frm_convictoBebida,  fdl_fechaConvictoBebida as frm_fechaconvictoBebida, fdl_convictoNarcotico as frm_convictoNarcotico,
     fdl_fechaConvictoNarcotico as frm_fechaconvictoNarcotico, fdl_obligacionAlimentaria as frm_obligacionAlimentaria, fdl_deudaAcca as frm_deudaAcca,
     '' as frm_serv_nec, fdl_codigoPostal as frm_codigoPostal, fdl_codigoPostal2 as frm_codigoPostal2, '' as frm_tipoVehiculo, '' as frm_paisProcede,
	 '' as frm_estadoProcede, '' as frm_numeroLicencia2,'' as frm_fechaExpiracion,'' as frm_numeroIdentificacion,'' as frm_nombrePadre,'' as frm_nombreMadre,'' as frm_numeroLicenciaPR
      FROM Cliente c
        LEFT JOIN Pueblos p ON c.pl_id = p.pl_id
        LEFT JOIN Frm_DuplicadoLicencia fdl ON c.cl_id = fdl.cl_id AND @tr_id = 3
        LEFT JOIN Pagos pg ON c.cl_id = pg.cl_id AND pg.tr_id = @tr_id
        WHERE c.cl_id = @cl_id;
    END
    ELSE IF @tr_id = 4
    BEGIN
        SELECT c.*, p.*, pg.*, flr_id as frm_id, flr_estado as frm_estado, flr_fecha as frm_fecha,
     flr_tipoLicencia as frm_tipoLicencia, flr_numeroLicencia as frm_numeroLicencia, flr_categoria as frm_categoria,
     '' as frm_vehiculoPesado, flr_identificacion as frm_identificacion, '' as frm_numero,
     flr_statusLegal as frm_statusLegal,  flr_genero as frm_genero, flr_donante as frm_donante, flr_tipoSangre as frm_tipoSangre,
     flr_talla as frm_talla, flr_peso as frm_peso, flr_tez as frm_tez, flr_colorPelo as frm_colorPelo, flr_colorOjo as frm_colorOjo, flr_direccion as frm_direccion,
	 flr_numeroDireccion as frm_numeroDireccion, flr_pueblo as frm_pueblo, flr_barrio as frm_barrio, flr_apartado as frm_apartado,
	 flr_pueblo2 as frm_pueblo2, flr_licenciaSuspendida as frm_licenciaSuspendida, flr_motivoSuspencion as frm_motivoSuspension, flr_recluido as frm_recluido,
     flr_convictoBebida as frm_convictoBebida, flr_fechaConvictoBebida as frm_fechaconvictoBebida, flr_convictoNarcotico as frm_convictoNarcotico,
     flr_fechaConvictoNarcotico as frm_fechaconvictoNarcotico, flr_obligacionAlimentaria as frm_obligacionAlimentaria, flr_deudaAcca as frm_deudaAcca,
      '' as frm_serv_nec, flr_codigoPostal as frm_codigoPostal, flr_codigoPostal2 as frm_codigoPostal2,
     flr_tipoVehiculo as frm_tipoVehiculo, flr_paisProcede as frm_paisProcede, flr_estadoProcede as frm_estadoProcede,
     flr_numeroLicencia2 as frm_numeroLicencia2, flr_fechaExpiracion as frm_fechaExpiracion, flr_numeroIdentificacion as frm_numeroIdentificacion,
	 flr_nombrePadre as frm_nombrePadre, flr_nombreMadre as frm_nombreMadre, flr_numeroLicenciaPR as frm_numeroLicenciaPR	
      FROM Cliente c
        LEFT JOIN Pueblos p ON c.pl_id = p.pl_id
        LEFT JOIN Frm_LicenciaReciprocidad flr ON c.cl_id = flr.cl_id AND @tr_id = 4
        LEFT JOIN Pagos pg ON c.cl_id = pg.cl_id AND pg.tr_id = @tr_id
        WHERE c.cl_id = @cl_id;
    END
    ELSE
    BEGIN
        PRINT 'El valor de tr_id no es válido';
    END
END;

ALTER PROCEDURE sp_RegistrarAsignacion
    @asig_id INT, @frm_id INT,    @tr_id INT,    @adm_id1 INT,    @adm_id2 INT,
    @asig_fecha DATETIME,    @asig_Activo BIT,  @resultado INT OUTPUT
AS
BEGIN
	 SET NOCOUNT ON;
    INSERT INTO Asignacion (frm_id, tr_id, adm_id1, adm_id2, asig_fecha, asig_Activo)
    VALUES (@frm_id, @tr_id, @adm_id1, @adm_id2, @asig_fecha, @asig_Activo);
   SET @resultado = SCOPE_IDENTITY();
END;





CREATE PROCEDURE sp_RegistrarCliente
    @pl_id INT, @cl_nombre NVARCHAR(100), @cl_primerApellido NVARCHAR(100),   @cl_segundoApellido NVARCHAR(100),
    @cl_zip NVARCHAR(10), @cl_direccion NVARCHAR(255), @cl_numeroLicencia NVARCHAR(25), @cl_numeroSeguro NVARCHAR(25),
    @cl_fechaNacimiento DATETIME,  @cl_numeroTelefono NVARCHAR(15),  @cl_correo NVARCHAR(255), @cl_nombreUsuario NVARCHAR(50),
    @cl_contrasena NVARCHAR(50),  @cl_fechaRegistro DATETIME,  @cl_estado INT,  @resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;

   IF EXISTS (SELECT 1 FROM Cliente WHERE cl_correo = @cl_correo)
    BEGIN SET @resultado = -2; RETURN;
    END

    IF EXISTS (SELECT 1 FROM Cliente WHERE cl_nombreUsuario = @cl_nombreUsuario)
    BEGIN SET @resultado = -3; RETURN;
    END

    IF EXISTS (SELECT 1 FROM Cliente WHERE cl_numeroLicencia = @cl_numeroLicencia)
    BEGIN SET @resultado = -4; RETURN; 
    END
    INSERT INTO Cliente (pl_id, cl_nombre, cl_primerApellido, cl_segundoApellido, cl_zip, cl_direccion, cl_numeroLicencia, cl_numeroSeguro, cl_fechaNacimiento, cl_numeroTelefono, cl_correo, cl_nombreUsuario, cl_contrasena, cl_fechaRegistro, cl_estado)
    VALUES (@pl_id, @cl_nombre, @cl_primerApellido, @cl_segundoApellido, @cl_zip, @cl_direccion, @cl_numeroLicencia, @cl_numeroSeguro, @cl_fechaNacimiento, @cl_numeroTelefono, @cl_correo, @cl_nombreUsuario, @cl_contrasena, @cl_fechaRegistro, @cl_estado);

    SET @resultado = SCOPE_IDENTITY();
    RETURN;
END;

ALTER PROCEDURE spPanel_ActualizarFormularioCliente
    @frm_id int,
    @tr_id int,    
    @frm_estado int,
    @frm_tipoLicencia nvarchar(100),
    @frm_numeroLicencia NVARCHAR(25), @frm_categoria NVARCHAR(20), @frm_vehiculoPesado NVARCHAR(10),
	@frm_identificacion NVARCHAR(50), @frm_numero NVARCHAR(25), @frm_statusLegal NVARCHAR(255),
	@frm_genero NVARCHAR(10), @frm_donante NVARCHAR(2), @frm_tipoSangre NVARCHAR(20), @frm_talla NVARCHAR(20),
	@frm_peso NVARCHAR(20), @frm_tez NVARCHAR(20), @frm_colorPelo NVARCHAR(20), @frm_colorOjo NVARCHAR(20),
	@frm_direccion NVARCHAR(255), @frm_numeroDireccion NVARCHAR(10), @frm_pueblo NVARCHAR(100),
	@frm_codigoPostal INT, @frm_barrio NVARCHAR(255), @frm_apartado NVARCHAR(255), @frm_pueblo2 NVARCHAR(100),
	@frm_codigoPostal2 INT, @frm_licenciaSuspendida NVARCHAR(2), @frm_motivoSuspension NVARCHAR(50),
	@frm_recluido NVARCHAR(2), @frm_convictoBebida NVARCHAR(2), @frm_fechaConvictoBebida DATE, @frm_convictoNarcotico NVARCHAR(2),
	@frm_fechaConvictoNarcotico DATE, @frm_obligacionAlimentaria NVARCHAR(2), @frm_deudaAcca NVARCHAR(2),
	
	@frm_tipoVehiculo NVARCHAR(50), @frm_paisProcede NVARCHAR(50), @frm_estadoProcede NVARCHAR(50),
    @frm_numeroLicencia2 NVARCHAR(25), @frm_fechaExpiracion DATE, @frm_numeroIdentificacion NVARCHAR(50),
	@frm_nombrePadre NVARCHAR(255), @frm_nombreMadre NVARCHAR(255), @frm_numeroLicenciaPR NVARCHAR(25),
    @Resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    IF @tr_id = 1
    BEGIN        
        UPDATE Frm_RenovacionLicencia  
        SET
        frl_estado = @frm_estado,  frl_tipoLicencia = @frm_tipoLicencia,            
        frl_numeroLicencia = @frm_numeroLicencia,  frl_categoria = @frm_categoria, frl_identificacion = @frm_identificacion,  frl_numero = @frm_numero,
        frl_statusLegal = @frm_statusLegal, frl_genero = @frm_genero, frl_donante = @frm_donante, frl_tipoSangre = @frm_tipoSangre,
		frl_talla = @frm_talla,  frl_peso = @frm_peso, frl_tez = @frm_tez, frl_colorPelo = @frm_colorPelo, frl_colorOjo = @frm_colorOjo,
        frl_direccion = @frm_direccion, frl_numeroDireccion = @frm_numeroDireccion, frl_pueblo = @frm_pueblo, frl_codigoPostal = @frm_codigoPostal,
        frl_barrio = @frm_barrio, frl_apartado = @frm_apartado, frl_pueblo2 = @frm_pueblo2, frl_codigoPostal2 = @frm_codigoPostal2,
        frl_licenciaSuspendida = @frm_licenciaSuspendida, frl_motivoSuspension = @frm_motivoSuspension, frl_recluido = @frm_recluido,
        frl_convictoBebida = @frm_convictoBebida, frl_fechaConvictoBebida = @frm_fechaConvictoBebida, frl_convictoNarcotico = @frm_convictoNarcotico,
        frl_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, frl_obligacionAlimentaria = @frm_obligacionAlimentaria,
        frl_deudaAcca = @frm_deudaAcca, frl_vehiculoPesado = @frm_vehiculoPesado		
        WHERE
            frl_id = @frm_id;
    END
    ELSE IF @tr_id = 2
    BEGIN
        UPDATE Frm_LicenciaAprendizaje
        SET
            fla_estado = @frm_estado

        WHERE  fla_id = @frm_id;
    END
    ELSE IF @tr_id = 3
    BEGIN
        UPDATE Frm_DuplicadoLicencia
        SET
        fdl_estado = @frm_estado, fdl_tipoLicencia = @frm_tipoLicencia, fdl_numeroLicencia = @frm_numeroLicencia,  fdl_categoria = @frm_categoria,
		fdl_identificacion = @frm_identificacion,  fdl_numero = @frm_numero, fdl_statusLegal = @frm_statusLegal, fdl_genero = @frm_genero,
		fdl_donante = @frm_donante, fdl_tipoSangre = @frm_tipoSangre,
		fdl_talla = @frm_talla,  fdl_peso = @frm_peso, fdl_tez = @frm_tez, fdl_colorPelo = @frm_colorPelo, fdl_colorOjo = @frm_colorOjo,
        fdl_direccion = @frm_direccion, fdl_numeroDireccion = @frm_numeroDireccion, fdl_pueblo = @frm_pueblo, fdl_codigoPostal = @frm_codigoPostal,
        fdl_barrio = @frm_barrio, fdl_apartado = @frm_apartado, fdl_pueblo2 = @frm_pueblo2, fdl_codigoPostal2 = @frm_codigoPostal2,
        fdl_licenciaSuspendida = @frm_licenciaSuspendida, fdl_motivoSuspension = @frm_motivoSuspension, fdl_recluido = @frm_recluido,
        fdl_convictoBebida = @frm_convictoBebida, fdl_fechaConvictoBebida = @frm_fechaConvictoBebida, fdl_convictoNarcotico = @frm_convictoNarcotico,
        fdl_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, fdl_obligacionAlimentaria = @frm_obligacionAlimentaria,
        fdl_deudaAcca = @frm_deudaAcca, fdl_vehiculoPesado = @frm_vehiculoPesado
        WHERE
            fdl_id = @frm_id;
    END
    ELSE IF @tr_id = 4
    BEGIN
        UPDATE Frm_LicenciaReciprocidad
        SET
            flr_estado = @frm_estado,  flr_tipoLicencia = @frm_tipoLicencia,            
        flr_numeroLicencia = @frm_numeroLicencia,  flr_categoria = @frm_categoria, flr_identificacion = @frm_identificacion,
        flr_statusLegal = @frm_statusLegal, flr_genero = @frm_genero, flr_donante = @frm_donante, flr_tipoSangre = @frm_tipoSangre,
		flr_talla = @frm_talla,  flr_peso = @frm_peso, flr_tez = @frm_tez, flr_colorPelo = @frm_colorPelo, flr_colorOjo = @frm_colorOjo,
        flr_direccion = @frm_direccion, flr_numeroDireccion = @frm_numeroDireccion, flr_pueblo = @frm_pueblo, flr_codigoPostal = @frm_codigoPostal,
        flr_barrio = @frm_barrio, flr_apartado = @frm_apartado, flr_pueblo2 = @frm_pueblo2, flr_codigoPostal2 = @frm_codigoPostal2,
        flr_licenciaSuspendida = @frm_licenciaSuspendida, flr_motivoSuspencion = @frm_motivoSuspension, flr_recluido = @frm_recluido,
        flr_convictoBebida = @frm_convictoBebida, flr_fechaConvictoBebida = @frm_fechaConvictoBebida, flr_convictoNarcotico = @frm_convictoNarcotico,
        flr_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, flr_obligacionAlimentaria = @frm_obligacionAlimentaria,
        flr_deudaAcca = @frm_deudaAcca, 
                
        flr_tipoVehiculo = @frm_tipoVehiculo, flr_paisProcede = @frm_paisProcede, flr_estadoProcede = @frm_estadoProcede,
        flr_numeroLicencia2 = @frm_numeroLicencia2, flr_fechaExpiracion = @frm_fechaExpiracion, flr_numeroIdentificacion = @frm_numeroIdentificacion,
	    flr_nombrePadre = @frm_nombrePadre, flr_nombreMadre = @frm_nombreMadre, flr_numeroLicenciaPR = @frm_numeroLicenciaPR	       
        WHERE
            flr_id = @frm_id;
           
    END
    ELSE
    BEGIN
        PRINT 'El valor de tr_id no es válido';
    END
    SET @Resultado = @@ROWCOUNT;
END;



ALTER PROCEDURE spPanel_ActualizarFormularioCliente
    @frm_id int,  @tr_id int,   @frm_estado int, @frm_tipoLicencia nvarchar(100),
    @frm_numeroLicencia NVARCHAR(25), @frm_categoria NVARCHAR(20), @frm_vehiculoPesado NVARCHAR(10),
	@frm_identificacion NVARCHAR(50), @frm_numero NVARCHAR(25), @frm_statusLegal NVARCHAR(255),
	@frm_genero NVARCHAR(10), @frm_donante NVARCHAR(2), @frm_tipoSangre NVARCHAR(20), @frm_talla NVARCHAR(20),
	@frm_peso NVARCHAR(20), @frm_tez NVARCHAR(20), @frm_colorPelo NVARCHAR(20), @frm_colorOjo NVARCHAR(20),
	@frm_direccion NVARCHAR(255), @frm_numeroDireccion NVARCHAR(10), @frm_pueblo NVARCHAR(100),
	@frm_codigoPostal INT, @frm_barrio NVARCHAR(255), @frm_apartado NVARCHAR(255), @frm_pueblo2 NVARCHAR(100),
	@frm_codigoPostal2 INT, @frm_licenciaSuspendida NVARCHAR(2), @frm_motivoSuspension NVARCHAR(50),
	@frm_recluido NVARCHAR(2), @frm_convictoBebida NVARCHAR(2), @frm_fechaConvictoBebida DATE, @frm_convictoNarcotico NVARCHAR(2),
	@frm_fechaConvictoNarcotico DATE, @frm_obligacionAlimentaria NVARCHAR(2), @frm_deudaAcca NVARCHAR(2),
	
	@frm_tipoVehiculo NVARCHAR(50), @frm_paisProcede NVARCHAR(50), @frm_estadoProcede NVARCHAR(50),
    @frm_numeroLicencia2 NVARCHAR(25), @frm_fechaExpiracion DATE, @frm_numeroIdentificacion NVARCHAR(50),
	@frm_nombrePadre NVARCHAR(255), @frm_nombreMadre NVARCHAR(255), @frm_numeroLicenciaPR NVARCHAR(25),
	@frm_estadoRevision int, @frm_estadoProceso int, 
    @Resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    IF @tr_id = 1
    BEGIN        
        UPDATE Frm_RenovacionLicencia  
        SET
        frl_estado = @frm_estado,  frl_tipoLicencia = @frm_tipoLicencia,            
        frl_numeroLicencia = @frm_numeroLicencia,  frl_categoria = @frm_categoria, frl_identificacion = @frm_identificacion,  frl_numero = @frm_numero,
        frl_statusLegal = @frm_statusLegal, frl_genero = @frm_genero, frl_donante = @frm_donante, frl_tipoSangre = @frm_tipoSangre,
		frl_talla = @frm_talla,  frl_peso = @frm_peso, frl_tez = @frm_tez, frl_colorPelo = @frm_colorPelo, frl_colorOjo = @frm_colorOjo,
        frl_direccion = @frm_direccion, frl_numeroDireccion = @frm_numeroDireccion, frl_pueblo = @frm_pueblo, frl_codigoPostal = @frm_codigoPostal,
        frl_barrio = @frm_barrio, frl_apartado = @frm_apartado, frl_pueblo2 = @frm_pueblo2, frl_codigoPostal2 = @frm_codigoPostal2,
        frl_licenciaSuspendida = @frm_licenciaSuspendida, frl_motivoSuspension = @frm_motivoSuspension, frl_recluido = @frm_recluido,
        frl_convictoBebida = @frm_convictoBebida, frl_fechaConvictoBebida = @frm_fechaConvictoBebida, frl_convictoNarcotico = @frm_convictoNarcotico,
        frl_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, frl_obligacionAlimentaria = @frm_obligacionAlimentaria,
        frl_deudaAcca = @frm_deudaAcca, frl_vehiculoPesado = @frm_vehiculoPesado,  frl_estadoProceso = @frm_estadoProceso, frl_estadoRevision= @frm_estadoRevision		
        WHERE
            frl_id = @frm_id;
    END
    ELSE IF @tr_id = 2
    BEGIN
        UPDATE Frm_LicenciaAprendizaje
        SET
            fla_estado = @frm_estado

        WHERE  fla_id = @frm_id;
    END
    ELSE IF @tr_id = 3
    BEGIN
        UPDATE Frm_DuplicadoLicencia
        SET
        fdl_estado = @frm_estado, fdl_tipoLicencia = @frm_tipoLicencia, fdl_numeroLicencia = @frm_numeroLicencia,  fdl_categoria = @frm_categoria,
		fdl_identificacion = @frm_identificacion,  fdl_numero = @frm_numero, fdl_statusLegal = @frm_statusLegal, fdl_genero = @frm_genero,
		fdl_donante = @frm_donante, fdl_tipoSangre = @frm_tipoSangre,
		fdl_talla = @frm_talla,  fdl_peso = @frm_peso, fdl_tez = @frm_tez, fdl_colorPelo = @frm_colorPelo, fdl_colorOjo = @frm_colorOjo,
        fdl_direccion = @frm_direccion, fdl_numeroDireccion = @frm_numeroDireccion, fdl_pueblo = @frm_pueblo, fdl_codigoPostal = @frm_codigoPostal,
        fdl_barrio = @frm_barrio, fdl_apartado = @frm_apartado, fdl_pueblo2 = @frm_pueblo2, fdl_codigoPostal2 = @frm_codigoPostal2,
        fdl_licenciaSuspendida = @frm_licenciaSuspendida, fdl_motivoSuspension = @frm_motivoSuspension, fdl_recluido = @frm_recluido,
        fdl_convictoBebida = @frm_convictoBebida, fdl_fechaConvictoBebida = @frm_fechaConvictoBebida, fdl_convictoNarcotico = @frm_convictoNarcotico,
        fdl_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, fdl_obligacionAlimentaria = @frm_obligacionAlimentaria,
        fdl_deudaAcca = @frm_deudaAcca, fdl_vehiculoPesado = @frm_vehiculoPesado
        WHERE
            fdl_id = @frm_id;
    END
    ELSE IF @tr_id = 4
    BEGIN
        UPDATE Frm_LicenciaReciprocidad
        SET
            flr_estado = @frm_estado,  flr_tipoLicencia = @frm_tipoLicencia,            
        flr_numeroLicencia = @frm_numeroLicencia,  flr_categoria = @frm_categoria, flr_identificacion = @frm_identificacion,
        flr_statusLegal = @frm_statusLegal, flr_genero = @frm_genero, flr_donante = @frm_donante, flr_tipoSangre = @frm_tipoSangre,
		flr_talla = @frm_talla,  flr_peso = @frm_peso, flr_tez = @frm_tez, flr_colorPelo = @frm_colorPelo, flr_colorOjo = @frm_colorOjo,
        flr_direccion = @frm_direccion, flr_numeroDireccion = @frm_numeroDireccion, flr_pueblo = @frm_pueblo, flr_codigoPostal = @frm_codigoPostal,
        flr_barrio = @frm_barrio, flr_apartado = @frm_apartado, flr_pueblo2 = @frm_pueblo2, flr_codigoPostal2 = @frm_codigoPostal2,
        flr_licenciaSuspendida = @frm_licenciaSuspendida, flr_motivoSuspencion = @frm_motivoSuspension, flr_recluido = @frm_recluido,
        flr_convictoBebida = @frm_convictoBebida, flr_fechaConvictoBebida = @frm_fechaConvictoBebida, flr_convictoNarcotico = @frm_convictoNarcotico,
        flr_fechaConvictoNarcotico = @frm_fechaConvictoNarcotico, flr_obligacionAlimentaria = @frm_obligacionAlimentaria,
        flr_deudaAcca = @frm_deudaAcca, 
                
        flr_tipoVehiculo = @frm_tipoVehiculo, flr_paisProcede = @frm_paisProcede, flr_estadoProcede = @frm_estadoProcede,
        flr_numeroLicencia2 = @frm_numeroLicencia2, flr_fechaExpiracion = @frm_fechaExpiracion, flr_numeroIdentificacion = @frm_numeroIdentificacion,
	    flr_nombrePadre = @frm_nombrePadre, flr_nombreMadre = @frm_nombreMadre, flr_numeroLicenciaPR = @frm_numeroLicenciaPR	       
        WHERE
            flr_id = @frm_id;
           
    END
    ELSE
    BEGIN
        PRINT 'El valor de tr_id no es válido';
    END
    SET @Resultado = @@ROWCOUNT;
END;







































ALTER PROCEDURE sp_RegistrarCertifMed
   ( @cl_id int, @pg_id int, @tr_id int, @frm_id int,  @fcm_numeroSeguro nvarchar(15), @fcm_numeroLicencia nvarchar(15), @fcm_ojoDerechoSinLentes nvarchar(10),
    @fcm_ojoDerechoConLentes nvarchar(10), @fcm_ojoIzquierdoSinLentes nvarchar(10), @fcm_ojoIzquierdoConLentes nvarchar(10), @fcm_ambosOjos nvarchar(10),
    @fcm_condicion nvarchar(20), @fcm_espejuelos nvarchar(2), @fcm_usaLentes nvarchar(2), @fcm_condicionOido nvarchar(255), @fcm_condicionBrazo nvarchar(255),
    @fcm_condicionPierna nvarchar(255), @fcm_condicionFisica nvarchar(255), @fcm_observacion nvarchar(255), @fcm_estadoInconciencia nvarchar(2),
    @fcm_padeceCorazon nvarchar(2), @fcm_marcapaso nvarchar(2), @fcm_protesis nvarchar(2), @fcm_estaturaPies nvarchar(20),
    @fcm_estaturaPulgada nvarchar(20), @fcm_peso nvarchar(20), @fcm_colorPelo nvarchar(20), @fcm_colorOjo nvarchar(20), 
     @fcm_nombreMedico nvarchar(255), @fcm_licenciaMedico nvarchar(20), @fcm_fecha datetime, @fcm_estado int, @Resultado INT OUTPUT )
AS
BEGIN   SET NOCOUNT ON;
    INSERT INTO Frm_CertificadoMedico (
        cl_id, pg_id, tr_id, frm_id, fcm_numeroSeguro, fcm_numeroLicencia, fcm_ojoDerechoSinLentes, fcm_ojoDerechoConLentes, fcm_ojoIzquierdoSinLentes,
        fcm_ojoIzquierdoConLentes, fcm_ambosOjos, fcm_condicion, fcm_espejuelos, fcm_usaLentes, fcm_condicionOido, fcm_condicionBrazo, fcm_condicionPierna,
        fcm_condicionFisica, fcm_observacion, fcm_estadoInconciencia, fcm_padeceCorazon, fcm_marcapaso, fcm_protesis, fcm_estaturaPies, fcm_estaturaPulgada,
        fcm_peso, fcm_colorPelo, fcm_colorOjo, fcm_nombreMedico, fcm_licenciaMedico,  fcm_fecha, fcm_estado) 
        VALUES (  @cl_id,        @pg_id,        @tr_id,         @frm_id,        @fcm_numeroSeguro,  @fcm_numeroLicencia,        @fcm_ojoDerechoSinLentes,
		@fcm_ojoDerechoConLentes,  @fcm_ojoIzquierdoSinLentes, @fcm_ojoIzquierdoConLentes, @fcm_ambosOjos, @fcm_condicion,  @fcm_espejuelos,
        @fcm_usaLentes, @fcm_condicionOido, @fcm_condicionBrazo, @fcm_condicionPierna, @fcm_condicionFisica, @fcm_observacion,
        @fcm_estadoInconciencia, @fcm_padeceCorazon, @fcm_marcapaso, @fcm_protesis, @fcm_estaturaPies, @fcm_estaturaPulgada, @fcm_peso,
        @fcm_colorPelo,  @fcm_colorOjo,  @fcm_nombreMedico, @fcm_licenciaMedico,  @fcm_fecha, @fcm_estado );
	SET @Resultado = SCOPE_IDENTITY();
END;




ALTER PROCEDURE sp_ActualizarCertifMed
   ( @fcm_id INT, @cl_id int, @pg_id int, @tr_id int, @frm_id int,  @fcm_numeroSeguro nvarchar(15), @fcm_numeroLicencia nvarchar(15), @fcm_ojoDerechoSinLentes nvarchar(10),
    @fcm_ojoDerechoConLentes nvarchar(10), @fcm_ojoIzquierdoSinLentes nvarchar(10), @fcm_ojoIzquierdoConLentes nvarchar(10), @fcm_ambosOjos nvarchar(10),
    @fcm_condicion nvarchar(20), @fcm_espejuelos nvarchar(2), @fcm_usaLentes nvarchar(2), @fcm_condicionOido nvarchar(255), @fcm_condicionBrazo nvarchar(255),
    @fcm_condicionPierna nvarchar(255), @fcm_condicionFisica nvarchar(255), @fcm_observacion nvarchar(255), @fcm_estadoInconciencia nvarchar(2),
    @fcm_padeceCorazon nvarchar(2), @fcm_marcapaso nvarchar(2), @fcm_protesis nvarchar(2), @fcm_estaturaPies nvarchar(20),
    @fcm_estaturaPulgada nvarchar(20), @fcm_peso nvarchar(20), @fcm_colorPelo nvarchar(20), @fcm_colorOjo nvarchar(20), 
    @fcm_nombreMedico nvarchar(255), @fcm_licenciaMedico nvarchar(20), @fcm_fecha datetime, @fcm_estado int, @Resultado INT OUTPUT )
AS
BEGIN
	SET NOCOUNT ON;
    UPDATE Frm_CertificadoMedico SET  cl_id = @cl_id, pg_id = @pg_id, tr_id = @tr_id, frm_id = @frm_id, fcm_numeroSeguro = @fcm_numeroSeguro,
		fcm_numeroLicencia = @fcm_numeroLicencia, fcm_ojoDerechoSinLentes = @fcm_ojoDerechoSinLentes, fcm_ojoDerechoConLentes = @fcm_ojoDerechoConLentes,
        fcm_ojoIzquierdoSinLentes = @fcm_ojoIzquierdoSinLentes, fcm_ojoIzquierdoConLentes = @fcm_ojoIzquierdoConLentes,
        fcm_ambosOjos = @fcm_ambosOjos, fcm_condicion = @fcm_condicion, fcm_espejuelos = @fcm_espejuelos, fcm_usaLentes = @fcm_usaLentes,
        fcm_condicionOido = @fcm_condicionOido, fcm_condicionBrazo = @fcm_condicionBrazo, fcm_condicionPierna = @fcm_condicionPierna,
        fcm_condicionFisica = @fcm_condicionFisica, fcm_observacion = @fcm_observacion, fcm_estadoInconciencia = @fcm_estadoInconciencia,
        fcm_padeceCorazon = @fcm_padeceCorazon, fcm_marcapaso = @fcm_marcapaso, fcm_protesis = @fcm_protesis,
        fcm_estaturaPies = @fcm_estaturaPies, fcm_estaturaPulgada = @fcm_estaturaPulgada, fcm_peso = @fcm_peso, fcm_colorPelo = @fcm_colorPelo,
        fcm_colorOjo = @fcm_colorOjo, fcm_nombreMedico = @fcm_nombreMedico, fcm_licenciaMedico = @fcm_licenciaMedico, fcm_fecha = @fcm_fecha, fcm_estado = @fcm_estado
    WHERE fcm_id = @fcm_id; 
   SET @Resultado = @@ROWCOUNT;
END;



DECLARE @tr_id INT = 4; -- Valor de ejemplo para tr_id
DECLARE @pg_id INT = 51; -- Valor de ejemplo para pg_id
DECLARE @cl_id INT = 60; -- Valor de ejemplo para cl_id
DECLARE @Resultado INT;

EXEC sp_formDataExists @tr_id, @pg_id, @cl_id;, @Resultado OUTPUT;

PRINT 'El resultado de la prueba es: ' + CAST(@Resultado AS VARCHAR);









ALTER TABLE Frm_LicenciaReciprocidad
CHANGE flr_licenciaProcede flr_paisProcede VARCHAR(50);
EXEC sp_rename 'Frm_LicenciaReciprocidad.flr_licenciaProcede', 'flr_paisProcede', 'COLUMN';

CREATE PROCEDURE sp_ObtenerEstados
As BEgin SELECT * FROM Estados;
END;



CREATE TABLE DB_163557_tulicencia.dbo.Frm_CertificadoMedico (
    fcm_id int IDENTITY(1,1) NOT NULL,
    cl_id int NULL,
    pg_id int NULL,
    tr_id int NULL,
    frm_id int NULL,
    fcm_numeroSeguro nvarchar(15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_numeroLicencia nvarchar(15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,    
    fcm_ojoDerechoSinLentes nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_ojoDerechoConLentes nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_ojoIzquierdoSinLentes nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_ojoIzquierdoConLentes nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_ambosOjos nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_condicion nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_espejuelos nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_usaLentes nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_condicionOido nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_condicionBrazo nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_condicionPierna nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_condicionFisica nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_observacion nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,    
    fcm_estadoInconciencia nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_padeceCorazon nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_marcapaso nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_protesis nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_estaturaPies nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_estaturaPulgada nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_peso nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_colorPelo nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_colorOjo nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_examinado nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_capacitado nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_nombreMedico nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_licenciaMedico nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    fcm_fecha datetime NULL,
    fcm_estado int NULL,
    CONSTRAINT pk_Frm_CertificadoMedico PRIMARY KEY (fcm_id),
    FOREIGN KEY (cl_id) REFERENCES DB_163557_tulicencia.dbo.Cliente(cl_id),
    FOREIGN KEY (pg_id) REFERENCES DB_163557_tulicencia.dbo.Pagos(pg_id),
    FOREIGN KEY (tr_id) REFERENCES DB_163557_tulicencia.dbo.Tramites(tr_id),
    FOREIGN KEY (frm_id) REFERENCES DB_163557_tulicencia.dbo.Frm_RenovacionLicencia(frl_id)
);




ALTER TABLE DB_163557_tulicencia.dbo.Frm_CertificadoMedico
DROP CONSTRAINT FK__Frm_Certi__frm_i__14E61A24;




ALTER PROCEDURE sp_RegistrarLicenciaRec
(   @cl_id INT, @pg_id INT, @tr_id INT, @flr_fecha DATETIME, @flr_estado INT, @flr_tipoLicencia NVARCHAR(50), @flr_numeroLicencia NVARCHAR(25),
    @flr_categoria NVARCHAR(20),  @flr_tipoVehiculo NVARCHAR(50), @flr_paisProcede NVARCHAR(50), @flr_estadoProcede NVARCHAR(50), @flr_numeroLicencia2 NVARCHAR(25),
    @flr_fechaExpiracion DATE, @flr_identificacion NVARCHAR(50), @flr_numeroIdentificacion NVARCHAR(50), @flr_statusLegal NVARCHAR(255),
    @flr_genero NVARCHAR(20),  @flr_donante NVARCHAR(2),  @flr_tipoSangre NVARCHAR(20), @flr_talla NVARCHAR(20), @flr_peso NVARCHAR(20),
    @flr_tez NVARCHAR(20),  @flr_colorPelo NVARCHAR(20), @flr_colorOjo NVARCHAR(20), @flr_nombrePadre NVARCHAR(255), @flr_nombreMadre NVARCHAR(255),
    @flr_direccion NVARCHAR(255), @flr_numeroDireccion NVARCHAR(20), @flr_pueblo NVARCHAR(100), @flr_codigoPostal NVARCHAR(5), @flr_barrio NVARCHAR(255),
    @flr_apartado NVARCHAR(255), @flr_pueblo2 NVARCHAR(100), @flr_codigoPostal2 NVARCHAR(5), @flr_licenciaSuspendida NVARCHAR(2),
    @flr_motivoSuspencion NVARCHAR(20), @flr_numeroLicenciaPR NVARCHAR(25), @flr_recluido NVARCHAR(2), @flr_convictoBebida NVARCHAR(2), @flr_fechaConvictoBebida DATE,
	@flr_convictoNarcotico NVARCHAR(2), @flr_fechaConvictoNarcotico DATE,  @flr_obligacionAlimentaria NVARCHAR(2),  @flr_deudaAcca NVARCHAR(2),
    @Resultado INT OUTPUT )
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO Frm_LicenciaReciprocidad 
    (   cl_id, pg_id, tr_id, flr_fecha, flr_estado, flr_tipoLicencia, flr_numeroLicencia, flr_categoria, flr_tipoVehiculo,
        flr_paisProcede, flr_estadoProcede, flr_numeroLicencia2, flr_fechaExpiracion, flr_identificacion, flr_numeroIdentificacion,
        flr_statusLegal, flr_genero, flr_donante, flr_tipoSangre, flr_talla, flr_peso, flr_tez, flr_colorPelo, flr_colorOjo,
        flr_nombrePadre, flr_nombreMadre, flr_direccion, flr_numeroDireccion, flr_pueblo, flr_codigoPostal, flr_barrio,
        flr_apartado, flr_pueblo2, flr_codigoPostal2, flr_licenciaSuspendida, flr_motivoSuspencion, flr_numeroLicenciaPR,
        flr_recluido, flr_convictoBebida, flr_fechaConvictoBebida, flr_convictoNarcotico, flr_fechaConvictoNarcotico,
        flr_obligacionAlimentaria, flr_deudaAcca )
    VALUES 
    (   @cl_id, @pg_id, @tr_id, @flr_fecha, @flr_estado, @flr_tipoLicencia, @flr_numeroLicencia, @flr_categoria,
        @flr_tipoVehiculo, @flr_paisProcede, @flr_estadoProcede, @flr_numeroLicencia2, @flr_fechaExpiracion, @flr_identificacion,
        @flr_numeroIdentificacion, @flr_statusLegal, @flr_genero, @flr_donante, @flr_tipoSangre, @flr_talla, @flr_peso,
        @flr_tez, @flr_colorPelo, @flr_colorOjo, @flr_nombrePadre, @flr_nombreMadre, @flr_direccion, @flr_numeroDireccion,
        @flr_pueblo, @flr_codigoPostal, @flr_barrio, @flr_apartado, @flr_pueblo2, @flr_codigoPostal2,
        @flr_licenciaSuspendida, @flr_motivoSuspencion, @flr_numeroLicenciaPR, @flr_recluido, @flr_convictoBebida,
        @flr_fechaConvictoBebida, @flr_convictoNarcotico, @flr_fechaConvictoNarcotico, @flr_obligacionAlimentaria,
        @flr_deudaAcca );
    SET @Resultado = SCOPE_IDENTITY();
END;






ALTER PROCEDURE sp_ActualizarLicenciaRec
    @flr_id INT,
    @cl_id INT,
    @pg_id INT,
    @tr_id INT,
    @flr_fecha DATETIME,
    @flr_estado INT,
    @flr_tipoLicencia NVARCHAR(50),
    @flr_numeroLicencia NVARCHAR(25),
    @flr_categoria NVARCHAR(20),
    @flr_tipoVehiculo NVARCHAR(50),
    @flr_paisProcede NVARCHAR(50), @flr_estadoProcede NVARCHAR(50),
    @flr_numeroLicencia2 NVARCHAR(25),
    @flr_fechaExpiracion DATE,
    @flr_identificacion NVARCHAR(50),
    @flr_numeroIdentificacion NVARCHAR(50),
    @flr_statusLegal NVARCHAR(255),
    @flr_genero NVARCHAR(20),
    @flr_donante NVARCHAR(2),
    @flr_tipoSangre NVARCHAR(20),
    @flr_talla NVARCHAR(20),
    @flr_peso NVARCHAR(20),
    @flr_tez NVARCHAR(20),
    @flr_colorPelo NVARCHAR(20),
    @flr_colorOjo NVARCHAR(20),
    @flr_nombrePadre NVARCHAR(255),
    @flr_nombreMadre NVARCHAR(255),
    @flr_direccion NVARCHAR(255),
    @flr_numeroDireccion NVARCHAR(20),
    @flr_pueblo NVARCHAR(100),
    @flr_codigoPostal NVARCHAR(5),
    @flr_barrio NVARCHAR(255),
    @flr_apartado NVARCHAR(255),
    @flr_pueblo2 NVARCHAR(100),
    @flr_codigoPostal2 NVARCHAR(5),
    @flr_licenciaSuspendida NVARCHAR(2),
    @flr_motivoSuspencion NVARCHAR(20),
    @flr_numeroLicenciaPR NVARCHAR(25),
    @flr_recluido NVARCHAR(2),
    @flr_convictoBebida NVARCHAR(2),
    @flr_fechaConvictoBebida DATE,
    @flr_convictoNarcotico NVARCHAR(2),
    @flr_fechaConvictoNarcotico DATE,
    @flr_obligacionAlimentaria NVARCHAR(2),
    @flr_deudaAcca NVARCHAR(2),
    @Resultado INT OUTPUT
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE Frm_LicenciaReciprocidad
    SET 
        cl_id = @cl_id,
        pg_id = @pg_id,
        tr_id = @tr_id,
        flr_fecha = @flr_fecha,
        flr_estado = @flr_estado,
        flr_tipoLicencia = @flr_tipoLicencia,
        flr_numeroLicencia = @flr_numeroLicencia,
        flr_categoria = @flr_categoria,
        flr_tipoVehiculo = @flr_tipoVehiculo,
        flr_paisProcede = @flr_paisProcede, flr_estadoProcede = @flr_estadoProcede,
        flr_numeroLicencia2 = @flr_numeroLicencia2,
        flr_fechaExpiracion = @flr_fechaExpiracion,
        flr_identificacion = @flr_identificacion,
        flr_numeroIdentificacion = @flr_numeroIdentificacion,
        flr_statusLegal = @flr_statusLegal,
        flr_genero = @flr_genero,
        flr_donante = @flr_donante,
        flr_tipoSangre = @flr_tipoSangre,
        flr_talla = @flr_talla,
        flr_peso = @flr_peso,
        flr_tez = @flr_tez,
        flr_colorPelo = @flr_colorPelo,
        flr_colorOjo = @flr_colorOjo,
        flr_nombrePadre = @flr_nombrePadre,
        flr_nombreMadre = @flr_nombreMadre,
        flr_direccion = @flr_direccion,
        flr_numeroDireccion = @flr_numeroDireccion,
        flr_pueblo = @flr_pueblo,
        flr_codigoPostal = @flr_codigoPostal,
        flr_barrio = @flr_barrio,
        flr_apartado = @flr_apartado,
        flr_pueblo2 = @flr_pueblo2,
        flr_codigoPostal2 = @flr_codigoPostal2,
        flr_licenciaSuspendida = @flr_licenciaSuspendida,
        flr_motivoSuspencion = @flr_motivoSuspencion,
        flr_numeroLicenciaPR = @flr_numeroLicenciaPR,
        flr_recluido = @flr_recluido,
        flr_convictoBebida = @flr_convictoBebida,
        flr_fechaConvictoBebida = @flr_fechaConvictoBebida,
        flr_convictoNarcotico = @flr_convictoNarcotico,
        flr_fechaConvictoNarcotico = @flr_fechaConvictoNarcotico,
        flr_obligacionAlimentaria = @flr_obligacionAlimentaria,
        flr_deudaAcca = @flr_deudaAcca
    WHERE flr_id = @flr_id;
    
    SET @Resultado = @@ROWCOUNT;
END;


ALTER TABLE Cliente
ADD cl_genero VARCHAR(10),
    cl_talla VARCHAR(20),
   cl_peso VARCHAR(20),
  cl_tez VARCHAR(20),
 cl_colorPelo VARCHAR(20),
    cl_colorOjo VARCHAR(20),
   cl_codigoPostal VARCHAR(4),
  cl_pueblo VARCHAR(100);



DELETE FROM Cliente WHERE cl_id = 10;
DELETE FROM Pagos WHERE pg_id = 36;
DELETE FROM Frm_RenovacionLicencia WHERE frl_id = 40;
DELETE FROM Estados






CREATE TABLE DB_163557_tulicencia.dbo.Frm_RenovacionLicencia (
	frl_id int IDENTITY(1,1) NOT NULL,
	cl_id int NULL,
	pg_id int NULL,
	tr_id int NULL,
	frl_fecha datetime NULL,
	frl_estado int NULL,
	frl_tipoLicencia nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_numeroLicencia nvarchar(25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_categoria nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_vehiculoPesado nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_identificacion nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_numero nvarchar(25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_statusLegal nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_genero nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_donante nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_tipoSangre nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_talla nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_peso nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_tez nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_colorPelo nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_colorOjo nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_direccion nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_numeroDireccion nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_pueblo nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_codigoPostal nvarchar(5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_barrio nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_apartado nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_pueblo2 nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_codigoPostal2 nvarchar(5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_licenciaSuspendida nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_motivoSuspension nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_recluido nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_convictoBebida nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_fechaConvictoBebida date NULL,
	frl_convictoNarcotico nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_fechaConvictoNarcotico date NULL,
	frl_obligacionAlimentaria nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_deudaAcca nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	frl_serv_nec nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT pk_Frm_RenovacionLicencia PRIMARY KEY (frl_id),
	CONSTRAINT FK__Frm_Renov__cl_id__4CA06362 FOREIGN KEY (cl_id) REFERENCES DB_163557_tulicencia.dbo.Cliente(cl_id),
	CONSTRAINT FK__Frm_Renov__pg_id__4D94879B FOREIGN KEY (pg_id) REFERENCES DB_163557_tulicencia.dbo.Pagos(pg_id),
	CONSTRAINT FK__Frm_Renov__tr_id__4E88ABD4 FOREIGN KEY (tr_id) REFERENCES DB_163557_tulicencia.dbo.Tramites(tr_id)
);




INSERT INTO Administrador
( adm_user, adm_clv, adm_nombres, adm_est, adm_nivel, adm_fech_reg, adm_email)
VALUES('admin', N'2024', N'Andres Lopez', 1, 1, '2024-03-13', N'andreslg20@gmail.com');




















ALTER TABLE Frm_RenovacionLicencia ADD CONSTRAINT pk_Frm_RenovacionLicencia PRIMARY KEY (frl_id);
ALTER TABLE Frm_LicenciaReciprocidad ADD CONSTRAINT pk_Frm_LicenciaReciprocidad PRIMARY KEY (flr_id);
ALTER TABLE Frm_CertificadoMedico ADD CONSTRAINT pk_Frm_CertificadoMedico PRIMARY KEY (fcm_id);
ALTER TABLE Frm_LicenciaAprendizaje ADD CONSTRAINT pk_Frm_LicenciaAprendizaje PRIMARY KEY (fla_id);
ALTER TABLE Frm_DuplicadoLicencia ADD CONSTRAINT pk_Frm_DuplicadoLicencia PRIMARY KEY (fdl_id);


EXEC sp_ValidarArchivos 1,60,17

DECLARE @tr_id INT = 1;
DECLARE @cl_id INT = 64;
DECLARE @frm_id INT = 45;
EXEC sp_ValidarArchivos @tr_id, @cl_id, @frm_id;





CREATE PROCEDURE sp_ValidarArchivos
    @tr_id INT, @cl_id INT, @frm_id INT
AS BEGIN
    DECLARE @tr_nombreTabla NVARCHAR(255);
    DECLARE @frm_table_id NVARCHAR(50);
    DECLARE @conteoArchivos INT;
    SELECT @tr_nombreTabla = tr_nombreTabla FROM Tramites WHERE tr_id = @tr_id;
	SELECT @frm_table_id = COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE 
	OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
    AND TABLE_NAME = @tr_nombreTabla;
    IF @tr_nombreTabla IS NOT NULL
    BEGIN
        DECLARE @sql NVARCHAR(MAX);
        SET @sql = N'SELECT @frm_id_out = COALESCE(' + @frm_table_id + ', 0) FROM ' + QUOTENAME(@tr_nombreTabla) + ' WHERE tr_id = @tr_id AND cl_id = @cl_id';
        DECLARE @frm_id_from_table INT;
        EXEC sp_executesql @sql, N'@frm_id_out INT OUTPUT, @tr_id INT, @cl_id INT', @frm_id_from_table OUTPUT, @tr_id, @cl_id;
        IF @frm_id_from_table = @frm_id
        BEGIN
            SELECT @conteoArchivos = COUNT(*) FROM Archivos WHERE frm_id = @frm_id AND cl_id = @cl_id AND tr_id = @tr_id;
            SELECT 
                CASE 
                    WHEN @conteoArchivos > 0 THEN 'Hay archivos' ELSE 'No hay archivos' 
                END AS EstadoArchivos,
                @conteoArchivos AS ConteoArchivos;
        END
        ELSE
        BEGIN
            SELECT 'No hay registros' AS EstadoArchivos, 0 AS ConteoArchivos;
        END
    END
    ELSE
    BEGIN
        SELECT 'Error: Tabla no encontrada' AS EstadoArchivos, 0 AS ConteoArchivos;
    END
END






DECLARE @codigoPago NVARCHAR(20) = '163483TQKWHK';
EXEC ObtenerPagoPorCodigoPago @codigoPago;

CREATE PROCEDURE sp_ActualizarClientePassword
    @cl_id INT, @cl_contrasena NVARCHAR(255), @Resultado INT output
AS
BEGIN
    UPDATE Cliente  SET cl_contrasena = @cl_contrasena  WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END;


ALTER PROCEDURE sp_ActualizarKeyTemporal
    @cl_id INT, @cl_keyTemporal NVARCHAR(15),   @Resultado INT output
AS BEGIN
    UPDATE Cliente  SET cl_keyTemporal = @cl_keyTemporal  WHERE cl_id = @cl_id; SET @Resultado = @@ROWCOUNT;
END;

CREATE PROCEDURE sp_ActualizarClienteKeyTemporal
    @cl_id INT,  @cl_keyTemporal NVARCHAR(15),
    @Resultado INT output
AS
BEGIN
    UPDATE Cliente SET cl_keyTemporal = @cl_keyTemporal
    WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END;







CREATE PROCEDURE sp_ActualizarClienteCambioPass
    @cl_id INT,  @cl_contrasena NVARCHAR(255), @Resultado INT output
AS BEGIN
    UPDATE Cliente SET cl_contrasena = @cl_contrasena  WHERE cl_id = @cl_id;
   SET @Resultado = @@ROWCOUNT;
END;


ALTER PROCEDURE sp_ActualizarCliente
    @cl_id INT, @pl_id INT,    @cl_nombre NVARCHAR(255),    @cl_primerApellido NVARCHAR(255),    @cl_segundoApellido NVARCHAR(255), @cl_zip NVARCHAR(5),
	@cl_direccion NVARCHAR(255),     @cl_numeroLicencia NVARCHAR(15),    @cl_numeroSeguro NVARCHAR(11),    @cl_fechaNacimiento DATE,@cl_numeroTelefono NVARCHAR(255), 
	@cl_correo NVARCHAR(255),    @cl_nombreUsuario NVARCHAR(255),    @cl_contrasena NVARCHAR(255), @cl_estado INT, @cl_keyTemporal NVARCHAR(15),@Resultado INT OUTPUT
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Cliente
    SET 
        pl_id = @pl_id,  cl_nombre = @cl_nombre, cl_primerApellido = @cl_primerApellido, cl_segundoApellido = @cl_segundoApellido,  cl_zip = @cl_zip,
        cl_direccion = @cl_direccion, cl_numeroLicencia = @cl_numeroLicencia, cl_numeroSeguro = @cl_numeroSeguro, cl_fechaNacimiento = @cl_fechaNacimiento,
        cl_numeroTelefono = @cl_numeroTelefono,  cl_correo = @cl_correo, cl_nombreUsuario = @cl_nombreUsuario, cl_contrasena = @cl_contrasena, cl_estado = @cl_estado,
        cl_keyTemporal = @cl_keyTemporal
    WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END;









ALTER PROCEDURE sp_ValidarArchivos
    @tr_id INT, @cl_id INT, @frm_id INT
AS BEGIN
    DECLARE @tr_nombreTabla NVARCHAR(255);
    DECLARE @frm_table_id NVARCHAR(50);
    DECLARE @conteoArchivos INT;
    SELECT @tr_nombreTabla = tr_nombreTabla FROM Tramites WHERE tr_id = @tr_id;
	SELECT @frm_table_id = COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE 
	OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
    AND TABLE_NAME = @tr_nombreTabla;
    IF @tr_nombreTabla IS NOT NULL
    BEGIN
        DECLARE @sql NVARCHAR(MAX);
        SET @sql = N'SELECT @frm_id_out = COALESCE(' + @frm_table_id + ', 0) FROM ' + QUOTENAME(@tr_nombreTabla) + ' WHERE tr_id = @tr_id AND cl_id = @cl_id';
        DECLARE @frm_id_from_table INT;
        EXEC sp_executesql @sql, N'@frm_id_out INT OUTPUT, @tr_id INT, @cl_id INT', @frm_id_from_table OUTPUT, @tr_id, @cl_id;
        IF @frm_id_from_table = @frm_id
        BEGIN
            SELECT @conteoArchivos = COUNT(*) FROM Archivos WHERE frm_id = @frm_id AND cl_id = @cl_id AND tr_id = @tr_id;
            SELECT 
                CASE 
                    WHEN @conteoArchivos > 0 THEN 'Hay archivos' ELSE 'No hay archivos' 
                END AS EstadoArchivos,
                @conteoArchivos AS ConteoArchivos;
        END
        ELSE
        BEGIN
            SELECT 'No hay registros' AS EstadoArchivos, 0 AS ConteoArchivos;
        END
    END
    ELSE
    BEGIN
        SELECT 'Error: Tabla no encontrada' AS EstadoArchivos, 0 AS ConteoArchivos;
    END
END











EXEC sp_ObtenerClientePorCorreo @cl_correo = 'angel@gmail.com';

EXEC sp_ActualizarClientePassword @cl_id = 1, @cl_contrasena = 'NuevaContr';


EXEC  sp_VerificarArchivoExistente  1,60

CREATE PROCEDURE sp_VerificarArchivoExistente
    @tr_id INT, @cl_id INT, @frm_id INT
AS
BEGIN
    DECLARE @tr_nombretabla NVARCHAR(255);
    DECLARE @frm_id INT;
    DECLARE @existeArchivo BIT = 0;

    SELECT @tr_nombretabla = tr_nombretabla FROM Tramites WHERE tr_id = @tr_id;
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = N'SELECT @frm_id_out = COALESCE(frl_id, flr_id) FROM ' + QUOTENAME(@tr_nombretabla) + ' WHERE pg_id = @pg_id AND cl_id = @cl_id AND tr_id = @tr_id';

    EXEC sp_executesql @sql, N'@frm_id_out INT OUTPUT, @pg_id INT, @cl_id INT, @tr_id INT', @frm_id OUTPUT, @cl_id, @tr_id;

    IF @frm_id IS NOT NULL
    BEGIN
        SET @existeArchivo = CASE WHEN EXISTS (SELECT 1 FROM Archivos WHERE tr_id = @tr_id AND cl_id = @cl_id AND frm_id = @frm_id) THEN 1 ELSE 0 END;
    END
    SELECT @existeArchivo AS ExisteArchivo;
END


debo enviar frm_id; tr_id, cl_id

SELECT tr_nombreTabla  from Tramites WHERE tr_id = 1;  -- Esto es igual a Frm_RenovacionLicencia o Frm_LicenciaReciprocidad
SELECT frl_id  FROM Frm_RenovacionLicencia 			-- esto es igual a 17
SELECT flr_id  FROM Frm_LicenciaReciprocidad    ---
 -- LUEGO DEBE SER UNA COMPARACION  segun tabla
  frl_id = frm_id   ó  flr_id= frm_id
  
 --- SI SON DIFERENTES 
  NO HAY REGISTROS Y TERMINA
  si son iguales entonces buscar en la tabla Archivos
select * from Archivos  WHERE frm_id =17 AND cl_id and tr_id 
-- si hay archivos necesito 2 respuestas 
1. si hay registros
2. conteo de los archivos 
 
  

ALTER PROCEDURE sp_RegistrarPago
@pg_fecha DATETIME,
@pg_codigo NVARCHAR(255),
@pg_estado NVARCHAR(255),
@cl_id INT,
@tr_id INT,
@pg_nota NVARCHAR(255),
@pg_status NVARCHAR(255),
@pg_txid NVARCHAR(25),
@pg_metodo INT,
@Resultado INT output
AS BEGIN
SET NOCOUNT ON;
INSERT INTO Pagos (cl_id, tr_id, pg_fecha, pg_codigo, pg_estado, pg_nota, pg_status, pg_txid, pg_metodo)
VALUES (@cl_id, @tr_id, @pg_fecha, @pg_codigo, @pg_estado, @pg_nota, @pg_status, @pg_txid, @pg_metodo);
SET @Resultado = SCOPE_IDENTITY();
END 



DECLARE @tr_id INT = 1;
DECLARE @pg_id INT = 14;
DECLARE @cl_id INT = 60;
DECLARE @Resultado INT;
EXEC sp_formDataExists @tr_id, @pg_id, @cl_id, 



CREATE PROCEDURE ObtenerDetalleTramitePorId
    @cl_id INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT c.*, t.*, f.* FROM Cliente c 
    INNER JOIN Frm_RenovacionLicencia f ON c.cl_id = f.cl_id
    INNER JOIN Tramites t ON f.tr_id = t.tr_id
    WHERE c.cl_id = @cl_id;
END;


ALTER PROCEDURE ObtenerDetalleTramitePorId
    @cl_id INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT c.*, t.*, f.*, p.* FROM Cliente c
    INNER JOIN Frm_RenovacionLicencia f ON c.cl_id = f.cl_id
    INNER JOIN Tramites t ON f.tr_id = t.tr_id
    LEFT JOIN Pagos p ON c.cl_id = p.cl_id
    WHERE c.cl_id = @cl_id;
END;
EXEC ObtenerDetalleTramitePorId 60;


EXEC ObtenerDetalleTramitePorId 83;



CREATE PROCEDURE sp_ObtenerClientePaginado
    @PageSize INT, @PageNumber INT
AS BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    SELECT * FROM Cliente ORDER BY cl_id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;


EXEC sp_ObtenerCliente @PageSize = 10, @PageNumber = 1;

EXEC sp_ObtenerCliente @PageSize = 10, @PageNumber = 2;




