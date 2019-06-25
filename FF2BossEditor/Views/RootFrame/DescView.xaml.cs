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

namespace FF2BossEditor.Views.RootFrame
{
    /// <summary>
    /// Interaction logic for DescView.xaml
    /// </summary>
    public partial class DescView : Core.ExpandedTabControl
    {
        public DescView()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActualBoss == null)
                ActualBoss = new Core.Classes.Boss();

            ActualBoss.Descriptions.Add(new Core.Classes.Description());
        }

        private void DescText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == null || e == null)
                return;

            if (e.Key == Key.Enter)
            {
                TextBox senderBox = sender as TextBox;
                if (senderBox == null)
                    return;

                int count = senderBox.Text.Count(t => t == '\n');

                e.Handled = count > 5;
            }
        }

        private void LangBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is ComboBox comboBox)
            {
                if (comboBox.Template.FindName("PART_EditableTextBox", comboBox) is TextBox textBox)
                {
                    textBox.MaxLength = 2;
                }
            }
        }
    }
}
