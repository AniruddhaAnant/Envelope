using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Helpers;
using Model;
using System.Windows;

namespace ViewModels
{
    public class ExplorerVM : ViewModelBase
    {
        private ObservableCollection<Folder> m_folders;
        private SQLiteHandler m_handler;
        private Command m_createFolderCommand;
        private Command m_addFolderOnClickCommand;
        private bool m_addFolderClicked = false;


        public string NewFolderName { get; set; }

        public ObservableCollection<Folder> Folders
        {
            get { return m_folders; }
            set { m_folders = value; }
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
            var user = m_handler.GetUser();
            var newFolder = new Folder(NewFolderName, user);
            m_handler.InsertFolder(newFolder);
            AddFolderClicked = false;
            UpdateFolders();
        }

        private void UpdateFolders()
        {
            Folders.Clear();
            foreach (var folder in m_handler.GetFolders())
            {
                Folders.Add(folder);
            }
        }
        public ExplorerVM()
        {
            Folders = new ObservableCollection<Folder>();
            m_handler = new SQLiteHandler();
            UpdateFolders();
        }
    }
}
