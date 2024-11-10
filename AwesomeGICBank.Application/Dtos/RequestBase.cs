using System.ComponentModel.DataAnnotations;

namespace AwesomeGICBank.Application.Dtos
{
    public class RequestBase
    {
        [Required]
        public virtual DateTime CreationDate { get; set; }
    }
}
