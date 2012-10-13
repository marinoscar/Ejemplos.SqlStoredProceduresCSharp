IF OBJECT_ID('[dbo].[Persona]') IS NOT NULL
	DROP TABLE [dbo].[Persona]
GO

CREATE TABLE [dbo].[Persona](
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[Nombre] [varchar](50) NOT NULL,
	[Apellido1] [varchar](50) NOT NULL,
	[Apellido2] [varchar](50) NOT NULL,
	[CorreoElectronico] [varchar](50) NULL,
	[FechaUltimaModificacion] [datetime] NOT NULL
)
GO

IF OBJECT_ID('[dbo].[usp_AgregarPersona]') IS NOT NULL
	DROP PROCEDURE [dbo].[usp_AgregarPersona]
GO

CREATE PROCEDURE [dbo].[usp_AgregarPersona]
	@Id [int],
	@Nombre [varchar](50),
	@Apellido1 [varchar](50),
	@Apellido2 [varchar](50),
	@CorreoElectronico [varchar](50) = NULL

AS
BEGIN
	INSERT INTO [dbo].[Persona] ([Nombre], [Apellido1], [Apellido2], [CorreoElectronico], [FechaUltimaModificacion])
	VALUES (@Nombre, @Apellido1, @Apellido2, @CorreoElectronico, GETDATE())
	SELECT IDENT_CURRENT('[dbo].[Persona]')
END
GO

IF OBJECT_ID('[dbo].[usp_ModificarPersona]') IS NOT NULL
	DROP PROCEDURE [dbo].[usp_ModificarPersona]
GO

CREATE PROCEDURE [dbo].[usp_ModificarPersona]
	@Id [int],
	@Nombre [varchar](50),
	@Apellido1 [varchar](50),
	@Apellido2 [varchar](50),
	@CorreoElectronico [varchar](50) = NULL

AS
BEGIN
	UPDATE [dbo].[Persona] SET
		[FechaUltimaModificacion] = GETDATE(),
		[Nombre] = @Nombre,
		[Apellido1]=@Apellido1,
		[Apellido2]=@Apellido2,
		[CorreoElectronico]=@CorreoElectronico
	WHERE [Id] = @Id
END
GO

IF OBJECT_ID('[dbo].[usp_EliminarPersona]') IS NOT NULL
	DROP PROCEDURE [dbo].[usp_EliminarPersona]
GO

CREATE PROCEDURE [dbo].[usp_EliminarPersona]
	@Id [int]

AS
BEGIN
	DELETE FROM [dbo].[Persona] WHERE [Id] = @Id
END
GO

IF OBJECT_ID('[dbo].[usp_ListarPersonas]') IS NOT NULL
	DROP PROCEDURE [dbo].[usp_ListarPersonas]
GO

CREATE PROCEDURE [dbo].[usp_ListarPersonas]
	@Nombre [varchar](50) = NULL

AS
BEGIN
	SELECT
		 [Id]
		,[Nombre]
		,[Apellido1]
		,[Apellido2]
		,[CorreoElectronico]
		,[FechaUltimaModificacion]
	FROM
		[dbo].[Persona]
	WHERE
		[Nombre] LIKE ISNULL(@Nombre, [Nombre])
		
END
GO

IF OBJECT_ID('[dbo].[usp_TraerPersona]') IS NOT NULL
	DROP PROCEDURE [dbo].[usp_TraerPersona]
GO

CREATE PROCEDURE [dbo].[usp_TraerPersona]
	@Id [int]

AS
BEGIN
	SELECT
		 [Id]
		,[Nombre]
		,[Apellido1]
		,[Apellido2]
		,[CorreoElectronico]
		,[FechaUltimaModificacion]
	FROM
		[dbo].[Persona]
	WHERE
		[Id] = @Id
		
END
GO
