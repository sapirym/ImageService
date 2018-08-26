
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class AddFile : ICommand
    {
        private IImageServiceModal modal;

        public AddFile(IImageServiceModal modal2)
        {
            this.modal = modal2;
        }

        public string Execute(string[] args, out bool result)
        {
            string msg= null;
            try
            {
               msg = this.modal.AddFile(args[0], out result);
                return msg;
            }
            catch (Exception e )
            {
                result = false;
                return "failed with transfer file"+ msg;
            }
        }
    }
}
