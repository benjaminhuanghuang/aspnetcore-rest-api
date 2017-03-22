Add dependencies
-----------------
 ```xml
    <PackageReference Include="Swashbuckle" Version="6.0.0" />
```
Coding
-----------------
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMvc();

    services.AddSwaggerGen();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    ...
    app.UseMvc();

    app.UseSwaggerGen();
    app.UseSwaggerUi();
}

Ues
---------------
http://localhost:5000/swagger/ui/index.html