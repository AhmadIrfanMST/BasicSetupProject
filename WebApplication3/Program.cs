using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using WebApplication3.Authentication;
using WebApplication3.Models;
using WebApplication3.Permission;
using WebApplication3.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddControllers();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();

builder.Services.AddSwaggerGen();

// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM"))
    };
});

//Adding Authorization 
builder.Services.AddAuthorization(options =>
{
    //below fallback is equivalentr to applying authorize on controller side or on an action
    //options.FallbackPolicy = new AuthorizationPolicyBuilder()// fallback is triggered when no authorize attribute is present. Authorize is not on controller nor on action
    //.RequireAuthenticatedUser()// it will check if there is an identity cookie present
    //.Build();
    options.AddPolicy("AdminAssignRolePolicy", policy =>
        policy.RequireClaim(Permissions.AssignRole).RequireRole(UserRoles.Admin));
});
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
//here

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:MyDBContextConnection"]);
});
var app = builder.Build();
app.UseStaticFiles();
// if user is not super admin then send client id and get connection response base on it
using (HttpClient client = new HttpClient())
{
    try
    {
        // Make the API request and get the response
        HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/todos/");

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            // Use the response data in your program
            Console.WriteLine(responseBody);
        }
        else
        {
            Console.WriteLine("API request failed with status code: " + response.StatusCode);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }
}



//For Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("app");
    try
    {
        var dbContext = services.GetRequiredService<MyDbContext>();

        // Seed the permissions
        await PermissionSeeder.SeedPermissionsAsync(dbContext); // Add permissions in permissions table 


        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultUsers.SeedBasicUserAsync(userManager, roleManager);
        await DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);

        await DefaultRolePermissions.SeedRolePermissionsAsync(roleManager,UserRoles.SuperAdmin, dbContext);// Assign permissions to Role Super Admin
        await DefaultRolePermissions.SeedRolePermissionsAsync(roleManager,UserRoles.Admin, dbContext);// Assign permissions to Role Admin
        await DefaultRolePermissions.SeedRolePermissionsAsync(roleManager,UserRoles.User, dbContext);// Assign permissions to Role User

        logger.LogInformation("Finished Seeding Default Data");
        logger.LogInformation("Application Starting");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "An error occurred seeding the DB");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseRouting();
// Handling request response middleware
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(new ErrorHandle()
        {
            StatusCode = 401,
            Message = "Token is not valid"
        }.ToString());
    }
    else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden) // 403
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(new ErrorHandle()
        {
            StatusCode = 403,
            Message = "You do not have any access for this resource, Please contact administrator!"
        }.ToString());
    }
});
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

