using ImageService;
using ImageServiceWeb.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ImageWebModel
    {
        static string DirectoryOutput { get; set; }

        static int count = 0;
        
        /// <summary>
        /// imageWebModel constructor
        /// </summary>
        public ImageWebModel()
        {
            clientTcpConnectionWeb singelton = clientTcpConnectionWeb.Instance;
            string serviceConnect = singelton.IsServiceConnect();
            Status = serviceConnect;
            if (serviceConnect.Equals("Connect"))
            {
                singelton.MessageReceived += getConfig;
                DataInfo msg = new DataInfo(CommandEnum.GetConfigCommand, null);
                singelton.WriteToServer(msg.toJson());
                Students = getStudents();
                count++;
            }
            
        }

        /// <summary>
        /// get student from file
        /// </summary>
        /// <returns></returns>
        static List<Student> getStudents()
        {
            List<Student> students = new List<Student>();
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/studentsInfo.txt"));
            string[] first = reader.ReadLine().Split(';');
            students.Add(new Student()
            {
                FirstName = first[0],
                LastName = first[1],
                ID = first[2]
            });
            string[] sec = reader.ReadLine().Split(';');
            students.Add(new Student()
            {
                FirstName = sec[0],
                LastName = sec[1],
                ID = sec[2]
            });
            reader.Close();
            return students;
        }

        /// <summary>
        /// get config daat
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
        /// get number of pics
        /// </summary>
        /// <returns></returns>
        private int getNumber()
        {
            return NumOfPics;
        }


        /// <summary>
        /// properties
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Status:    ")]
        public string Status{ get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Number of pics:    ")]
        public int NumOfPics { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Students")]
        public List<Student> Students { get; set; }



    }
}