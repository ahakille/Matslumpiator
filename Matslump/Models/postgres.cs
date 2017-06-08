using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Matslump.Models
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
            _conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {

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
                return null;
                
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
               
            }

            finally
            {
                _conn.Close();
            }
        }
    }
}