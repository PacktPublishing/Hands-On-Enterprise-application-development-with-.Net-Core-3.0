using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Draken.Web.Infrastructure
{
    public class SalesChampionRequirement : IAuthorizationRequirement
    {
        public SalesChampionRequirement(int numberOfAwards)
        {
            NumberOfAwards = numberOfAwards;
        }

        public int NumberOfAwards { get; set; }
    }
}
