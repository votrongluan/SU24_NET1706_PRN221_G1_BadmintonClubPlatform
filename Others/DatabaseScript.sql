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
(N'Xuân Đông', N'23 đường Nguyễn Bình', 12, 'https://facebook.com/xuandong', 'https://i.pinimg.com/1200x/70/bb/1e/70bb1e22f39392a3c18f983749f79409.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441234', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Thu Hạ', N'25 đường Nguyễn Trãi', 14, 'https://facebook.com/thuha', 'https://i.pinimg.com/1200x/27/02/0e/27020ea41f73d5078ff4f0b71fcb04d5.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441234', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Hạ Chiu', N'27 đường Ngũ Lão', 22, 'https://facebook.com/hachiu', 'https://i.pinimg.com/1200x/e0/e6/30/e0e630de917d41d1952b8c446832ca36.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441234', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Đông Thu', N'84 đường Nguyễn Xỉn', 26, 'https://facebook.com/dongthu', 'https://i.pinimg.com/1200x/ad/e2/f0/ade2f0f4e6eda0ecc53eaf8bc31c3e86.jpg', '07:00:00.0000000', '22:00:00.0000000', 'xuandong@gmail.com', '0944441234', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 1', N'123 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club1', 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club1@gmail.com', '0944441001', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 2', N'456 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club2', 'https://i.pinimg.com/1200x/70/bb/1e/70bb1e22f39392a3c18f983749f79409.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club2@gmail.com', '0944441002', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 3', N'789 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club3',   'https://i.pinimg.com/1200x/82/ab/02/82ab02f56bfff75243e3f4353cf0b825.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club3@gmail.com', '0944441003', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 4', N'101 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club4', 'https://i.pinimg.com/1200x/ea/8d/9e/ea8d9eea77c060e5abbdd5ab35b66144.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club4@gmail.com', '0944441004', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 5', N'202 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club5', 'https://i.pinimg.com/1200x/80/8e/2e/808e2ed6e3eb6e9c5525614bf28d5deb.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club5@gmail.com', '0944441005', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 6', N'303 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club6', 'https://i.pinimg.com/1200x/e6/ee/1e/e6ee1e5a56e5f5dc7dd32fef9a9eb1c1.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club6@gmail.com', '0944441006', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 7', N'404 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club7', 'https://i.pinimg.com/1200x/23/1a/b1/231ab1ad7761163944343c96deed7b56.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club7@gmail.com', '0944441007', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 8', N'505 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club8', 'https://i.pinimg.com/1200x/4a/33/1c/4a331c1398d3d7a67ecba739d2e8bb43.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club8@gmail.com', '0944441008', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 9', N'606 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club9', 'https://i.pinimg.com/1200x/ae/5c/33/ae5c33441ef2a2b4ad4586604deb6628.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club9@gmail.com', '0944441009', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 10', N'707 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club10', 'https://i.pinimg.com/1200x/e6/79/a8/e679a85e758f70d3f4f779393561f284.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club10@gmail.com', '0944441010', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 11', N'808 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club11', 'https://i.pinimg.com/1200x/1d/09/b6/1d09b6860160e069e35f256a09a15f6d.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club11@gmail.com', '0944441011', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 12', N'909 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club12', 'https://i.pinimg.com/1200x/ef/21/a6/ef21a62ba9e47dd51e2a673204a550f2.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club12@gmail.com', '0944441012', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 13', N'111 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club13', 'https://i.pinimg.com/1200x/d4/0b/5f/d40b5f9e9805d7c59757b56be2b165d9.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club13@gmail.com', '0944441013', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 14', N'222 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club14', 'https://i.pinimg.com/1200x/44/c2/61/44c261775d876c4116168008d14979ad.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club14@gmail.com', '0944441014', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 15', N'333 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club15', 'https://i.pinimg.com/1200x/ea/a5/9d/eaa59d41d6eb05ff4cc8fee3d9b61ea2.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club15@gmail.com', '0944441015', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 16', N'444 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club16', 'https://i.pinimg.com/1200x/06/c3/ec/06c3ecd6a7f65b067f3d01d626386129.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club16@gmail.com', '0944441016', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 17', N'555 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club17', 'https://i.pinimg.com/1200x/17/58/21/1758213bf20493917f9fcb4daae025e7.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club17@gmail.com', '0944441017', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 18', N'666 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club18', 'https://i.pinimg.com/1200x/7a/4d/a1/7a4da175edbf7adcdf31ab69489173be.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club18@gmail.com', '0944441018', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 19', N'777 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club19', 'https://i.pinimg.com/1200x/6f/84/46/6f844691fc8fdc34a2eeeb82c169d2bb.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club19@gmail.com', '0944441019', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 20', N'888 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club20', 'https://i.pinimg.com/1200x/29/35/3a/29353a8081c366d11c4b671a4c0babf2.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club20@gmail.com', '0944441020', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 21', N'999 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club21', 'https://i.pinimg.com/1200x/07/9f/c1/079fc1f3c071d6ef3e38774bebf2eed0.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club21@gmail.com', '0944441021', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 22', N'1111 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club22', 'https://i.pinimg.com/1200x/99/fa/2b/99fa2bfc0e77625c68de12fc0f530675.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club22@gmail.com', '0944441022', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 23', N'1222 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club23', 'https://i.pinimg.com/1200x/1f/d9/cc/1fd9cc3bfcf04631970043487f439415.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club23@gmail.com', '0944441023', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 24', N'1333 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club24', 'https://i.pinimg.com/1200x/13/29/76/13297614ae9179db5b064de672338025.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club24@gmail.com', '0944441024', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 25', N'1444 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club25', 'https://i.pinimg.com/1200x/84/15/d3/8415d326965b5c71083dbd6e71bb1d7e.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club25@gmail.com', '0944441025', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 26', N'1555 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club26', 'https://i.pinimg.com/1200x/10/3e/5e/103e5ec0e037a8c6d3c34ddea6055aba.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club26@gmail.com', '0944441026', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 27', N'1666 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club27', 'https://i.pinimg.com/1200x/cc/13/0b/cc130b1a2ebe8807f2ee6a5a4786b106.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club27@gmail.com', '0944441027', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 28', N'1777 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club28', 'https://i.pinimg.com/1200x/c4/fe/44/c4fe44611251c341bc316baed1685d28.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club28@gmail.com', '0944441028', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 29', N'1888 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club29', 'https://i.pinimg.com/1200x/74/6f/43/746f43fc2b4afe48800ab5bac18d2280.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club29@gmail.com', '0944441029', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 30', N'1999 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club30', 'https://i.pinimg.com/1200x/9c/95/a4/9c95a45616c0c9072b501933cac63976.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club30@gmail.com', '0944441030', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 31', N'2101 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club31', 'https://i.pinimg.com/1200x/9b/67/6b/9b676bf8c964bb631625b9334d2e20c8.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club31@gmail.com', '0944441031', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 32', N'2202 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club32', 'https://i.pinimg.com/1200x/9e/32/78/9e3278c9988e3487dbfaf09a0f2438ee.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club32@gmail.com', '0944441032', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 33', N'2303 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club33', 'https://i.pinimg.com/1200x/22/77/54/22775473b3b622d38a4ad395a1b97282.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club33@gmail.com', '0944441033', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 34', N'2404 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club34', 'https://i.pinimg.com/1200x/21/fd/f2/21fdf271623ada7fae37e4f824685a25.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club34@gmail.com', '0944441034', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 35', N'2505 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club35', 'https://i.pinimg.com/1200x/77/00/3f/77003f8f55538243ae96d658ca0109dc.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club35@gmail.com', '0944441035', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 36', N'2606 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club36', 'https://i.pinimg.com/1200x/02/c4/c5/02c4c5fd9be830489a95de2ab5f1ac38.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club36@gmail.com', '0944441036', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 37', N'2707 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club37', 'https://i.pinimg.com/1200x/84/a0/dc/84a0dcd413fbdd5429e2f2657e450c88.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club37@gmail.com', '0944441037', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 38', N'2808 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club38', 'https://i.pinimg.com/1200x/9f/bd/89/9fbd893054ffc4c59f8ffdaeb075c1a2.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club38@gmail.com', '0944441038', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 39', N'2909 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club39', 'https://i.pinimg.com/1200x/bd/9f/99/bd9f9973f777f359ac55639c036aac1b.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club39@gmail.com', '0944441039', NULL, NULL, NULL, NULL, 0, 0, 20000),
(N'Club 40', N'3010 Club Street', FLOOR(RAND() * 62) + 1, 'https://facebook.com/club40', 'https://i.pinimg.com/1200x/1d/5f/b4/1d5fb40ef0fbba623903b55af4a90812.jpg', '07:00:00.0000000', '22:00:00.0000000', 'club40@gmail.com', '0944441040', NULL, NULL, NULL, NULL, 0, 0, 20000);

-- Insert data into AvailableBookingType table
INSERT INTO AvailableBookingType (ClubId, BookingTypeId)
VALUES 
(1, 2);

-- Insert data into Slot table
INSERT INTO Slot (StartTime, EndTime, Status, ClubId, Price)
VALUES 
('06:00:00.0000000', '07:00:00.0000000', NULL, 1, 12000),
('07:00:00.0000000', '08:00:00.0000000', NULL, 1, 13000),
('08:00:00.0000000', '09:00:00.0000000', NULL, 1, 14000),
('09:00:00.0000000', '10:00:00.0000000', NULL, 1, 15000),
('10:00:00.0000000', '11:00:00.0000000', NULL, 1, 16000),
('11:00:00.0000000', '12:00:00.0000000', NULL, 1, 17000),
('12:00:00.0000000', '13:00:00.0000000', NULL, 1, 18000),
('13:00:00.0000000', '14:00:00.0000000', NULL, 1, 19000),
('14:00:00.0000000', '15:00:00.0000000', NULL, 1, 20000),
('15:00:00.0000000', '16:00:00.0000000', NULL, 1, 21000),
('16:00:00.0000000', '17:00:00.0000000', NULL, 1, 22000),
('17:00:00.0000000', '18:00:00.0000000', NULL, 1, 23000),
('18:00:00.0000000', '19:00:00.0000000', NULL, 1, 24000),
('19:00:00.0000000', '20:00:00.0000000', NULL, 1, 25000);

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
('owner4', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 4),
('owner5', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 5),
('owner6', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 6),
('owner7', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 7),
('owner8', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 8),
('owner9', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 9),
('owner10', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 10),
('owner11', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 11),
('owner12', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 12),
('owner13', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 13),
('owner14', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 14),
('owner15', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 15),
('owner16', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 16),
('owner17', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 17),
('owner18', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 18),
('owner19', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 19),
('owner20', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 20),
('owner21', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 21),
('owner22', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 22),
('owner23', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 23),
('owner24', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 24),
('owner25', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 25),
('owner26', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 26),
('owner27', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 27),
('owner28', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 28),
('owner29', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 29),
('owner30', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 30),
('owner31', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 31),
('owner32', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 32),
('owner33', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 33),
('owner34', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 34),
('owner35', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 35),
('owner36', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 36),
('owner37', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 37),
('owner38', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 38),
('owner39', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 39),
('owner40', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 40),
('owner41', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 41),
('owner42', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 42),
('owner43', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 43),
('owner44', '12345', NULL, NULL, NULL, 'https://i.pinimg.com/1200x/2d/2a/bc/2d2abcceb7d190475adb3549f8f8d91e.jpg', 'Staff', 44);