CREATE TABLE TaxInformation(
       Id integer primary key autoincrement,
       ProductName nvarchar(50) not null unique,
       Tax integer not null);       

INSERT INTO TaxInformation(ProductName, Tax) VALUES
       ('Beer "Beck''s"',	20),       
       ('Beer "Zagorka"',	20),       
       ('Chocolate "Milka"',	18),       
       ('Vodka "Targovishte"',	25);