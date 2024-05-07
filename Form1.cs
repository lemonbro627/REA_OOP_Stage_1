using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Сохранение...");
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
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Загрузка...");
            string fname;
            BinaryFormatter bf;
            FileStream fs;
            //Save Books
            fname = "data_book.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            books = (List<Book>)bf.Deserialize(fs);
            fs.Close();

            //Save Books
            fname = "data_bookToReaders.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            booksToReaders = (List<BookToReader>)bf.Deserialize(fs);
            fs.Close();

            //Save Books
            fname = "data_readers.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            readers = (List<Reader>)bf.Deserialize(fs);
            fs.Close();

            //Save Books
            fname = "data_readersToHalls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            readerToHalls = (List<ReaderToHall>)bf.Deserialize(fs);
            fs.Close();

            //Save Books
            fname = "data_halls.bin"; // прописываем путь к файлу
            bf = new BinaryFormatter();
            fs = new FileStream(fname, FileMode.OpenOrCreate);
            halls = (List<Hall>)bf.Deserialize(fs);
            fs.Close();
            Console.WriteLine("Загружено");
        }
    }
}
