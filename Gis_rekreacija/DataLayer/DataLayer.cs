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
        public static NpgsqlConnection DbConnection;
        public static DataRowCollection GetLayerColumns(VectorLayer layer) {
            if (layer.LayerName.Contains("Selection")) {
                return null;
            }
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

        public static DataRowCollection GetTablesFromDB(string sample) {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();
            string sql = "SELECT table_name FROM information_schema.tables where table_name like '%"+sample+"' ORDER BY table_schema,table_name; ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt.Rows;
        }
        public static void CreateDatabaseConnection()
        {
            DbConnection = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
        }

        public static void OpenConnection()
        {
            if (DbConnection != null)
            {
                DbConnection.Open();
            }
        }

        public static void CloseConnection()
        {
            if (DbConnection != null)
            {
                DbConnection.Close();
            }
        }

        public static string CreateQueryIntersects(params object[] args)
        {
            string sql = "SELECT gid, fclass, name, geom AS _smtmp_ FROM " + args[0] + " WHERE ST_Intersects(ST_SetSRID(ST_GeomFromText('" + args[1] + "'), 3857),geom)";

            return sql;
        }

        public static string CreateQueryDWithin(params object[] args)
        {
            string sql = "SELECT gid, fclass, name, geom AS _smtmp_ FROM " + args[0] + " WHERE ST_DWithin(ST_SetSRID(ST_MakePoint(" + args[1] + ", " + args[2] + "), 3857),geom," + args[3] + ")";

            return sql;
        }
    }
}
