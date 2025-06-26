CREATE DATABASE BagStoreDB;
USE BagStoreDB;

CREATE TABLE Customers (
    customer_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20),
    address VARCHAR(255)
);

CREATE TABLE Materials (
    material_id INT AUTO_INCREMENT PRIMARY KEY,
    material_name VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Categories (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Bags (
    bag_id INT AUTO_INCREMENT PRIMARY KEY,
    bag_name VARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    country_of_origin ENUM('Turkey', 'China', 'Italy') NOT NULL,
    material_id INT,
    gender ENUM('Women', 'Men') NOT NULL,
    category_id INT,
    FOREIGN KEY (material_id) REFERENCES Materials(material_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

CREATE TABLE Orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT,
    order_date DATE NOT NULL,
    total_amount DECIMAL(10, 2) NOT NULL,
    courier VARCHAR(50),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id)
);

CREATE TABLE Order_Bags (
    order_id INT,
    bag_id INT,
    quantity INT NOT NULL,
    PRIMARY KEY (order_id, bag_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (bag_id) REFERENCES Bags(bag_id)
);

INSERT INTO Materials (material_name) VALUES 
('Genuine Leather'), 
('Faux Leather'), 
('Canvas'), 
('Nylon'), 
('Polyester');

INSERT INTO Categories (category_name) VALUES 
('Shoulder Bag'), 
('Backpack'), 
('Wallet'), 
('Evening Bag'), 
('Office Bag'), 
('Crossbody Bag');

INSERT INTO Customers (first_name, last_name, email, phone, address) VALUES 
('Ivan', 'Petrov', 'ivan.petrov@example.com', '0888123456', 'Sofia, Bulgaria'),
('Maria', 'Ivanova', 'maria.ivanova@example.com', '0888765432', 'Plovdiv, Bulgaria'),
('Georgi', 'Dimitrov', 'georgi.dimitrov@example.com', '0878123456', 'Varna, Bulgaria'),
('Anna', 'Koleva', 'anna.koleva@example.com', '0888987654', 'Burgas, Bulgaria'),
('Peter', 'Stoyanov', 'peter.stoyanov@example.com', '0899123456', 'Ruse, Bulgaria'),
('Elena', 'Nikolova', 'elena.nikolova@example.com', '0888321456', 'Pleven, Bulgaria'),
('Nikolay', 'Hristov', 'nikolay.hristov@example.com', '0878456123', 'Stara Zagora, Bulgaria'),
('Viktoria', 'Georgieva', 'viktoria.georgieva@example.com', '0888543210', 'Blagoevgrad, Bulgaria'),
('Dimitar', 'Marinov', 'dimitar.marinov@example.com', '0899765432', 'Veliko Tarnovo, Bulgaria'),
('Yana', 'Todorova', 'yana.todorova@example.com', '0888671234', 'Dobrich, Bulgaria');

INSERT INTO Bags (bag_name, price, country_of_origin, material_id, gender, category_id) VALUES 
('Elegant Shoulder Bag', 120.50, 'Italy', 1, 'Women', 1),
('Classic Backpack', 85.00, 'Turkey', 3, 'Women', 2),
('Business Office Bag', 150.75, 'China', 4, 'Men', 5),
('Luxury Wallet', 45.90, 'Italy', 2, 'Women', 3),
('Chic Evening Bag', 200.00, 'Turkey', 1, 'Women', 4),
('Casual Crossbody Bag', 60.30, 'China', 5, 'Women', 6),
('Professional Office Bag', 170.45, 'Italy', 1, 'Men', 5),
('Sporty Backpack', 95.60, 'Turkey', 4, 'Women', 2),
('Compact Wallet', 30.00, 'China', 2, 'Women', 3),
('Trendy Shoulder Bag', 110.20, 'Italy', 3, 'Women', 1);

INSERT INTO Orders (customer_id, order_date, total_amount, courier) VALUES 
(1, '2024-02-01', 240.50, 'DHL'),
(2, '2024-02-03', 150.75, 'FedEx'),
(3, '2024-02-05', 120.00, 'Speedy'),
(4, '2024-02-07', 200.00, 'Econt'),
(5, '2024-02-09', 95.60, 'DHL'),
(6, '2024-02-11', 170.45, 'FedEx'),
(7, '2024-02-13', 60.30, 'Speedy'),
(8, '2024-02-15', 110.20, 'Econt'),
(9, '2024-02-17', 85.00, 'DHL'),
(10, '2024-02-19', 45.90, 'FedEx');

INSERT INTO Order_Bags (order_id, bag_id, quantity) VALUES 
(1, 1, 2),
(1, 4, 1),
(2, 3, 1),
(3, 2, 3),
(4, 5, 1),
(5, 8, 2),
(6, 7, 1),
(7, 6, 1),
(8, 10, 2),
(9, 9, 1),
(10, 4, 1),
(10, 3, 1);

ALTER TABLE Orders
ADD COLUMN delivery_status ENUM('In Progress', 'Delivered') NOT NULL DEFAULT 'In Progress';

UPDATE Orders
SET delivery_status = 'Delivered'
WHERE order_id = 1;

SELECT o.order_id, o.order_date, o.total_amount, o.delivery_status, c.first_name, c.last_name
FROM Orders o
JOIN Customers c ON o.customer_id = c.customer_id;

SELECT o.order_id, o.order_date, c.first_name, c.last_name, o.total_amount
FROM Orders o
JOIN Customers c ON o.customer_id = c.customer_id
WHERE o.delivery_status = 'In Progress';

SELECT o.order_id, o.order_date, c.first_name, c.last_name, o.total_amount
FROM Orders o
JOIN Customers c ON o.customer_id = c.customer_id
WHERE o.delivery_status = 'Delivered';

ALTER TABLE Bags
ADD COLUMN stock_quantity INT DEFAULT 0;

UPDATE Bags
SET stock_quantity = 50
WHERE bag_id = 1;

UPDATE Bags
SET stock_quantity = 30
WHERE bag_id = 2;

UPDATE Bags
SET stock_quantity = 25
WHERE bag_id = 3;

UPDATE Bags
SET stock_quantity = 40
WHERE bag_id = 4;

UPDATE Bags
SET stock_quantity = 15
WHERE bag_id = 5;

UPDATE Bags
SET stock_quantity = 20
WHERE bag_id = 6;

UPDATE Bags
SET stock_quantity = 35
WHERE bag_id = 7;

UPDATE Bags
SET stock_quantity = 45
WHERE bag_id = 8;

UPDATE Bags
SET stock_quantity = 60
WHERE bag_id = 9;

UPDATE Bags
SET stock_quantity = 50
WHERE bag_id = 10;
