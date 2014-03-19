using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SerializedFencer
/// </summary>
public class SerializedResultArrayV1
{
    public Uri ReferenceURI { get; set; }
    public PaginationMaker Pagination { get; set; }
    public List<SerializedResultArrayEntry> Results { get; set; }
    
    public SerializedResultArrayV1(List<Result> x, UriTemplateMatch templateMatch, Uri Prefix, UriTemplate ResultResourceTemplate, UriTemplate ResourceArrayTemplate)
	{           
        Pagination = createPagination(x.Count,templateMatch, Prefix, ResourceArrayTemplate);

        List<SerializedResultArrayEntry> r = new List<SerializedResultArrayEntry>();
        foreach (Result result in x)
        {
            r.Add(new SerializedResultArrayEntry(result, Prefix, ResultResourceTemplate));
        }
        Results = filterResults(r);
        ReferenceURI = ResourceArrayTemplate.BindByPosition(Prefix,templateMatch.BoundVariables["id"]);
	}

    public PaginationMaker createPagination(int count,UriTemplateMatch templateMatch, Uri Prefix, UriTemplate ResourceArrayTemplate)
    {
        int ID = Convert.ToInt32(templateMatch.BoundVariables["id"]);
        try
        {
            string page = templateMatch.BoundVariables["page"];
            

            if (page != null)
            {
                return new PaginationMaker(count, ResourceArrayTemplate, Prefix, ID, Convert.ToInt32(page));
            }
            else
            {
                return new PaginationMaker(count, ResourceArrayTemplate, Prefix, ID);
            }
        }
        catch (Exception e)
        {
            return new PaginationMaker(count, ResourceArrayTemplate, Prefix, ID);
        }
       
    }

    public List<SerializedResultArrayEntry> filterResults(List<SerializedResultArrayEntry> r)
    { 
        int index = ((Pagination.per_page * Pagination.page) - Pagination.per_page);
        int tempPer_Page = Pagination.per_page;

        if(Pagination.pages == 1 && (Pagination.per_page > Pagination.items))
        {
            tempPer_Page = Pagination.items;
        }
        else if(Pagination.per_page * Pagination.page > Pagination.items)
        {
            tempPer_Page = ((Pagination.per_page * Pagination.page) - Pagination.items);
        }
        return r.GetRange(index, tempPer_Page);
    }
}

public class SerializedResultArrayBeta
{   
    public List<SerializedResultArrayEntry> Results { get; set; }

    public SerializedResultArrayBeta(List<Result> x, UriTemplateMatch templateMatch, Uri Prefix, UriTemplate ResultResourceTemplate, UriTemplate ResourceArrayTemplate)
    {
        List<SerializedResultArrayEntry> r = new List<SerializedResultArrayEntry>();
        foreach (Result result in x)
        {
            r.Add(new SerializedResultArrayEntry(result, Prefix, ResultResourceTemplate));
        }
        Results = r;
        
    }
}