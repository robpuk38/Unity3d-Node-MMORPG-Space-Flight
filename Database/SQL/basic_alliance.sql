/*
Navicat MySQL Data Transfer

Source Server         : Database
Source Server Version : 50505
Source Host           : 10.0.0.7:3306
Source Database       : serverstorage

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2017-03-22 18:44:21
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
