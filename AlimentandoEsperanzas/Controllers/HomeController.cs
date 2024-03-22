using AlimentandoEsperanzas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.sqlClient;
using System.Data;
using System.Text;
using ClosedXML.Excel;

namespace AlimentandoEsperanzas.Controllers
{
    public class HomeController : Controller
    {
        private readonly AlimentandoesperanzasContext _dbcontext;

        public HomeController(AlimentandoesperanzasContext context)
        {
            _dbcontext = context;
        }


        public IActionResult resumenDonation()
        {
            DataTime FechaInicio = DataTime.Now;
            fechaInicio = FechaInicio.AddDays(-5);

            List<Donation> Lista = (from tbdonation in _dbcontext.Donations where tbdonation.DateTime Date.Value.Date>= FechaInicio.Date group tbdonation by tbdonation.DateTime Date .Value.Date into grupo select new Donation
            {
                DateTime Date  = grupo,.Key.ToString("dd/MM/yyyy"),
                Amount = grupo.Coount(),
            }).ToList();

            return StatusCode(StatusCodes.Status2000K, Lista)
        }
        public IActionResult resumenDonor()
        {
            

            List<Donor> Lista = (from tbdonor in _dbcontext.Donor
                                    where tbdonor.DateTime Date.Value.Date >= FechaInicio.Date
                                    group tbdonor by tbdonor.Name into grupo orderby grupo.count() descending
                                    select new Donor
                                    {
                                        Name = grupo,.Key,
                                        cantidad = grupo.Coount(),
                                    }).ToList();

            return StatusCode(StatusCodes.Status2000K, Lista)
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public fileResult exportar(string Donor,string fechainicio,stringfechafinal)
        {
            DataTable dt = new DataTable();

            using (sqlConnection cn = new sqlConnection("Data Source=.; Initial Catalog = alimenrandoesperanzas;Integrated Security=True"))
            {
                StringBuilder sb new StringBuilder;

                sb.AppendLine("SET DATAFORMAT dmy;");
                sb.AppendLine("select * from[dbo].[Donation] where DonorID = iff(@Donor= 0,DonorID,@Donor) and convert(date,OrderDate) between @fechainicio and @fechafinal");

                SqlCommand cmd = sqlCommand(sb.ToString(), cn);
                cmd.Parameters.AddwithValue("@Donor", Donor);
                cmd.Parameters.AddwithValue("fechainicio", fechainicio);
                cmd.Parameters.AddwithValue("fechafinal", fechafinal);
                cmd.commandType = CommandType.Text;
                cn.Open();

               using(SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            dt.TableName = "datos";

            using(XLWorkbook libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.add(dt);

                hoja.ColumnsUsed().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    libro.SaveAs(stream);

                    return file(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report" + DataTime.Now.ToString() + ".xlsx");
                }

            }

        }

        public JsonResult obtenerDonors() { 

            list<Donor> listadonor new list<Donor>();

            using (SqlConnection cn = new SqlConnection("Data Source=.; Initial Catalog = alimenrandoesperanzas;Integrated Security=True")){

                SqlCommand cmd = SqlCommand("select DonorID,concat(Name,' ',LastName)[Nombres]from [dbo].[Donor]", cn);
                cmd.commandType = CommandType.Text;
                cn.Open();
                using(sqlDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        listadonor.add(new Donor(){
                            DonorID = Convert.ToInt32(dr["DonorID"]),
                            Nombres = dr{ "Nombres"}.ToString()
                        });
                    }
                }
                 
            }



            return Json(listadonor, JsonRequestBehavior.AllowGet);
        
        
        }

    }
}
