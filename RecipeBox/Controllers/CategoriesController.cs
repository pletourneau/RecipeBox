using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace RecipeBox.Controllers
{
  [Authorize]
  public class CategoriesController : Controller
  {
    private readonly RecipeBoxContext _db;

    public CategoriesController(RecipeBoxContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      ViewBag.Title = "List of Categories";
      return View(_db.Categories.ToList());
    }
  
    public ActionResult Create()
    {
      ViewBag.Title = "Add a new category guey";
      return View();
    }

    [HttpPost]
    public ActionResult Create(Category category)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.Title = "Add a new category guey";
        return View(category);
      }
      else
      {
        _db.Categories.Add(category);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      ViewBag.Title = "Category Details";
      Category targetCategory = _db.Categories.Include(entry => entry.JoinEntities)
                                              .ThenInclude(join => join.Recipe)
                                              .FirstOrDefault(entry => entry.CategoryId == id);
      return View(targetCategory);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Edit Category";
      Category targetCategory = _db.Categories.FirstOrDefault(entry => entry.CategoryId == id); 
      return View(targetCategory);     
    }

    [HttpPost]
    public ActionResult Edit(Category categoryToEdit)
    {
      _db.Categories.Update(categoryToEdit);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = categoryToEdit.CategoryId });
    }

    public ActionResult Delete(int id)
    {
      ViewBag.Title = "Delete Category";
      Category targetCategory = _db.Categories.FirstOrDefault(entry => entry.CategoryId == id);
      return View(targetCategory);            
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Category targetCategory = _db.Categories.FirstOrDefault(entry => entry.CategoryId == id);
      _db.Categories.Remove(targetCategory);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    public ActionResult AddRecipe(int id)
    {
      ViewBag.Title = "Add Recipe to this Category";
      Category thisCategory = _db.Categories.FirstOrDefault(model => model.CategoryId == id);
      ViewBag.RecipeId = new SelectList(_db.Recipes, "RecipeId", "Title");
      return View(thisCategory);
    }

    [HttpPost]
    public ActionResult AddRecipe(Category category, int recipeId)
    {
      #nullable enable
      CategoryRecipe? joinEntity = _db.CategoryRecipes.FirstOrDefault(join =>(join.RecipeId == recipeId && join.CategoryId == category.CategoryId));
      #nullable disable
      if (joinEntity == null && recipeId != 0)
      {
        _db.CategoryRecipes.Add(new CategoryRecipe() {RecipeId = recipeId, CategoryId = category.CategoryId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new {id = category.CategoryId});
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      CategoryRecipe joinEntry = _db.CategoryRecipes.FirstOrDefault(entry => entry.CategoryRecipeId == joinId);
      _db.CategoryRecipes.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.CategoryId });
    }
  }
}