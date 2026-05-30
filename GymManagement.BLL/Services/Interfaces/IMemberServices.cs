using GymManagement.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberServices
    {
        public Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        public Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
    }
}
