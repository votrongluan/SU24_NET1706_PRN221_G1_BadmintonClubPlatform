using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class MatchService : IMatchService
{
    private readonly IRepositoryManager _repo;

    public MatchService(IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public List<Match> GetAllMatches()
    {
        return _repo.Match.GetAllMatches();
    }

    public void AddMatch(Match match)
    {
        _repo.Match.AddMatch(match);
    }

    public void DeleteMatch(int matchId)
    {
        _repo.Match.DeleteMatch(matchId);
    }

    public Match GetMatchById(int matchId)
    {
        return _repo.Match.GetMatchById(matchId);
    }

    public void UpdateMatch(Match match)
    {
        _repo.Match.UpdateMatch(match);
    }
}