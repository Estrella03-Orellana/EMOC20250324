using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMOC20250324.AppWebMVC.Models;

public partial class User
{
    public int Id { get; set; }


    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    [Display(Name = "Usuario")]
    public string Username { get; set; } = null!;


    [Required(ErrorMessage = "El email es obligatorio.")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El password es obligatorio.")]
    [DataType(DataType.Password)]
    [StringLength(40, MinimumLength = 5, ErrorMessage = "El passowrd debe tener entre 5 y 50 caracteres.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "El role es obligatorio.")]
    [Display(Name = "Rol")]
    public string Role { get; set; } = null!;


    [Display(Name = "Nota")]
    public string? Notes { get; set; }
}
