/*
Navicat MySQL Data Transfer

Source Server         : Database
Source Server Version : 50505
Source Host           : 10.0.0.7:3306
Source Database       : serverstorage

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2017-03-22 18:33:03
*/

SET FOREIGN_KEY_CHECKS=0;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;
