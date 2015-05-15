using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Academy.Domain.Entities
{
    public class Group
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Group")]
        [MaxLength(50, ErrorMessage = "Maximum length Exceeded")]
        public string Name { get; set; }
    }
}
