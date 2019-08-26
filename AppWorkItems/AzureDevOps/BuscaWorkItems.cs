using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppWorkItems.Entity;
using AppWorkItems.Servicos;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace AppWorkItems.AzureDevOps
{
    public class BuscaWorkItems
    {
        List<Items> lista = new List<Items>();
        public List<Items> BuscarWorkItens(string urlOrganizacao, string token, string projeto)
        {
            Uri orgUrl = new Uri(urlOrganizacao);         // Organization URL, for example: https://dev.azure.com/fabrikam               


            VssConnection connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, token));

            BuscarDetalhesWorkItems(connection, projeto).Wait();

            return lista;
        }

        private async Task BuscarDetalhesWorkItems(VssConnection connection, string projeto)
        {
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                WorkItemService service = new WorkItemService();
                int ultimoId = service.BuscarUltimoIdInserido();
                List<WorkItem> workItems = new List<WorkItem>();
                for (int i = ultimoId + 1; i <= ultimoId + 200; i++)
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

                lista = CarregarItemsEncontrados(workItems);
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
