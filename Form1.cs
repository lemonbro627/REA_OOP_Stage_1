using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private void UpdateBookGrid()
        {
            dataGridView1.Rows.Clear();
            comboBox1.Items.Clear();
            foreach (Book tmp in books.Where(w => w.deleted == false).Select(w => w).ToList())
            {
                var book = tmp.ForDataGrid();
                var free = tmp.Count - booksToReaders.Where(w => w.IDBook == tmp.ID).Where(w => w.ReceiveDate == null).Count();
                dataGridView1.Rows.Add(book.Append(free.ToString()).ToArray());
                comboBox1.Items.Add(tmp.Title + " ID: " + tmp.ID.ToString());
            }
        }

        private void UpdateReaderGrid()
        {
            dataGridView2.Rows.Clear();
            comboBox2.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            foreach (Reader tmp in readers.Where(w => w.deleted == false).Select(w => w).ToList())
            {
                dataGridView2.Rows.Add(tmp.ForDataGrid());
                comboBox2.Items.Add(tmp.FullName + " ID: " + tmp.ID.ToString());
                comboBox4.Items.Add(tmp.FullName + " ID: " + tmp.ID.ToString());
                comboBox5.Items.Add(tmp.FullName + " ID: " + tmp.ID.ToString());
            }
        }

        private void UpdateHallGrid()
        {
            dataGridView3.Rows.Clear();
            comboBox3.Items.Clear();
            foreach (Hall tmp in halls.Where(w => w.deleted == false).Select(w => w).ToList())
            {
                var hall = tmp.ForDataGrid();
                var free = tmp.SitCount - readerToHalls.Where(w => w.IDHall == tmp.ID).Count();
                dataGridView3.Rows.Add(hall.Append(free.ToString()).ToArray());
                comboBox3.Items.Add(tmp.Name + " ID: " + tmp.ID.ToString());
            }
        }

        private void UpdateBooksToReaderGrid()
        {
            dataGridView4.Rows.Clear();
            foreach (BookToReader tmp in booksToReaders.Select(w => w).ToList())
            {
                var reader = readers.Where(w => w.ID == tmp.IDReader).First();
                var book = books.Where(w => w.ID == tmp.IDBook).First();
                string receiveDate = tmp.ReceiveDate.GetValueOrDefault() < tmp.IssueDate ? "" : tmp.ReceiveDate.GetValueOrDefault().ToLongDateString();
                string[] tmpStr = { tmp.ID.ToString(), reader.FullName + " ID: " + reader.ID.ToString(), book.Title + " ID: " + book.ID.ToString(), tmp.IssueDate.ToLongDateString(), receiveDate };
                dataGridView4.Rows.Add(tmpStr);
            }
        }

        private void UpdateReadersToHallGrid()
        {
            dataGridView5.Rows.Clear();
            foreach (ReaderToHall tmp in readerToHalls.Where(w => w.deleted == false).Select(w => w).ToList())
            {
                var reader = readers.Where(w => w.ID == tmp.IDReader).First();
                var hall = halls.Where(w => w.ID == tmp.IDHall).First();
                string[] tmpStr = { tmp.ID.ToString(), reader.FullName + " ID: " + reader.ID.ToString(), hall.Name + " ID: " + hall.ID.ToString() };
                dataGridView5.Rows.Add(tmpStr);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Сохранение...");
            toolStripStatusLabel1.Text = "Сохранение...";
            string fname;
            BinaryFormatter bf;
            FileStream fs;
            //Save Book
            fname = "data_book.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            bf.Serialize(fs, books);
            fs.Close();

            //Save BookToReaders
            fname = "data_bookToReaders.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            bf.Serialize(fs, booksToReaders);
            fs.Close();

            //Save Reader
            fname = "data_readers.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            bf.Serialize(fs, readers);
            fs.Close();

            //Save ReaderToHall
            fname = "data_readersToHalls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            bf.Serialize(fs, readerToHalls);
            fs.Close();

            //Save Hall
            fname = "data_halls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            bf.Serialize(fs, halls);
            fs.Close();
            Console.WriteLine("Сохранено");
            toolStripStatusLabel1.Text = "Сохранено";
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Загрузка...");
            toolStripStatusLabel1.Text = "Загрузка...";
            string fname;
            BinaryFormatter bf;
            FileStream fs;

            //Load Books
            fname = "data_book.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            books = (List<Book>)bf.Deserialize(fs);
            fs.Close();
            if (books.Count > 0)
            {
                Book.RestoreIndex(books.Last().ID + 1);
            }

            //Load BooksToReaders
            fname = "data_bookToReaders.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            booksToReaders = (List<BookToReader>)bf.Deserialize(fs);
            fs.Close();
            if (booksToReaders.Count > 0)
            {
                BookToReader.RestoreIndex(booksToReaders.Last().ID + 1);
            }

            //Save Readers
            fname = "data_readers.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            readers = (List<Reader>)bf.Deserialize(fs);
            fs.Close();
            if (readers.Count > 0)
            {
                Reader.RestoreIndex(readers.Last().ID + 1);
            }

            //Save ReadersToHalls
            fname = "data_readersToHalls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            readerToHalls = (List<ReaderToHall>)bf.Deserialize(fs);
            fs.Close();
            if (readerToHalls.Count > 0)
            {
                ReaderToHall.RestoreIndex(readerToHalls.Last().ID + 1);
            }

            //Save Halls
            fname = "data_halls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            halls = (List<Hall>)bf.Deserialize(fs);
            fs.Close();
            if (halls.Count > 0)
            {
                Hall.RestoreIndex(halls.Last().ID + 1);
            }
            Console.WriteLine("Загружено");
            toolStripStatusLabel1.Text = "Загружено";

            UpdateBookGrid();
            UpdateBooksToReaderGrid();
            UpdateReaderGrid();
            UpdateReadersToHallGrid();
            UpdateHallGrid();

            numericUpDown3.Value = numericUpDown3.Minimum + Reader.GetIndex();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox9.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                label16.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                Book tmp = books.Where(w => w.ID == Int32.Parse(label16.Text)).First();
                textBox8.Text = tmp.Title;
                textBox7.Text = tmp.Author;
                maskedTextBox6.Text = tmp.ReleaseYear;
                maskedTextBox5.Text = tmp.BookCode;
                maskedTextBox4.Text = tmp.Count.ToString();
            }

        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox10.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                label19.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();

                Reader tmp = readers.Where(w => w.ID == Int32.Parse(label19.Text)).First();
                textBox14.Text = tmp.FullName;
                numericUpDown4.Value = tmp.TicketNum;
                dateTimePicker5.Value = tmp.Birthday;
                maskedTextBox8.Text = tmp.Phone;
                textBox11.Text = tmp.Education;
            }

        }

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox13.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                label23.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();

                Hall tmp = halls.Where(w => w.ID == Int32.Parse(label23.Text)).First();
                textBox20.Text = tmp.Name;
                numericUpDown5.Value = tmp.HallNum;
                textBox17.Text = tmp.Spec;
                numericUpDown8.Value = tmp.SitCount;
            }

        }

        private void dataGridView4_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                label6.Text = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();

                BookToReader tmp = booksToReaders.Where(w => w.ID == Int32.Parse(label6.Text)).First();
                textBox5.Text = books.Where(w => w.ID == tmp.IDBook).First().Title;
                textBox4.Text = readers.Where(w => w.ID == tmp.IDReader).First().FullName;
            }

        }

        private void dataGridView5_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                label5.Text = dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString();

                ReaderToHall tmp = readerToHalls.Where(w => w.ID == Int32.Parse(label5.Text)).First();
                textBox6.Text = readers.Where(w => w.ID == tmp.IDReader).First().FullName;
                textBox3.Text = halls.Where(w => w.ID == tmp.IDHall).First().Name;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book tmp = new Book(textBox1.Text, textBox2.Text, maskedTextBox1.Text, maskedTextBox2.Text, Int32.Parse(maskedTextBox3.Text));
            books.Add(tmp);
            UpdateBookGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var bookId = Int32.Parse(textBox9.Text);
            var free = books.Where(w => w.ID == bookId).First().Count - booksToReaders.Where(w => w.IDBook == bookId).Where(w => w.ReceiveDate == null).Count();
            var qr = books.Where(w => w.ID == bookId).Select(w => w).First();
            if (free == qr.Count)
            {
                errorProvider1.SetError(this.button3, String.Empty);
                qr.deleted = true;
                UpdateBookGrid();
            } else
            {
                errorProvider1.SetError(this.button3, "Имеются не возвращённые экземпляры");
            }
            if (books.Count > 0) { Book.RestoreIndex(books.Last().ID + 1); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var qr = books.Where(w => w.ID == Int32.Parse(label16.Text));
            if (books.Count > 0 && qr.ToList().Count > 0)
            {
                qr.First().Title = textBox8.Text;
                qr.First().Author = textBox7.Text;
                qr.First().ReleaseYear = maskedTextBox6.Text;
                qr.First().BookCode = maskedTextBox5.Text;
                qr.First().Count = Int32.Parse(maskedTextBox4.Text);
            }
            UpdateBookGrid();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reader tmp = new Reader(textBox18.Text, (int)numericUpDown3.Value, dateTimePicker7.Value, maskedTextBox7.Text, textBox15.Text);
            var doubleCheck = readers.Where(w => w.TicketNum == tmp.TicketNum).Count();
            if (doubleCheck == 0)
            {
                errorProvider1.SetError(this.button6, String.Empty);
                readers.Add(tmp);
                numericUpDown3.Value = numericUpDown3.Minimum + Reader.GetIndex();
            }
            else
            {
                errorProvider1.SetError(this.button6, "Нельзя создать читателя с таким же номером читательского билета");
            }
            UpdateReaderGrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var qr = readers.Where(w => w.ID == Int32.Parse(label19.Text));
            if (readers.Count > 0 && qr.ToList().Count > 0)
            {
                qr.First().FullName = textBox14.Text;
                qr.First().TicketNum = (int)numericUpDown4.Value;
                qr.First().Birthday = dateTimePicker5.Value;
                qr.First().Phone = maskedTextBox8.Text;
                qr.First().Education = textBox11.Text;
            }
            UpdateReaderGrid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var readerId = Int32.Parse(textBox10.Text);
            var qr = readers.Where(w => w.ID == readerId).First();
            qr.deleted = true;
            UpdateReaderGrid();
            if (readers.Count > 0) { Reader.RestoreIndex(readers.Last().ID + 1); }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Hall tmp = new Hall(textBox23.Text, (int)numericUpDown6.Value, textBox22.Text, (int)numericUpDown7.Value);
            var free = tmp.SitCount - readerToHalls.Where(w => w.IDHall == tmp.ID).Count();
            halls.Add(tmp);
            UpdateHallGrid();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var qr = halls.Where(w => w.ID == Int32.Parse(label23.Text));
            if (halls.Count > 0 && qr.ToList().Count > 0)
            {
                qr.First().Name = textBox20.Text;
                qr.First().HallNum = (int)numericUpDown5.Value;
                qr.First().Spec = textBox17.Text;
                qr.First().SitCount = (int)numericUpDown8.Value;
            }
            UpdateHallGrid();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var hallId = Int32.Parse(textBox13.Text);
            var qr = halls.Where(w => w.ID == hallId).First();
            var free = qr.SitCount - readerToHalls.Where(w => w.IDHall == qr.ID).Count();
            if (free == qr.SitCount)
            {
                errorProvider1.SetError(this.button7, String.Empty);
                halls.Remove(qr);
                UpdateHallGrid();
            } else
            {
                errorProvider1.SetError(this.button7, "В зале имеются не выписанные читатели");
            }
            if (halls.Count > 0) { Hall.RestoreIndex(halls.Last().ID + 1); }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var book = comboBox1.Text.Split(' ');
            var reader = comboBox2.Text.Split(' ');
            var free = books.Where(w => w.ID == Int32.Parse(book.Last())).First().Count - booksToReaders.Where(w => w.IDBook == Int32.Parse(book.Last())).Where(w => w.ReceiveDate == null).Count();
            if (free > 0)
            {
                errorProvider1.SetError(this.button12, String.Empty);
                BookToReader tmp = new BookToReader(Int32.Parse(book.Last()), Int32.Parse(reader.Last()), DateTime.Now);
                booksToReaders.Add(tmp);
                UpdateBooksToReaderGrid();
                UpdateBookGrid();
            } else
            {
                errorProvider1.SetError(this.button12, "Не осталось экземпляров книг");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            BookToReader tmp = booksToReaders.Where(w => w.ID == Int32.Parse(label6.Text)).First();
            if (tmp.ReceiveDate == null)
            {
                tmp.ReceiveDate = DateTime.Now;
            }
            UpdateBooksToReaderGrid();
            UpdateBookGrid();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var reader = comboBox4.Text.Split(' ');
            var hall = comboBox3.Text.Split(' ');
            ReaderToHall tmp = new ReaderToHall(Int32.Parse(reader.Last()), Int32.Parse(hall.Last()));
            var free = halls.Where(w => w.ID == Int32.Parse(hall.Last())).First().SitCount - readerToHalls.Where(w => w.IDHall == Int32.Parse(hall.Last())).Count();
            var checkDouble = readerToHalls.Where(w => w.IDHall == Int32.Parse(hall.Last())).Where(w => w.IDReader == Int32.Parse(reader.Last())).Count();
            if (free > 0 && checkDouble == 0)
            {
                errorProvider1.SetError(this.button13, String.Empty);
                readerToHalls.Add(tmp);
            }
            if (free < 1)
            {
                errorProvider1.SetError(this.button13, "В зале нет мест");
            }
            if (checkDouble > 0) {
                errorProvider1.SetError(this.button13, "Нельзя записать человека в один зал дважды");
            }
            UpdateReadersToHallGrid();
            UpdateHallGrid();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ReaderToHall tmp = readerToHalls.Where(w => w.ID == Int32.Parse(label5.Text)).First();
            tmp.deleted = true;
            UpdateReadersToHallGrid();
            UpdateHallGrid();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            BooksOfReader form = new BooksOfReader();

            var reader = comboBox5.Text.Split(' ');

            Reader r = readers.Where(w => w.ID == Int32.Parse(reader.Last())).First();
            var readerStr = r.FullName + " ID: " + r.ID.ToString();
            var booksArr = booksToReaders.
                Where(w => w.IDReader == r.ID).
                Join(
                    books, 
                    booksToReader => booksToReader.IDBook, book => book.ID, 
                    (booksToReader, book) => new { BookName = $"{book.Title + " ID: " + book.ID.ToString()}" }).
               ToArray();

            form.DataGridVisual(readerStr, booksArr);
            form.ShowDialog();
        }
    }
}
