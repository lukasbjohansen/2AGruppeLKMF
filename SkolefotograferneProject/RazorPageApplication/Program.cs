using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IRepository<School>, SchoolRepositoryAsync>();
builder.Services.AddTransient<IRepository<SchoolClass>, SchoolClassRepositoryAsync>();
builder.Services.AddTransient<IRepository<Teacher>, TeacherRepositoryAsync>();
builder.Services.AddTransient<IRepository<Secretary>, SecretaryRepositoryAsync>();
builder.Services.AddTransient<IRepository<Parent>, ParentRepositoryAsync>();
builder.Services.AddTransient<IRepository<Student>, StudentRepositoryAsync>();
builder.Services.AddTransient<IRepository<Photographer>, PhotographerRepositoryAsync>();
builder.Services.AddTransient<IRepository<Photo>, PhotoRepositoryAsync>();
builder.Services.AddTransient<IRepository<PhotoEvent>, PhotoEventRepositoryAsync>();
builder.Services.AddTransient<IRepository<Order>, OrderRepositoryAsync>();
builder.Services.AddTransient<IRepository<OrderLine>, OrderLineRepositoryAsync>();
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
