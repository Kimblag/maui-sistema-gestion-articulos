CREATE DATABASE BDCatalogo;

GO

USE BDCatalogo;

GO

CREATE TABLE Marcas (
	IdMarca INT PRIMARY KEY IDENTITY(1, 1),
	Descripcion NVARCHAR(50) NOT NULL
);


GO

CREATE TABLE Categorias (
	IdCategoria INT PRIMARY KEY IDENTITY(1, 1),
	Descripcion NVARCHAR(50) NOT NULL
);

GO

CREATE TABLE Articulos (
	IdArticulo INT PRIMARY KEY IDENTITY(1, 1),
	Codigo NVARCHAR(50) NOT NULL,
	Nombre NVARCHAR(50) NOT NULL,
	Descripcion NVARCHAR(255),
	Precio Decimal(10, 2) NOT NULL DEFAULT 0,
	IdMarca INT NOT NULL,
	IdCategoria INT NOT NULL,
	CONSTRAINT FK_Articulos_Marcas FOREIGN KEY(IdMarca) REFERENCES Marcas(IdMarca),
	CONSTRAINT FK_Articulos_Categorias FOREIGN KEY(IdCategoria) REFERENCES Categorias(IdCategoria)
);


GO

CREATE TABLE Imagenes (
	IdImagen INT PRIMARY KEY IDENTITY(1, 1),
	UrlImagen NVARCHAR(255) NOT NULL,
	IdArticulo INT NOT NULL,
	CONSTRAINT FK_Imagenes_Articulos FOREIGN KEY(IdArticulo) REFERENCES Articulos(IdArticulo)
);