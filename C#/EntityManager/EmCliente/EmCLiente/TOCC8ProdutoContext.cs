using System.Data.Entity;

public class TOCC8ProdutoContext : DbContext
{
    
    public TOCC8ProdutoContext() : base("name=TOCC8_CodeFirst")
    {
    }

    
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        .
        modelBuilder.HasDefaultSchema("public");
        base.OnModelCreating(modelBuilder);
    }
}