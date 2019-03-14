using System;
using System.Data.Entity;

namespace MobileReward.Models
{
  public class MobiRewardDb : DbContext
  {
    public MobiRewardDb() : base("name=DefaultConnection")
    {

    }
    public DbSet<ApplicationSettings> ApplicationSettings { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<MasterState> MasterStates { get; set; }
    public DbSet<MasterCountry> MasterCountries { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionsCoupon> PromotionsCoupon { get; set; }
   
  }
}