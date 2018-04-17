using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using Matslumpiator.Tools;
using Microsoft.Extensions.Configuration;

namespace Matslumpiator.Services
{
    public class postgres
    {
        private NpgsqlConnection _conn;
        private NpgsqlCommand _cmd;
        private NpgsqlDataReader _dr;
        private DataTable _table;
   

        public static List<NpgsqlParameter> list { get; set; }
        public postgres()
        {
            
            _conn = new NpgsqlConnection("Server=localhost;port=5433; user id=matslumpiator;Password=mat123;database=matslumpiator;");
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                LogWriter log = new LogWriter(ex.Message);
            }
        }
  
        public DataTable SqlQuery(string sqlquery, List<NpgsqlParameter> parametrar)
        {
         
            try

            {
                _table = new DataTable();
                _cmd = new NpgsqlCommand(sqlquery, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                _dr = _cmd.ExecuteReader();
                _table.Load(_dr);
                return _table;

            }
            catch (Exception ex)
            {
                LogWriter log = new LogWriter(ex.Message);
                return null;
                
            }

            finally
            {
                _conn.Close();
            }
        }
        public bool SqlQueryExist(string sqlquery, List<NpgsqlParameter> parametrar)
        {

            try

            {
                
                _cmd = new NpgsqlCommand(sqlquery, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                bool check =(bool)_cmd.ExecuteScalar();

              
                return check;

            }
            catch (Exception ex)
            {
                LogWriter log = new LogWriter(ex.Message);
                return false;

            }

            finally
            {
                _conn.Close();
            }
        }
        public int SqlQueryString(string sqlquery, List<NpgsqlParameter> parametrar)
        {

            try

            {

                _cmd = new NpgsqlCommand(sqlquery, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                int check = (int)_cmd.ExecuteScalar();


                return check;

            }
            catch (Exception ex)
            {
                LogWriter log = new LogWriter(ex.Message);
                return 0;

            }

            finally
            {
                _conn.Close();
            }
        }

        public void SqlNonQuery(string sqlquery, List<NpgsqlParameter> parametrar)
        {
            
            try
            {
                _cmd = new NpgsqlCommand(sqlquery, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogWriter log = new LogWriter(ex.Message);
            }

            finally
            {
                _conn.Close();
            }
        }
    }
}