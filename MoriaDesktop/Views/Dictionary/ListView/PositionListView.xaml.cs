﻿using MoriaDesktop.ViewModels.Base;
using MoriaDesktop.ViewModels.Dictionary.ListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoriaDesktop.Views.Dictionary.ListView
{
    /// <summary>
    /// Interaction logic for PositionListView.xaml
    /// </summary>
    public partial class PositionListView : Page
    {
        public PositionListView(PositionListViewModel listViewModel) : this()
        {
            DataContext = listViewModel;
        }
        public PositionListView()
        {
            InitializeComponent();
        }

        private void PositionDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DataGrid dg)
            {
                (DataContext as BaseListViewModel).OnRowSelected(dg.CurrentItem);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await (DataContext as PositionListViewModel).OnLoaded();

            var vm = DataContext as PositionListViewModel;
            if (vm != null && !vm.Permission_Name.CanRead)
            {
                PositionDataGrid.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }

            if (vm != null && !vm.Permission_Code.CanRead)
            {
                PositionDataGrid.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
