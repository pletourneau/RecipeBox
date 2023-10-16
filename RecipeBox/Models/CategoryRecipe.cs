namespace RecipeBox.Models
{
  public class CategoryRecipe
  {
    public int CategoryRecipeId { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
  }
}