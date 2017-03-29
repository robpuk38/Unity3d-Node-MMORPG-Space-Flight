/*
Navicat MySQL Data Transfer

Source Server         : Database
Source Server Version : 50505
Source Host           : 10.0.0.7:3306
Source Database       : serverstorage

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2017-03-29 17:46:43
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for clients
-- ----------------------------
DROP TABLE IF EXISTS `clients`;
CREATE TABLE `clients` (
  `Id` int(255) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserName` varchar(225) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserPic` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserFirstName` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserLastName` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserAccessToken` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserState` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserAccess` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserCredits` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserLevel` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserMana` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserHealth` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserExp` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;
