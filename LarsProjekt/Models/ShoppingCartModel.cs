using LarsProjekt.Application;
using LarsProjekt.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarsProjekt.Models;

public partial class ShoppingCartModel
{
    public List<ShoppingCartItemModel> Items { get; set; }

}