namespace WpfNetBootstrap.Common.Services.IO
{


    public static class FileIOFactory
    {
        private static IFileIO _FileIO = null;
        public static IFileIO GetFileIO()
        {
            return _FileIO;
        }

        public static void SetFileIO(IFileIO fileIO)
        {
            _FileIO = fileIO;
        }

    }
}
