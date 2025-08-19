using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Cliente
{
    public partial class FGrafico : Form
    {
        public FGrafico()
        {
            InitializeComponent();

            mostrarGrafico();
        }

        public void mostrarGrafico()
        {
            DataTable tabela;
            ClienteDAO clienteDAO;
            int i;
            double valor = 0;

            try
            {
                clienteDAO = new ClienteDAO();
                tabela = clienteDAO.listar();

                chart1.ChartAreas[0].AxisX.Title = " Nomes ";
                chart1.ChartAreas[0].AxisY.Title = " Anos ";
                chart1.Titles.Add(" Lista de idades ");

                chart1.Series[0].Name = "Idade";
                chart1.Series[0].IsVisibleInLegend = true;

                chart1.Series.Add(new Series()); //inclui uma série nova
                chart1.Series[1].Name = "Media";
                chart1.Series[1].ChartType = SeriesChartType.Line;
                chart1.Series[1].IsVisibleInLegend = true;
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();

                for (i = 0; i < tabela.Rows.Count; i++)
                {
                    if (!((tabela.Rows[i][2] != DBNull.Value) &&
                        (Double.TryParse(tabela.Rows[i][2].ToString(), out valor))))
                    {
                        valor = 0;
                    }
                    chart1.Series[0].Points.AddXY((string)tabela.Rows[i][1], valor);

                    chart1.Series[1].Points.Add(45);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
