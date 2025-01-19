using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using CaddyBrowseDeleter.ViewModel;

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
    [HttpGet("{*path}")]
    public async Task<ActionResult<List<ToDoDeleteFile>>> GetToDoDeleteFiles(string? path)
    {
        if (path != null)
        {
            return await _context.ToDoDeleteFiles
            .Include(x => x.Users)
            .Where(x => x.DirPath == path).ToListAsync();
        }
        return await _context.ToDoDeleteFiles.Include(x => x.Users).ToListAsync();
    }



    // POST: api/ToDoDeleteFile
    [HttpPost]
    public async Task<ActionResult<ToDoDeleteFile>> PostToDoDeleteFile(DoDeleteFileViewModel viewModel)
    {
        // 查出使用者
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == viewModel.UserName);
        if (user == null)
        {
            return BadRequest("使用者不存在");
        }
        // 查看是否有刪除檔案
        var toDoDeleteFile = await _context.ToDoDeleteFiles.FirstOrDefaultAsync(x => x.FilePath == viewModel.FilePath);
        // 有檔案代表要取消刪除要求
        if (toDoDeleteFile != null && toDoDeleteFile.Users.Contains(user))
        {
            _context.ToDoDeleteFiles.Remove(toDoDeleteFile);
            return NoContent();
        }
        else if (toDoDeleteFile != null)
        {
            toDoDeleteFile.Users.Add(user);
        }
        else
        {
            toDoDeleteFile = new ToDoDeleteFile
            {
                FilePath = viewModel.FilePath,
                Users = new List<User> { user }
            };
            _context.ToDoDeleteFiles.Add(toDoDeleteFile);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }


}
