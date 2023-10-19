using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Category
  {
    public int CategoryId { get; set; }
    [Required(ErrorMessage = "Your category needs a name!")]
    public string Name { get; set; }
    
    // foreign key
    public List<CategoryRecipe> JoinEntities { get; set;}
  }
}