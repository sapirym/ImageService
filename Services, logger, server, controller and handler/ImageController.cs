using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal model; // The Modal Object
        private Dictionary<int, ICommand> commands;

         /**
          * constructor for the current class.
          */
        public ImageController(IImageServiceModal model1){
            this.model = model1;
            commands = new Dictionary<int, ICommand>();
            commands.Add( (int)CommandEnum.NewFileCommand, new AddFile(model));
            //commands.Add((int)CommandEnum.CloseCommand, new AddFile(model)); - possible to next exercise
         }


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
