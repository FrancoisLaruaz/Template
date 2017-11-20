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
`Description`,
`CategoryTypeId`,
`DateModification`)
VALUES
(1001,
'forgotpassword',
'forgotpassword',
null,
1,
NOW());

INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`Description`,
`CategoryTypeId`,
`DateModification`)
VALUES
(2001,
'Admin',
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
`Description`,
`CategoryTypeId`,
`DateModification`)
VALUES
(3001,
'English',
'en',
'English',
3,
NOW());

INSERT INTO `template`.`category`
(`Id`,
`Name`,
`Code`,
`Description`,
`CategoryTypeId`,
`DateModification`)
VALUES
(3002,
'French',
'fr',
'French',
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
`Description`,
`CategoryTypeId`,
`DateModification`)
VALUES
(4001,
'ErrorCleanUp',
'ErrorCleanUp',
null,
1,
NOW());


update category
set `Order`=Id
where id>1

update template.category
set description='Reset Your Password'
where id=1001