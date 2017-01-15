using System.Collections.ObjectModel;

namespace WPF_FileContentDelete.ViewModels
{
    public class ListViewDataSource : ObservableCollection<SelectionFileData> { }

    public class SelectionFileData
    {
        private string _filePath;
        private long _fileSize;

        public string FilePath
        {
            get { return _filePath; }
        }

        public long FileSize
        {
            get { return _fileSize; }
        }

        public override string ToString()
        {
            return FilePath;
        }

        public SelectionFileData( string filePath, long fileSize )
        {
            _filePath = filePath;
            _fileSize = fileSize;
        }
    }
}
