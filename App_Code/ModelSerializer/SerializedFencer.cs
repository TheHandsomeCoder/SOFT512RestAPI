using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SerializedFencer
/// </summary>
public class SerializedFencer
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Club { get; set; }
    public Uri ReferenceURI { get; set; }
    public Uri FencerResultsURI { get; set; }
   
   
    public SerializedFencer(Fencer x, Uri Prefix, UriTemplate FencerTemplate, UriTemplate FencerResultTemplate)
	{
        ID = x.FencerID;
        LastName = x.LastName;
        FirstName = x.FirstName;
        Club = x.Club;
        ReferenceURI = FencerTemplate.BindByPosition(Prefix, ID.ToString());
        FencerResultsURI = FencerResultTemplate.BindByPosition(Prefix, ID.ToString());
	}
}