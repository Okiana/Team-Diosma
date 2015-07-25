DROP DATABASE IF EXISTS `marketsystem`;

CREATE DATABASE `marketsystem`
CHARACTER SET utf8 COLLATE utf8_unicode_ci;

USE `marketsystem`;

CREATE TABLE `towns` (
	id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL UNIQUE);

CREATE TABLE `supermarkets`(
	id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL UNIQUE,
    town_id INT NOT NULL,
    FOREIGN KEY(town_id) REFERENCES towns(id));

CREATE TABLE `measures` (
    id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL UNIQUE);    

CREATE TABLE `vendors`(
	id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL UNIQUE);
    
CREATE TABLE `products_types`(
	id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL UNIQUE);
    
CREATE TABLE `products`(
	id INT PRIMARY KEY,
    name NVARCHAR(200) NOT NULL UNIQUE,
    vendor_id INT NOT NULL,
    measure_id INT NOT NULL,
    type_id INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY(vendor_id) REFERENCES vendors(id),
	FOREIGN KEY(measure_id) REFERENCES measures(id),
	FOREIGN KEY(type_id) REFERENCES products_types(id));
    
CREATE TABLE `sales`(
	id INT PRIMARY KEY,
    date datetime NOT NULL,
    supermarket_id INT NOT NULL ,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    total_sum DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY(supermarket_id) REFERENCES supermarkets(id),
    FOREIGN KEY(product_id) REFERENCES products(id));
    
CREATE TABLE `vendor_expenses` (
	vendor_id INT NOT NULL,
    month datetime NOT NULL,
    expenses DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (vendor_id, month),
    FOREIGN KEY(vendor_id) REFERENCES vendors(id));
 
DELETE FROM vendor_expenses;
DELETE FROM sales;
DELETE FROM products;
DELETE FROM products_types;
DELETE FROM measures;
DELETE FROM vendors;
DELETE FROM supermarkets;
DELETE FROM towns;
 
SELECT * FROM towns;
SELECT * FROM supermarkets;
SELECT * FROM vendors;
SELECT * FROM measures;
SELECT * FROM products_types;
SELECT * FROM products;
SELECT * FROM sales;
SELECT * FROM vendor_expenses;