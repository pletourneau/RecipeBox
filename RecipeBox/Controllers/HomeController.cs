using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBox.Controllers
{
  public class HomeController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      ViewBag.Title = "Welcome to the Recipe Box!";
      Category[] cats = _db.Categories.ToArray();
      Dictionary<string,object[]> model = new Dictionary<string, object[]>();
      model.Add("categories", cats);
      Recipe[] recipes = _db.Recipes.ToArray();
      model.Add("recipes", recipes);
   
      return View(model);
    }
  
  }
}
  

// [HttpGet("/")]
//     public async Task<ActionResult> Index()
//     {
//       ViewBag.PageTitle = "Welcome to the Recipe Box!";
//       Category[] cats = _db.Categories.ToArray();
//       Dictionary<string,object[]> model = new Dictionary<string, object[]>();
//       model.Add("categories", cats);
//       string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//       ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
//       if (currentUser != null)
//       {
//         Recipe[] recipes = _db.Recipes
//                               .Where(entry => entry.User.Id == currentUser.Id)
//                               .ToArray();
//         model.Add("recipes", recipes);
//       }
//       return View(model);
//     }