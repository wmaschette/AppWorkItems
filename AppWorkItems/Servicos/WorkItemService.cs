using AppWorkItems.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AppWorkItems.Servicos
{
    public class WorkItemService
    {
        public bool SalvarWorkItems(List<Items> lista)
        {
            bool retorno = true;

            try
            {
                using (var db = new AzureContext())
                {
                    foreach (Items item in lista)
                    {
                        db.Items.Add(item);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                retorno = false;
            }

            return retorno;
        }
        public int BuscarUltimoIdInserido()
        {
            int retorno = 1;
            try
            {
                using (var db = new AzureContext())
                {
                    retorno = db.Items.LastOrDefault().WorkItemId;
                    return retorno;
                }
            }
            catch (Exception)
            {
                retorno = 0;
            }

            return retorno;
        }
    }
}
