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
    public partial class OneCountBooks : Form
    {
        public OneCountBooks()
        {
            InitializeComponent();
        }

        private void OneCountBooks_Load(object sender, EventArgs e)
        {

        }

        internal void GridViewVisual(IEnumerable<dynamic> list)
        {
            dataGridView1.Rows.Clear();
            foreach (dynamic item in list)
            {
                string[] tmp = { item.bookName, item.readerName };
                dataGridView1.Rows.Add(tmp);
            }
        }
    }
}
