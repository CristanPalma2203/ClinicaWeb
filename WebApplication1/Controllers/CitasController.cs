using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;


namespace WebApplication1.Controllers
{
    public class CitasController : Controller
    {
        private readonly SistemaClinicoContext _context;
        public static List<Cita> listaCitas = new List<Cita>();
        public CitasController(SistemaClinicoContext context)
        {
            _context = context;
            
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var sistemaClinicoContext = _context.Citas.Include(c => c.DuiNavigation);
            var citas = await sistemaClinicoContext.ToListAsync();

            foreach (var citaDb in citas)
            {
                Cita cita = new Cita
                {
                    NumCita = citaDb.NumCita,
                    FechaHoraCreacion = citaDb.FechaHoraCreacion,
                    FechaHoraCita = citaDb.FechaHoraCita,
                    Motivo = citaDb.Motivo,
                    CreadoPor = citaDb.CreadoPor,
                    Precio = citaDb.Precio,
                    Dui = citaDb.Dui,
                    DuiNavigation = citaDb.DuiNavigation
                };

                listaCitas.Add(cita);
            }

            return View(await sistemaClinicoContext.ToListAsync());
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.DuiNavigation)
                .FirstOrDefaultAsync(m => m.NumCita == id);
            if (cita == null)
            {
                return NotFound();
            }
            var x = View(cita);
            return x;
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            ViewData["Dui"] = new SelectList(_context.Pacientes, "Dui", "Dui");
            return View();
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumCita,FechaHoraCreacion,FechaHoraCita,Motivo,CreadoPor,Precio,Dui")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Dui"] = new SelectList(_context.Pacientes, "Dui", "Dui", cita.Dui);
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["Dui"] = new SelectList(_context.Pacientes, "Dui", "Dui", cita.Dui);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumCita,FechaHoraCreacion,FechaHoraCita,Motivo,CreadoPor,Precio,Dui")] Cita cita)
        {
            if (id != cita.NumCita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.NumCita))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Dui"] = new SelectList(_context.Pacientes, "Dui", "Dui", cita.Dui);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.DuiNavigation)
                .FirstOrDefaultAsync(m => m.NumCita == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Citas == null)
            {
                return Problem("Entity set 'SistemaClinicoContext.Citas'  is null.");
            }
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
          return (_context.Citas?.Any(e => e.NumCita == id)).GetValueOrDefault();
        }

        public IActionResult DescargarPDFCitas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio == DateTime.MinValue || fechaFin == DateTime.MinValue)
            {
                fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                int ultimoDiaMes = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, ultimoDiaMes);
            }
            List<Models.Cita> listaCitasLocal = listaCitas;
            List<Models.Cita> citasFiltradas = listaCitasLocal.Where(c => c.FechaHoraCreacion >= fechaInicio && c.FechaHoraCreacion <= fechaFin).ToList();
            double? total = 0;
            DateTime horaActual = DateTime.Now;
            string horaFormateada = horaActual.ToString("HH:mm");
            DateTime fechaActual = DateTime.Now;
            string fechaFormateada = fechaActual.ToString("dd-MM-yyyy");
            var data = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);

                    page.Header().ShowOnce().Row(row =>
                    {
                        


                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Clinica Buena Vida").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("Col. Las Margaritas Pasaje Roque Local 11-B - Soyapango, San Salvador").FontSize(9);
                            col.Item().AlignCenter().Text("6083-3018 / 2227-0978").FontSize(9);
                            col.Item().AlignCenter().Text("2508782019@mail.utec.edu.sv").FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272")
                            .AlignCenter().Text("Fecha: " + fechaFormateada);

                            col.Item().Background("#257272").Border(1)
                            .BorderColor("#257272").AlignCenter()
                            .Text("Reportes de Citas").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("Hora: " + horaFormateada);


                        });
                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {


                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn(1);
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272")
                               .Padding(2).Text("Paciente").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Hora de Cita").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Motivo").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Agendada Por").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Precio").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Fecha y Hora de Creacion").FontColor("#fff");
                            });

                            foreach (var cita in citasFiltradas)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.DuiNavigation.NombrePaciente +" "+ cita.DuiNavigation.ApellidosPaciente).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.FechaHoraCita.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.Motivo).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.CreadoPor).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text($"$.{cita.Precio}").FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.FechaHoraCreacion.ToString()).FontSize(10);

                                total += cita.Precio;
                            }

                        });

                        col1.Item().AlignRight().Text($"Total: $.{Math.Round((decimal)total,2)}").FontSize(12);

                        if (1 == 1)
                            

                        col1.Spacing(10);
                    });


                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(data);
            return File(stream, "application/pdf", $"Ciatas_{fechaActual}.pdf");

        }
    }
}
