﻿using System;
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

namespace Randomizer_WPF.Views
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();
        }

        private void UpdateRandomizedTableCount()
        {
            lblRandomizedTableCount.Text = (DataContext as kotor_Randomizer_2.Models.Kotor1Randomizer).Table2DAs.Count(rt => rt.IsRandomized).ToString("D2");
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateRandomizedTableCount();
        }

        private void LvColumns_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            (lvRandomized.ItemsSource as IList<string>).Add((string)(sender as ListViewItem).Content);
            (lvColumns.ItemsSource as IList<string>).Remove((string)(sender as ListViewItem).Content);
            UpdateRandomizedTableCount();
        }

        private void LvRandomized_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            (lvColumns.ItemsSource as IList<string>).Add((string)(sender as ListViewItem).Content);
            (lvRandomized.ItemsSource as IList<string>).Remove((string)(sender as ListViewItem).Content);
            UpdateRandomizedTableCount();
        }

        private void BtnRandomizeAll_Click(object sender, RoutedEventArgs e)
        {
            var source = lvColumns.ItemsSource as IList<string>;
            var destination = lvRandomized.ItemsSource as IList<string>;
            foreach (var item in source)
            {
                destination.Add(item);
            }
            source.Clear();
            UpdateRandomizedTableCount();
        }

        private void BtnRandomizeSelected_Click(object sender, RoutedEventArgs e)
        {
            var toMove = lvColumns.SelectedItems.OfType<string>().ToList();
            var source = lvColumns.ItemsSource as IList<string>;
            var destination = lvRandomized.ItemsSource as IList<string>;
            foreach (var item in toMove)
            {
                destination.Add(item);
                source.Remove(item);
            }
            UpdateRandomizedTableCount();
        }

        private void BtnOmitSelected_Click(object sender, RoutedEventArgs e)
        {
            var toMove = lvRandomized.SelectedItems.OfType<string>().ToList();
            var source = lvRandomized.ItemsSource as IList<string>;
            var destination = lvColumns.ItemsSource as IList<string>;
            foreach (var item in toMove)
            {
                destination.Add(item);
                source.Remove(item);
            }
            UpdateRandomizedTableCount();
        }

        private void BtnOmitAll_Click(object sender, RoutedEventArgs e)
        {
            var source = lvRandomized.ItemsSource as IList<string>;
            var destination = lvColumns.ItemsSource as IList<string>;
            foreach (var item in source)
            {
                destination.Add(item);
            }
            source.Clear();
            UpdateRandomizedTableCount();
        }
    }
}
