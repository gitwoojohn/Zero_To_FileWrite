using System.ComponentModel;

namespace WPF_FileContentDelete.ViewModels
{
    public class SelectionFileDataSource : INotifyPropertyChanged
    {
        private string filePath;
        private long fileSize;

        public string FilePath
        {
            get { return filePath; }
            set
            {
                if( filePath != value )
                {
                    filePath = value;
                    OnPropertyChanged( nameof( FilePath ) );
                }
            }
        }

        public long FileSize
        {
            get { return fileSize; }
            set
            {
                if( fileSize != value )
                {
                    fileSize = value;
                    OnPropertyChanged( nameof( FileSize ) );
                }
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged( string propertyName )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
        #endregion
    }
}
