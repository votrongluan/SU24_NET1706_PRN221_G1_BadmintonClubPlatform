using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CourtTypeService : ICourtTypeService
    {

        private readonly IRepositoryManager _repo;

        public CourtTypeService(IRepositoryManager repo)
        {
            _repo = repo;
        }
        public List<CourtType> GetAllCourtTypes()
        {
            return _repo.CourtType.GetAllCourtTypes();
        }
    }
}
