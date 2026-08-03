using CommunityToolkit.Mvvm.ComponentModel;
using EcoSystem.Client.Models;
using System.Collections.Generic;

namespace EcoSystem.Client.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Title { get; set; } = "EcoSystem Connect";

    [ObservableProperty]
    public partial List<Ecosystem> Ecosystems { get; set; } = new();
}