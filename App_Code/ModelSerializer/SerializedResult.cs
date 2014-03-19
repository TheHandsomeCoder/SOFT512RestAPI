using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SerializedFencer
/// </summary>
public class SerializedResult
{
    public int ID { get; set; }
    public int Placing { get; set; }
    public Uri ReferenceURI { get; set; }
    public Uri FencerURI { get; set; }
    public Uri CompetitionURI { get; set; }
   
    public SerializedResult(Result x, Uri Prefix, UriTemplate ResultResourceTemplate, UriTemplate FencerResourceTemplate,UriTemplate CompetitionResourceTemplate)
	{
        ID = x.ResultID;   
        Placing = x.Placing;
        ReferenceURI = ResultResourceTemplate.BindByPosition(Prefix, ID.ToString());
        FencerURI = FencerResourceTemplate.BindByPosition(Prefix, x.FencerID.ToString());
        CompetitionURI = CompetitionResourceTemplate.BindByPosition(Prefix, x.CompetitionID.ToString());
	}
}