using Bogus;
using Newtonsoft.Json;

namespace PatientConsole;

// Model representing a patient in our healthcare system
public class Patient
{
    public int PatientId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Age { get; set; }
    public string City { get; set; } = "";
    public bool Active { get; set; }

}

internal class Program
{
    static void Main()
    {
        // Create a fake-data generator for the Patient class
        // Faker<T> comes from the Bogus NuGet package
        var patientGenerator = new Faker<Patient>()

            // Generate a random PatientId between 1000 and 9999
            .RuleFor(p => p.PatientId,
                f => f.Random.Number(1000, 9999))

            // Generate a realistic first name
            .RuleFor(p => p.FirstName,
                f => f.Name.FirstName())
            // Generate a realistic lastr name
            .RuleFor(p => p.LastName,
                f => f.Name.LastName())

            // Generate a random age between 18 and 90
            .RuleFor(p => p.Age,
                f => f.Random.Number(18, 90))

            // Generate a realistic city name
            .RuleFor(p => p.City,
                f => f.Address.City())

            // Randomly assign Active = true or false
            .RuleFor(p => p.Active,
                f => f.Random.Bool());
        // Generate 1000 fake patient records
        // Generate() is a method provided by Bogus
        var patients = patientGenerator.Generate(1000);

        Console.WriteLine($"Generated {patients.Count} fake patients");
        Console.WriteLine();
        string json = JsonConvert.SerializeObject(patients.Take(10), Formatting.Indented);
        Console.WriteLine($"Top 10 records in json:{json}");
        // Display only the first 10 records
        //Imagine these 1000 records being used for testing
        foreach (var patient in patients.Take(10))
            {
                Console.WriteLine(
                    $"ID: {patient.PatientId} | " +
                    $"First Name: {patient.FirstName} | " +
                    $"Last Name{patient.LastName} |" +
                    $"Age: {patient.Age} | " +
                    $"City: {patient.City} | " +
                    $"Active: {patient.Active}");
            }

        Console.WriteLine();
        Console.WriteLine("Remaining 990 records were generated but not displayed.");
    }
}
