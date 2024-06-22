using BusinessObjects.Dtos.Court;
using BusinessObjects.Entities;

namespace WebAppRazor.Mappers
{
    public static class CourtMapper
    {
        public static ResponseCourtDto ToResponseCourtDto(this Court e)
        {
            return new ResponseCourtDto()
            {
                CourtId = e.CourtId,
                CourtTypeId = e.CourtTypeId,
                ClubId = e.ClubId
            };
        }
        public static Court ToCourt(this CreateCourtDto e)
        {
            return new Court()
            {
                CourtId = e.CourtId,
                CourtTypeId = e.CourtTypeId,
                ClubId = e.ClubId
            };
        }
    }
}
