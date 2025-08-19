-- Script para criar a tabela produto no banco PostgreSQL TOCC8
-- Execute este script no PostgreSQL para criar a tabela

CREATE TABLE IF NOT EXISTS produto (
    codigo SERIAL PRIMARY KEY,
    descricao VARCHAR(100),
    datavalidade DATE,
    preco FLOAT,
    taxalucro FLOAT
);

-- Inserir alguns dados de exemplo
INSERT INTO produto (descricao, datavalidade, preco, taxalucro) VALUES
('Arroz Integral', '2024-12-31', 8.50, 15.0),
('Feijão Preto', '2024-10-15', 6.80, 12.5),
('Macarrão Espaguete', '2024-08-20', 4.20, 20.0),
('Óleo de Soja', '2024-06-30', 7.90, 18.0),
('Leite Integral', '2024-03-15', 3.50, 25.0),
('Pão Francês', '2024-02-10', 0.50, 30.0),
('Queijo Minas', '2024-04-25', 12.80, 22.0),
('Presunto', '2024-05-12', 15.60, 28.0),
('Banana Prata', '2024-01-20', 4.90, 35.0),
('Maçã Fuji', '2024-02-28', 8.70, 40.0);
