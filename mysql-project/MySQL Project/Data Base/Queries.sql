USE BagStoreDB;

-- List with all categories and the bags in them
SELECT c.category_name, b.bag_name
FROM Categories c
JOIN Bags b ON c.category_id = b.category_id
ORDER BY c.category_name, b.bag_name;

-- List with all materials and the bags made from that material
SELECT m.material_name, b.bag_name
FROM Materials m
JOIN Bags b ON m.material_id = b.material_id
ORDER BY m.material_name, b.bag_name;

-- How many types of bags are available for each gender 
SELECT gender, COUNT(*) AS total_bags
FROM Bags
GROUP BY gender;

-- List of all customers and their orders
SELECT c.customer_id, c.first_name, c.last_name, c.email, o.order_id, o.order_date, o.total_amount, o.delivery_status
FROM Customers c
JOIN Orders o ON c.customer_id = o.customer_id;

-- Prices on all bags in a descending order
SELECT bag_name, price
FROM Bags
ORDER BY price DESC;

-- Find the most expensive bag in each category
SELECT c.category_name, b.bag_name, b.price
FROM Bags b
JOIN Categories c ON b.category_id = c.category_id
WHERE b.price = (
    SELECT MAX(price)
    FROM Bags
    WHERE category_id = b.category_id
)
ORDER BY b.price DESC;


-- Total orders number
SELECT COUNT(order_id) AS total_orders
FROM Orders;

-- Check the stock for all bags 
SELECT bag_name, stock_quantity
FROM Bags;

-- Find the best selling bag 
SELECT Bags.bag_name, SUM(Order_Bags.quantity) AS total_sold
FROM Order_Bags
JOIN Bags ON Order_Bags.bag_id = Bags.bag_id
GROUP BY Bags.bag_name
ORDER BY total_sold DESC
LIMIT 1;

-- Find my revenue 
SELECT SUM(Bags.price * Order_Bags.quantity) AS total_revenue
FROM Order_Bags
JOIN Bags ON Order_Bags.bag_id = Bags.bag_id;

-- Check the remaning stock
SELECT 
    b.bag_name,
    b.stock_quantity - SUM(ob.quantity) AS remaining_stock
FROM Bags b
JOIN Order_Bags ob ON b.bag_id = ob.bag_id
GROUP BY b.bag_id, b.bag_name, b.stock_quantity;

-- Find me how many bags were sold 
SELECT SUM(quantity) AS total_bags_sold
FROM Order_Bags;
