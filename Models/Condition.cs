using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Crm;

namespace Automation.API.Models
{
    public class Condition
    {
        public int Id { get; set; }
        public string Operator { get; set; }
        public string Threshold { get; set; }
        public string Type { get; set; }
        public MetaData MetaData { get; set; }
        public void SetMeta(MetaData meta)
        {
            MetaData = meta;
        }
        public string GetOperator()
        {
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
        public string GetLinqExpression(int i)
        {
            if(MetaData == null)
            {
                throw new NullReferenceException(nameof(MetaData));
            }
            //TODO check if null for condition type of object
            // if (Type == "obj")
            // {
            //     return MetaData.Field + " " + SqlOperator.DIF + " @" + i.ToString();
            // }
            return MetaData.Field + " " + GetOperator() + " @" + i.ToString();
            
        }

        public string GetQuery()
        {
            if (Type == "" || Type != MetaData.Type)
            {
                throw new InvalidCastException();
            }
            if (GetOperator() == "")
            {
                return "";
            }
            string result = "WHERE ";
            switch (Type.ToLower())
            {
                case "str":
                    result += MetaData.Field + GetOperator() + "N'" + Threshold + "'";
                    break;
                case "int":
                    if (Threshold.Contains("_"))
                    {
                        result += MetaData.Field + " " + Convert.ToInt32(Threshold.Split("_")[0]) + " AND" + Convert.ToInt32(Threshold.Split("_")[1]);
                        break;
                    }
                    else
                    {
                        result += MetaData.Field + " " + GetOperator() + " " + Convert.ToInt32(Threshold);
                        break;
                    }
                default:
                    return "";
            }
            return result;
        }
    }
}