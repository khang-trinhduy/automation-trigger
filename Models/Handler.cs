using System.Collections.Generic;
using Crm.Context;

namespace Automation.API.Models
{
    public class Handler
    {
        public SqlHelper SqlHelper { get; set; }

        public CrmContext Context {get; set;}

        public Handler(SqlHelper helper)
        {
            SqlHelper = helper;
        }

        public Handler(CrmContext context)
        {
            Context = context;
        }

        // public void ExecuteTrigger(Trigger trigger)
        // {
        //     var query = trigger.GetQuery();
        //     var type = trigger.GetType();
        //     string result = "";
        //     IEnumerable<List<object>> objs = null;
        //     if (type == "create")
        //     {
        //         result = SqlHelper.ExecuteWriter(query);
        //     }
        //     else if (type == "update")
        //     {
        //         result = SqlHelper.ExecuteUpdate(query);
        //     }
        //     else if (type == "delete")
        //     {
        //         result = SqlHelper.ExecuteDelete(query);
        //     }
        //     else
        //     {
        //         objs = SqlHelper.ExecuteReader(query);
        //     }
        // }
    }
}