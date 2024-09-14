namespace ProjectManagementSystem.Data.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
