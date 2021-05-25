using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessMultiplayer.Droid;
using System.IO;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(AndroidDBPath))]

namespace ChessMultiplayer.Droid
{
    public class AndroidDBPath : IPath
    {
        //public string GetDataBasePath(string filename)
        //{
        //    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        //}

        public string GetDataBasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }
    }
}