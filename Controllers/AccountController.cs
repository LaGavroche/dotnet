using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;


public class AccountController : Controller
{
    private readonly SignInManager<Etudiant> _signInManager;
    private readonly UserManager<Etudiant> _userManager;

    public AccountController(SignInManager<Etudiant> signInManager, UserManager<Etudiant> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(AccountViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new Etudiant
        {
            UserName = model.Email,
            Email = model.Email,
            Prenom = model.Firstname,
            Nom = model.Lastname,
            DateNaissance = model.DateNaissance,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(AccountViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction();
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
}