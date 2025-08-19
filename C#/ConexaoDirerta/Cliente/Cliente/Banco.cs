
//menu Ferramentas - Gerenciador de pacotes do Nuget - gerenciar pacotes do nuget - intalar - npgsql 
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public class Banco
    {
        public static NpgsqlConnection conexao;
        public NpgsqlCommand comando;
        public NpgsqlDataReader reader; // tabela no formato postgres
        public DataTable tabela;

        public Banco()
        {
            try
            {
                if((conexao==null)||(conexao.State != ConnectionState.Open))
                {
conexao = new NpgsqlConnection("Server=127.0.0.1;Port=5432;" +
    "User Id=postgres;Password=ifsp;Database=TOCC8;");
                    conexao.Open();
                }
                comando = new NpgsqlCommand();
                comando.Connection = conexao;
            }
            catch (Exception ex)
            {
                
                throw new Exception("Erro na conexão com o banco: " + ex.Message);
                //dispara um exceção
            }
        }
    }
}
