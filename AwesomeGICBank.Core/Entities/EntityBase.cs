using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeGICBank.Core.Entities
{
    public class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
