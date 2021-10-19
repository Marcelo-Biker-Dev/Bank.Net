using System;
using System.Collections.Generic;

namespace Bank.Net.Entities
{
    public class Conta
    {
        public List<AtividadesNaConta> transacoes = new List<AtividadesNaConta>();
		protected LinkedList<Credito> movimentosNoSaldoDeCredito = new List<Credito>();
		protected static string inicioDeConta = "INICIO DE CONTA";
		private protected string Nome { get; set; }
		private protected string Id { get; set; }
		private protected bool iniciandoConta { get; set;}
		private protected decimal DepositoInicial { get; set; }

		private protected decimal CreditoInicial
		{
			get
			{
				return Credito.credito(this.Id);
			}
		}

		private protected decimal SaldoDeCredito
		{
			get
			{
				decimal credito = this.CreditoInicial;

				if (movimentosNoSaldoDeCredito.Count != 0)
				{
					foreach (var item in movimentosNoSaldoDeCredito)
					{
						credito += item.getValor();
					}
				}
				
				return credito;
			}
		}
		
		private protected decimal Saldo
		{
			get
			{
				decimal saldo = 0;

				if (transacoes.Count > 0)
				{
					foreach (var item in transacoes)
					{
						saldo += item.getValor();
					}
				}

				return saldo;
			}
		}

		public Conta(){}

		protected abstract string GeradorDeIdDeContas();

		public void Sacar(decimal valorSaque, DateTime data, string observacao)
		{
            // Validação de valor permitido para saque
			if (valorSaque <=0)
			{
				throw new ArgumentOutOfRangeException(nameof(valorSaque), "Valor do saque deve ser maior do que zero");
			}

            // Verificação de saldo suficiente para saque
			if (this.Saldo >= valorSaque)
			{
				var saqueSaldoSuficiente = new AtividadesNaConta(-valorSaque, DateTime.Now, observacao);
				transacoes.Add(saqueSaldoSuficiente);

				Console.WriteLine($"Saque realizado com sucesso. Saldo atual da conta {this.Id} de {this.Nome} é {this.Saldo}");
				return;
			}

			// Verificação de possibilidade de saque com saldo + crédito
            decimal saldoComLimite = this.Saldo + this.SaldoDeCredito;
			if (valorSaque > saldoComLimite)
			{
				throw new InvalidOperationException($"Saldo não suficiente para saque.\nSaldo atual: {this.Saldo}\nSaldo de crédito: {this.SaldoDeCredito}");
			}

			// Retirada efetuada utilizando apenas saldo do crédito
			else if (this.Saldo == 0)
			{
				var saqueSomenteDoCredito = new Credito(-valorSaque, DateTime.Now, ($"retirada de {valorSaque}"));
				movimentosNoSaldoDeCredito.Add(saqueSomenteDoCredito);

				Console.WriteLine($"Saque realizado com sucesso. Saldo atual da conta {this.Id} de {this.Nome} é {this.Saldo} e o saldo de crédito é {this.SaldoDeCredito}");
				return;
			}

			// Realização do saque a utilizar o Saldo + Saldo de crédito
			else
			{
				decimal valorDebitarDoCredito = valorSaque - this.Saldo;
				decimal valor = this.Saldo;
					
				var saque = new AtividadesNaConta(-valor, DateTime.Now, observacao);
				transacoes.Add(saque);
				
				var saqueDoCredito = new Credito(-valorDebitarDoCredito, DateTime.Now, ($"debitado {valorDebitarDoCredito} do crédito em uma retirada de {valorSaque}"));
				movimentosNoSaldoDeCredito.Add(saqueDoCredito);
				
				Console.WriteLine($"Saque realizado com sucesso. Saldo atual da conta {this.Id} de {this.Nome} é {this.Saldo} e o saldo de crédito é {this.SaldoDeCredito}");
			}

		}

		public void Depositar(decimal valorDeposito, DateTime data, string observacao)
		{	
            // Validação de valor permitido para deposito
			if (valorDeposito <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(valorDeposito), "Valor do depósito deve ser maior do que zero");
			}

			// Variável para valor utilizado do saldo de crédito
			var diferencaReporSaldoDeCredito = this.CreditoInicial - this.SaldoDeCredito;

			// Deposita direto sobre saldo normal
			if(diferencaReporSaldoDeCredito == 0)
			{
				var deposito = new AtividadesNaConta(valorDeposito, DateTime.Now, observacao);
				transacoes.Add(deposito);

				Console.WriteLine($"Depósito realizado com sucesso. Saldo atual da conta {this.Id} de {this.Nome} é {this.Saldo}.");
				return;
			}

			// Repõe saldo de crédito
			else if (diferencaReporSaldoDeCredito > 0 && valorDeposito <= diferencaReporSaldoDeCredito)
			{
				var repoeCredito = new Credito(valorDeposito, DateTime.Now, observacao);
				movimentosNoSaldoDeCredito.Add(repoeCredito);

				Console.WriteLine($"Depósito realizado com sucesso. Saldo atual da conta {this.Id} de {this.Nome} é {this.Saldo} e o saldo de crédito é {this.SaldoDeCredito}");
				return;
			}
				
			// Repõe saldo de crédito e deposita restante no saldo normal
			else
			{
				var aposReporCredito = valorDeposito - diferencaReporSaldoDeCredito;

				var reporCredito = new Credito(diferencaReporSaldoDeCredito, DateTime.Now, observacao);
				movimentosNoSaldoDeCredito.Add(reporCredito);

				var reporSaldo = new AtividadesNaConta(aposReporCredito, DateTime.Now, observacao);
				transacoes.Add(reporSaldo);

				Console.WriteLine($"Depósito realizado com sucesso. Saldo atual da conta {this.Id} de {this.Nome} é {this.Saldo} e o saldo de crédito é {this.SaldoDeCredito}");
				return;
			} 
		}

		public string getNome()
        {return this.Nome; }

		public string getId()
        {return this.Id; }

		public decimal getSaldo()
        {return this.Saldo; }

		public decimal getSaldoDeCredito()
        {return this.SaldoDeCredito; }

        public override string ToString()
		{
			string movimentos = "";
			foreach (var t in transacoes)
			{
				movimentos += t.ToString();
			}

			string retorno = "";
            retorno += "\nConta Id: " + this.Id + " | ";
            retorno += "Nome " + this.Nome + " | ";
            retorno += "Saldo " + this.Saldo + " | ";
            retorno += "Crédito " + this.SaldoDeCredito + "\n\n";
			retorno += "Transações:" + movimentos;

			return retorno;
		}
    }
}