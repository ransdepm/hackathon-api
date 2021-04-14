using Hackathon.Data.Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Hackathon.Data
{
    public abstract class DataAccessBase : IDisposable
    {
        #region Fields  
        private MySqlConnection connection = null;
        #endregion

        #region Properties  
        public MySqlConnection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }
        #endregion

        #region Constructors
        public DataAccessBase(string connectionString)
        {
            this.connection = new MySqlConnection(connectionString);
        }

        public DataAccessBase(MySqlConnection connection)
        {
            this.connection = connection;
        }
        #endregion

        #region ADO operations

        /// <summary>  
        /// Raterss SqlDataReader for stored procedure with optional paramets list  
        /// </summary>  
        /// <param name="procedureName"></param>  
        /// <param name="parameters"></param>  
        /// <returns></returns>  
        public MySqlDataReader ExecuteReader(string procedureName, IDictionary<string, IConvertible> parameters = null)
        {
            return this.GetCommand(procedureName, parameters).ExecuteReader();
        }

        /// <summary>  
        /// Raterss DataTable for stored procedure with optional paramets list  
        /// </summary>  
        /// <param name="procedureName"></param>  
        /// <param name="parameters"></param>  
        /// <returns></returns>  
        public DataTable ExecuteDataTable(string procedureName, IDictionary<string, IConvertible> parameters = null)
        {
            DataTable dataTable = new DataTable();
            this.GetAdapter(procedureName, parameters).Fill(dataTable);
            return dataTable;
        }

        /// <summary>  
        /// Raterss DataSet for stored procedure with optional paramets list  
        /// </summary>  
        /// <param name="procedureName"></param>  
        /// <param name="parameters"></param>  
        /// <returns></returns>  
        public DataSet ExecuteDataSet(string procedureName, IDictionary<string, IConvertible> parameters = null)
        {
            DataSet dataSet = new DataSet();
            this.GetAdapter(procedureName, parameters).Fill(dataSet);
            return dataSet;
        }

        public void InsertData(string procedureName, IDictionary<string, IConvertible> parameters = null)
        {
            try
            {
                this.GetCommand(procedureName, parameters).ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new MySQLServerException(ex.Message);
            }
        }

        public async Task RunScriptAsync(string script)
        {
            try
            {
                await this.GetScript(script).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new MySQLServerException(ex.Message);
            }
        }

        public void RunCommand(string command)
        {
            try
            {
                this.GetCommandText(command).ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new MySQLServerException(ex.Message);
            }
        }

        public int InsertDataReturnId(string procedureName, IDictionary<string, IConvertible> parameters = null)
        {
            int recordID;
            try
            {
                MySqlCommand command = this.GetCommand(procedureName, parameters);
                MySqlParameter outputIdParam = new MySqlParameter("@new_record", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                command.ExecuteNonQuery();

                recordID = int.Parse(outputIdParam.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new MySQLServerException(ex.Message);
            }
            return recordID;
        }

        public Guid InsertDataRatersGuid(string procedureName, IDictionary<string, IConvertible> parameters = null)
        {
            Guid recordID;
            try
            {
                MySqlCommand command = this.GetCommand(procedureName, parameters);
                MySqlParameter outputIdParam = new MySqlParameter("@new_record", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                command.ExecuteNonQuery();

                recordID = Guid.Parse(outputIdParam.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new MySQLServerException(ex.Message);
            }

            return recordID;
        }

        #region Command preparing methods  

        /// <summary>  
        /// Creates SqlAdapter instance for the stored procedure with optional paramets  
        /// </summary>  
        /// <param name="procedureName"></param>  
        /// <param name="parameters"></param>  
        /// <returns></returns>  
        private MySqlDataAdapter GetAdapter(string procedureName, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
        {
            return new MySqlDataAdapter(this.GetCommand(procedureName, parameters));
        }

        /// <summary>  
        /// Creates SqlAdapter instance for the command  
        /// </summary>  
        /// <param name="command"></param>  
        /// <returns></returns>  
        private MySqlDataAdapter GetAdapter(MySqlCommand command)
        {
            return new MySqlDataAdapter(command);
        }

        /// <summary>  
        /// Prepares MySqlCommand for stored procedure with oprional parameters list  
        /// </summary>  
        /// <param name="procedureName"></param>  
        /// <param name="parameters"></param>  
        /// <returns></returns>  
        private MySqlCommand GetCommand(string procedureName, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null)
        {
            MySqlCommand command = new MySqlCommand(procedureName);
            command.CommandType = CommandType.StoredProcedure;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = this.connection;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(new MySqlParameter(param.Key, param.Value));
                }
            }
            return command;
        }

        private MySqlScript GetScript(string sqlScript)
        {
            MySqlScript script = new MySqlScript(sqlScript);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            script.Connection = this.connection;
            return script;
        }

        private MySqlCommand GetCommandText(string comm)
        {
            MySqlCommand command = new MySqlCommand(comm);
            command.CommandType = CommandType.Text;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = this.connection;
            return command;
        }

        #endregion

        #endregion

        #region IDisposable implementation  

        public void CloseConnection()
        {
            if (this.connection != null)
            {
                this.connection.Close();
                this.connection.Dispose();
            }
        }

        /// <summary>  
        /// Dispose DataAccessLayer instance and closes database connection  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                    this.connection.Dispose();
                }
            }
        }

        #endregion
    }
}
