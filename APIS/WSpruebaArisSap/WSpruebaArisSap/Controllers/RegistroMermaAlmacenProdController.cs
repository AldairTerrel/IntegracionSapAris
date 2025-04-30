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
using LanguageExt;

namespace WSpruebaArisSap.Controllers
{
    [ApiController]
    [Route("api/")]
    public class RegistroMermaAlmacenProdController : ControllerBase
    {
        public class NotConsOiItem
        {
            public string MATNR { get; set; }
            public string WERKS { get; set; }
            public string LGORT { get; set; }
            public string CHARG { get; set; }
            public string ENTRY_QNT { get; set; }
            public string COST_CENTER { get; set; }
        }

        private readonly IConfiguration _configuration;

        public RegistroMermaAlmacenProdController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("RegistroMermaAlmacenProdController")]
        public async Task<IActionResult> GetRegistroMermaAlmacenProd(string I_MIGO_PED_IMP="",string I_MIGO_MERMAS ="", string BUDAT ="", string BLDAT = "",
            string EBELN ="", string UARIS_CREA = "", string UARIS_MOD = "", string MATNR = "", string WERKS = "", string LGORT = "", string CHARG = "", string ENTRY_QNT = "", string COST_CENTER = "")
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

                    I_MIGO_PED_IMP = string.IsNullOrEmpty(I_MIGO_PED_IMP) ? "" : I_MIGO_PED_IMP;
                    I_MIGO_MERMAS = string.IsNullOrEmpty(I_MIGO_MERMAS) ? "" : I_MIGO_MERMAS;
                    BUDAT = string.IsNullOrEmpty(BUDAT) ? "" : BUDAT;
                    BLDAT = string.IsNullOrEmpty(BLDAT) ? "" : BLDAT;
                    UARIS_CREA = string.IsNullOrEmpty(UARIS_CREA) ? "" : UARIS_CREA;
                    UARIS_MOD = string.IsNullOrEmpty(UARIS_MOD) ? "" : UARIS_MOD;


                    var items = new List<NotConsOiItem>
{
                            new NotConsOiItem
                            {
                                MATNR = MATNR,
                                WERKS = WERKS,
                                LGORT = LGORT,
                                CHARG = CHARG,
                                ENTRY_QNT = ENTRY_QNT,
                                COST_CENTER = COST_CENTER,
                            }
                            
                    };

                    var result = await context.CallFunction("ZMM_FM_GEN_MIGO_PED",
                        Input: f => f
                        .SetField("I_MIGO_PED_IMP", I_MIGO_PED_IMP)
                         .SetField("I_MIGO_MERMAS", I_MIGO_MERMAS)
                        .SetStructure("ES_CABECERA_MIGO", s =>s
                        .SetField("BUDAT", DateTime.ParseExact(BUDAT, "dd.MM.yyyy", null))
                        .SetField("BLDAT", DateTime.ParseExact(BLDAT, "dd.MM.yyyy", null))
                        .SetField("EBELN", EBELN)
                        .SetField("UARIS_CREA", UARIS_CREA)
                        .SetField("UARIS_MOD", UARIS_MOD))
                          .SetTable("T_DETALLE_MIGO", items,
                                         (structure, items) => structure
                                                 .SetField("MATNR", items.MATNR)
                                                 .SetField("WERKS", items.WERKS)
                                                 .SetField("LGORT", items.LGORT)
                                                 .SetField("CHARG", items.CHARG)
                                                 .SetField("ENTRY_QNT", items.ENTRY_QNT)
                                                 .SetField("COST_CENTER", items.COST_CENTER)
                        ),


                        Output: f => (
                           from T_DETALLE_MIGO in f.MapTable("T_DETALLE_MIGO", s =>
                        from MATNR in s.GetField<string>("MATNR")
                        from WERKS in s.GetField<string>("WERKS")
                        from LGORT in s.GetField<string>("LGORT")
                        from CHARG in s.GetField<string>("CHARG")
                        from ENTRY_QNT in s.GetField<string>("ENTRY_QNT")
                        from COST_CENTER in s.GetField<string>("COST_CENTER")

                        select new
                        {

                            MATNR,
                            WERKS,
                            LGORT,
                            CHARG,
                            ENTRY_QNT,
                            COST_CENTER

                        })

                           from T_RETURN in f.MapTable("T_RETURN", s =>
                               from TYPE in s.GetField<decimal>("TYPE")
                               from ID in s.GetField<string>("ID")
                               from NUMBER in s.GetField<string>("NUMBER")
                               from MESSAGE in s.GetField<string>("MESSAGE")
                               from LOG_NO in s.GetField<string>("LOG_NO")
                               from LOG_MSG_NO in s.GetField<decimal>("LOG_MSG_NO")
                               from MESSAGE_V1 in s.GetField<string>("MESSAGE_V1")
                               from MESSAGE_V2 in s.GetField<string>("MESSAGE_V2")
                               from MESSAGE_V3 in s.GetField<string>("MESSAGE_V3")
                               from MESSAGE_V4 in s.GetField<string>("MESSAGE_V4")
                               from PARAMETER in s.GetField<string>("PARAMETER")
                               from ROW in s.GetField<string>("ROW")
                               from FIELD in s.GetField<string>("FIELD")
                               from SYSTEM in s.GetField<string>("SYSTEM")




                               select new
                               {
                                   TYPE,
                                   ID,
                                   NUMBER,
                                   MESSAGE,
                                   LOG_NO,
                                   LOG_MSG_NO,
                                   MESSAGE_V1,
                                   MESSAGE_V2,
                                   MESSAGE_V3,
                                   MESSAGE_V4,
                                   PARAMETER,
                                   ROW,
                                   FIELD,
                                   SYSTEM
                               })

                           

                           select new
                           {

                               T_DETALLE_MIGO,
                               T_RETURN
                           })
            );

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