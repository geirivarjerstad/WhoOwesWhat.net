using System.Data.Entity.Migrations;

namespace WhoOwesWhat.DataProvider
{
    public class MigrationConfigurations : DbMigrationsConfiguration<WhoOwesWhatContext>
    {
        public MigrationConfigurations()
        {
            // NICE! Sletter man et felt så forsvinner feltet!
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WhoOwesWhatContext context)
        {
          
            base.Seed(context);

            //#if (DEBUG)
            //{
            //    if (!context.Feedlist.Any())
            //    {
            //        var user = new UserCredential()
            //        {
            //            Displayname = "asdasd",
            //            Password = "asdasd",
            //            FeedPassword = "feedme"
            //        };

            //        var team = new Handleliste()
            //        {
            //            FeedName = "TeamAwesome",
            //            FeedId = "aaa1",
            //            Commodities = new Collection<Commodity>()
            //            {
            //                new Commodity {Displayname = "Garg", CommodityGuid = Guid.NewGuid()},
            //                new Commodity {Displayname = "Bob BlahBlah", CommodityGuid = Guid.NewGuid()}
            //            },
            //        };
            //        user.Handlelister.Add(team);

            //        context.UserCredentials.Add(user);

            //        try
            //        {
            //            context.SaveChanges();
            //        }
            //        catch (Exception ex)
            //        {
            //            // TODO Log Exception et eller annet sted!
            //            throw;
            //        }

            //    }
            //    //#endif
            //}

            //var lister = context.Feedlist.ToList();
        }
    }
}