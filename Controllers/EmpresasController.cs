using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ControleNotasFiscais.Data;
using ControleNotasFiscais.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ControleNotasFiscais.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empresa empresa, IFormFile logo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Salvar logo
                    if (logo != null && logo.Length > 0)
                    {
                        string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        string filePath = Path.Combine(uploadDir, logo.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await logo.CopyToAsync(stream);
                        }

                        empresa.Logo = "/uploads/" + logo.FileName;
                    }

                    _context.Add(empresa);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erro ao criar a empresa: " + ex.Message);
                }
            }

            return View(empresa);
        }

        // GET: Empresas/Index
        public async Task<IActionResult> Index()
        {
            var empresas = await _context.Empresas.ToListAsync();
            return View(empresas);
        }
    }
}
