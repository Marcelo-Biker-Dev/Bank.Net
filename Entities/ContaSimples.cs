using System;

namespace Bank.Net.Entities
{
    public class ContaSimples : Conta
    {
        
		private static int geradorDeNumeracaoDeContas = 1001; // Usado para gerar novos de números de Contas Simples e Contas Empresa
		private static string prefixoContaSimples = "S"; // Usado para gerar novos de números de Contas Simples e Contas Empresa

		public ContaSimples(decimal depositoInicial, string nome)
		{
			this.DepositoInicial = depositoInicial;
			this.Nome = nome;
			this.Id = GeradorDeIdDeContas();
			System.Console.WriteLine("dentro do construtor");
 			Depositar(this.DepositoInicial, DateTime.Now, inicioDeConta);
			System.Console.WriteLine($"Conta #{this.Id} iniciada com sucesso!");
		}

		protected override string GeradorDeIdDeContas()
		{
			string id = prefixoContaSimples + geradorDeNumeracaoDeContas.ToString();
			geradorDeNumeracaoDeContas++;

			return id;
		}
    }
}