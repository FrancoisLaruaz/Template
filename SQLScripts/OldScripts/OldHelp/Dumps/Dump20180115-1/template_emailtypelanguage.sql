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
-- Table structure for table `emailtypelanguage`
--

DROP TABLE IF EXISTS `emailtypelanguage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `emailtypelanguage` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EMailTypeId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Subject` varchar(255) NOT NULL,
  `TemplateName` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_emailtypelanguage_Language` (`LanguageId`),
  KEY `FK_emailtypelanguage_EMailType` (`EMailTypeId`),
  CONSTRAINT `FK_emailtypelanguage_EMailType` FOREIGN KEY (`EMailTypeId`) REFERENCES `category` (`Id`),
  CONSTRAINT `FK_emailtypelanguage_Language` FOREIGN KEY (`LanguageId`) REFERENCES `category` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emailtypelanguage`
--

LOCK TABLES `emailtypelanguage` WRITE;
/*!40000 ALTER TABLE `emailtypelanguage` DISABLE KEYS */;
INSERT INTO `emailtypelanguage` VALUES (1,1001,3001,'Reset your password','forgotpassword_en'),(2,1001,3002,'Réinitialisez votre mot de passe','forgotpassword_fr'),(3,1002,3001,'Welcome','userwelcome_en'),(4,1002,3002,'Bienvenue','userwelcome_fr');
/*!40000 ALTER TABLE `emailtypelanguage` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-01-15 10:34:54
