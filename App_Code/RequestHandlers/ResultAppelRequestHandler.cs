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
public class ResultAppelRequestHandler : BaseAppelRequstHandler
{
    protected UriTemplate ResultRootTemplate { get; set; }
    protected UriTemplate ResultResourceTemplate { get; set; }
    protected UriTemplate FencerResourceTemplate { get; set; }
    protected UriTemplate CompetitionResourceTemplate { get; set; }

    public ResultAppelRequestHandler(HttpRequest Request, HttpResponse Response, Uri Prefix, UriTemplate ResultRootTemplate, UriTemplate ResultResourceTemplate, UriTemplate FencerResourceTemplate, UriTemplate CompetitionResourceTemplate, string AcceptHeader)
        : base(Request, Response, Prefix, AcceptHeader)
    {
        this.ResultRootTemplate = ResultRootTemplate;
        this.ResultResourceTemplate = ResultResourceTemplate;
        this.FencerResourceTemplate = FencerResourceTemplate;
        this.CompetitionResourceTemplate = CompetitionResourceTemplate;
        processRequest();
    }

    protected override void processGETRequest()
    {
        //Check if the request is on the Resource Template
        if (ResultRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        else if (ResultResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            //Parse the template to get the ID of the resource
            UriTemplateMatch uriTemplate = ResultResourceTemplate.Match(Prefix, Request.Url);
            using (var db = new AppelContext())
            {
                try
                {
                    int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                    var x = db.Results.Where(f => f.FencerID == ID).First();
                    setResponseVariables(HttpStatusCode.OK, serializeResult(x));
                }
                catch (System.InvalidOperationException)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
                catch (Exception e)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
            }
        }
    }

    protected override void processPOSTRequest()
    {
        //Check if the request is on the Resource Template
        if (ResultResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Root Template
        else if (ResultRootTemplate.Match(Prefix, Request.Url) != null)
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string JSONInput = stream.ReadToEnd();

            //Check if the input matches the Input Schema (has reqired fields)
            if (Result.validSchema(JSONInput))
            {
                Result deserializedResult = JsonConvert.DeserializeObject<Result>(JSONInput);
                if (deserializedResult.hasRequiredFields())
                {
                    using (var db = new AppelContext())
                    {
                        try
                        {
                            db.Results.Add(deserializedResult);
                            db.SaveChanges();
                            setResponseVariables(HttpStatusCode.Created, serializeResult(deserializedResult));
                        }
                        catch (Exception e)
                        {
                            setResponseVariables(HttpStatusCode.NotFound, null);
                        }

                    }
                }
                else
                {
                    setResponseVariables("Unprocessable", null);
                }
            }
            else
            {
                setResponseVariables("Unprocessable", null);
            }
        }
    }

    protected override void processPUTRequest()
    {
        //Check if the request is on the Root Template
        if (ResultRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (ResultResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string JSONInput = stream.ReadToEnd();

            //Check if the input matches the Input Schema (has reqired fields)
            if (Result.validSchema(JSONInput))
            {
                Result deserializedResult = JsonConvert.DeserializeObject<Result>(JSONInput);

                if (deserializedResult.hasRequiredFields())
                {
                    //Parse the template to get the ID of the resource
                    UriTemplateMatch uriTemplate = ResultResourceTemplate.Match(Prefix, Request.Url);
                    using (var db = new AppelContext())
                    {
                        try
                        {
                            int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                            Result x = db.Results.Where(f => f.ResultID == ID).First();
                            x.updateResult(deserializedResult, db);
                            db.SaveChanges();
                            setResponseVariables(HttpStatusCode.OK, serializeResult(x));
                        }
                        catch (System.InvalidOperationException e)
                        {
                            setResponseVariables(HttpStatusCode.NotFound, null);
                        }
                        catch (Exception e)
                        {
                            setResponseVariables(HttpStatusCode.NotFound, null);
                        }
                    }
                }
                else
                {
                    setResponseVariables("Unprocessable", null);
                }
            }
            else
            {
                setResponseVariables("Unprocessable", null);
            }
        }
    }

    protected override void processDELETERequest()
    {
        //Check if the request is on the Root Template
        if (ResultRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (ResultResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            //Parse the template to get the ID of the resource
            UriTemplateMatch uriTemplate = ResultResourceTemplate.Match(Prefix, Request.Url);
            using (var db = new AppelContext())
            {
                try
                {
                    int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                    Result x = db.Results.Where(f => f.ResultID == ID).First();
                    db.Results.Remove(x);
                    db.SaveChanges();
                    setResponseVariables(HttpStatusCode.NoContent, null);
                }
                catch (System.InvalidOperationException)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
                catch (Exception e)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
            }

        }
    }


    private string serializeResult(Result x)
    {
        SerializedResult c = new SerializedResult(x, Prefix, ResultResourceTemplate, FencerResourceTemplate, CompetitionResourceTemplate);
        return JsonConvert.SerializeObject(c);
    }







}