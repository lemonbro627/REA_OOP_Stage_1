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
        public int ID { get; set; } //Ключ
        public string Name { get; set; } //Название библиотеки
        public int HallNum { get; set; } //Зал
        public string Spec { get; set; } //Специализация
        public int Size { get; set; } //Количество мест
        public List<Reader> Readers { get; set; } //Список читателей
        private static int size = 0;

        public Hall(string name, int hallNum, string spec, int size)
        {
            ID = size;
            Name = name;
            HallNum = hallNum;
            Spec = spec;
            Size = size;
        }

        public int CompareTo(Hall obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }
}
