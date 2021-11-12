using System.Collections.Generic;

namespace Monaco.PathTree.Samples.Wpf.DomainModels;

public class Department : IOrgResource
{
    public string Name { get; set; } = string.Empty;
    public List<Department> Departments { get; set; } = new();
    public Employee? Manager { get; set; }
    public List<Employee> Staff { get; set; } = new();
}
