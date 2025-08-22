using Application;
using AspNet.Security.OAuth.Discord;
using BlazorApp2.Authrization;
using BlazorApp2.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorApp2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("libAppsettings.json");

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddHttpClient();

            builder.Services.Configure<Config>(builder.Configuration.GetSection("MapSettings"));
            builder.Services.Configure<DiscordSettings>(builder.Configuration.GetSection("DiscordSettings"));

            builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("DefaultConnection"));

            var discordSettings = builder.Configuration.GetSection("DiscordSettings").Get<DiscordSettings>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.Redirect("/not-in-guild");
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToLogin = context =>
                {
                    // Можно оставить стандартный редирект на логин
                    return Task.CompletedTask;
                };
            })
            .AddDiscord(options =>
            {
                options.ClientId = discordSettings.ClientId.ToString();
                options.ClientSecret = discordSettings.ClientSecret;
                options.Scope.Add("identify");
                options.Scope.Add("guilds");
                options.CallbackPath = new PathString("/auth/discord/callback");
                // Убрана проверка членства из OnCreatingTicket
            });

            // Регистрируем кастомный handler и политику
            builder.Services.AddSingleton<IAuthorizationHandler, GuildMemberHandler>();
            builder.Services.AddSingleton(sp =>
                sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<DiscordSettings>>().Value);
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireGuildMember", policy =>
                    policy.Requirements.Add(new GuildMemberRequirement()));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
