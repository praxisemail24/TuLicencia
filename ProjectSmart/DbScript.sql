CREATE DATABASE TuLicenciaPR;

USE DB_163557_tulicencia;

SELECT * FROM Pueblos;
SELECT * FROM Cliente;
SELECT * FROM Pagos;
SELECT * FROM Tramites;


CREATE TABLE Pueblos (
    pl_id INT IDENTITY(1,1) PRIMARY KEY,
    pl_nombre NVARCHAR(100)
);

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





--CODIGO 

CREATE PROCEDURE sp_ObtenerPueblosPorId
(@pl_id INT)
as
begin
	SELECT * FROM Pueblos where pl_id = @pl_id	
end

EXEC sp_ObtenerClientePorId 32;

sp_ObtenerClientePorId
CREATE PROCEDURE sp_ObtenerClientePorId
(@cl_id INT)
as
begin
	SELECT * FROM Cliente where cl_id = @cl_id	
end

CREATE PROCEDURE sp_ObtenerDatosporLogin
(@cl_nombreUsuario NVARCHAR(255), @cl_contrasena NVARCHAR(255))
as
begin
	SELECT * FROM Cliente WHERE cl_nombreUsuario = @cl_nombreUsuario  AND cl_contrasena = @cl_contrasena
end


SELECT * FROM Cliente WHERE cl_nombreUsuario = 'juanUsu'  AND cl_contrasena = '12456512'

ALTER PROCEDURE sp_RegistrarCliente
@pl_id INT,
@cl_nombre NVARCHAR(255),
@cl_primerApellido NVARCHAR(255),
@cl_segundoApellido NVARCHAR(255),
@cl_zip NVARCHAR(5),
@cl_direccion NVARCHAR(255),
@cl_numeroLicencia NVARCHAR(15),
@cl_numeroSeguro NVARCHAR(11),
@cl_fechaNacimiento DATE,
@cl_numeroTelefono NVARCHAR(255),
@cl_correo NVARCHAR(255),
@cl_nombreUsuario NVARCHAR(255),
@cl_contrasena NVARCHAR(255),
@cl_fechaRegistro DATE,
@cl_estado INT,
@Resultado INT output
AS BEGIN
SET NOCOUNT ON;
INSERT INTO Cliente (pl_id, cl_nombre, cl_primerApellido, cl_segundoApellido, cl_zip, cl_direccion, cl_numeroLicencia, cl_numeroSeguro,
cl_fechaNacimiento, cl_numeroTelefono, cl_correo, cl_nombreUsuario, cl_contrasena, cl_fechaRegistro, cl_estado)
VALUES (@pl_id, @cl_nombre, @cl_primerApellido, @cl_segundoApellido, @cl_zip, @cl_direccion, @cl_numeroLicencia, @cl_numeroSeguro, @cl_fechaNacimiento, 
@cl_numeroTelefono, @cl_correo, @cl_nombreUsuario, @cl_contrasena, @cl_fechaRegistro, @cl_estado);
SET @Resultado = SCOPE_IDENTITY();
END





EXEC sp_rename 'Cliente.cl_direcion', 'cl_direccion', 'COLUMN';


ALTER PROCEDURE sp_ActualizarCliente
    @cl_id INT,
    @pl_id INT,
    @cl_nombre NVARCHAR(255),
    @cl_primerApellido NVARCHAR(255),
    @cl_segundoApellido NVARCHAR(255),
    @cl_zip NVARCHAR(5),
    @cl_direccion NVARCHAR(255), 
    @cl_numeroLicencia NVARCHAR(15),
    @cl_numeroSeguro NVARCHAR(11),
    @cl_fechaNacimiento DATE,
    @cl_numeroTelefono NVARCHAR(255),
    @cl_correo NVARCHAR(255),
    @cl_nombreUsuario NVARCHAR(255),
    @cl_contrasena NVARCHAR(255),
    @cl_estado INT,
    @Resultado INT OUTPUT
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE Cliente
    SET 
        pl_id = @pl_id,
        cl_nombre = @cl_nombre,
        cl_primerApellido = @cl_primerApellido,
        cl_segundoApellido = @cl_segundoApellido,
        cl_zip = @cl_zip,
        cl_direccion = @cl_direccion, 
        cl_numeroLicencia = @cl_numeroLicencia,
        cl_numeroSeguro = @cl_numeroSeguro,
        cl_fechaNacimiento = @cl_fechaNacimiento,
        cl_numeroTelefono = @cl_numeroTelefono,
        cl_correo = @cl_correo,
        cl_nombreUsuario = @cl_nombreUsuario,
        cl_contrasena = @cl_contrasena,
        cl_estado = @cl_estado
    WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END;



INSERT INTO Usuario (Name, Email, Password)
VALUES ('Juan Perez', 'juan@gmail.com', '1234');




CREATE PROCEDURE sp_ObtenerCliente
as
begin
SELECT * FROM Cliente
end

CREATE PROCEDURE sp_EliminarCliente
@cl_id INT,
@Resultado INT OUTPUT
AS 
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Cliente 
    WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END


CREATE PROCEDURE RegistrarUsuario
@Name NVARCHAR(100),
@Email NVARCHAR(100),
@Password NVARCHAR(100),
@Resultado bit output
AS BEGIN
SET NOCOUNT ON;
INSERT INTO Usuario (Name, Email, Password)
VALUES (@Name, @Email, @Password);
SET @Resultado = SCOPE_IDENTITY();
END;

CREATE PROCEDURE ActualizarUsuario
@Id INT,
@Name NVARCHAR(100),
@Email NVARCHAR(100),
@Password NVARCHAR(100),
@Resultado INT OUTPUT
AS 
BEGIN
    SET NOCOUNT ON;    
    UPDATE Usuario 
    SET Name = @Name, Email = @Email, Password = @Password
    WHERE Id = @Id;
    SET @Resultado = @@ROWCOUNT;
END


CREATE PROCEDURE EliminarUsuario
@Id INT,
@Resultado INT OUTPUT
AS 
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Usuario 
    WHERE Id = @Id;
    SET @Resultado = @@ROWCOUNT;
END


CREATE PROCEDURE ObtenerUsuario
as
begin
SELECT * FROM Usuario
end

CREATE PROCEDURE ObtenerUsuarioPorId
(@Id INT)
as
begin
	SELECT * FROM Usuario where id = @Id	
end

SELECT * FROM Cliente