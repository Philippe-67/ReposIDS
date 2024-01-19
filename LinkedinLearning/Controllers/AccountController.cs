using LinkedinLearning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkedinLearning.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        // ajout constructeur
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        // méthode accès à la vue "Subscription"
        public IActionResult Subscription()
        {
            return View();
        }
        //// méthode accès à la vue "Login"
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //methode pour valider une "subscription"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscription(SubscriptionViewModel subscriptionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(subscriptionVM);
            }
            var result = await this.userManager.CreateAsync(new User
            {
                Email = subscriptionVM.Email,
                UserName = subscriptionVM.Email,
                FirstName = subscriptionVM.FirstName,
                LastName = subscriptionVM.LastName
            }, subscriptionVM.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(subscriptionVM);
            }
        }
        // méthode accès à la vue "Login"
        public IActionResult Login()
        {
            return View();
        }
        //methode pour valider une "Login"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel LoginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(LoginVM);
            }
            var result = await this.signInManager.PasswordSignInAsync(

                userName: LoginVM.Email,
                password: LoginVM.Password,
                isPersistent: true,// permet la persistance du cookie de login
                lockoutOnFailure: false // efface le cookie si mauvais login
                );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                ModelState.AddModelError("Identifiant incorrect", "Identifiant incorrect");
            }
            return View(LoginVM);
        }

    }
}



