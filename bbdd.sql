CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
	Email NVARCHAR(100) NOT NULL UNIQUE,
    Salt NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE Privileges (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL UNIQUE,
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE RefreshToken (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Token NVARCHAR(500) NOT NULL UNIQUE,
    Expires DATETIME2 NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE TABLE UsersPrivileges (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    PrivilegeId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (PrivilegeId) REFERENCES Privileges(Id) ON DELETE CASCADE,
    UNIQUE (UserId, PrivilegeId)
);

CREATE TABLE Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL,
	Description NVARCHAR(500) NOT NULL,
	Author NVARCHAR(50) NOT NULL,
	Category NVARCHAR(50) NOT NULL,
)

INSERT INTO Privileges (Name) VALUES
    ('BOOKS_LIST'),
    ('BOOKS_CREATE'),
    ('BOOKS_DELETE'),
    ('BOOKS_UPDATE'),
    ('USERS_LIST'),
    ('USERS_CREATE'),
    ('USERS_DELETE'),
    ('USERS_UPDATE');

INSERT INTO Books (Name, Description, Author, Category) VALUES
('Cien años de soledad', 'Una novela que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.', 'Gabriel García Márquez', 'Realismo Mágico'),
('Don Quijote de la Mancha', 'Relata las aventuras de un hidalgo que enloquece leyendo libros de caballerías y decide convertirse en caballero andante.', 'Miguel de Cervantes', 'Clásico'),
('La casa de los espíritus', 'Una saga familiar marcada por amores, tragedias y fantasmas que retrata la historia política y social de Chile.', 'Isabel Allende', 'Ficción Histórica'),
('Rayuela', 'Una novela revolucionaria que propone múltiples lecturas y explora la vida bohemia en París.', 'Julio Cortázar', 'Experimental'),
('El amor en los tiempos del cólera', 'Una historia de amor que perdura durante más de cincuenta años a pesar del tiempo y las circunstancias.', 'Gabriel García Márquez', 'Romance'),
('Pedro Páramo', 'Un hombre viaja a Comala en busca de su padre y se encuentra con un pueblo habitado por fantasmas.', 'Juan Rulfo', 'Realismo Mágico'),
('Fervor de Buenos Aires', 'Una colección de poemas que explora la identidad porteña y los paisajes urbanos.', 'Jorge Luis Borges', 'Poesía'),
('Sobre héroes y tumbas', 'Una novela oscura que mezcla historia, mito y locura en la Buenos Aires de mediados del siglo XX.', 'Ernesto Sabato', 'Ficción'),
('Los detectives salvajes', 'Relata la búsqueda de un misterioso poeta desaparecido a través de múltiples voces y testimonios.', 'Roberto Bolaño', 'Novela Contemporánea'),
('Como agua para chocolate', 'Un relato que mezcla recetas de cocina con la vida de una joven que expresa sus emociones a través de la comida.', 'Laura Esquivel', 'Romántica');

SELECT * FROM Users;
SET
IDENTITY_INSERT Users ON;
-- Password: 1234
INSERT INTO Users (Id, UserName, Email, Salt, PasswordHash, CreatedAt, UpdatedAt) VALUES
(1, 'Admin', 'admin@gmail.com', '7w2MLiaoWz5fGQk8CJdjdA==', 'fcd9a7c683231a99bbadb657964241d3a945c5a5ad69d1e2c17d9cf1ca68f0db', '2025-07-13 11:01:03.7866667', '2025-07-13 11:01:03.7866667');

SET IDENTITY_INSERT Users OFF;
