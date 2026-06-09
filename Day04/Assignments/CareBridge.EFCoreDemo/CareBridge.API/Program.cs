using CareBridge.EFCoreDemo.Models.Generated;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register EF Core DbContext.
// ASP.NET Core will automatically create and inject it when needed.
builder.Services.AddDbContext<CareBridgeScaffoldContext>();

// Add Swagger support.
// Swagger gives us a testing screen for APIs.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow Vue.js running on another port
// to call this API from the browser.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Enable Swagger.
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS.
app.UseCors();

// Simple health-check endpoint.
app.MapGet("/", () =>
{
    return "CareBridge API is running";
});

// EF Core converts this LINQ query into SQL.
app.MapGet("/api/patients",
    (CareBridgeScaffoldContext db, string? city, bool? isActive, string? search) =>
    {
        var query = db.Patients.AsQueryable();
        if(!string.IsNullOrEmpty(city))
        {
            query = query.Where(p => p.City == city);
        }
        if(isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }
        if(!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.FullName.Contains(search));
        }
        return query
                 .Select(p => new
                 {
                     p.PatientId,
                     p.FullName,
                     p.City,
                     p.IsActive
                 })
                 
                .ToList();
    });
app.MapGet("/api/cities",
    (CareBridgeScaffoldContext db) =>
    {
        return db.Patients
                 .Select(p => p.City)
                 .Distinct()
                 .ToList();
    });
app.MapGet("/api/analytics/department-load",
    async (CareBridgeScaffoldContext db) =>
    {
        var data = await db.Encounters
            .Include(e => e.Department)
            .GroupBy(e => e.Department.Name)
            .Select(g => new
            {
                DepartmentName = g.Key,
                Inpatient = g.Count(e => e.EncounterType == "inpatient"),
                Outpatient = g.Count(e => e.EncounterType == "outpatient"),
                ED = g.Count(e => e.EncounterType == "ed"),
                Total = g.Count()
            })
            .OrderByDescending(x => x.Total)
            .ToListAsync();

        return Results.Ok(data);
    });
app.Run();
app.Run();
