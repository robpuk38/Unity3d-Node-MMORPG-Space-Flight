/*
Navicat MySQL Data Transfer

Source Server         : Database
Source Server Version : 50505
Source Host           : 10.0.0.7:3306
Source Database       : serverstorage

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2017-03-12 17:24:40
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for basic_players_info
-- ----------------------------
DROP TABLE IF EXISTS `basic_players_info`;
CREATE TABLE `basic_players_info` (
  `Id` int(255) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserName` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserPic` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserToken` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserPosX` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserPosY` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserPosZ` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserLevel` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserCurrency` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserExpierance` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserHealth` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserPower` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserGpsX` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserGpsY` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserGpsZ` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserVungleApi` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserAdcolonyApi` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserAdcolonyZone` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;
