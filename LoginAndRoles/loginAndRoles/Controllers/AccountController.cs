using loginAndRoles.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace loginAndRoles.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // Registering a user
    // We create a returnUrl to allow the user get back to the previous page in a easy way
    public async Task<IActionResult> Register(string returnUrl = null)
    {
        var registerViewModel = new RegisterViewModel();

        registerViewModel.ReturnUrl = returnUrl;

        return View(registerViewModel);
    }
}