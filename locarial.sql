-- --------------------------------------------------------
-- Hôte:                         127.0.0.1
-- Version du serveur:           8.4.3 - MySQL Community Server - GPL
-- SE du serveur:                Win64
-- HeidiSQL Version:             12.15.0.7171
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Listage de la structure de la base pour locarial
CREATE DATABASE IF NOT EXISTS `locarial` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `locarial`;

-- Listage de la structure de table locarial. utilisateurs
CREATE TABLE IF NOT EXISTS `utilisateurs` (
  `Id_Utilisateurs` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `mot_de_passe` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `prenom` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `ville` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `rue` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `numero_de_telephone` varchar(20) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `code_postal` varchar(10) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id_Utilisateurs`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Les données exportées n'étaient pas sélectionnées.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
