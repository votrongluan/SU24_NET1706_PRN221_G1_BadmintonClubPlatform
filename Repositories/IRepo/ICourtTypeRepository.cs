using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface ICourtTypeRepository
{
    List<CourtType> GetAllCourtTypes();
}