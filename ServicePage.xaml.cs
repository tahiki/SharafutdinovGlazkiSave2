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

namespace SharafutdinovGlazkiSave2
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        public ServicePage()
        {
            InitializeComponent();
            var currentServices = ШарафутдиновГлазкиSaveEntities.GetContext().Agent.ToList();
            ServiceListView.ItemsSource = currentServices;
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }

        private void UpdateServices()
        {
            var currentServices = ШарафутдиновГлазкиSaveEntities.GetContext().Agent.ToList();
            currentServices = currentServices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(TBoxSearch.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""))).ToList();

            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeText.ToString() == "МФО")).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeText.ToString() == "ООО")).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeText.ToString() == "ЗАО")).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeText.ToString() == "МКК")).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeText.ToString() == "ОАО")).ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeText.ToString() == "ПАО")).ToList();
            }

            if (SortType.SelectedIndex == 1)
            {
                currentServices = currentServices.OrderBy(p => p.Title).ToList();
            }
            if (SortType.SelectedIndex == 2)
            {
                currentServices = currentServices.OrderByDescending(p => p.Title).ToList();
            }
            if (SortType.SelectedIndex == 3)
            {
                currentServices = currentServices.OrderBy(p => p.SkidkaInt).ToList();
            }
            if (SortType.SelectedIndex == 4)
            {
                currentServices = currentServices.OrderByDescending(p => p.SkidkaInt).ToList();
            }
            if (SortType.SelectedIndex == 5)
            {
                currentServices = currentServices.OrderBy(p => p.Priority).ToList();
            }
            if (SortType.SelectedIndex == 6)
            {
                currentServices = currentServices.OrderByDescending(p => p.Priority).ToList();
            }

            ServiceListView.ItemsSource = currentServices.ToList();

            ServiceListView.ItemsSource = currentServices;

            TableList = currentServices;

            ChangePage(0, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void DiscountButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void SortType_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }

            Boolean Ifupdate = true;


            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();

                ServiceListView.ItemsSource = CurrentPageList;
                ServiceListView.Items.Refresh();
            }
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void ChangePriorityButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPriority = 0;
            foreach (Agent agent in ServiceListView.SelectedItems)
            {
                if (agent.Priority > maxPriority)
                    maxPriority = agent.Priority;
            }
            SetWindow myWindow = new SetWindow(maxPriority);
            myWindow.ShowDialog();

            if (string.IsNullOrEmpty(myWindow.TBPriority.Text))
            {
                MessageBox.Show("Изменений не произошло");
            }
            else
            {
                int newPriority = Convert.ToInt32(myWindow.TBPriority.Text);
                foreach (Agent agent in ServiceListView.SelectedItems)
                {
                    agent.Priority = newPriority;
                }
                try
                {
                    ШарафутдиновГлазкиSaveEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!");
                    UpdateServices();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ServiceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceListView.SelectedItems.Count > 1)
                ChangePriorityButton.Visibility = Visibility.Visible;
            else
                ChangePriorityButton.Visibility = Visibility.Hidden;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ШарафутдиновГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServiceListView.ItemsSource = ШарафутдиновГлазкиSaveEntities.GetContext().Agent.ToList();
                UpdateServices();
            }
        }
    }
}
