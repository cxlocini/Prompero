using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("produto")]               
public class Produto
{
    [Key]
    [Column("codigo")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Codigo { get; set; }

    [Column("descricao")]
    [MaxLength(100)]
    public string Descricao { get; set; }

    [Column("datavalidade", TypeName = "date")]
    public DateTime? DataValidade { get; set; }

    [Column("preco")]
    public double? Preco { get; set; }

    [Column("taxalucro")]
    public double? TaxaLucro { get; set; }

    // calculados (não mapeados)
    [NotMapped] public double PrecoFinal => (Preco ?? 0) * (1 + (TaxaLucro ?? 0));
    [NotMapped] public double LucroReais => (Preco ?? 0) * (TaxaLucro ?? 0);
    [NotMapped]
    public int PrazoValidadeDias =>
        DataValidade.HasValue ? (DataValidade.Value.Date - DateTime.Today).Days : 0;
}
