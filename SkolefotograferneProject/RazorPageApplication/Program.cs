using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IRepositoryAsync<School>, SchoolRepositoryAsync>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<SchoolClass>, SchoolClassRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<Secretary>, SecretaryRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<Parent>, ParentRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<Student>, StudentRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<Photographer>, PhotographerRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<Photo>, PhotoRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<PhotoEvent>, PhotoEventRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<Order>, OrderRepositoryAsync>();
builder.Services.AddTransient<IRepositoryAsync<OrderLine>, OrderLineRepositoryAsync>();
builder.Services.AddTransient<LoginUserRepository>();

// Login
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Login
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
