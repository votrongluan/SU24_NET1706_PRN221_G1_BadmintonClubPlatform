using BusinessObjects.Entities;

namespace Services.IService;

public interface IMatchService
{
    List<Match> GetAllMatches();
    void AddMatch(Match match);
    void DeleteMatch(int matchId);
    Match GetMatchById(int matchId);
    void UpdateMatch(Match match);
}