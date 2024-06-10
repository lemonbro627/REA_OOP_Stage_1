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
        public int ID { get; set; } //ID книги
        public string Title { get; set; } //Название книги
        public string Author { get; set; } // Автор
        public string ReleaseYear { get; set; } //Год издания
        public string BookCode { get; set; } //Шифр книги ISBN13
        public int Count { get; set; } //Кол-во экземпляров
        private static int size = 0; //Размер массива/ID следующей книги

        public bool deleted { get; set; } //Статус, false - активна, true - удалена

        // Конструктор класса Book
        public Book(string title, string author, string releaseYear, string bookCode, int count)
        {
            ID = size;
            Title = title;
            Author = author;
            ReleaseYear = releaseYear;
            BookCode = bookCode;
            Count = count;
            size++;
            deleted = false;
        }

        // публичный статический int Index
        // используется для восстановления индекса после загрузки данных
        public static int Index
        {
            set => size = value;
        }

        // публичная функция object[] ForDataGrid()
        // используется для предоставления данных объекта в удобном виде для добавления в таблицу
        public object[] ForDataGrid()
        {
            object[] tmp = {
                this.ID,
                this.Title,
                this.Author,
                this.ReleaseYear,
                this.BookCode,
                this.Count,
            };
            return tmp;
        }

        // публичный int CompareTo
        // реализуем метод интерфейса IComparable
        public int CompareTo(Book obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }

    [Serializable]
    internal class BookToReader: IComparable<BookToReader>
    {
        public int ID { get; set;} //ID записи
        public int BookId { get; set;}//ID книги
        public int ReaderId { get; set;}//ID читателя
        public DateTime IssueDate { get; set; } //Дата выдачи книги
        public DateTime? ReceiveDate { get; set; } //Дата получени/возврата книги
        private static int size = 0; //Размер массива/ID следующей записи

        // Конструктор класса BookToReader
        public BookToReader(int bookId, int readerId, DateTime issueDate)
        {
            ID = size;
            BookId = bookId;
            ReaderId = readerId;
            IssueDate = issueDate;
            ReceiveDate = null;
            size++;
        }

        // публичный статический int Index
        // используется для восстановления индекса после загрузки данных
        public static int Index
        {
            set => size = value;
        }

        // публичный int CompareTo
        // реализуем метод интерфейса IComparable
        public int CompareTo(BookToReader obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }

    }
}
