namespace Monaco.PathTree.Samples.Wpf.DomainModels
{
    public class Employee : IOrgResource
    {
        public string Name { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
