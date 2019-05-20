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
        public List<Condition> All { get; set; }
        public List<Condition> Any {get; set;}

        public string GetExpression()
        {
            if (All == null || Any == null)
            {
                throw new NullReferenceException(nameof(Condition));
            }
            List<string> all = new List<string>();
            List<string> any = new List<string>();
            int count = 0;
            foreach (var c in All)
            {
                //TODO get linq expression
                all.Add(c.GetLinqExpression(count));
                count++;
            }
            count = 0;
            //TODO append All linqexp to one condition
            foreach (var c in Any)
            {
                //TODO get Any linq expression
                any.Add(c.GetLinqExpression(count));
                count++;
            }
            string exp = "(" + String.Join("and", all) + ")";
            if (any != null && any.Count >= 1)
            {
                exp += " or " + "(" + String.Join("or", any) + ")";
            }
            return exp;
            //TODO append Any linqexp to one condition
            //TODO merge All && Any into one linqexp
            //TODO return expression
        }
        public void AddAction(Action action)
        {
            if (Actions == null)
            {
                Actions = new List<Action>();

            }
            Actions.Add(action);
        }
        public void AddAll(Condition condition)
        {
            if (All == null)
            {
                All = new List<Condition>();

            }
            foreach (var c in All)
            {
                //NOTE condition on same table
                if (condition.MetaData != null && condition.MetaData.Table == c.MetaData.Table)
                {
                    All.Add(condition);
                }   
            }
        }
        public void AddAny(Condition condition)
        {
            if (Any == null)
            {
                Any = new List<Condition>();

            }
            foreach (var c in Any)
            {
                //NOTE condition on same table
                if (condition.MetaData != null && condition.MetaData.Table == c.MetaData.Table)
                {
                    Any.Add(condition);
                }   
            }
        }
        
    }
}