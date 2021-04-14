﻿using System;
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
    }
}
