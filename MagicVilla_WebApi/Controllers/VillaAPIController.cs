﻿using MagicVilla_WebApi.Data;
using MagicVilla_WebApi.Logging;
using MagicVilla_WebApi.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_WebApi.Controllers
{
    //[Route("api/VillaAPI")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;

        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Getting all villas", "");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        public ActionResult<VillaDTO> GetVilla(int id) 
        {
            if (id == 0)
            {
                _logger.Log("Get Villa Error with id " + id, "error");
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if(villa == null)
                return NotFound();

            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if(VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "Villa already exists!");
                return BadRequest(ModelState);
            }

            if(villaDTO == null)
                return BadRequest();

            if (villaDTO.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if(villa == null)
                return NotFound();

            VillaStore.villaList.Remove(villa);

            return NoContent();
        }

        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if(villa == null )
                return NotFound();

            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;
            
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
                return NotFound();

            patchDTO.ApplyTo(villa, ModelState);

            if(!ModelState.IsValid)
                return BadRequest();

            return NoContent();
        }

    }
}
