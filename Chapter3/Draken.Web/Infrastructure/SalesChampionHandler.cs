using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Draken.Web.Infrastructure
{
    public class SalesChampionHandler : AuthorizationHandler<SalesChampionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            SalesChampionRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "NumberOfAwards"))
            {
                return Task.CompletedTask;
            }
            var numberOfAwards = int.Parse(context.User.FindFirst(c => c.Type == "NumberOfAwards").Value);
            if (numberOfAwards > requirement.NumberOfAwards)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
