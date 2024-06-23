using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IMatchRepository
{
    List<Match> GetAllMatches();
    Match GetMatchById(int matchId);
    void DeleteMatch(int matchId);
    void UpdateMatch(Match match);
    void AddMatch(Match match);
}