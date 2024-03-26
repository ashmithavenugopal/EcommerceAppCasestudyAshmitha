create database Ecommerceapplication
------creating tables 
-----customers
CREATE TABLE customers (
    customer_id int PRIMARY KEY identity (1,1),
    customer_name VARCHAR(25),
    customer_email VARCHAR(25),
    customer_password VARCHAR(25)
);

-------products table
CREATE TABLE products (
    product_id INT PRIMARY KEY identity (101,1),
    prod_name VARCHAR(25),
    prod_price INT,
    prod_description VARCHAR(100),
    stock_Quantity INT
);

-------cart table
CREATE TABLE cart (
    cart_id INT PRIMARY KEY identity (1001,1),
    customer_id INT,
    product_id INT,
    quantity INT,
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id) on delete cascade,
    FOREIGN KEY (product_id) REFERENCES products(product_id) on delete cascade
);
-- Create orders table
CREATE TABLE orders (
    order_id INT PRIMARY KEY identity (2001,1),
    customer_id INT,
    order_date DATETIME,
    total_price DECIMAL(10, 2),
    shipping_address VARCHAR(250),
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id) on delete cascade 
);

-- Create order_items table
CREATE TABLE order_items (
    order_item_id INT PRIMARY KEY identity (201,1),
    order_id INT,
    product_id INT,
    quantity INT,
    FOREIGN KEY (order_id) REFERENCES orders(order_id) on delete cascade,
    FOREIGN KEY (product_id) REFERENCES products(product_id) on delete cascade
);

insert into customers(customer_name,customer_email,customer_password)
values 
('Akshay','akshay@gmail.com','passwordakshay'),
('Deekshita','deekshu@gmail.com','passworddeekshu'),
('Eesha','eesha@gmail.com','passwordeesha'),
('Vamika','vamika@gmail.com','passwordvamika'),
('Meer','meer@gmail.com','passwordmeer');

insert into products(prod_name,prod_price,prod_description,stock_Quantity) values 
('Samsung S24 Ultra',199000,'Latest smartphone mobile',5),
('Vaccum Cleaner',45000,'Makes your cleaning easy',8),
('Women kurti set',2499,'Bright colours to make your day bright',3),
('Baby Walker',4500,'First steps are precious',8),
('wireless mouse',699,'Connect from everywhere',12);

insert into cart(customer_id,product_id,quantity) values 
(1,101,1),
(2,105,3),
(3,104,4),
(4,102,2),
(5,103,1);

insert into orders(customer_id,order_date,total_price,shipping_address) values
(1, '2024-03-08 08:30:00', 1500.99, '1 Main Street, Chennai'),
(2, '2024-03-09 09:45:00', 799.50, '4 Oak Avenue, Gangtok'),
(3, '2024-03-10 10:15:00', 1249.75, '7 Pine Road, Vishakapatnam'),
(4, '2024-03-11 11:00:00', 750.25, '17 E Lane, Bhopal'),
(5, '2024-03-12 12:30:00', 1990.99, '56 Birla Street, Coimbatore');

insert into order_items(order_id,product_id,quantity) values
(2001,101,5),
(2002,102,3),
(2003,103,4),
(2004,104,2),
(2005,105,1);

------functionalities

-----1. createProduct()

insert into products(product_id,prod_name,prod_price,prod_description,stock_Quantity) values 
(106,'Tripod',1900,'Reduce your difficulties to shoot',10);

-----2. createCustomer()

insert into customers(customer_id,customer_name,customer_email,customer_password)
values 
(6,'Rahul','rahul@gmail.com','passwordrahul');

-----3.get order from customer

select order_id from Orders
where customer_id=1

----4. remove from cart

delete from Cart where product_id=104













