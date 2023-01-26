//using WaiterAvailabilityApp;

using WaiterAvailabilityApp;
using WaiterAvailabilityApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IWaiterAvailability, ApplicationDB>(x => 
    new ApplicationDB("Server=localhost;Port=5432;Database=waiter_db;UserId=postgres;Password=0000"));

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();