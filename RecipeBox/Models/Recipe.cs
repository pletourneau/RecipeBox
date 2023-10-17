using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public int RecipeId { get; set; }
    public string Title { get; set; }
    public string Ingredients{ get; set; }
    public string Instructions{ get; set; }
    public int Rating { get; set; }

    // foreign key
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public List <CategoryRecipe> JoinEntities { get; set; }
    public ApplicationUser User { get; set; }
  }
}