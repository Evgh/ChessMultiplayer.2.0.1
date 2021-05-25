using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMultiplayer
{
    public interface ISQLite
    {
        string GetDatabasePath(string filename);
    }

    public interface IPath
    {
        string GetDataBasePath(string filename);
    }
}
