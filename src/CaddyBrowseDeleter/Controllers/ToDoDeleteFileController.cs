using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using CaddyBrowseDeleter.ViewModel;
using Coravel.Scheduling.Schedule.Interfaces;

namespace CaddyBrowseDeleter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoDeleteFileController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly DeleteFilesJob _deleteFileJob;

    public ToDoDeleteFileController(AppDbContext context, DeleteFilesJob deleteFileJob)
    {
        _context = context;
        _deleteFileJob = deleteFileJob;

    }

    // GET: api/ToDoDeleteFile
    [HttpGet()]
    public async Task<ActionResult<List<ToDoDeleteFile>>> GetToDoDeleteFiles(string? path)
    {
        var allUsers = await _context.Users.ToListAsync();
        var filesQuery =  _context.ToDoDeleteFiles
            .Include(x => x.Users);
        if (path != null)
        {
            var filteredFiles = await filesQuery
                .Where(x => EF.Functions.Like(x.FilePath, $"{path}%")).ToListAsync();
            // 檢查檔案是否被所有使用者確認，確認修改 IsReadyToDelete
            foreach (var file in filteredFiles)
            {
                file.IsReadyToDelete = allUsers.All(x => file.Users.Contains(x));
            }
            return filteredFiles;
        }
        var files = await filesQuery.ToListAsync();
        foreach (var file in files)
        {
            file.IsReadyToDelete = allUsers.All(x => file.Users.Contains(x));
        }

        return files;
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
        var toDoDeleteFile = await _context.ToDoDeleteFiles
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.FilePath == viewModel.FilePath);
        // 有檔案代表要取消刪除要求
        if (toDoDeleteFile != null && toDoDeleteFile.Users.Contains(user))
        {
            toDoDeleteFile.Users.Remove(user);
        }
        else if (toDoDeleteFile != null)
        {
            toDoDeleteFile.Users.Add(user);
        }
        else
        {
            var dirpath = Path.GetDirectoryName(viewModel.FilePath)?.Replace("\\", "/");
            if (dirpath == null)
            {
                return BadRequest("路徑錯誤");
            }
            toDoDeleteFile = new ToDoDeleteFile
            {
                FilePath = viewModel.FilePath,
                DirPath = dirpath,
                Users = new List<User> { user }
            };
            _context.ToDoDeleteFiles.Add(toDoDeleteFile);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("execute")]
    public async Task<IActionResult> RunDeleteFilesJob()
    {
        await _deleteFileJob.Invoke();
        return Ok("排程已執行");
    }
}
