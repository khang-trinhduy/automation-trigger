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
        public string Table { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<Action> Actions { get; set; }
        public List<Condition> Conditions { get; set; }

        public string GetQuery()
        {
            if (Actions == null || Conditions == null)
            {
                throw new NullReferenceException();
            }
            string query = "";
            List<string> ac = new List<string>();
            foreach (var action in Actions)
            {
                List<string> actionQueries = action.GetActionQuery();
                if (actionQueries.Count == 2)
                {
                    query += string.Join(" " + Table + " ", actionQueries);
                }
            }
            foreach (var condition in Conditions)
            {
                query += "\n" + condition.GetQuery();
            }
            return query;
        }

        public void AddAction(Action action)
        {
            if (Actions == null)
            {
                Actions = new List<Action>();

            }
            Actions.Add(action);
        }
        public void AddCondition(Condition condition)
        {
            if (Conditions == null)
            {
                Conditions = new List<Condition>();

            }
            Conditions.Add(condition);
        }

        public string GetType()
        {
            if (Actions == null || Conditions == null)
            {
                throw new NullReferenceException();
            }
            return Actions[0].Type;
        }
    }
}