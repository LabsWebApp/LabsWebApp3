namespace LabsWebApp3.Helpers
{
    public class Config
    {
        public static string ConnectionString { get; set; } = 
            "Data Source=(local)\\SQLEXPRESS;  Database=AppDb; Persist Security Info=false; User ID='sa'; Password='sa'; MultipleActiveResultSets=True; Trusted_Connection=False;";

        public static string Name { get; set; }
        public static string Email { get; set; }

        public static string WebRootPath { get; set; }
    }
}
