using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using RecipeBox.Models;
using Identity.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Controllers 
{
  public class RoleController : Controller
  {
    private RoleManager<IdentityRole> roleManager;
    private UserManager<ApplicationUser> userManager;
    public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
    {
      roleManager = roleMgr;
      userManager = userMgr;
    }

    public ViewResult Index() => View(roleManager.Roles);

    private void Errors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
        ModelState.AddModelError("", error.Description);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create([Required] string name)
    {
      if (ModelState.IsValid)
      {
        IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
        if (result.Succeeded)
          return RedirectToAction("Index");
        else  
          Errors(result);  
      }
      return View(name);
    }

    public async Task<IActionResult> Update(string id)
    {
      IdentityRole role = await roleManager.FindByIdAsync(id);
      List<ApplicationUser> members = new List<ApplicationUser>();
      List<ApplicationUser> nonMembers = new List<ApplicationUser>();
      List<ApplicationUser> users = await userManager.Users.ToListAsync();

      foreach(ApplicationUser user in users)
      //what about line 50? (Paul)
      // I dont even know how he came up with this (Jon)
      //put him in the river... if he floats he is a witch/warlock (Paul)
      // totally sorcery. Saw all the evidence I needed! (Jon)
      {
        if(await userManager.IsInRoleAsync(user, role.Name))
        {
          members.Add(user);
        }
        else
        {
          nonMembers.Add(user);
        }
        // var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
        // list.Add(user);
      }
      return View(new RoleEdit
      {
        Role = role,
        Members = members,
        NonMembers = nonMembers
      });
    }

    [HttpPost]
    public async Task<IActionResult> Update(RoleModification model)
    {
      IdentityResult result;
      if (ModelState.IsValid)
      {
        foreach (string userId in model.AddIds ?? new string[] { })
        {
          ApplicationUser user = await userManager.FindByIdAsync(userId);
          if (user != null)
          {
            result = await userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
            {
              Errors(result);
            }
          }
        }
        foreach (string userId in model.DeleteIds ?? new string[] { })
        {
          ApplicationUser user = await userManager.FindByIdAsync(userId);
          if(user != null)
          {
            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
            {
              Errors(result);
            }
          }
        }
      }
      if(ModelState.IsValid)
      {
        return RedirectToAction(nameof(Index));
      }
      else
      {
        return await Update(model.RoleId);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
      IdentityRole role = await roleManager.FindByIdAsync(id);
      if (role != null)
      {
        IdentityResult result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
          return RedirectToAction("Index");
        else
          Errors(result);
      }
      else
        ModelState.AddModelError("", "No role found");
        return View("Index", roleManager.Roles);
    }
  }
}