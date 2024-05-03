namespace AllUsefulInformationSearch.StackOverflow.Models.Entities;

public class SettingEntity : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}