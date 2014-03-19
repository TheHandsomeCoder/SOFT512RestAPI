using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for FencerAppelRequestHandler
/// </summary>
public class FencerResultAppelRequestHandler : BaseAppelRequstHandler
{
    protected UriTemplate FencerResultsTemplate { get; set; }
    protected UriTemplate ResultResourceTemplate { get; set; }

    public FencerResultAppelRequestHandler(HttpRequest Request, HttpResponse Response, Uri Prefix, UriTemplate FencerResultsTemplate, UriTemplate ResultResourceTemplate, string AcceptHeader)
        : base(Request, Response, Prefix, AcceptHeader)
    {
        this.FencerResultsTemplate = FencerResultsTemplate;
        this.ResultResourceTemplate = ResultResourceTemplate;    
        processRequest();
    }

    protected override void processGETRequest()
    {
        if (FencerResultsTemplate.Match(Prefix, Request.Url) != null)
        {
            //Parse the template to get the ID of the resource
            UriTemplateMatch uriTemplate = FencerResultsTemplate.Match(Prefix, Request.Url);
            using (var db = new AppelContext())
            {
                try
                {
                    int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                    var x = db.Results.Where(f => f.FencerID == ID).ToList();
                    setResponseVariables(HttpStatusCode.OK, serializeResultsArray(x, uriTemplate));
                }
                catch (System.InvalidOperationException)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
            }
        }
        else
        {
            setResponseVariables(HttpStatusCode.NotFound, null);
        }
    }

    protected override void processPOSTRequest()
    {
        setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
    }

    protected override void processPUTRequest()
    {
        setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
    }

    protected override void processDELETERequest()
    {
        setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
    }


    private string serializeResultsArray(List<Result> x, UriTemplateMatch templateMatch)
    {
        if (VersionHeader == "application/vnd.appel.v1+json")
        {
            SerializedResultArrayV1 c = new SerializedResultArrayV1(x, templateMatch, Prefix, ResultResourceTemplate, FencerResultsTemplate);
            return JsonConvert.SerializeObject(c);
        }
        else if (VersionHeader == "application/vnd.appel.beta+json")
        {
            SerializedResultArrayBeta c = new SerializedResultArrayBeta(x, templateMatch, Prefix, ResultResourceTemplate, FencerResultsTemplate);
            return JsonConvert.SerializeObject(c);
        }
        else 
        {
            SerializedResultArrayV1 c = new SerializedResultArrayV1(x, templateMatch, Prefix, ResultResourceTemplate, FencerResultsTemplate);
            return JsonConvert.SerializeObject(c);
        }
    }







}