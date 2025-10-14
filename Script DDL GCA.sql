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


GO
--Mock Data para testeo
INSERT INTO Marcas (Descripcion) VALUES
('Sony'),
('Samsung'),
('Apple'),
('LG'),
('Lenovo'),
('HP'),
('Asus'),
('Dell'),
('Microsoft'),
('Xiaomi');
GO
INSERT INTO Categorias (Descripcion) VALUES
('Smartphones'),
('Televisores'),
('Laptops'),
('Auriculares'),
('Tablets'),
('Monitores'),
('Cámaras'),
('Smartwatch'),
('Impresoras'),
('Accesorios');

GO
INSERT INTO Articulos (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) VALUES
('A1001', 'iPhone 14', 'Smartphone Apple con chip A15 Bionic', 999.99, 3, 1),
('A1002', 'Galaxy S23', 'Smartphone Samsung de alta gama con cámara triple', 899.50, 2, 1),
('A1003', 'Bravia X90J', 'Televisor 4K HDR con procesador XR de Sony', 1200.00, 1, 2),
('A1004', 'OLED C1', 'Televisor OLED LG 55 pulgadas con AI ThinQ', 1350.00, 4, 2),
('A1005', 'ThinkPad X1 Carbon', 'Laptop ultraliviana de Lenovo con Intel i7', 1550.00, 5, 3),
('A1006', 'Spectre x360', 'Laptop convertible HP con pantalla táctil', 1425.00, 6, 3),
('A1007', 'ROG Strix G16', 'Laptop gamer Asus con RTX 4070', 1899.99, 7, 3),
('A1008', 'AirPods Pro 2', 'Auriculares inalámbricos con cancelación activa', 249.99, 3, 4),
('A1009', 'WH-1000XM5', 'Auriculares Sony con noise cancelling avanzado', 379.00, 1, 4),
('A1010', 'Mi Pad 6', 'Tablet Xiaomi con Snapdragon 870 y pantalla 144Hz', 420.00, 10, 5),
('A1011', 'Surface Pro 9', 'Tablet/laptop 2 en 1 de Microsoft', 1150.00, 9, 5),
('A1012', 'Canon EOS R10', 'Cámara mirrorless APS-C de alta velocidad', 1100.00, 1, 7),
('A1013', 'Galaxy Watch 6', 'Reloj inteligente con sensor ECG', 299.99, 2, 8),
('A1014', 'DeskJet 4120e', 'Impresora multifunción inalámbrica HP', 130.00, 6, 9),
('A1015', 'MX Master 3S', 'Mouse ergonómico recargable', 99.99, 9, 10);


GO

INSERT INTO Imagenes (UrlImagen, IdArticulo) VALUES
('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTR3ZCalaHH45ZWIsgIa4RPjKbcYYCzo6bU3w&s', 1),  -- smartphone cercano
('https://img.global.news.samsung.com/ar/wp-content/uploads/2024/12/019-galaxy-a55-5g-awesomelemon-front.jpg', 2),  -- otro smartphone
('https://www.hp.com/content/dam/sites/worldwide/laptops-and-2-in-1s/pavilion/HP%20Pavilion%20laptops%20-%20Desktop@2x.png', 5),  -- laptop
('https://techsite.ar/wp-content/uploads/2024/06/Asus-TUF-FX507VI-GAMING-Core-i7-13620H-1TB-SSD-32GB-15.622-FULL-HD-144Hz-IPS-WIN11-NVIDIA-RTX-4070-8GB-TECHSITE-2.jpg', 7),  -- dispositivo / gadget
('https://cdn.media.amplience.net/i/canon/eos-r10-frt_range_9feaf965e90e48d3a164d106d9939020', 12), -- cámara
('https://resource.logitech.com/c_fill,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/logitech/en/products/mice/mx-master-3s/2025-update/mx-master-3s-bluetooth-edition-top-view-graphite-new-1.png', 15);    -- accesorio / periférico

INSERT INTO Imagenes (UrlImagen, IdArticulo) VALUES
('https://resource.logitech.com/c_fill,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/logitech/en/products/mice/mx-master-3s/migration-assets-for-delorean-2025/gallery/mx-master-3s-top-view-pale-grey.png', 15);    -- accesorio / periférico
