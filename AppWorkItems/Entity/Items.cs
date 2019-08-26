using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWorkItems.Entity
{
    public class Items
    {
        [Key]
        public int InternalId { get; set; }
        public int WorkItemId { get; set; }
        public string Tipo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
