using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Student
    {
        /// <summary>
        /// copy method
        /// </summary>
        /// <param name="pm"></param>
        public void copy(Student pm)
        {
            ID = pm.ID;
            FirstName = pm.FirstName;
            LastName = pm.LastName;
        }

        /// <summary>
        /// properties
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
    }
}