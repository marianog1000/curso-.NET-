using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    // [Table("Empleados")] para cambiar el nombre de la tabla en la bd
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        // [Column("Primer Nombre")] para cambiar el nombre del campo en la base de datos
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage ="The field {0} must be between {1} and {2} characters ", MinimumLength =3)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {1} and {2} characters ", MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Salario")]
        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Salary { get; set; }

        [Display(Name = "Bonus %")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public float BonusPercent { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Start Time")]
        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime StarTime { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        public string URL { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "You must enter {0}")]
        // [ForeignKey("nombre")] el nombre del campo en la otra tabla
        public int DocumentTypeID { get; set; }

        [NotMapped] // para que no se guarde en la base de datos
        public int Age { get { return DateTime.Now.Year - DateOfBirth.Year; } }


        public virtual DocumentType DocumentType { get; set; }


    }
}