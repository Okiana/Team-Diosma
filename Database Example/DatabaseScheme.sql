CREATE DATABASE SupermarketsChainTest
GO

USE SupermarketsChainTest
CREATE TABLE Vendors
(
	Id INT PRIMARY KEY,
	Name VARCHAR(50)
)
GO

USE SupermarketsChainTest
CREATE TABLE Measures
(
	Id INT PRIMARY KEY,
	Name VARCHAR(50)
)
GO

USE SupermarketsChainTest
CREATE TABLE Products
(
	Id INT PRIMARY KEY,
	VendorId INT,
	Name VARCHAR (50),
	MeasureId INT,
	CONSTRAINT fk_ProductsVendors FOREIGN KEY (VendorId) REFERENCES Vendors(Id),
	CONSTRAINT fk_ProductsMeasures FOREIGN KEY (MeasureId) REFERENCES Measures(Id)
)
GO

USE SupermarketsChainTest
CREATE TABLE Stores
(
	Id INT PRIMARY KEY,
	Name VARCHAR(50)
)
GO

USE SupermarketsChainTest
CREATE TABLE ProductsStores
(
	ProductId INT,
	StoreId INT,
	Price MONEY,
	CONSTRAINT fk_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
	CONSTRAINT fk_Stores FOREIGN KEY (StoreId) REFERENCES Stores(Id),
	CONSTRAINT PK_ProductsMeasures PRIMARY KEY (StoreId, ProductId)
)
GO

USE SupermarketsChainTest
CREATE TABLE Sales
(
	Id INT PRIMARY KEY,
	ProductId INT,
	StoreId INT,
	Quantity INT,
	DateOfPurchase SMALLDATETIME,
	UnitPrice MONEY
	CONSTRAINT fk_SalesProducts FOREIGN KEY (ProductId) REFERENCES Products(Id),
	CONSTRAINT fk_SalesStores FOREIGN KEY (StoreId) REFERENCES Stores(Id),
)
GO

USE SupermarketsChainTest
CREATE TABLE VendorExpenses
(
	VendorId INT,
	[Month] SMALLDATETIME,
	Expences MONEY,
	CONSTRAINT PK_VendorMonth PRIMARY KEY (VendorId, [Month]),
	CONSTRAINT FK_VendorExpences FOREIGN KEY (VendorId) REFERENCES Vendors(Id)
)
GO