using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using RecipeBox.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBox.Controllers
{
  [Authorize]
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    // public RecipesController(RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    // public ActionResult Index()
    // { 
    //   ViewBag.PageTitle = "List of Recipes";
    //   List<Recipe> model = _db.Recipes.ToList();
    //   return View(model);
    // }

    public async Task<ActionResult> Index()
    {
      ViewBag.PageTitle = "List of Recipes";
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      List<Recipe> userRecipes = _db.Recipes
                                        .Where(entry => entry.User.Id == currentUser.Id)
                                        // .Include(recipe.Category)
                                        .ToList(); 
      return View(userRecipes);
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe, int CategoryId)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.PageTitle = "Add a new receta guey";
        ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
        return View(recipe);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        recipe.User = currentUser;
        _db.Recipes.Add(recipe);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    public ActionResult Create()
    {
      ViewBag.PageTitle = "Add a new receta guey";
      return View();
    }

    // [HttpPost]
    // public ActionResult Create(Recipe recipe)
    // {
    //   if (!ModelState.IsValid)
    //   {
    //     return View(recipe);
    //   }
    //   _db.Recipes.Add(recipe);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }

    public ActionResult Details(int id)
    {
      ViewBag.PageTitle = "Recipe Details";
      Recipe targetRecipe = _db.Recipes.Include(entry => entry.JoinEntities)
                                       .ThenInclude(join => join.Category)
                                       .FirstOrDefault(entry => entry.RecipeId == id);
      return View(targetRecipe);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.PageTitle = "Edit Recipe";
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult Edit(Recipe recipe)
    {
      _db.Recipes.Update(recipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      ViewBag.PageTitle = "Delete Recipe";
      Recipe targetRecipe = _db.Recipes.FirstOrDefault(entry => entry.RecipeId == id);
      return View(targetRecipe);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Recipe targetRecipe = _db.Recipes.FirstOrDefault(entry => entry.RecipeId == id);
      _db.Recipes.Remove(targetRecipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCategory(int id)
    {
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(model => model.RecipeId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult AddCategory(Recipe recipe, int categoryId)
    {
      #nullable enable
      CategoryRecipe? joinEntity = _db.CategoryRecipes.FirstOrDefault(join =>(join.CategoryId == categoryId && join.RecipeId == recipe.RecipeId));
      #nullable disable
      if (joinEntity == null && categoryId != 0)
      {
        _db.CategoryRecipes.Add(new CategoryRecipe() {RecipeId = recipe.RecipeId, CategoryId = categoryId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new {id = recipe.RecipeId});
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      CategoryRecipe joinEntry = _db.CategoryRecipes.FirstOrDefault(entry => entry.CategoryRecipeId == joinId);
      _db.CategoryRecipes.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.RecipeId });
    }
  }

}