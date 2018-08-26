
using ImageServiceWeb.Communication;
using ImageServiceWeb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;
using ImageService;
using Newtonsoft.Json;
using System.Windows;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageServiceWeb.Controllers
{
    public class PhotoController : Controller
    {
        private string ToRemove;
        private static  string photoToRemove;
        static clientTcpConnectionWeb singelton = clientTcpConnectionWeb.Instance;
        static string serviceConnect = singelton.IsServiceConnect();
        static LogModel logModel;
        static PhotoModel photoModel;
        static ConfigModel configModel;
        static ImageWebModel imageWebModel;
        static int NumOfPicture;

        
        /// <summary>
        /// photo controller constructor
        /// </summary>
        public PhotoController()
        {
            photoModel = new PhotoModel();
            logModel = new LogModel();
            configModel = new ConfigModel();
            imageWebModel = new ImageWebModel();
            NumOfPicture = photoModel.getNumber();
        }

        /// <summary>
        /// index of action result
        /// </summary>
        /// <returns></returns>
        // GET: Photo
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// remove handler page
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult RemoveHandler(string path)
        {
            ToRemove = path;
            return View();
        }       

        /// <summary>
        /// http post requst to delete after ok
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult deleteAfterOk() 
        {
            int i = 0;
            if (configModel.Handlers != null)
            {
                foreach (string emp in configModel.Handlers)
                {
                    if (emp.Equals(ToRemove))
                    {
                        DataInfo remove = new DataInfo(CommandEnum.CloseCommand, ToRemove);
                        singelton.WriteToServer(remove.toJson());
                        configModel.Handlers.RemoveAt(i);
                        return RedirectToAction("Config");
                    }
                    i++;
                }
            }
            return RedirectToAction("ImageWeb");
        }

        /// <summary>
        /// http post requst to delete pics
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult deleteAfterOkPics()
        {
            int i = 0;
            if (photoModel.Photos != null)
            {
                foreach (PhotoWeb pic in photoModel.Photos)
                {
                    if (pic.PhotoName.Equals(photoToRemove))
                    {
                        System.IO.File.Delete(pic.FullPath);
                        System.IO.File.Delete(pic.Photo);
                        return RedirectToAction("Photos");
                    }
                    i++;
                }
                return RedirectToAction("Photos");
            }
            return RedirectToAction("ImageWeb");
        }

        /// <summary>
        /// show page of photos
        /// </summary>
        /// <returns></returns>
        public ActionResult Photos()
        {
            return View(photoModel.Photos);
        }

        /// <summary>
        /// delete photo page
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        public ActionResult DeletePhoto(string photo)
        {
            if (photo != null)
                photoToRemove = photo;
            foreach (PhotoWeb item in photoModel.Photos)
            {
                if (item.PhotoName.Equals(photoToRemove))
                {
                    return View(item);
                }
            }
            return View();
        }

        /// <summary>
        /// view photo page
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        public ActionResult ViewPhoto(string photo)
        {
            photoToRemove = photo;
            foreach(PhotoWeb item in photoModel.Photos)
            {
                if (item.PhotoName.Equals(photoToRemove))
                {
                    return View(item);
                }
            }
            return View();
        }

        /// <summary>
        /// logs page
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            return View(logModel.Logs);
        }

        /// <summary>
        /// image web page
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageWeb()
        {
            imageWebModel.NumOfPics = NumOfPicture;
            return View(imageWebModel);
        }

        /// <summary>
        /// config page
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            return View(configModel);
        }
    }
}