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
        public List<Condition> Any { get; set; }

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
            count = 1;
            //TODO append All linqexp to one condition
            foreach (var c in Any)
            {
                //TODO get Any linq expression
                any.Add(c.GetLinqExpression(count));
                count++;
            }
            string exp = "(" + String.Join(" and ", all) + ")";
            if (any != null && any.Count >= 1)
            {
                exp += " or " + "(" + String.Join(" or ", any) + ")";
            }
            return exp;
            //TODO append Any linqexp to one condition
            //TODO merge All && Any into one linqexp
            //TODO return expression
        }
        public object[] GetParams()
        {
            int length = All.Count + Any.Count;
            object[] p = new object[length];
            int count = 0;
            for (int i = 0; i < All.Count; i++)
            {
                if (All[i].MetaData.Type == "int")
                {
                    p[i] = Convert.ToInt32(All[i].Threshold);
                }
                else if (All[i].MetaData.Type == "obj" && All[i].Threshold == "null")
                {
                    p[i] = null;
                }
                else
                    p[i] = All[i].Threshold;
                count++;
            }
            for (int i = 0; i < Any.Count; i++)
            {
                if (Any[i].MetaData.Type == "int")
                {
                    p[count] = Convert.ToInt32(Any[i].Threshold);
                }
                else if (Any[i].MetaData.Type == "obj" && Any[i].Threshold == "null")
                {
                    p[count] = null;
                }
                else
                    p[count] = Any[i].Threshold;
                count++;
            }
            return p;
        }

        public void AddAction(Action action)
        {
            if (Actions == null)
            {
                Actions = new List<Action>();

            }
            Actions.Add(action);
        }
        public void RemoveAction(Action action)
        {
            if (Actions != null)
            {
                Actions.Remove(action);
            }
        }
        public void AddAll(Condition condition)
        {
            if (All == null)
            {
                All = new List<Condition>();
                All.Add(condition);
            }
            else
            {
                foreach (var c in All)
                {
                    //NOTE condition on same table
                    if (condition.MetaData != null && condition.MetaData.Table == c.MetaData.Table)
                    {
                        All.Add(condition);
                    }
                }

            }
        }
        public void AddAny(Condition condition)
        {
            if (Any == null)
            {
                Any = new List<Condition>();
                Any.Add(condition);
            }
            else
            {
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
        public void RemoveCondition(Condition condition, bool isAll)
        {
            if (condition != null)
            {
                if (isAll)
                {
                    if (All != null)
                    {
                        All.Remove(condition);
                    }
                }
                else
                {
                    if (Any != null)
                    {
                        Any.Remove(condition);
                    }
                }
            }
        }

    }
}