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
    public class CreateOrdenInversionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CreateOrdenInversionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("CreateOrdenInversionController")]
        public async Task<IActionResult> GetCreateOrdenInversion(string CO_AREA, string COMP_CODE, string ORDER_TYPE,string ORDER, string FUNC_AREA_LONG, string PROFIT_CTR,string REQU_COMP_CODE, string INVEST_PROFILE,string CURRENCY,string OBJECTCLASS="")
        {
            var settings = new Dictionary<string, string>
            {
                {"ashost", "10.45.4.163"},
                {"sysnr", "01"},
                {"client", "200"},
                {"user", "USU_INTEGRAC"},
                {"passwd","Rocio*25"},
                {"lang", "ES"}
            };

            var connectionBuilder = new ConnectionBuilder(settings);
            var connFunc = connectionBuilder.Build();

            using (var context = new RfcContext(connFunc))
            {
                try
                {
                    OBJECTCLASS = string.IsNullOrEmpty(OBJECTCLASS) ? "" : OBJECTCLASS;
                    var result = await context.CallFunction("ZCO_FM_CREATE_ORDEN_INV",
                        Input: f => f.SetStructure("IS_DAT_ORDEN", s => s
                                        .SetField("CO_AREA", CO_AREA)
                                        .SetField("COMP_CODE", COMP_CODE)
                                        .SetField("ORDER_TYPE", ORDER_TYPE)
                                        .SetField("ORDER",ORDER)
                                        .SetField("FUNC_AREA_LONG", FUNC_AREA_LONG)
                                        .SetField("OBJECTCLASS", OBJECTCLASS)
                                        .SetField("PROFIT_CTR", PROFIT_CTR)
                                        .SetField("REQU_COMP_CODE", REQU_COMP_CODE)
                                        .SetField("INVEST_PROFILE", INVEST_PROFILE)
                                        .SetField("CURRENCY", CURRENCY)),
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