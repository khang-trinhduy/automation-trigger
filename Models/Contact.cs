using System.ComponentModel.DataAnnotations.Schema;

namespace Crm
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public Campaign Campaign { get; set; }
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
    }
}