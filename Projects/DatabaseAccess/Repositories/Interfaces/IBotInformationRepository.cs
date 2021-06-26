using DataClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories.Interfaces
{
    public interface IBotInformationRepository
    {
        Task<MinionInformationModel> GetBotInformationAsync();
    }
}
