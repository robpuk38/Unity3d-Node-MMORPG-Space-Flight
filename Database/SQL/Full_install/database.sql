/*
Navicat MySQL Data Transfer

Source Server         : Database
Source Server Version : 50505
Source Host           : 10.0.0.7:3306
Source Database       : serverstorage

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2017-03-22 18:44:37
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for basic_alliance
-- ----------------------------
DROP TABLE IF EXISTS `basic_alliance`;
CREATE TABLE `basic_alliance` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `myid` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `friendid` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;

-- ----------------------------
-- Table structure for basic_object
-- ----------------------------
DROP TABLE IF EXISTS `basic_object`;
CREATE TABLE `basic_object` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `power` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `health` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `level` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `posx` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `posy` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `posz` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `rotx` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `roty` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  `rotz` varchar(255) COLLATE utf32_unicode_ci DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;

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
  `UserRotX` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserRotY` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  `UserRotZ` varchar(255) COLLATE utf32_unicode_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;
