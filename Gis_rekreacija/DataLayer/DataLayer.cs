using Npgsql;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gis_rekreacija.DataLayer
{
    public static class DataLayer
    {
        public static DataRowCollection GetLayerColumns(VectorLayer layer) {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();

            string tableName = layer.DataSource.GetFeature(1).Table.TableName;
            string sqlColumns = "SELECT * FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '" + tableName + "'";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlColumns, conn);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            conn.Close();
            return dt.Rows;
        }

        public static DataRowCollection GetDistinctValues(VectorLayer layer, string column_name)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();
            string sql = " SELECT distinct "+column_name+" FROM " + layer.DataSource.GetFeature(1).Table.TableName;
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt.Rows;
        }
    }
}
