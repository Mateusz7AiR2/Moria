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
    /// Interaction logic for MotorListView.xaml
    /// </summary>
    public partial class MotorListView : Page
    {
        public MotorListView(MotorListViewModel listViewModel) : this()
        {
            DataContext = listViewModel;
        }
        public MotorListView()
        {
            InitializeComponent();
        }

        private void MotorDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DataGrid dg)
            {
                (DataContext as BaseListViewModel).OnRowSelected(dg.CurrentItem);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await (DataContext as MotorListViewModel).OnLoaded();

            var vm = DataContext as MotorListViewModel;
            if (vm != null && !vm.Permission_Name.CanRead)
            {
                MotorDataGrid.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }

            if (vm != null && !vm.Permission_Symbol.CanRead)
            {
                MotorDataGrid.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            }

            if (vm != null && !vm.Permission_Power.CanRead)
            {
                MotorDataGrid.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
