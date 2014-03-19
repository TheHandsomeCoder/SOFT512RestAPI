using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SerializedFencer
/// </summary>
public class SerializedResultArrayEntry
{
    public int ID { get; set; }    
    public Uri ReferenceURI { get; set; }

    public SerializedResultArrayEntry(Result x, Uri Prefix, UriTemplate ResultResourceTemplate)
	{
        ID = x.ResultID;         
        ReferenceURI = ResultResourceTemplate.BindByPosition(Prefix, ID.ToString());     
	}
}