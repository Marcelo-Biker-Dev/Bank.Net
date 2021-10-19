using System;

namespace Bank.Net.Entities
{
    public class AtividadesNaConta
    {
        private decimal Valor { get; }
        private DateTime Data { get; }
        private string Observacao { get; }

        public AtividadesNaConta(decimal valor, DateTime data, string observacao)
        {
            this.Valor = valor;
            this.Data = data;
            this.Observacao = observacao;
        }

        public decimal getValor()
        {return this.Valor; }

        
        public override string ToString()
        {
            string retorno = "";
            retorno += "\t Valor da operação: " + this.Valor + " | ";
            retorno += "Data da operação: " + this.Data.ToString() + " | ";
            retorno += "Informação sobre a operação: " + this.Observacao + "\n\t";
            return retorno;
		}
    }
}