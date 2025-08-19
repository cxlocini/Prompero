using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeProdutos
{
    [Table("produto")]
    public class Produto
    {
        [Key]
        [Column("codigo")]
        public int Codigo { get; set; }

        [Column("descricao")]
        [MaxLength(100)]
        public string? Descricao { get; set; }

        [Column("datavalidade")]
        public DateTime DataValidade { get; set; }

        [Column("preco")]
        public double Preco { get; set; }

        [Column("taxalucro")]
        public double TaxaLucro { get; set; }

        
        [NotMapped]
        public double PrecoFinal
        {
            get { return Preco * (1 + (TaxaLucro / 100)); }
        }

       
        [NotMapped]
        public int PrazoValidade
        {
            
            get
            {
                var dias = (DataValidade.Date - DateTime.Now.Date).Days;
                return dias > 0 ? dias : 0;
            }
        }

        
        [NotMapped]
        public double LucroEmReais
        {
            get { return PrecoFinal - Preco; }
        }
    }
}