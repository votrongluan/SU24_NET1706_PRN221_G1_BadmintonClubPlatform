using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class MatchRepository : IMatchRepository
{
    public List<Match> GetAllMatches()
    {
        return MatchDao.GetAll().ToList();
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