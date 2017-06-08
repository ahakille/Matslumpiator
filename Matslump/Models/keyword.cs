using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Matslump.Models
{
    public class keyword
    {

        public int id { get; set; }
        public string name { get; set; }
        public keyword()
        {

        }
        public List<keyword> getMatTyp(string psql)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<keyword> mt = new List<keyword>();
            dt = m.SqlQuery(psql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@idag", DateTime.Now)
            });
            foreach (DataRow dr in dt.Rows)
            {
                keyword t = new keyword();
                t.id = Convert.ToInt16(dr["pk_id"].ToString());
                t.name = dr["name"].ToString();
                mt.Add(t);
            }
            return mt;
        }
        public override string ToString()
        {
            return name + " ";
        }
    }
}