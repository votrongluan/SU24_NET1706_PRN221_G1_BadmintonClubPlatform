using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class ClubRepository : IClubRepository
{
    public List<Club> GetClubs()
    {
        return ClubDao.GetAll().ToList();
    }

    public List<Club> GetMostRatingClubs()
    {
        return ClubDao.GetAll().OrderByDescending(e => e.TotalStar).Take(4).ToList();
    }

    public List<Club> GetMostBookingClubs()
    {
        return ClubDao.GetAll().Include(e => e.Bookings).OrderByDescending(e => e.Bookings.Count).Take(4).ToList();
    }

    public List<Club> GetMostPopularClubs()
    {
        return ClubDao.GetAll().OrderByDescending(e => e.TotalReview).Take(4).ToList();
    }
}