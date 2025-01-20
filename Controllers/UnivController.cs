using Microsoft.AspNetCore.Mvc;
using WebApplication2.Model;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnivController : ControllerBase
    {
        // Data universitas statis untuk contoh
        private static List<Univ> University = new List<Univ>
        {
            new Univ { Id = 1, Name = "Telkom University" },
            new Univ { Id = 2, Name = "Institut Teknologi Bandung" }
        };

        // GET: Get all universities
        [HttpGet]
        public ActionResult<IEnumerable<Univ>> GetUniversities()
        {
            return Ok(University);
        }

        // GET: Get a university by ID
        [HttpGet("{id}")]
        public ActionResult<Univ> GetUniversity(int id)
        {
            var univ = University.FirstOrDefault(u => u.Id == id);
            if (univ == null) return NotFound($"University with ID {id} not found.");
            return Ok(univ);
        }

        // POST: Add a new university
        [HttpPost]
        public ActionResult AddUniversity(Univ newUniv)
        {
            if (newUniv == null) return BadRequest("University data is required.");

            // Check for duplicate ID
            if (University.Any(u => u.Id == newUniv.Id))
            {
                return BadRequest($"University with ID {newUniv.Id} already exists.");
            }

            University.Add(newUniv);
            return CreatedAtAction(nameof(GetUniversity), new { id = newUniv.Id }, newUniv);
        }

        // PUT: Update a university
        [HttpPut("{id}")]
        public ActionResult UpdateUniversity(int id, Univ updatedUniv)
        {
            if (updatedUniv == null) return BadRequest("University data is required.");

            var univ = University.FirstOrDefault(u => u.Id == id);
            if (univ == null) return NotFound($"University with ID {id} not found.");

            univ.Name = updatedUniv.Name;

            return NoContent();
        }

        // DELETE: Delete a university
        [HttpDelete("{id}")]
        public ActionResult DeleteUniversity(int id)
        {
            var univ = University.FirstOrDefault(u => u.Id == id);
            if (univ == null) return NotFound($"University with ID {id} not found.");

            University.Remove(univ);
            return NoContent();
        }
    }
}
