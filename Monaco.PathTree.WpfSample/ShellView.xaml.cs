using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;

namespace Monaco.PathTree.Samples.Wpf;

/// <summary>
/// Interaction logic for ShellView.xaml
/// </summary>
public partial class ShellView : Window
{
    public ShellView()
    {
        DataContext = Ioc.Default.GetService<ShellViewModel>();
        InitializeComponent();
    }
}
