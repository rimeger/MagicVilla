using MagicVilla_WebApi.Models.Dto;

namespace MagicVilla_WebApi.Data
{
    public class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO {Id = 1, Name = "Pool View"},
                new VillaDTO {Id = 2, Name = "Beach View"}
            };
    }
}
