using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.API.Models
{
    public class Action
    {
        public int Id { get; set; }
        public MetaData MetaData { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int? TriggerId {get; set;}
        [ForeignKey("TriggerId")]
        public Trigger Trigger { get; set; }
        public void SetMeta(MetaData meta)
        {
            MetaData = meta;
        }
        public bool HaveValidType(string type, string value)
        {
            if (type == "int")
            {
                try
                {
                    int temVal = Convert.ToInt16(value);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
            else if (type == "date")
            {

            }
            return false;
        }
        public bool Execute()
        {
            Assembly asl = Assembly.GetExecutingAssembly();
            Type[] assemblyTypes = asl.GetTypes();
            foreach (var t in assemblyTypes)
            {
                if (t.Name == MetaData.Table)
                {
                    foreach (var prop in t.GetProperties())
                    {
                        if (prop.Name == MetaData.Field)
                        {
                            prop.SetValue(null, Value);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public List<string> GetActionQuery()
        {
            switch (Type.ToLower())
            {
                case "create":
                    if (MetaData == null || Value.Split("_").Length != MetaData.Type.Split("_").Length || MetaData.Type.Split("_").Length != MetaData.Field.Split("_").Length)
                    {
                        return null;
                    }
                    var type = MetaData.Type.Split("_");
                    var value = "";
                    int count = 0;
                    //TODO multiple actions
                    foreach (var item in Value.Split("_"))
                    {
                        if (!HaveValidType("int", item))
                        {
                            throw new InvalidCastException(nameof(Int32));
                        }
                        if (type[count] == "int")
                        {
                            value += item + ", ";
                        }
                        else if (type[count] == "str")
                        {
                            value += "N'" + item + "', ";
                        }
                        count++;
                    }
                    return new List<string> { "INSERT INTO", " (" + MetaData.Field.Replace("_", ", ") + ") VALUES(" + value.Substring(0, value.Length - 2) + ")" };
                case "update":
                    if (MetaData == null || Value.Split("_").Length != MetaData.Type.Split("_").Length)
                    {
                        return null;
                    }
                    //TODO multiple actions
                    var action2 = "";
                    string[] values = Value.Split("_");
                    string[] metas = MetaData.Field.Split("_");
                    string[] types = MetaData.Type.Split("_");
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i <= values.Length - 2)
                        {
                            if (types[i] == "int")
                            {
                                if (!HaveValidType("int", values[i]))
                                {
                                    throw new InvalidCastException(nameof(Int32));
                                }
                                action2 += metas[i] + "=" + values[i] + ",";
                            }
                            else
                            {
                                action2 += metas[i] + "=N'" + values[i] + "',";
                            }
                        }
                        else
                        {
                            if (types[i] == "int")
                            {
                                if (!HaveValidType("int", values[i]))
                                {
                                    throw new InvalidCastException(nameof(Int32));
                                }
                                action2 += metas[i] + "=" + values[i];

                            }
                            else if (types[i] == "str")
                            {
                                action2 += metas[i] + "=N'" + values[i] + "'";
                            }
                        }
                    }
                    return new List<string> { "UPDATE", "SET " + action2 };
                case "delete":
                    return new List<string> { "DELETE FROM", " " };
                default:
                    return null;
            }
        }
    }
}