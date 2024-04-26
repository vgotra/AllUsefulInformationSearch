namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public class SettingEntity : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}