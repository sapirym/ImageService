using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using static System.IDisposable;
using static System.Drawing.Imaging.PropertyItem;
using Newtonsoft.Json;

namespace ImageService
{
    public class ImageServiceModal : IImageServiceModal
    {

        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        private static Regex r = new Regex(":");

        #endregion

        /**
         * constructor
         */
        public ImageServiceModal(string OutputFolder, int thumbnailSize)
        {
            this.m_OutputFolder = OutputFolder;
            this.m_thumbnailSize = thumbnailSize;
        }

        /**
         * handle new file.
         */
        public string AddFile(string path, out bool result)
        {
            try
            {
                if (File.Exists(path))
                {

                    DateTime dateImg = GetDateTakenFromImage(path);

                    string year = dateImg.Year.ToString();
                    string month = dateImg.Month.ToString();
                    string thumb = this.m_OutputFolder + "\\" + "Thumbnail";

                    DirectoryInfo di = Directory.CreateDirectory(this.m_OutputFolder); // create OutputDir
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    Directory.CreateDirectory(thumb); // create Thumbnail

                    string fullPathOut = HandleFileAccordingType(dateImg, this.m_OutputFolder) + "\\" + Path.GetFileName(path);
                    string fullPatThumb = HandleFileAccordingType(dateImg, thumb);

                    string newNameOut = renameFileAlreadyExists(fullPathOut);
                    string newNameThumb = renameFileAlreadyExists(fullPatThumb);

                    File.Copy(path, newNameOut, true);
                    File.Delete(path);
                    Image image = Image.FromFile(newNameOut);
                    image = resizeImage(image, new Size(m_thumbnailSize, m_thumbnailSize));
                    image.Save(newNameThumb + "\\" + Path.GetFileName(newNameOut));

                    result = true;
                    return "the file create correctly in" + newNameOut;
                }
                else
                {
                    result = false;
                    return "Not successed found file";
                }
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }

        public string renameFileAlreadyExists(string fullPathOut)
        {
            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPathOut);
            string extension = Path.GetExtension(fullPathOut);
            string path1 = Path.GetDirectoryName(fullPathOut);
            string newFullPath = fullPathOut;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path1, tempFileName + extension);
            }
            return newFullPath;
        }

        /**
* make the right path for the file.
*/
        public string HandleFileAccordingType(DateTime dateImg, string destPath)
        {
            Directory.CreateDirectory(destPath + "\\" + dateImg.Year.ToString());
            string fullPath = destPath + "\\" + dateImg.Year.ToString() + "\\" + dateImg.Month.ToString();
            Directory.CreateDirectory(fullPath);
            return fullPath;
        }

        /**
         *  The Function resize the image size
         */
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        /**
         *  the function move file from dir source to dir dst
         */
        private string MoveFile(string pathsrc, string pathdst)
        {
            System.IO.File.Move(pathsrc, pathdst);
            if (!System.IO.File.Exists(pathdst))
                return "false";
            else
                return "true";
        }

        /**
         * retrieves the datetime WITHOUT loading the whole image
         */
        private DateTime GetDateTakenFromImage(string path)
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
                    return DateTime.Parse(dateTaken);
                }
                else
                    return new FileInfo(path).LastWriteTime;
            }
        }

        /**
         *  the function copy file from dir source to dir dst
         */
        private string CopyFile(string pathsrc, string pathdst)
        {
            System.IO.File.Copy(pathsrc, pathdst);
            if (!System.IO.File.Exists(pathdst))
                return "false";
            else
                return "true";

        }

        public string LogNotification(string type, string messege, out bool result)
        {

            try
            {
                
                result = true;
                return "the file create correctly in";// + newNameOut;


            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
            }

       

    }
}

