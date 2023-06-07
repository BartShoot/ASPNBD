using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ASPNBD.Models;
using ASPNBD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ComputerStoreDatabaseSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddSingleton<GridFSBucket>(provider =>
{
    var mongoClient = new MongoClient(builder.Configuration.GetSection("MongoDb:ConnectionString").Value);
    var mongoDatabase = mongoClient.GetDatabase(builder.Configuration.GetSection("MongoDb:DatabaseName").Value);
    return new GridFSBucket(mongoDatabase);
});

builder.Services.AddSingleton<IDataService, DataService>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
