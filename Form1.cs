using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace REA_OOP_Stage_1
{
    public partial class Form1 : Form
    {
        List<Book> books = new List<Book>();
        List<BookToReader> booksToReaders = new List<BookToReader>();
        List<Reader> readers = new List<Reader>();
        List<ReaderToHall> readerToHalls = new List<ReaderToHall>();
        List<Hall> halls = new List<Hall>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private float GetMaxBooksGet()
        { // функция ищем книги, которую чаще всего выдавали
            float maxGets = 1.0F;
            foreach (Book tmp in books.Where(w => w.deleted == false).Select(w => w).ToList())
            { // Идём по циклу всех книг, имеющихся в библиотеке
                var gets = booksToReaders.Where(w => w.BookId == tmp.ID).Count(); // А у книги выбираем все записи когда её выдавали и считаем количество
                if (gets > maxGets) { maxGets = (float)gets; } // Проверяем, больше ли кол-во выдач текущей книги, чем предыдущий максимум
            }
            return maxGets; // Возвращаем максимальное кол-во выдач одной книги
        }

        // процедура для обновления всего связанного с книгами
        private void UpdateBookGrid()
        {
            // чистим таблицу "Книги"
            dataGridView1.Rows.Clear();
            // чистим выпадающий список книг в "Книги у читателей"
            comboBox1.Items.Clear();
            // чистим выпадающий список авторов в "Дополнительная информация"
            comboBox7.Items.Clear();
            // получаем максимальное кол-во выдач одной книги
            var maxGets = GetMaxBooksGet();
            // берём все не удалённые книги и преобразуем их в список
            foreach (Book tmp in books.Where(w => w.deleted == false).Select(w => w).ToList())
            { // идём по списку книг
                // получаем массив данных книги для таблицы
                var book = tmp.ForDataGrid();
                // считаем кол-во свободных экземпляров книги
                var free = tmp.Count - booksToReaders.Where(w => w.BookId == tmp.ID).Where(w => w.ReceiveDate == null).Count();
                // получаем кол-во выдач книги
                var gets = booksToReaders.Where(w => w.BookId == tmp.ID).Count();
                // добавляем дополнительную информацию в список, кол-во свободных экземпляров и рейтинг книги
                book = book.Append(free).ToArray();
                book = book.Append(5*Math.Round(gets / maxGets, 1)).ToArray();
                // Добавляем книгу в таблицу
                dataGridView1.Rows.Add(book);
                // Добавляем книгу в выпадающий список в "Книги у читателей"
                comboBox1.Items.Add(tmp.Title + " ID: " + tmp.ID.ToString());
                // Добавляем автора в выпадающий список в "Дополнительная информация"
                comboBox7.Items.Add(tmp.Author + " ID Книги: " + tmp.ID.ToString());
            }
        }

        // процедура для обновления всего связанного с читателями
        private void UpdateReaderGrid()
        {
            // чистим таблицу "Читатели"
            dataGridView2.Rows.Clear();
            // чистим выпадающий список в "Книги у читателей"
            comboBox2.Items.Clear();
            // чистим выпадающий список в "Читатели в залах"
            comboBox4.Items.Clear();
            // берём всех не удалённых читателй и преобразуем их в список
            foreach (Reader tmp in readers.Where(w => w.deleted == false).Select(w => w).ToList())
            { // идём по списку читателей
                // получаем и сразу добавляем читателя в таблицу
                dataGridView2.Rows.Add(tmp.ForDataGrid());
                // добавляем читателя в выпадающий список в "Книги у читателей"
                comboBox2.Items.Add(tmp.FullName + " ID: " + tmp.ID.ToString());
                // добавляем читателя в выпадающий список в "Читатели в залах"
                comboBox4.Items.Add(tmp.FullName + " ID: " + tmp.ID.ToString());
            }
        }

        // процедура для обновления всего связанного с читальными залами
        private void UpdateHallGrid()
        {
            // чистим таблицу "Читальные залы"
            dataGridView3.Rows.Clear();
            // чистим выпадающий список в "Читатели в залах"
            comboBox3.Items.Clear();
            // чистим выпадающий список в "Дополнительная информация"
            comboBox6.Items.Clear();
            // берём все не удалённых залы и преобразуем в список
            foreach (Hall tmp in halls.Where(w => w.deleted == false).Select(w => w).ToList())
            { // идём по списку залов
                // получаем массив данных зала для таблицы
                var hall = tmp.ForDataGrid();
                // высчитываем количество свободных мест в зале
                var free = tmp.SitCount - readerToHalls.Where(w => w.HallId == tmp.ID).Count();
                // добавляем кол-во свободных мест в массив данных, а массив данных в таблицу
                dataGridView3.Rows.Add(hall.Append(free).ToArray());
                // добавляем зал в выпадающий список в "Читатели в залах"
                comboBox3.Items.Add(tmp.Name + " Зал: " + tmp.HallNum.ToString() + " ID: " + tmp.ID.ToString());
                // добавляем зал в выпадающий список в "Дополнительная информация"
                comboBox6.Items.Add(tmp.Name + " Зал: " + tmp.HallNum.ToString() + " ID: " + tmp.ID.ToString());
            }
        }
        
        // процедура для обновления таблицы "Книги у читателей"
        private void UpdateBooksToReaderGrid()
        {
            // чистим таблицу "Книги у читателей"
            dataGridView4.Rows.Clear();
            // берём все выдачи книг и преобразуем в список
            foreach (BookToReader tmp in booksToReaders.Select(w => w).ToList())
            { // идём по списку выдач книг
                // получаем имя читателя по ID
                var reader = readers.Where(w => w.ID == tmp.ReaderId).First();
                // получаем имя книги по ID
                var book = books.Where(w => w.ID == tmp.BookId).First();
                // если дата возврата книги МЕНЬШЕ даты выдачи - оставить пустую строку, иначе подставить дату возврата
                string receiveDate = tmp.ReceiveDate.GetValueOrDefault() < tmp.IssueDate ? "" : tmp.ReceiveDate.GetValueOrDefault().ToLongDateString();
                // создаём массив данных выдачи книги
                object[] tmpStr = { tmp.ID, reader.FullName + " ID: " + reader.ID.ToString(), book.Title + " ID: " + book.ID.ToString(), tmp.IssueDate.ToLongDateString(), receiveDate };
                // добавляем выдачу книги в таблицу
                dataGridView4.Rows.Add(tmpStr);
            }
        }

        // процедура для обновления таблицы "Читатели в залах"
        private void UpdateReadersToHallGrid()
        {
            // чистим таблицу "Читатели в залах"
            dataGridView5.Rows.Clear();
            // берём все не удалённых записи и преобразуем в список
            foreach (ReaderToHall tmp in readerToHalls.Where(w => w.deleted == false).Select(w => w).ToList())
            { // идём по записям
                // получаем имя читателя по ID
                var reader = readers.Where(w => w.ID == tmp.ReaderId).First();
                // получаем читальный зал по ID
                var hall = halls.Where(w => w.ID == tmp.HallId).First();
                // создаём массив данных для читателей в зале
                object[] tmpStr = { tmp.ID, reader.FullName + " ID: " + reader.ID.ToString(), hall.Name + " ID: " + hall.ID.ToString() };
                // добавляем запись читателя в зал в таблицу
                dataGridView5.Rows.Add(tmpStr);
            }
        }

        // процедура сохранения сериализованных данных в файлы
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Сохранение...");
            toolStripStatusLabel1.Text = "Сохранение...";
            // Объявляем необходимые переменные
            string fname;
            BinaryFormatter bf;
            FileStream fs;
            //Save Book
            fname = "data_book.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, books); // сериализуем данные в файл
            fs.Close(); // закрываем файл

            //Save BookToReaders
            fname = "data_bookToReaders.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, booksToReaders); // сериализуем данные в файл
            fs.Close(); // закрываем файл

            //Save Reader
            fname = "data_readers.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, readers); // сериализуем данные в файл
            fs.Close(); // закрываем файл

            //Save ReaderToHall
            fname = "data_readersToHalls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, readerToHalls); // сериализуем данные в файл
            fs.Close(); // закрываем файл

            //Save Hall
            fname = "data_halls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, halls); // сериализуем данные в файл
            fs.Close(); // закрываем файл
            Console.WriteLine("Сохранено");
            toolStripStatusLabel1.Text = "Сохранено";
        }

        // процеду загрузки и десереализации данных из файлов
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Загрузка...");
            toolStripStatusLabel1.Text = "Загрузка...";
            // объявляем переменные
            string fname;
            BinaryFormatter bf;
            FileStream fs;

            //Load Books
            fname = "data_book.bin"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                books = (List<Book>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (books.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    Book.Index = books.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }

            //Load BooksToReaders
            fname = "data_bookToReaders.bin"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                booksToReaders = (List<BookToReader>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (booksToReaders.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    BookToReader.Index = booksToReaders.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }

            //Save Readers
            fname = "data_readers.bin"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                readers = (List<Reader>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (readers.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    Reader.Index = readers.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }

            //Save ReadersToHalls
            fname = "data_readersToHalls.bin"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                readerToHalls = (List<ReaderToHall>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (readerToHalls.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    ReaderToHall.Index = readerToHalls.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }

            //Save Halls
            fname = "data_halls.bin"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                halls = (List<Hall>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (halls.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    Hall.Index = halls.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }
            Console.WriteLine("Загружено");
            toolStripStatusLabel1.Text = "Загружено";

            UpdateBookGrid(); // обновляем таблицу книг и выпадающие списки, где используются книги
            UpdateBooksToReaderGrid(); // обновляем таблицу выданных книг
            UpdateReaderGrid(); // обновляем таблицу читателей и выпадающие списки, где используются читатели
            UpdateReadersToHallGrid(); // обновляем таблицу записей читателей в читальные залы
            UpdateHallGrid(); // обновляем таблицу читальных залов и выпадающие списки, где используются читальные залы

            numericUpDown3.Value = numericUpDown3.Minimum + Reader.Index; // выставляем рекомендуемое значение для следующего читательского билета
        }

        // процедура выбора книги по клику мышкой из таблицы
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // проверяем, что индекс > 0 т.к. нажатие на Заголовок таблицы может вызвать ошибку в строках ниже
            if (e.RowIndex >= 0)
            {
                // берём индекс из ячейки таблицы и записываем в textBox с ID книги для удаления
                textBox9.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // и так же записываем ID в label для ID редактируемой книги
                label16.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                // по ID книги подгружаем данные и заполняем или поля в форме редактирования книги
                Book tmp = books.Where(w => w.ID == Int32.Parse(label16.Text)).First();
                textBox8.Text = tmp.Title;
                textBox7.Text = tmp.Author;
                maskedTextBox6.Text = tmp.ReleaseYear;
                maskedTextBox5.Text = tmp.BookCode;
                maskedTextBox4.Text = tmp.Count.ToString();
            }

        }

        // процедура выбора читателя по клику мышкой из таблицы
        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // проверяем, что индекс > 0 т.к. нажатие на Заголовок таблицы может вызвать ошибку в строках ниже
            if (e.RowIndex >= 0)
            {
                // берём индекс из ячейки таблицы и записываем в textBox с ID читателя для удаления
                textBox10.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                // и так же записываем ID в label для ID редактируемого читателя
                label19.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();

                // по ID читателя подгружаем данные и заполняем поля в форме для редактирования читателя
                Reader tmp = readers.Where(w => w.ID == Int32.Parse(label19.Text)).First();
                textBox14.Text = tmp.FullName;
                numericUpDown4.Value = tmp.TicketNum;
                dateTimePicker5.Value = tmp.Birthday;
                maskedTextBox8.Text = tmp.Phone;
                textBox11.Text = tmp.Education;
            }

        }

        // процедура выбора читального зала по клику мышкой из таблицы
        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // проверяем, что индекс > 0 т.к. нажатие на Заголовок таблицы может вызвать ошибку в строках ниже
            if (e.RowIndex >= 0)
            {
                // берём индекс из ячейки таблицы и записываем в textBox с ID зала для удаления
                textBox13.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                // и так же записываем ID в label для ID редактируемого читального зала
                label23.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();

                // по ID зала подгружаем данные и заполняем поля в форме для редактирования зала
                Hall tmp = halls.Where(w => w.ID == Int32.Parse(label23.Text)).First();
                textBox20.Text = tmp.Name;
                numericUpDown5.Value = tmp.HallNum;
                textBox17.Text = tmp.Spec;
                numericUpDown8.Value = tmp.SitCount;
            }

        }

        // процедура выбора записи выдачи книги по клику мышки из таблицы
        private void dataGridView4_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // проверяем, что индекс > 0 т.к. нажатие на Заголовок таблицы может вызвать ошибку в строках ниже
            if (e.RowIndex >= 0)
            {
                // берём индекс из ячейки таблицы и записываем в label для ID книги которую планируется вернуть
                label6.Text = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();

                // по ID выдачи подгружаем данные и заполняем поля в форме возврата книги
                BookToReader tmp = booksToReaders.Where(w => w.ID == Int32.Parse(label6.Text)).First();
                textBox5.Text = books.Where(w => w.ID == tmp.BookId).First().Title;
                textBox4.Text = readers.Where(w => w.ID == tmp.ReaderId).First().FullName;
            }

        }

        // процедура выбора записи читателя в зал по клику мышки из таблицы
        private void dataGridView5_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // проверяем, что индекс > 0 т.к. нажатие на Заголовок таблицы может вызвать ошибку в строках ниже
            if (e.RowIndex >= 0)
            {
                // берём индекс из ячейки таблицы и записываем в label для ID записи, которую планируется удалить
                label5.Text = dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString();

                // по ID записи читателя в зал подгружаем данные и заполняем поля в форме выписывания читателя из зала
                ReaderToHall tmp = readerToHalls.Where(w => w.ID == Int32.Parse(label5.Text)).First();
                textBox6.Text = readers.Where(w => w.ID == tmp.ReaderId).First().FullName;
                textBox3.Text = halls.Where(w => w.ID == tmp.HallId).First().Name;
            }

        }

        // процедура добавления новой книги
        private void button1_Click(object sender, EventArgs e)
        {
            // создаём экземпляр книги через конструктор с данными из формы создания книги
            Book tmp = new Book(textBox1.Text, textBox2.Text, maskedTextBox1.Text, maskedTextBox2.Text, Int32.Parse(maskedTextBox3.Text));
            // добавляем экземпляр в массив
            books.Add(tmp);
            // обновляем таблицу в меню "Книги"
            UpdateBookGrid();
        }

        // процедура удаления книги
        private void button3_Click(object sender, EventArgs e)
        {
            // парсим из текстового поля ID книги
            var bookId = Int32.Parse(textBox9.Text);
            // выбираем по ID нужную книгу
            var qr = books.Where(w => w.ID == bookId).Select(w => w).First();
            // считаем кол-во свободных экземпляров книги
            var free = qr.Count - booksToReaders.Where(w => w.BookId == bookId).Where(w => w.ReceiveDate == null).Count();
            
            if (free == qr.Count)
            { // если количество свободных экземпляров равно кол-во экземпляров - книгу можно удалить
                // убираем сообщение об ошибке, если ранее ошибка была выставлена
                errorProvider1.SetError(this.button3, String.Empty);
                // помечаем книгу как удалённую (полностью удалять из базы нельзя, иначе потеряются данные в таблице "Книги у читателей")
                qr.deleted = true;
                // обновляем таблицу в меню "Книги"
                UpdateBookGrid();
            } else
            { // иначе если кол-во свободных и всего экземпляров не сходится - такую книгу нельзя удалить
                // ставим сообщение об ошибке и больше ничего не делаем
                errorProvider1.SetError(this.button3, "Имеются не возвращённые экземпляры");
            }
        }

        // процедура редактирования книги
        private void button2_Click(object sender, EventArgs e)
        {
            // берём книгу по ID из поля для ID редактируемой книги
            var qr = books.Where(w => w.ID == Int32.Parse(label16.Text));
            // проверяем что книг в массиве и в выборке больше нуля
            if (books.Count > 0 && qr.ToList().Count > 0)
            {
                // и записываем в первую и единствунную в выборке книгу данные из полей редактирования книги
                qr.First().Title = textBox8.Text;
                qr.First().Author = textBox7.Text;
                qr.First().ReleaseYear = maskedTextBox6.Text;
                qr.First().BookCode = maskedTextBox5.Text;
                qr.First().Count = Int32.Parse(maskedTextBox4.Text);
            }
            // обновляем таблицу в меню "Книги"
            UpdateBookGrid();
        }

        // процедура создания читателя
        private void button6_Click(object sender, EventArgs e)
        {
            // создаём нового читателя из данных из формы создания читателя
            Reader tmp = new Reader(textBox18.Text, (int)numericUpDown3.Value, dateTimePicker7.Value, maskedTextBox7.Text, textBox15.Text);
            // запрашиваем из базы кол-во читателей с таким же номером читательского билета
            var doubleCheck = readers.Where(w => w.TicketNum == tmp.TicketNum).Count();
            // если таких записей нет
            if (doubleCheck == 0)
            {
                // убираем сообщение об ошибке
                errorProvider1.SetError(this.button6, String.Empty);
                // добавляем читателя в базу
                readers.Add(tmp);
                // выставляем в поле для читательского билета рекомендуемое значение
                numericUpDown3.Value = numericUpDown3.Minimum + Reader.Index;
            }
            else
            { // если читатели с таким же номером читательского билета нашлись
                // То выставляем сообщение об ошибке
                errorProvider1.SetError(this.button6, "Нельзя создать читателя с таким же номером читательского билета");
            }
            // обновляем таблицу "Читатели"
            UpdateReaderGrid();
        }

        // процедура редактирования читателя
        private void button5_Click(object sender, EventArgs e)
        {
            // берём читателя по ID из поля для ID редактируемого читателя
            var qr = readers.Where(w => w.ID == Int32.Parse(label19.Text));
            // проверяем что читателей в массиве и в выборке больше нуля
            if (readers.Count > 0 && qr.ToList().Count > 0)
            {
                // и записываем в первого и единствунного в выборке читателя данные из полей редактирования читателя
                qr.First().FullName = textBox14.Text;
                qr.First().TicketNum = (int)numericUpDown4.Value;
                qr.First().Birthday = dateTimePicker5.Value;
                qr.First().Phone = maskedTextBox8.Text;
                qr.First().Education = textBox11.Text;
            }
            // обновляем таблицу "Читатели"
            UpdateReaderGrid();
        }

        // процедура удаления читателя
        private void button4_Click(object sender, EventArgs e)
        {
            // парсим ID читателя из textBox для ID читателя для удаления
            var readerId = Int32.Parse(textBox10.Text);
            // запрашиваем по ID читателя
            var qr = readers.Where(w => w.ID == readerId).First();
            // проверяем, что у читателя кол-во невозвращённых книг равно нулю
            if (booksToReaders.Where(w => w.ReaderId == readerId).Where(w => w.ReceiveDate != null).Count() == 0)
            {
                // убираем сообщение об ошибке если такое было
                errorProvider1.SetError(this.button4, String.Empty);
                // помечаем читателя удлённым (полностью удалять из базы нельзя т.к. сломаются "Книги у читателей" историчность данных и "Читатели в залах")
                qr.deleted = true;
                // обновляем таблицу "Читатели"
                UpdateReaderGrid();
            }
            else
            { // если есть не возвращённые книги
                // ставим сообщение об ошибке рядом с кнопкой удаления
                errorProvider1.SetError(this.button4, "У читателя имеются не возвращённые книги");
            }
        }

        // процедура создания читального зала
        private void button9_Click(object sender, EventArgs e)
        {
            // создаём читальных зал через конструктор с данными из формы для создания читального зала
            Hall tmp = new Hall(textBox23.Text, (int)numericUpDown6.Value, textBox22.Text, (int)numericUpDown7.Value);
            // Добавляем зал в базу
            halls.Add(tmp);
            // обновляем таблицу "Читальные залы"
            UpdateHallGrid();

        }

        // процедура редактирования зала
        private void button8_Click(object sender, EventArgs e)
        {
            // выбираем зал по ID из label для ID редактируемого зала
            var qr = halls.Where(w => w.ID == Int32.Parse(label23.Text));
            // проверяем что залов в массиве и в выборке больше нуля
            if (halls.Count > 0 && qr.ToList().Count > 0)
            {
                // и записываем в первый и единствунный в выборке зал данные из полей редактирования зала
                qr.First().Name = textBox20.Text;
                qr.First().HallNum = (int)numericUpDown5.Value;
                qr.First().Spec = textBox17.Text;
                qr.First().SitCount = (int)numericUpDown8.Value;
            }
            // обновляем таблицу "Читальные залы"
            UpdateHallGrid();
        }

        // процедура удаления читального зала
        private void button7_Click(object sender, EventArgs e)
        {
            // парсим ID Зала из textBox для ID зала под удаление
            var hallId = Int32.Parse(textBox13.Text);
            // выбираем зал из базы по ID
            var qr = halls.Where(w => w.ID == hallId).First();
            // считаем кол-во свободных мест в зале
            var free = qr.SitCount - readerToHalls.Where(w => w.HallId == qr.ID).Count();
            // если кол-во свободных мест совпадает с общим кол-вом мест в зале
            if (free == qr.SitCount)
            {
                // убираем сообщение об ошибке
                errorProvider1.SetError(this.button7, String.Empty);
                // удаляем зал из базы
                halls.Remove(qr);
                // обновляем таблицу "Читальные залы"
                UpdateHallGrid();
            } else
            { // если в зале остались читатели
                // ставим сообщение об ошибке
                errorProvider1.SetError(this.button7, "В зале имеются не выписанные читатели");
            }
            // т.к. залы удаляются - необходимо переписывать индекс что бы не было пробелов и ошибок в будущем
            if (halls.Count > 0) { Hall.Index = halls.Last().ID + 1; }
        }

        // процедура выдачи книги читателю
        private void button12_Click(object sender, EventArgs e)
        {
            // разделяем по пробелу значения из comboBox в котором хранятся данные книги
            var book = comboBox1.Text.Split(' ');
            // разделяем по пробелу значения из comboBox в котором хранятся данные читателя
            var reader = comboBox2.Text.Split(' ');
            // считаем количество свободных экземпляров книги
            var free = books.
                Where(w => w.ID == Int32.Parse(book.Last())).
                First().
                Count - booksToReaders.
                Where(w => w.BookId == Int32.Parse(book.Last())).
                Where(w => w.ReceiveDate == null).
                Count();
            // если есть свободные экземпляры книги
            if (free > 0)
            {
                // убираем сообщение об ошибке
                errorProvider1.SetError(this.button12, String.Empty);
                // создаём запись о выдаче книги читателю с сегодняшней датой
                BookToReader tmp = new BookToReader(Int32.Parse(book.Last()), Int32.Parse(reader.Last()), DateTime.Now);
                // Добавляем запись в базу
                booksToReaders.Add(tmp);
                // обновляем таблицу "Книги у читателей"
                UpdateBooksToReaderGrid();
                // обновляем таблицу "Книги"
                UpdateBookGrid();
            } else
            { // если свободных экземпляров книги нет
                // выводим сообщение об ошибке
                errorProvider1.SetError(this.button12, "Не осталось экземпляров книг");
            }
        }

        // процедура возврата книги от читателя в библиотеку
        private void button11_Click(object sender, EventArgs e)
        {
            // подгружаем запись о выдаче по ID из label для ID выдачи предназначенной для возвращения
            BookToReader tmp = booksToReaders.Where(w => w.ID == Int32.Parse(label6.Text)).First();
            // если в дата возврата отсутствует
            if (tmp.ReceiveDate == null)
            {
                // ставим текущую даты как дату возврата
                tmp.ReceiveDate = DateTime.Now;
                // соответственно если дата уже стоит и случайно была нажата кнопка возврата на уже возвращённую книгу
                // ничего не случится т.к. значение не будет равно null
            }
            // обновляем таблицу "Книги у читателей"
            UpdateBooksToReaderGrid();
            // обновляем таблицу "Книги"
            UpdateBookGrid();
        }

        // процедура записи читателя в зал
        private void button13_Click(object sender, EventArgs e)
        {
            // разделяем по пробелу значения из comboBox с данными читателя
            var reader = comboBox4.Text.Split(' ');
            // разделяем по пробелу значения из comboBox с данными читального зала
            var hall = comboBox3.Text.Split(' ');
            // создаём запись читателя в читальный зал
            ReaderToHall tmp = new ReaderToHall(Int32.Parse(reader.Last()), Int32.Parse(hall.Last()));
            // считаем количество свободных мест в зале
            var free = halls.Where(w => w.ID == Int32.Parse(hall.Last())).First().SitCount - readerToHalls.Where(w => w.HallId == Int32.Parse(hall.Last())).Count();
            // считаем кол-во этого человека в этом зале
            var checkDouble = readerToHalls.Where(w => w.HallId == Int32.Parse(hall.Last())).Where(w => w.ReaderId == Int32.Parse(reader.Last())).Count();
            // если есть свободные места в зале И этот человек ещё не записан в этот зал
            if (free > 0 && checkDouble == 0)
            {
                // убираем сообщение об ошибке
                errorProvider1.SetError(this.button13, String.Empty);
                // добавляем запись читателя в базу
                readerToHalls.Add(tmp);
            }
            // если свободных мест нет
            if (free < 1)
            {
                // выводим соотвествующую ошибку
                errorProvider1.SetError(this.button13, "В зале нет мест");
            }
            // если человек уже записан в этот зал
            if (checkDouble > 0)
            {
                // выводим другую соответствующую ошибку
                errorProvider1.SetError(this.button13, "Нельзя записать человека в один зал дважды");
            }
            // обновляем таблицу "Читатели в залах"
            UpdateReadersToHallGrid();
            // Обновляем таблицу "Залы"
            UpdateHallGrid();
        }

        // процедуры выписки читателя из зала
        private void button10_Click(object sender, EventArgs e)
        {
            // подгружаем запись о читателе в зале
            ReaderToHall tmp = readerToHalls.Where(w => w.ID == Int32.Parse(label5.Text)).First();
            // выставляем, что читатель удалён из зала
            tmp.deleted = true;
            // обновляем таблицу "Читатели в залах"
            UpdateReadersToHallGrid();
            // обновляем таблицу "Залы"
            UpdateHallGrid();
        }

        // процедура поиска всех книг заданного читателя
        private void button14_Click(object sender, EventArgs e)
        {
            // создаём новый экземпляр формы
            BooksOfReader form = new BooksOfReader();

            // парсим ID читателя из поля ID читателя предназначенного для редактирования
            var reader = Int32.Parse(label19.Text);

            // выбираем читателя из базы
            Reader r = readers.Where(w => w.ID == reader).First();
            // формируем строку для отображения
            var readerStr = r.FullName + " ID: " + r.ID.ToString();
            // формируем массив книг взятых читателем
            var booksArr = booksToReaders.
                // берём все книги выданные пользователю
                Where(w => w.ReaderId == r.ID).
                // из них выбираем только не возвращённые
                Where(w => w.ReceiveDate == null).
                // склеиваем с таблицей books по booksToReaders.BookId=books.ID
                Join(
                    books, 
                    booksToReader => booksToReader.BookId, book => book.ID, 
                    // сразу превращаем в красивую строку для отображения
                    (booksToReader, book) => new { BookName = $"{book.Title + " ID: " + book.ID.ToString()}" }).
               // и превращаем всё это в массив
               ToArray();

            // передаём данные в функцию для отображения в отдельном окне
            form.DataGridVisual(readerStr, booksArr);
            // отображаем окно
            form.ShowDialog();
        }

        // процедура поиска читателей, которые взяли книги в единственном экземпляре
        private void button15_Click(object sender, EventArgs e)
        {
            // создаём новый экземпляр формы
            OneCountBooks form = new OneCountBooks();
            // формируем массив записей
            var lst = books.
                // берём все книги, которые есть в одном экземпляре
                Where(w => w.Count == 1).
                // склеиваем с таблицей booksToReader по books.ID == booksToReaders.BookId
                Join(
                    booksToReaders, 
                    book => book.ID, booksToReader => booksToReader.BookId, 
                    // создаём новое поле bookName и берём дальше из booksToReaders поле ReaderId
                    (book, booksToReader) => new { bookName = $"{book.Title + " ID: " + book.ID.ToString()}", booksToReader.ReaderId }).
                // полученное склеиваем с таблицей readers по booksToReaders.ReaderId == reader.ID
                Join(
                    readers, 
                    booksToReader => booksToReader.ReaderId, reader => reader.ID, 
                    // созздаём новое поле readerName и берём с предыдущего шага bookName
                    (booksToReader, reader) => new { readerName = $"{reader.FullName + " ID: " + reader.ID.ToString()}", booksToReader.bookName }).
                // и превращаем это всё в массив
                ToArray();

            // передаём данные в функциб для отображения в отдельном окне
            form.GridViewVisual(lst);
            // отображаем окно
            form.ShowDialog();
        }

        // процедура подсчёта книг заданного автора в заданном читальном зале
        private void button16_Click(object sender, EventArgs e)
        {
            // если текст в полях comboBox присутствует как элемент в списках этих comboBox
            if (comboBox6.Items.Contains(comboBox6.Text) && comboBox7.Items.Contains(comboBox7.Text))
            {
                // получаем данные о читальнорм зале из comboBox
                var hall = comboBox6.Text.Split(' ');
                // получаем Данные о авторе из comboBox (в последнем поле ID книги)
                var book = comboBox7.Text.Split(' ');
                // парсим текст в числа
                var hallId = Int32.Parse(hall.Last());
                var bookId = Int32.Parse(book.Last());
                // по ID книги получаем автора
                var author = books.Where(w => w.ID == Int32.Parse(book.Last())).First().Author;

                // формируем список
                var qr = readerToHalls.
                    // получаем список читателей выбранного зала
                    Where(w => w.HallId == Int32.Parse(hall.Last())).
                    // склеиваем readersToHalls и booksToReader по ReaderId
                    Join(
                        booksToReaders,
                        readerToHall => readerToHall.ReaderId, booksToReader => booksToReader.ReaderId,
                        // из всего запроса нас интересуют ID книг в нашем зале
                        (readerToHall, booksToReader) => new { booksToReader.BookId }).
                    // из которым мы берём толко нужные нам книги
                    Where(w => w.BookId == Int32.Parse(book.Last())).
                    // и в итоге берём их количесво
                    Count();

                // формируем данные для отображения
                var message = $"Кол-во книг автора {author} в зале {comboBox6.Text} - {qr}";
                var caption = "Кол-во книг автора в зале";
                // вызываем небольшой MessageBox
                MessageBox.Show(message, caption, MessageBoxButtons.OK);
            }
        }
    }
}