var builder = WebApplication.CreateBuilder(args);

//omers

builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1",
        policy =>
        {
            policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });


});
//omers

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//omers
builder.Services.AddHttpClient();
//omers

var app = builder.Build();
//omers
app.UseCors("Policy1");
//omes


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
