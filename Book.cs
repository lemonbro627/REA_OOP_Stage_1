﻿using System;
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
        public int Count { get; set; } //Кол-во экземпляров
        private static int size = 0;

        public bool deleted { get; set; } //Статус, false - активна, true - удалена

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

        public static void RestoreIndex(int index)
        {
            size = index;
        }

        public string[] ForDataGrid()
        {
            string[] tmp = {
                this.ID.ToString(),
                this.Title,
                this.Author,
                this.ReleaseYear,
                this.BookCode,
                this.Count.ToString(),
            };
            return tmp;
        }

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
        public int ID { get; set;} //ID
        public int IDBook { get; set;}//ID книги
        public int IDReader { get; set;}//ID читателя
        public DateTime IssueDate { get; set; } //Дата выдачи
        public DateTime? ReceiveDate { get; set; } //Дата получени/возврата
        private static int size = 0;

        public BookToReader(int iDBook, int iDReader, DateTime issueDate)
        {
            ID = size;
            IDBook = iDBook;
            IDReader = iDReader;
            IssueDate = issueDate;
            ReceiveDate = null;
            size++;
        }

        public static void RestoreIndex(int index)
        {
            size = index;
        }

        public int CompareTo(BookToReader obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }

    }
}
