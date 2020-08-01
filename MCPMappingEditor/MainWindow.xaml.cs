using MCPMappingEditor.Model;
using MCPMappingEditor.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace MCPMappingEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ClassMapViewModel> Classes { get; } = new ObservableCollection<ClassMapViewModel>();
        private MapFileManager mapFileManager = new MapFileManager();
        private MapCollection mappings;

        private string filePath;
        private string lastDirectory = "%userprofile%\\Documents";

        public bool IsFileOpen => filePath != null;

        private bool decompileOnSave = false;

        public MainWindow()
        {
            InitializeComponent();

            ClassesTreeView.DataContext = this;

            ClassesTreeView.MouseDoubleClick += ClassesTreeView_MouseDoubleClick;
            ClassesTreeView.SelectedItemChanged += ClassesTreeView_SelectedItemChanged;
            ClassesTreeView.KeyDown += ClassesTreeView_KeyDown;

            SearchBox.TextChanged += SearchBox_TextChanged;

            if (File.Exists(".\\decompile.bat"))
            {
                FileMenu.Items.Insert(3, new Separator());
                MenuItem decompileOnSaveItem = new MenuItem();
                decompileOnSaveItem.Header = "Decompile On Save";
                decompileOnSaveItem.IsCheckable = true;
                decompileOnSaveItem.Click += DecompileOnSaveItem_Click;
                FileMenu.Items.Insert(4, decompileOnSaveItem);
            }
        }

        private void DecompileOnSaveItem_Click(object sender, RoutedEventArgs e)
        {
            decompileOnSave = (sender as MenuItem).IsChecked;
        }

        private void ClassesTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BaseMapEntryViewModel selectedItem = ClassesTreeView.SelectedItem as BaseMapEntryViewModel;
                if (selectedItem == null) return;

                selectedItem.IsSubstituteTextBoxVisible = selectedItem.IsSubstituteTextBoxVisible switch
                {
                    Visibility.Visible => Visibility.Hidden,
                    _ => Visibility.Visible
                };

                selectedItem.IsTextBoxFocused = selectedItem.IsSubstituteTextBoxVisible == Visibility.Visible;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "") return;

            string[] parts = SearchBox.Text.Split('.');

            List<ClassMapViewModel> candidates = Classes.Where((x) => x.Substitute.StartsWith(parts[0])).ToList();
            candidates.Sort((x, y) => x.Substitute.Length - y.Substitute.Length);

            if (candidates.Count != 0)
            {
                TreeViewItem classItem = ClassesTreeView.ItemContainerGenerator.ContainerFromItem(candidates[0]) as TreeViewItem;

                if (parts.Length > 1)
                {
                    if (parts[1] == "") return;
                    List<BaseClassMemberMapEntryViewModel> classCandidates = candidates[0].Members.Where((x) => x.Substitute.StartsWith(parts[1])).ToList();
                    classCandidates.Sort((x, y) => x.Substitute.Length - y.Substitute.Length);
                    if (classCandidates.Count != 0)
                    {
                        classItem.IsExpanded = true;
                        TreeViewItem classMemberItem = classItem.ItemContainerGenerator.ContainerFromItem(classCandidates[0]) as TreeViewItem;
                        if (classMemberItem == null) return;
                        classMemberItem.IsSelected = true;
                        classMemberItem.BringIntoView();
                    }
                }
                else
                {
                    classItem.IsSelected = true;
                    classItem.BringIntoView();
                }

            }
        }

        private void ClassesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.OldValue == null) return;

            BaseMapEntryViewModel oldItem = e.OldValue as BaseMapEntryViewModel;
            oldItem.IsSubstituteTextBoxVisible = Visibility.Hidden;

            oldItem.IsTextBoxFocused = false;
        }

        private void ClassesTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseMapEntryViewModel activeItem = ClassesTreeView.SelectedItem as BaseMapEntryViewModel;
            activeItem.IsSubstituteTextBoxVisible = Visibility.Visible;

            activeItem.IsTextBoxFocused = true;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveFile(string filePath)
        {
            mapFileManager.Serialise(filePath);
            mappings.CollectionModified = false;
            if (decompileOnSave)
            {
                Decompile(".");
            }
        }

        private void OpenFile(string filePath)
        {
            Classes.Clear();

            mappings = mapFileManager.Deserialize(filePath);

            foreach (ClassMap classMap in mappings.ClassMaps)
            {
                Classes.Add(new ClassMapViewModel(classMap, mappings, mappings.ClassMembers(classMap)));
            }

            ClassesTreeView.Items.Refresh();
            ClassesTreeView.Items.SortDescriptions.Add(new SortDescription("Original", ListSortDirection.Ascending));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (mappings?.CollectionModified == true)
            {
                MessageBoxResult result = MessageBox.Show(this, "Save before quitting?", "Quit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveFile(filePath);
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
            base.OnClosing(e);
        }

        private void CommandOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "RetroGuard Script (.rgs)|*.rgs|All files|*.*",
                InitialDirectory = lastDirectory,
                Multiselect = false
            };
            bool? result = dialog.ShowDialog();
            while (result == null) ;
            if (result == true)
            {
                lastDirectory = Directory.GetDirectoryRoot(dialog.FileName);
                filePath = dialog.FileName;

                OpenFile(filePath);

                Title = $"MCP Mapping Editor ({filePath})";

                SaveMenuItem.IsEnabled = true;
                SaveAsMenuItem.IsEnabled = true;
            }
        }

        private void CommandSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!IsFileOpen) return;
            SaveFile(filePath);
        }

        private void CommandSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!IsFileOpen) return;
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "RetroGuard Script (.rgs)|*.rgs|All files|*.*",
                InitialDirectory = lastDirectory
            };
            bool? result = dialog.ShowDialog();
            while (result == null) ;

            if (result == true)
            {
                lastDirectory = Directory.GetDirectoryRoot(dialog.FileName);
                if (System.IO.Path.HasExtension(dialog.FileName) == false)
                {
                    filePath = dialog.FileName + ".rgs";
                }
                else
                {
                    filePath = dialog.FileName;
                }

                SaveFile(filePath);

                Title = $"MCP Mapping Editor ({filePath})";
            }
        }

        private void Decompile(string mcpPath)
        {
            Process process = Process.Start(mcpPath + "\\decompile.bat");
            process.WaitForExit();
        }
    }
}
