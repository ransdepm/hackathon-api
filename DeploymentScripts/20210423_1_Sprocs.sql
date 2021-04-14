DELIMITER ;;

DROP PROCEDURE IF EXISTS uspGetActiveAdminUserByEmail;;

CREATE PROCEDURE uspGetActiveAdminUserByEmail(IN pEmail VARCHAR(250))
BEGIN
    SELECT 
        AU.Id, 
        AU.Email,
        AU.CreatedDate,
        AU.ActivatedDate,
        AU.PasswordHash,
        AU.Salt
    FROM AdminUser AU
    WHERE AU.Email = pEmail 
      AND AU.ActivatedDate IS NOT NULL;
END;;

DROP PROCEDURE IF EXISTS uspGetAdminUserById;;

CREATE PROCEDURE uspGetAdminUserById(IN pId CHAR(38))
BEGIN
    SELECT 
        AU.Id, 
        AU.Email,
        AU.CreatedDate,
        AU.ActivatedDate
    FROM AdminUser AU
    WHERE AU.Id = pId;
END;;


DROP PROCEDURE IF EXISTS uspAddAdminUser;;

CREATE PROCEDURE uspAddAdminUser( IN pId CHAR(38),
                                  IN pEmail VARCHAR(250),
                                  IN pPasswordHash VARCHAR(255),
                                  IN pSalt CHAR(38))
BEGIN
    INSERT INTO AdminUser (Id, Email, PasswordHash, Salt)
    VALUES (pId, pEmail, pPasswordHash, pSalt);
END;;



DROP PROCEDURE IF EXISTS uspGetMessages;;

CREATE PROCEDURE uspGetMessages()
BEGIN
    SELECT
        Id, Name
    FROM Message;
END;;


DELIMITER ;