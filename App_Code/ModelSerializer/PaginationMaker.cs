using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PaginationMaker
/// </summary>
public class PaginationMaker
{
    private int resourceID;
    private UriTemplate resourceUri;
    private Uri prefix;
    private int ID;
    public int per_page = 10;
    public int page = 1;
    public int items;
    public int pages;
    public Object urls;

    public PaginationMaker(int count, UriTemplate resourceUri, Uri prefix, int ID)
    {
        items = count;
        this.pages = calculateNumPages();
        formatUrls(resourceUri, prefix, ID);        
    }  

    public PaginationMaker(int count, UriTemplate resourceUri, Uri prefix,  int ID, int page)
    {
        items = count;
       
        
        this.pages = calculateNumPages();
        if (page > pages)
        {
            this.page = 1;
        }
        else 
        {
            this.page = page;
        }       
        formatUrls(resourceUri, prefix, ID);
    }   

    private int calculateNumPages()
    {
        if (items < per_page)
        {
            return 1;
        }
        else
        {
           return (int)Math.Ceiling((double)items / (double)per_page);
        }        
    }

    private void formatUrls(UriTemplate resourceUri, Uri prefix, int ID)
    {
        dynamic dynamicUrls = new ExpandoObject();
        //First
        
       

        if (page != 1)
        {

            Uri first = resourceUri.BindByPosition(prefix, ID.ToString(), "1");
            dynamicUrls.first = first;
            Uri previous = resourceUri.BindByPosition(prefix, ID.ToString(), (page - 1).ToString());
            dynamicUrls.previous = previous;
        }
        if (page != pages)
        {
            Uri last = resourceUri.BindByPosition(prefix, ID.ToString(), pages.ToString());
            dynamicUrls.last = last;
            Uri next = resourceUri.BindByPosition(prefix, ID.ToString(), (page + 1).ToString());
            dynamicUrls.next = next;
        }
        urls = dynamicUrls;
    }
}