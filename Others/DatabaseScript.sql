-- Drop and create the database
USE master;
GO
DROP DATABASE IF EXISTS BCBP;
GO
CREATE DATABASE BCBP;
GO
USE BCBP;

-- Create User table
CREATE TABLE Account (
  UserId INT IDENTITY(1,1) NOT NULL,
  Username VARCHAR(MAX) NOT NULL, 
  Password VARCHAR(MAX) NOT NULL,
  Fullname NVARCHAR(MAX),
  Email VARCHAR(MAX),
  UserPhone VARCHAR(MAX),
  AvatarLink VARCHAR(MAX),
  Role VARCHAR(MAX),
  ClubManageId INT,
  PRIMARY KEY (UserId),
);

-- Create CourtType table
CREATE TABLE CourtType (
  CourtTypeId INT IDENTITY(1,1) NOT NULL,
  TypeName NVARCHAR(MAX) NOT NULL,
  TypeDescription NVARCHAR(MAX),
  PRIMARY KEY (CourtTypeId),
);

-- Create Booking table
CREATE TABLE Booking (
  BookingId INT IDENTITY(1,1) NOT NULL,
  UserId INT,
  ClubId INT NOT NULL,
  BookingTypeId INT NOT NULL,
  PaymentStatus BIT,
  TotalPrice INT,
  PRIMARY KEY (BookingId),
);

-- Create District table
CREATE TABLE District (
  DistrictId INT IDENTITY(1,1) NOT NULL,
  DistrictName NVARCHAR(MAX) NOT NULL,
  CityId INT NOT NULL,
  PRIMARY KEY (DistrictId),
);

-- Create City table
CREATE TABLE City (
  CityId INT IDENTITY(1,1) NOT NULL,
  CityName NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (CityId),
);

-- Create Club table
CREATE TABLE Club (
  ClubId INT IDENTITY(1,1) NOT NULL,
  ClubName NVARCHAR(MAX) NOT NULL,
  Address NVARCHAR(MAX) NOT NULL,
  DistrictId INT NOT NULL,
  FanpageLink VARCHAR(MAX),
  AvatarLink VARCHAR(MAX),
  OpenTime TIME,
  CloseTime TIME,
  ClubEmail VARCHAR(MAX),
  ClubPhone VARCHAR(MAX),
  ClientId VARCHAR(MAX),
  ApiKey VARCHAR(MAX),
  ChecksumKey VARCHAR(MAX),
  Status BIT,
  TotalStar INT,
  TotalReview INT,
  DefaultPricePerHour INT,
  PRIMARY KEY (ClubId),
);

-- Create Court table
CREATE TABLE Court (
  CourtId INT IDENTITY(1,1) NOT NULL,
  CourtTypeId INT NOT NULL,
  ClubId INT NOT NULL,
  Status BIT,
  PRIMARY KEY (CourtId),
);

-- Create BookingDetail table
CREATE TABLE BookingDetail (
  BookingDetailId INT IDENTITY(1,1) NOT NULL,
  BookingId INT NOT NULL,
  StartTime TIME,
  EndTIme TIME,
  CourtId INT NOT NULL,
  BookDate DATE,
  PRIMARY KEY (BookingDetailId)
);

-- Create Match table
CREATE TABLE Match (
  MatchId INT IDENTITY(1,1) NOT NULL,
  BookingId INT NOT NULL UNIQUE,
  Title NVARCHAR(MAX),
  Description NVARCHAR(MAX),
  PRIMARY KEY (MatchId)
);

-- Create Slot table
CREATE TABLE Slot (
  SlotId INT IDENTITY(1,1) NOT NULL,
  StartTime TIME,
  EndTime TIME,
  Status BIT,
  ClubId INT NOT NULL,
  Price INT,
  PRIMARY KEY (SlotId)
);

-- Create Review table
CREATE TABLE Review (
  ReviewId INT IDENTITY(1,1) NOT NULL,
  Star INT NOT NULL,
  Comment NVARCHAR(MAX),
  ClubId INT NOT NULL,
  UserId INT NOT NULL,
  ReviewDateTime DATETIME,
  PRIMARY KEY (ReviewId)
);

-- Create BookingType table
CREATE TABLE BookingType (
  BookingTypeId INT IDENTITY(1,1) NOT NULL,
  Description NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (BookingTypeId)
);

-- Create AvailableBookingType table
CREATE TABLE AvailableBookingType (
  AvailableBookingTypeId INT IDENTITY(1,1) NOT NULL,
  ClubId INT NOT NULL,
  BookingTypeId INT NOT NULL,
  PRIMARY KEY (AvailableBookingTypeId)
);

-- Define foreign keys

-- User
ALTER TABLE Account
ADD CONSTRAINT FK_User_ClubManageId FOREIGN KEY (ClubManageId) REFERENCES Club(ClubId);

-- Booking
ALTER TABLE Booking  
ADD CONSTRAINT FK_Booking_UserId FOREIGN KEY (UserId) REFERENCES Account(UserId);

ALTER TABLE Booking
ADD CONSTRAINT FK_Booking_ClubId FOREIGN KEY (ClubId) REFERENCES Club(ClubId);

ALTER TABLE Booking
ADD CONSTRAINT FK_Booking_BookingTypeId FOREIGN KEY (BookingTypeId) REFERENCES BookingType(BookingTypeId);

-- District
ALTER TABLE District
ADD CONSTRAINT FK_District_CityId FOREIGN KEY (CityId) REFERENCES City(CityId);

-- Club
ALTER TABLE Club
ADD CONSTRAINT FK_Club_DistrictId FOREIGN KEY (DistrictId) REFERENCES District(DistrictId);

-- Review
ALTER TABLE Review
ADD CONSTRAINT FK_Review_ClubId FOREIGN KEY (ClubId) REFERENCES Club(ClubId);

ALTER TABLE Review
ADD CONSTRAINT FK_Review_UserId FOREIGN KEY (UserId) REFERENCES Account(UserId);

-- Court 
ALTER TABLE Court
ADD CONSTRAINT FK_Court_CourtTypeId FOREIGN KEY (CourtTypeId) REFERENCES CourtType(CourtTypeId);

ALTER TABLE Court
ADD CONSTRAINT FK_Court_ClubId FOREIGN KEY (ClubId) REFERENCES Club(ClubId);

-- BookingDetail
ALTER TABLE BookingDetail
ADD CONSTRAINT FK_BookingDetail_BookingId FOREIGN KEY (BookingId) REFERENCES Booking(BookingId);

ALTER TABLE BookingDetail
ADD CONSTRAINT FK_BookingDetail_CourtId FOREIGN KEY (CourtId) REFERENCES Court(CourtId);

-- Match
ALTER TABLE Match
ADD CONSTRAINT FK_Match_BookingId FOREIGN KEY (BookingId) REFERENCES Booking(BookingId);

-- Slot
ALTER TABLE Slot
ADD CONSTRAINT FK_Slot_ClubId FOREIGN KEY (ClubId) REFERENCES Club(ClubId);

-- AvailableBookingType
ALTER TABLE AvailableBookingType
ADD CONSTRAINT FK_AvailableBookingType_ClubId FOREIGN KEY (ClubId) REFERENCES Club(ClubId);

ALTER TABLE AvailableBookingType
ADD CONSTRAINT FK_AvailableBookingType_BookingTypeId FOREIGN KEY (BookingTypeId) REFERENCES BookingType(BookingTypeId);

-- Insert data into BookingType table
INSERT INTO BookingType (Description)
VALUES 
(N'Lịch cố định'),
(N'Lịch ngày'),
(N'Lịch thi đấu');

-- Insert data into City table
INSERT INTO City (CityName)
VALUES 
(N'Hồ Chí Minh'),
(N'Hà Nội'),
(N'Đà Nẵng');

-- Insert data into District table
INSERT INTO District (DistrictName, CityId)
VALUES 
(N'Quận 1', 1),
(N'Quận 2', 1),
(N'Quận 3', 1),
(N'Quận 4', 1),
(N'Quận 5', 1),
(N'Quận 6', 1),
(N'Quận 7', 1),
(N'Quận 8', 1),
(N'Quận 9', 1),
(N'Quận 10', 1),
(N'Quận 11', 1),
(N'Quận 12', 1),
(N'Quận Bình Thạnh', 1),
(N'Quận Gò Vấp', 1),
(N'Quận Phú Nhuận', 1),
(N'Quận Tân Bình', 1),
(N'Quận Tân Phú', 1),
(N'Quận Bình Tân', 1),
(N'Thành phố Thủ Đức', 1),
(N'Huyện Nhà Bè', 1),
(N'Huyện Hóc Môn', 1),
(N'Huyện Bình Chánh', 1),
(N'Huyện Củ Chi', 1),
(N'Huyện Cần Giờ', 1),
(N'Quận Ba Đình', 2),
(N'Quận Cầu Giấy', 2),
(N'Quận Hoàn Kiếm', 2),
(N'Quận Hai Bà Trưng', 2),
(N'Quận Hoàng Mai', 2),
(N'Quận Đống Đa', 2),
(N'Quận Tây Hồ', 2),
(N'Quận Thanh Xuân', 2),
(N'Quận Bắc Từ Liêm', 2),
(N'Quận Hà Đông', 2),
(N'Quận Long Biên', 2),
(N'Quận Nam Từ Liêm', 2),
(N'Huyện Ba Vì', 2),
(N'Huyện Chương Mỹ', 2),
(N'Huyện Đan Phượng', 2),
(N'Huyện Đông Anh', 2),
(N'Huyện Gia Lâm', 2),
(N'Huyện Hoài Đức', 2),
(N'Huyện Mê Linh', 2),
(N'Huyện Mỹ Đức', 2),
(N'Huyện Phú Xuyên', 2),
(N'Huyện Phúc Thọ', 2),
(N'Huyện Quốc Oai', 2),
(N'Huyện Sóc Sơn', 2),
(N'Huyện Thạch Thất', 2),
(N'Huyện Thanh Oai', 2),
(N'Huyện Thanh Trì', 2),
(N'Huyện Thường Tín', 2),
(N'Huyện Ứng Hòa', 2),
(N'Thị xã Sơn Tây', 2),
(N'Quận Cẩm Lệ', 3),
(N'Quận Hải Châu', 3),
(N'Quận Liên Chiểu', 3),
(N'Quận Ngũ Hành Sơn', 3),
(N'Quận Sơn Trà', 3),
(N'Quận Thanh Khê', 3),
(N'Huyện Hòa Vang', 3),
(N'Huyện Hoàng Sa', 3);

-- Insert data into CourtType table
INSERT INTO CourtType (TypeName, TypeDescription)
VALUES 
(N'Sân đơn', N'Tổng chiều dài sân đấu: 13.40m. Chiều rộng sân, không tính hai đường bên: 5.18m. Đường chéo sân: 14.30m'),
(N'Sân đôi', N'Chiều rộng tối đa của sân cầu lông trong đánh đôi là 6');

-- Insert data into Club table
INSERT INTO Club (ClubName, Address, DistrictId, FanpageLink, AvatarLink, OpenTime, CloseTime, ClubEmail, ClubPhone, ClientId, ApiKey, ChecksumKey, Status, TotalStar, TotalReview, DefaultPricePerHour)
VALUES
(N'Xuân Đông', N'23 đường Nguyễn Bình', 12, 'https://facebook.com/xuandong', 'https://i.pinimg.com/1200x/70/bb/1e/70bb1e22f39392a3c18f983749f79409.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441231', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Thu Hạ', N'25 đường Nguyễn Trãi', 14, 'https://facebook.com/thuha', 'https://i.pinimg.com/1200x/27/02/0e/27020ea41f73d5078ff4f0b71fcb04d5.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441232', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Hạ Chiu', N'27 đường Ngũ Lão', 22, 'https://facebook.com/hachiu', 'https://i.pinimg.com/1200x/e0/e6/30/e0e630de917d41d1952b8c446832ca36.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441233', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Đông Thu', N'84 đường Nguyễn Xỉn', 26, 'https://facebook.com/dongthu', 'https://i.pinimg.com/1200x/ad/e2/f0/ade2f0f4e6eda0ecc53eaf8bc31c3e86.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441234', NULL, NULL, NULL, NULL, 0, 0, 20000);

-- Insert data into AvailableBookingType table
INSERT INTO AvailableBookingType (ClubId, BookingTypeId)
VALUES 
(1, 2),
(1, 1),
(2, 1),
(2, 2),
(3, 1),
(3, 2),
(4, 1),
(4, 2);

-- Insert data into Slot table
INSERT INTO Slot (StartTime, EndTime, Status, ClubId, Price)
VALUES 
('06:00:00.0000000', '07:00:00.0000000', NULL, 1, 12000),
('07:00:00.0000000', '08:00:00.0000000', NULL, 1, 13000);

-- Insert data into Court table
INSERT INTO Court (CourtTypeId, ClubId, Status)
VALUES 
(1, 1, NULL),
(1, 1, NULL),
(1, 1, NULL),
(1, 1, NULL),
(2, 1, NULL),
(2, 1, NULL),
(2, 1, NULL);

-- Insert data into Users table
INSERT INTO Account (Username, Password, Fullname, Email, UserPhone, AvatarLink, Role, ClubManageId)
VALUES
('admin', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Admin', NULL),
('owner1', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 1),
('user', '12345', N'Luân Võ Trọng', 'trongluan115@gmail.com', '0971781333', 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Customer', NULL),
('owner2', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 2),
('owner3', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 3),
('owner4', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 4);