using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public int RecipeId { get; set; }
    [Required(ErrorMessage = "The recipe must have a title")]
    public string Title { get; set; }
    [Required(ErrorMessage = "The recipe must have ingredients")]
    public string Ingredients{ get; set; }
    [Required(ErrorMessage = "The recipe must have instructions")]
    public string Instructions{ get; set; }
    [Range(1, 5, ErrorMessage = "Give a rating between 1-5!")]
    public int Rating { get; set; }
    // foreign key
    // public int CategoryId { get; set; }
    // public Category Category { get; set; }

    public List <CategoryRecipe> JoinEntities { get; set; }
    public ApplicationUser User { get; set; }
  }
}