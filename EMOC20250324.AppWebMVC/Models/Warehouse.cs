using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMOC20250324.AppWebMVC.Models;

public partial class Warehouse
{
    public int Id { get; set; }

    [Display(Name = "Bodega")]
    [Required(ErrorMessage = "La bodega es obligatoria.")]
    public string WarehouseName { get; set; } = null!;

    [Required(ErrorMessage = "La nota es obligatoria.")]
    [Display(Name = "Nota")]
    public string? Notes { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
