using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using RecipeBox.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Controllers
{
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    public RecipesController(RecipesBoxContext db)
    {
      _db = db;
    }
    
    public ActionResult Index()
    {
      List<Recipe> model = _db.Recipes.ToList(); 
      return View(model);
    }
  }
}