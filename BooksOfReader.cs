using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REA_OOP_Stage_1
{
    public partial class BooksOfReader : Form
    {
        public BooksOfReader()
        {
            InitializeComponent();
        }

        private void BooksOfReader_Load(object sender, EventArgs e)
        {
            
        }

        internal void DataGridVisual(string reader, IEnumerable<dynamic> books)
        {
            dataGridView1.Rows.Clear();
            foreach (dynamic book in books)
            {
                string[] tmp = { reader, book.BookName };
                dataGridView1.Rows.Add(tmp);
            }
        }
    }
}
