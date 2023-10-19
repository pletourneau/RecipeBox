using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using RecipeBox.Models;

namespace Identity.Models
{
  public class RoleEdit
  {
    public IdentityRole Role { get; set; }
    // public IEnumerable<ApplicationUser> Members { get; set; }
    // public IEnumerable<ApplicationUser> NonMembers { get; set; }
    public List<ApplicationUser> Members { get; set; }
    public List<ApplicationUser> NonMembers { get; set; }
  }
}