using System;
using System.Collections.Generic;
using Bank.Net.Entities;

namespace Bank.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            var contas = new List<Conta>{};
			Conta contaSimples;
			Conta contaEmpresa;

			var opcaoUsuario = ObterOpcaoUsuario();

			while (opcaoUsuario != "X")
			{
				switch (opcaoUsuario)
				{
					case "1":
						IniciarConta();
						break;
					case "2":
						ListarContas();
						break;
					case "3":
						VerificarSaldo();
						break;
					case "4":
						Sacar();
						break;
					case "5":
						Depositar();
						break;
					case "6":
						Transferir();
						break;
                    case "7":
						ListarTransacoes();
						break;
                    case "C":
						Console.Clear();
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(opcaoUsuario), "Opção informada não é válida! Tente novamente com uma das opções mostradas.");
				}
				ObterOpcaoUsuario();
			}
			
			string ObterOpcaoUsuario()
			{
				Console.WriteLine();
				Console.WriteLine("Bank_dotNet a seu dispor!");
				Console.WriteLine("Informe a opção desejada:");
				Console.WriteLine();
				Console.WriteLine("1- Iniciar conta");
				Console.WriteLine("2- Listar contas");
				Console.WriteLine("3- Verificar saldo");
				Console.WriteLine("4- Sacar");
				Console.WriteLine("5- Depositar");
				Console.WriteLine("6- Transferir valores entre contas");
				Console.WriteLine("7- Relatório de Transacoes");
				Console.WriteLine("C- Limpar Tela");
				Console.WriteLine("X- Sair");
				Console.WriteLine();

				opcaoUsuario = Console.ReadLine().ToUpper();
				return opcaoUsuario;
			}

			Console.WriteLine("Obrigado por utilizar nossos serviços.");


			void IniciarConta()
			{
				Console.WriteLine("\nEscolha o tipo de conta a iniciar");

				Console.Write("Digite 1 para Conta Simples ou 2 para Conta Empresa: ");
				int entradaTipoConta = int.Parse(Console.ReadLine());

				Console.Write("\nDigite o Nome do Cliente: ");
				string entradaNome = Console.ReadLine();

				Console.Write("\nQual o valor do deposito inicial: ");
				decimal entradaDepositoInicial = decimal.Parse(Console.ReadLine());
				Console.WriteLine();

				if (entradaTipoConta == 1)
				{
					contaSimples = new ContaSimples(entradaDepositoInicial, entradaNome);
					contas.Add(contaSimples);
				}

				if (entradaTipoConta == 2)
				{
					contaEmpresa = new ContaEmpresa(entradaDepositoInicial, entradaNome);
					contas.Add(contaEmpresa);
				}
			}

			void ListarContas()
			{
				if (contas.Count == 0)
				{
					Console.WriteLine("\nNenhuma conta cadastrada. Inicie uma conta no Bank_dotNet.");
					return;
				}

				Console.Write("\nDigite o Nome do Cliente: ");
				string entradaNome = Console.ReadLine();

				var constaConta = 0;
				foreach (var conta in contas)
				{
					if (conta.getNome() == entradaNome)
					{
						Console.WriteLine($"\nConta {conta.getId()}");
						Console.WriteLine("Saldo: " + conta.getSaldo());
						constaConta ++;
					}
				}	

				if (constaConta == 0)
				{
					Console.WriteLine("\nNenhuma conta cadastrada para o nome informado. Tente novamente");
		
				}
			}	

			void Depositar()
			{
				Console.Write("Digite a identificação da conta: ");
				string idConta = Console.ReadLine().ToUpper();

				Console.Write("Digite o valor a ser depositado: ");
				decimal valorDeposito = decimal.Parse(Console.ReadLine());

				Console.Write("Insira alguma anotação ao depósito: ");
				string anotacao = Console.ReadLine();

				foreach (var conta in contas)
				{
					if (conta.getId() == idConta)
					{
						conta.Depositar(valorDeposito, DateTime.Now , anotacao);
					}
				}
			}

			void Sacar()
			{
				Console.Write("Digite a identificação da conta: ");
				string idConta = Console.ReadLine().ToUpper();

				Console.Write("Digite o valor a ser retirado: ");
				decimal valorSaque = decimal.Parse(Console.ReadLine());

				Console.Write("Insira alguma anotação à retirada: ");
				string anotacao = Console.ReadLine();

				foreach (var conta in contas)
				{
					if (conta.getId() == idConta)
					{
						conta.Sacar(valorSaque, DateTime.Now , anotacao);
					}
				}
			}
			
			void VerificarSaldo()
			{
				Console.Write("\nDigite a identificação da conta: ");
				string idConta = Console.ReadLine().ToUpper();

				foreach (var conta in contas)
				{
					if (conta.getId() == idConta)
					{
						Console.WriteLine($"\nO saldo da conta #{conta.getId()} é: {conta.getSaldo()}");
					}
				}
			}

			void Transferir()
			{
				Console.Write("Digite a identificação da conta de origem: ");
				string idContaOrigem = Console.ReadLine().ToUpper();

				Console.Write("Digite a identificação da conta de destino: ");
				string idContaDestino = Console.ReadLine().ToUpper();

				Console.Write("Digite o valor a ser transferido: ");
				decimal valorTransferencia = decimal.Parse(Console.ReadLine());

				Console.Write("Insira alguma anotação à transferência: ");
				string anotacao = Console.ReadLine();

				bool saqueOk = false;

				foreach (var conta in contas)
				{
					if (conta.getId() == idContaOrigem)
					{
						conta.Sacar(valorTransferencia, DateTime.Now , anotacao);
					}
					saqueOk = true;
				}

				if (saqueOk == true)
				{
					foreach (var conta in contas)
					{
						if (conta.getId() == idContaDestino)
						{
							conta.Depositar(valorTransferencia, DateTime.Now , anotacao);
						}
					}
				}

				Console.WriteLine($"\nTransferência no valor de {valorTransferencia} da conta de origem {idContaOrigem} para a conta destino {idContaDestino} realizada com sucesso.");
			}

			void ListarTransacoes()
			{			
				foreach (var c in contas)
				{
					System.Console.WriteLine(c.ToString());
				}
			}
        }
    }
}
