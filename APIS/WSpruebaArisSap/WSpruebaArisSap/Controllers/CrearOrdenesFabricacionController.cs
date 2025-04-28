using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dbosoft.YaNco;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dbosoft.YaNco.TypeMapping;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WSpruebaArisSap.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CrearOrdenesFabricacionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CrearOrdenesFabricacionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("CrearOrdenesFabricacionController")]
        public async Task<IActionResult> GetUpdateOrdenInversion(string MATNR, string WERKS, string AUFART , decimal GAMNG , string START_DATE , string END_DATE,string DISPO,string FEVOR,string INSMK,string LGORT,string CHARG)
        {
            var settings = new Dictionary<string, string>
            {
                {"ashost", "10.45.4.163"},
                {"sysnr", "01"},
                {"client", "200"},
                {"user", "USU_INTEGRAC"},
                {"passwd","Rocio*25"},
                {"lang", "EN"}
            };

            var connectionBuilder = new ConnectionBuilder(settings);
            var connFunc = connectionBuilder.Build();

            using (var context = new RfcContext(connFunc))
            {
                try
                {

             
                    var result = await context.CallFunction("ZPP_FM_CREATE_ORDEN_FAB",
                        Input: f => f.SetStructure("IS_DAT_ORDEN_FAB", s => s
                                        .SetField("MATNR", MATNR)
                                        .SetField("WERKS", WERKS)
                                        .SetField("AUFART", AUFART)
                                        .SetField("GAMNG", GAMNG)
                                        .SetField("START_DATE", DateTime.ParseExact(START_DATE, "dd.MM.yyyy", null))
                                        .SetField("END_DATE", DateTime.ParseExact(END_DATE, "dd.MM.yyyy", null))
                                        .SetField("DISPO", DISPO)
                                        .SetField("FEVOR", FEVOR)
                                        .SetField("INSMK", INSMK)
                                        .SetField("LGORT", LGORT)
                                        .SetField("CHARG", CHARG)), 

                        Output: f => f
                            .MapTable("T_RETURN", s =>
                                 from TYPE in s.GetField<string>("TYPE")    // CHAR
                                 from ID in s.GetField<string>("ID")    // CHAR
                                 from NUMBER in s.GetField<string>("NUMBER")    // CHAR
                                 from MESSAGE in s.GetField<string>("MESSAGE")
                                 select new
                                 {
                                     TYPE,
                                     ID,
                                     NUMBER,
                                     MESSAGE
                                 }));

                    return Ok(new
                    {
                        Data = result.Case
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message });
                }
            }
        }
    }
}