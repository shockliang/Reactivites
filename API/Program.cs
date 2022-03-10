using Application.Activities;

var builder = WebApplication.CreateBuilder(args);

// add services to container
builder.Services
    .AddControllers(opt =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        opt.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddFluentValidation(config => { config.RegisterValidatorsFromAssemblyContaining<Create>(); });
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// Configure the http request pipeline
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

app.UseXContentTypeOptions();
app.UseReferrerPolicy(opt => opt.NoReferrer());
app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
app.UseXfo(opt => opt.Deny());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
    app.UseCspReportOnly(opt => opt
        .BlockAllMixedContent()
        .StyleSources(s => s.Self()
            .CustomSources(
                "https://fonts.googleapis.com",
                "sha256-yChqzBduCCi4o4xdbXRXh4U/t1rP4UUUMJt+rB+ylUI="))
        .FontSources(s => s.Self()
            .CustomSources("https://fonts.gstatic.com/", "data:"))
        .FormActions(s => s.Self())
        .FrameAncestors(s => s.Self())
        .ImageSources(s => s.Self()
            .CustomSources(
                "https://platform-lookaside.fbsbx.com/",
                "https://www.facebook.com",
                "https://res.cloudinary.com",
                "data:"))
        .ScriptSources(s => s.Self()
            .CustomSources(
                "sha256-yChqzBduCCi4o4xdbXRXh4U/t1rP4UUUMJt+rB+ylUI=",
                "sha256-GR9OwjbT/NvvB6BtJTihFr8QTDsAMhjYE+BJwlB2n70=",
                "sha256-FxWwTAdSjATwPlsAFcWm2r75EsXr0nltQlm6b6QNLt4=",
                "https://connect.facebook.net"))
    );
}
else
{
    app.UseCsp(opt => opt
        .BlockAllMixedContent()
        .StyleSources(s => s.Self()
            .CustomSources(
                "https://fonts.googleapis.com",
                "sha256-yChqzBduCCi4o4xdbXRXh4U/t1rP4UUUMJt+rB+ylUI="))
        .FontSources(s => s.Self()
            .CustomSources("https://fonts.gstatic.com/", "data:"))
        .FormActions(s => s.Self())
        .FrameAncestors(s => s.Self())
        .ImageSources(s => s.Self()
            .CustomSources(
                "https://platform-lookaside.fbsbx.com/",
                "https://www.facebook.com",
                "https://res.cloudinary.com",
                "data:"))
        .ScriptSources(s => s.Self()
            .CustomSources(
                "sha256-yChqzBduCCi4o4xdbXRXh4U/t1rP4UUUMJt+rB+ylUI=",
                "sha256-GR9OwjbT/NvvB6BtJTihFr8QTDsAMhjYE+BJwlB2n70=",
                "sha256-FxWwTAdSjATwPlsAFcWm2r75EsXr0nltQlm6b6QNLt4=",
                "https://connect.facebook.net"))
    );
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
        await next.Invoke();
    });
}

// app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat");
app.MapFallbackToController("Index", "Fallback");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occured during migration");
}

await app.RunAsync();