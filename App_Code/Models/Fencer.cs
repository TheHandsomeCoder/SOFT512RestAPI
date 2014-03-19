using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Fencer
/// </summary>
public class Fencer
{
    private static JsonSchema schema = JsonSchema.Parse(@"{'type': 'object','properties': {'FirstName':{'type':'string', 'required' : true},'LastName':{'type':'string', 'required' : true},'Club':{'type':'string', 'required' : true}}}");

    public int FencerID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Club { get; set; }   

    public virtual ICollection<Result> Results { get; set; }

    public void updateFencer(Fencer x)
    {
	    LastName = x.LastName;
	    FirstName = x.FirstName;
	    Club = x.Club;
    }

    public bool hasRequiredFields()
    {
        if (FirstName == null || LastName == null || Club == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool validSchema(string JSONInput)
    {        
        JObject fencer = JObject.Parse(JSONInput);       
        if (fencer.IsValid(schema))
        {
            return true;
        }
        else {
            return false;
        }
    }
}