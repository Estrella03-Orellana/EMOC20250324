using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EMOC20250324.AppWebMVC.Models;

public partial class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nonbre del producto es obligatorio.")]
    [Display(Name = "Producto")]
    public string ProductName { get; set; } = null!;

    [Required(ErrorMessage = "La descripcion es obligatoria.")]
    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(typeof(decimal), "0.001", "99999999.99", ErrorMessage = "El precio debe estar entre 0.01 y 99999999.99")]

    [Display(Name = "Precio")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "El precio de la compra es obligatorio.")]
    [Range(typeof(decimal), "0.001", "99999999.99", ErrorMessage = "El precio debe estar entre 0.01 y 99999999.99")]
    [Display(Name = "Precio de compra")]
    public decimal PurchasePrice { get; set; }

    [Display(Name = "Bodega")]
    [Required(ErrorMessage = "La bodega es obligatoria.")]
    public int? WarehouseId { get; set; }

    [Display(Name = "Marca")]
    [Required(ErrorMessage = "La marca es obligatoria.")]
    public int? BrandId { get; set; }

    [Display(Name = "Nota adicional")]
    [Required(ErrorMessage = "La nota adicional es obligatoria.")]
    public string? Notes { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
