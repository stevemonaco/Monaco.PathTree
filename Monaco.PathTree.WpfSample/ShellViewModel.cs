using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Monaco.PathTree.Samples.Wpf.DomainModels;
using Monaco.PathTree.Samples.Wpf.Tree;
using Monaco.PathTree.Samples.Wpf.Services;

namespace Monaco.PathTree.Samples.Wpf;

public class ShellViewModel : ObservableObject
{
    private ObservableCollection<Organization> _organizations = new();
    public ObservableCollection<Organization> Organizations
    {
        get => _organizations;
        set => SetProperty(ref _organizations, value);
    }

    private string _status = string.Empty;
    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private object? _selectedNode;
    public object? SelectedNode
    {
        get => _selectedNode;
        set
        {
            SetProperty(ref _selectedNode, value);
            //if (SetProperty(ref _selectedNode, value))
            //    SelectedPathKey = SelectedNode?.Name ?? string.Empty;
        }
    }

    private string _selectedPathKey = string.Empty;
    public string SelectedPathKey
    {
        get => _selectedPathKey;
        set => SetProperty(ref _selectedPathKey, value);
    }

    public IAsyncRelayCommand LoadOrganizationCommand { get; }

    private readonly IOrganizationService _organizationService;
    private readonly string _organizationFileName = $"Assets/organization.json";

    public ShellViewModel(IOrganizationService organizationService)
    {
        _organizationService = organizationService;

        LoadOrganizationCommand = new AsyncRelayCommand(LoadOrganization);
    }

    public async Task LoadOrganization()
    {
        Status = "Initializing...";
        var org = await _organizationService.DeserializeFromFile(_organizationFileName);

        if (org is object)
        {
            Organizations.Add(org);
            await Task.Delay(1000);
            Status = $"Loaded '{Path.GetFileName(_organizationFileName)}'";
            Status = "Ready";
        }
        else
        {
            Status = $"Failed to load '{Path.GetFileName(_organizationFileName)}'";
        }
    }
}
