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
    Console.WriteLine("Compliant Access Portal");
    Console.WriteLine("1. Clinical View");
    Console.WriteLine("2. Billing View");
    Console.WriteLine("3. Analytics View ");
    Console.WriteLine("4. Exit");
    Console.Write("Select an option: ");
    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            Console.WriteLine("Accessing Clinical View...");
            DisplayClinicalView(conn);
            break;

        case "2":
            Console.WriteLine("Accessing Billing View...");
            DisplayBillingView(conn);
            break;

        case "3":
            Console.WriteLine("Accessing Analytics View...");
            DisplayAnalyticsView(conn);
            break;

        case "4":
            Console.WriteLine("Exiting...");
            return;

        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}
    

    static void DisplayClinicalView(SqlConnection conn)
{
    //used TOP 20 beacause of bulk data
    string query = "SELECT TOP 20 * FROM vw_Clinical";
    using SqlCommand cmd = new SqlCommand(query, conn);
    using SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"EncounterId: {reader["EncounterId"]}, " +
                          $"PatientId: {reader["PatientId"]}, " +
                          $"Admit Date: {reader["AdmitDate"]}, " +
                          $"Type: {reader["EncounterType"]}, " +
                          $"ICDCode: {reader["IcdCode"]}, " +
                          $"Diagnosis: {reader["Description"]}");
    }
    reader.Close();
}

static void DisplayBillingView(SqlConnection conn)
{
    //used TOP 20 beacause of bulk data
    string query = "SELECT TOP 20 * FROM vw_Billing";
    using SqlCommand cmd = new SqlCommand(query, conn);
    using SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"ClaimId: {reader["ClaimId"]}, " +
                          $"EncounterId: {reader["EncounterId"]}, " +
                          $"Billed: {reader["BilledAmount"]}, " +
                          $"Reimbursed: {reader["ReimbursedAmount"]}, " +
                          $"Leakage: {reader["RevenueLeakage"]}, " +
                          $"Payer: {reader["Payer"]}");
    }
    reader.Close();
}

static void DisplayAnalyticsView(SqlConnection conn)
{
    string query = "SELECT * FROM vw_Analytics ORDER BY AgeGroup";
    using SqlCommand cmd = new SqlCommand(query, conn);
    using SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"AgeGroup: {reader["AgeGroup"]}, " +
                          $"EncounterType: {reader["EncounterType"]}, " +
                          $"Count: {reader["EncounterCount"]}");
    }
    reader.Close();
}

