using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LancuchyDostaw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Logic logic = new Logic();
            List<Row> listOfRows = logic.CreateRows();
            tableLayoutPanel1.RowCount = listOfRows.Count + 1;
            tableLayoutPanel1.ColumnCount = listOfRows[0].ValueList.Count;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / tableLayoutPanel1.ColumnCount));
            }
            for (int i = 0; i < listOfRows.Count; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / tableLayoutPanel1.RowCount));
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    tableLayoutPanel1.Controls.Add(new Label()
                    {
                        Text = listOfRows[i].ValueList[j],
                        Font = new Font("Arial", 12),
                        Margin = Padding.Empty,
                        AutoSize = true,
                        Anchor = ( AnchorStyles.None )
                    }, j, i);
                }
            }
        }
    }
}
