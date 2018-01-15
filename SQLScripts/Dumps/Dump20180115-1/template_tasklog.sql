CREATE DATABASE  IF NOT EXISTS `template` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `template`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: template
-- ------------------------------------------------------
-- Server version	5.7.20-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tasklog`
--

DROP TABLE IF EXISTS `tasklog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tasklog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime DEFAULT NULL,
  `Result` bit(1) DEFAULT NULL,
  `Comment` varchar(8000) DEFAULT NULL,
  `GroupName` varchar(100) DEFAULT NULL,
  `CallbackId` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=98 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tasklog`
--

LOCK TABLES `tasklog` WRITE;
/*!40000 ALTER TABLE `tasklog` DISABLE KEYS */;
INSERT INTO `tasklog` VALUES (91,'2018-01-11 19:27:00','2018-01-11 19:27:00','','N/A','DeleteLogs','65f3d5e8-84e3-431f-8d3f-0b65c5df0ae7'),(92,'2018-01-11 19:28:00','2018-01-11 19:28:00','','5 files analyzed : </br> - 0 files deleted </br> - 0 errors','DeleteUploadedFile','9059d8a1-f617-4617-992a-cd33a0380a42'),(93,'2018-01-11 19:38:00',NULL,NULL,NULL,'DeleteUploadedFile','7e3b4a04-6525-411e-aa42-d28881623e08'),(94,'2018-01-11 19:41:00',NULL,NULL,NULL,'DeleteUploadedFile','67681a48-c872-4a36-9245-bbfa887b876e'),(95,'2018-01-11 19:43:00','2018-01-11 19:44:18','','5 files analyzed : </br> - 1 files deleted </br> - 0 errors','DeleteUploadedFile','bb454b29-4bc5-4ff6-a2ee-e87377f39dea'),(96,'2018-01-14 19:39:00','2018-01-14 19:39:00','','N/A','DeleteLogs','1d526860-a5ca-4ea7-9055-a7e34c456bc6'),(97,'2018-01-14 19:43:00','2018-01-14 19:43:00','','8 files analyzed : </br> - 3 files deleted </br> - 0 errors','DeleteUploadedFile','8e22609a-0220-4a62-a557-a62004993964');
/*!40000 ALTER TABLE `tasklog` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-01-15 10:34:52
