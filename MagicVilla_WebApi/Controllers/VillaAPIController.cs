using MagicVilla_WebApi.Data;
using MagicVilla_WebApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_WebApi.Controllers
{
    //[Route("api/VillaAPI")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        public ActionResult<VillaDTO> GetVilla(int id) 
        {
            if (id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if(villa == null)
                return NotFound();

            return Ok(villa);

        }
    }
}
