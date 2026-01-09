-- 1. LIMPIEZA (Borrar tablas si existen para evitar conflictos)
-- --------------------------------------------------------------------------------
IF OBJECT_ID('dbo.ColaboradoresEmpresas', 'U') IS NOT NULL DROP TABLE dbo.ColaboradoresEmpresas;
IF OBJECT_ID('dbo.Colaboradores', 'U') IS NOT NULL DROP TABLE dbo.Colaboradores;
IF OBJECT_ID('dbo.Empresas', 'U') IS NOT NULL DROP TABLE dbo.Empresas;
IF OBJECT_ID('dbo.Municipios', 'U') IS NOT NULL DROP TABLE dbo.Municipios;
IF OBJECT_ID('dbo.Departamentos', 'U') IS NOT NULL DROP TABLE dbo.Departamentos;
IF OBJECT_ID('dbo.Paises', 'U') IS NOT NULL DROP TABLE dbo.Paises;
GO

-- 2. CREACION DE TABLAS
-- --------------------------------------------------------------------------------

CREATE TABLE Paises (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(MAX) NOT NULL -- 'MAX' para asegurar compatibilidad total con string de C#
);

CREATE TABLE Departamentos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(MAX) NOT NULL,
    PaisId INT NOT NULL,
    CONSTRAINT FK_Departamentos_Paises_PaisId FOREIGN KEY (PaisId) REFERENCES Paises(Id) ON DELETE CASCADE
);

CREATE TABLE Municipios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(MAX) NOT NULL,
    DepartamentoId INT NOT NULL,
    CONSTRAINT FK_Municipios_Departamentos_DepartamentoId FOREIGN KEY (DepartamentoId) REFERENCES Departamentos(Id) ON DELETE CASCADE
);

CREATE TABLE Empresas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial NVARCHAR(MAX) NOT NULL,
    NombreComercial NVARCHAR(MAX) NOT NULL,
    Nit NVARCHAR(450) NOT NULL, -- 450 es el maximo permitido para indices/unique en algunas versiones de SQL
    Direccion NVARCHAR(MAX) NOT NULL,
    Telefono NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    CONSTRAINT AK_Empresas_Nit UNIQUE (Nit) -- Unicidad para el NIT
);

CREATE TABLE Colaboradores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(MAX) NOT NULL,
    Edad INT NOT NULL,
    Telefono NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(450) NOT NULL,
    CONSTRAINT AK_Colaboradores_Email UNIQUE (Email) -- Unicidad para el Email
);

-- Tabla Intermedia (Nombre coincidiendo con el DbSet 'ColaboradoresEmpresas')
CREATE TABLE ColaboradoresEmpresas (
    ColaboradorId INT NOT NULL,
    EmpresaId INT NOT NULL,
    CONSTRAINT PK_ColaboradoresEmpresas PRIMARY KEY (ColaboradorId, EmpresaId),
    CONSTRAINT FK_ColaboradoresEmpresas_Colaboradores_ColaboradorId FOREIGN KEY (ColaboradorId) REFERENCES Colaboradores(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ColaboradoresEmpresas_Empresas_EmpresaId FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id) ON DELETE CASCADE
);
GO

-- 3. INSERTAR DATOS DE PRUEBA
-- --------------------------------------------------------------------------------

SET IDENTITY_INSERT Paises ON;
INSERT INTO Paises (Id, Nombre) VALUES (1, 'Guatemala');
INSERT INTO Paises (Id, Nombre) VALUES (2, 'El Salvador');
SET IDENTITY_INSERT Paises OFF;

SET IDENTITY_INSERT Departamentos ON;
INSERT INTO Departamentos (Id, Nombre, PaisId) VALUES (1, 'Guatemala', 1);
INSERT INTO Departamentos (Id, Nombre, PaisId) VALUES (2, 'Sacatepéquez', 1);
SET IDENTITY_INSERT Departamentos OFF;

SET IDENTITY_INSERT Municipios ON;
INSERT INTO Municipios (Id, Nombre, DepartamentoId) VALUES (1, 'Ciudad de Guatemala', 1);
INSERT INTO Municipios (Id, Nombre, DepartamentoId) VALUES (2, 'Mixco', 1);
INSERT INTO Municipios (Id, Nombre, DepartamentoId) VALUES (3, 'Antigua Guatemala', 2);
SET IDENTITY_INSERT Municipios OFF;

SET IDENTITY_INSERT Empresas ON;
INSERT INTO Empresas (Id, RazonSocial, NombreComercial, Nit, Direccion, Telefono, Email) 
VALUES (1, 'Tecnologia Avanzada S.A.', 'TechSolutions', '123456-7', 'Zona 10', '2222-0000', 'info@tech.com');
INSERT INTO Empresas (Id, RazonSocial, NombreComercial, Nit, Direccion, Telefono, Email) 
VALUES (2, 'Comercializadora Global', 'GlobalTrade', '987654-K', 'Zona 1', '2222-1111', 'ventas@global.com');
SET IDENTITY_INSERT Empresas OFF;

SET IDENTITY_INSERT Colaboradores ON;
INSERT INTO Colaboradores (Id, NombreCompleto, Edad, Telefono, Email) 
VALUES (1, 'Juan Perez', 30, '5555-4444', 'juan@gmail.com');
INSERT INTO Colaboradores (Id, NombreCompleto, Edad, Telefono, Email) 
VALUES (2, 'Maria Lopez', 25, '4444-3333', 'maria@gmail.com');
SET IDENTITY_INSERT Colaboradores OFF;

-- Asignaciones Muchos a Muchos
INSERT INTO ColaboradoresEmpresas (ColaboradorId, EmpresaId) VALUES (1, 1); -- Juan -> TechSolutions
INSERT INTO ColaboradoresEmpresas (ColaboradorId, EmpresaId) VALUES (2, 1); -- Maria -> TechSolutions
INSERT INTO ColaboradoresEmpresas (ColaboradorId, EmpresaId) VALUES (2, 2); -- Maria -> GlobalTrade

GO
