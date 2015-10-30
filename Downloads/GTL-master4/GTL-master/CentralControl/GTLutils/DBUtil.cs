using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace CentralControl
{
    public class DBUtil
    {
        //private static String ConnectionString = "data source = LAB229\\SQLEXPRESS;initial catalog = gtltest; user id = gtltest;password = jiaoda";
        private static String ConnectionString = ConfigurationSettings.AppSettings["connectionstring"];
        private static List<string> columnname;
        private static SqlConnection getConnection() 
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConnectionString;
            return conn;
        }

        public static ArrayList getTableList() 
        {
            ArrayList list = new ArrayList();
            SqlConnection conn = getConnection();
            try
            {
                conn.Open();
                DataTable tables = conn.GetSchema("Tables");
                foreach (DataRow row in tables.Rows)
                {
                    list.Add(row[2].ToString());
                }  
            }
            catch (Exception ex)
            {
            
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return list;
        }

        public static ArrayList getTableColumns(String tableName) 
        {
            ArrayList list = new ArrayList();
            SqlConnection conn = getConnection();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select Name FROM SysColumns Where id=Object_Id('" + tableName + "')", conn);
                SqlDataReader objReader = cmd.ExecuteReader();
                while (objReader.Read())
                {
                    list.Add(objReader[0].ToString());

                }  
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return list;
        }

        public List<List<string>> executeQueryCmd(string cmdStr) 
        {
            SqlConnection conn = getConnection();
            List<List<string>> list = new List<List<string>>();
            String[] ele;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                columnname = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columnname.Add(reader.GetName(i));
                }
                while (reader.Read())
                {
                    List<string> col=new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++) 
                    {
                        col.Add(reader[i].ToString());
                    }
                    list.Add(col);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return list;
        }

        public List<string> getcloumnname()
        {
            return columnname;
        }

        public static int executedNonQueryCmd(string cmdStr) 
        {
            SqlConnection conn = getConnection();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                return cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return -1;
        }
    }
}
