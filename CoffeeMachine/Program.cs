using CoffeeMachine.Services.DataAccess;
using CoffeeMachine.Services.CoffeeMachine;
using CoffeeMachine.Application;
using CoffeeMachine.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICoffeeMachine, CoffeeMachineStub>();
builder.Services.AddScoped<IDataAccess, DataAccess>();
builder.Services.AddScoped<ICoffeeMachineApplication, CoffeeMachineApplication>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CoffeeMachine}/{action=HomeScreen}");

app.MapGet("/State", (ICoffeeMachineApplication coffeeMachineApplication) =>
{
    var ret = coffeeMachineApplication.StateCurrentGet();

    return ret;
});

app.MapGet("/ElementStates", (ICoffeeMachineApplication coffeeMachineApplication) =>
{
    var ret = coffeeMachineApplication.CoffeeMachineElementsGet();

    return ret;
});

app.MapGet("/TurnOff", async (ICoffeeMachineApplication coffeeMachineApplication) =>
{
    var vm = await coffeeMachineApplication.TurnOff();
    return vm.LastActionMessageGet();
});

app.MapGet("/TurnOn", async (ICoffeeMachineApplication coffeeMachineApplication) =>
{
    var vm = await coffeeMachineApplication.TurnOn();
    return vm.LastActionMessageGet();
});

app.MapGet("/MakeCoffee", async (int numEspressoShots, bool addMilk, ICoffeeMachineApplication coffeeMachineApplication) =>
{
    var vm = await coffeeMachineApplication.MakeCoffee(numEspressoShots, addMilk);
    return vm.LastActionMessageGet();
});

app.Run();
