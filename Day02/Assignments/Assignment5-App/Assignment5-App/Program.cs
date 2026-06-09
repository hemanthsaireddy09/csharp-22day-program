using Microsoft.Data.SqlClient;
string connectionString =
    "Server=localhost;" +
    "Database=CareBridgeDB;" +
    "Trusted_Connection=True;" +
    "TrustServerCertificate=True;";
using SqlConnection conn =
    new SqlConnection(connectionString);
conn.Open();
while (true)
{
    Console.WriteLine("Welcome to CareBridge!");
    Console.WriteLine("-------------------------------------");
    Console.WriteLine("Available Operational Reports:");
    Console.WriteLine("1. 30-Day Readmissions");
    Console.WriteLine("2. High-Risk Patients");
    Console.WriteLine("3. Provider Workload");
    Console.WriteLine("4. Revenue Analysis");
    Console.WriteLine("5. Exit");  
    Console.WriteLine("Please select an option:");
    String option;
    option = Console.ReadLine();
    switch (option)
    {
        case "1":
            DisplayReadmissionsReport(conn);
            break;
        case "2":
            DisplayHighRiskPatientReport(conn);
            break;
        case "3":
            DisplayProviderWorkloadReport(conn);
            break;
        case "4":
            DisplayRevenueAnalysisReport(conn);
            break;
        case "5":
            Console.WriteLine("Exiting the application. Goodbye!");
            return;
        default :
            Console.WriteLine("Invalid option. Please try again.");
            break;

    }


}
static void DisplayReadmissionsReport(SqlConnection conn)
{
    string procedure = "_30_DAY_READMISSIONS";

    using SqlCommand cmd = new SqlCommand(procedure, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@WithinDays",30);
    using SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"Patient ID: {reader["PatientID"]}, Encounter ID: {reader["DaysSincePreviousVisit"]}");

    }
}

static void DisplayHighRiskPatientReport(SqlConnection conn)
{
    string procedure = "high_risk_patients";
    using SqlCommand cmd = new SqlCommand(procedure, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    using SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"Patient ID: {reader["PatientID"]}, MRN: {reader["MRN"]},FullName: {reader["FullName"]}, DateOfBirth: {reader["DateOfBirth"]}, Age: {reader["Age"]}, Gender: {reader["Gender"]}, City: {reader["City"]}");
    }
}

static void DisplayProviderWorkloadReport(SqlConnection conn)
{
    string procedure = "provider_workload";
    using SqlCommand cmd = new SqlCommand(procedure, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    using SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"Full Name: {reader["FullName"]}, EncounterCount: {reader["EncounterCount"]}, VolumeRank: {reader["VolumeRank"]}");
    }
}

static void DisplayRevenueAnalysisReport(SqlConnection conn)
{
    string procedure = "revenue_analysis";
    using SqlCommand cmd = new SqlCommand(procedure, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    using SqlDataReader reader = cmd.ExecuteReader();

    // Overall revenue summary
    Console.WriteLine("=== Overall Revenue Summary ===");
    if (reader.Read())
    {
        Console.WriteLine($"Total Billed: {reader["TotalBilled"]}, " +
                          $"Total Reimbursed: {reader["TotalReimbursed"]}, " +
                          $"Revenue Leakage: {reader["RevenueLeakage"]}, " +
                          $"Total Claims: {reader["TotalClaims"]}");
    }

    // Move to Department result set
    if (reader.NextResult())
    {
        Console.WriteLine("\n=== Revenue by Department ===");
        while (reader.Read())
        {
            Console.WriteLine($"Department: {reader["Name"]}, " +
                              $"Billed: {reader["DeptBilled"]}, " +
                              $"Reimbursed: {reader["DeptReimbursed"]}, " +
                              $"Leakage: {reader["DeptLeakage"]}, " +
                              $"Claims: {reader["ClaimCount"]}");
        }
    }

    // Move to Insurance Payer result set
    if (reader.NextResult())
    {
        Console.WriteLine("\n=== Revenue by Insurance Payer ===");
        while (reader.Read())
        {
            Console.WriteLine($"Payer: {reader["Payer"]}, " +
                              $"Billed: {reader["PayerBilled"]}, " +
                              $"Reimbursed: {reader["PayerReimbursed"]}, " +
                              $"Leakage: {reader["PayerLeakage"]}, " +
                              $"Claims: {reader["ClaimCount"]}");
        }
    }
}
