using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

/// <summary>
/// Summary description for Competition
/// </summary>
public class Competition
{
    private static JsonSchema schema = JsonSchema.Parse(@"{'type': 'object','properties': {'Title':{'type':'string', 'required' : true},'Venue':{'type':'string', 'required' : true}}}");


    public int CompetitionID { get; set; }
    public string Title { get; set; }    
    public string Venue { get; set; }

    public virtual ICollection<Result> Results { get; set; }

    public void updateCompetition(Competition x)
    {
        Title = x.Title;
        Venue = x.Venue;
      
    }

    public bool hasRequiredFields()
    {
        if (Title == null || Venue == null)
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