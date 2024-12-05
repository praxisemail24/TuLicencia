CREATE DATABASE TuLicenciaPR;

USE TuLicenciaPR;


CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Password NVARCHAR(100)
);
SELECT * FROM Cliente;
SELECT * FROM Usuario;
CREATE TABLE Cliente (
    cl_id INT IDENTITY(1,1) PRIMARY KEY,
    cl_nombre NVARCHAR(100),
    cl_primerApellido NVARCHAR(100),
	cl_segundoApellido NVARCHAR(100),
	cl_numeroLicencia NVARCHAR(100),
    cl_numeroSeguro NVARCHAR(100),
	cl_fechaNacimiento DATE,
    cl_numeroTelefono NVARCHAR(100),
    cl_correo NVARCHAR(100),
    cl_nombreUsuario NVARCHAR(100),
    cl_contrasena NVARCHAR(100)
);

CREATE PROCEDURE sp_RegistrarCliente
@cl_nombre NVARCHAR(100),
@cl_primerApellido NVARCHAR(100),
@cl_segundoApellido NVARCHAR(100),
@cl_numeroLicencia NVARCHAR(100),
@cl_numeroSeguro NVARCHAR(100),
@cl_fechaNacimiento NVARCHAR(100),
@cl_numeroTelefono NVARCHAR(100),
@cl_correo NVARCHAR(100),
@cl_nombreUsuario NVARCHAR(100),
@cl_contrasena NVARCHAR(100),
@Resultado bit output
AS BEGIN
SET NOCOUNT ON;
INSERT INTO Cliente (cl_nombre, cl_primerApellido, cl_segundoApellido, cl_numeroLicencia, cl_numeroSeguro,
cl_fechaNacimiento, cl_numeroTelefono, cl_correo, cl_nombreUsuario, cl_contrasena)
VALUES (@cl_nombre, @cl_primerApellido, @cl_segundoApellido, @cl_numeroLicencia, @cl_numeroSeguro, @cl_fechaNacimiento, 
@cl_numeroTelefono, @cl_correo, @cl_nombreUsuario, @cl_contrasena);
SET @Resultado = SCOPE_IDENTITY();
END


INSERT INTO Usuario (Name, Email, Password)
VALUES ('Juan Perez', 'juan@gmail.com', '1234');


CREATE PROCEDURE sp_ActualizarCliente
@cl_id INT,
@cl_nombre NVARCHAR(100),
@cl_primerApellido NVARCHAR(100),
@cl_segundoApellido NVARCHAR(100),
@cl_numeroLicencia NVARCHAR(100),
@cl_numeroSeguro NVARCHAR(100),
@cl_fechaNacimiento NVARCHAR(100),
@cl_numeroTelefono NVARCHAR(100),
@cl_correo NVARCHAR(100),
@cl_nombreUsuario NVARCHAR(100),
@cl_contrasena NVARCHAR(100),
@Resultado INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;    
    UPDATE Cliente 
    SET cl_nombre = @cl_nombre, cl_primerApellido = @cl_primerApellido, cl_segundoApellido = @cl_segundoApellido, cl_numeroLicencia =@cl_numeroLicencia,
	cl_numeroSeguro = @cl_numeroSeguro, cl_fechaNacimiento = @cl_fechaNacimiento, cl_numeroTelefono = @cl_numeroTelefono, cl_correo = @cl_correo, 
	cl_nombreUsuario = @cl_nombreUsuario , cl_contrasena = @cl_contrasena
    WHERE cl_id = @cl_id;
    SET @Resultado = @@ROWCOUNT;
END;

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
