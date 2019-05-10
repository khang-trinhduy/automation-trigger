using System;
using System.Collections.Generic;

namespace Automation.API.Models
{
    public class Trigger
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNotActive { get; set; }
        public int Position { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<Action> Actions { get; set; }
        public List<Condition> Conditions { get; set; }
    }
}