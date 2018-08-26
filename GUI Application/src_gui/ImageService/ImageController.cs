
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageController : IImageController
    {
        private IImageServiceModal model; // The Modal Object
        private Dictionary<int, ICommand> commands;
        public event EventHandler<DirectoryCloseEventArgs> HandlerClosedEvent;

        /**
         * constructor for the current class.
         */
        public ImageController(IImageServiceModal model1, ILoggingService logging)
        {
            this.model = model1;
            commands = new Dictionary<int, ICommand>();
            commands.Add((int)CommandEnum.NewFileCommand, new AddFile(model));
            commands.Add((int)CommandEnum.GetConfigCommand, new ConfigCommand(model));
            commands.Add((int)CommandEnum.LogCommand, new LogCommand(logging));
            // commands.Add((int)CommandEnum.CloseCommand, new RemoveHandlerCommand());
            commands.Add((int)CommandEnum.CloseCommand, new CloseCommand());
            commands.Add((int)CommandEnum.CloseAll, new CloseAllHendlersCommand());
            ((CloseCommand)commands[(int)CommandEnum.CloseCommand]).Closed += HandlerClosed;
        }

        public void HandlerClosed(object sender, DirectoryCloseEventArgs e)
        {
            HandlerClosedEvent?.Invoke(sender, e);
        }
        //TODO : fix it

    


        /**
         *  execute the current command
         */
        public string ExecuteCommand(int commandID, string[] args, out bool result) {
            if (!commands.ContainsKey(commandID)) {
                result = false;
                return "Command not found " + commandID;
            }
            ICommand command = commands[commandID];
            Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() =>
             {
                 bool tempResult;
                 return Tuple.Create(command.Execute(args, out tempResult), tempResult);
             });
            task.Start();
            Tuple<string, bool> tuple = task.Result;
            result = tuple.Item2;
            return tuple.Item1;
        }
    }
}
