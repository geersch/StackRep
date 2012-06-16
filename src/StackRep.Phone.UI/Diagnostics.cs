using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;
using StackRep.Phone.UI.Resources;

namespace StackRep.Phone.UI
{
    public class Diagnostics
    {
        private static bool _initiliazed;

        public Diagnostics()
        {
            EmailSubject = "Error report: [{0}] [{1}]. Hashcode: [{2}]";
            HandleUnhandledException = true;
        }

        public string EmailTo { get; set; }

        public string EmailSubject { get; set; }

        public string ApplicationName { get; set; }

        public string ApplicationVersion { get; set; }

        public bool HandleUnhandledException { get; set; }

        public void Initialize()
        {
            if (_initiliazed || Application.Current == null)
                return;

            AssemblyName assemblyName = null;
            if (String.IsNullOrEmpty(ApplicationName))
            {
                assemblyName = new AssemblyName(Application.Current.GetType().Assembly.FullName);
                ApplicationName = assemblyName.Name;
            }

            if (String.IsNullOrEmpty(ApplicationVersion))
            {
                if (assemblyName == null)
                {
                    assemblyName = new AssemblyName(Application.Current.GetType().Assembly.FullName);
                }

                ApplicationVersion = assemblyName.Version.ToString();
            }

            Application.Current.UnhandledException += OnUnhandledException;

            _initiliazed = true;
        }

        private void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Application.Current.UnhandledException -= OnUnhandledException;
            ProcessException(e);
            Application.Current.UnhandledException += OnUnhandledException;
        }

        private void ProcessException(ApplicationUnhandledExceptionEventArgs e)
        {
            e.Handled = HandleUnhandledException;
            ReportHandledException(e.ExceptionObject);
        }

        private void ReportHandledException(Exception e)
        {
            if (MessageBox.Show(Text.ApplicationErrorInfo, Text.ApplicationError, MessageBoxButton.OKCancel) ==
                MessageBoxResult.OK)
            {
                SendErrorReport(e);
            }
        }

        private void SendErrorReport(Exception e)
        {
            var stackTrace = e != null && e.StackTrace != null ? e.StackTrace.GetHashCode() : 0;
            var emailTask = new EmailComposeTask
            {
                To = EmailTo,
                Subject = String.Format(EmailSubject, ApplicationName, ApplicationVersion, stackTrace),
                Body = ComposeMessageBody(e)
            };

            emailTask.Show();
        }

        private string ComposeMessageBody(Exception e)
        {
            var stringBuilder = new StringBuilder();

            AddDiagnosticLine(stringBuilder, "ExceptionMessage", e.Message);
            AddDiagnosticLine(stringBuilder, "StackTrace", String.Concat(Environment.NewLine, e.StackTrace));
            AddDiagnosticLine(stringBuilder, "OccurrenceDate", DateTime.Now.ToUniversalTime().ToString("r"));
            AddDiagnosticLine(stringBuilder, "AppVersion", ApplicationVersion);
            AddDiagnosticLine(stringBuilder, "Culture", CultureInfo.CurrentCulture);
            if (Application.Current.RootVisual != null)
            {
                var rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (rootFrame != null && rootFrame.CurrentSource != null)
                {
                    AddDiagnosticLine(stringBuilder, "CurrentPageSource", rootFrame.CurrentSource.ToString());
                    AddDiagnosticLine(stringBuilder, "NavigationStack", GetNavigationStackInfo(rootFrame));
                }
            }

            AddDiagnosticLine(stringBuilder, "DeviceManufacturer", DeviceStatus.DeviceManufacturer);
            AddDiagnosticLine(stringBuilder, "DeviceModel", DeviceStatus.DeviceName);
            AddDiagnosticLine(stringBuilder, "DeviceHardwareVersion", DeviceStatus.DeviceHardwareVersion);
            AddDiagnosticLine(stringBuilder, "DeviceFirmwareVersion", DeviceStatus.DeviceFirmwareVersion);
            AddDiagnosticLine(stringBuilder, "OSVersion", Environment.OSVersion);
            AddDiagnosticLine(stringBuilder, "CLRVersion", Environment.Version);
            AddDiagnosticLine(stringBuilder, "DeviceType", Microsoft.Devices.Environment.DeviceType);
            AddDiagnosticLine(stringBuilder, "NetworkType", NetworkInterface.NetworkInterfaceType);

            return stringBuilder.ToString();
        }

        private void AddDiagnosticLine(StringBuilder builder, object paramName, object paramValue)
        {
            builder.AppendFormat("[{0}]:[{1}]", paramName, paramValue);
            builder.AppendLine();
        }

        private string GetNavigationStackInfo(PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null)
                return String.Empty;

            var stackInfo = new StringBuilder();
            foreach (var entry in rootFrame.BackStack)
            {
                stackInfo.AppendLine();
                stackInfo.Append(entry.Source.ToString());
            }

            return stackInfo.ToString();
        }
    }
}
