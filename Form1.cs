//  // ////// ////// ////// /////   ////  //   //
// //    //     //     //   //     //  // /// ///
////     //     //     //   ////   ////// // / //
// //    //     //     //   //     //  // //   //
//  // //////   //     //   /////  //  // //   //

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Organizer
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Подключение к базе 
        /// </summary>
        SQLite sql;

        /// <summary>
        /// запрос
        /// </summary>
        string Query;
        /// <summary>
        /// Текущая таблица
        /// </summary>
        string SelectTable = "Event";
        /// <summary>
        /// Выбранная для удаления строка
        /// </summary>
        int RowSelect = 0;
        /// <summary>
        /// Режим, добавление или изменение (true - добавление)
        /// </summary>
        bool AddEdit = true;

        object[][] searchItems;

        public Form1()
        {
            InitializeComponent();
            Event.BackColor = Color.FromArgb(251, 171, 114);

            try
            {
                sql = new SQLite("sqlite.db");
            }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
                Application.Exit();
            }

            searchItems = new object[][] {
                new object[] {
                    "Событие",
                    "Дата",
                    "Время"
                },
                new object[] {
                    "Фамилия",
                    "Имя",
                    "Отчество",
                    "Дата рождения",
                    "Почта",
                    "Мобильный",
                    "Домашний",
                    "Рабочий",
                    "Место работы",
                    "Должность"
                },
                new object[] {
                    "Фамилия",
                    "Номер заказа",
                    "Продукт",
                    "Дата приема",
                    "Дата выполнения",
                    "Мастер",
                    "Цена"
                },
                new object[] {
                    "Фамилия",
                    "Номер цеха",
                    "Номер продукта",
                    "Количество продукта",
                    "День"
                }
            };

            comboBox1.Items.AddRange(searchItems[0]);

            comboBox1.SelectedIndex = 0;

            UpdateTable();
        }

        /// <summary>
        /// Метод для обнавления таблицы, принимает таблицу
        /// </summary>
        public void UpdateTable()
        {
            sql.getTable("SELECT * FROM "+SelectTable, dataGridView1);

            if (SelectTable == "Orders")
                    Calc23();
            else if (SelectTable == "Assembly")
                Calc2();
            else panel1.Visible = false;
        }

        /// <summary>
        /// Удаление записей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0) return;
            int ID = Convert.ToInt32(dataGridView1.Rows[RowSelect].Cells[0].Value);
            sql.execute("DELETE FROM " + SelectTable + " WHERE ID = " + ID);
            UpdateTable();
        }

        /// <summary>
        /// Возврат выделенной строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowSelect = e.RowIndex;
        }
        
        /// <summary>
        /// Поиск записей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, EventArgs e)
        {
            sql.getTable("SELECT * FROM " + SelectTable + " WHERE [" + comboBox1.SelectedItem + "] = " + "'" + textBox4.Text + "'", dataGridView1);
        }

        /// <summary>
        /// Отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            EventPanel.Visible = false;
            AddressBookPanel.Visible = false;
            AssemblyPanel.Visible = false;
            OrderPanel.Visible = false;
            Clear();
        }

        /// <summary>
        /// Очистка текстовых полей
        /// </summary>
        private void Clear()
        {
            switch (SelectTable)
            {
                case "Event":
                    EventName4.Text = "";
                    Comment4.Text = "";
                    break;
                case "AddressBook":
                    LName6.Text = "";
                    FName6.Text = "";
                    Patr6.Text = "";
                    Email6.Text = "";
                    MPhone6.Text = "";
                    HPhone6.Text = "";
                    WPhone6.Text = "";
                    Company6.Text = "";
                    Position6.Text = "";
                    break;
                case "Orders":
                    LName23.Text = "";
                    NumOrd23.Text = "";
                    Product23.Text = "";
                    LNameM23.Text = "";
                    Price23.Text = "";
                    break;
                case "Assembly":
                    LName2.Text = "";
                    NumAssembly2.Text = "";
                    NumProduct2.Text = "";
                    CountProduct2.Text = "";
                    Day2.SelectedIndex = 0;
                    break;
            }
        }
        
        /// <summary>
        /// Добавление записи в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            if (AddEdit)
            {
                switch (SelectTable)
                {
                    case "Event":
                        Query = (string.Format("INSERT INTO Event ([Событие],[Дата],[Время], [Комментарий]) VALUES ('{0}','{1}','{2}','{3}')"
                                                        , EventName4.Text, Date4.Value.Date.ToString("yyyy-MM-dd"), Time4.Value.TimeOfDay.ToString("t"), Comment4.Text));
                        break;
                    case "AddressBook":
                        Query = (string.Format("INSERT INTO AddressBook ([Фамилия],[Имя], [Отчество], [Дата рождения], [Почта], [Мобильный], [Домашний], [Рабочий], [Место работы], [Должность]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')"
                                                         , LName6.Text, FName6.Text, Patr6.Text, DofBirth6.Value.Date.ToString("yyyy-MM-dd"), Email6.Text, MPhone6.Text, HPhone6.Text, WPhone6.Text, Company6.Text, Position6.Text));
                        break;
                    case "Orders":
                        Query = (string.Format("INSERT INTO Orders ([Фамилия],[Номер заказа], [Продукт], [Дата приема], [Дата выполнения], [Мастер], [Цена]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')"
                                                          , LName23.Text, NumOrd23.Text, Product23.Text, DateF23.Value.Date.ToString("yyyy-MM-dd"), DateL23.Value.Date.ToString("yyyy-MM-dd"), LNameM23.Text, Price23.Text));
                        break;
                    case "Assembly":
                        Query = (string.Format("INSERT INTO Assembly ([Фамилия], [Номер цеха], [Номер продукта], [Количество продукта], [День]) VALUES ('{0}','{1}','{2}','{3}','{4}')"
                                                         , LName2.Text, NumAssembly2.Text, NumProduct2.Text, CountProduct2.Text, Day2.SelectedItem));
                        break;
                }
                if (!sql.execute(Query))
                 MessageBox.Show("Запись не добавлена!");
            }
            else
            {
                switch (SelectTable)
                {
                    case "Event":
                        Query = (string.Format("Update Event set [Событие]='{0}', [Дата]='{1}', [Время]='{2}', [Комментарий]='{3}' where id={4}"
                                                           , EventName4.Text, Date4.Value.Date.ToString("yyyy-MM-dd"), Time4.Value.TimeOfDay.ToString("t"), Comment4.Text, Convert.ToInt32(dataGridView1.Rows[RowSelect].Cells[0].Value)));
                        break;
                    case "AddressBook":
                        Query = (string.Format("Update AddressBook set [Фамилия]='{0}', [Имя]='{1}', [Отчество]='{2}', [Дата рождения]='{3}', [Почта]='{4}', [Мобильный]='{5}', [Домашний]='{6}', [Рабочий]='{7}', [Место работы]='{8}', [Должность]='{9}' where id={10}"
                                                          , LName6.Text, FName6.Text, Patr6.Text, DofBirth6.Value.Date.ToString("yyyy-MM-dd"), Email6.Text, MPhone6.Text, HPhone6.Text, WPhone6.Text, Company6.Text, Position6.Text, Convert.ToInt32(dataGridView1.Rows[RowSelect].Cells[0].Value)));
                        break;
                    case "Orders":
                        Query = (string.Format("Update Orders set [Фамилия]='{0}', [Номер заказа]='{1}', [Продукт]='{2}', [Дата приема]='{3}', [Дата выполнения]='{4}', [Мастер]='{5}', [Цена]='{6}' where id={7}"
                                                          , LName23.Text, NumOrd23.Text, Product23.Text, DateF23.Value.Date.ToString("yyyy-MM-dd"), DateL23.Value.Date.ToString("yyyy-MM-dd"), LNameM23.Text, Price23.Text, Convert.ToInt32(dataGridView1.Rows[RowSelect].Cells[0].Value)));
                        break;
                    case "Assembly":
                        Query = (string.Format("Update Assembly set [Фамилия]='{0}', [Номер цеха]='{1}', [Номер продукта]='{2}', [Количество продукта]='{3}', [День]='{4}' where id={5}"
                                                         , LName2.Text, NumAssembly2.Text, NumProduct2.Text, CountProduct2.Text, Day2.SelectedText, Convert.ToInt32(dataGridView1.Rows[RowSelect].Cells[0].Value)));
                        break;
                }
                if (!sql.execute(Query))
                    MessageBox.Show("Запись не обновлена!");
            }
            UpdateTable();
            Clear();
        }

        /// <summary>
        /// Показывает панель добавления записей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, EventArgs e)
        {
            Clear();
            AddEdit = true;

            switch (SelectTable)
            {
                case "Event":
                    EventPanel.Visible = true;
                    break;
                case "AddressBook":
                    AddressBookPanel.Visible = true;
                    break;
                case "Orders":
                    OrderPanel.Visible = true;
                    break;
                case "Assembly":
                    AssemblyPanel.Visible = true;
                    Day2.SelectedIndex = 0;
                    DayX2.SelectedIndex = 0;
                    break;
            }
        }
        /// <summary>
        /// Показывает панель изменения записей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, EventArgs e)
        {
            AddEdit = false;
            switch (SelectTable)
            {
                case "Event":
                    EventPanel.Visible = true;
                    EventName4.Text = dataGridView1.Rows[RowSelect].Cells[1].Value.ToString();
                    Date4.Value = DateTime.Parse(dataGridView1.Rows[RowSelect].Cells[2].Value.ToString());
                    Time4.Value = DateTime.Parse(dataGridView1.Rows[RowSelect].Cells[3].Value.ToString());
                    Comment4.Text = dataGridView1.Rows[RowSelect].Cells[4].Value.ToString();
                    break;
                case "AddressBook":
                    AddressBookPanel.Visible = true;
                    LName6.Text = dataGridView1.Rows[RowSelect].Cells[1].Value.ToString();
                    FName6.Text = dataGridView1.Rows[RowSelect].Cells[2].Value.ToString();
                    Patr6.Text = dataGridView1.Rows[RowSelect].Cells[3].Value.ToString();
                    DofBirth6.Value = DateTime.Parse(dataGridView1.Rows[RowSelect].Cells[4].Value.ToString());
                    Email6.Text = dataGridView1.Rows[RowSelect].Cells[5].Value.ToString();
                    MPhone6.Text = dataGridView1.Rows[RowSelect].Cells[6].Value.ToString();
                    HPhone6.Text = dataGridView1.Rows[RowSelect].Cells[7].Value.ToString();
                    WPhone6.Text = dataGridView1.Rows[RowSelect].Cells[8].Value.ToString();
                    Company6.Text = dataGridView1.Rows[RowSelect].Cells[9].Value.ToString();
                    Position6.Text = dataGridView1.Rows[RowSelect].Cells[10].Value.ToString();
                    break;
                case "Orders":
                    OrderPanel.Visible = true;
                    LName23.Text = dataGridView1.Rows[RowSelect].Cells[1].Value.ToString();
                    NumOrd23.Text = dataGridView1.Rows[RowSelect].Cells[2].Value.ToString();
                    Product23.Text = dataGridView1.Rows[RowSelect].Cells[3].Value.ToString();
                    DateF23.Value = DateTime.Parse(dataGridView1.Rows[RowSelect].Cells[4].Value.ToString());
                    DateL23.Value = DateTime.Parse(dataGridView1.Rows[RowSelect].Cells[5].Value.ToString());
                    LNameM23.Text = dataGridView1.Rows[RowSelect].Cells[6].Value.ToString();
                    Price23.Text = dataGridView1.Rows[RowSelect].Cells[7].Value.ToString();
                    break;
                case "Assembly":
                    AssemblyPanel.Visible = true;
                    LName2.Text = dataGridView1.Rows[RowSelect].Cells[1].Value.ToString();
                    NumAssembly2.Text = dataGridView1.Rows[RowSelect].Cells[2].Value.ToString();
                    NumProduct2.Text = dataGridView1.Rows[RowSelect].Cells[3].Value.ToString();
                    CountProduct2.Text = dataGridView1.Rows[RowSelect].Cells[4].Value.ToString();
                    Day2.SelectedItem = dataGridView1.Rows[RowSelect].Cells[5].Value.ToString();
                    DayX2.SelectedIndex = 0;
                    break;
            }
        }

        private void UpdateData_Click(object sender, EventArgs e)
        {
            UpdateTable();
        }
        
        /// <summary>
        /// Изменение коллекции ComboBox
        /// </summary>
        /// <param name="i">Индекс коллекции</param>
        private void Clear_ComboBox(short i)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(searchItems[i]);
            comboBox1.SelectedIndex = 0;
        }
        /// <summary>
        /// Сброс стилей
        /// </summary>
        private void Reset()
        {
            Event.BackColor = Color.FromArgb(244, 179, 80);
            AddressBook.BackColor = Color.FromArgb(244, 179, 80);
            Variant23.BackColor = Color.FromArgb(244, 179, 80);
            Variant2.BackColor = Color.FromArgb(244, 179, 80);

            EventPanel.Visible = false;
            OrderPanel.Visible = false;
            AssemblyPanel.Visible = false;
            AddressBookPanel.Visible = false;
        }

        private void Event_Click(object sender, EventArgs e)
        {
            SelectTable = "Event";
            Reset();
            Event.BackColor = Color.FromArgb(251, 171, 114);
            Clear_ComboBox(0);
            UpdateTable();
        }

        private void AddressBook_Click(object sender, EventArgs e)
        {
            SelectTable = "AddressBook";
            Reset();
            AddressBook.BackColor = Color.FromArgb(251, 171, 114);
            Clear_ComboBox(1);
            UpdateTable();
        }

        private void Orders_Click(object sender, EventArgs e)
        {
            SelectTable = "Orders";
            Reset();
            Variant23.BackColor = Color.FromArgb(251, 171, 114);
            Clear_ComboBox(2);
            UpdateTable();
        }

        private void Variant2_Click(object sender, EventArgs e)
        {
            SelectTable = "Assembly";
            Reset();
            Variant2.BackColor = Color.FromArgb(251, 171, 114);
            Clear_ComboBox(3);
            UpdateTable();
        }

        private void Show23_Click(object sender, EventArgs e)
        {
            long Count = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (Search23.Text == dataGridView1.Rows[i].Cells[6].Value.ToString())
                    Count++;
            }
            label19.Text = Count.ToString();
        }

        private void Delete23_Click(object sender, EventArgs e)
        {
            sql.execute("DELETE FROM Orders WHERE [Мастер] = '" + Search23.Text + "'");
            UpdateTable();
        }

        private void Calc23()
        {
            panel1.Visible = true;
            label17.Text = 0.ToString();
            label15.Text = 0.ToString();

            label18.Text = "Самый дорогой заказ:";
            label16.Text = "Средняя стоимость заказов:";
            label29.Text = "";

            if (dataGridView1.RowCount == 0) return;

            long MaxPrice = 0, AverageCost = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (MaxPrice < long.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString()))
                    MaxPrice = long.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());

                AverageCost += long.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
            }
            label15.Text = (AverageCost / dataGridView1.RowCount).ToString();
            label17.Text = MaxPrice.ToString();
        }

        /// <summary>
        /// 2
        /// </summary>
        private void Calc2() {
            panel1.Visible = true;
            label18.Text = "Общее количество изд:";
            label16.Text = "Лучше всех:";
            label15.Text = "";

            var reader = sql.getReader("select sum([Количество продукта]) from Assembly");
            while (reader.Read())
                label17.Text = reader.GetInt32(0).ToString();
            reader.Close();

            var reader2 = sql.getReader("select [Фамилия], [День], max([Количество продукта]) from Assembly");
            while (reader2.Read())
                label29.Text = reader2.GetString(0).ToString() + " в " + reader2.GetString(1).ToString();
            reader2.Close();
        }
        private void Execute2_Click(object sender, EventArgs e) {
            sql.execute("update Assembly set [Количество продукта]=[Количество продукта] * " + X2.Text +" where [День]='" +DayX2.SelectedItem+"'");
            UpdateTable();
        }
    }
}