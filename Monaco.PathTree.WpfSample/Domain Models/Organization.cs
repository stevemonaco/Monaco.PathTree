using System.Collections.Generic;

namespace Monaco.PathTree.Samples.Wpf.DomainModels;

public class Organization : IOrgResource
{
    public string Name { get; set; } = string.Empty;
    public List<Department> Departments { get; set; } = new();
}
