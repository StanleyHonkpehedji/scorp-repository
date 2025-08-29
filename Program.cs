var builder = WebApplication.CreateBuilder(args);

// Ajoute le support MVC
builder.Services.AddControllersWithViews();

// Ajoute IHttpClientFactory
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure le pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Landing}/{action=Index}/{id?}");

app.Run();
