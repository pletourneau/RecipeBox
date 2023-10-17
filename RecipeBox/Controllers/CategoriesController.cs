using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeBox.Controllers
{
  public class CategoriesController : Controller
  {
    private readonly RecipeBoxContext _db;

    public CategoriesController(RecipeBoxContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      ViewBag.PageTitle = "List of Categories";
      return View(_db.Categories.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.PageTitle = "Add a new category guey";
      return View();
    }

    [HttpPost]
    public ActionResult Create(Category category)
    {
      _db.Categories.Add(category);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      ViewBag.PageTitle = "Category Details";
      Category targetCategory = _db.Categories.FirstOrDefault(entry => entry.CategoryId == id);
      return View(targetCategory);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.PageTitle = "Edit Category";
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
      ViewBag.PageTitle = "Delete Category";
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

  }
}