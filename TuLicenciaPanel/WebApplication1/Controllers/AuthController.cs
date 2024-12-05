using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        public readonly string _baseApiEndPoint;
        private readonly HttpClient _httpClient;

        public AuthController(IConfiguration configuration)
        {
            _baseApiEndPoint = configuration.GetValue<string>("ApiUrlEndPoint");
            _httpClient = new HttpClient();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string? returnUrl)
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated && !string.IsNullOrWhiteSpace(returnUrl)) {
                if(string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction(controllerName: "Home", actionName: "Index");
                } else
                {
                    return Redirect(returnUrl);
                }
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Check(CredentialViewModel credentials)
        {
            try
            {
                //var response = await _httpClient.GetAsync($"{_baseApiEndPoint}/Administrador/loginAdm/{credentials.Usr}/{credentials.Pwd}");
                var response = await _httpClient.GetAsync($"https://api.tulicenciapr.com/api/Administrador/loginAdm/{credentials.Usr}/{credentials.Pwd}");

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var jsonObj = JsonConvert.DeserializeObject<ResponseUser>(body);

                    if (jsonObj != null && jsonObj.success)
                    {
                        string rolName = jsonObj.item.rol;

                        if (string.IsNullOrWhiteSpace(rolName))
                        {
                            if (jsonObj.item.adm_nivel == 1)
                                rolName = "Administrador";

                            if (jsonObj.item.adm_nivel == 2)
                                rolName = "Operador";

                            if (jsonObj.item.adm_nivel == 3)
                                rolName = "Radicador";
                            if (jsonObj.item.adm_nivel == 4)
                                rolName = "Dcotor";
                        }

                        var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, jsonObj.item.adm_user),
                            new Claim(ClaimTypes.Role, rolName),
                            new Claim(ClaimTypes.Actor, jsonObj.item.adm_nombres),
                            new Claim("Id", jsonObj.item.adm_id.ToString()),
                            new Claim("Email", jsonObj.item.adm_email),
                            new Claim("FullName", jsonObj.item.adm_nombres),
                            new Claim("Nivel", jsonObj.item.adm_nivel.ToString()),
                            new Claim("Token", jsonObj.item.token),
                            new Claim("Avatar", "https://ui-avatars.com/api/?name="+jsonObj.item.adm_nombres.Replace(" ", "+")),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                        {
                            IsPersistent = true,
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12),
                            RedirectUri = credentials.ReturnUrl
                        });

                        if (rolName == "Doctor")
                        {
                            return RedirectToAction(actionName: "Doctor", controllerName: "Home", new { doctorId = jsonObj.item.adm_id });
                        }

                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                    }
                }

                ModelState.AddModelError("usr", "Las credenciales ingresadas no se encuentran en nuestros registros.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("usr", ex.Message);
            }
            return View("~/Views/Auth/Index.cshtml");
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(actionName: "Index", controllerName: "Auth");
        }

        [Authorize(Policy = "ElevatedRights")]
        public IActionResult AccessDenied(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Authorize(Policy = "ElevatedRights")]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
