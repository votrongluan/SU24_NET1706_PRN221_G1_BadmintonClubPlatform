using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class MatchRepository : IMatchRepository
{
    public List<Match> GetAllMatches()
    {
        return MatchDao.GetAll()
            .Include(x => x.Booking).ThenInclude(x => x.Club)
            .ThenInclude(x => x.District).ThenInclude(x => x.City)
            .Include(e => e.Booking).ThenInclude(e => e.BookingDetails).ThenInclude(e => e.Slot).OrderByDescending(e => e.MatchId).ToList();
    }

    public Match GetMatchById(int matchId)
    {
        return GetAllMatches().Where(e => e.MatchId == matchId).FirstOrDefault();
    }

    public void DeleteMatch(int matchId)
    {
        var match = GetMatchById(matchId);
        MatchDao.Delete(match);
    }

    public void UpdateMatch(Match match)
    {
        MatchDao.Update(match);
    }

    public void AddMatch(Match match)
    {
        MatchDao.Add(match);
    }
}