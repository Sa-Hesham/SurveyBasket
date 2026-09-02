

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Dependcies(builder.Configuration);
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<AppDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePages();   
app.UseHttpsRedirection();
app.MapIdentityApi<ApplicationUser>();
app.UseAuthorization();

app.MapControllers();

app.Run();
