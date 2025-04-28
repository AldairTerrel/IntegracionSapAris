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
    public class ConsumirEncasetamientoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConsumirEncasetamientoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("ConsumirEncasetamientoController")]
        public async Task<IActionResult> GetUpdateOrdenInversion(string BUDAT, string BLDAT, string UARIS_CREA, string UARIS_MOD)
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



                    var result = await context.CallFunction("ZPP_FM_NOTIF_CONS_ORDEN_INV",
                        Input: f => f.SetStructure("ES_CAB_NOT_CONS_OI", s =>s
                        .SetField("BUDAT", DateTime.ParseExact(BUDAT, "dd.MM.yyyy", null))
                         .SetField("BLDAT", DateTime.ParseExact(BLDAT, "dd.MM.yyyy", null))
                          .SetField("UARIS_CREA", UARIS_CREA)
                           .SetField("UARIS_MOD", UARIS_MOD)),



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