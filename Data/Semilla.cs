using ApiSalvarVidas.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiSalvarVidas.Data
{
    public class Semilla
    {
        public static async Task Inicializar(IServiceProvider services)
        {
            var userManager  = services.GetRequiredService<UserManager<AspNetUser>>();
            var roleManager  = services.GetRequiredService<RoleManager<AspNetRole>>();

            // =========================
            // ROLES
            // =========================
            foreach (var rol in new[] { "Admin", "Voluntario" })
            {
                if (!await roleManager.RoleExistsAsync(rol))
                    await roleManager.CreateAsync(new AspNetRole { Name = rol });
            }

            // =========================
            // ADMINISTRADOR
            // =========================
            await CrearUsuario(userManager,
                email:    "admin@salvavidas.com",
                password: "Admin123*",
                rol:      "Admin");

            // =========================
            // VOLUNTARIO
            // =========================
            await CrearUsuario(userManager,
                email:    "voluntario@salvavidas.com",
                password: "Voluntario123*",
                rol:      "Voluntario");
        }

        private static async Task CrearUsuario(
            UserManager<AspNetUser> userManager,
            string email, string password, string rol)
        {
            if (await userManager.FindByEmailAsync(email) is not null) return;

            var user = new AspNetUser
            {
                UserName       = email,
                Email          = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, rol);
        }
    }
}
