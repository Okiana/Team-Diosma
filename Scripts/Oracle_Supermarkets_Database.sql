-- Create tablespace
-- not obligatory part
create tablespace supermarkets_tabspace
datafile 'supermarkets_tabspace.dat'
size 50M autoextend on;

-- Create temporary tablespace
-- not obligatory part

create temporary tablespace supermarkets_tabspace_temp
tempfile 'supermarkets_tabspace_temp.dat'
size 10M autoextend on;

--Cretae User    [username] ... [password]
create user diosma IDENTIFIED BY diosma
default tablespace supermarkets_tabspace
temporary tablespace supermarkets_tabspace_temp;

--Grant privileges
grant connect, resource to diosma

--Assign SYSTEM privileges to new user in Oracle

GRANT create session TO diosma;
GRANT create table TO diosma;
GRANT create view TO diosma;
GRANT create any trigger TO diosma;
GRANT create any procedure TO diosma;
GRANT create sequence TO diosma;
GRANT create synonym TO diosma;
GRANT unlimited tablespace TO diosma;


--Change the default db schema
ALTER SESSION SET CURRENT_SCHEMA = diosma;

CREATE TABLE TOWNS
(
Id DECIMAL(10, 0) NOT NULL,
Name nvarchar2(255) NOT NULL CONSTRAINT TOWNNAME_UQ UNIQUE,
CONSTRAINT PK_TOWNS PRIMARY KEY (Id)
);

-- This makes auto-increment sequence for primary key in table "Towns"
CREATE SEQUENCE towns_sequence
START WITH 1
INCREMENT BY 1
MINVALUE 1
MAXVALUE 1000000;

CREATE TABLE SUPERMARKETS
(
Id DECIMAL(10, 0) NOT NULL,
Name nvarchar2(255) NOT NULL CONSTRAINT SUPERMARKET_UQ UNIQUE,
TownId DECIMAL(10, 0) NOT NULL,
CONSTRAINT PK_SUPERMARKETS PRIMARY KEY (Id)
);

CREATE SEQUENCE supermarkets_sequence
START WITH 1
INCREMENT BY 1
MINVALUE 1
MAXVALUE 1000000;

ALTER TABLE SUPERMARKETS ADD CONSTRAINT supermarkets_ts_fk FOREIGN KEY 
(TownId) REFERENCES TOWNS(Id);

CREATE TABLE MEASURES
(
Id DECIMAL(10, 0) NOT NULL,
Name nvarchar2(255) NOT NULL CONSTRAINT MEASURENAME_UQ UNIQUE,
CONSTRAINT PK_MEASURES PRIMARY KEY (Id)
);

-- This makes auto-increment sequence for primary key in table "Measures"
CREATE SEQUENCE measures_sequence
START WITH 1
INCREMENT BY 1
MINVALUE 1
MAXVALUE 1000000;

CREATE TABLE VENDORS
(
Id DECIMAL(10, 0) NOT NULL,
Name nvarchar2(255) NOT NULL CONSTRAINT VENDORNAME_UQ UNIQUE,
CONSTRAINT PK_VENDORS PRIMARY KEY (Id)
);

-- This makes auto-increment sequence for primary key in table "Vendors"
CREATE SEQUENCE vendors_sequence
START WITH 1
INCREMENT BY 1
MINVALUE 1
MAXVALUE 1000000;

CREATE TABLE PRODUCTSTYPES
(
Id DECIMAL(10, 0) NOT NULL,
Name nvarchar2(255) NOT NULL CONSTRAINT TYPENAME_UQ UNIQUE,
CONSTRAINT PK_PRODUCTSTYPES PRIMARY KEY (Id)
);

-- This makes auto-increment sequence for primary key in table "Productstypes"
CREATE SEQUENCE productstypes_sequence
START WITH 1
INCREMENT BY 1
MINVALUE 1
MAXVALUE 1000000;

CREATE TABLE PRODUCTS
(
Id DECIMAL(10, 0) NOT NULL ,
VendorId DECIMAL(10, 0) NOT NULL,
Name nvarchar2(255) NOT NULL,
MeasureId DECIMAL(10, 0) NOT NULL,
TypeId DECIMAL(10, 0) NOT NULL,
Price FLOAT NOT NULL,
CONSTRAINT PK_PRODUCTS PRIMARY KEY (Id)
);

-- This makes auto-increment sequence for primary key in table "Products"
CREATE SEQUENCE products_sequence
START WITH 1
INCREMENT BY 1
MINVALUE 1
MAXVALUE 1000000;

--Add foreign key constraints in table "Products"
ALTER TABLE PRODUCTS ADD CONSTRAINT products_ms_fk FOREIGN KEY 
(MeasureId) REFERENCES MEASURES(Id);

ALTER TABLE PRODUCTS ADD CONSTRAINT products_vd_fk FOREIGN KEY 
(VendorId) REFERENCES VENDORS(Id);

ALTER TABLE PRODUCTS ADD CONSTRAINT products_tps_fk FOREIGN KEY 
(TypeId) REFERENCES PRODUCTSTYPES(Id);

--Insert some data
insert into TOWNS(Id, Name)
values (towns_sequence.NEXTVAL,'Plovdiv');
insert into TOWNS(Id, Name)
values (towns_sequence.NEXTVAL,'Kaspichan');
insert into TOWNS(Id, Name)
values (towns_sequence.NEXTVAL,'Bourgas');
insert into TOWNS(Id, Name)
values (towns_sequence.NEXTVAL,'Zmeyovo');

insert into SUPERMARKETS(Id, Name, TownId)
values (supermarkets_sequence.NEXTVAL, 'Supermarket "Plovdiv - Stolipinovo"', 1);
insert into SUPERMARKETS(Id, Name, TownId)
values (supermarkets_sequence.NEXTVAL, 'Supermarket "Kaspichan - Center"', 2);
insert into SUPERMARKETS(Id, Name, TownId)
values (supermarkets_sequence.NEXTVAL, 'Supermarket "Bourgas - Plaza"', 3);
insert into SUPERMARKETS(Id, Name, TownId)
values (supermarkets_sequence.NEXTVAL, 'Supermarket "Bay Ivan" - Zmeyovo', 4);

insert into MEASURES(Id, Name)
values (measures_sequence.NEXTVAL,'liters');
insert into MEASURES(Id, Name)
values (measures_sequence.NEXTVAL,'pieces');
insert into MEASURES(Id, Name)
values (measures_sequence.NEXTVAL,'kg');

insert into VENDORS(Id, Name)
values (vendors_sequence.NEXTVAL,'Nestle Sofia Corp.');
insert into VENDORS(Id, Name)
values (vendors_sequence.NEXTVAL,'Zagorka Corp.');
insert into VENDORS(Id, Name)
values (vendors_sequence.NEXTVAL,'Kamenitza Ltd.');
insert into VENDORS(Id, Name)
values (vendors_sequence.NEXTVAL,'Targovishte Bottling Company Ltd.');
insert into VENDORS(Id, Name)
values (vendors_sequence.NEXTVAL,'Coca-Cola HBC AG');

insert into PRODUCTSTYPES(Id, Name)
values (productstypes_sequence.NEXTVAL,'Alcohol');
insert into PRODUCTSTYPES(Id, Name)
values (productstypes_sequence.NEXTVAL,'Sweets');
insert into PRODUCTSTYPES(Id, Name)
values (productstypes_sequence.NEXTVAL,'Non-alcohol');

insert into PRODUCTS(Id, VendorId, Name, MeasureId, TypeId, Price)
values (products_sequence.NEXTVAL, 4, 'Vodka "Targovishte"', 1, 1, 7.7);
insert into PRODUCTS(Id, VendorId, Name, MeasureId, TypeId, Price)
values (products_sequence.NEXTVAL, 3, 'Beer "Beck''s"', 1, 1, 1.05);
insert into PRODUCTS(Id, VendorId, Name, MeasureId, TypeId, Price)
values (products_sequence.NEXTVAL, 2, 'Beer "Zagorka"', 1, 1 , 0.88);
insert into PRODUCTS(Id, VendorId, Name, MeasureId, TypeId, Price)
values (products_sequence.NEXTVAL, 1, 'Chocolate "Milka"', 2, 2 , 2.9);

-- Test if everything is ok
select * from MEASURES;

select * from VENDORS;

select * from PRODUCTSTYPES;

select * from PRODUCTS;

select 
  p.Name AS "Product",
  p.Price AS "Price",
  v.Name AS "Vendor",
  pt.Name as "Type",
  m.Name as "Measure"
from products p
join VENDORS v
  on v.ID = p.VENDORID
join MEASURES m
  on m.ID = p.MEASUREID
join PRODUCTSTYPES pt
  on pt.ID = p.TYPEID;