using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMultiplayer.UWP;
using Xamarin.Forms;
using System.IO;
using Windows.Storage;

[assembly: Dependency(typeof(UwpDBPath))]

namespace ChessMultiplayer.UWP
{
    class UwpDBPath : IPath
    {
        public string GetDataBasePath(string sqliteFilename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
        }
    }
}
