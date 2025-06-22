using Microsoft.AspNetCore.Mvc;
using DocVault.API.Data;
using DocVault.API.Models;

namespace DocVault.API.Controllers;

[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public DocumentsController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads");
        Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var doc = new Document
        {
            Name = file.FileName,
            FilePath = filePath
        };

        _context.Documents.Add(doc);
        await _context.SaveChangesAsync();

        return Ok(doc);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Documents.ToList());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Downoad(int id)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null) return NotFound();

        var bytes = await System.IO.File.ReadAllBytesAsync(doc.FilePath);
        return File(bytes, " application/octet-stream", doc.Name);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null) return NotFound();

        if (System.IO.File.Exists(doc.FilePath))
            System.IO.File.Delete(doc.FilePath);

        _context.Documents.Remove(doc);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}