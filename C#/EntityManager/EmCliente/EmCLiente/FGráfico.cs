using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace EmCRUD
{
    public partial class FGráfico : Form
    {
        public FGráfico()
        {
            InitializeComponent();
            mostrarGrafico();
        }
        public void mostrarGrafico()
        {
            using (var contexto = new TOCC8Entities())
            {
               
                var dados = contexto.produto
                    .OrderBy(p => p.descricao)
                    .Select(p => new
                    {
                        nome = (p.descricao ?? ("#" + p.codigo)),
                        lucro = (p.preco ?? 0) * (p.taxalucro ?? 0),                                   
                        prazo = DbFunctions.DiffDays(DateTime.Today, p.datavalidade) ?? 0             
                    })
                    .ToList();

                
                chart1.Titles.Clear();
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();

                var area = new ChartArea("area");
                area.AxisX.Interval = 1;
                area.AxisX.Title = "Descrição";
                area.AxisY.Title = "Valores";
                chart1.ChartAreas.Add(area);

                
                var sLucro = new Series("Lucro (R$)")
                {
                    ChartType = SeriesChartType.Column,
                    ChartArea = "area",
                    IsValueShownAsLabel = false
                };
                var sPrazo = new Series("Prazo (dias)")
                {
                    ChartType = SeriesChartType.Column,
                    ChartArea = "area",
                    IsValueShownAsLabel = false
                };

                foreach (var d in dados)
                {
                    sLucro.Points.AddXY(d.nome, d.lucro);
                    sPrazo.Points.AddXY(d.nome, d.prazo);
                }

                chart1.Series.Add(sLucro);
                chart1.Series.Add(sPrazo);

                if (chart1.Legends.Count == 0)
                    chart1.Legends.Add(new Legend("legend"));

                chart1.ChartAreas["area"].AxisX.LabelStyle.Angle = -45;
                chart1.Titles.Add("Lucro (R$) e Prazo de Validade (dias)");
            }
        }

    }
}
