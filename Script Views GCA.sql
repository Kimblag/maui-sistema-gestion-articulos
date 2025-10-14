USE BDCatalogo;

GO

-- Definici�n de vista para listar art�culos
CREATE VIEW vw_Articulos
AS
	SELECT AR.IdArticulo, 
		   AR.Codigo, 
		   AR.Nombre,
		   AR.Descripcion,
		   AR.Precio,
		   AR.IdMarca,
		   MAR.Descripcion AS Marca,
		   AR.IdCategoria,
		   CAT.Descripcion AS Categoria 
		FROM Articulos AR
		JOIN Marcas MAR ON AR.IdMarca = MAR.IdMarca
		JOIN Categorias CAT ON AR.IdCategoria = CAT.IdCategoria
