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
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `category` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Code` varchar(25) DEFAULT NULL,
  `Order` int(11) NOT NULL DEFAULT '99999999',
  `Active` bit(1) NOT NULL DEFAULT b'1',
  `CategoryTypeId` int(11) NOT NULL,
  `DateModification` datetime NOT NULL,
  `Field1` varchar(2000) DEFAULT NULL,
  `Field2` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_category_categorytype` (`CategoryTypeId`),
  CONSTRAINT `fk_category_categorytype` FOREIGN KEY (`CategoryTypeId`) REFERENCES `categorytype` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `category`
--

LOCK TABLES `category` WRITE;
/*!40000 ALTER TABLE `category` DISABLE KEYS */;
INSERT INTO `category` VALUES (1001,'forgotpassword','forgotpassword',1001,'',1,'2017-11-07 20:22:55',NULL,NULL),(1002,'userwelcome',NULL,99999999,'',1,'2017-11-30 16:01:18',NULL,NULL),(2001,'Admin','Admin',2001,'',2,'2017-11-07 20:22:55',NULL,NULL),(3001,'English','en',1,'',3,'2017-11-13 09:33:58','_EndMail_en',NULL),(3002,'French','fr',2,'',3,'2017-11-13 09:34:19','_EndMail_fr',NULL),(4001,'ErrorCleanUp','ErrorCleanUp',99999999,'',1,'2017-11-18 09:33:12',NULL,NULL),(4002,'UploadFilesCleanUp',NULL,99999999,'',1,'2017-12-04 21:07:45',NULL,NULL),(4003,'CancelledScheduledTasksCleanUp',NULL,99999999,'',1,'2017-12-17 10:04:29',NULL,NULL),(5001,'Publish',NULL,1,'',5,'2017-12-13 19:14:38',NULL,NULL),(5002,'Publish & Mail',NULL,2,'',5,'2017-12-13 19:14:39',NULL,NULL),(5003,'Mail only',NULL,3,'',5,'2017-12-15 09:39:27',NULL,NULL),(6001,'All users',NULL,1,'',6,'2017-12-13 19:14:39',NULL,NULL),(6002,'Confirmed Users',NULL,2,'',6,'2017-12-13 19:14:39',NULL,NULL);
/*!40000 ALTER TABLE `category` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-01-15 10:34:53
