using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMOC20250324.AppWebMVC.Models;

public partial class Brand
{
    public int Id { get; set; }

    [Display(Name = "Marca")]
    [Required(ErrorMessage = "La marca es obligatoria.")]
    public string BrandName { get; set; } = null!;

    [Display(Name = "Pais")]
    [Required(ErrorMessage = "El pais es obligatorio.")]
    public string? Country { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
