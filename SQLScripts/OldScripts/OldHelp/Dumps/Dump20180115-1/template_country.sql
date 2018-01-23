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
-- Table structure for table `country`
--

DROP TABLE IF EXISTS `country`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `country` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Code` varchar(255) DEFAULT NULL,
  `Order` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=231 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `country`
--

LOCK TABLES `country` WRITE;
/*!40000 ALTER TABLE `country` DISABLE KEYS */;
INSERT INTO `country` VALUES (1,'Andorra','AD',2),(2,'United Arab Emirates','AE',3),(3,'Afghanistan','AF',4),(4,'Antigua and Barbuda','AG',5),(5,'Albania','AL',6),(6,'Armenia','AM',7),(7,'Angola','AO',8),(8,'Antarctica','AQ',9),(9,'Argentina','AR',10),(10,'American Samoa','AS',11),(11,'Austria','AT',12),(12,'Australia','AU',13),(13,'Aruba','AW',14),(14,'Aland Islands','AX',15),(15,'Azerbaijan','AZ',16),(16,'Bosnia and Herzegovina','BA',17),(17,'Barbados','BB',18),(18,'Bangladesh','BD',19),(19,'Belgium','BE',20),(20,'Burkina Faso','BF',21),(21,'Bulgaria','BG',22),(22,'Bahrain','BH',23),(23,'Burundi','BI',24),(24,'Benin','BJ',25),(25,'Bermuda','BM',26),(26,'Brunei','BN',27),(27,'Bolivia','BO',28),(28,'Bonaire, Saint Eustatius and Saba ','BQ',29),(29,'Brazil','BR',30),(30,'Bahamas','BS',31),(31,'Bhutan','BT',32),(32,'Bouvet Island','BV',33),(33,'Botswana','BW',34),(34,'Belarus','BY',35),(35,'Belize','BZ',36),(36,'Canada','CA',1),(37,'Democratic Republic of the Congo','CD',37),(38,'Central African Republic','CF',38),(39,'Congo','CG',39),(40,'Switzerland','CH',40),(41,'Ivory Coast','CI',41),(42,'Chile','CL',42),(43,'Cameroon','CM',43),(44,'China','CN',44),(45,'Colombia','CO',45),(46,'Costa Rica','CR',46),(47,'Cuba','CU',47),(48,'Cape Verde','CV',48),(49,'Cyprus','CY',49),(50,'Czech Republic','CZ',50),(51,'Germany','DE',51),(52,'Djibouti','DJ',52),(53,'Denmark','DK',53),(54,'Dominica','DM',54),(55,'Dominican Republic','DO',55),(56,'Algeria','DZ',56),(57,'Ecuador','EC',57),(58,'Estonia','EE',58),(59,'Egypt','EG',59),(60,'Western Sahara','EH',60),(61,'Eritrea','ER',61),(62,'Spain','ES',62),(63,'Ethiopia','ET',63),(64,'Finland','FI',64),(65,'Fiji','FJ',65),(66,'Micronesia','FM',66),(67,'Faroe Islands','FO',67),(68,'France','FR',68),(69,'Gabon','GA',69),(70,'United Kingdom','GB',70),(71,'Grenada','GD',71),(72,'Georgia','GE',72),(73,'French Guiana','GF',73),(74,'Guernsey','GG',74),(75,'Ghana','GH',75),(76,'Greenland','GL',76),(77,'Gambia','GM',77),(78,'Guinea','GN',78),(79,'Guadeloupe','GP',79),(80,'Equatorial Guinea','GQ',80),(81,'Greece','GR',81),(82,'Guatemala','GT',82),(83,'Guam','GU',83),(84,'Guinea-Bissau','GW',84),(85,'Guyana','GY',85),(86,'Hong Kong','HK',86),(87,'Honduras','HN',87),(88,'Croatia','HR',88),(89,'Haiti','HT',89),(90,'Hungary','HU',90),(91,'Indonesia','ID',91),(92,'Ireland','IE',92),(93,'Israel','IL',93),(94,'Isle of Man','IM',94),(95,'India','IN',95),(96,'British Indian Ocean Territory','IO',96),(97,'Iraq','IQ',97),(98,'Iran','IR',98),(99,'Iceland','IS',99),(100,'Italy','IT',100),(101,'Jersey','JE',101),(102,'Jamaica','JM',102),(103,'Jordan','JO',103),(104,'Japan','JP',104),(105,'Kenya','KE',105),(106,'Kyrgyzstan','KG',106),(107,'Cambodia','KH',107),(108,'Kiribati','KI',108),(109,'Comoros','KM',109),(110,'Saint Kitts and Nevis','KN',110),(111,'North Korea','KP',111),(112,'South Korea','KR',112),(113,'Kuwait','KW',113),(114,'Kazakhstan','KZ',114),(115,'Laos','LA',115),(116,'Lebanon','LB',116),(117,'Saint Lucia','LC',117),(118,'Liechtenstein','LI',118),(119,'Sri Lanka','LK',119),(120,'Liberia','LR',120),(121,'Lesotho','LS',121),(122,'Lithuania','LT',122),(123,'Luxembourg','LU',123),(124,'Latvia','LV',124),(125,'Libya','LY',125),(126,'Morocco','MA',126),(127,'Monaco','MC',127),(128,'Moldova','MD',128),(129,'Montenegro','ME',129),(130,'Madagascar','MG',130),(131,'Marshall Islands','MH',131),(132,'Macedonia','MK',132),(133,'Mali','ML',133),(134,'Myanmar','MM',134),(135,'Mongolia','MN',135),(136,'Macao','MO',136),(137,'Northern Mariana Islands','MP',137),(138,'Martinique','MQ',138),(139,'Mauritania','MR',139),(140,'Montserrat','MS',140),(141,'Mauritius','MU',141),(142,'Maldives','MV',142),(143,'Malawi','MW',143),(144,'Mexico','MX',144),(145,'Malaysia','MY',145),(146,'Mozambique','MZ',146),(147,'Namibia','NA',147),(148,'New Caledonia','NC',148),(149,'Niger','NE',149),(150,'Nigeria','NG',150),(151,'Nicaragua','NI',151),(152,'Netherlands','NL',152),(153,'Norway','NO',153),(154,'Nepal','NP',154),(155,'Nauru','NR',155),(156,'New Zealand','NZ',156),(157,'Oman','OM',157),(158,'Panama','PA',158),(159,'Peru','PE',159),(160,'French Polynesia','PF',160),(161,'Papua New Guinea','PG',161),(162,'Philippines','PH',162),(163,'Pakistan','PK',163),(164,'Poland','PL',164),(165,'Saint Pierre and Miquelon','PM',165),(166,'Puerto Rico','PR',166),(167,'Palestinian Territory','PS',167),(168,'Portugal','PT',168),(169,'Palau','PW',169),(170,'Paraguay','PY',170),(171,'Qatar','QA',171),(172,'Reunion','RE',172),(173,'Romania','RO',173),(174,'Serbia','RS',174),(175,'Russia','RU',175),(176,'Rwanda','RW',176),(177,'Saudi Arabia','SA',177),(178,'Solomon Islands','SB',178),(179,'Seychelles','SC',179),(180,'Sudan','SD',180),(181,'Sweden','SE',181),(182,'Singapore','SG',182),(183,'Saint Helena','SH',183),(184,'Slovenia','SI',184),(185,'Svalbard and Jan Mayen','SJ',185),(186,'Slovakia','SK',186),(187,'Sierra Leone','SL',187),(188,'San Marino','SM',188),(189,'Senegal','SN',189),(190,'Somalia','SO',190),(191,'Suriname','SR',191),(192,'Sao Tome and Principe','ST',192),(193,'El Salvador','SV',193),(194,'Syria','CAD',194),(195,'Swaziland','SZ',195),(196,'Chad','TD',196),(197,'French Southern Territories','TF',197),(198,'Togo','TG',198),(199,'Thailand','TH',199),(200,'Tajikistan','TJ',200),(201,'Tokelau','TK',201),(202,'East Timor','TL',202),(203,'Turkmenistan','TM',203),(204,'Tunisia','TN',204),(205,'Tonga','TO',205),(206,'Turkey','TR',206),(207,'Trinidad and Tobago','TT',207),(208,'Tuvalu','TV',208),(209,'Taiwan','TW',209),(210,'Tanzania','TZ',210),(211,'Ukraine','UA',211),(212,'Uganda','UG',212),(213,'United States Minor Outlying Islands','UM',213),(214,'United States','US',214),(215,'Uruguay','UY',215),(216,'Uzbekistan','UZ',216),(217,'Saint Vincent and the Grenadines','VC',217),(218,'Venezuela','VE',218),(219,'U.S. Virgin Islands','VI',219),(220,'Vietnam','VN',220),(221,'Vanuatu','VU',221),(222,'Wallis and Futuna','WF',222),(223,'Samoa','WS',223),(224,'Kosovo','XK',224),(225,'Yemen','YE',225),(226,'Mayotte','YT',226),(227,'South Africa','ZA',227),(228,'Zambia','ZM',228),(229,'Zimbabwe','ZW',229),(230,'Other','OTHER',230);
/*!40000 ALTER TABLE `country` ENABLE KEYS */;
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
