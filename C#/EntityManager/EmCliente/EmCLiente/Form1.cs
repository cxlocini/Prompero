using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using System.Globalization;

namespace EmCRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
            this.txtBuscar.KeyUp += txtBuscar_KeyUp;                  // busca por parte da descrição
            this.txtCodigo.Leave += txtCodigo_Leave;                  // preenche ao sair do código
            this.dgvDados.CellDoubleClick += dgvDados_CellDoubleClick;// transfere dados ao duplo clique
            this.btnListar.Click += btnListar_Click;                  // listar todos os campos
            this.btnGrafico.Click += btnGrafico_Click;                // abre o gráfico

            ConfigurarGridConsulta();
        }

        
        private void ConfigurarGridConsulta()
        {
            dgvDados.AutoGenerateColumns = false;
            dgvDados.Columns.Clear();

            dgvDados.Columns.Add(new DataGridViewTextBoxColumn { Name = "codigo", HeaderText = "Código", DataPropertyName = "codigo", Width = 80 });
            dgvDados.Columns.Add(new DataGridViewTextBoxColumn { Name = "descricao", HeaderText = "Descrição", DataPropertyName = "descricao", Width = 260 });
            dgvDados.Columns.Add(new DataGridViewTextBoxColumn { Name = "datavalidade", HeaderText = "Validade", DataPropertyName = "datavalidade", Width = 110 });
            dgvDados.Columns.Add(new DataGridViewTextBoxColumn { Name = "precofinal", HeaderText = "Preço Final (R$)", DataPropertyName = "precofinal", Width = 120, DefaultCellStyle = { Format = "N2" } });
            dgvDados.Columns.Add(new DataGridViewTextBoxColumn { Name = "prazovalidade", HeaderText = "Prazo (dias)", DataPropertyName = "prazovalidade", Width = 100 });
        }

        private double? ParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return Convert.ToDouble(s.Replace(',', '.'), CultureInfo.InvariantCulture);
        }

        
        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            var termo = (txtBuscar.Text ?? "").ToLower();

            using (var contexto = new TOCC8ProdutoContext())
            {
                var dados = contexto.Produtos
                    .Where(p => (p.Descricao ?? "").ToLower().Contains(termo))
                    .Select(p => new
                    {
                        codigo = p.Codigo,
                        descricao = p.Descricao,
                        datavalidade = p.DataValidade,
                        precofinal = (p.Preco ?? 0) * (1 + (p.TaxaLucro ?? 0)),
                        prazovalidade = DbFunctions.DiffDays(DateTime.Today, p.DataValidade) ?? 0
                    })
                    .OrderBy(p => p.descricao)
                    .ToList();

                ConfigurarGridConsulta();
                dgvDados.DataSource = dados;
            }
        }

        
        private void btnListar_Click(object sender, EventArgs e)
        {
            using (var contexto = new TOCC8ProdutoContext())
            {
                dgvDados.AutoGenerateColumns = true;
                dgvDados.Columns.Clear();
                dgvDados.DataSource = contexto.Produtos
                                              .OrderBy(p => p.Codigo)
                                              .ToList();
            }
        }

        
        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var contexto = new TOCC8ProdutoContext())
                {
                    var p = new Produto
                    {
                        Descricao = txtDescricao.Text?.Trim(),
                        DataValidade = dtValidade.Value.Date,
                        Preco = ParseDouble(txtPreco.Text),
                        TaxaLucro = ParseDouble(txtTaxa.Text)
                    };
                    contexto.Produtos.Add(p);
                    contexto.SaveChanges();
                }
                MessageBox.Show("Salvo com sucesso.");
                btnListar_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gravar: " + ex.Message);
            }
        }

        
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtCodigo.Text.Trim(), out int codigo)) return;

                using (var contexto = new TOCC8ProdutoContext())
                {
                    var p = contexto.Produtos.FirstOrDefault(x => x.Codigo == codigo);
                    if (p == null) { MessageBox.Show("Registro não encontrado."); return; }

                    p.Descricao = txtDescricao.Text?.Trim();
                    p.DataValidade = dtValidade.Value.Date;
                    p.Preco = ParseDouble(txtPreco.Text);
                    p.TaxaLucro = ParseDouble(txtTaxa.Text);

                    contexto.SaveChanges();
                }
                MessageBox.Show("Alterado com sucesso.");
                btnListar_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void btnRemover_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtCodigo.Text.Trim(), out int codigo)) return;

                using (var contexto = new TOCC8ProdutoContext())
                {
                    var p = contexto.Produtos.FirstOrDefault(x => x.Codigo == codigo);
                    if (p == null) { MessageBox.Show("Registro não encontrado."); return; }

                    contexto.Produtos.Remove(p);
                    contexto.SaveChanges();
                }
                MessageBox.Show("Removido com sucesso.");
                btnListar_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void btnGrafico_Click(object sender, EventArgs e)
        {
            using (var f = new FGráfico())
            {
                f.WindowState = FormWindowState.Maximized;
                f.ShowDialog(this);
            }
        }

        
        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCodigo.Text.Trim(), out int codigo)) return;

            using (var contexto = new TOCC8ProdutoContext())
            {
                var p = contexto.Produtos.FirstOrDefault(x => x.Codigo == codigo);
                if (p != null)
                {
                    txtDescricao.Text = p.Descricao ?? "";
                    dtValidade.Value = p.DataValidade ?? DateTime.Today;
                    txtPreco.Text = (p.Preco ?? 0).ToString("0.00");
                    txtTaxa.Text = (p.TaxaLucro ?? 0).ToString("0.####");
                }
                else
                {
                    txtDescricao.Clear();
                    txtPreco.Clear();
                    txtTaxa.Clear();
                    dtValidade.Value = DateTime.Today;
                    txtDescricao.Focus();
                }
            }
        }

        
        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvDados.Rows[e.RowIndex];
            var val = (row.Cells["codigo"] ?? row.Cells[0])?.Value;
            if (val == null || !int.TryParse(val.ToString(), out int codigo)) return;

            using (var contexto = new TOCC8ProdutoContext())
            {
                var p = contexto.Produtos.FirstOrDefault(x => x.Codigo == codigo);
                if (p != null)
                {
                    txtCodigo.Text = p.Codigo.ToString();
                    txtDescricao.Text = p.Descricao ?? "";
                    dtValidade.Value = p.DataValidade ?? DateTime.Today;
                    txtPreco.Text = (p.Preco ?? 0).ToString("0.00");
                    txtTaxa.Text = (p.TaxaLucro ?? 0).ToString("0.####");
                }
            }
        }
    }
}
