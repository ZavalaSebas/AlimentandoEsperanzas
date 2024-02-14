/* Proyecto análisis */

CREATE DATABASE IF NOT EXISTS AlimentandoEsperanzas;
USE AlimentandoEsperanzas;

CREATE TABLE `Role` (
    `RoleID` int(15) NOT NULL AUTO_INCREMENT,
    `Role` varchar(50) NOT NULL,
    PRIMARY KEY (`RoleID`)
);

CREATE TABLE `IdType` (
    `ID` int(15) NOT NULL AUTO_INCREMENT,
    `Description` varchar(255) NOT NULL,
    PRIMARY KEY (`ID`)
);

CREATE TABLE `User` (
    `UserID` int(15) NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `LastName` varchar(50) NOT NULL,
    `Email` varchar(50) NOT NULL,
    `Password` varchar(255) NOT NULL, -- Añadida columna para almacenar la contraseña
    `IdNumber` varchar(30) NOT NULL,
    `IdentificationType` int(15) NOT NULL, -- FK
    `PhoneNumber` int(11) NOT NULL,
    `Date` datetime NOT NULL,
    `Role` int(15) NOT NULL,
    PRIMARY KEY (`UserID`),
    KEY `FK_USERS_ROLE` (`Role`),
    CONSTRAINT `FK_USERS_ROLE` FOREIGN KEY (`Role`) REFERENCES `Role` (`RoleID`),
    CONSTRAINT `FK_USER_TYPEID` FOREIGN KEY (`IdentificationType`) REFERENCES `IdType` (`ID`)
);

CREATE TABLE `UserRoles` (
    `UserRolesID` int(15) NOT NULL AUTO_INCREMENT,
    `RoleID` int(15),
    `UserID` int(15),
    PRIMARY KEY (`UserRolesID`),
    KEY `FK_USERS_USERROLE` (`UserID`),
    KEY `FK_ROLE_USERROLE` (`RoleID`),
    CONSTRAINT `FK_USERS_USERROLE` FOREIGN KEY (`UserID`) REFERENCES `User` (`UserID`),
    CONSTRAINT `FK_ROLE_USERROLE` FOREIGN KEY (`RoleID`) REFERENCES `Role` (`RoleID`)
);

CREATE TABLE `Donor` (
    `DonorID` int(15) NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `LastName` varchar(50) NOT NULL,
    `Email` varchar(50) NOT NULL,
    `IdNumber` varchar(30) NOT NULL,
    `IdentificationType` INT(15) NOT NULL, -- fk
    `PhoneNumber` int(11) NOT NULL,
    `Date` datetime NOT NULL,
    `Comments` varchar(100),
    PRIMARY KEY (`DonorID`),
    CONSTRAINT `FK_DONOR_TYPEID` FOREIGN KEY (`IdentificationType`) REFERENCES `IdType` (`ID`)
);

CREATE TABLE `DonationType` (
    `DonationTypeID` int(15) NOT NULL AUTO_INCREMENT,
    `DonationType` varchar(50) NOT NULL,
    PRIMARY KEY (`DonationTypeID`)
);

CREATE TABLE `PaymentMethod` (
    `PaymentMethodID` int(15) NOT NULL AUTO_INCREMENT,
    `PaymentMethod` varchar(50) NOT NULL,
    PRIMARY KEY (`PaymentMethodID`)
);

CREATE TABLE `Category` (
    `CategoryID` int(15) NOT NULL AUTO_INCREMENT,
    `Category` varchar(50) NOT NULL,
    PRIMARY KEY (`CategoryID`)
);

CREATE TABLE `Donation` (
    `DonationID` int(15) NOT NULL AUTO_INCREMENT,
    `DonorID` int(15) NOT NULL,
    `Amount` double NOT NULL,
    `DonationTypeID` int(15) NOT NULL,
    `Date` datetime NOT NULL,
    `PaymentMethodID` int(15) NOT NULL,
    `CategoryID` int(15) NOT NULL,
    `Comments` varchar(100),
    PRIMARY KEY (`DonationID`),
    KEY `FK_DONATION_DONOR` (`DonorID`),
    KEY `FK_DONATION_TYPE` (`DonationTypeID`),
    KEY `FK_DONATION_PAYMENT` (`PaymentMethodID`),
    KEY `FK_DONATION_CATEGORY` (`CategoryID`),
    CONSTRAINT `FK_DONATION_DONOR` FOREIGN KEY (`DonorID`) REFERENCES `Donor` (`DonorID`),
    CONSTRAINT `FK_DONATION_TYPE` FOREIGN KEY (`DonationTypeID`) REFERENCES `DonationType` (`DonationTypeID`),
    CONSTRAINT `FK_DONATION_PAYMENT` FOREIGN KEY (`PaymentMethodID`) REFERENCES `PaymentMethod` (`PaymentMethodID`),
    CONSTRAINT `FK_DONATION_CATEGORY` FOREIGN KEY (`CategoryID`) REFERENCES `Category` (`CategoryID`)
);

CREATE TABLE `ActionLog` (
    `ActionLogID` int(15) NOT NULL AUTO_INCREMENT,
    `Date` datetime NOT NULL,
    `Action` varchar(50) NOT NULL,
    `Document` varchar(100) NOT NULL,
    `UserID` int(15),
    PRIMARY KEY (`ActionLogID`),
    KEY `FK_ACTION_USERS` (`UserID`),
    CONSTRAINT `FK_ACTION_USERS` FOREIGN KEY (`UserID`) REFERENCES `User` (`UserID`)
);

CREATE TABLE `ErrorLog` (
    `ErrorLogID` int(15) NOT NULL AUTO_INCREMENT,
    `Date` datetime NOT NULL,
    `ErrorMessage` varchar(255) NOT NULL,
    `UserID` int(15),
    PRIMARY KEY (`ErrorLogID`),
    CONSTRAINT `FK_ERROR_USERS` FOREIGN KEY (`UserID`) REFERENCES `User` (`UserID`)
);

CREATE TABLE `ItemCategory` (
    `ID` int(15) NOT NULL AUTO_INCREMENT,
    `Description` varchar(255) NOT NULL,
    PRIMARY KEY (`ID`)
);

CREATE TABLE `item` (
    `ID` int(15) NOT NULL AUTO_INCREMENT,
    `Description` varchar(255) NOT NULL,
    `Quantity` int not null,
    `Category` int not null,
    PRIMARY KEY (`ID`),
    CONSTRAINT `FK_Category_Item` FOREIGN KEY (`Category`) REFERENCES `ItemCategory` (`ID`)
);

-- Inserts para la tabla Role
INSERT INTO Role (Role) VALUES ('Admin');
INSERT INTO Role (Role) VALUES ('Donor');
INSERT INTO Role (Role) VALUES ('User');

-- Inserts para la tabla IdType
INSERT INTO IdType (Description) VALUES ('DNI');
INSERT INTO IdType (Description) VALUES ('Passport');
INSERT INTO IdType (Description) VALUES ('Driver License');

-- Inserts para la tabla User
INSERT INTO User (Name, LastName, Email, Password, IdNumber, IdentificationType, PhoneNumber, Date, Role) 
VALUES ('John', 'Doe', 'john.doe@example.com', 'password123', '123456789', 1, 123456789, NOW(), 1);

-- Inserts para la tabla Donor
INSERT INTO Donor (Name, LastName, Email, IdNumber, IdentificationType, PhoneNumber, Date, Comments) 
VALUES ('Jane', 'Smith', 'jane.smith@example.com', '987654321', 2, 987654321, NOW(), 'Regular donor');

-- Inserts para la tabla DonationType
INSERT INTO DonationType (DonationType) VALUES ('Cash');
INSERT INTO DonationType (DonationType) VALUES ('Goods');
INSERT INTO DonationType (DonationType) VALUES ('Service');

-- Inserts para la tabla PaymentMethod
INSERT INTO PaymentMethod (PaymentMethod) VALUES ('Credit Card');
INSERT INTO PaymentMethod (PaymentMethod) VALUES ('Debit Card');
INSERT INTO PaymentMethod (PaymentMethod) VALUES ('PayPal');

-- Inserts para la tabla Category
INSERT INTO Category (Category) VALUES ('Food');
INSERT INTO Category (Category) VALUES ('Clothing');
INSERT INTO Category (Category) VALUES ('Medical Supplies');

-- Inserts para la tabla Donation
INSERT INTO Donation (DonorID, Amount, DonationTypeID, Date, PaymentMethodID, CategoryID, Comments) 
VALUES (1, 100.00, 1, NOW(), 1, 1, 'Monthly donation');

-- Inserts para la tabla ActionLog
INSERT INTO ActionLog (Date, Action, Document, UserID) 
VALUES (NOW(), 'Viewed profile', 'Profile of user John Doe', 1);

-- Inserts para la tabla ErrorLog
INSERT INTO ErrorLog (Date, ErrorMessage, UserID) 
VALUES (NOW(), 'Database connection error', 1);

-- Inserts para la tabla ItemCategory
INSERT INTO ItemCategory (Description) VALUES ('Electronics');
INSERT INTO ItemCategory (Description) VALUES ('Toys');
INSERT INTO ItemCategory (Description) VALUES ('Furniture');

-- Inserts para la tabla item
INSERT INTO item (Description, Quantity, Category) 
VALUES ('Laptop', 5, 1);
INSERT INTO item (Description, Quantity, Category) 
VALUES ('LEGO Set', 10, 2);
INSERT INTO item (Description, Quantity, Category) 
VALUES ('Sofa', 3, 3);
