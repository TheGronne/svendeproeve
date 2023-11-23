-- --------------------------------------------------------
-- Vært:                         127.0.0.1
-- Server-version:               5.7.24 - MySQL Community Server (GPL)
-- ServerOS:                     Win64
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for svendeproeve
CREATE DATABASE IF NOT EXISTS `svendeproeve` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `svendeproeve`;

-- Dumping structure for tabel svendeproeve.matches
CREATE TABLE IF NOT EXISTS `matches` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Player1` int(11) NOT NULL,
  `Player2` int(11) NOT NULL,
  `Player3` int(11) DEFAULT NULL,
  `Player4` int(11) DEFAULT NULL,
  `Winner` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `Player1` (`Player1`),
  KEY `Player2` (`Player2`),
  KEY `Player3` (`Player3`),
  KEY `Player4` (`Player4`),
  KEY `Winner` (`Winner`),
  CONSTRAINT `matches_ibfk_1` FOREIGN KEY (`Player1`) REFERENCES `users` (`ID`),
  CONSTRAINT `matches_ibfk_2` FOREIGN KEY (`Player2`) REFERENCES `users` (`ID`),
  CONSTRAINT `matches_ibfk_3` FOREIGN KEY (`Player3`) REFERENCES `users` (`ID`),
  CONSTRAINT `matches_ibfk_4` FOREIGN KEY (`Player4`) REFERENCES `users` (`ID`),
  CONSTRAINT `matches_ibfk_5` FOREIGN KEY (`Winner`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=latin1;

-- Dumping data for table svendeproeve.matches: ~26 rows (approximately)
/*!40000 ALTER TABLE `matches` DISABLE KEYS */;
INSERT INTO `matches` (`ID`, `Player1`, `Player2`, `Player3`, `Player4`, `Winner`) VALUES
	(1, 9, 18, NULL, NULL, 9),
	(2, 9, 18, NULL, NULL, 9),
	(3, 9, 18, NULL, NULL, 9),
	(4, 9, 18, NULL, NULL, 9),
	(5, 18, 9, NULL, NULL, 9),
	(6, 9, 18, NULL, NULL, 9),
	(7, 9, 18, NULL, NULL, 9),
	(8, 9, 18, NULL, NULL, 9),
	(9, 18, 9, NULL, NULL, 9),
	(10, 9, 18, NULL, NULL, 9),
	(11, 9, 18, NULL, NULL, 9),
	(12, 9, 18, NULL, NULL, 9),
	(13, 9, 16, NULL, NULL, 9),
	(14, 9, 18, NULL, NULL, 9),
	(15, 9, 18, NULL, NULL, 18),
	(16, 9, 18, NULL, NULL, 18),
	(17, 9, 18, NULL, NULL, 9),
	(18, 9, 18, NULL, NULL, 9),
	(19, 9, 18, NULL, NULL, 9),
	(20, 9, 18, NULL, NULL, 9),
	(21, 9, 18, NULL, NULL, 18),
	(22, 18, 9, NULL, NULL, 9),
	(23, 9, 18, NULL, NULL, 18),
	(24, 9, 18, NULL, NULL, 9),
	(25, 9, 18, NULL, NULL, 9),
	(26, 9, 18, NULL, NULL, 9),
	(27, 9, 18, 16, NULL, 16),
	(28, 9, 18, NULL, NULL, 9),
	(29, 9, 18, NULL, NULL, 9),
	(30, 9, 18, NULL, NULL, 9),
	(31, 21, 9, NULL, NULL, 21);
/*!40000 ALTER TABLE `matches` ENABLE KEYS */;

-- Dumping structure for tabel svendeproeve.matchstats
CREATE TABLE IF NOT EXISTS `matchstats` (
  `MatchID` int(11) NOT NULL,
  `PlayerID` int(11) NOT NULL,
  `Kills` int(11) NOT NULL,
  `Deaths` int(11) NOT NULL,
  KEY `MatchID` (`MatchID`),
  KEY `PlayerID` (`PlayerID`),
  CONSTRAINT `matchstats_ibfk_1` FOREIGN KEY (`MatchID`) REFERENCES `matches` (`ID`),
  CONSTRAINT `matchstats_ibfk_2` FOREIGN KEY (`PlayerID`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table svendeproeve.matchstats: ~17 rows (approximately)
/*!40000 ALTER TABLE `matchstats` DISABLE KEYS */;
INSERT INTO `matchstats` (`MatchID`, `PlayerID`, `Kills`, `Deaths`) VALUES
	(23, 9, 0, 1),
	(23, 18, 1, 0),
	(24, 9, 0, 0),
	(24, 18, 1, 1),
	(25, 9, 1, 0),
	(25, 18, 1, 2),
	(26, 9, 5, 4),
	(26, 18, 4, 5),
	(27, 9, 0, 4),
	(27, 18, 0, 4),
	(27, 16, 8, 0),
	(28, 9, 1, 0),
	(28, 18, 4, 5),
	(29, 9, 0, 0),
	(29, 18, 5, 5),
	(30, 9, 0, 0),
	(30, 18, 6, 6),
	(31, 21, 4, 1),
	(31, 9, 2, 5);
/*!40000 ALTER TABLE `matchstats` ENABLE KEYS */;

-- Dumping structure for tabel svendeproeve.users
CREATE TABLE IF NOT EXISTS `users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) NOT NULL,
  `LoginName` varchar(255) NOT NULL,
  `PASSWORD` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Must_be_unique` (`LoginName`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=latin1;

-- Dumping data for table svendeproeve.users: ~9 rows (approximately)
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`ID`, `Username`, `LoginName`, `PASSWORD`) VALUES
	(4, 'string', 'stig', 'string'),
	(7, 'string', 'sng', 'string'),
	(9, 'lol', 'lol', 'lol'),
	(12, 'asdf', 'sadf', 'asdf'),
	(13, 'fdsafsa', 'fdsaf', 'sadfasdfasdf'),
	(14, '12345', '12345', '12345'),
	(16, 'lul', 'lul', 'lul'),
	(18, 'lil', 'lil', 'lil'),
	(19, 'lyl', 'lyl', 'lyl'),
	(21, 'dwd', 'dwd', 'dwd');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
