﻿using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using ToDoApp.Model;

namespace ToDoApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ToDoItem> ToDoItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ToDoItems = new ObservableCollection<ToDoItem>();
            lstToDoItems.ItemsSource = ToDoItems;

        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(NewItemTextBox.Text))
            {
                ToDoItems.Add(new ToDoItem { Description = NewItemTextBox.Text, IsDone = false });
                NewItemTextBox.Clear();
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button && button.DataContext is ToDoItem toDoItem)
            {
                ToDoItems.Remove(toDoItem);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if(ToDoItems.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "ToDoList";
                saveFileDialog.DefaultExt = ".json";
                saveFileDialog.Filter = "JSON documents (.json)|*.json";

                bool? result = saveFileDialog.ShowDialog();

                if(result == true)
                {
                    string filename = saveFileDialog.FileName;

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(ToDoItems, options);

                    File.WriteAllText(filename, jsonString);

                    MessageBox.Show("Tasks exported successfully.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else 
            {
                MessageBox.Show("There are no tasks to export.", "Error while exporting!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}