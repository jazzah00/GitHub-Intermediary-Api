using GitHub_Intermediary_Api.Interfaces;
using GitHub_Intermediary_Api.Services;
using GitHub_Intermediary_Api.Services.Authentication;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.AddScoped<IApiConnector, ApiConnector>();
builder.Services.AddScoped<IValidator, Validator>();
builder.Services.AddScoped<IConverter, Converter>();

builder.Services.AddAuthentication("Bearer").AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>("Bearer", options => { });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(ui => {
    ui.SwaggerEndpoint("/swagger/v1/swagger.json", "GitHub API v1");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/GitHub"), appBuilder => {
    appBuilder.Use(async (context, next) => {
        var authService = context.RequestServices.GetRequiredService<IAuthService>();

        if (!context.Request.Headers.TryGetValue("Authorization", out var tokenHeader) || tokenHeader.Count == 0 || string.IsNullOrWhiteSpace(tokenHeader[0])) {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization token is missing.");
            return;
        }

        string tokenValue = tokenHeader[0];
        if (!tokenValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization token is not in the correct format");
            return;
        }

        tokenValue = tokenValue["Bearer ".Length..].Trim();
        if (!authService.ValidateToken(tokenValue)) {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or expired token.");
            return;
        }
        await next();
    });
});

app.Run();
