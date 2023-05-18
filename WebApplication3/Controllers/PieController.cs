using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Security.Claims;
using WebApplication3.Authentication;
using WebApplication3.helpers;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyDbContext dbContext;
        public PieController(IPieRepository pie,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,MyDbContext context)
        {
            _pieRepository = pie;
            _userManager = userManager;
            _roleManager = roleManager;
            dbContext = context;
        }
       
        [HttpGet]
        [Route("getAllPieList")]
        //[Authorize(Policy = "Permissions.Dashboard_ViewMap")]
        public async Task<IActionResult> getAllPieList()
        {
            string id = HttpContext.User.FindFirstValue("UserId");
            return Ok(new Response { Status = "Success", Message = "Pie List Record", data = id });

            bool isPermitted = await ClaimsHelper.GetPermissionsByRoleAsync(_roleManager,_userManager,"Permissions.Dashboard_ViewMap", HttpContext.User.FindFirstValue("CurrentUser"));
            if (isPermitted){
               
                PieListViewModel piesListViewModel = new PieListViewModel(_pieRepository.AllPies, "Cheese cakes");
                return Ok(new Response { Status = "Success", Message = "Pie List Record", data = piesListViewModel.Pies.ToList() });
            }
            else {
                return Ok(new Response { Status = "Failed", Message = "User doesn't have the permission", data = { } });

            }
        }

        [HttpGet]
        public IActionResult List()
        {
            PieListViewModel piesListViewModel = new PieListViewModel(_pieRepository.AllPies, "Cheese cakes");
            return View(piesListViewModel);
        }
    }
}
