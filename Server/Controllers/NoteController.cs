using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/{id?}")]
    public class NoteController : ControllerBase
    {
        public IActionResult Get(int id)
        {
            return Ok($"notes. id: {id}");
        }
    }
}
