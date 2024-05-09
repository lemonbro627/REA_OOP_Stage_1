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
            foreach (Book tmp in books)
            {
                dataGridView1.Rows.Add(tmp.ForDataGrid());
            }
        }

        private void UpdateReaderGrid()
        {
            dataGridView2.Rows.Clear();
            foreach (Reader tmp in readers)
            {
                dataGridView2.Rows.Add(tmp.ForDataGrid());
            }
        }

        private void UpdateHallGrid()
        {
            dataGridView3.Rows.Clear();
            foreach (Hall tmp in halls)
            {
                dataGridView3.Rows.Add(tmp.ForDataGrid());
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
            UpdateReaderGrid();
            UpdateHallGrid();
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
                maskedTextBox6.Text = tmp.ReleaseYear;
                maskedTextBox5.Text = tmp.BookCode;
                dateTimePicker4.Value = tmp.IssueDate;
                dateTimePicker3.Value = tmp.ReceiveDate;
                maskedTextBox4.Text = tmp.Count.ToString();
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

                Hall tmp = halls.Where(w => w.ID == Int32.Parse(dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString())).Select(w => w).ToList()[0];
                textBox20.Text = tmp.Name;
                numericUpDown5.Value = tmp.HallNum;
                textBox17.Text = tmp.Spec;
                numericUpDown8.Value = tmp.SitCount;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book tmp = new Book(textBox1.Text, textBox2.Text, maskedTextBox1.Text, maskedTextBox2.Text, dateTimePicker1.Value, dateTimePicker2.Value, Int32.Parse(maskedTextBox3.Text));
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
                qr.First().IssueDate = dateTimePicker4.Value;
                qr.First().ReceiveDate = dateTimePicker3.Value;
                qr.First().Count = Int32.Parse(maskedTextBox4.Text);
            }
            UpdateBookGrid();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reader tmp = new Reader(textBox18.Text, (int)numericUpDown3.Value, dateTimePicker7.Value, maskedTextBox7.Text, textBox15.Text);
            readers.Add(tmp);
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
            var qr = readers.Where(w => w.ID == readerId).Select(w => w).ToList();
            var tmpReaders = readers;
            foreach (Reader item in qr)
            {
                tmpReaders.Remove(item);
            }
            readers = tmpReaders;
            UpdateReaderGrid();
            if (readers.Count > 0) { Reader.RestoreIndex(readers.Last().ID + 1); }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Hall tmp = new Hall(textBox23.Text, (int)numericUpDown6.Value, textBox22.Text, (int)numericUpDown7.Value);
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
            var qr = halls.Where(w => w.ID == hallId).Select(w => w).ToList();
            var tmpHalls = halls;
            foreach (Hall item in qr)
            {
                tmpHalls.Remove(item);
            }
            halls = tmpHalls;
            UpdateHallGrid();
            if (halls.Count > 0) { Hall.RestoreIndex(halls.Last().ID + 1); }
        }
    }
}
