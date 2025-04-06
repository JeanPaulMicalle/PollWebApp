using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace JeanPaulMicallefEPSolution.FIlters
{
    public class EnsureSingleVoteAttribute: ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<(string userId, int pollId), bool> _voteRecords = new ConcurrentDictionary<(string, int), bool>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            // Ensure the user is authenticated; if not, redirect to the login page.
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Check if the action arguments contain "pollId" and that it is an integer.
            if (context.ActionArguments.TryGetValue("pollId", out var pollIdObj) && pollIdObj is int pollId)
            {
                string userId = user.Identity.Name;

                // If a vote record exists for this user and poll, prevent another vote.
                if (_voteRecords.ContainsKey((userId, pollId)))
                {
                    context.Result = new RedirectToActionResult("Details", "Poll", new { id = pollId, message = "You have already voted in this poll." });
                    return;
                }
            }
            else
            {
                context.Result = new BadRequestResult();
                return;
            }

            base.OnActionExecuting(context);
        }

        public static void RecordVote(string userId, int pollId)
        {
            _voteRecords.TryAdd((userId, pollId), true);
        }
    }
}
