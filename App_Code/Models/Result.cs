using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Result
/// </summary>
public class Result
{
    private static JsonSchema schema = JsonSchema.Parse(@"{'type': 'object', 'properties': {'CompetitionID':{'type':'number', 'required' : true }, 'FencerID':{'type':'number', 'required' : true}, 'Placing':{'type':'number' , 'required' : true}}}");
    
    public int ResultID { get; set; }
    public int FencerID { get; set; }
    public int CompetitionID { get; set; }
    public int Placing { get; set; }
    public virtual Fencer Fencer { get; set; }
    public virtual Competition Competition { get; set; }


    public void updateResult(Result x, AppelContext db)
    {
         
        Placing = x.Placing;
        Competition = db.Competitions.Where(f => f.CompetitionID == x.CompetitionID).First();
        Fencer = db.Fencers.Where(f => f.FencerID == x.FencerID).First(); 
    }

    public bool hasRequiredFields()
    {
        if (Placing == null || ResultID == null || FencerID == null)
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
        else
        {
            return false;
        }
    }
}