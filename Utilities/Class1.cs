using System.Data.SqlClient;
using Utilities;

public class PasswordManager
{
    private string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Izakaya;User ID=sa5;Password=sa5;Encrypt=False;Trust Server Certificate=True";

    public void ChangeUserPassword(string username, string newPassword)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // 從資料庫中取得使用者的 salt
            string salt = GetUserSaltFromDatabase(username);

            // 使用新的密碼和原始的 salt 計算新的密碼雜湊值
            string newHashedPassword = HashHalper.ToSHA256(newPassword, salt);

            // 更新資料庫中的密碼
            UpdateUserPasswordInDatabase(username, newHashedPassword);
        }
    }

    private string GetUserSaltFromDatabase(string username)
    {
        string salt = "";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT Salt FROM Members WHERE Username = @Username";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        salt = reader["Salt"].ToString();
                    }
                }
            }
        }

        return salt;
    }

    private void UpdateUserPasswordInDatabase(string username, string hashedPassword)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "UPDATE Members SET PasswordHash = @PasswordHash WHERE Username = @Username";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                command.Parameters.AddWithValue("@Username", username);
                command.ExecuteNonQuery();
            }
        }
    }
}
