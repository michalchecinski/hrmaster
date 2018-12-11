using HRMasterASP.Models.AAD;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace HRMasterASP.Helpers
{
    public class AADGraph
    {
        private AuthenticationContext authContext;
        private ClientCredential clientCredential;
        const string AAD_GRAPH_URI = "https://graph.windows.net";
        AppSettings _appSetings;
        public AADGraph(AppSettings appSettings)
        {
            _appSetings = appSettings;
            authContext = new AuthenticationContext(_appSetings.AuthenticationContext);
            string APPLICATION_ID = _appSetings.GroupApplicationId;
            string APPLICATION_SECRET = _appSetings.GroupApplicationKey;
            clientCredential = new ClientCredential(APPLICATION_ID, APPLICATION_SECRET);

        }
        public async Task<bool> IsUserInGroup(IEnumerable<Claim> userClaims, string groupId)
        {         
            AuthenticationResult token = await authContext.AcquireTokenAsync(AAD_GRAPH_URI, clientCredential);
                        
            var ID_CLAIM_NAME = _appSetings.UserIdClaimName;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessTokenType, token.AccessToken);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["api-version"] = "1.6";
            var uri = AAD_GRAPH_URI + "/myorganization/groups/" + groupId + "/$links/members?" + queryString;

            var members = await client.GetAsync(uri);
            bool userIsInGroup = false;
            if (members.Content != null)
            {
                var responseString = await members.Content.ReadAsStringAsync();
                AADGroupMembers gm = JsonConvert.DeserializeObject<AADGroupMembers>(responseString);
                List<string> userIds = new List<string>();
                foreach (var m in gm.value)
                {
                    userIds.Add(m.url.Split("/")[5]);
                }
                foreach (Claim c in userClaims)
                {
                    if (String.Compare(c.Type, ID_CLAIM_NAME) == 0)
                    {
                        foreach (string uId in userIds)
                        {
                            userIsInGroup = String.Compare(c.Value, uId) == 0 ? true : false;
                            break;
                        }
                        break;
                    }
                }
            }
            return userIsInGroup;
        }
    }
}
