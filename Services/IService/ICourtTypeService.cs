using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ICourtTypeService
    {
        List<CourtType> GetAllCourtTypes();
    }
}
