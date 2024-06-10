using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REA_OOP_Stage_1
{

    //-Читатель
    //Ключ
    //ФИО
    //No билета
    //Дата рождения
    //Телефон
    //Образование
    //Зал
    //Список книг
    [Serializable]
    internal class Reader: IComparable<Reader>
    {
        public int ID { get; set; } //Ключ
        public string FullName { get; set; } //ФИО
        public int TicketNum { get; set; } //No билета
        public DateTime Birthday { get; set; } //Дата рождения
        public string Phone { get; set; } //Телефон
        public string Education { get; set; } //Образование
        private static int size = 0;

        public bool deleted { get; set; } //Статус, false - активна, true - удалена

        // Конструктор класса Reader
        public Reader(string fullName, int ticketNum, DateTime birthday, string phone, string education)
        {
            ID = size;
            FullName = fullName;
            TicketNum = ticketNum;
            Birthday = birthday;
            Phone = phone;
            Education = education;
            size++;
            deleted = false;
        }

        // публичный int CompareTo
        // реализуем метод интерфейса IComparable
        public int CompareTo(Reader obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }

        // публичный статический int Index
        // используется для восстановления индекса после загрузки данных
        public static int Index {
            set => size = value;
            get => size; 
        }

        // публичная функция object[] ForDataGrid()
        // используется для предоставления данных объекта в удобном виде для добавления в таблицу
        public object[] ForDataGrid()
        {
            object[] tmp = {
                this.ID,
                this.FullName,
                this.TicketNum,
                this.Birthday.ToLongDateString(),
                this.Phone,
                this.Education
            };
            return tmp;
        }
    }

    [Serializable]
    internal class ReaderToHall : IComparable<ReaderToHall>
    {
        public int ID { get; set; } // ID записи
        public int ReaderId { get; set; } // ID читателя
        public int HallId { get; set; } // ID зала
        private static int size = 0; // Размер массива/ID следующей записи

        public bool deleted { get; set; } //Статус, false - активна, true - удалена

        // Конструктор класса ReaderToHall
        public ReaderToHall(int readerId, int hallId)
        {
            ID = size;
            ReaderId = readerId;
            HallId = hallId;
            size++;
            deleted = false;
        }

        // публичный статический int Index
        // используется для восстановления индекса после загрузки данных
        public static int Index
        {
            set => size = value;
        }

        // публичный int CompareTo
        // реализуем метод интерфейса IComparable
        public int CompareTo(ReaderToHall obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }
}
