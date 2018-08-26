

using System;

namespace ImageService
{
    public interface IImageController
    {
         event EventHandler<DirectoryCloseEventArgs> HandlerClosedEvent;

        string ExecuteCommand(int commandID, string[] args, out bool result);// Executing the Command Requet
    }
}
