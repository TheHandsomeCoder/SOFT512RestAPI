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
public class FencerAppelRequestHandler : BaseAppelRequstHandler
{
    protected UriTemplate FencerRootTemplate { get; set; }
    protected UriTemplate FencerResourceTemplate { get; set; }
    protected UriTemplate FencerResultTemplate { get; set; }

    public FencerAppelRequestHandler(HttpRequest Request, HttpResponse Response, Uri Prefix, UriTemplate FencerRootTemplate, UriTemplate FencerResourceTemplate, UriTemplate FencerResultTemplate, string AcceptHeader)
        : base(Request, Response, Prefix, AcceptHeader)
    {
        this.FencerRootTemplate = FencerRootTemplate;
        this.FencerResourceTemplate = FencerResourceTemplate;
        this.FencerResultTemplate = FencerResultTemplate;
        processRequest();
    }

    protected override void processGETRequest()
    {
        //Check if the request is on the Root Template
        if (FencerRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (FencerResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            //Parse the template to get the ID of the resource
            UriTemplateMatch uriTemplate = FencerResourceTemplate.Match(Prefix, Request.Url);
            using (var db = new AppelContext())
            {
                try
                {
                    int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                    Fencer x = db.Fencers.Where(f => f.FencerID == ID).First();
                    setResponseVariables(HttpStatusCode.OK, serializeFencer(x));
                }
                catch (System.InvalidOperationException)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
                catch(Exception e)
                {
                    setResponseVariables(HttpStatusCode.NotFound, null);
                }
            }
        }
    }

    protected override void processPOSTRequest()
    {
        //Check if the request is on the Resource Template
        if (FencerResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Root Template
        else if (FencerRootTemplate.Match(Prefix, Request.Url) != null)
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string JSONInput = stream.ReadToEnd();

            //Check if the input matches the Input Schema (has reqired fields)
            if (Fencer.validSchema(JSONInput))
            {
                Fencer deserializedFencer = JsonConvert.DeserializeObject<Fencer>(JSONInput);
                if (deserializedFencer.hasRequiredFields())
                {
                    using (var db = new AppelContext())
                    {
                        try
                        {
                            db.Fencers.Add(deserializedFencer);
                            db.SaveChanges();
                            setResponseVariables(HttpStatusCode.Created, serializeFencer(deserializedFencer));
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
        if (FencerRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (FencerResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string JSONInput = stream.ReadToEnd();

            //Check if the input matches the Input Schema (has reqired fields)
            if (Fencer.validSchema(JSONInput))
            {
                Fencer deserializedFencer = JsonConvert.DeserializeObject<Fencer>(JSONInput);

                if (deserializedFencer.hasRequiredFields())
                {
                    //Parse the template to get the ID of the resource
                    UriTemplateMatch uriTemplate = FencerResourceTemplate.Match(Prefix, Request.Url);
                    using (var db = new AppelContext())
                    {
                        try
                        {
                            int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                            Fencer x = db.Fencers.Where(f => f.FencerID == ID).First();
                            x.updateFencer(deserializedFencer);
                            db.SaveChanges();
                            setResponseVariables(HttpStatusCode.OK, serializeFencer(x));
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
        if (FencerRootTemplate.Match(Prefix, Request.Url) != null)
        {
            setResponseVariables(HttpStatusCode.MethodNotAllowed, null);
        }
        //Check if the request is on the Resource Template
        else if (FencerResourceTemplate.Match(Prefix, Request.Url) != null)
        {
            //Parse the template to get the ID of the resource
            UriTemplateMatch uriTemplate = FencerResourceTemplate.Match(Prefix, Request.Url);
            using (var db = new AppelContext())
            {
                try
                {
                    int ID = Convert.ToInt32(uriTemplate.BoundVariables["id"]);
                    Fencer x = db.Fencers.Where(f => f.FencerID == ID).First();
                    db.Fencers.Remove(x);
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


    private string serializeFencer(Fencer x)
    {
        SerializedFencer f = new SerializedFencer(x, Prefix, FencerResourceTemplate, FencerResultTemplate);
        return JsonConvert.SerializeObject(f);
    }







}