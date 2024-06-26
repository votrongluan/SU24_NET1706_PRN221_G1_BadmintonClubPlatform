using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class ClubRepository : IClubRepository
{
    public List<Club> GetAllClubs()
    {
        return ClubDao.GetAll().Include(e => e.Bookings).Include(e => e.AvailableBookingTypes).Include(e => e.District).ThenInclude(e => e.City).OrderByDescending(e => e.ClubId).Where(e => e.Status != false).ToList();
    }

    public List<Club> GetAllDeActiveClubs()
    {
        return ClubDao.GetAll().Include(e => e.Bookings).Include(e => e.AvailableBookingTypes).Include(e => e.District).ThenInclude(e => e.City).OrderByDescending(e => e.ClubId).Where(e => e.Status == false).ToList();
    }

    public Club GetClubById(int id)
    {
        return GetAllClubs().Where(e => e.ClubId == id).FirstOrDefault();
    }

    public Club GetDeActiveClubById(int id)
    {
        return ClubDao.FindByCondition(e => e.ClubId == id && e.Status == false).FirstOrDefault();
    }

    public void DeleteClub(int id)
    {
        var club = GetClubById(id);
        club.Status = false;
        ClubDao.Update(club);
    }

    public void UpdateClub(Club club)
    {
        ClubDao.Update(club);
    }

    public void AddClub(Club club)
    {
        ClubDao.Add(club);
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