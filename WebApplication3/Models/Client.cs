using Microsoft.AspNetCore.Identity;

namespace WebApplication3.Models
{
    public class Client
    {
        public string discriminator { get; set; }
        public string client_id { get; set; }
        public string client_name { get; set; }
        public string client_email { get; set; }
        public string client_address { get; set; }
        public string db_host { get; set; }
        public string db_port { get; set; }
        public string db_name { get; set; }
        public string db_username { get; set; }
        public string db_password { get; set; }

    }
}
