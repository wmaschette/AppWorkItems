using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppWorkItems.Entity;
using AppWorkItems.AzureDevOps;
using AppWorkItems.Servicos;

namespace AppWorkItems
{
    class Program
    {
        public static string url , projeto, token;
        static void Main(string[] args)
        {
            int tentativa = 0;
            bool pedeDados = true;
            while (tentativa < 20)
            {
                if (pedeDados)
                {
                    Console.WriteLine("Insira a url do AzureDevOps: ");
                    url = Console.ReadLine().ToString();
                    Console.WriteLine("Insira o projeto: ");
                    projeto = Console.ReadLine().ToString();
                    Console.WriteLine("Insira o seu token pessoal: ");
                    token = Console.ReadLine().ToString();
                    Console.WriteLine("Buscando dados na API do Azure DevOps...");
                }

                BuscaWorkItems busca = new BuscaWorkItems();
                var lista = busca.BuscarWorkItens(url, token, projeto);

                if (lista.Count > 0)
                {
                    WorkItemService service = new WorkItemService();
                    service.SalvarWorkItems(lista);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Nenhuma Work Item encontrada para esse endereço e projeto.");
                    Console.WriteLine("Escolha a opção que deseja: 1- Tentar novamente com outros dados / 2- Tentar novamente com os mesmos dados / 3- Sair");
                    var retornoInteracao = Console.ReadLine();
                    if(retornoInteracao.Equals("1"))
                    {
                        tentativa++;
                        pedeDados = true;
                    }
                    else if (retornoInteracao.Equals("2"))
                    {
                        tentativa++;
                        pedeDados = false;
                    }
                    else
                    {
                        Console.Write("Processo finalizado!");
                    }
                }
            }
        }
    }
}
