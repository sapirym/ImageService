using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotoWeb
    {
        public PhotoWeb() { }
        /// <summary>
        /// copy method
        /// </summary>
        /// <param name="pm"></param>
        public void copy(PhotoWeb pm)
        {
            PhotoName = pm.PhotoName;
            PhotoDate = pm.PhotoDate;
            Photo = pm.Photo;
            ThumbPhoto = pm.ThumbPhoto;
        }

        /// <summary>
        /// properties
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoName:")]
        public string PhotoName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "PhotoDate:")]
        public string PhotoDate { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ThumbPhoto:")]
        public string ThumbPhoto { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Photo:")]
        public string Photo { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Photo:")]
        public string FullPath { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Photo:")]
        public string PhotoToShow { get; set; }
    }
}