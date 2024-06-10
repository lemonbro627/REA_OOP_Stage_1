using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REA_OOP_Stage_1
{

    //-Зал
    //Ключ
    //Название библиотеки
    //Зал
    //Специализация
    //Количество мест
    //Список читателей
    [Serializable]
    internal class Hall: IComparable<Hall>
    {
        public int ID { get; set; } //ID зала
        public string Name { get; set; } //Название библиотеки
        public int HallNum { get; set; } //Зал
        public string Spec { get; set; } //Специализация
        public int SitCount { get; set; } //Количество мест
        private static int size = 0; //Размер массива/ID следующего зала

        public bool deleted { get; set; } //Статус, false - активна, true - удалена

        // Конструктор класса Hall
        public Hall(string name, int hallNum, string spec, int sitCount)
        {
            ID = size;
            Name = name;
            HallNum = hallNum;
            Spec = spec;
            SitCount = sitCount;
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
                this.Name,
                this.HallNum,
                this.Spec,
                this.SitCount
            };
            return tmp;
        }

        // публичный int CompareTo
        // реализуем метод интерфейса IComparable
        public int CompareTo(Hall obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }
}
