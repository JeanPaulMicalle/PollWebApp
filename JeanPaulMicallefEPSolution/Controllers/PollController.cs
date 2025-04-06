using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Domain.Models;
using DataAccess.Repositeries;
using JeanPaulMicallefEPSolution.FIlters;

namespace JeanPaulMicallefEPSolution.Controllers
{
    public class PollController : Controller
    {
        private readonly IPollRepository _pollRepository;

        //Repository is injected into the controller.
        public PollController(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        // GET Poll/Index
        public IActionResult Index()
        {
            var polls = _pollRepository.GetPolls()
                                       .OrderByDescending(p => p.DateCreated)
                                       .ToList();
            return View(polls);
        }

        // GET Poll/Details/{id}
        public IActionResult Details(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.PollId == id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        [EnsureSingleVote]
        // POS Poll/Vote
        public IActionResult Vote(int pollId, int optionNumber)
        {
            _pollRepository.Vote(pollId, optionNumber);

            string userId = User.Identity.Name;
            EnsureSingleVoteAttribute.RecordVote(userId, pollId);

            return RedirectToAction("Details", new { id = pollId });

        }

        // GET Poll/Results/{id}
        public IActionResult Results(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.PollId == id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        // GET Poll/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST Poll/Create
        [HttpPost]
        public IActionResult Create(PollCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _pollRepository.CreatePoll(model.Title, model.Option1Text, model.Option2Text, model.Option3Text);
            return RedirectToAction("Index");
        }
    }
}