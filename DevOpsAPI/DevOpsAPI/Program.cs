using DevOpsAPI.Chat;
using DevOpsAPI.DAL;
using DevOpsAPI.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonConfig.DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonConfig.DateTimeConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

StartupBuilder.ConfigureAuthorization(builder.Services, builder.Configuration);

builder.Services.AddDbContext<DataContext>();

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
});


var app = builder.Build();

// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
    opt.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials());

app.UseHttpsRedirection();
app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatsHub>("/Hubs/Chats");

app.Run();
