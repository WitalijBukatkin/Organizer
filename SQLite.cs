//  // ////// ////// ////// /////   ////  //   //
// //    //     //     //   //     //  // /// ///
////     //     //     //   ////   ////// // / //
// //    //     //     //   //     //  // //   //
//  // //////   //     //   /////  //  // //   //

using System;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;

namespace Organizer {
    class SQLite {
        SQLiteConnection connection;

        /// <summary>
        /// Конструктор SQL
        /// </summary>
        /// <param name="sqlpath">Путь к файлу</param>
        public SQLite(string sqlpath) {
            connection = new SQLiteConnection("Data Source=" + sqlpath + "; Version=3");
            connection.Open();
        }

        /// <summary>
        /// Ничего не возращающий запрос
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns>Если все успешно тогда true</returns>
        public bool execute(string query) {
            try {
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Возращает выборку данных
        /// </summary>
        /// <param name="query">Запрос на выборку</param>
        /// <returns>Взращаю итератор с данными</returns>
        public SQLiteDataReader getReader(string query) {
            SQLiteCommand command = new SQLiteCommand(query, connection);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Заполняю таблицу в соответствии с запросом
        /// </summary>
        /// <param name="query">Запрос на выборку таблицы</param>
        /// <param name="table">Сама таблица</param>
        /// <returns>Если все нормально тогда true</returns>
        public bool getTable(string query, DataGridView table) {
            try {
                SQLiteCommand sqlCommand = new SQLiteCommand(query, connection);
                sqlCommand.ExecuteNonQuery();

                DataTable dataTable = new DataTable();
                SQLiteDataAdapter sqlAdapter = new SQLiteDataAdapter(sqlCommand);

                sqlAdapter.Fill(dataTable);
                table.DataSource = dataTable;
                return true;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}