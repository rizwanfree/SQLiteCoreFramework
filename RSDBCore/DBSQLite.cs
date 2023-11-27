using Microsoft.Data.Sqlite;
using System.Data;
using System.Reflection;
using System.Reflection.Metadata;

namespace RSDBCore;

public class DBSQLite
{
    private string _connstring;

    public DBSQLite()
    {
        _connstring = "Data Source=data.db";
    }

    public object GetScalarValue(string query)
    {
        object value = null;

        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                value = cmd.ExecuteScalar();
            }
        }
        return value;
    }

    public object GetScalarValue(string query, DBParameter parameter)
    {
        object value = null;

        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue(parameter.Parameter, parameter.Value);
                value = cmd.ExecuteScalar();
            }
        }
        return value;
    }

    public object GetScalarValue(string query, DBParameter[] parameters)
    {
        object? value = null;

        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                foreach (DBParameter para in parameters)
                {
                    cmd.Parameters.AddWithValue(para.Parameter, para.Value);
                }
                
                value = cmd.ExecuteScalar();
            }
        }
        
        return value;
    }

    public DataTable GetDataList(string query)
    {
        DataTable dt = new DataTable();
        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqliteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
        }
        return dt;
    }

    public DataTable GetDataList(string query, DBParameter parameter)
    {
        DataTable dt = new DataTable();
        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue(parameter.Parameter, parameter.Value);                
                SqliteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
        }
        return dt;
    }

    public DataTable GetDataList(string query, DBParameter[] parameters)
    {
        DataTable dt = new DataTable();
        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();

                // Parameters

                foreach (var para in parameters)
                {
                    cmd.Parameters.AddWithValue(para.Parameter, para.Value);
                }
                SqliteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
        }
        return dt;
    }

    public DataTable GetDataList(string query, object obj)
    {
        DataTable dt = new DataTable();
        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();

                // Parameters
                Type type = obj.GetType();
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                PropertyInfo[] properties = type.GetProperties(flags);

                foreach (var property in properties)
                {
                    cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj, null));
                }
                SqliteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
        }
        return dt;
    }

    public void InsertOrUpdateRecord(string query, object obj)
    {
        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();

                //Parameters
                Type type = obj.GetType();
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                PropertyInfo[] properties = type.GetProperties(flags);

                foreach (var property in properties)
                {
                    cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj, null));
                }
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void InsertOrUpdateRecord(string query, DBParameter[] parameters)
    {
        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();
                //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

                //Parameters

                foreach (var para in parameters)
                {
                    cmd.Parameters.AddWithValue(para.Parameter, para.Value);
                }
                cmd.ExecuteNonQuery();
                
            }
            
        }
    }

    public void DeleteRecord(string query, DBParameter para)
    {
        using (SqliteConnection conn = new SqliteConnection(_connstring))
        {
            using (SqliteCommand cmd = new SqliteCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();

                //Parameters

                cmd.Parameters.AddWithValue(para.Parameter, para.Value);
                
                cmd.ExecuteNonQuery();
                
            }
        }
    }
}
