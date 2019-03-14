using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using MobiRewards.Service.Model;
using MobiReward.Common;

namespace MobiRewards.Service
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MobiRewardService : System.Web.Services.WebService
    {

        [WebMethod]
        public bool Login(string userId, string password)
        {
            var db = new MobileRewardsEntities();
        }
    }
}
