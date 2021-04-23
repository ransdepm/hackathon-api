using System;
using System.Collections.Generic;
using System.Data;

namespace Hackathon.Data
{
    public class DataAccess : DataAccessBase
    {
        protected string ConnectionString = null;
        public DataAccess(string sqlConnection) : base(sqlConnection)
        {
        }

        public DataSet GetActiveAdminUserByEmail(string email)
        {
            DataSet ds = ExecuteDataSet("uspGetActiveAdminUserByEmail", new Dictionary<string, IConvertible>(){
                    {"pEmail", email}
                });
            return ds;
        }

        public DataSet GetAdminUserById(Guid userId)
        {
            DataSet ds = ExecuteDataSet("uspGetAdminUserById", new Dictionary<string, IConvertible>(){
                    {"pId", userId.ToString()}
                });
            return ds;
        }

        public DataSet GetGameUserById(Guid userId)
        {
            DataSet ds = ExecuteDataSet("uspGetGameUserById", new Dictionary<string, IConvertible>(){
                    {"pId", userId.ToString()}
                });
            return ds;
        }

        public DataSet GetBaseballGames()
        {
            DataSet ds = ExecuteDataSet("uspGetBaseballGames");
            return ds;
        }

        public Guid CreateAdminUser(string email, string passwordHash, Guid salt)
        {
            Guid id = Guid.NewGuid();
            var parameters = new Dictionary<string, IConvertible>()
            {
                {"pId", id.ToString()},
                {"pEmail", email},
                {"pPasswordHash", passwordHash},
                {"pSalt", salt.ToString()}
            };

            InsertData("uspAddAdminUser", parameters);
            return id;
        }

        public DataSet GetMessages()
        {
            DataSet ds = ExecuteDataSet("uspGetMessages");
            return ds;
        }

        public Guid CreateUser(string name)
        {
            Guid id = Guid.NewGuid();
            var parameters = new Dictionary<string, IConvertible>()
            {
                {"pId", id.ToString()},
                {"pName", name}
            };

            InsertData("uspAddGameUser", parameters);
            return id;
        }

        public DataSet GetActiveUserByName(string name)
        {
            DataSet ds = ExecuteDataSet("uspGetGameUserByName", new Dictionary<string, IConvertible>(){
                    {"pName", name}
                });
            return ds;
        }

        public DataSet GetMoundGameByBaseballGameId(int baseballGameId)
        {
            DataSet ds = ExecuteDataSet("uspGetMoundGameByBaseballGameId", new Dictionary<string, IConvertible>(){
                    {"pBaseballGameId", baseballGameId}
                });
            return ds;
        }

        public DataSet GetMoundResultById(int moundResultId)
        {
            DataSet ds = ExecuteDataSet("uspGetMoundResultById", new Dictionary<string, IConvertible>(){
                    {"pMoundResultId", moundResultId}
                });
            return ds;
        }

        public DataSet GetMoundGameResults(int moundGameId)
        {
            DataSet ds = ExecuteDataSet("uspGetMoundResults", new Dictionary<string, IConvertible>(){
                    {"pMoundGameId", moundGameId}
                });
            return ds;
        }

        public int StartNextMoundRound(int moundGameId)
        {
            var parameters = new Dictionary<string, IConvertible>()
            {
                {"pMoundGameId", moundGameId}
            };

            var id = InsertDataReturnId("uspCreateMoundResult", parameters);
            return id;
        }

        public void LockMoundRound(int moundGameId)
        {
            var parameters = new Dictionary<string, IConvertible>()
            {
                {"pMoundResultId", moundGameId}
            };

            InsertData("uspLockMoundResult", parameters);
        }

        public void AddMoundResult(int moundResultId, string moundResult)
        {
            var parameters = new Dictionary<string, IConvertible>()
            {
                {"pMoundResultId", moundResultId},
                {"pMoundResult", moundResult},
            };

            InsertData("uspAddMoundResult", parameters);
        }

        public int AddUserMoundResult(int id, Guid userId, string submission)
        {
            return InsertDataReturnId("uspAddUserMoundResult", new Dictionary<string, IConvertible>(){
                    {"pMoundResultId", id},
                    {"pGameUserId", userId.ToString()},
                    {"pSubmission",submission}
                });
        }

        public DataSet GetUserMoundResultByIds(int id, Guid userId)
        {
            DataSet ds = ExecuteDataSet("uspGetUserMoundResultById", new Dictionary<string, IConvertible>(){
                    {"pMoundResultId", id},
                    {"pGameUserId", userId.ToString()}
                });
            return ds;
        }

        public void UpdateUserMoundResult(int id, Guid userId, string submission)
        {
            InsertData("uspUpdateUserMoundResult", new Dictionary<string, IConvertible>(){
                    {"pMoundResultId", id},
                    {"pGameUserId", userId.ToString()},
                    {"pSubmission",submission}
                });
        }

        public DataSet GetUserResultsByMoundGame(int moundGameId, Guid userId)
        {
            DataSet ds = ExecuteDataSet("uspGetUserResultsByMoundGame", new Dictionary<string, IConvertible>(){
                    {"pMoundGameId", moundGameId},
                    {"pGameUserId", userId.ToString()}
                });
            return ds;
        }

        public DataSet GetAllResultsByMoundGame(int moundGameId)
        {
            DataSet ds = ExecuteDataSet("uspGetAllResultsByMoundGame", new Dictionary<string, IConvertible>(){
                    {"pMoundGameId", moundGameId}
                });
            return ds;
        }
    }
}
