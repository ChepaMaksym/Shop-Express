using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_Express.Data;
using Shop_Express.Models;
using Shop_Express.ViewModels;
using System.Security.Claims;

namespace TaskAuthenticationAuthorization.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    public class JobController : Controller
    {
        private readonly JobbingContext _context;

        public JobController(JobbingContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            var userRole = HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            IEnumerable<Job> jobs;

            switch (userRole)
            {
                case "Admin":
                    jobs = await _context.Jobs
                        .Include(j => j.User)
                        .ToListAsync();
                    break;

                case "Reader":
                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (currentUser != null)
                    {
                        jobs = await _context.Jobs
                            .Include(j => j.User)
                            .Where(j => j.User.Email == currentUser.Email)
                            .ToListAsync();
                    }
                    else
                        return NotFound();
                    break;

                default: return NotFound();
            }

            return View(jobs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var jobs = await _context.Jobs
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobs == null) return NotFound();

            return View(jobs);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Job, User")] JobModel jobModel)
        {
            var job = await _context.Jobs
               .Include(j => j.User)
               .FirstOrDefaultAsync(j => j.Id == jobModel.Job.Id);

            if (job == null)
            {
                job = new Job(jobModel);
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var job = await _context.Jobs
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null) return NotFound();

            var jobModel = new JobModel
            {
                Job = job,
                User = job.User
            };

            return View(jobModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Job, User")] JobModel jobModel)
        {

            var job = await _context.Jobs
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job != null && id == jobModel.Job.Id)
            {
                try
                {
                    job = new Job(jobModel);

                    _context.Update(job);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty, "Concurrency error occurred while saving the job.");
                }
            }
            return View(jobModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var job = await _context.Jobs
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null) return NotFound();

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if(job == null) return NotFound();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
