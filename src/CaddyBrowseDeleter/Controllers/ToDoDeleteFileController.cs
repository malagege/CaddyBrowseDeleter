using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;

namespace CaddyBrowseDeleter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoDeleteFileController : ControllerBase
{
    private readonly AppDbContext _context;

    public ToDoDeleteFileController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/ToDoDeleteFile
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoDeleteFile>>> GetToDoDeleteFiles()
    {
        return await _context.ToDoDeleteFiles.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoDeleteFile>> GetToDoDeleteFile(long id)
    {
        var toDoDeleteFile = await _context.ToDoDeleteFiles.FindAsync(id);

        if (toDoDeleteFile == null)
        {
            return NotFound();
        }

        return toDoDeleteFile;
    }

    // POST: api/ToDoDeleteFile
    [HttpPost]
    public async Task<ActionResult<ToDoDeleteFile>> PostToDoDeleteFile(ToDoDeleteFile toDoDeleteFile)
    {
        _context.ToDoDeleteFiles.Add(toDoDeleteFile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetToDoDeleteFiles), new { id = toDoDeleteFile.Id }, toDoDeleteFile);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoDeleteFile(long id)
    {
        var toDoDeleteFile = await _context.ToDoDeleteFiles.FindAsync(id);
        if (toDoDeleteFile == null)
        {
            return NotFound();
        }

        _context.ToDoDeleteFiles.Remove(toDoDeleteFile);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
