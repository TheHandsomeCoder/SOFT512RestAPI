using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SerializedFencer
/// </summary>
public class SerializedCompetition
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Venue { get; set; }   
    public Uri ReferenceURI { get; set; }
    public Uri CompetitionResultsURI { get; set; }


    public SerializedCompetition(Competition x, Uri Prefix, UriTemplate CompetitionTemplate, UriTemplate CompetitionResultTemplate)
	{
        ID = x.CompetitionID;
        Title = x.Title;
        Venue = x.Venue;
        ReferenceURI = CompetitionTemplate.BindByPosition(Prefix, ID.ToString());
        CompetitionResultsURI = CompetitionResultTemplate.BindByPosition(Prefix, ID.ToString());
	}
}