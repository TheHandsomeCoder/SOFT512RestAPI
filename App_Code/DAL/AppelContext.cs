using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

/// <summary>
/// Summary description for AppelContext
/// </summary>
public class AppelContext : DbContext
{
    public AppelContext()
        : base("AppelContextConnectionString")     
	{
        Database.SetInitializer<AppelContext>(new AppelInitializer());
	}

    public DbSet<Fencer> Fencers { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<APIKey> APIKeys { get; set; }
    public DbSet<Competition> Competitions { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }

}