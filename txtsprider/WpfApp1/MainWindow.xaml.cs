using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            web.EnsureCoreWebView2Async();

            web.CoreWebView2InitializationCompleted += Web_CoreWebView2InitializationCompleted;
        }

        private void Web_CoreWebView2InitializationCompleted(
            object? sender,
            CoreWebView2InitializationCompletedEventArgs e
        )
        {
            web.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upload.html");

            web.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

            web.Source = new Uri(filePath);

            web.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            supportEleName = "id:filePatcher";
        }

        private void CoreWebView2_ProcessFailed(
            object? sender,
            CoreWebView2ProcessFailedEventArgs e
        )
        {

        }

        private async void CoreWebView2_WebMessageReceived(
            object? sender,
            CoreWebView2WebMessageReceivedEventArgs e
        )
        {
            if (e.WebMessageAsJson.Contains("checkFile"))
            {
                string domId = "id:filePatcher";
                await ShowCustomFilePicker(domId!);
            }
        }

        string supportEleName = "";

        private async void CoreWebView2_NewWindowRequested(
            object? sender,
            CoreWebView2NewWindowRequestedEventArgs e2
        )
        {
            using (e2.GetDeferral())
            {
                e2.Handled = true;
                String _fileurl = e2.Uri.ToString();
                if (_fileurl.StartsWith("file://") && !string.IsNullOrWhiteSpace(supportEleName))
                {
                    //set file to input[type=file] by cdp
                    await ShowCustomFilePicker(supportEleName, _fileurl);
                }
            }
        }

        private async Task ShowCustomFilePicker(string DomEleId, string checkPath = "")
        {
            OpenFileDialog dialog = null;
            if (string.IsNullOrEmpty(checkPath))
            {
                dialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Title = "find file",
                    Filter = "all file(*.*)|*.*"
                };
            }
            if (dialog?.ShowDialog() ?? false || !string.IsNullOrEmpty(checkPath))
            {
                try
                {
                    string file = string.IsNullOrEmpty(checkPath) ? dialog.FileName : checkPath;
                    var nextHelper = web.CoreWebView2.GetDevToolsProtocolHelper();
                    DOM dom = nextHelper.DOM;
                    DOM.Node t = await dom.GetDocumentAsync(0, false);
                    DomEleId = DomEleId.Replace("id:", "#");
                    int querySelectorResponse = await dom.QuerySelectorAsync(t.NodeId, DomEleId);
                    await nextHelper.DOM.SetFileInputFilesAsync(
                        new string[] { file },
                        querySelectorResponse
                    );
                    await nextHelper.DOM.SetAttributeValueAsync(
                        querySelectorResponse,
                        "curFilePatcher",
                        file
                    );
                }
                catch (Exception ex)
                {


                }
            }
        }

        private void CoreWebView2_ServerCertificateErrorDetected(
            object? sender,
            CoreWebView2ServerCertificateErrorDetectedEventArgs e
        )
        {
            throw new NotImplementedException();
        }

        private void Web_PreviewDragEnter(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CoreWebView2_ScriptDialogOpening(
            object? sender,
            CoreWebView2ScriptDialogOpeningEventArgs e
        )
        { }
    }
}
