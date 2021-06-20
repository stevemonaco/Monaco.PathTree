using System.Threading.Tasks;
using System.Text.Json;
using Monaco.PathTree.Samples.Wpf.DomainModels;
using System.IO;
using Monaco.PathTree.Samples.Wpf.Tree;
using System;

namespace Monaco.PathTree.Samples.Wpf.Services
{
    public interface IOrganizationService
    {
        Task<Organization?> DeserializeFromFile(string organizationFileName);
        Task<OrganizationTree> DeserializeTreeFromFile(string organizationFileName);
    }

    public class OrganizationService : IOrganizationService
    {
        public async Task<Organization?> DeserializeFromFile(string organizationFileName)
        {
            var contents = await File.ReadAllTextAsync(organizationFileName);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<Organization>(contents, options);
        }

        public async Task<OrganizationTree> DeserializeTreeFromFile(string organizationFileName)
        {
            throw new NotImplementedException();

            //var contents = await File.ReadAllTextAsync(organizationFileName);
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true
            //};

            //var org = JsonSerializer.Deserialize<Organization>(contents, options);

            //if (org is null)
            //    throw new JsonException();

            //// Map to PathTree
            //var root = new OrganizationNode(org.Name, org);
            //var tree = new OrganizationTree(root);

            //var departmentVisitor =
        }
    }
}
