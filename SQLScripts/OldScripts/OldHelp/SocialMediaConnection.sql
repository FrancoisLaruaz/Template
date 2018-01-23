CREATE TABLE `SocialMediaConnection` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Date` datetime DEFAULT NULL,
  `LoginProvider` varchar(128) not NULL,
  `ProviderKeyUserSignedUp` varchar(128) not NULL,
  `ProviderKeyUserFriend` varchar(128) not NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `id_UNIQUE` (`Id`),
  UNIQUE KEY `id_UNIQUE_SocialMediaConnection` (`LoginProvider`,`ProviderKeyUserSignedUp`,`ProviderKeyUserFriend`)
) ENGINE=InnoDB AUTO_INCREMENT=580232 DEFAULT CHARSET=utf8;
