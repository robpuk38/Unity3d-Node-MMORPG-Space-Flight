
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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;
