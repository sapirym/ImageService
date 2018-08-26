
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Server;
using System.Configuration;
using ImageService.Logging.Modal;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        #region Members
        private int eventId = 1;
        private ImageServer m_imageServer; // The Image Server
        private IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;
        private string sourceName;
        private string outputDir;
        private int thumbnailSize;
        private string logName;
        #endregion

        /*
         * Constructor (no parameter)
         * */
        public ImageService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            initializeConf();
            if (!System.Diagnostics.EventLog.SourceExists(sourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    sourceName, logName);
            }
            eventLog1.Source = sourceName;
            eventLog1.Log = logName;

            logging = new LoggingService();
            logging.MessageRecieved += Logging_MessageRecieved;
            initilaizeClasses();

        }

        /*
         * initialize the member -parse the app confog file
         * */
        public void initializeConf()
        {
            this.sourceName = ConfigurationManager.AppSettings["SourceName"];
            this.logName = "ImageServiceLog";//ConfigurationManager.AppSettings["LogName"];
            this.thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            this.outputDir = ConfigurationManager.AppSettings["OutputDir"];
        }

        /**
         *  Constructor (with parameter)
         */
        public ImageService(string[] args)
        {
            InitializeComponent();
            initializeConf();
            if (args.Count() > 0)
            {
                sourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(sourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(sourceName, logName);
            }
            eventLog1.Source = this.sourceName;
            eventLog1.Log = this.logName;

            logging = new LoggingService();
            logging.MessageRecieved += Logging_MessageRecieved;
            initilaizeClasses();

        }

        /**
         * start run service.
         */
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In OnStart");

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        /**
         * initialize all classes.
         */
        public void initilaizeClasses()
        {
            modal = new ImageServiceModal(this.outputDir, this.thumbnailSize);
            eventLog1.WriteEntry("initilaizeClasses: " + this.outputDir);
            controller = new ImageController(modal);
            m_imageServer = new ImageServer(this.controller, this.logging);
            m_imageServer.Start();
            logging.MessageRecieved += Logging_MessageRecieved;// (this,eventArgs) ;
        }

        /**
         * update the log that msg recived.
         */
        private void Logging_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            eventLog1.WriteEntry(e.Status + ": " + e.Message);
        }

        /**
         * stop the service and update the log.
         */
        protected override void OnStop()
        {
            eventLog1.WriteEntry("Service stopped.");
            m_imageServer.CloseService();
        }

       
        /**
         * monitor and update log.
         */
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        /**
         * on continue function.
         */
        protected override void OnContinue()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In OnContinue.");

            // Update the service state to Paused.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
    }
}
