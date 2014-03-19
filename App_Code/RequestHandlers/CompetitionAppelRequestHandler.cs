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
public class CompetitionAppelRequestHandler : BaseAppelRequstHandler
{
    protected UriTemplate CompetitionRootTemplate { get; set; }
    protected UriTemplate CompetitionResourceTemplate { get; set; }
    protected UriTemplate CompetitionResultTemplate { get; set; }

    public CompetitionAppelRequestHandler(HttpRequest Request, HttpResponse Response, Uri Prefix, UriTemplate CompetitionRootTemplate, UriTemplate CompetitionResourceTemplate, UriTemplate CompetitionResultTemplate, string AcceptHeader)
        : base(Request, Response, Prefix, AcceptHeader)
    {
        this.CompetitionRootTemplate = CompetitionRootTemplate;
        this.CompetitionResourceTemplate = CompetitionResourceTemplate;
        this.CompetitionResultTemplate = CompetitionResultTemplate;
        processRequest();
    }

    protected override void processGETRequest()
    {
        //Check if the request is on the Root Template
        if (CompetitionRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (CompetitionResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            //Parse the template to get the ID of the resource
            UriTemplateMatch uriTemplate = CompetitionResourceTemplate.Match(Prefix, Request.Url);
            using (var db = new AppelContext())
            {
                try
                {
                    int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                    Competition x = db.Competitions.Where(f => f.CompetitionID == ID).First();
                    setResponseVariables(HttpStatusCode.OK, serializeCompetition(x));
                }
                catch (System.InvalidOperationException)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
            }
        }
    }

    protected override void processPOSTRequest()
    {
        //Check if the request is on the Resource Template
        if (CompetitionResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Root Template
        else if (CompetitionRootTemplate.Match(Prefix, Request.Url) != null)
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string JSONInput = stream.ReadToEnd();

            //Check if the input matches the Input Schema (has reqired fields)
            if (Competition.validSchema(JSONInput))
            {
                Competition deserializedCompetition = JsonConvert.DeserializeObject<Competition>(JSONInput);
                if (deserializedCompetition.hasRequiredFields())
                {
                    using (var db = new AppelContext())
                    {
                        try
                        {
                            db.Competitions.Add(deserializedCompetition);
                            db.SaveChanges();
                            setResponseVariables(HttpStatusCode.Created, serializeCompetition(deserializedCompetition));
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
        if (CompetitionRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (CompetitionResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string JSONInput = stream.ReadToEnd();

            //Check if the input matches the Input Schema (has reqired fields)
            if (Competition.validSchema(JSONInput))
            {
                Competition deserializedFencer = JsonConvert.DeserializeObject<Competition>(JSONInput);

                if (deserializedFencer.hasRequiredFields())
                {
                    //Parse the template to get the ID of the resource
                    UriTemplateMatch uriTemplate = CompetitionResourceTemplate.Match(Prefix, Request.Url);
                    using (var db = new AppelContext())
                    {
                        try
                        {
                            int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                            Competition x = db.Competitions.Where(f => f.CompetitionID == ID).First();
                            x.updateCompetition(deserializedFencer);
                            db.SaveChanges();
                            setResponseVariables(HttpStatusCode.OK, serializeCompetition(x));
                        }
                        catch (System.InvalidOperationException e)
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
        if (CompetitionRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (CompetitionResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            //Parse the template to get the ID of the resource
            UriTemplateMatch uriTemplate = CompetitionResourceTemplate.Match(Prefix, Request.Url);
            using (var db = new AppelContext())
            {
                try
                {
                    int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                    Competition x = db.Competitions.Where(f => f.CompetitionID == ID).First();
                    db.Competitions.Remove(x);
                    db.SaveChanges();
                    setResponseVariables(HttpStatusCode.NoContent, null);
                }
                catch (System.InvalidOperationException)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }

            }

        }
    }


    private string serializeCompetition(Competition x)
    {
        SerializedCompetition c = new SerializedCompetition(x, Prefix, CompetitionResourceTemplate, CompetitionResultTemplate);
        return JsonConvert.SerializeObject(c);
    }







}