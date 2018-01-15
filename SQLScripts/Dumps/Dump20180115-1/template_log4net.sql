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
-- Table structure for table `log4net`
--

DROP TABLE IF EXISTS `log4net`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `log4net` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Date` datetime DEFAULT NULL,
  `Thread` varchar(255) DEFAULT NULL,
  `Level` varchar(50) DEFAULT NULL,
  `Logger` varchar(5000) DEFAULT NULL,
  `Message` varchar(5000) DEFAULT NULL,
  `Exception` varchar(5000) DEFAULT NULL,
  `UserName` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `id_UNIQUE` (`Id`),
  KEY `UserName` (`UserName`),
  CONSTRAINT `log4net_ibfk_1` FOREIGN KEY (`UserName`) REFERENCES `user` (`UserName`)
) ENGINE=InnoDB AUTO_INCREMENT=580093 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `log4net`
--

LOCK TABLES `log4net` WRITE;
/*!40000 ALTER TABLE `log4net` DISABLE KEYS */;
INSERT INTO `log4net` VALUES (580083,'2018-01-14 17:44:20','9','ERROR','System.Web.HttpApplication','- Message => A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'. </br></br>- Url => http://localhost:54808/Account/_SignUpPicture </br></br>- Url Referrer => http://localhost:54808/ </br></br>- HttpMethod => POST </br></br>- Form => X-Requested-With=XMLHttpRequest','System.Web.HttpException (0x80004005): A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'.\r\n   at System.Web.Mvc.Controller.HandleUnknownAction(String actionName)\r\n   at System.Web.Mvc.Controller.<BeginExecuteCore>b__1d(IAsyncResult asyncResult, ExecuteCoreState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecuteCore(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecute(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.<BeginProcessRequest>b__5(IAsyncResult asyncResult, ProcessRequestState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.EndProcessRequest(IAsyncResult asyncResult)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580084,'2018-01-14 17:44:28','9','ERROR','System.Web.HttpApplication','- Message => A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'. </br></br>- Url => http://localhost:54808/Account/_SignUpPicture </br></br>- Url Referrer => http://localhost:54808/ </br></br>- HttpMethod => POST </br></br>- Form => X-Requested-With=XMLHttpRequest','System.Web.HttpException (0x80004005): A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'.\r\n   at System.Web.Mvc.Controller.HandleUnknownAction(String actionName)\r\n   at System.Web.Mvc.Controller.<BeginExecuteCore>b__1d(IAsyncResult asyncResult, ExecuteCoreState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecuteCore(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecute(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.<BeginProcessRequest>b__5(IAsyncResult asyncResult, ProcessRequestState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.EndProcessRequest(IAsyncResult asyncResult)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580085,'2018-01-14 17:44:30','9','ERROR','System.Web.HttpApplication','- Message => A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'. </br></br>- Url => http://localhost:54808/Account/_SignUpPicture </br></br>- Url Referrer => http://localhost:54808/ </br></br>- HttpMethod => POST </br></br>- Form => X-Requested-With=XMLHttpRequest','System.Web.HttpException (0x80004005): A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'.\r\n   at System.Web.Mvc.Controller.HandleUnknownAction(String actionName)\r\n   at System.Web.Mvc.Controller.<BeginExecuteCore>b__1d(IAsyncResult asyncResult, ExecuteCoreState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecuteCore(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecute(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.<BeginProcessRequest>b__5(IAsyncResult asyncResult, ProcessRequestState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.EndProcessRequest(IAsyncResult asyncResult)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580086,'2018-01-14 17:44:33','9','ERROR','System.Web.HttpApplication','- Message => A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'. </br></br>- Url => http://localhost:54808/Account/_SignUpPicture </br></br>- Url Referrer => http://localhost:54808/ </br></br>- HttpMethod => POST </br></br>- Form => X-Requested-With=XMLHttpRequest','System.Web.HttpException (0x80004005): A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'.\r\n   at System.Web.Mvc.Controller.HandleUnknownAction(String actionName)\r\n   at System.Web.Mvc.Controller.<BeginExecuteCore>b__1d(IAsyncResult asyncResult, ExecuteCoreState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecuteCore(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecute(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.<BeginProcessRequest>b__5(IAsyncResult asyncResult, ProcessRequestState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.EndProcessRequest(IAsyncResult asyncResult)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580087,'2018-01-14 17:44:38','9','ERROR','System.Web.HttpApplication','- Message => A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'. </br></br>- Url => http://localhost:54808/Account/_SignUpPicture </br></br>- Url Referrer => http://localhost:54808/ </br></br>- HttpMethod => POST </br></br>- Form => X-Requested-With=XMLHttpRequest','System.Web.HttpException (0x80004005): A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'.\r\n   at System.Web.Mvc.Controller.HandleUnknownAction(String actionName)\r\n   at System.Web.Mvc.Controller.<BeginExecuteCore>b__1d(IAsyncResult asyncResult, ExecuteCoreState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecuteCore(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecute(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.<BeginProcessRequest>b__5(IAsyncResult asyncResult, ProcessRequestState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.EndProcessRequest(IAsyncResult asyncResult)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580088,'2018-01-14 17:44:42','9','ERROR','System.Web.HttpApplication','- Message => A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'. </br></br>- Url => http://localhost:54808/Account/_SignUpPicture </br></br>- Url Referrer => http://localhost:54808/ </br></br>- HttpMethod => POST </br></br>- Form => X-Requested-With=XMLHttpRequest','System.Web.HttpException (0x80004005): A public action method \'_SignUpPicture\' was not found on controller \'Website.Controllers.AccountController\'.\r\n   at System.Web.Mvc.Controller.HandleUnknownAction(String actionName)\r\n   at System.Web.Mvc.Controller.<BeginExecuteCore>b__1d(IAsyncResult asyncResult, ExecuteCoreState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecuteCore(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecute(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.<BeginProcessRequest>b__5(IAsyncResult asyncResult, ProcessRequestState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.EndProcessRequest(IAsyncResult asyncResult)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580089,'2018-01-14 18:07:21','16','ERROR','System.Web.HttpApplication','- Message => The controller for path \'/41ed35a4-d73c-4868-8d5b-5e9b2871c75a\' was not found or does not implement IController. </br></br>- Url => http://localhost:54808/41ed35a4-d73c-4868-8d5b-5e9b2871c75a </br></br>- HttpMethod => GET </br></br>','System.Web.HttpException (0x80004005): The controller for path \'/41ed35a4-d73c-4868-8d5b-5e9b2871c75a\' was not found or does not implement IController.\r\n   at System.Web.Mvc.DefaultControllerFactory.GetControllerInstance(RequestContext requestContext, Type controllerType)\r\n   at System.Web.Mvc.DefaultControllerFactory.CreateController(RequestContext requestContext, String controllerName)\r\n   at System.Web.Mvc.MvcHandler.ProcessRequestInit(HttpContextBase httpContext, IController& controller, IControllerFactory& factory)\r\n   at System.Web.Mvc.MvcHandler.BeginProcessRequest(HttpContextBase httpContext, AsyncCallback callback, Object state)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580090,'2018-01-14 18:07:27','5','ERROR','*** JAVASCRIPT ***','- Message => ReferenceError: idImage is not defined </br></br>- URL Error => http://localhost:54808/Scripts/General/FileDropHelper.js </br></br>- URL Referrer => http://localhost:54808/ </br></br>- Line Number => 118 </br></br>- Col => 29 </br></br>- Browser => Firefox 57.0 </br></br>- Error => ReferenceError: idImage is not defined','','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580091,'2018-01-14 18:25:08','30','ERROR','*** JAVASCRIPT ***','- Message => TypeError: data.data is undefined </br></br>- URL Error => http://localhost:54808/Scripts/General/FileDropHelper.js </br></br>- URL Referrer => http://localhost:54808/ </br></br>- Line Number => 106 </br></br>- Col => 1 </br></br>- Browser => Firefox 57.0 </br></br>- Error => TypeError: data.data is undefined','','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016'),(580092,'2018-01-14 18:36:54','32','ERROR','System.Web.HttpApplication','- Message => A public action method \'Profile\' was not found on controller \'Website.Controllers.AccountController\'. </br></br>- Url => http://localhost:54808/Account/Profile </br></br>- Url Referrer => http://localhost:54808/ </br></br>- HttpMethod => GET </br></br>','System.Web.HttpException (0x80004005): A public action method \'Profile\' was not found on controller \'Website.Controllers.AccountController\'.\r\n   at System.Web.Mvc.Controller.HandleUnknownAction(String actionName)\r\n   at System.Web.Mvc.Controller.<BeginExecuteCore>b__1d(IAsyncResult asyncResult, ExecuteCoreState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecuteCore(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.Controller.EndExecute(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.<BeginProcessRequest>b__5(IAsyncResult asyncResult, ProcessRequestState innerState)\r\n   at System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncVoid`1.CallEndDelegate(IAsyncResult asyncResult)\r\n   at System.Web.Mvc.MvcHandler.EndProcessRequest(IAsyncResult asyncResult)\r\n   at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()\r\n   at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)\r\n','202110219041104046161057117160011087078232185145181026165137189003194239088134072150077231151016');
/*!40000 ALTER TABLE `log4net` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-01-15 10:34:51
