var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddHttpClient("category", c =>
{
	c.BaseAddress = new Uri("http://localhost:7082/g/");
});
builder.Services.AddHttpClient("course", c =>
{
	c.BaseAddress = new Uri("http://localhost:7082/c/");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();	
}

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
