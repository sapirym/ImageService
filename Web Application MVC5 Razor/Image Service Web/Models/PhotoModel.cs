using ImageService;
using ImageServiceWeb.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotoModel
    {
        static string DirectoryOutput { get; set; }
        private static Regex r = new Regex(":");
        private static int numOfPic { get; set; }
        public int NumOfPic { get; set; }
        static int count = 0;
        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoModel()
        {
            clientTcpConnectionWeb singelton = clientTcpConnectionWeb.Instance;
            string serviceConnect = singelton.IsServiceConnect();
            numOfPic = 0;
            if (serviceConnect.Equals("Connect"))
            {
                singelton.MessageReceived += getConfig;
                DataInfo msg = new DataInfo(CommandEnum.GetConfigCommand, null);
                singelton.WriteToServer(msg.toJson());
                Photos = new List<PhotoWeb>();
                getListPhotos();
                count++;
            }
        }

        /// <summary>
        /// get number of pics
        /// </summary>
        /// <returns></returns>
        /*public int getNumber()
        {
            return numOfPic;
        }*/
        /// <summary>
        /// get number of pics
        /// </summary>
        /// <returns></returns>
        public int getNumber()
        {
            int numOfPhoto = 0;
            string path = cutPath(DirectoryOutput, "C:", 2);
            string outputDir = @"../../" + path;
            string[] files = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories);
            foreach (string photo in files)
            {
                string fileEnding = Path.GetExtension(photo);
                if (fileEnding.Equals(".png", StringComparison.CurrentCultureIgnoreCase)
                            || fileEnding.Equals(".gif", StringComparison.CurrentCultureIgnoreCase)
                            || fileEnding.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase)
                            || fileEnding.Equals(".bmp", StringComparison.CurrentCultureIgnoreCase))
                {
                    numOfPhoto++;
                }
            }
            return numOfPhoto;
        }

        /// <summary>
        /// get config details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        static void getConfig(object sender, DataInfo msg)
        {
            if (msg.Id == CommandEnum.GetConfigCommand)
            {
                string[] arr = JsonConvert.DeserializeObject<string[]>(msg.Args);
                DirectoryOutput = arr[1];
            }
        }

        /// <summary>
        /// get the list of the photos
        /// </summary>
        private void getListPhotos()
        {
            List<PhotoWeb> list = new List<PhotoWeb>();
            if (DirectoryOutput != null)
            {
                string path = cutPath(DirectoryOutput, "C:", 2);
                string outputDir = @"../../" + path;
                string[] files = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories);
                Photos.Clear();
                foreach (string photo in files)
                {
                    string fileEnding = Path.GetExtension(photo);
                    if (fileEnding.Equals(".png", StringComparison.CurrentCultureIgnoreCase)
                                || fileEnding.Equals(".gif", StringComparison.CurrentCultureIgnoreCase)
                                || fileEnding.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase)
                                || fileEnding.Equals(".bmp", StringComparison.CurrentCultureIgnoreCase))
                    {
                        numOfPic++;
                        PhotoWeb temp = new PhotoWeb()
                        {
                            PhotoToShow = @"~/" +  cutPath(getNamePath(photo), "ImageServiceWeb", 15),
                            Photo = getNamePath(photo),
                            PhotoName = newGetName(photo),
                            PhotoDate = getDataFromPic(photo),
                            ThumbPhoto = @"~/" +  cutPath(photo, "ImageServiceWeb", 15),
                            FullPath = photo

                        };
                        if (Photos != null && temp.ThumbPhoto.Contains("Thumbnail"))
                            Photos.Insert(0, temp);
                    }
                }
            }
        }

        /// <summary>
        /// get thumb path
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private static string getThumb(string photo)
        {
            if (photo.Contains("\\Thumbnail"))
            {
                return photo;
            }
            return "";
        }

        /// <summary>
        /// get name path
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private static string getNamePath(string photo)
        {
            try
            {
                string[] paths = photo.Split(new string[] { "\\Thumbnail" }, StringSplitOptions.None);
                return paths[0] + paths[1];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// cut the path
        /// </summary>
        /// <param name="a_photo"></param>
        /// <param name="cut"></param>
        /// <param name="wordlen"></param>
        /// <returns></returns>
        private static string cutPath(string a_photo, string cut, int wordlen)
        {
            if (a_photo != "")
            {
                return a_photo.Substring(a_photo.LastIndexOf(cut) + 1 + wordlen);
            }
            else
            {
                string returnVal = "";
                return returnVal;
            }
        }

        /// <summary>
        /// get the name 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string newGetName(string path)
        {
            return Path.GetFileName(path);
        }
        /// <summary>
        /// get the data from the pic
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string getDataFromPic(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = null;
                try
                {
                    propItem = myImage.GetPropertyItem(36867);
                }
                catch { }
                if (propItem != null)
                {
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken).ToShortDateString();
                }
                else
                    return new FileInfo(path).LastWriteTime.ToShortDateString();
            }

        }

        /// <summary>
        /// properties
        /// </summary>
        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "logList:   ")]
        public List<PhotoWeb> Photos { get; set; } = new List<PhotoWeb>();

    }
}