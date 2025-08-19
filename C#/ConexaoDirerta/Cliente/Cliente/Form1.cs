using System;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Cliente cliente;
            ClienteDAO clienteDAO;
            try
            {
                cliente = new Cliente();
                cliente.setNome(txtNome.Text);
                cliente.setIdade(txtIdade.Text);

                clienteDAO = new ClienteDAO();
                if (clienteDAO.gravar(cliente) > 0)
                {
                    MessageBox.Show("Salvo com sucesso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            ClienteDAO clienteDAO;
            try
            {
                clienteDAO = new ClienteDAO();
                dgvDados.DataSource = clienteDAO.listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            ClienteDAO clienteDAO;
            Cliente cliente;
            int codigo;
            try
            {
                if (txtCodigo.Text.Trim().Length > 0)
                {
                    codigo = Convert.ToInt32(txtCodigo.Text);
                    clienteDAO = new ClienteDAO();
                    cliente = clienteDAO.preencher(codigo);
                    if (cliente != null)
                    {
                        txtCodigo.Text = cliente.codigo.ToString();
                        txtNome.Text = cliente.nome;
                        txtIdade.Text = cliente.idade.ToString();
                    }
                    else
                    {
                        txtNome.Clear();
                        txtIdade.Clear();
                        txtNome.Focus();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int linha;
            linha = this.dgvDados.SelectedCells[0].RowIndex;
            this.txtCodigo.Text = this.dgvDados.Rows[linha].Cells[0].Value.ToString();
            this.txtNome.Text = this.dgvDados.Rows[linha].Cells[1].Value.ToString();
            this.txtIdade.Text = this.dgvDados.Rows[linha].Cells[2].Value.ToString();

        }
        private void btnGrafico_Click(object sender, EventArgs e)
        {
            FGrafico f;
            f = new FGrafico();
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog(); //não perde o foco
            
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Cliente cliente;
            ClienteDAO clienteDAO;
            try
            {
                cliente = new Cliente();
                cliente.setCodigo(this.txtCodigo.Text);
                cliente.setNome(txtNome.Text);
                cliente.setIdade(txtIdade.Text);

                clienteDAO = new ClienteDAO();
                if (clienteDAO.alterar(cliente) > 0)
                {
                    MessageBox.Show("Alterado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            Cliente cliente;
            ClienteDAO clienteDAO;
            try
            {
                cliente = new Cliente();
                cliente.setCodigo(this.txtCodigo.Text);

                clienteDAO = new ClienteDAO();
                if (clienteDAO.remover(cliente) > 0)
                {
                    MessageBox.Show("Removido com sucesso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
