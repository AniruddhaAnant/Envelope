using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Helpers;
using Model;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;

namespace ViewModels
{
    public class ExplorerVM : ViewModelBase
    {
        private ObservableCollection<Folder> m_folders;
        private ObservableCollection<File> m_files;
        File m_selectedFile;

        private SQLiteHandler m_dbhandler;
        private FileHandler m_filehandler;
        private Stack<Folder> m_folderStack;

        private Command m_createFolderCommand;
        private Command m_addFolderOnClickCommand;
        private Command m_openFolderCommand;
        private Command m_openFileCommand;
        private Command m_navigateBackCommand;
        private Command m_importFilesCommand;
        private Command m_exportFilesCommand;
        private Command m_deleteCommand;

        private bool m_addFolderClicked = false;
        private Folder m_selectedFolder;

        private bool m_isInRootDir = true;

        public bool IsInRootDir
        {
            get
            {
                return m_isInRootDir;
            }
            set
            {
                m_isInRootDir = value;
                RaiseEvent("IsInRootDir");
            }
        }

        public Folder SelectedFolder
        {
            get { return m_selectedFolder; }
            set
            {
                m_selectedFolder = value;
                RaiseEvent("SelectedFolder");
            }
        }

        public File SelectedFile
        {
            get { return m_selectedFile; }
            set
            {
                m_selectedFile = value;
                RaiseEvent("SelectedFile");
            }
        }

        public string NewFolderName { get; set; }

        public ObservableCollection<Folder> Folders
        {
            get { return m_folders; }
            set { m_folders = value; }
        }

        public ObservableCollection<File> Files
        {
            get { return m_files; }
            set { m_files = value; }
        }

        public string CurrentFolderName
        {
            get { return m_folderStack.Peek().FolderName; }
        }

        public bool AddFolderClicked
        {
            get
            {
                return m_addFolderClicked;
            }
            set
            {
                m_addFolderClicked = value;
                RaiseEvent("AddFolderClicked");
            }
        }

        public Command CreateFolderCommand
        {
            get
            {
                if (m_createFolderCommand == null)
                {
                    m_createFolderCommand = new Command(CreateFolder, CanCreateFolder);

                }
                return m_createFolderCommand;
            }
        }

        public Command AddFolderOnClickCommand
        {
            get
            {
                if (m_addFolderOnClickCommand == null)
                {
                    m_addFolderOnClickCommand = new Command(AddFolderOnClick);

                }
                return m_addFolderOnClickCommand;
            }
        }
        public Command OpenFolderCommand
        {
            get
            {
                if (m_openFolderCommand == null)
                {
                    m_openFolderCommand = new Command(OpenFolder, CanOpenFolder);
                }
                return m_openFolderCommand;
            }
        }

        public Command OpenFileCommand
        {
            get
            {
                if (m_openFileCommand == null)
                {
                    m_openFileCommand = new Command(OpenFile, CanOpenFile);
                }
                return m_openFileCommand;
            }
        }

        private bool CanOpenFile(object arg)
        {
            return SelectedFile != null;
        }

        private void OpenFile(object obj)
        {
            var filename = SelectedFile.FileName;
            Process.Start("rundll32.exe", string.Format("shell32.dll,OpenAs_RunDLL {0}", filename));
        }

        public Command NavigateBackCommand
        {
            get
            {
                if (m_navigateBackCommand == null)
                {
                    m_navigateBackCommand = new Command(NavigateBack, CanNavigateBack);
                }
                return m_navigateBackCommand;
            }
        }

        public Command ImportFileCommand
        {
            get
            {
                if (m_importFilesCommand == null)
                {
                    m_importFilesCommand = new Command(ImportFiles, CanImportFiles);
                }
                return m_importFilesCommand;
            }
        }

        public Command DeleteCommand
        {
            get
            {
                if (m_deleteCommand == null)
                {
                    m_deleteCommand = new Command(Delete, CanDelete);
                }
                return m_deleteCommand;
            }
        }

        private bool CanDelete(object arg)
        {
            return SelectedFile != null;
        }

        private void Delete(object obj)
        {
            if (SelectedFile != null)
            {
                Files.Remove(SelectedFile);
            }

            if (SelectedFolder != null)
            {
                Folders.Remove(SelectedFolder);
            }
        }

        public Command ExportFileCommand
        {
            get
            {
                if (m_exportFilesCommand == null)
                {
                    m_exportFilesCommand = new Command(ExportFiles, CanExportFiles);
                }
                return m_exportFilesCommand;
            }
        }


        private bool CanExportFiles(object arg)
        {
            return SelectedFile != null;
        }

        private void ExportFiles(object obj)
        {
            FolderBrowserDialog saveFilesDialog = new FolderBrowserDialog();
            saveFilesDialog.Description = "Select Export Directory";
            if (saveFilesDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = saveFilesDialog.SelectedPath;
                var file = SelectedFile;
                m_filehandler.ExportFile(file.FilePath, System.IO.Path.Combine(filePath, file.FileName));
            }
        }

        private bool CanImportFiles(object arg)
        {
            return true;
        }

        private void ImportFiles(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import File(s)";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var filename in openFileDialog.FileNames)
                {
                    var encFilePath = m_filehandler.ImportFile(filename);
                    File file = new File(System.IO.Path.GetFileName(filename),
                        SelectedFolder.FolderId, encFilePath, System.IO.Path.GetExtension(filename));
                    m_dbhandler.InsertFile(file);
                }
            }
            UpdateFiles();
        }

        private bool CanNavigateBack(object arg)
        {
            return !IsInRootDir;
        }

        private void NavigateBack(object obj)
        {
            IsInRootDir = true;
            m_folderStack.Pop();
            SelectedFolder = m_folderStack.Peek();
            UpdateFiles();
        }

        private bool CanOpenFolder(object arg)
        {
            return SelectedFolder != Folders[0];
        }

        private void OpenFolder(object obj)
        {
            IsInRootDir = false;
            m_folderStack.Push(SelectedFolder);
            UpdateFiles();
        }

        private void AddFolderOnClick(object obj)
        {
            AddFolderClicked = !AddFolderClicked;
        }

        private bool CanCreateFolder(object arg)
        {
            return true;
        }

        private void CreateFolder(object obj)
        {
            var user = m_dbhandler.GetUser();
            var newFolder = new Folder(NewFolderName, user);
            m_dbhandler.InsertFolder(newFolder);
            AddFolderClicked = false;
            UpdateFolders();
        }

        private void UpdateFolders()
        {
            Folders.Clear();
            foreach (var folder in m_dbhandler.GetFolders())
            {
                Folders.Add(folder);
            }
        }

        private void UpdateFiles()
        {
            Files.Clear();
            foreach (var file in m_dbhandler.GetFiles(SelectedFolder))
            {
                Files.Add(file);
            }
        }
        public ExplorerVM()
        {
            Folders = new ObservableCollection<Folder>();
            Files = new ObservableCollection<File>();

            m_dbhandler = new SQLiteHandler();
            var user = m_dbhandler.GetUser();
            m_filehandler = new FileHandler(user.Key, user.Uid);

            m_folderStack = new Stack<Folder>();
            UpdateFolders();
            m_folderStack.Push(Folders[0]);
            SelectedFolder = m_folderStack.Peek();

            UpdateFiles();
        }
    }
}
