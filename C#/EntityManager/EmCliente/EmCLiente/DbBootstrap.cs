using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace EmCRUD
{
    internal static class DbBootstrap
    {
        
        public static void EnsureProdutoTableAndSeed()
        {
            using (var ctx = new TOCC8Entities())
            {
                /
                if (!ctx.Database.Exists())
                    throw new Exception("Não foi possível conectar ao banco TOCC8. Verifique App.config, usuário/senha e se o DB existe.");

                
                ctx.Database.ExecuteSqlCommand(@"
                    CREATE TABLE IF NOT EXISTS public.produto(
                      codigo       SERIAL PRIMARY KEY,
                      descricao    VARCHAR(100),
                      datavalidade DATE,
                      preco        DOUBLE PRECISION,
                      taxalucro    DOUBLE PRECISION
                    );
                ");

                
                var count = ctx.Database.SqlQuery<int>("SELECT COUNT(*) FROM public.produto;").Single();
                if (count == 0)
                {
                    ctx.Database.ExecuteSqlCommand(@"
                        INSERT INTO public.produto (descricao, datavalidade, preco, taxalucro) VALUES
                        ('Arroz 5kg','2025-12-31',25.90,0.20),
                        ('Feijão 1kg','2025-10-15',8.50,0.30),
                        ('Café 500g','2025-09-10',17.40,0.25);
                    ");
                }
            }
        }

      
        public static bool TryInitWithMessage()
        {
            try
            {
                EnsureProdutoTableAndSeed();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro de banco de dados ao inicializar:\n\n" + ex.Message +
                    "\n\nDicas:\n- Confirme App.config (connectionStrings)\n- Confirme se o DB TOCC8 existe\n- Verifique usuário/senha/porta",
                    "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
