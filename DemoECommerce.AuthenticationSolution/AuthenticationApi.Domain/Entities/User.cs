

using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationApi.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
      
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? userName { get; set; }
        public string? phoneNumber { get; set; }

        [Column(TypeName = "text")] // Force le type Text en base de données
        public string image { get; set; }
        public Role role { get; set; }
       
    }
}
