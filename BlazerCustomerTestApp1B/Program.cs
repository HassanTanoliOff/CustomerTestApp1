using BlazerCustomerTestApp1B.Components;
using BlazerCustomerTestApp1B.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpClient(
  "CustomerTestApp1",
  client =>
  {
    client.BaseAddress = new Uri("http://localhost:5174/");
  }
);
builder.Services.AddScoped<CustomerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
