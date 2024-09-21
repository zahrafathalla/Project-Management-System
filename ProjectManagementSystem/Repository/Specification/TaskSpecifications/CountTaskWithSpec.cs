using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.TaskSpecifications;

public class CountTaskWithSpec : BaseSpecification<WorkTask>
{
    public CountTaskWithSpec(SpecParams specParams)
        : base(p => !p.IsDeleted)
    {

    }
}