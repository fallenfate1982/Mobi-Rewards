using System;
using WebMatrix.WebData;

namespace MobileReward
{
  public class DataBaseConfig
  {
    public static void RegisterDatabaseConnection()
    {
      //TODO : Move to register area
      WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
    }
  }
}