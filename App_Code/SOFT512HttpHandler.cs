using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for SOFT512HttpHandler
/// </summary>
public class SOFT512HttpHandler : IHttpHandler
{
    Uri prefix = new Uri("http://localhost:9630");
    //Uri prefix = new Uri("http://fostvm.fost.plymouth.ac.uk/Modules/SOFT338/somalley/");	
    UriTemplate fencerRootTemplate = new UriTemplate("fencers");
    UriTemplate fencerResourceTemplate = new UriTemplate("fencers/{id}");
    UriTemplate fencerResultsTemplate = new UriTemplate("fencers/{id}/results?page={page}");

    UriTemplate resultsRootTemplate = new UriTemplate("results");
    UriTemplate resultsResourceTemplate = new UriTemplate("results/{id}");

    UriTemplate competitionsRootTemplate = new UriTemplate("competitions");
    UriTemplate competitionsResourceTemplate = new UriTemplate("competitions/{id}");
    UriTemplate competitionsResultsTemplate = new UriTemplate("competitions/{id}/results?page={page}");

    string defaultHeader = "application/vnd.appel.v1+json";
    string[] acceptedheaders = { "application/vnd.appel.v1+json", "application/vnd.appel.beta+json" };
    bool rateLimitExceeded = false;

    public bool IsReusable
    {
        get { return true; }
    }

    public SOFT512HttpHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }    

    private string GetAcceptHeader(HttpRequest Request)
    {
        if (Request.AcceptTypes != null)
        {
            foreach (string header in Request.AcceptTypes)
            {
                foreach (string versiontype in acceptedheaders)
                {
                    if (header == versiontype)
                    {
                        return header;
                    }
                }
            }
        }
        return defaultHeader;

    }

    private bool HasValidAPIKey(HttpRequest Request)
    {
        if (Request.Headers != null)
        {
            string key = Request.Headers.Get("X-API-Key");
            if (key == null)
            {
                return false;
            }
            else
            {
                try
                {
                    using (var db = new AppelContext())
                    {
                        APIKey k = db.APIKeys.Where(a => a.Key == key).First();
                        if (DateTime.Now > k.resetTime)
                        {
                            k.resetTime = DateTime.Now.AddMinutes(60);
                            k.numberOfQueriesThisHour = 0;
                        }
                        if (!k.isBlocked && k.numberOfQueriesThisHour < k.maxNumberOfQueries)
                        {                            
                            k.numberOfQueriesEver += 1;
                            k.numberOfQueriesThisHour += 1;
                            return true;
                        }
                        else if (k.isBlocked)
                        {
                            k.numberOfQueriesEver += 1;
                            k.numberOfQueriesThisHour += 1;
                            return false;
                        }
                        else
                        {
                            k.numberOfQueriesEver += 1;
                            k.numberOfQueriesThisHour += 1;
                            rateLimitExceeded = true;
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

        }
        return false;

    }

    public void ProcessRequest(HttpContext context)
    {
        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;       
        if (HasValidAPIKey(Request))
        {
            string acceptHeader = GetAcceptHeader(Request);

            BaseAppelRequstHandler handler;

            if (matchTemplate(fencerResourceTemplate, Request.Url) || matchTemplate(fencerRootTemplate, Request.Url))
            {
                handler = new FencerAppelRequestHandler(Request, Response, prefix, fencerRootTemplate, fencerResourceTemplate, fencerResultsTemplate, acceptHeader);
                Response = buildResponse(Response, handler.getStatusCode(), Request.HttpMethod, handler.getResponseBody());
            }
            else if (matchTemplate(competitionsResourceTemplate, Request.Url) || matchTemplate(competitionsRootTemplate, Request.Url))
            {
                handler = new CompetitionAppelRequestHandler(Request, Response, prefix, competitionsRootTemplate, competitionsResourceTemplate, competitionsResultsTemplate, acceptHeader);
                Response = buildResponse(Response, handler.getStatusCode(), Request.HttpMethod, handler.getResponseBody());
            }
            else if (matchTemplate(resultsResourceTemplate, Request.Url) || matchTemplate(resultsRootTemplate, Request.Url))
            {
                handler = new ResultAppelRequestHandler(Request, Response, prefix, resultsRootTemplate, resultsResourceTemplate, fencerResourceTemplate, competitionsResourceTemplate, acceptHeader);
                Response = buildResponse(Response, handler.getStatusCode(), Request.HttpMethod, handler.getResponseBody());
            }
            else if (matchTemplate(fencerResultsTemplate, Request.Url))
            {
                handler = new FencerResultAppelRequestHandler(Request, Response, prefix, fencerResultsTemplate, resultsResourceTemplate, acceptHeader);
                Response = buildResponse(Response, handler.getStatusCode(), Request.HttpMethod, handler.getResponseBody());
            }
            else if (matchTemplate(competitionsResultsTemplate, Request.Url))
            {
                handler = new FencerResultAppelRequestHandler(Request, Response, prefix, competitionsResultsTemplate, resultsResourceTemplate, acceptHeader);
                Response = buildResponse(Response, handler.getStatusCode(), Request.HttpMethod, handler.getResponseBody());
            }
            else
            {
                Response = buildResponse(Response, HttpStatusCode.NotFound.ToString(), Request.HttpMethod, null);
            }
        }
        else if (rateLimitExceeded)
        {
            Response = buildResponse(Response, "TooManyRequests", Request.HttpMethod, null);
        }
        else
        {
            Response = buildResponse(Response, HttpStatusCode.Unauthorized.ToString(), Request.HttpMethod, null);
        }
    }

    private bool matchTemplate(UriTemplate template, Uri uri)
    {
        if (template.Match(prefix, uri) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private HttpResponse buildResponse(HttpResponse Response, string statusCode, string method, string message)
    {
        if (statusCode == HttpStatusCode.OK.ToString())
        {
            Response.StatusCode = 200;
            Response.Write(message);
        }
        else if (statusCode == HttpStatusCode.Created.ToString())
        {
            Response.StatusCode = 201;
            Response.Write(message);
        }
        else if (statusCode == HttpStatusCode.NoContent.ToString())
        {
            Response.StatusCode = 204;
        }
        else if (statusCode == HttpStatusCode.Unauthorized.ToString())
        {
            Response.StatusCode = 401;
            message = "You’re attempting to access a resource that first requires authentication or are using an API key that has been blocked. See Authenticating in the documentaion";
            Response.Write(formatResponseMessage(message));
        }
        else if (statusCode == HttpStatusCode.Forbidden.ToString())
        {
            Response.StatusCode = 403;
            message = "You’re not allowed to access this resource. Even if you authenticated, or already have, you simply don’t have permission.";
            Response.Write(formatResponseMessage(message));
        }
        else if (statusCode == HttpStatusCode.NotFound.ToString())
        {
            Response.StatusCode = 404;
            message = "The resource you requested doesn’t exist.";
            Response.Write(formatResponseMessage(message));
        }
        else if (statusCode == HttpStatusCode.MethodNotAllowed.ToString())
        {
            Response.StatusCode = 405;
            message = "You’re trying to use an HTTP verb that isn’t supported by the resource.";
            Response.Write(formatResponseMessage(message));
        }
        else if (statusCode == "Unprocessable")
        {
            Response.StatusCode = 422;
            message = "Your request was well-formed, but there’s something semantically wrong with the body of the request. This can be due to malformed JSON, a parameter that’s missing or the wrong type, or trying to perform an action that doesn’t make any sense.";
            Response.Write(formatResponseMessage(message));
        }
        else if (statusCode == "TooManyRequests")
        {
            Response.StatusCode = 429;
            message = "This API only supports 200 Requests Per Key Per Hour for normal API Keys.";
            Response.Write(formatResponseMessage(message));
        }

        else if (statusCode == HttpStatusCode.InternalServerError.ToString())
        {
            Response.StatusCode = 500;
            message = "Something went wrong on our end while attempting to process your request.";
            Response.Write(formatResponseMessage(message));
        }
        else
        {
            Response.StatusCode = 400;
            message = "An unknown error what thrown.";
            Response.Write(formatResponseMessage(message));
        }

        return Response;
    }

    private string formatResponseMessage(string messageBody)
    {
        return JsonConvert.SerializeObject(new { Message = messageBody });
    }
}