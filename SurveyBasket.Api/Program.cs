

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Dependcies(builder.Configuration);
builder.Host.UseSerilog((Context, Configuration) =>
{
    Configuration.ReadFrom.Configuration(Context.Configuration);
   
});  

builder.Services.AddDistributedMemoryCache();   

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();


app.UseCors();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();