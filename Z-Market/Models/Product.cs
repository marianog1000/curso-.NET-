using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Z_Market.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [StringLength(30,ErrorMessage ="The Field {0} must be between {2} and {1} characteres", MinimumLength =3)]
        [Required(ErrorMessage ="You must enter the field {0}")]
        [Display(Name ="Product Description")]
        public string Descripcion { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode =false)]
        [Required(ErrorMessage ="You must enter the field {0}")]
        public decimal Price { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Last Buy")]
        public DateTime LastBuy { get; set; }


        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString ="{0:N2}",ApplyFormatInEditMode = false)]
        public float Stock { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [JsonIgnore]
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}