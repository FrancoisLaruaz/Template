alter table scheduledtask
drop foreign key scheduledtask_ibfk_1

ALTER TABLE `template`.`news` 
CHANGE COLUMN `Id` `Id` INT(11) NOT NULL AUTO_INCREMENT;


alter table scheduledtask
add foreign key FK_scheduledtask_News  (`NewsId`) REFERENCES `news` (`Id`);

INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`Order`,
`Active`,
`CategoryTypeId`,
`DateModification`,
`Field1`,
`Field2`)
VALUES
(1003,
'News',
null,
1003,
1,
1,
now(),
null,
null);


INSERT INTO `emailtypelanguage` VALUES (5,1003,3001,'','news_en'),
(6,1003,3002,'','news_fr');
