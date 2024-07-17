using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class ClubService : IClubService
{
    private readonly IRepositoryManager _repo;

    public ClubService(IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public bool CheckPhoneExisted(string phone)
    {
        return _repo.Club.GetAllClubs().Any(e => e.ClubPhone.Equals(phone));
    }

    public List<Club> GetAllClubs()
    {
        return _repo.Club.GetAllClubs();
    }

    public List<Club> GetAllDeActiveClubs()
    {
        return _repo.Club.GetAllDeActiveClubs();
    }

    public void AddClub(Club club)
    {
        _repo.Club.AddClub(club);
    }

    public Club GetClubByIdReal(int id)
    {
        return _repo.Club.GetClubByIdReal(id);
    }

    public void DeleteClub(int clubId)
    {
        _repo.Club.DeleteClub(clubId);
    }

    public Club GetClubById(int clubId)
    {
        return _repo.Club.GetClubById(clubId);
    }

    public Club GetDeActiveClubById(int clubId)
    {
        return _repo.Club.GetDeActiveClubById(clubId);
    }

    public void UpdateClub(Club c)
    {
        _repo.Club.UpdateClub(c);
    }


    public List<Club> GetMostRatingClubs()
    {
        return _repo.Club.GetMostRatingClubs();
    }

    public List<Club> GetMostBookingClubs()
    {
        return _repo.Club.GetMostBookingClubs();
    }

    public List<Club> GetMostPopularClubs()
    {
        return _repo.Club.GetMostPopularClubs();
    }
    public double GetAverageRatingStar (int clubId)
    {
        return _repo.Club.GetAverageRatingStar(clubId);
    }

    public ClubDto ToDto(Club entity)
    {
        return new ClubDto
        {
            ClubId = entity.ClubId,
            ClubName = entity.ClubName,
            Address = entity.Address,
            DistrictId = entity.DistrictId,
            FanpageLink = entity.FanpageLink,
            AvatarLink = entity.AvatarLink,
            OpenTime = entity.OpenTime,
            CloseTime = entity.CloseTime,
            ClubEmail = entity.ClubEmail,
            ClubPhone = entity.ClubPhone,
            ClientId = entity.ClientId,
            ApiKey = entity.ApiKey,
            ChecksumKey = entity.ChecksumKey,
            Status = entity.Status,
            TotalStar = entity.TotalStar,
            TotalReview = entity.TotalReview,
            DefaultPricePerHour = entity.DefaultPricePerHour
        };
    }

    public Club ToEntity(ClubDto dto)
    {
        return new Club
        {
            ClubName = dto.ClubName,
            Address = dto.Address,
            DistrictId = dto.DistrictId,
            FanpageLink = dto.FanpageLink,
            AvatarLink = dto.AvatarLink,
            OpenTime = dto.OpenTime,
            CloseTime = dto.CloseTime,
            ClubEmail = dto.ClubEmail,
            ClubPhone = dto.ClubPhone,
            ClientId = dto.ClientId,
            ApiKey = dto.ApiKey,
            ChecksumKey = dto.ChecksumKey,
            Status = dto.Status,
            TotalStar = dto.TotalStar,
            TotalReview = dto.TotalReview,
            DefaultPricePerHour = dto.DefaultPricePerHour
        };
    }

    public Club ToUpdateEntity(ClubDto dto)
    {
        return new Club
        {
            ClubId = dto.ClubId,
            ClubName = dto.ClubName,
            Address = dto.Address,
            DistrictId = dto.DistrictId,
            FanpageLink = dto.FanpageLink,
            AvatarLink = dto.AvatarLink,
            OpenTime = dto.OpenTime,
            CloseTime = dto.CloseTime,
            ClubEmail = dto.ClubEmail,
            ClubPhone = dto.ClubPhone,
            ClientId = dto.ClientId,
            ApiKey = dto.ApiKey,
            ChecksumKey = dto.ChecksumKey,
            Status = dto.Status,
            TotalStar = dto.TotalStar,
            TotalReview = dto.TotalReview,
            DefaultPricePerHour = dto.DefaultPricePerHour
        };
    }
}