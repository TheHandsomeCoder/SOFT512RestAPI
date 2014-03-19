using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

/// <summary>
/// Summary description for Competition
/// </summary>
public class APIKey
{
    public int APIKeyID { get; set; }
    public string Key { get; set; }    
    public bool isBlocked { get; set; }
    public long numberOfQueriesThisHour { get; set; } 
    public long numberOfQueriesEver { get; set; }
    public long maxNumberOfQueries { get; set; }
    public DateTime resetTime { get; set; }
        
}