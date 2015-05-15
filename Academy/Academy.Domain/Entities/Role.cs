using System.ComponentModel;

namespace Academy.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [DisplayName("Role")]
        public string Name { get; set; }
    }
}
