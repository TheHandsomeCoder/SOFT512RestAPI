using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for BaseAppelRequstHandler
/// </summary>
public abstract class BaseAppelRequstHandler
{
    protected HttpRequest Request { get; set; }
    protected HttpResponse Response { get; set; }
    protected String ResponseBody { get; set; }
    protected Uri Prefix { get; set; }
    protected string StatusCode { get; set; }
    protected string VersionHeader { get; set; }

    public BaseAppelRequstHandler(HttpRequest Request, HttpResponse Response, Uri Prefix, string VersionHeader)
	{
        this.Request = Request;
        this.Response = Response;
        this.Prefix = Prefix;
        this.VersionHeader = VersionHeader;
	}

    protected void processRequest()
    {
        switch (Request.HttpMethod)
        {
            case "GET":
                {
                    processGETRequest();
                    break;
                }
            case "POST":
                {
                    processPOSTRequest();
                    break;
                }
            case "PUT":
                {
                    processPUTRequest();
                    break;
                }
            case "DELETE":
                {
                    processDELETERequest();
                    break;
                }
            default:
                {
                    //
                    // TODO: 404.6 Error
                    //
                    break;
                }
        }
    }

    protected virtual void processGETRequest()
    { }

    protected virtual void processPOSTRequest()
    { }

    protected  virtual void processPUTRequest()
    { }

    protected virtual void processDELETERequest()
    { }

    protected virtual void responseBuilder()
    { }

    public string getResponseBody()
    {
        return ResponseBody;
    }

    public string getStatusCode()
    {
        return StatusCode;
    }

    protected void setResponseVariables(HttpStatusCode status, string messageBody)
    {
        StatusCode = status.ToString();
        ResponseBody = messageBody;

    }

    protected void setResponseVariables(String status, string messageBody)
    {
        StatusCode = status.ToString();
        ResponseBody = messageBody;

    }

}