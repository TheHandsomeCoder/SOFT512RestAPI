using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AppelInitializer
/// </summary>
public class AppelInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AppelContext>
{
	protected override void Seed(AppelContext context)
	{
		var fencers = new List<Fencer>
			{
			new Fencer{FirstName="Carson",LastName="Alexander",Club="Salle Dublin"},
			new Fencer{FirstName="Meredith",LastName="Alonso",Club="Salle Dublin"},
			new Fencer{FirstName="Arturo",LastName="Anand",Club="Salle Dublin"},
			new Fencer{FirstName="Gytis",LastName="Barzdukas",Club="Salle Dublin"},
			new Fencer{FirstName="Yan",LastName="Li",Club="Pembroke Fencing Club"},
			new Fencer{FirstName="Peggy",LastName="Justice",Club="Pembroke Fencing Club"},
			new Fencer{FirstName="Laura",LastName="Norman",Club="Pembroke Fencing Club"},
			new Fencer{FirstName="Nino",LastName="Olivetto",Club="Pembroke Fencing Club"}
			};

		fencers.ForEach(f => context.Fencers.Add(f));
		context.SaveChanges();

        var keys = new List<APIKey>
			{
			    new APIKey{Key="111", isBlocked = false, maxNumberOfQueries=200, numberOfQueriesThisHour = 0, numberOfQueriesEver=0, resetTime = DateTime.Now.AddMinutes(60)},
			    new APIKey{Key="222", isBlocked = true,  maxNumberOfQueries=200, numberOfQueriesThisHour = 0, numberOfQueriesEver=0, resetTime = DateTime.Now.AddMinutes(60)},
		        new APIKey{Key="000", isBlocked = false,  maxNumberOfQueries=-1, numberOfQueriesThisHour = 0, numberOfQueriesEver=0, resetTime = DateTime.Now.AddMinutes(60)}
			};

        keys.ForEach(f => context.APIKeys.Add(f));
        context.SaveChanges();

		var competitions = new List<Competition>
			{
			new Competition{Title="Irish Open",Venue="UCD"},
			new Competition{Title="Dublin Epee",Venue="Dublin University"}           
			};
		competitions.ForEach(s => context.Competitions.Add(s));
		context.SaveChanges();


		var results = new List<Result>
			{
			new Result{FencerID=1,CompetitionID=1,Placing=1},
			new Result{FencerID=1,CompetitionID=2,Placing=1},

			new Result{FencerID=2,CompetitionID=1,Placing=2},
			new Result{FencerID=2,CompetitionID=2,Placing=2},

			new Result{FencerID=3,CompetitionID=1,Placing=3},
			new Result{FencerID=3,CompetitionID=2,Placing=3},

            new Result{FencerID=4,CompetitionID=1,Placing=3},
            new Result{FencerID=4,CompetitionID=2,Placing=3},

            new Result{FencerID=5,CompetitionID=1,Placing=5},
            new Result{FencerID=5,CompetitionID=2,Placing=5},

            new Result{FencerID=6,CompetitionID=1,Placing=6},
            new Result{FencerID=6,CompetitionID=2,Placing=6},

            new Result{FencerID=7,CompetitionID=1,Placing=7},
            new Result{FencerID=7,CompetitionID=2,Placing=7},

            new Result{FencerID=8,CompetitionID=1,Placing=8},
            new Result{FencerID=8,CompetitionID=2,Placing=8},




			};


		results.ForEach(s => context.Results.Add(s));
		context.SaveChanges();
	}
}