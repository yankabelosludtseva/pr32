using System.Data;
using System.Data.SqlClient;

namespace VinylRecordsApplication.Classes
{
    public class DBConnection
    {
        public static DataTable Connection(string SQL) {
            // Создаём локальную таблицу данных
            DataTable dataTable = new DataTable("Datatable");
            // Подключаемся к серверу
            SqlConnection sqlConnection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=VinylRecords;Trusted_Connection=True;TrustServerCertificate=True;");
            // Открываем соединение
            sqlConnection.Open();
            // Создаём команду
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            // Указываем SQL текст
            sqlCommand.CommandText = SQL;
            // Создаём адаптер
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            // Получаем данные в таблицу
            sqlDataAdapter.Fill(dataTable);
            // Возвращаем данные
            return dataTable;
        }
    }
}