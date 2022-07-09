using loginAndRoles.Models;
using loginAndRoles.ViewModels;
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

    // GET Login
    [HttpGet]
    public IActionResult
        Login(string? returnUrl = null) // Assigning a value to ReturnUrl (string) in case of it be null
    {
        var loginViewModel = new LoginViewModel();
        loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
        return View(loginViewModel);
    }

    // POST Login
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
    {
        // Validation
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password,
                loginViewModel.RememberMe, false);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Could not login into the account");
            return View(loginViewModel);
        }

        return View(loginViewModel);
    }

    // Registering a user
    // We create a returnUrl to allow the user get back to the previous page in a easy way
    public async Task<IActionResult> Register(string? returnUrl = null)
    {
        var registerViewModel = new RegisterViewModel();

        registerViewModel.ReturnUrl = returnUrl;

        return View(registerViewModel);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
    {
        registerViewModel.ReturnUrl = returnUrl;
        returnUrl = returnUrl ?? Url.Content("~/"); // Null

        // Basic validation
        if (ModelState.IsValid)
        {
            var user = new AppUser {Email = registerViewModel.Email, UserName = registerViewModel.UserName};
            var result =
                await _userManager.CreateAsync(user,
                    registerViewModel.Password); // User Manager will handle the Email + Pwd

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false); // Ain't be using persistent cookie
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError("Password", "User could not be created. Password is not unique");
        }

        return View(registerViewModel);
    }

    // POST Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}