namespace MonsterAuto.Client;

public class DropdownModel
{
    public string Key;
    public List<string> Values;
    public string SelectedValue;
    public List<DropdownModel> Children;

    public DropdownModel(string key, List<string> values)
    {
        Key = key;
        Values = values;
        SelectedValue = Values.First();
        Children = new List<DropdownModel>();
    }

    public DropdownModel? GetNextDropdown()
    {
        return Children.FirstOrDefault(x => string.Equals(x.Key, SelectedValue, StringComparison.OrdinalIgnoreCase));
    }
    public int CountNodes()
    {
        if (this == null)
            return 0;
        int count = 1;
        foreach (var child in this.Children)
        {
            count += child.CountNodes();
        }
        return count;
    }
    public (string, string) GetSelectedValue()
    {
        return (this.Key, this.SelectedValue);
    }
    public IDictionary<string, string> GetAllSelectedValues()
    {
        Dictionary<string, string> _allSelectedValues = new();
        if (this == null) return _allSelectedValues;
        _allSelectedValues.Add(this.GetSelectedValue().Item1, this.GetSelectedValue().Item2);
        var child = this.Children.FirstOrDefault(x => string.Equals(x.Key, SelectedValue, StringComparison.OrdinalIgnoreCase));
        if (child != null)
            _allSelectedValues = _allSelectedValues.Concat(child.GetAllSelectedValues()).ToDictionary();
        return _allSelectedValues;
    }
    public IEnumerable<DropdownModel> GetAllDropdowns()
    {
        List<DropdownModel> dropdownModels = new List<DropdownModel>();
        var nextDropdown = this.GetNextDropdown();
        if (nextDropdown != null)
        {
            dropdownModels.Add(nextDropdown);
            dropdownModels.AddRange(nextDropdown.GetAllDropdowns());
        }
        return dropdownModels;
    }
}