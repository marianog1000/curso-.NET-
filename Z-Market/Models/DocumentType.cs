using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class DocumentType
    {
        [Key]
        [Display(Name = "Document Type")]
        public int DocumentTypeID { get; set; }

        [Display(Name = "Tipo de Documento")]
        public string Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Customers> Customers { get; set; }

    }
}