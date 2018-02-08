using System;

namespace Fatec.RD.Bussiness.Inputs
{
    public class DespesaInput
    {
        public int IdTipoDespesa { get; set; }
        public int IdTipoPagamento { get; set; }
        public Decimal Valor { get; set; }
        public string Comentario { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataCriacao => DateTime.Now;
    }

}
