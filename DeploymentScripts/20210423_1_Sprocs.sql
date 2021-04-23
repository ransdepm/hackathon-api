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

DROP PROCEDURE IF EXISTS uspCreateBaseballGame;;

CREATE PROCEDURE uspCreateBaseballGame(IN pHomeTeam VARCHAR(250),
                                       IN pHomeTeamLogo VARCHAR(250),
                                       IN pAwayTeam VARCHAR(250),
                                       IN pAwayTeamLogo VARCHAR(250),
                                       IN pStartDate DATETIME,
                                       OUT new_record INT)
BEGIN
    INSERT INTO BaseballGame (HomeTeam, HomeTeamLogo, AwayTeam, AwayTeamLogo, StartDate, Status)
    VALUES (pHomeTeam, pHomeTeamLogo, pAwayTeam, pAwayTeamLogo, pStartDate, 'PENDING');
    
    SELECT LAST_INSERT_ID() INTO new_record;
    
    INSERT INTO MoundGame (Status, pBaseballGameId)
    VALUES ('STARTED', new_record);
    
    INSERT INTO MoundResult (Status, MoundGameId)
    VALUES ('PENDING', LAST_INSERT_ID());
END;;

DROP PROCEDURE IF EXISTS uspGetMoundGameByBaseballGameId;;

CREATE PROCEDURE uspGetMoundGameByBaseballGameId(IN pBaseballGameId INT)
BEGIN
    SELECT 
        Id, 
        Status,
        BaseballGameId
    FROM MoundGame
    WHERE BaseballGameId = pBaseballGameId;
END;;

DROP PROCEDURE IF EXISTS uspCreateMoundGame;;

CREATE PROCEDURE uspCreateMoundGame(IN pBaseballGameId INT,
                                    OUT new_record INT)
BEGIN
    INSERT INTO MoundGame (Status, BaseballGameId)
    VALUES ('STARTED', pBaseballGameId);
    
    SELECT LAST_INSERT_ID() INTO new_record;
END;;

DROP PROCEDURE IF EXISTS uspCreateMoundResult;;

CREATE PROCEDURE uspCreateMoundResult(IN pMoundGameId INT, OUT new_record INT)
BEGIN
    INSERT INTO MoundResult (Status, MoundGameId)
    VALUES ('PENDING', pMoundGameId);
    
    SELECT LAST_INSERT_ID() INTO new_record;
END;;

DROP PROCEDURE IF EXISTS uspLockMoundResult;;

CREATE PROCEDURE uspLockMoundResult(IN pMoundResultId INT)
BEGIN
    UPDATE MoundResult 
    SET Status='LOCKED'
    WHERE Id = pMoundResultId;
END;;

DROP PROCEDURE IF EXISTS uspAddMoundResult;;

CREATE PROCEDURE uspAddMoundResult(IN pMoundResultId INT,
                                   IN pMoundResult VARCHAR(250))
BEGIN
    UPDATE MoundResult 
    SET Status='COMPLETED',
        MoundResult = pMoundResult
    WHERE Id = pMoundResultId;
END;;


DROP PROCEDURE IF EXISTS uspGetMoundResults;;

CREATE PROCEDURE uspGetMoundResults(IN pMoundGameId INT)
BEGIN
    SELECT 
        Id, 
        MoundResult,
        Status,
        MoundGameId
    FROM MoundResult
    WHERE MoundGameId = pMoundGameId;
END;;

DROP PROCEDURE IF EXISTS uspGetMoundResultById;;

CREATE PROCEDURE uspGetMoundResultById(IN pMoundResultId INT)
BEGIN
    SELECT 
        Id, 
        MoundResult,
        Status,
        MoundGameId
    FROM MoundResult
    WHERE Id = pMoundResultId;
END;;

DROP PROCEDURE IF EXISTS uspAddUserMoundResult;;

CREATE PROCEDURE uspAddUserMoundResult(IN pMoundResultId INT,
                                       IN pGameUserId CHAR(38),
                                       IN pSubmission VARCHAR(250),
                                       OUT new_record INT)
BEGIN
    INSERT INTO UserMoundResult (MoundResultId, GameUserId, Submission)
    VALUES (pMoundResultId, pGameUserId, pSubmission);
    
    SELECT LAST_INSERT_ID() INTO new_record;
END;;

DROP PROCEDURE IF EXISTS uspUpdateUserMoundResult;;

CREATE PROCEDURE uspUpdateUserMoundResult(IN pMoundResultId INT,
                                       IN pGameUserId CHAR(38),
                                       IN pSubmission VARCHAR(250))
BEGIN
    UPDATE UserMoundResult
    SET Submission = pSubmission
    WHERE pGameUserId = GameUserId AND pMoundResultId = MoundResultId;
END;;

DROP PROCEDURE IF EXISTS uspGetUserMoundResultById;;

CREATE PROCEDURE uspGetUserMoundResultById(IN pMoundResultId INT,
                                           IN pGameUserId CHAR(38))
BEGIN
    SELECT 
        Id, 
        Submission
    FROM UserMoundResult
    WHERE pGameUserId = GameUserId AND pMoundResultId = MoundResultId;
END;;

DROP PROCEDURE IF EXISTS uspGetUserResultsByMoundGame;;

CREATE PROCEDURE uspGetUserResultsByMoundGame(IN pMoundGameId INT,
                                              IN pGameUserId CHAR(38))
BEGIN
    SELECT MR.MoundGameId,
           MR.Id,
           MR.MoundResult, 
           UMR.Submission 
    FROM MoundResult MR
     LEFT JOIN UserMoundResult UMR on MR.Id = UMR.MoundResultId AND (UMR.GameUserId = pGameUserId OR UMR.GameUserId IS NULL)
    WHERE MR.Status = 'COMPLETED' AND
          MR.MoundGameId = pMoundGameId
    ORDER BY MR.Id;
END;;


DROP PROCEDURE IF EXISTS uspGetAllResultsByMoundGame;;

CREATE PROCEDURE uspGetAllResultsByMoundGame(IN pMoundGameId INT)
BEGIN
    SELECT GU.Name, count(GU.Name) as Score
    FROM UserMoundResult UMR
     LEFT JOIN MoundResult MR on UMR.MoundResultId = MR.Id
     LEFT JOIN MoundGame MG on MR.MoundGameId = MG.Id
     LEFT JOIN GameUser GU on GU.Id = UMR.GameUserId
    WHERE MG.Id = pMoundGameId
    GROUP BY GU.Name;
END;;


DELIMITER ;