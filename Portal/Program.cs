using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Portal.Settings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<NCDCContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NiabatDB"), o => o.CommandTimeout(180));
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.AccessDeniedPath = new PathString("/");
    options.LoginPath = new PathString("/");
    options.Cookie.Name = "NCDC";
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portal", Version = "v1" });
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(o => { });

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new AuthorizeFilter());
    config.Filters.Add(typeof(ModelValidationFilter));
})
    .AddFluentValidation(c =>
    {
        c.ValidatorOptions.CascadeMode = CascadeMode.Stop;
        c.RegisterValidatorsFromAssemblyContaining<ModelValidationFilter>();
    });

builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

builder.Services.AddSingleton<IValidatorInterceptor, ValidatorInterceptor>();
builder.Services.AddSingleton<FileService>();

builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("Security"));
builder.Services.Configure<GeneralSettings>(builder.Configuration.GetSection("GeneralSettings"));
builder.Services.Configure<Paths>(builder.Configuration.GetSection("Paths"));

builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.

var logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(builder.Configuration.GetSection("Logging"))
                 .Enrich.FromLogContext();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portal v1"));
}

Serilog.Log.Logger = logger.CreateLogger();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();