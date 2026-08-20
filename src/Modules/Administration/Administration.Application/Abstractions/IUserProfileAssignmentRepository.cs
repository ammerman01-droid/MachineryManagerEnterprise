using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Administration.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Administration.Domain.UserProfileAssignment"/> aggregate.</summary>
public interface IUserProfileAssignmentRepository : IRepository<global::Administration.Domain.UserProfileAssignment, global::Administration.Domain.UserProfileAssignmentId>
{
}