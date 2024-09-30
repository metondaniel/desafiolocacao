using FluentValidation;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Locacao.Service.Interfaces.Services;
using Locacao.Service.Validators;
using Locacao.Service;
using Locacao.Repository;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<IEntregadorRepository, EntregadorRepository>();
builder.Services.AddScoped<ILocacaoRepository, LocacaoRepository>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IEntregadorService, EntregadorService>();
builder.Services.AddScoped<ILocacaoService, LocacaoService>();
builder.Services.AddScoped<IImageUploadService>(sp =>
{
    var s3Client = sp.GetRequiredService<IAmazonS3>();
    var bucketName = builder.Configuration["AWS:BucketName"];
    return new ImageService(s3Client, bucketName);
});
builder.Services.AddScoped<IValidator<Moto>, MotoValidator>();
builder.Services.AddScoped<IValidator<Entregador>, EntregadorValidator>();
builder.Services.AddScoped<IValidator<LocacaoMoto>, LocacaoValidator>();

builder.Services.AddScoped<IMessageQueueService, MessageQueueService>();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

app.UseRouting();
app.Urls.Add("http://0.0.0.0:5000");
app.UseAuthorization();
app.MapControllers();

// Executa a aplicação
app.Run();
