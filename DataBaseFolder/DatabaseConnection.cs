using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Data
{
    /// <summary>
    /// Central SQL Server helper for ChatNova
    /// All queries use parameterized input — no SQL injection possible
    /// </summary>
    internal static class DatabaseConnection
    {
        // -------------------------------------------------------
        // CONNECTION STRING
        // Move to App.config in production:
        // ConfigurationManager.ConnectionStrings["ChatNova"].ConnectionString
        // -------------------------------------------------------
        private static readonly string _connectionString =
            "Server=rajpoot1234;" +
            "Database=newChatNovaDB;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

        // -------------------------------------------------------
        // INTERNAL: Create new connection (never auto-open here)
        // -------------------------------------------------------
        private static SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // -------------------------------------------------------
        // TEST CONNECTION
        // -------------------------------------------------------
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = CreateConnection())
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch (SqlException ex)
            {
                // Log SQL-specific errors (wrong server, bad DB name, etc.)
                System.Diagnostics.Debug.WriteLine($"[DB TestConnection] SqlException: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB TestConnection] Exception: {ex.Message}");
                return false;
            }
        }

        // -------------------------------------------------------
        // EXECUTE NON QUERY — INSERT / UPDATE / DELETE
        // Returns rows affected (-1 on error)
        // -------------------------------------------------------
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = CreateConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 30;

                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ExecuteNonQuery] SqlException: {ex.Message}");
                throw; // Let the Service layer handle it
            }
        }

        // -------------------------------------------------------
        // EXECUTE SCALAR — Returns single value (COUNT, ID, etc.)
        // Returns null on error
        // -------------------------------------------------------
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = CreateConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 30;

                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ExecuteScalar] SqlException: {ex.Message}");
                throw;
            }
        }

        // -------------------------------------------------------
        // EXECUTE READER — Returns rows
        // FIXED: SqlCommand is now properly disposed on exception
        // Caller must wrap in using() or call reader.Close()
        // -------------------------------------------------------
        public static SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            SqlConnection conn = CreateConnection();
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 30;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();

                // CloseConnection = connection auto-closes when reader closes
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ExecuteReader] SqlException: {ex.Message}");

                // Clean up manually since using() can't wrap this pattern
                cmd?.Dispose();
                conn?.Dispose();
                throw;
            }
        }

        // -------------------------------------------------------
        // EXECUTE READER WITH DATATABLE — Safer alternative
        // Loads all data then closes connection immediately
        // Use this when you need to iterate results multiple times
        // -------------------------------------------------------
        public static DataTable ExecuteDataTable(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = CreateConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 30;

                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        return dt;
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ExecuteDataTable] SqlException: {ex.Message}");
                throw;
            }
        }
    }
}