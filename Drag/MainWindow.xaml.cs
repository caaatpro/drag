using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Drag
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public delegate void InvokeDelegate();
        public static List<string> FileList;

        public FileSystemWatcher Watcher = new FileSystemWatcher();
        public static string[] Paths = {""};
        private static string DirPath = @"D:\Downloads";

        public MainWindow()
        {
            InitializeComponent();

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = DirPath;
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.jpg";

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Paths[0] = e.FullPath;
            
            
         
                this.Dispatcher.BeginInvoke((ThreadStart)delegate () {
                    // в случае отображения изображения оно блокируется и считается открытым - нужно копировать
                    //image1.Source = new BitmapImage(new Uri(e.FullPath, UriKind.RelativeOrAbsolute));
                    label1.Content = e.Name;
                });
        }

        // Define the event handlers.
        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            Paths[0] = "";
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (File.Exists(Paths[0]))
            {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, Paths),
                                    DragDropEffects.Copy);
            }
        }
    }
}
