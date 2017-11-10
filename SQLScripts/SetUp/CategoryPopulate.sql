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
1,
NOW());



update category
set `Order`=Id
where id>1

update template.category
set description='Reset Your Password'
where id=1001