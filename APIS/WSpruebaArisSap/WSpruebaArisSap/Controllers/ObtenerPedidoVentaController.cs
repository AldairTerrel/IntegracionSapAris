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
    public class ObtenerPedidoVentaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ObtenerPedidoVentaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("ObtenerPedidoVentaController")]
        public async Task<IActionResult> GetObtenerPedidoVenta(string FEC_CREA_INICIO, string FEC_CREA_FIN, string TIPO_DOCU, string SOCIEDAD, string NRO_PEDIDO = "")
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

                    NRO_PEDIDO = string.IsNullOrEmpty(NRO_PEDIDO) ? "" : NRO_PEDIDO;

                    var result = await context.CallFunction("ZSD_PEDIDO_VENTA",
                        Input: f => f
                                        .SetField("FEC_CREA_INICIO", DateTime.ParseExact(FEC_CREA_INICIO, "dd.MM.yyyy", null))
                                        .SetField("FEC_CREA_FIN", DateTime.ParseExact(FEC_CREA_FIN, "dd.MM.yyyy", null))
                                        .SetField("TIPO_DOCU", TIPO_DOCU)
                                        .SetField("NRO_PEDIDO", NRO_PEDIDO)
                                        .SetField("SOCIEDAD", SOCIEDAD),


                        Output: f => f
                            .MapTable("ET_CABECERA", s =>
                                 from BEZEI in s.GetField<string>("BEZEI")    // CHAR
                                 from VBELN in s.GetField<string>("VBELN")
                                 from KUNNR in s.GetField<string>("KUNNR")
                                 from KUNWE in s.GetField<DateTime>("KUNWE")
                                 from BSTNK in s.GetField<string>("BSTNK")
                                 from NETWR in s.GetField<string>("NETWR")
                                 from WAERK in s.GetField<string>("WAERK")
                                 from BSTDK in s.GetField<DateTime>("BSTDK")
                                 from VDATU in s.GetField<DateTime>("VDATU")
                                 from VSBED in s.GetField<string>("VSBED")
                                 from BRGEW in s.GetField<string>("BRGEW")
                                 from GEWEI in s.GetField<string>("GEWEI")
                                 from NTGEW in s.GetField<string>("NTGEW")
                                 from AUART in s.GetField<string>("AUART")
                                 from AUGRU in s.GetField<string>("AUGRU")
                                 from VKORG in s.GetField<string>("VKORG")
                                 from VTWEG in s.GetField<string>("VTWEG")    // CHAR
                                 from SPART in s.GetField<string>("SPART")
                                 from VKBUR in s.GetField<string>("VKBUR")
                                 from VKGRP in s.GetField<DateTime>("VKGRP")
                                 from AUDAT in s.GetField<string>("AUDAT")
                                 from ERNAM in s.GetField<string>("ERNAM")
                                 from AEDAT in s.GetField<string>("AEDAT")
                                 from ERDAT in s.GetField<DateTime>("ERDAT")
                                 from ERZET in s.GetField<string>("ERZET")

                                 from WAERK_3   in s.GetField<string>("WAERK_3")
                                 from KURSK     in s.GetField<string>("KURSK")
                                 from KALSM     in s.GetField<string>("KALSM")
                                 from KONDA     in s.GetField<string>("KONDA")
                                 from PRSDT     in s.GetField<string>("PRSDT")
                                 from KDGRP     in s.GetField<string>("KDGRP")
                                 from BZIRK     in s.GetField<string>("BZIRK")    // CHAR
                                 from BRGEW_2   in s.GetField<string>("BRGEW_2")
                                 from WAERK_2   in s.GetField<string>("WAERK_2")
                                 from KUNRG_ANA in s.GetField<DateTime>("KUNRG_ANA")
                                 from ZTERM     in s.GetField<string>("ZTERM")
                                 from FKDAT     in s.GetField<string>("FKDAT")
                                 from BUKRS_VF  in s.GetField<string>("BUKRS_VF")
                                 from AKPRZ     in s.GetField<DateTime>("AKPRZ")

                                 from DDTEXT_GBSTK in s.GetField<string>("DDTEXT_GBSTK")
                                 from DDTEXT_ABSTK in s.GetField<string>("DDTEXT_ABSTK")
                                 from DDTEXT_LFSTK in s.GetField<string>("DDTEXT_LFSTK")
                                 from DDTEXT_CMGST in s.GetField<string>("DDTEXT_CMGST")
                                 from DDTEXT_FSSTK in s.GetField<string>("DDTEXT_FSSTK")

                                 from KTGRD         in s.GetField<string>("KTGRD")
                                 from KTGRD_2       in s.GetField<string>("KTGRD_2")
                                 from IMPUESTO      in s.GetField<string>("IMPUESTO")
                                 from PARVW         in s.GetField<string>("PARVW")
                                 from KUNNR_3        in s.GetField<string>("KUNNR_3")
                                 from ASSIGNED_BP   in s.GetField<DateTime>("ASSIGNED_BP")
                                 from NAME1         in s.GetField<string>("NAME1")
                                 from STRAS             in s.GetField<string>("STRAS")
                                 from PSTLZ             in s.GetField<string>("PSTLZ")
                                 from ORT01             in s.GetField<DateTime>("ORT01")
                                 from ACTIVIDAD         in s.GetField<string>("ACTIVIDAD")
                                 from MOTIVO_RESCISION      in s.GetField<string>("MOTIVO_RESCISION")
                                 from DOCUMENTO_RESCISION   in s.GetField<string>("DOCUMENTO_RESCISION")
                  
                                 select new
                                 {
                                    BEZEI,
                                    VBELN,
                                    KUNNR,
                                    KUNWE,
                                    BSTNK,
                                    NETWR,
                                    WAERK,
                                    BSTDK,
                                    VSBED,
                                    BRGEW,
                                    GEWEI,
                                    NTGEW,
                                    AUART,
                                    AUGRU,
                                    VKORG,
                                    VTWEG,
                                    SPART,
                                    VKBUR,
                                    VKGRP,
                                    AUDAT,
                                    ERNAM,
                                    AEDAT,
                                    ERDAT,
                                    ERZET,
                                    WAERK_3,
                                    KURSK,
                                    KALSM,
                                    KONDA,
                                    PRSDT,
                                    KDGRP,
                                    BZIRK,
                                    BRGEW_2,
                                    WAERK_2,
                                    KUNRG_ANA,
                                    ZTERM,
                                    FKDAT,
                                    BUKRS_VF,
                                    AKPRZ,
                                    DDTEXT_GBSTK,
                                    DDTEXT_ABSTK,
                                    DDTEXT_LFSTK,
                                    DDTEXT_CMGST,
                                    DDTEXT_FSSTK,
                                    KTGRD,
                                    KTGRD_2,
                                    IMPUESTO,
                                    PARVW,
                                    KUNNR_3,
                                    ASSIGNED_BP,
                                    NAME1,
                                    STRAS,
                                    PSTLZ,
                                    ORT01,
                                    ACTIVIDAD,
                                    MOTIVO_RESCISION,
                                    DOCUMENTO_RESCISION
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