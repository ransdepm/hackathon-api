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


DROP PROCEDURE IF EXISTS uspAddGameUser;;

CREATE PROCEDURE uspAddGameUser ( IN pId CHAR(38),
                                  IN pName VARCHAR(250))
BEGIN
    INSERT INTO GameUser (Id, Name)
    VALUES (pId, pName);
END;;


DROP PROCEDURE IF EXISTS uspGetGameUserById;;

CREATE PROCEDURE uspGetGameUserById(IN pId CHAR(38))
BEGIN
    SELECT 
        GU.Id, 
        GU.Name,
        GU.CreatedDate,
        GU.Active
    FROM GameUser GU
    WHERE GU.Id = pId;
END;;

DROP PROCEDURE IF EXISTS uspGetGameUserByName;;

CREATE PROCEDURE uspGetGameUserByName(IN pName VARCHAR(250))
BEGIN
    SELECT 
        GU.Id, 
        GU.Name,
        GU.CreatedDate,
        GU.Active
    FROM GameUser GU
    WHERE GU.Name = pName;
END;;



DROP PROCEDURE IF EXISTS uspGetMessages;;

CREATE PROCEDURE uspGetMessages()
BEGIN
    SELECT
        Id, Name
    FROM Message;
END;;

DROP PROCEDURE IF EXISTS uspGetBaseballGames;;

CREATE PROCEDURE uspGetBaseballGames()
BEGIN
    SELECT 
        Id, 
        HomeTeam,
        HomeTeamLogo,
        AwayTeam,
        AwayTeamLogo,
        StartDate,
        Status
    FROM BaseballGame
    WHERE DATE(StartDate) = CURDATE();
END;;


DELIMITER ;