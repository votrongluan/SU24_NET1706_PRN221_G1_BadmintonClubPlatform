using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;

namespace WebAppRazor.Mappers;

public static class ClubMapper
{
    public static ResponseClubDto ToResponseClubDto(this Club e)
    {
        return new ResponseClubDto()
        {
            Address = $"{e.Address}, {e.District.DistrictName}, {e.District.City.CityName}",
            ClubId = e.ClubId,
            ClubName = e.ClubName,
            ClubPhone = e.ClubPhone,
        };
    }

    public static Club ToClub(this CreateClubDto e)
    {
        return new Club()
        {
            ClubName = e.ClubName,
            ClubPhone = e.ClubPhone,
            DistrictId = e.DistrictId,
            Address = e.Address,
            TotalReview = 0,
            TotalStar = 0,
        };
    }
}