delete from tasklog  where id>=1;
delete from scheduledtask  where id>=1;
delete from emailaudit where id>=1;
delete from userrole where id>=1;
delete from user where id>=1;
delete from category where id>=1;
delete from categorytype where id>=1;



-- CATEGORY TYPE

INSERT INTO `categorytype`
(`Id`,
`Name`)
VALUES
(1,
'EmailType');



INSERT INTO `categorytype`
(`Id`,
`Name`)
VALUES
(2,
'UserRole');


-- CATEGORY


INSERT INTO `category`
(`Id`,
`Name`,
`Code`,
`CategoryTypeId`,
`DateModification`)
VALUES
(1001,
'forgotpassword',
null,
1,
NOW());

INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`CategoryTypeId`,
`DateModification`)
VALUES
(2001,
'Admin',
null,
2,
NOW());



INSERT INTO `categorytype`
(`Id`,
`Name`)
VALUES
(3,
'Language');


INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`CategoryTypeId`,
`DateModification`)
VALUES
(3001,
'English',
'en',
3,
NOW());

INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`CategoryTypeId`,
`DateModification`)
VALUES
(3002,
'French',
'fr',
3,
NOW());


update category
set `Name`=Description,
`Order`=Id-3000
where CategoryTypeId=3


INSERT INTO `template`.`emailtypelanguage`
(
`EMailTypeId`,
`LanguageId`,
`Subject`,
`TemplateName`)
VALUES
(
1001,
3001,
'Reset your password',
'forgotpassword_en');


INSERT INTO `template`.`emailtypelanguage`
(
`EMailTypeId`,
`LanguageId`,
`Subject`,
`TemplateName`)
VALUES
(
1001,
3002,
'Réinitialisez votre mot de passe',
'forgotpassword_fr');


INSERT INTO `category`
(`Id`,
`Name`,
`Code`,
`CategoryTypeId`,
`DateModification`)
VALUES
(1002,
'userwelcome',
null,
1,
NOW());

INSERT INTO `template`.`emailtypelanguage`
(
`EMailTypeId`,
`LanguageId`,
`Subject`,
`TemplateName`)
VALUES
(
1002,
3001,
'Welcome',
'forgotpassword_en');


INSERT INTO `template`.`emailtypelanguage`
(
`EMailTypeId`,
`LanguageId`,
`Subject`,
`TemplateName`)
VALUES
(
1002,
3002,
'Bienvenue',
'forgotpassword_fr');


INSERT INTO `categorytype`
(`Id`,
`Name`)
VALUES
(4,
'TaskLogType');


INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`CategoryTypeId`,
`DateModification`)
VALUES
(4001,
'ErrorCleanUp',
null,
1,
NOW());


update category
set `Order`=Id
where id>1

update category
set `Field1`='_EndMail_fr'
where `code`='fr' and id>=1



update category
set `Field1`='_EndMail_en'
where `code`='en' and id>=1



INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`CategoryTypeId`,
`DateModification`)
VALUES
(4002,
'UploadFilesCleanUp',
null,
1,
NOW());