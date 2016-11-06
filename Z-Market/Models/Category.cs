using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [StringLength(30, ErrorMessage = "The Field {0} must be between {2} and {1} characteres", MinimumLength = 3)]
        [Required(ErrorMessage = "You must enter the field {0}")]
        [Display(Name = "Category Description")]
        public string Descripcion { get; set; }

    }
}