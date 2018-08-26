using ImageService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class LogWeb
    {
        public LogWeb() { }

        /// <summary>
        /// copy method
        /// </summary>
        /// <param name="pm"></param>
        public void copy(LogWeb pm)
        {
            Type = pm.Type;
            Msg = pm.Msg;
        }

        /// <summary>
        /// propeties
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type:  ")]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Msg:   ")]
        public string Msg { get; set; }

    }
}