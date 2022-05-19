using Microsoft.AspNetCore.ResponseCompression;
using Windows.Gaming.Input;
using Capgemini.Slotmachine.BackgroundServices;
using Capgemini.Slotmachine.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://localhost:7001", "http://localhost:7000");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<StateHttpClient>(
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:6000");
    });
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddSingleton((RawGameController controller) => controller.ButtonCount > 0);
builder.Services.AddHostedService<GameControllerBackgroundService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<GameControllerHub>("/game-controller-hub");
app.MapFallbackToPage("/_Host");

app.Run();