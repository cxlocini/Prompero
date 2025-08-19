using System.Data.Entity;

public class TOCC8ProdutoContext : DbContext
{
    
    public TOCC8ProdutoContext() : base("TOCC8_CodeFirst")
    {
        
        Database.ExecuteSqlCommand(@"
            CREATE TABLE IF NOT EXISTS public.produto(
              codigo       SERIAL PRIMARY KEY,
              descricao    VARCHAR(100),
              datavalidade DATE,
              preco        DOUBLE PRECISION,
              taxalucro    DOUBLE PRECISION
            );");
    }

    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(DbModelBuilder mb)
    {
        mb.HasDefaultSchema("public"); 
        base.OnModelCreating(mb);
    }
}
