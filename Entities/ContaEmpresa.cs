using System;

namespace Bank.Net.Entities
{
    public class ContaEmpresa : Conta
    {
        private static int geradorDeNumeracaoDeContas = 1001; // Usado para gerar novos de números de Contas Simples e Contas Empresa
		private static string prefixoContaEmpresa = "E"; // Usado para gerar novos de números de Contas Simples e Contas Empresa

		public ContaEmpresa(decimal depositoInicial, string nome)
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
			string id = prefixoContaEmpresa + geradorDeNumeracaoDeContas.ToString();
            geradorDeNumeracaoDeContas++;
        
			return id;
		}
    }
}