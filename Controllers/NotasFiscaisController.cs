using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleNotasFiscais.Data;
using ControleNotasFiscais.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ControleNotasFiscais.Controllers
{
    public class NotasFiscaisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotasFiscaisController> _logger;

        public NotasFiscaisController(ApplicationDbContext context, ILogger<NotasFiscaisController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: NotasFiscais/Create
        public IActionResult Create()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas, "Id", "Nome");
            return View();
        }

        // POST: NotasFiscais/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotaFiscal notaFiscal, IFormFile image)
        {
            try
            {
                // Validações personalizadas
                if (!ValidarNotaFiscal(notaFiscal, image, out var validationErrors))
                {
                    foreach (var error in validationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }

                    ViewBag.Empresas = new SelectList(_context.Empresas, "Id", "Nome", notaFiscal.EmpresaId);
                    return View(notaFiscal);
                }

                // Salvar imagem
                if (image != null && image.Length > 0)
                {
                    string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string filePath = Path.Combine(uploadDir, image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    notaFiscal.CaminhoImagem = "/uploads/" + image.FileName;
                }

                // Salvar nota fiscal no banco
                _context.Add(notaFiscal);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Nota fiscal criada com sucesso: {@NotaFiscal}", notaFiscal);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar nota fiscal");
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a nota fiscal. Por favor, tente novamente.");
            }

            ViewBag.Empresas = new SelectList(_context.Empresas, "Id", "Nome", notaFiscal.EmpresaId);
            return View(notaFiscal);
        }

        // Validação personalizada sem ModelState
        private bool ValidarNotaFiscal(NotaFiscal notaFiscal, IFormFile image, out string[] errors)
        {
            var errorList = new System.Collections.Generic.List<string>();

            if (notaFiscal.EmpresaId <= 0)
                errorList.Add("A empresa é obrigatória.");

            if (notaFiscal.Valor <= 0)
                errorList.Add("O valor deve ser positivo.");

            if (string.IsNullOrWhiteSpace(notaFiscal.TipoCompra))
                errorList.Add("O tipo de compra é obrigatório.");

            if (string.IsNullOrWhiteSpace(notaFiscal.Usuario))
                errorList.Add("O usuário é obrigatório.");

            if (image == null || image.Length == 0)
                errorList.Add("O upload da imagem é obrigatório.");

            errors = errorList.ToArray();
            return !errors.Any();
        }

        // GET: NotasFiscais/Index
        public async Task<IActionResult> Index()
        {
            var notasFiscais = await _context.NotasFiscais.Include(n => n.Empresa).ToListAsync();
            return View(notasFiscais);
        }

        // GET: NotasFiscais/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Obter total de notas fiscais
                var totalNotasFiscais = await _context.NotasFiscais.CountAsync();

                // Obter total de valor das notas fiscais
                var totalValorNotasFiscais = await _context.NotasFiscais.SumAsync(n => n.Valor);

                // Obter número de notas fiscais por tipo de compra
                var notasPorTipoCompra = await _context.NotasFiscais
                    .GroupBy(n => n.TipoCompra)
                    .Select(g => new NotaFiscalPorTipoCompra
                    {
                        TipoCompra = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                // Obter informações sobre empresas
                var empresas = await _context.Empresas.ToListAsync();

                // Criar o modelo para o dashboard
                var dashboardViewModel = new DashboardViewModel
                {
                    TotalNotasFiscais = totalNotasFiscais,
                    TotalValorNotasFiscais = totalValorNotasFiscais,
                    NotasPorTipoCompra = notasPorTipoCompra,
                    Empresas = empresas
                };

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar o dashboard");
                return StatusCode(500, "Erro interno do servidor");
            }
        }



        // Outros métodos, como Edit, Delete, etc., podem ser adicionados aqui conforme necessário
    }
}
