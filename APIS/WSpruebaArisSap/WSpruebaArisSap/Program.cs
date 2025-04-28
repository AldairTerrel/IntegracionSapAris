var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options=>
 {
     options.AllowEmptyInputInBodyModelBinding = true;
     options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

 });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseDeveloperExceptionPage();


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/sap-api/swagger/v1/swagger.json", "WSpruebaArisSap"));

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();


