USE BDCatalogo;

GO

-- Como necesitamos pasar una lista de imágenes, tenemos que crear un Tipo de Tabla
CREATE TYPE dbo.TipoListaImagenes AS TABLE
(
	UrlImagen NVARCHAR(1000) NOT NULL
)

GO

-- Store Procedure para actualizar un artículo
CREATE PROCEDURE sp_Atualizar_Articulo
	(@idArticulo INT,
	 @Codigo NVARCHAR(50),
	 @Nombre NVARCHAR(50),
	 @Descripcion NVARCHAR(255),
	 @Precio DECIMAL(10, 2),
	 @IdMarca INT,
	 @IdCategoria INT,
	 @ListaImagenes TipoListaImagenes READONLY -- debe ser solo de lectura
	 )
AS
BEGIN
	BEGIN TRY

		-- Inicia transacción para evitar inconsistencias en el estado
		BEGIN TRANSACTION

		-- Actualiza el artículo
			UPDATE Articulos
			SET
				Codigo = @Codigo,
				Nombre = @Nombre,
				Descripcion = @Descripcion,
				Precio = @Precio,
				IdMarca = @IdMarca,
				IdCategoria = @IdCategoria
			WHERE IdArticulo = @idArticulo;

		-- Eliminar las imágenes viejas
		DELETE
			FROM Imagenes
		WHERE IdArticulo = @idArticulo
			AND UrlImagen NOT IN(SELECT UrlImagen FROM @ListaImagenes);

		-- Agregar imágenes nuevas
		INSERT INTO Imagenes (IdArticulo, UrlImagen)
		SELECT @idArticulo, li.UrlImagen
			FROM @ListaImagenes li
		WHERE li.UrlImagen NOT IN (SELECT UrlImagen FROM Imagenes WHERE IdArticulo = @IdArticulo)

		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		DECLARE @mensaje NVARCHAR(4000), @severidad INT, @estado INT;
		SELECT @mensaje = ERROR_MESSAGE(),
			   @severidad = ERROR_SEVERITY(),
			   @estado = ERROR_STATE();
		RAISERROR(@mensaje, @severidad, @estado);
	END CATCH
END;

GO


-- Store Procedure para insertar un nuevo artículo
CREATE PROCEDURE sp_Agregar_Articulo
	(
	 @Codigo NVARCHAR(50),
	 @Nombre NVARCHAR(50),
	 @Descripcion NVARCHAR(255),
	 @Precio DECIMAL(10, 2),
	 @IdMarca INT,
	 @IdCategoria INT,
	 @ListaImagenes TipoListaImagenes READONLY -- debe ser solo de lectura
	 )
AS
BEGIN
	BEGIN TRY

		-- Inicia transacción para evitar inconsistencias en el estado
		BEGIN TRANSACTION
			DECLARE @IdArticulo INT;
			-- Insertar el artículo
			INSERT INTO Articulos (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria)
				VALUES (@Codigo, @Nombre, @Descripcion, @Precio, @IdMarca, @IdCategoria);
			
			-- obtener el id del artículo
			SET @IdArticulo = SCOPE_IDENTITY();

			-- Agregar imágenes nuevas
			INSERT INTO Imagenes (IdArticulo, UrlImagen)
			SELECT @idArticulo, li.UrlImagen
				FROM @ListaImagenes li;

		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		DECLARE @mensaje NVARCHAR(4000), @severidad INT, @estado INT;
		SELECT @mensaje = ERROR_MESSAGE(),
			   @severidad = ERROR_SEVERITY(),
			   @estado = ERROR_STATE();
		RAISERROR(@mensaje, @severidad, @estado);
	END CATCH
END;

GO

-- Store Procedure para eliminar un artículo
CREATE PROCEDURE sp_Eliminar_Articulo
	(
	 @IdArticulo INT
	)
AS
BEGIN
	BEGIN TRY

		-- Inicia transacción para evitar inconsistencias en el estado
		BEGIN TRANSACTION
		
			-- Eliminar las imágenes asociadas
			DELETE FROM Imagenes WHERE IdArticulo = @IdArticulo;

			-- Eliminar el artículo
			DELETE FROM Articulos WHERE IdArticulo = @IdArticulo;

		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		DECLARE @mensaje NVARCHAR(4000), @severidad INT, @estado INT;
		SELECT @mensaje = ERROR_MESSAGE(),
			   @severidad = ERROR_SEVERITY(),
			   @estado = ERROR_STATE();
		RAISERROR(@mensaje, @severidad, @estado);
	END CATCH
END;

GO