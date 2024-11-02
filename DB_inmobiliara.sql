-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: raffarraffa_lab3_inmobiliaria
-- ------------------------------------------------------
-- Server version	8.3.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `ciudad`
--

DROP TABLE IF EXISTS `ciudad`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ciudad` (
  `id` int NOT NULL AUTO_INCREMENT,
  `ciudad` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ciudad`
--

LOCK TABLES `ciudad` WRITE;
/*!40000 ALTER TABLE `ciudad` DISABLE KEYS */;
INSERT INTO `ciudad` VALUES (1,'San Luis'),(2,'Carpinteria');
/*!40000 ALTER TABLE `ciudad` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contrato`
--

DROP TABLE IF EXISTS `contrato`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contrato` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_inquilino` int NOT NULL,
  `id_inmueble` int NOT NULL,
  `fecha_inicio` date DEFAULT NULL,
  `fecha_fin` date DEFAULT NULL,
  `fecha_efectiva` date DEFAULT NULL,
  `monto` decimal(9,2) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT '0',
  `creado_fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_usuario` int NOT NULL,
  `cancelado_fecha` datetime DEFAULT NULL,
  `cancelado_usuario` int NOT NULL,
  `editado_usuario` int DEFAULT NULL,
  `editado_fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `contrato_inmueble_idx` (`id_inmueble`),
  KEY `contrato_inquilino_idx` (`id_inquilino`),
  KEY `fecha_inicio` (`fecha_inicio`,`fecha_fin`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contrato`
--

LOCK TABLES `contrato` WRITE;
/*!40000 ALTER TABLE `contrato` DISABLE KEYS */;
INSERT INTO `contrato` VALUES (1,1,1,'2023-04-23','2024-04-30','2024-04-30',1500.00,0,'2024-04-23 01:47:34',0,'2024-04-24 16:31:44',0,0,NULL),(2,3,1,'2023-04-23','2025-04-23','2025-04-23',23425.00,0,'2024-04-23 01:49:19',0,'2024-04-23 01:53:57',0,0,NULL),(3,1,1,'2024-04-26','2025-04-25','2025-04-25',2500.00,0,'2024-04-23 02:04:46',0,'2024-04-23 02:04:46',0,0,NULL),(4,4,3,'2024-04-25','2024-04-30','2024-04-30',1000.00,0,'2024-04-24 22:10:10',0,NULL,0,NULL,NULL),(5,4,10,'2024-04-25','2024-05-01','2024-05-01',1000.00,0,'2024-04-25 12:15:10',2,NULL,0,2,'2024-04-25 12:22:08'),(6,4,11,'2024-04-25','2025-04-25','2024-04-30',1000.00,0,'2024-04-25 16:33:24',2,NULL,0,NULL,NULL);
/*!40000 ALTER TABLE `contrato` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmueble` (
  `id` int NOT NULL AUTO_INCREMENT,
  `direccion` varchar(100) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'calle y altura',
  `uso` enum('Comercial','Residencial') COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'Comercial',
  `id_tipo` int NOT NULL,
  `ambientes` tinyint NOT NULL DEFAULT '1',
  `coordenadas` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `precio` decimal(11,2) DEFAULT NULL,
  `id_propietario` int NOT NULL,
  `estado` enum('Disponible','Retirado') COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'Disponible',
  `id_ciudad` int NOT NULL,
  `id_zona` int NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT '0',
  `descripcion` text COLLATE utf8mb4_general_ci,
  `url_img` varchar(245) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`id`),
  KEY `propietario_inmueble_idx` (`id_propietario`),
  KEY `fk_inmueble_ciudad1_idx` (`id_ciudad`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
INSERT INTO `inmueble` VALUES (1,'san martin 45','Comercial',1,9,'-32.414566613131946, -65.00877119828924',1.26,4,'Disponible',1,2,0,'Casa  de 2 ambientes','qwerty.jpg'),(2,'av Los Mandarinos 566','Comercial',4,0,'-32.4239588393048, -65.01171109578154',2.00,3,'Disponible',2,2,0,'Oficina de 250m2','qwerty.jpg'),(3,'Av. Illia 1234','Residencial',2,0,'-32.35045498578529, -65.01366813627159',3.00,3,'Disponible',2,2,0,'Local con dependencias, oficina de 12m2','qwerty.jpg'),(4,'Junin 345','Comercial',2,0,'-32.35045498578529, -65.01366813627159',4.00,4,'Disponible',2,2,0,'Local 232 metros, con entrepiso  2 baños','qwerty.jpg'),(5,'Junin 345','Comercial',1,5,'-33.25, -66.36',55.00,4,'Disponible',1,2,0,'Casa 2 plantas, techo tecja, con cochera 3 autos','qwerty.jpg'),(6,'Junin 345','Comercial',2,9,'-33.25, -66.36',2635.52,4,'Disponible',1,1,0,'Dpto 5º piso,  3 dormitorios, uno en suite. ','qwerty.jpg'),(7,'Junin 345','Comercial',1,0,'-33.25, -66.36',5555.00,3,'Disponible',1,1,0,'55dthysh','qwerty.jpg'),(8,'nose','Comercial',2,0,'-33.25, -66.36',150000.00,4,'Disponible',2,2,0,'6666666666666','qwerty.jpg'),(9,'Junin 345','Comercial',1,5,'-33.25, -66.36',5625.00,4,'Disponible',1,1,0,'csa de 2 planta, 3 dormitorios, uno en suite\r\n','qwerty.jpg'),(10,'Junin 345','Comercial',1,4,'-33.25, -66.36',666.00,4,'Disponible',1,1,0,'ssaf','qwerty.jpg'),(11,'calle 123','Comercial',2,2,'-32.33956201596181, -65.02491930996581',1200.00,3,'Disponible',1,1,0,'Inmueble del profe','qwerty.jpg'),(12,'Junin 345','Comercial',1,2,'-33.25, -66.36',55.00,94,'Disponible',1,2,0,'Casa de 2 plantas, techo de teja, con cochera para 3 autos','qwerty.jpg');
/*!40000 ALTER TABLE `inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilino` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `apellido` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `dni` varchar(11) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `telefono` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (1,'Marcelo','Jofre','1256555','b1729985-ffa2-11ee-a424-b8aeedb3ac9e','b1729993-ffa2-11ee-a424-b8aeedb3ac9e',0),(2,'Jorge','Mendez','1256555','905b3f1b-ee14-11ee-8ebc-b8aeedb3ac9e','905b3f29-ee14-11ee-8ebc-b8aeedb3ac9e',0),(3,'Natalia','Gomez','1256555','0f078f6f-ff9f-11ee-a424-b8aeedb3ac9e','0f078f7c-ff9f-11ee-a424-b8aeedb3ac9e',0),(4,'Maria Florencia','Fernandez','1256555','d3d99b4c-0053-11ef-a424-b8aeedb3ac9e','d3d99b62-0053-11ef-a424-b8aeedb3ac9e',0),(6,'Eduardo','Maldonado','1234567890','ajfb@sas','12345678',0);
/*!40000 ALTER TABLE `inquilino` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pago`
--

DROP TABLE IF EXISTS `pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pago` (
  `id` int NOT NULL,
  `id_contrato` int NOT NULL,
  `fecha_pago` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `importe` decimal(11,2) NOT NULL COMMENT 'si es negativo es una nota de credito',
  `estado` varchar(25) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `numero_pago` int unsigned NOT NULL DEFAULT '1',
  `detalle` varchar(150) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'aca van los detalles de cada abono -> paga el mes x - abono mes x',
  `creado_fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_usuario` int NOT NULL,
  `editado_usuario` int DEFAULT NULL,
  `editado_fecha` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
INSERT INTO `pago` VALUES (2,1,'2024-04-24 00:00:00',1500.00,'0',1,'pago de mayo','2024-04-24 21:52:00',1,NULL,NULL),(3,1,'2024-04-24 00:00:00',1500.00,'0',2,'pago adelantado de junio','2024-04-24 23:10:21',1,NULL,NULL),(4,1,'2024-04-24 00:00:00',-500.00,'0',3,'Nota de crédito: reposición de un caño ','2024-04-24 23:11:21',1,NULL,NULL);
/*!40000 ALTER TABLE `pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietario`
--

DROP TABLE IF EXISTS `propietario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietario` (
  `id` int NOT NULL,
  `nombre` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `apellido` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `dni` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `telefono` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `avatar` varchar(200) COLLATE utf8mb4_general_ci NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT '0',
  `pass_restore` varchar(255) COLLATE utf8mb4_general_ci DEFAULT 'x'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietario`
--

LOCK TABLES `propietario` WRITE;
/*!40000 ALTER TABLE `propietario` DISABLE KEYS */;
INSERT INTO `propietario` VALUES (1,'Jose','Perezfdywr','12345678','uno@prop.com','6985','$2b$10$dn2/7iKEbU63VbXBhb5rbult1IpEOX.J338VeZAGv8LK.mxGgdFgq','avatar1.png',0,'x'),(2,'Jose','Perez','12345678','dos@prop.com','12345','$2b$10$dn2/7iKEbU63VbXBhb5rbult1IpEOX.J338VeZAGv8LK.mxGgdFgq','img6_2_e6a18f08-f5e1-485d-85e1-f16e08da311c.png',0,'x'),(3,'Jossssse','Perez','12345678','tres@prop.com','12345','$2b$10$RDwKyJMvmBj6SEZVzHnsteSHBczk7Uss1Q8FoJzg3BeQ28TeoQncq','avatar2.png',0,'x'),(4,'Marcelo','JOF','12345678','lopezrafa@gmail.com','123','$2b$10$dn2/7iKEbU63VbXBhb5rbult1IpEOX.J338VeZAGv8LK.mxGgdFgq','4/avatares/',1,'{\"pass\":\"$2b$10$hscCVzTcUpR3tyoAXLGeXum5Czf5OUL/BeEvsqG5dQ0VWOaQRSFHe\",\"timestamp\":1729705003}'),(5,'Pedro','Junco','12569865','cinco@prop.com','123456789','$2b$10$RDwKyJMvmBj6SEZVzHnsteSHBczk7Uss1Q8FoJzg3BeQ28TeoQncq','',0,'x'),(9,'striasddasfdng','asdasdstring','1234235g','user@example.com','string','$2b$10$RDwKyJMvmBj6SEZVzHnsteSHBczk7Uss1Q8FoJzg3BeQ28TeoQncq','string',1,'x');
/*!40000 ALTER TABLE `propietario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_inmueble`
--

DROP TABLE IF EXISTS `tipo_inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_inmueble` (
  `id` int NOT NULL,
  `tipo` varchar(200) COLLATE utf8mb4_general_ci NOT NULL,
  `borrado` tinyint(1) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_inmueble`
--

LOCK TABLES `tipo_inmueble` WRITE;
/*!40000 ALTER TABLE `tipo_inmueble` DISABLE KEYS */;
INSERT INTO `tipo_inmueble` VALUES (1,'Casa',0),(2,'Departamento',0),(3,'Deposito',0),(4,'Local',0),(5,'Cabaña',0),(6,'Quintas',0),(7,'hostel',0),(8,'CAMPING',0);
/*!40000 ALTER TABLE `tipo_inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id` int NOT NULL,
  `nombre` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `apellido` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `dni` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(45) COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(250) COLLATE utf8mb4_general_ci NOT NULL,
  `rol` enum('usuario','administrador') COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'usuario' COMMENT 'solo vamos a usar 2 tipos de usuarios.\\n- usuario normal de la plataforma\\n- un  administrador',
  `avatarUrl` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT '0' COMMENT '0 para activo, 1 para inactivo',
  `update_at` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='tabla para usuarios internos del sistema';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Leonel','Toloza','123456789','admin@admin.com','$2a$11$5p7Wyffaj/wgMbVWxBhNjOFVmhAw/Fw.XTk1Na4HqYyOOFeYkB9LC','administrador',NULL,0,'2024-05-04 14:19:05'),(2,'Santiago Leonel','Toloza','987654321','leotoloza6@gmail.com','$2a$11$IYzzl8cwybgKg7dAe/URhO9qZXGXAUtcqZXrkpHnjahqzGzJoQDuG','usuario',NULL,0,'2024-04-25 11:58:13'),(3,'Rafael ','Lopez','123456','lopezrafa@gmail.com','$2a$11$zujeCmTH/ewXdFu738wpqur5i69oPqLpIam6vGtLmHRdr55zft.m6','administrador',NULL,0,'2024-04-25 10:31:20');
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zona`
--

DROP TABLE IF EXISTS `zona`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zona` (
  `id` int NOT NULL,
  `zona` varchar(50) COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zona`
--

LOCK TABLES `zona` WRITE;
/*!40000 ALTER TABLE `zona` DISABLE KEYS */;
INSERT INTO `zona` VALUES (1,'Norte'),(2,'Centro');
/*!40000 ALTER TABLE `zona` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-02 17:59:02
