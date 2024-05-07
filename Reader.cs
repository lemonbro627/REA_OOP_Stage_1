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

        public Reader(string fullName, int ticketNum, DateTime birthday, string phone, string education)
        {
            ID = size;
            FullName = fullName;
            TicketNum = ticketNum;
            Birthday = birthday;
            Phone = phone;
            Education = education;
            size++;
        }

        public int CompareTo(Reader obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }

        public static void RestoreIndex(int index)
        {
            size = index;
        }

        public string[] ForDataGrid()
        {
            string[] tmp = {
                this.ID.ToString(),
                this.FullName,
                this.TicketNum.ToString(),
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
        public int ID { get; set; }
        public int IDReader { get; set; }
        public int IDHall { get; set; }
        private static int size = 0;

        public ReaderToHall(int iDReader, int iDHall)
        {
            ID = size;
            IDReader = iDReader;
            IDHall = iDHall;
            size++;
        }

        public int CompareTo(ReaderToHall obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }
}
