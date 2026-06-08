-- 1. Tabla de Categorías (sin dependencias externas)
CREATE TABLE Categorias (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    descripcion TEXT,
    activo BOOLEAN DEFAULT TRUE
);

-- 2. Tabla de Usuarios (independiente)
CREATE TABLE Usuarios (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    rol VARCHAR(50) DEFAULT 'Estudiante',
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 3. Tabla de Productos con Llave Foránea
CREATE TABLE Productos (
    id SERIAL PRIMARY KEY,
    categoria_id INT NOT NULL,
    nombre VARCHAR(150) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(10, 2) NOT NULL CHECK (precio >= 0),
    stock INT NOT NULL DEFAULT 0 CHECK (stock >= 0),
    sku VARCHAR(50) UNIQUE,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_categoria FOREIGN KEY (categoria_id) REFERENCES Categorias(id) ON DELETE CASCADE
);

-- Paso 1: Insertar Categorías base
INSERT INTO Categorias (nombre, descripcion)
VALUES
('Tecnología Verde', 'Dispositivos de bajo consumo y soluciones IoT sustentables.'),
('Energía Renovable', 'Paneles solares, inversores y almacenamiento de energía.');

-- Paso 2: Insertar Usuario Administrador
INSERT INTO Usuarios (nombre, email, password_hash, rol)
VALUES
('Coordinador EcoSystem', 'admin@ecosystem.com', 'AQAAAAIAAYagAAAAEG...', 'Administrador');

-- Paso 3: Insertar Producto vinculado a Categoria 1
INSERT INTO Productos (categoria_id, nombre, descripcion, precio, stock, sku)
VALUES
(1, 'Sensor IoT Humedad v2', 'Sensor de bajo consumo para monitoreo de suelos agrícolas.', 45.99, 120, 'ECO-IOT-HUM-02');

SELECT * FROM Productos;

SELECT p.nombre, p.precio, c.nombre AS categoria
FROM Productos p
JOIN Categorias c ON p.categoria_id = c.id;