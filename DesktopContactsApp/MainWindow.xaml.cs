using DesktopContactsApp.Classes;
using SQLite;
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

namespace DesktopContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Contact> contacts;
        public MainWindow()
        {
            InitializeComponent();

            contacts = new List<Contact>();

            ReadDatabase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog(); // new window must be closed before main window can be accessed

            ReadDatabase();
        }

        private void ReadDatabase()
        {
            
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Contact>();
                contacts = (connection.Table<Contact>().ToList()).OrderBy(c => c.Name).ToList();

                //define a sample Linq syntax
                //var variable = from c2 in contacts
                //               orderby c2.Name
                //               select c2;
            }

            if (contacts != null)
            {
                //the following codes are not the correct ones
                //foreach(var contact in contacts)
                //{
                //    contactListView.Items.Add(new ListViewItem()
                //    {
                //        Content = contact
                //    });
                //}

                contactListView.ItemsSource = contacts;
            }
        }

        /// <summary>
        /// search throgh contacts with search term input by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;

            var filteredList = contacts.Where(c => c.Name.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();

            // define Linq syntax that performs the same search function as filteredList
            var filteredList2 = (from c2 in contacts
                                where c2.Name.ToLower().Contains(searchTextBox.Text.ToLower())
                                orderby c2.Email
                                select c2).ToList();

            contactListView.ItemsSource = filteredList;
        }

        private void ContactListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectedContact = (Contact)contactListView.SelectedItem;

            if (selectedContact != null)
            {
                ContactDetailWindow contactDetailWindow = new ContactDetailWindow(selectedContact);
                contactDetailWindow.ShowDialog();
            }
        }
    }
}
