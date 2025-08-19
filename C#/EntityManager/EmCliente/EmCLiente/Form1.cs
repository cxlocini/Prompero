using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Windows.Forms;

namespace ControleDeProdutos
{
    public partial class Form1 : Form
    {
        // Instância do nosso contexto do banco de dados
        private readonly AppDbContext _context = new AppDbContext();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Garante que o banco de dados seja criado se não existir
            _context.Database.EnsureCreated();
            ConfigurarGrid();
            ListarTodosProdutos();
        }

        // Configura as colunas do DataGridView
        private void ConfigurarGrid()
        {
            dgvProdutos.AutoGenerateColumns = false;
            dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Codigo", HeaderText = "Código" });
            dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Descricao", HeaderText = "Descrição", Width = 250 });
            dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DataValidade", HeaderText = "Data Validade" });
            dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrecoFinal", HeaderText = "Preço Final (R$)", DefaultCellStyle = { Format = "N2" } });
            dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrazoValidade", HeaderText = "Prazo Validade (dias)" });
        }

        private void LimparCampos()
        {
            txtCodigo.Clear();
            txtDescricao.Clear();
            dtpDataValidade.Value = DateTime.Now;
            numPreco.Value = 0;
            numTaxaLucro.Value = 0;
            txtDescricao.Focus();
        }

        private void PreencherCampos(Produto produto)
        {
            txtCodigo.Text = produto.Codigo.ToString();
            txtDescricao.Text = produto.Descricao;
            dtpDataValidade.Value = produto.DataValidade;
            numPreco.Value = (decimal)produto.Preco;
            numTaxaLucro.Value = (decimal)produto.TaxaLucro;
        }

        private void AtualizarGrid(List<Produto> produtos)
        {
            dgvProdutos.DataSource = null;
            dgvProdutos.DataSource = produtos;
            dgvProdutos.Refresh();
        }

        private void ListarTodosProdutos()
        {
            var produtos = _context.Produtos.AsNoTracking().ToList();
            AtualizarGrid(produtos);
        }

        // --- EVENTOS DOS COMPONENTES ---

        private void btnListar_Click(object sender, EventArgs e)
        {
            ListarTodosProdutos();
        }

        private void txtDescricao_KeyUp(object sender, KeyEventArgs e)
        {
            string textoBusca = txtDescricao.Text.ToLower().Trim();
            if (string.IsNullOrEmpty(textoBusca))
            {
                ListarTodosProdutos();
            }
            else
            {
                var produtosFiltrados = _context.Produtos
                    .Where(p => p.Descricao != null && p.Descricao.ToLower().Contains(textoBusca))
                    .AsNoTracking()
                    .ToList();
                AtualizarGrid(produtosFiltrados);
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(txtCodigo.Text, out int codigo))
            {
                var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.Codigo == codigo);
                if (produto != null)
                {
                    PreencherCampos(produto);
                }
            }
        }

        private void dgvProdutos_DoubleClick(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow != null)
            {
                var produtoSelecionado = dgvProdutos.CurrentRow.DataBoundItem as Produto;
                if (produtoSelecionado != null)
                {
                    PreencherCampos(produtoSelecionado);
                }
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Se o campo código tem um valor, é uma ATUALIZAÇÃO
                if (int.TryParse(txtCodigo.Text, out int codigo))
                {
                    var produtoExistente = _context.Produtos.Find(codigo);
                    if (produtoExistente != null)
                    {
                        produtoExistente.Descricao = txtDescricao.Text;
                        produtoExistente.DataValidade = dtpDataValidade.Value;
                        produtoExistente.Preco = (double)numPreco.Value;
                        produtoExistente.TaxaLucro = (double)numTaxaLucro.Value;
                        _context.Produtos.Update(produtoExistente);
                        MessageBox.Show("Produto atualizado com sucesso!");
                    }
                }
                // Senão, é uma CRIAÇÃO
                else
                {
                    var novoProduto = new Produto
                    {
                        Descricao = txtDescricao.Text,
                        DataValidade = dtpDataValidade.Value,
                        Preco = (double)numPreco.Value,
                        TaxaLucro = (double)numTaxaLucro.Value
                    };
                    _context.Produtos.Add(novoProduto);
                    MessageBox.Show("Produto cadastrado com sucesso!");
                }
                _context.SaveChanges();
                LimparCampos();
                ListarTodosProdutos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao salvar: {ex.Message}");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtCodigo.Text, out int codigo))
            {
                var produtoParaExcluir = _context.Produtos.Find(codigo);
                if (produtoParaExcluir != null)
                {
                    var result = MessageBox.Show("Tem certeza que deseja excluir este produto?", "Confirmação", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        _context.Produtos.Remove(produtoParaExcluir);
                        _context.SaveChanges();
                        MessageBox.Show("Produto excluído com sucesso!");
                        LimparCampos();
                        ListarTodosProdutos();
                    }
                }
                else
                {
                    MessageBox.Show("Produto não encontrado.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um produto para excluir.");
            }
        }

        private void btnGrafico_Click(object sender, EventArgs e)
        {
            // Pega todos os produtos para exibir no gráfico
            var todosProdutos = _context.Produtos.AsNoTracking().ToList();
            if (todosProdutos.Any())
            {
                FormGrafico formGrafico = new FormGrafico(todosProdutos);
                formGrafico.ShowDialog();
            }
            else
            {
                MessageBox.Show("Não há dados para exibir no gráfico.");
            }
        }
    }
}