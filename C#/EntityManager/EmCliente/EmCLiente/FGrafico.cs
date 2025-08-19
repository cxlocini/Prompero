using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ControleDeProdutos
{
    public partial class FGrafico : Form
    {
        private readonly List<Produto> _produtos;

        // Construtor modificado para receber a lista de produtos
        public FGrafico(List<Produto> produtos)
        {
            InitializeComponent();
            _produtos = produtos;
        }

        private void FormGrafico_Load(object sender, System.EventArgs e)
        {
            MontarGrafico();
        }

        private void MontarGrafico()
        {
            
            chart1.Series.Clear();
            chart1.Titles.Clear();

            chart1.Titles.Add("Lucro (R$) e Prazo de Validade (dias) por Produto");

            
            var seriesLucro = new Series("Lucro (R$)")
            {
                ChartType = SeriesChartType.Column
            };

            var seriesValidade = new Series("Prazo de Validade (dias)")
            {
                ChartType = SeriesChartType.Column
            };

            
            foreach (var produto in _produtos)
            {
                
                seriesLucro.Points.AddXY(produto.Descricao, produto.LucroEmReais);
                seriesValidade.Points.AddXY(produto.Descricao, produto.PrazoValidade);
            }

            
            chart1.Series.Add(seriesLucro);
            chart1.Series.Add(seriesValidade);

            
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45; 
        }
    }
}