-- Crear tabla de sampleusers
CREATE TABLE IF NOT EXISTS sampleusers (
    username VARCHAR(50) PRIMARY KEY,
    password VARCHAR(255) NOT NULL,
    dni VARCHAR(20) UNIQUE NOT NULL,
    name VARCHAR(100) NOT NULL,
    surnames VARCHAR(100) NOT NULL,
    age INT NOT NULL
);

-- Insertar datos iniciales
INSERT INTO sampleusers (username, password, dni, name, surnames, age) VALUES
('jperez', 'pass123', '11111111A', 'Juan', 'Perez', 25),
('mgomez', 'pass456', '22222222B', 'Maria', 'Gomez', 22),
('crodriguez', 'pass789', '33333333C', 'Carlos', 'Rodriguez', 30),
('amartinez', 'pass012', '44444444D', 'Ana', 'Martinez', 21),
('ltorres', 'pass345', '55555555E', 'Luis', 'Torres', 24);
