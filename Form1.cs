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
            foreach (Book book in books)
            {
                dataGridView1.Rows.Add(book.ForDataGrid());
            }
        }

        private void UpdateReaderGrid()
        {
            dataGridView2.Rows.Clear();
            foreach (Reader reader in readers)
            {
                dataGridView2.Rows.Add(reader.ForDataGrid());
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
            UpdateBookGrid();

            //Load BooksToReaders
            fname = "data_bookToReaders.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            booksToReaders = (List<BookToReader>)bf.Deserialize(fs);
            fs.Close();

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
            UpdateReaderGrid();

            //Save ReadersToHalls
            fname = "data_readersToHalls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            readerToHalls = (List<ReaderToHall>)bf.Deserialize(fs);
            fs.Close();

            //Save Halls
            fname = "data_halls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            halls = (List<Hall>)bf.Deserialize(fs);
            fs.Close();
            Console.WriteLine("Загружено");
            toolStripStatusLabel1.Text = "Загружено";

            UpdateBookGrid();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox9.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                label16.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                Book tmp = books.Where(w => w.ID == Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString())).Select(w => w).ToList()[0];
                textBox8.Text = tmp.Title;
                textBox7.Text = tmp.Author;
                textBox6.Text = tmp.ReleaseYear;
                textBox5.Text = tmp.BookCode;
                dateTimePicker4.Value = tmp.IssueDate;
                dateTimePicker3.Value = tmp.ReceiveDate;
                numericUpDown2.Value = tmp.Count;
            }

        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox10.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                label19.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();

                Reader tmp = readers.Where(w => w.ID == Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString())).Select(w => w).ToList()[0];
                textBox14.Text = tmp.FullName;
                numericUpDown4.Value = tmp.TicketNum;
                dateTimePicker5.Value = tmp.Birthday;
                textBox12.Text = tmp.Phone;
                textBox11.Text = tmp.Education;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book tmp = new Book(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, dateTimePicker1.Value, dateTimePicker2.Value, (int)numericUpDown1.Value);
            books.Add(tmp);
            UpdateBookGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var bookId = Int32.Parse(textBox9.Text);
            var qr = books.Where(w => w.ID == bookId).Select(w => w).ToList();
            var tmpBooks = books;
            foreach (Book item in qr) { 
                tmpBooks.Remove(item);
            }
            books = tmpBooks;
            UpdateBookGrid();
            Book.RestoreIndex(books.Last().ID + 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var qr = books.Where(w => w.ID == Int32.Parse(label16.Text)).First();
            qr.Title = textBox8.Text;
            qr.Author = textBox7.Text;
            qr.ReleaseYear = textBox6.Text;
            qr.BookCode = textBox5.Text;
            qr.IssueDate = dateTimePicker4.Value;
            qr.ReceiveDate = dateTimePicker3.Value;
            qr.Count = (int)numericUpDown2.Value;
            UpdateBookGrid();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reader tmp = new Reader(textBox18.Text, (int)numericUpDown3.Value, dateTimePicker7.Value, textBox16.Text, textBox15.Text);
            readers.Add(tmp);
            UpdateReaderGrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var qr = readers.Where(w => w.ID == Int32.Parse(label19.Text)).First();
            qr.FullName = textBox14.Text;
            qr.TicketNum = (int)numericUpDown4.Value;
            qr.Birthday = dateTimePicker5.Value;
            qr.Phone = textBox12.Text;
            qr.Education = textBox11.Text;
            UpdateReaderGrid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var readerId = Int32.Parse(textBox10.Text);
            var qr = readers.Where(w => w.ID == readerId).Select(w => w).ToList();
            var tmpReaders = readers;
            foreach (Reader item in qr)
            {
                tmpReaders.Remove(item);
            }
            readers = tmpReaders;
            UpdateReaderGrid();
            Reader.RestoreIndex(readers.Last().ID + 1);
        }
    }
}
