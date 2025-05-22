using Quartz;
using Quartz.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddQuartz(config =>
{
    config.UseMicrosoftDependencyInjectionJobFactory();
    var key = new JobKey("JobSendEmail");

    config.AddJob<JobSendEmail>(opt => opt.WithIdentity(key));

    config.AddTrigger(opt => opt.ForJob(key)
    .WithIdentity("Trigger Send Email")
    .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()));// envia cada 1 minuto y se repite siempre el JOB

    //config.AddTrigger(opts => opts
    //       .ForJob(key)
    //       .WithIdentity("Trigger Send Email")
    //       .WithCronSchedule("0 0 22 * * ?")); // segundos, minutos, horas, día del mes, mes, día de la semana

});



builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
