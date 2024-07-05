using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class ClubRepository : IClubRepository
{
    public List<Club> GetAllClubs ()
    {
        return ClubDao.GetAll().Include(e => e.Bookings).Include(e => e.AvailableBookingTypes).Include(e => e.District).ThenInclude(e => e.City).OrderByDescending(e => e.ClubId).Where(e => e.Status != false).ToList();
    }

    public List<Club> GetAllDeActiveClubs ()
    {
        return ClubDao.GetAll().Include(e => e.Bookings).Include(e => e.AvailableBookingTypes).Include(e => e.District).ThenInclude(e => e.City).OrderByDescending(e => e.ClubId).Where(e => e.Status == false).ToList();
    }

    public Club GetClubById (int id)
    {
        return GetAllClubs().Where(e => e.ClubId == id).FirstOrDefault();
    }

    public Club GetDeActiveClubById (int id)
    {
        return ClubDao.FindByCondition(e => e.ClubId == id && e.Status == false).FirstOrDefault();
    }

    public void DeleteClub (int id)
    {
        var club = GetClubById(id);
        club.Status = false;
        ClubDao.Update(club);
    }

    public void UpdateClub (Club club)
    {
        ClubDao.Update(club);
    }

    public void AddClub (Club club)
    {
        ClubDao.Add(club);
    }

    public List<Club> GetMostRatingClubs ()
    {
        return ClubDao.GetAll()
            .Include(x => x.District).ThenInclude(x => x.City)
            .OrderByDescending(e => e.TotalStar).Take(4).ToList();
    }

    public List<Club> GetMostBookingClubs ()
    {
        return ClubDao.GetAll()
            .Include(x => x.District).ThenInclude(x => x.City)
            .Include(e => e.Bookings).OrderByDescending(e => e.Bookings.Count).Take(4).ToList();
    }

    public List<Club> GetMostPopularClubs ()
    {
        return ClubDao.GetAll()
            .Include(x => x.District).ThenInclude(x => x.City)
            .OrderByDescending(e => e.TotalReview).Take(4).ToList();
    }

    public double GetAverageRatingStar (int clubId)
    {
        // Get the average rating star of the club by get all star of review then divide by the number of review
        var club = GetClubById(clubId);

        double totalStar = club.TotalStar ?? 0;      // Use 0 if TotalStar is null
        double totalReview = club.TotalReview ?? 0;

        if (club.TotalReview == 0)
        {
            return 0;
        }

        return  totalStar / totalReview;
    }
}