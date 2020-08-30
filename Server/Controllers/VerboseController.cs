using CheckNote.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]/{id:int}")]
    public class VerboseController : ControllerBase
    {
        private readonly NoteService noteService;

        public VerboseController(NoteService noteService)
        {
            this.noteService = noteService;
        }

        public async Task<IActionResult> Note(int id) => (await noteService.Get(id)).Result();
    }
}
