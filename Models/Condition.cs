namespace Automation.API.Models
{
    public class Condition
    {
        public int Id { get; set; }
        public string Operator { get; set; }
        public MetaData MetaData { get; set; }
    }
}