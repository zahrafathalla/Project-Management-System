using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.ProjectSpecifications
{
    public class CountProjectWithSpec :BaseSpecification<Project>
    {
        public CountProjectWithSpec(SpecParams specParams)
            :base(p => !p.IsDeleted)
        {
            
        }
    }
}
