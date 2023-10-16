using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Models 
{
  public class RecipeBoxContext : DbContext
  {
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryRecipe> CategoryRecipes { get; set; }

    public RecipeBoxContext(DbContextOptions options) : base(options) {}
  }
}