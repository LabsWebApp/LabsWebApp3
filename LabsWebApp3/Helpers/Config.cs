namespace LabsWebApp3.Helpers
{
    public class Config
    {
        //НУЖНО ПРОДУМАТЬ БЕЗОПАСНОСТЬ!!!!!
        public const string EmailPass = "WebAppLabs";

        public static readonly string
            Chat = "Chat",
            Admin = "admin",
            RoleReader = "chatreader",
            RoleWriter = "chatwriter",
            RoleModerator = "chatmoderator",
            RoleAdmin = Admin;

        public static string ConnectionString { get; set; }

        public static string Name { get; set; }
        public static string Email { get; set; }

        public static string WebRootPath { get; set; }
    }
}
