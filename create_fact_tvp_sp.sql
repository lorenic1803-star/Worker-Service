USE AnalisisOpiniones_DW;
GO

-- =========================================================================
-- 1. PROCEDIMIENTO DE LIMPIEZA PREVIA DE TABLAS DE HECHOS (REQUISITO ETL)
-- =========================================================================
CREATE OR ALTER PROCEDURE [dbo].[CleanFactOpiniones]
AS
BEGIN
    SET NOCOUNT ON;
    TRUNCATE TABLE [dbo].[Fact_Opiniones];
END;
GO

-- =========================================================================
-- 2. TIPO DE TABLA DEFINIDO POR EL USUARIO (TVP) PARA CARGA MASIVA
-- =========================================================================
IF TYPE_ID(N'dbo.FactOpinionesType') IS NOT NULL
BEGIN
    DROP PROCEDURE IF EXISTS [dbo].[LoadFactOpiniones];
    DROP TYPE dbo.FactOpinionesType;
END
GO

CREATE TYPE dbo.FactOpinionesType AS TABLE
(
    IdOpinion INT NOT NULL,
    IdCliente INT NULL,
    IdProducto INT NOT NULL,
    IdFuente VARCHAR(10) NOT NULL,
    IdClasificacion INT NOT NULL,
    IdFecha INT NOT NULL,
    PuntajeSatisfaccionOriginal INT NULL,
    PuntajeNormalizado DECIMAL(5,2) NULL,
    Comentario VARCHAR(MAX) NULL,
    CantidadOpiniones INT NOT NULL
);
GO

-- =========================================================================
-- 3. PROCEDIMIENTO ALMACENADO DE CARGA MASIVA CON TVP Y PARÁMETROS OUTPUT
-- =========================================================================
CREATE OR ALTER PROCEDURE [dbo].[LoadFactOpiniones]
    @Opiniones dbo.FactOpinionesType READONLY,
    @Success BIT OUTPUT,
    @Message VARCHAR(500) OUTPUT,
    @RowsAffected INT OUTPUT,
    @ErrorCode INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        SET @Success = 0;
        SET @Message = '';
        SET @RowsAffected = 0;
        SET @ErrorCode = 0;

        BEGIN TRANSACTION;

        INSERT INTO [dbo].[Fact_Opiniones] (
            [IdOpinion],
            [IdCliente],
            [IdProducto],
            [IdFuente],
            [IdClasificacion],
            [IdFecha],
            [PuntajeSatisfaccionOriginal],
            [PuntajeNormalizado],
            [Comentario],
            [CantidadOpiniones]
        )
        SELECT
            op.[IdOpinion],
            op.[IdCliente],
            op.[IdProducto],
            op.[IdFuente],
            op.[IdClasificacion],
            op.[IdFecha],
            op.[PuntajeSatisfaccionOriginal],
            op.[PuntajeNormalizado],
            op.[Comentario],
            op.[CantidadOpiniones]
        FROM @Opiniones op
        WHERE NOT EXISTS (
            SELECT 1
            FROM [dbo].[Fact_Opiniones] f
            WHERE f.[IdOpinion] = op.[IdOpinion]
        );

        SET @RowsAffected = @@ROWCOUNT;

        COMMIT TRANSACTION;

        SET @Success = 1;
        SET @Message = 'Carga de Fact_Opiniones completada exitosamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Success = 0;
        SET @ErrorCode = ERROR_NUMBER();
        SET @Message = 'ERROR CARGANDO Fact_Opiniones: ' + ERROR_MESSAGE();
        SET @RowsAffected = 0;
    END CATCH
END;
GO

-- =========================================================================
-- 4. ÍNDICE NON-CLUSTERED PARA OPTIMIZAR LA VALIDACIÓN Y CONSULTAS
-- =========================================================================
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Fact_Opiniones_ValidacionDuplicados' AND object_id = OBJECT_ID('dbo.Fact_Opiniones'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Fact_Opiniones_ValidacionDuplicados
    ON [dbo].[Fact_Opiniones] (
        [IdFecha],
        [IdProducto],
        [IdCliente],
        [IdFuente],
        [IdClasificacion]
    );
END;
GO
