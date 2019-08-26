using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace ConectaAzureDevOps
{
    public class BuscaWorkItems
    {
        public void BuscarWorkItens(string urlOrganizacao, string token, string projeto)
        {
            Uri orgUrl = new Uri(urlOrganizacao);         // Organization URL, for example: https://dev.azure.com/fabrikam               


            VssConnection connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, token));

            BuscarDetalhesWorkItems(connection, projeto).Wait();
        }

        private async Task BuscarDetalhesWorkItems(VssConnection connection, string projeto)
        {
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                List<WorkItem> workItems = new List<WorkItem>();
                for (int i = 1; i <= 200; i++)
                {
                    try
                    {
                        workItems.Add(await witClient.GetWorkItemAsync(projeto, i));
                    }
                    catch
                    {
                        break;
                    }
                }


                Console.Write("Foram encontradas " + workItems.Count + " Data: " + DateTime.Now.ToString("dd/MM/yyyy hh:MM"));

                var retorno = CarregarItemsEncontrados(workItems);

                WorkItemsRepository rep = new WorkItemsRepository();
                var lista = CarregarItemsEncontrados(workItems);
                rep.SalvarDados(lista);
            }
            catch (Exception ex)
            {
                Console.Write("Ocorreu um erro na busca pelas WorkItems. Data do Erro: " + DateTime.Now.ToString("dd/MM/yyyy hh:MM"));
            }
        }

        public List<Items> CarregarItemsEncontrados(List<WorkItem> workItems)
        {
            List<Items> items = new List<Items>();

            foreach (WorkItem i in workItems)
            {
                string tipo = string.Empty;
                DateTime dataCriacao = new DateTime();

                foreach (var field in i.Fields)
                {
                    if (field.Key == "System.WorkItemType")
                    {
                        tipo = field.Value.ToString();
                    }

                    if (field.Key == "System.CreatedDate")
                    {
                        dataCriacao = Convert.ToDateTime(field.Value);
                    }

                    if (tipo != string.Empty && dataCriacao != new DateTime())
                    {
                        break;
                    }
                }

                items.Add(new Items()
                {
                    WorkItemId = (int)i.Id,
                    Tipo = tipo,
                    DataCriacao = dataCriacao
                });
            }

            return items;
        }
    }
}
