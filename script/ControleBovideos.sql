CREATE DATABASE controleBovideos
GO

USE controleBovideos
GO

CREATE TABLE [especie_bovideo] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	descricao varchar(255) NOT NULL
)
GO
CREATE TABLE [finalidade_venda] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	descricao varchar(255) NOT NULL
)
GO
CREATE TABLE [vacina] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	nome varchar(255) NOT NULL
)
GO

CREATE TABLE [municipio] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	nome varchar(255) NOT NULL
)
GO

CREATE TABLE [endereco] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	id_municipio int NOT NULL REFERENCES municipio(id),
	rua varchar(255) NOT NULL,
	numero varchar(255) NOT NULL
)
GO

CREATE TABLE [produtor] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	id_endereco int NOT NULL REFERENCES endereco(id),
	nome varchar(255) NOT NULL,
	cpf varchar(255) NOT NULL
)
GO

CREATE TABLE [propriedade] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	id_produtor int NOT NULL REFERENCES produtor(id),
	id_municipio int NOT NULL REFERENCES municipio(id),
	inscricao_estadual varchar(255) NOT NULL,
	nome_propriedade varchar(255) NOT NULL
)
GO
CREATE TABLE [rebanho] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	id_especie int NOT NULL REFERENCES especie_bovideo(id),
	id_propriedade int NOT NULL REFERENCES propriedade(id),
	qtde_total int NOT NULL,
	qtde_vacinado_aftosa int NOT NULL,
	qtde_vacinado_brucelose int NOT NULL
)
GO
CREATE TABLE [registro_vacina] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	id_rebanho int NOT NULL REFERENCES rebanho(id),
	id_vacina int NOT NULL REFERENCES vacina(id),
	[data] datetime NOT NULL,
	qtde_vacinado int NOT NULL

)
GO
CREATE TABLE [venda] (
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	id_propriedade_origem int NOT NULL REFERENCES propriedade(id),
	id_propriedade_destino int NOT NULL REFERENCES propriedade(id),
	id_especie int NOT NULL REFERENCES especie_bovideo(id),
	id_finalidade_venda int NOT NULL REFERENCES finalidade_venda(id),
	qtde_vendida int NOT NULL,
	[data] datetime NOT NULL
)
GO
