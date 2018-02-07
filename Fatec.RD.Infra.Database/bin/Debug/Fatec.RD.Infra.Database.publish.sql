/*
Script de implantação para ProjetoViceri

Este código foi gerado por uma ferramenta.
As alterações feitas nesse arquivo poderão causar comportamento incorreto e serão perdidas se
o código for gerado novamente.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "ProjetoViceri"
:setvar DefaultFilePrefix "ProjetoViceri"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL13.SQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL13.SQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detecta o modo SQLCMD e desabilita a execução do script se o modo SQLCMD não tiver suporte.
Para reabilitar o script após habilitar o modo SQLCMD, execute o comando a seguir:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'O modo SQLCMD deve ser habilitado para executar esse script com êxito.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'A operação de refatoração Renomear com chave 244c646f-eba2-4438-998f-7ce9557b051e foi ignorada; o elemento [dbo].[RelatorioDespesa].[Id] (SqlSimpleColumn) não será renomeado para IdRelatorio';


GO
PRINT N'Criando [dbo].[Despesa]...';


GO
CREATE TABLE [dbo].[Despesa] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [IdTipoDespesa]   INT             NOT NULL,
    [IdTipoPagamento] INT             NOT NULL,
    [Data]            DATETIME        NOT NULL,
    [Valor]           DECIMAL (18, 2) NOT NULL,
    [Comentario]      VARCHAR (100)   NOT NULL,
    [DataCriacao]     DATETIME        NOT NULL,
    CONSTRAINT [PK_Despesa] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Criando [dbo].[Relatorio]...';


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER OFF;


GO
CREATE TABLE [dbo].[Relatorio] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [IdTipoRelatorio] INT           NOT NULL,
    [Descricao]       VARCHAR (100) NOT NULL,
    [Comentario]      VARCHAR (100) NOT NULL,
    [DataCriacao]     DATETIME      NOT NULL,
    CONSTRAINT [PK_Relatorio] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Criando [dbo].[RelatorioDespesa]...';


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER OFF;


GO
CREATE TABLE [dbo].[RelatorioDespesa] (
    [IdRelatorio] INT NOT NULL,
    [IdDespesa]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([IdDespesa] ASC, [IdRelatorio] ASC)
);


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Criando [dbo].[TipoDespesa]...';


GO
CREATE TABLE [dbo].[TipoDespesa] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Descricao]   VARCHAR (100) NOT NULL,
    [DataCriacao] DATETIME      NOT NULL,
    CONSTRAINT [PK_TipoDespesa] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Criando [dbo].[TipoPagamento]...';


GO
CREATE TABLE [dbo].[TipoPagamento] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Descricao]   VARCHAR (100) NOT NULL,
    [DataCriacao] DATETIME      NOT NULL,
    CONSTRAINT [PK_TipoPagamento] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Criando [dbo].[TipoRelatorio]...';


GO
CREATE TABLE [dbo].[TipoRelatorio] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Descricao]   VARCHAR (100) NOT NULL,
    [DataCriacao] DATETIME      NOT NULL,
    CONSTRAINT [PK_TipoRelatorio] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Criando [dbo].[FK_Despesa_TipoDespesa]...';


GO
ALTER TABLE [dbo].[Despesa] WITH NOCHECK
    ADD CONSTRAINT [FK_Despesa_TipoDespesa] FOREIGN KEY ([IdTipoDespesa]) REFERENCES [dbo].[TipoDespesa] ([Id]);


GO
PRINT N'Criando [dbo].[FK_Despesa_TipoPagamento]...';


GO
ALTER TABLE [dbo].[Despesa] WITH NOCHECK
    ADD CONSTRAINT [FK_Despesa_TipoPagamento] FOREIGN KEY ([IdTipoPagamento]) REFERENCES [dbo].[TipoPagamento] ([Id]);


GO
PRINT N'Criando [dbo].[FK_Relatorio_TipoRelatorio]...';


GO
ALTER TABLE [dbo].[Relatorio] WITH NOCHECK
    ADD CONSTRAINT [FK_Relatorio_TipoRelatorio] FOREIGN KEY ([IdTipoRelatorio]) REFERENCES [dbo].[TipoRelatorio] ([Id]);


GO
PRINT N'Criando [dbo].[FK_RelatorioDespesa_Relatorio]...';


GO
ALTER TABLE [dbo].[RelatorioDespesa] WITH NOCHECK
    ADD CONSTRAINT [FK_RelatorioDespesa_Relatorio] FOREIGN KEY ([IdRelatorio]) REFERENCES [dbo].[Relatorio] ([Id]);


GO
PRINT N'Criando [dbo].[FK_RelatorioDespesa_Despesa]...';


GO
ALTER TABLE [dbo].[RelatorioDespesa] WITH NOCHECK
    ADD CONSTRAINT [FK_RelatorioDespesa_Despesa] FOREIGN KEY ([IdDespesa]) REFERENCES [dbo].[Despesa] ([Id]);


GO
-- Etapa de refatoração para atualizar o servidor de destino com logs de transação implantados

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '244c646f-eba2-4438-998f-7ce9557b051e')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('244c646f-eba2-4438-998f-7ce9557b051e')

GO

GO
PRINT N'Verificando os dados existentes em restrições recém-criadas';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Despesa] WITH CHECK CHECK CONSTRAINT [FK_Despesa_TipoDespesa];

ALTER TABLE [dbo].[Despesa] WITH CHECK CHECK CONSTRAINT [FK_Despesa_TipoPagamento];

ALTER TABLE [dbo].[Relatorio] WITH CHECK CHECK CONSTRAINT [FK_Relatorio_TipoRelatorio];

ALTER TABLE [dbo].[RelatorioDespesa] WITH CHECK CHECK CONSTRAINT [FK_RelatorioDespesa_Relatorio];

ALTER TABLE [dbo].[RelatorioDespesa] WITH CHECK CHECK CONSTRAINT [FK_RelatorioDespesa_Despesa];


GO
PRINT N'Atualização concluída.';


GO
