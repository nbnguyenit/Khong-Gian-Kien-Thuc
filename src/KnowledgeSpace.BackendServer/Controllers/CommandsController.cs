using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class CommandsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CommandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommands()
        {
            // var user = User.Identity.Name;
            // var query = _context.Commands;
            // tang tac do ung -> AsNoTracking() "trong trong hop chi doc du lieu"
            var query = _context.Commands.AsNoTracking();
            var CommandVms = await query.Select(x=>new CommandVm() { 
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return Ok(CommandVms);
        }
    }
}
