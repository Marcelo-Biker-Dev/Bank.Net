using System;

namespace Bank.Net.Entities
{
    public class Credito
    {
        private decimal Valor { get; }
        private DateTime Data { get; }
        private string Observacao { get; }

        public Credito(decimal valor, DateTime data, string observacao)
        {
            this.Valor = valor;
            this.Data = data;
            this.Observacao = observacao;
        }

        public static decimal credito(string idConta)
        {
            decimal creditoInicial = 0;

            if (idConta.StartsWith("S"))
            {
                creditoInicial = (decimal)NiveisDeCredito.CreditoPadraoContaSimples;
            }

            if (idConta.StartsWith("E"))
            {
                creditoInicial = (decimal)NiveisDeCredito.CreditoPadraoContaEmpresa;
            }

            return creditoInicial;
        }
        public decimal getValor()
        {return this.Valor; }
    }
}