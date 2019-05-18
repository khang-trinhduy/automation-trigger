using System.Collections.Generic;

namespace Crm
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public List<Contact> Subscribers { get; set; }
    }
}