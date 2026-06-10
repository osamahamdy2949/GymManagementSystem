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
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<bool> CreateMembreAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<MemberDetailsViewModel?> GetMemberByIdAsync(int id, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordByMemberIdAsync(int memberId, CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default);
        Task<UpdateMemberViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default);
        Task<bool> UpdateMemberAsync(int id, UpdateMemberViewModel model, CancellationToken ct = default);
    }
}
