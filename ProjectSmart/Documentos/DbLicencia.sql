CREATE DATABASE TuLicenciaPR;

USE DB_163557_tulicencia;

SELECT * FROM Pueblos;
SELECT * FROM Cliente;
SELECT * FROM Pagos;
SELECT * FROM Tramites;
SELECT * FROM TipoTramites;
SELECT * FROM Frm_RenovacionLicencia;
SELECT * FROM Frm_LicenciaReciprocidad;

ALTER TABLE Frm_RenovacionLicencia
DROP COLUMN frl_codigoPostal;


ALTER TABLE Frm_RenovacionLicencia
ADD frl_codigoPostal INT;




--- TABLAS

CREATE TABLE Pueblos (
    pl_id INT IDENTITY(1,1) PRIMARY KEY,
    pl_nombre NVARCHAR(100)
);
INSERT INTO Pueblos (pl_nombre) VALUES ('Sin pueblo');

CREATE TABLE Cliente (
    cl_id INT IDENTITY(1,1) PRIMARY KEY,
    pl_id INT,
    cl_nombre NVARCHAR(255),
    cl_primerApellido NVARCHAR(255),
    cl_segundoApellido NVARCHAR(255),
    cl_zip NVARCHAR(5),
    cl_direccion NVARCHAR(255),
    cl_numeroLicencia NVARCHAR(15),
    cl_numeroSeguro NVARCHAR(11),
    cl_fechaNacimiento DATE,
    cl_numeroTelefono NVARCHAR(255),
    cl_correo NVARCHAR(255),
    cl_nombreUsuario NVARCHAR(255),
    cl_contrasena NVARCHAR(255),
    cl_fechaRegistro DATE,
    cl_estado INT,
    FOREIGN KEY (pl_id) REFERENCES Pueblos(pl_id)
);

CREATE TABLE TipoTramites (
    tpr_id INT IDENTITY(1,1) PRIMARY KEY,
    tpr_nombre NVARCHAR(255),
    tpr_estado INT
);

CREATE TABLE Tramites (
    tr_id INT IDENTITY(1,1) PRIMARY KEY,
    tpr_id INT,
    tr_nombre NVARCHAR(255),
    tr_estado INT,
    tr_precio FLOAT(3),
    FOREIGN KEY (tpr_id) REFERENCES TipoTramites(tpr_id)
);

CREATE TABLE Archivos (
    ar_id INT IDENTITY(1,1) PRIMARY KEY,
    tr_id INT,    
    cl_id INT,
    frm_id INT,
    ar_nombre NVARCHAR(255),
    ar_fecha DATETIME,
    ar_estado INT,
    FOREIGN KEY (cl_id) REFERENCES Cliente(cl_id),
	FOREIGN KEY (tr_id) REFERENCES Tramites(tr_id)
);


INSERT INTO TipoTramites (tpr_nombre, tpr_estado)
VALUES ('Persona', 1),
       ('Vehículos', 1);

INSERT INTO Tramites (tpr_id, tr_nombre, tr_estado, tr_precio)
VALUES
(1, 'Renovación de Licencia', 1, 1),
(1, 'Licencia de Aprendizaje', 1, 1),
(1, 'Duplicado de Licencia', 1, 1),
(1, 'Reciprocidad de Licencia', 1, 1),
(2, 'Traspaso de Vehículos', 1, 1),
(2, 'Gestión de Titulo', 1, 1),
(2, 'Tablillas Especiales', 1, 1),
(2, 'Gravámenes', 1, 1);


CREATE TABLE Pagos (
	pg_id INT IDENTITY(1,1) PRIMARY KEY,
	cl_id INT,
	tr_id INT,
	pg_fecha DATE,
	pg_codigo NVARCHAR(20),
	pg_estado INT,
	pg_nota NVARCHAR(20),
	pg_status NVARCHAR(255),
	FOREIGN KEY (cl_id) REFERENCES Cliente(cl_id),
	FOREIGN KEY (tr_id) REFERENCES Tramites(tr_id)
);


-- FORMULARIOS

CREATE TABLE Frm_LicenciaAprendizaje (
	fla_id int IDENTITY(1,1) NOT NULL,
	cl_id int NULL,
	pg_id int NULL,
	tr_id int NULL,
	fla_fecha datetime NULL,
	fla_estado int NULL,
	fla_tipoLicencia NVARCHAR(50),
	fla_identificacion NVARCHAR(50),
	fla_numero NVARCHAR(25),
	fla_statusLegal NVARCHAR(255),
	fla_genero NVARCHAR(10),
	fla_donante NVARCHAR(2),
	fla_tipoSangre NVARCHAR(20),
	fla_talla NVARCHAR(20),
	fla_peso NVARCHAR(20),
	fla_tez NVARCHAR(20),
	fla_colorPelo NVARCHAR(20),
	fla_colorOjo NVARCHAR(20),
	fla_nombrePadre NVARCHAR(255),
	fla_nombreMadre NVARCHAR(255),
	fla_direccion NVARCHAR(255),
	fla_numeroDireccion NVARCHAR(255),
	fla_pueblo NVARCHAR(100),
	fla_codigoPostal NVARCHAR(5),
	fla_barrio NVARCHAR(255),
	fla_apartado NVARCHAR(255),
	fla_pueblo2 NVARCHAR(100),
	fla_codigoPostal2 NVARCHAR(5),
	fla_licencia NVARCHAR(2),
	fla_procedeLicencia NVARCHAR(50),
	fla_licenciaSuspendida NVARCHAR(2),
	fla_motivoSuspendido NVARCHAR(50),
	fla_recluido NVARCHAR(2),
	fla_convictoBebida NVARCHAR(2),
	fla_fechaConvictoBebida date NULL,
	fla_convictoNarcotico NVARCHAR(2),
	fla_fechaConvictoNarcotico date NULL,
	fla_obligacionAlimentaria NVARCHAR(2),
	fla_deudaAcca NVARCHAR(2),
	FOREIGN KEY (cl_id) REFERENCES Cliente(cl_id),
	FOREIGN KEY (pg_id) REFERENCES Pagos(pg_id),
	FOREIGN KEY (tr_id) REFERENCES Tramites(tr_id)
);


CREATE TABLE Frm_CertificadoMedico (
	fcm_id int IDENTITY(1,1) NOT NULL,
	cl_id int NULL,
	pg_id int NULL,
	tr_id int NULL,
	fcm_fecha datetime NULL,
	fcm_estado int NULL,
	fcm_numeroLicencia NVARCHAR(25),
	fcm_estadoInconciencia NVARCHAR(2),
	fcm_padeceCorazon NVARCHAR(2),
	fcm_marcapaso NVARCHAR(2),
	fcm_protesis NVARCHAR(2),
	fcm_talla NVARCHAR(20),
	fcm_peso NVARCHAR(20),
	fcm_colorPelo NVARCHAR(20),
	fcm_colorOjo NVARCHAR(20),
	FOREIGN KEY (cl_id) REFERENCES Cliente(cl_id),
	FOREIGN KEY (pg_id) REFERENCES Pagos(pg_id),
	FOREIGN KEY (tr_id) REFERENCES Tramites(tr_id)
);

CREATE TABLE Frm_DuplicadoLicencia (
	fdl_id int IDENTITY(1,1) NOT NULL,
	cl_id int NULL,
	pg_id int NULL,
	tr_id int NULL,
	fdl_fecha datetime NULL,
	fdl_estado int NULL,
	fdl_tipoLicencia nvarchar(50),
	fdl_numeroLicencia nvarchar(25),
	fdl_categoria nvarchar(20),
	fdl_vehiculoPesado nvarchar(10),
	fdl_identificacion nvarchar(50),
	fdl_numero nvarchar(25),
	fdl_statusLegal nvarchar(255),
	fdl_genero nvarchar(10),
	fdl_donante nvarchar(2),
	fdl_tipoSangre nvarchar(20),
	fdl_talla nvarchar(20),
	fdl_peso nvarchar(20),
	fdl_tez nvarchar(20),
	fdl_colorPelo nvarchar(20),
	fdl_colorOjo nvarchar(20),
	fdl_direccion nvarchar(255),
	fdl_numeroDireccion nvarchar(10),
	fdl_pueblo nvarchar(100),
	fdl_codigoPostal nvarchar(5),
	fdl_barrio nvarchar(255),
	fdl_apartado nvarchar(255),
	fdl_pueblo2 nvarchar(100),
	fdl_codigoPostal2 nvarchar(5),
	fdl_licenciaSuspendida nvarchar(2),
	fdl_motivoSuspension nvarchar(50),
	fdl_recluido nvarchar(2),
	fdl_convictoBebida nvarchar(2),
	fdl_fechaConvictoBebida date NULL,
	fdl_convictoNarcotico nvarchar(2),
	fdl_fechaConvictoNarcotico date NULL,
	fdl_obligacionAlimentaria nvarchar(2),
	fdl_deudaAcca nvarchar(2),
	FOREIGN KEY (cl_id) REFERENCES Cliente(cl_id),
	FOREIGN KEY (pg_id) REFERENCES Pagos(pg_id),
	FOREIGN KEY (tr_id) REFERENCES Tramites(tr_id)
);



-- SP

CREATE PROCEDURE sp_ObtenerTipoTramite
as
begin
SELECT * FROM TipoTramites
end

CREATE PROCEDURE sp_ObtenerTramite
as
begin
SELECT * FROM Tramites
end










CREATE PROCEDURE sp_RegistrarLicenciaApre
(
    @cl_id INT,
    @pg_id INT,
    @tr_id INT,
    @fla_fecha DATETIME,
    @fla_estado INT,
    @fla_tipoLicencia NVARCHAR(50),
    @fla_identificacion NVARCHAR(50),
    @fla_numero NVARCHAR(25),
    @fla_statusLegal NVARCHAR(255),
    @fla_genero NVARCHAR(10),
    @fla_donante NVARCHAR(2),
    @fla_tipoSangre NVARCHAR(20),
    @fla_talla NVARCHAR(20),
    @fla_peso NVARCHAR(20),
    @fla_tez NVARCHAR(20),
    @fla_colorPelo NVARCHAR(20),
    @fla_colorOjo NVARCHAR(20),
    @fla_nombrePadre NVARCHAR(255),
    @fla_nombreMadre NVARCHAR(255),
    @fla_direccion NVARCHAR(255),
    @fla_numeroDireccion NVARCHAR(255),
    @fla_pueblo NVARCHAR(100),
    @fla_codigoPostal NVARCHAR(5),
    @fla_barrio NVARCHAR(255),
    @fla_apartado NVARCHAR(255),
    @fla_pueblo2 NVARCHAR(100),
    @fla_codigoPostal2 NVARCHAR(5),
    @fla_licencia NVARCHAR(2),
    @fla_procedeLicencia NVARCHAR(50),
    @fla_licenciaSuspendida NVARCHAR(2),
    @fla_motivoSuspendido NVARCHAR(50),
    @fla_recluido NVARCHAR(2),
    @fla_convictoBebida NVARCHAR(2),
    @fla_fechaConvictoBebida DATE,
    @fla_convictoNarcotico NVARCHAR(2),
    @fla_fechaConvictoNarcotico DATE,
    @fla_obligacionAlimentaria NVARCHAR(2),
    @fla_deudaAcca NVARCHAR(2),
    @Resultado INT OUTPUT
)
AS 
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Frm_LicenciaAprendizaje 
    (
        cl_id, pg_id, tr_id, fla_fecha, fla_estado, fla_tipoLicencia, fla_identificacion, fla_numero, fla_statusLegal, fla_genero,
        fla_donante, fla_tipoSangre, fla_talla, fla_peso, fla_tez, fla_colorPelo, fla_colorOjo, fla_nombrePadre, fla_nombreMadre,
        fla_direccion, fla_numeroDireccion, fla_pueblo, fla_codigoPostal, fla_barrio, fla_apartado, fla_pueblo2, fla_codigoPostal2,
        fla_licencia, fla_procedeLicencia, fla_licenciaSuspendida, fla_motivoSuspendido, fla_recluido, fla_convictoBebida,
        fla_fechaConvictoBebida, fla_convictoNarcotico, fla_fechaConvictoNarcotico, fla_obligacionAlimentaria, fla_deudaAcca
    )
    VALUES 
    (
        @cl_id, @pg_id, @tr_id, @fla_fecha, @fla_estado, @fla_tipoLicencia, @fla_identificacion, @fla_numero, @fla_statusLegal,
        @fla_genero, @fla_donante, @fla_tipoSangre, @fla_talla, @fla_peso, @fla_tez, @fla_colorPelo, @fla_colorOjo,
        @fla_nombrePadre, @fla_nombreMadre, @fla_direccion, @fla_numeroDireccion, @fla_pueblo, @fla_codigoPostal, @fla_barrio,
        @fla_apartado, @fla_pueblo2, @fla_codigoPostal2, @fla_licencia, @fla_procedeLicencia, @fla_licenciaSuspendida,
        @fla_motivoSuspendido, @fla_recluido, @fla_convictoBebida, @fla_fechaConvictoBebida, @fla_convictoNarcotico,
        @fla_fechaConvictoNarcotico, @fla_obligacionAlimentaria, @fla_deudaAcca
    );

    SET @Resultado = SCOPE_IDENTITY();
END;



CREATE PROCEDURE sp_ActualizarLicenciaApre
(
    @fla_id INT,
    @cl_id INT,
    @pg_id INT,
    @tr_id INT,
    @fla_fecha DATETIME,
    @fla_estado INT,
    @fla_tipoLicencia NVARCHAR(50),
    @fla_identificacion NVARCHAR(50),
    @fla_numero NVARCHAR(25),
    @fla_statusLegal NVARCHAR(255),
    @fla_genero NVARCHAR(10),
    @fla_donante NVARCHAR(2),
    @fla_tipoSangre NVARCHAR(20),
    @fla_talla NVARCHAR(20),
    @fla_peso NVARCHAR(20),
    @fla_tez NVARCHAR(20),
    @fla_colorPelo NVARCHAR(20),
    @fla_colorOjo NVARCHAR(20),
    @fla_nombrePadre NVARCHAR(255),
    @fla_nombreMadre NVARCHAR(255),
    @fla_direccion NVARCHAR(255),
    @fla_numeroDireccion NVARCHAR(255),
    @fla_pueblo NVARCHAR(100),
    @fla_codigoPostal NVARCHAR(5),
    @fla_barrio NVARCHAR(255),
    @fla_apartado NVARCHAR(255),
    @fla_pueblo2 NVARCHAR(100),
    @fla_codigoPostal2 NVARCHAR(5),
    @fla_licencia NVARCHAR(2),
    @fla_procedeLicencia NVARCHAR(50),
    @fla_licenciaSuspendida NVARCHAR(2),
    @fla_motivoSuspendido NVARCHAR(50),
    @fla_recluido NVARCHAR(2),
    @fla_convictoBebida NVARCHAR(2),
    @fla_fechaConvictoBebida DATE,
    @fla_convictoNarcotico NVARCHAR(2),
    @fla_fechaConvictoNarcotico DATE,
    @fla_obligacionAlimentaria NVARCHAR(2),
    @fla_deudaAcca NVARCHAR(2),
    @Resultado INT OUTPUT
)
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE Frm_LicenciaAprendizaje
    SET 
        cl_id = @cl_id,
        pg_id = @pg_id,
        tr_id = @tr_id,
        fla_fecha = @fla_fecha,
        fla_estado = @fla_estado,
        fla_tipoLicencia = @fla_tipoLicencia, 
        fla_identificacion = @fla_identificacion,
        fla_numero = @fla_numero,
        fla_statusLegal = @fla_statusLegal,
        fla_genero = @fla_genero,
        fla_donante = @fla_donante,
        fla_tipoSangre = @fla_tipoSangre,
        fla_talla = @fla_talla,
        fla_peso = @fla_peso,
        fla_tez = @fla_tez,
        fla_colorPelo = @fla_colorPelo,
        fla_colorOjo = @fla_colorOjo,
        fla_nombrePadre = @fla_nombrePadre,
        fla_nombreMadre = @fla_nombreMadre,
        fla_direccion = @fla_direccion, 
        fla_numeroDireccion = @fla_numeroDireccion,
        fla_pueblo = @fla_pueblo,
        fla_codigoPostal = @fla_codigoPostal,
        fla_barrio = @fla_barrio,
        fla_apartado = @fla_apartado,
        fla_pueblo2 = @fla_pueblo2,
        fla_codigoPostal2 = @fla_codigoPostal2,
        fla_licencia = @fla_licencia,
        fla_procedeLicencia = @fla_procedeLicencia,
        fla_licenciaSuspendida = @fla_licenciaSuspendida,
        fla_motivoSuspendido = @fla_motivoSuspendido,
        fla_recluido = @fla_recluido,
        fla_convictoBebida = @fla_convictoBebida,
        fla_fechaConvictoBebida = @fla_fechaConvictoBebida,
        fla_convictoNarcotico = @fla_convictoNarcotico,
        fla_fechaConvictoNarcotico = @fla_fechaConvictoNarcotico,
        fla_obligacionAlimentaria = @fla_obligacionAlimentaria,
        fla_deudaAcca = @fla_deudaAcca
    WHERE fla_id = @fla_id;

    SET @Resultado = @@ROWCOUNT;
END;








CREATE PROCEDURE sp_ObtenerCertifMed
as
begin
SELECT * FROM Frm_CertificadoMedico
end;

CREATE PROCEDURE sp_ObtenerCertifMedPorId
(@fcm_id INT)
as
begin
	SELECT * FROM Frm_CertificadoMedico where fcm_id = @fcm_id	
end;

CREATE PROCEDURE sp_EliminaCertifMed
@fcm_id INT,
@Resultado INT OUTPUT
AS 
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Frm_CertificadoMedico 
    WHERE fcm_id = @fcm_id;
    SET @Resultado = @@ROWCOUNT;
END;


CREATE PROCEDURE sp_RegistrarCertifMed
(   @cl_id INT, @pg_id INT, @tr_id INT, @fcm_fecha DATETIME,  @fcm_estado INT, @fcm_numeroLicencia NVARCHAR(25), @fcm_estadoInconciencia NVARCHAR(2),
    @fcm_padeceCorazon NVARCHAR(2),  @fcm_marcapaso NVARCHAR(2),  @fcm_protesis NVARCHAR(2),  @fcm_talla NVARCHAR(20),
    @fcm_peso NVARCHAR(20), @fcm_colorPelo NVARCHAR(20),  @fcm_colorOjo NVARCHAR(20),
    @Resultado INT OUTPUT )
AS 
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Frm_CertificadoMedico
    (   cl_id, pg_id, tr_id, fcm_fecha, fcm_estado, fcm_numeroLicencia, fcm_estadoInconciencia, fcm_padeceCorazon,
        fcm_marcapaso, fcm_protesis, fcm_talla, fcm_peso, fcm_colorPelo, fcm_colorOjo )
    VALUES 
    (   @cl_id, @pg_id, @tr_id, @fcm_fecha, @fcm_estado, @fcm_numeroLicencia, @fcm_estadoInconciencia, @fcm_padeceCorazon,
        @fcm_marcapaso, @fcm_protesis, @fcm_talla, @fcm_peso, @fcm_colorPelo, @fcm_colorOjo );
    SET @Resultado = SCOPE_IDENTITY();
END;


CREATE PROCEDURE sp_ActualizarCertifMed
(   @fcm_id INT, @cl_id INT, @pg_id INT, @tr_id INT, @fcm_fecha DATETIME, @fcm_estado INT, @fcm_numeroLicencia NVARCHAR(25),
    @fcm_estadoInconciencia NVARCHAR(2), @fcm_padeceCorazon NVARCHAR(2), @fcm_marcapaso NVARCHAR(2), @fcm_protesis NVARCHAR(2),
    @fcm_talla NVARCHAR(20), @fcm_peso NVARCHAR(20), @fcm_colorPelo NVARCHAR(20), @fcm_colorOjo NVARCHAR(20),
    @Resultado INT OUTPUT )
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Frm_CertificadoMedico
    SET 
        cl_id = @cl_id, pg_id = @pg_id, tr_id = @tr_id, fcm_fecha = @fcm_fecha, fcm_estado = @fcm_estado,
        fcm_numeroLicencia = @fcm_numeroLicencia, fcm_estadoInconciencia = @fcm_estadoInconciencia,
        fcm_padeceCorazon = @fcm_padeceCorazon, fcm_marcapaso = @fcm_marcapaso, fcm_protesis = @fcm_protesis,
        fcm_talla = @fcm_talla,  fcm_peso = @fcm_peso, fcm_colorPelo = @fcm_colorPelo, fcm_colorOjo = @fcm_colorOjo
    WHERE fcm_id = @fcm_id;
    SET @Resultado = @@ROWCOUNT;
END;


CREATE PROCEDURE sp_RegistrarDuplicadoLic
(   @cl_id INT, @pg_id INT, @tr_id INT, @fdl_fecha DATETIME, @fdl_estado INT,  @fdl_tipoLicencia NVARCHAR(50),
    @fdl_numeroLicencia NVARCHAR(25), @fdl_categoria NVARCHAR(20), @fdl_vehiculoPesado NVARCHAR(10), @fdl_identificacion NVARCHAR(50),
    @fdl_numero NVARCHAR(25), @fdl_statusLegal NVARCHAR(255), @fdl_genero NVARCHAR(10), @fdl_donante NVARCHAR(2), @fdl_tipoSangre NVARCHAR(20),
    @fdl_talla NVARCHAR(20), @fdl_peso NVARCHAR(20), @fdl_tez NVARCHAR(20), @fdl_colorPelo NVARCHAR(20), @fdl_colorOjo NVARCHAR(20),
    @fdl_direccion NVARCHAR(255), @fdl_numeroDireccion NVARCHAR(10), @fdl_pueblo NVARCHAR(100), @fdl_codigoPostal NVARCHAR(5),
    @fdl_barrio NVARCHAR(255), @fdl_apartado NVARCHAR(255), @fdl_pueblo2 NVARCHAR(100), @fdl_codigoPostal2 NVARCHAR(5), @fdl_licenciaSuspendida NVARCHAR(2),
    @fdl_motivoSuspension NVARCHAR(50),@fdl_recluido NVARCHAR(2), @fdl_convictoBebida NVARCHAR(2), @fdl_fechaConvictoBebida DATE,
    @fdl_convictoNarcotico NVARCHAR(2), @fdl_fechaConvictoNarcotico DATE, @fdl_obligacionAlimentaria NVARCHAR(2), @fdl_deudaAcca NVARCHAR(2),
    @Resultado INT OUTPUT )
AS 
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Frm_DuplicadoLicencia 
    (   cl_id, pg_id, tr_id, fdl_fecha, fdl_estado, fdl_tipoLicencia, fdl_numeroLicencia, fdl_categoria, fdl_vehiculoPesado,
        fdl_identificacion, fdl_numero, fdl_statusLegal, fdl_genero, fdl_donante, fdl_tipoSangre, fdl_talla, fdl_peso, 
        fdl_tez, fdl_colorPelo, fdl_colorOjo, fdl_direccion, fdl_numeroDireccion, fdl_pueblo, fdl_codigoPostal, fdl_barrio,
        fdl_apartado, fdl_pueblo2, fdl_codigoPostal2, fdl_licenciaSuspendida, fdl_motivoSuspension, fdl_recluido, fdl_convictoBebida, 
        fdl_fechaConvictoBebida, fdl_convictoNarcotico, fdl_fechaConvictoNarcotico, fdl_obligacionAlimentaria, fdl_deudaAcca )
    VALUES 
    (   @cl_id, @pg_id, @tr_id, @fdl_fecha, @fdl_estado, @fdl_tipoLicencia, @fdl_numeroLicencia, @fdl_categoria, @fdl_vehiculoPesado,
		@fdl_identificacion, @fdl_numero, @fdl_statusLegal, @fdl_genero, @fdl_donante, @fdl_tipoSangre, @fdl_talla, @fdl_peso, @fdl_tez,
		@fdl_colorPelo, @fdl_colorOjo, @fdl_direccion, @fdl_numeroDireccion, @fdl_pueblo, @fdl_codigoPostal, @fdl_barrio, @fdl_apartado,
		@fdl_pueblo2, @fdl_codigoPostal2, @fdl_licenciaSuspendida, @fdl_motivoSuspension, @fdl_recluido, @fdl_convictoBebida, @fdl_fechaConvictoBebida,
        @fdl_convictoNarcotico, @fdl_fechaConvictoNarcotico, @fdl_obligacionAlimentaria, @fdl_deudaAcca
    );  SET @Resultado = SCOPE_IDENTITY();
END;


CREATE PROCEDURE sp_ActualizarDuplicadoLic
(	@fdl_id INT, @cl_id INT, @pg_id INT, @tr_id INT, @fdl_fecha DATETIME, @fdl_estado INT, @fdl_tipoLicencia NVARCHAR(50), @fdl_numeroLicencia NVARCHAR(25),
    @fdl_categoria NVARCHAR(20),  @fdl_vehiculoPesado NVARCHAR(10), @fdl_identificacion NVARCHAR(50), @fdl_numero NVARCHAR(25),
    @fdl_statusLegal NVARCHAR(255), @fdl_genero NVARCHAR(10), @fdl_donante NVARCHAR(2), @fdl_tipoSangre NVARCHAR(20), @fdl_talla NVARCHAR(20),
    @fdl_peso NVARCHAR(20), @fdl_tez NVARCHAR(20), @fdl_colorPelo NVARCHAR(20), @fdl_colorOjo NVARCHAR(20), @fdl_direccion NVARCHAR(255),
    @fdl_numeroDireccion NVARCHAR(10), @fdl_pueblo NVARCHAR(100), @fdl_codigoPostal NVARCHAR(5), @fdl_barrio NVARCHAR(255),
    @fdl_apartado NVARCHAR(255), @fdl_pueblo2 NVARCHAR(100), @fdl_codigoPostal2 NVARCHAR(5), @fdl_licenciaSuspendida NVARCHAR(2),
    @fdl_motivoSuspension NVARCHAR(50), @fdl_recluido NVARCHAR(2), @fdl_convictoBebida NVARCHAR(2), @fdl_fechaConvictoBebida DATE,
    @fdl_convictoNarcotico NVARCHAR(2), @fdl_fechaConvictoNarcotico DATE, @fdl_obligacionAlimentaria NVARCHAR(2), @fdl_deudaAcca NVARCHAR(2),
    @Resultado INT OUTPUT )
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Frm_DuplicadoLicencia
    SET 
        cl_id = @cl_id, pg_id = @pg_id, tr_id = @tr_id, fdl_fecha = @fdl_fecha, fdl_estado = @fdl_estado, fdl_tipoLicencia = @fdl_tipoLicencia,
        fdl_numeroLicencia = @fdl_numeroLicencia, fdl_categoria = @fdl_categoria, fdl_vehiculoPesado = @fdl_vehiculoPesado,
        fdl_identificacion = @fdl_identificacion, fdl_numero = @fdl_numero, fdl_statusLegal = @fdl_statusLegal, fdl_genero = @fdl_genero,
        fdl_donante = @fdl_donante, fdl_tipoSangre = @fdl_tipoSangre, fdl_talla = @fdl_talla, fdl_peso = @fdl_peso, fdl_tez = @fdl_tez,
        fdl_colorPelo = @fdl_colorPelo, fdl_colorOjo = @fdl_colorOjo, fdl_direccion = @fdl_direccion, fdl_numeroDireccion = @fdl_numeroDireccion,
        fdl_pueblo = @fdl_pueblo, fdl_codigoPostal = @fdl_codigoPostal, fdl_barrio = @fdl_barrio, fdl_apartado = @fdl_apartado,
        fdl_pueblo2 = @fdl_pueblo2, fdl_codigoPostal2 = @fdl_codigoPostal2, fdl_licenciaSuspendida = @fdl_licenciaSuspendida,
        fdl_motivoSuspension = @fdl_motivoSuspension, fdl_recluido = @fdl_recluido, fdl_convictoBebida = @fdl_convictoBebida,
        fdl_fechaConvictoBebida = @fdl_fechaConvictoBebida, fdl_convictoNarcotico = @fdl_convictoNarcotico, fdl_fechaConvictoNarcotico = @fdl_fechaConvictoNarcotico,
        fdl_obligacionAlimentaria = @fdl_obligacionAlimentaria, fdl_deudaAcca = @fdl_deudaAcca
    WHERE fdl_id = @fdl_id;
    SET @Resultado = @@ROWCOUNT;
END;


CREATE PROCEDURE sp_ObtenerDuplicadoLic
ASS
BEGIN
    SELECT * FROM Frm_DuplicadoLicencia;
END;


CREATE PROCEDURE sp_ObtenerDuplicadoLicPorId
( @fdl_id INT )
AS
BEGIN 
    SELECT * FROM Frm_DuplicadoLicencia WHERE fdl_id = @fdl_id;
END;


CREATE PROCEDURE sp_EliminaDuplicadoLic
( @fcm_id INT,@Resultado INT OUTPUT )
AS 
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Frm_DuplicadoLicencia 
    WHERE fdl_id = @fdl_id;
    SET @Resultado = @@ROWCOUNT;
END;


CREATE PROCEDURE sp_RegistrarArchivo
	@cl_id INT, @tr_id INT, @frm_id INT, @ar_nombre NVARCHAR(255),
    @ar_fecha DATETIME, @ar_estado INT,@Resultado INT output
AS BEGIN
	SET NOCOUNT ON;
    INSERT INTO Archivos ( cl_id, tr_id, frm_id, ar_nombre, ar_fecha, ar_estado)
    VALUES ( @cl_id, @tr_id, @frm_id, @ar_nombre, @ar_fecha, @ar_estado); SET @Resultado = SCOPE_IDENTITY();
END;



CREATE  PROCEDURE sp_ObtenerPago
AS BEGIN
    SELECT * FROM Pagos;
END


CREATE PROCEDURE sp_ActualizarClientePassword
    @cl_id INT,
    @cl_contrasena NVARCHAR(255),
    @Resultado INT output
AS
BEGIN
    UPDATE Cliente
    SET cl_contrasena = @cl_contrasena
    WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END;


CREATE PROCEDURE sp_ObtenerClientePorCorreo
(@cl_correo VARCHAR(255))
AS
BEGIN
    SELECT * FROM Cliente WHERE cl_correo = @cl_correo;
END


ALTER PROCEDURE ObtenerPagoPorCodigoPago
    @pg_codigo NVARCHAR(20)
AS BEGIN
    SELECT * FROM Pagos WHERE pg_codigo = @pg_codigo;
END;



ALTER PROCEDURE ObtenerPagoPorCodigoPago
    @pg_codigo NVARCHAR(20)
AS 
BEGIN
    SELECT P.*, C.cl_nombre, C.cl_primerApellido, C.cl_direccion, C.cl_numeroTelefono,  C.cl_zip,
        T.tr_nombre, T.tr_precio
    FROM 
        Pagos P
    INNER JOIN 
        Cliente C ON P.cl_id = C.cl_id
    INNER JOIN 
        Tramites T ON P.tr_id = T.tr_id
    WHERE 
        P.pg_codigo = @pg_codigo;
END;

CREATE PROCEDURE sp_ObtenerClientePorKeyTemporal
(@cl_keyTemporal  NVARCHAR(15))
as begin
	SELECT * FROM Cliente where cl_keyTemporal = @cl_keyTemporal	
end

CREATE PROCEDURE sp_ActualizarClientePassword
    @cl_id INT,  @cl_contrasena NVARCHAR(255), @cl_keyTemporal NVARCHAR(15),
    @Resultado INT output
AS
BEGIN
    UPDATE Cliente SET cl_contrasena = @cl_contrasena, cl_keyTemporal = ''
    WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END;


ALTER PROCEDURE sp_ActualizarClientePassword
    @cl_id INT, @cl_contrasena NVARCHAR(255), @cl_keyTemporal NVARCHAR(15), @Resultado INT OUTPUT
AS BEGIN
    -- Verificar si hay un valor en cl_keyTemporal
    IF EXISTS (SELECT 1 FROM Cliente WHERE cl_id = @cl_id AND cl_keyTemporal = @cl_keyTemporal)
	    BEGIN
	        -- Actualizar la contraseña y limpiar cl_keyTemporal
	        UPDATE Cliente SET cl_contrasena = @cl_contrasena, cl_keyTemporal = '' WHERE cl_id = @cl_id;	
	        -- Establecer el valor de @Resultado como el número de filas afectadas por la actualización
	        SET @Resultado = @@ROWCOUNT;
	    END
    ELSE BEGIN
        SET @Resultado = @@ROWCOUNT;
    END
END;





CREATE TABLE Estados (
    e_id INT PRIMARY KEY IDENTITY,
    e_nombre NVARCHAR(100) NOT NULL
);

insert into Estados (e_nombre) values ('Alabama'), ('Alaska'), ('Arizona'), ('Arkansas'), ('California'), ('Colorado'), ('Connecticut'), ('Delaware'), ('District of Columbia'), ('Florida'), ('Georgia'), ('Hawaii'), ('Idaho'), ('Illinois'), ('Indiana'), ('Iowa'), ('Kansas'), ('Kentucky'), ('Louisiana'), ('Maine'), ('Maryland'), ('Massachusetts'), ('Michigan'), ('Minnesota'), ('Mississippi'), ('Missouri'), ('Montana'), ('Nebraska'), ('Nevada'), ('New Hampshire'), ('New Jersey'), ('New Mexico'), ('New York'), ('North Carolina'), ('North Dakota'), ('Ohio'), ('Oklahoma'), ('Oregon'), ('Pennsylvania'), ('Rhode Island'), ('South Carolina'), ('South Dakota'), ('Tennessee'), ('Texas'), ('Utah'), ('Vermont'), ('Virginia'), ('Washington'), ('West Virginia'), ('Wisconsin'), ('Wyoming'), ('US virgin Island');






