using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace DatabaseAccessLibrary
{
    public class DatabaseManager
    {
        // Поле для хранения строки подключения к базе данных
        private string _connectionString;

        // Конструктор класса, принимающий строку подключения к базе данных
        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Метод для выполнения SQL-запроса и возвращения результатов в виде DataView
        public DataView ExecuteQuery(string query)
        {
            DataView result = null; // переменная для хранения результата запроса

            // Создание и запуск нового потока для выполнения запроса
            Thread queryThread = new Thread(() =>
            {
                try
                {
                    // Выполнение запроса и сохранение результата
                    result = ExecuteQueryInternal(query);
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке в консоль или запись в лог
                    Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
                }
            });

            queryThread.Start(); // Запуск потока
            queryThread.Join(); // Ожидание завершения потока

            return result; // Возвращение результата запроса
        }

        // Внутренний метод для выполнения запроса, используемый в многопоточной среде
        private DataView ExecuteQueryInternal(string query)
        {
            // Использование блока using для автоматического освобождения ресурсов
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet(); // Создание DataSet для хранения результатов
                        adapter.Fill(dataSet); // Заполнение DataSet данными из базы данных
                        return dataSet.Tables[0].DefaultView; // Возврат DataView первой таблицы в DataSet
                    }
                }
            }
        }
    }
}
