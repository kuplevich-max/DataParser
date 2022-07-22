using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;

namespace DataParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Item> items;
        public MainWindow()
        {
            InitializeComponent();
            itemReader = new ItemReader("data/data.xml");
        }

        private ItemReader itemReader;
       async private void ModelReadButtonClick(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            { 
                try { items = itemReader.ReadItemsModel(); }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } 
            }
            );
            MessageBox.Show("Done!");
        }

        async private void RegularReadButtonClick (object sender, RoutedEventArgs e)
        {         
            await Task.Run(() =>
                {
                    try { items = itemReader.ReadItemsRegular(); }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            );
            MessageBox.Show("Done!");
            
        }
        async private void WriteButtonClick(object sender, RoutedEventArgs e)
        {
            if(textBox.Text == "")
            {
                MessageBox.Show("Введите имя файла");
                return;
            }
            if (comboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип файла");
                    return;
            }            
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            ItemWriter itemWriter = new ItemWriter(textBox.Text + selectedItem.Content.ToString());

            if (selectedItem.Content.ToString() == ".txt")
            {
                await Task.Run(() => {
                    try { itemWriter.WriteToTxt(items); }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    }
                );
            }
            else if (selectedItem.Content.ToString() == ".docx")
            {

                await Task.Run(() => {
                    try { itemWriter.WriteToWord(items); }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    }
                );
            }
            else if(selectedItem.Content.ToString() == ".xlsx")
            {
                await Task.Run(() => {
                    try { itemWriter.WriteToExcel(items); }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    }
                );
            }
            
        }

    }

    
}
