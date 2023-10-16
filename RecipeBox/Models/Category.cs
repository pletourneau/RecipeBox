using System.Collections.Generic;

namespace RecipeBox.Models
{
  public class Category
  {
    public int CategoryId { get; set; }
    public string Name { get; set; }
    
    // foreign key
    public List<CategoryRecipe> JoinEntities { get; set;}
  }
}