﻿

using Microsoft.Extensions.Logging;
using MoriaDesktop.ViewModels.Base;

namespace MoriaDesktop.ViewModels.Dictionary;
public class SteelKindViewModel : ViewModelBase
{
    public SteelKindViewModel(ILogger<ViewModelBase> logger) : base(logger)
    {
    }

    #region properties

    string _Symbol;
    public string Symbol
    {
        get => _Symbol;
        set
        {
            _Symbol = value;
            RaisePropertyChanged(nameof(Symbol));
        }
    }

    string _Name;
    public string Name
    {
        get => _Name;
        set
        {
            _Name = value;
            RaisePropertyChanged(nameof(Name));
        }
    }

    #endregion
}
