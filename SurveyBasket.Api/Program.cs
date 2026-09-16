

using Hangfire;
using HangfireBasicAuthenticationFilter;
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
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
    [       new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSetting:UserName"),

            Pass = app.Configuration.GetValue<string>("HangfireSetting:Password")


        }
    ],
    DashboardTitle = "Survey Basket Dashboard "

});

 var scopfactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopfactory.CreateScope();
var notifiactionservice = scope.ServiceProvider.GetRequiredService<IPollNotfication>();

RecurringJob.AddOrUpdate("SendPollNotifcation", ()=> notifiactionservice.SendPollNotifcation(null),Cron.Daily() );

app.UseCors();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();