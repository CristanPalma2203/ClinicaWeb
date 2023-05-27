using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using static QuestPDF.Helpers.Colors;

namespace WebApplication1.Controllers
{
    public class PacientesController : Controller
    {
        private readonly SistemaClinicoContext _context;
        public static List<Paciente> listaPaciente = new List<Paciente>();
        public static List<Expediente> listaExpedientes= new List<Expediente>();
        public PacientesController(SistemaClinicoContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var sistemaClinicoContext = _context.Pacientes;
            var pacientes = await sistemaClinicoContext.ToListAsync();
            foreach (var pacienteDb in pacientes)
            {
                Paciente paciente = new Paciente
                {
                    Dui = pacienteDb.Dui,
                    NombrePaciente = pacienteDb.NombrePaciente,
                    ApellidosPaciente = pacienteDb.ApellidosPaciente,
                    SexoPaciente = pacienteDb.SexoPaciente,
                    TelefonoPaciente = pacienteDb.TelefonoPaciente,
                    DireccionPaciente = pacienteDb.DireccionPaciente,
                    EstadoCivil = pacienteDb.EstadoCivil,
                    FechaNacimiento = pacienteDb.FechaNacimiento,
                    Cita = pacienteDb.Cita,
                    Expedientes = pacienteDb.Expedientes
                };

                listaPaciente.Add(paciente);
            }

            var sistemaClinicoContext2 = _context.Expedientes.Include(c => c.Diagnosticos);
            var expedientes = await sistemaClinicoContext2.ToListAsync();
            foreach (var expedienteDb in expedientes)
            {
                Expediente expediente = new Expediente
                {
                    NumExpediente = expedienteDb.NumExpediente,
                    AntecedentesClinicos = expedienteDb.AntecedentesClinicos,
                    MedicamentosEscritos = expedienteDb.MedicamentosEscritos,
                    TipoSangre = expedienteDb.TipoSangre,
                    Dui = expedienteDb.Dui,
                    DuiNavigation = expedienteDb.DuiNavigation,
                    Diagnosticos = expedienteDb.Diagnosticos,
                    Movimientos = expedienteDb.Movimientos
                };
                listaExpedientes.Add(expediente);
            }

            return View(await sistemaClinicoContext.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Dui == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dui,NombrePaciente,ApellidosPaciente,SexoPaciente,TelefonoPaciente,DireccionPaciente,EstadoCivil,FechaNacimiento")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Dui,NombrePaciente,ApellidosPaciente,SexoPaciente,TelefonoPaciente,DireccionPaciente,EstadoCivil,FechaNacimiento")] Paciente paciente)
        {
            if (id != paciente.Dui)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Dui))
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
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Dui == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pacientes == null)
            {
                return Problem("Entity set 'SistemaClinicoContext.Pacientes'  is null.");
            }
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
          return (_context.Pacientes?.Any(e => e.Dui == id)).GetValueOrDefault();
        }
        public IActionResult DescargarPDFPacientes()
        {
            
            List<Models.Paciente> listaCitasLocal = listaPaciente;
            
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
                               .Padding(2).Text("Nombre").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Apellido").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Sexo").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Telefono").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Direecion").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Fecha de Nacimiento").FontColor("#fff");
                            });

                            foreach (var cita in listaCitasLocal)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.NombrePaciente).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.ApellidosPaciente).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.SexoPaciente).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.TelefonoPaciente.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text($"{cita.DireccionPaciente}").FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.FechaNacimiento?.ToString("yyyy-MM-dd")).FontSize(10);

                             
                            }

                        });


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
            return File(stream, "application/pdf", $"Paciente_{fechaActual}.pdf");

        }
        public IActionResult DescargarPDFExpediente(int id)
        {

            Models.Expediente expediente= listaExpedientes.Where(e => e.Dui == id).ToList().FirstOrDefault();
            


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
                            .Text("Expediente #"+ expediente.NumExpediente.ToString()).FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("Hora: " + horaFormateada);


                        });
                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {

                        col1.Item().Column(col2 =>
                        {
                            col2.Item().Text("Datos del Paciente").Underline().Bold();

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Nombre: ").SemiBold().FontSize(10);
                                txt.Span(expediente.DuiNavigation.NombrePaciente+" "+ expediente.DuiNavigation.ApellidosPaciente).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("DUI: ").SemiBold().FontSize(10);
                                txt.Span(expediente.Dui.ToString()).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Tipo Sangre: ").SemiBold().FontSize(10);
                                txt.Span(expediente.TipoSangre).FontSize(10);
                            });

                        });
                       

                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272")
                               .Padding(2).Text("Estado").FontColor("#fff").FontSize(9);

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Enfermedad").FontColor("#fff").FontSize(9);

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Peso").FontColor("#fff").FontSize(9);

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Estatura").FontColor("#fff").FontSize(9);

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Presion").FontColor("#fff").FontSize(9);

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Temperatura").FontColor("#fff").FontSize(9);

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Detalles").FontColor("#fff").FontSize(9);

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Fecha").FontColor("#fff").FontSize(9);
                            });

                            if (expediente.Diagnosticos.Count == 0) {
                                col1.Spacing(2);
                                col1.Item().AlignCenter().Text("No Hay Diagnosticos para este este paciente").FontSize(12); ;
                                col1.Spacing(2);
                            }
                            foreach (var cita in expediente.Diagnosticos)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.Estado).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.Enfermedad).FontSize(8);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.Peso.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.Estatura.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text($"$.{cita.Presion}").FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cita.Temperatura.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(cita.Detalles).FontSize(8);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(cita.FechaDiagnostico.ToString()).FontSize(10);

                            }

                        });

                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                            {
                               column.Item().Text("Medicamentos Pre Escritos: ").FontSize(10);
                               column.Item().Text(expediente.MedicamentosEscritos);
                               column.Spacing(5);
                           });

                        col1.Spacing(2);

                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Antecedentes Clinicos: ").FontSize(10);
                            column.Item().Text(expediente.AntecedentesClinicos);
                            column.Spacing(5);
                        });

                        col1.Spacing(2);
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
            return File(stream, "application/pdf", $"Paciente_{fechaActual}.pdf");

        }


    }
}
