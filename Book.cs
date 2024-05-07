using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REA_OOP_Stage_1
{

    //-Книга
    //Ключ
    //Название
    //Автор
    //Год издания
    //Шифр книги
    //Дата выдачи
    //Дата получения
    //Кол-во экземпляров
    [Serializable]
    internal class Book: IComparable<Book>
    {
        public int ID { get; set; } //Ключ
        public string Title { get; set; } //Название
        public string Author { get; set; } // Автор
        public string ReleaseYear { get; set; } //Год издания
        public string BookCode { get; set; } //Шифр книги
        public DateTime IssueDate { get; set; } //Дата выдачи
        public DateTime ReceiveDate { get; set; } //Дата получения
        public int Count { get; set; } //Кол-во экземпляров
        private static int size = 0;

        public Book(string title, string author, string releaseYear, string bookCode, DateTime issueDate, DateTime receiveDate, int count)
        {
            ID = size;
            Title = title;
            Author = author;
            ReleaseYear = releaseYear;
            BookCode = bookCode;
            IssueDate = issueDate;
            ReceiveDate = receiveDate;
            Count = count;
            size++;
        }

        public int CompareTo(Book obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }

    [Serializable]
    internal class  BookToReader: IComparable<BookToReader>
    {
        public int ID { get; set;} //ID
        public int IDBook { get; set;}//ID книги
        public int IDReader { get; set;}//ID читателя
        public int Count { get; set; }//Количество книг
        private static int size = 0;

        public BookToReader(int iDBook, int iDReader, int count)
        {
            ID = size;
            IDBook = iDBook;
            IDReader = iDReader;
            Count = count;
            size++;
        }

        public int CompareTo(BookToReader obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }

    }
}
