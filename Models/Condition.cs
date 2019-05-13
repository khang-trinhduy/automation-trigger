using System;

namespace Automation.API.Models
{
    public class Condition
    {
        public int Id { get; set; }
        public string Operator { get; set; }
        public string Threshold { get; set; }
        public string Type { get; set; }
        public MetaData MetaData { get; set; }

        public void SetMeta(MetaData meta) {
            MetaData = meta;
        }

        public string GetOperator() {
            switch (Operator.ToLower())
            {
                case "greater":
                    return SqlOperator.GREATER;
                case "equal":
                    return SqlOperator.EQUAL;
                case "less":
                    return SqlOperator.LESS;
                case "greaterequal":
                    return SqlOperator.GREATEREQUAL;
                case "like":
                    return SqlOperator.LIKE;
                case "dif":
                    return SqlOperator.DIF;
                default:
                    return "";
            }
        }

        public string GetQuery() {
            if (Type == "" || Type != MetaData.Type)
            {
                throw new InvalidCastException();
            }
            string result = "WHERE ";
            switch (Type.ToLower())
            {
                case "string":
                    result += MetaData.Field + GetOperator() + "'" +Threshold + "'";
                    break;
                case "integer":
                    if (Threshold.Contains("_"))
                    {
                        result += MetaData.Field + " " + Threshold.Split("_")[0] + " AND" + Threshold.Split("_")[1];
                        break;
                    }
                    else
                    {
                        result += MetaData.Field + " " + GetOperator() + " " + Threshold;
                        break;
                    }
                default:
                    return "";
            }
            return result;
        }
    }
}