using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Helper;
using Microsoft.AspNetCore.Components.Forms;

namespace Website.Pages.Tools
{
    public partial class Base64
    {
        private bool ShowValidationFileSize;
        private bool ShowValidationBase64;
        private int MaxSize = 10240000;
        private string Base64Text = string.Empty;
        private string PlainText = string.Empty;
        private string FileName = string.Empty;
        private string FileType = string.Empty;

        private async Task ConvertFile(InputFileChangeEventArgs e)
        {
            Reset();
            PlainText = string.Empty;

            IBrowserFile browserFile = e.File;
            if (Validate(browserFile))
            {
                StreamContent fileContent = new StreamContent(browserFile.OpenReadStream(MaxSize));
                byte[] body = await fileContent.ReadAsByteArrayAsync();
                Base64Text = Convert.ToBase64String(body);
                FileName = browserFile.Name;
                FileType = browserFile.ContentType;
            }
        }

        private void ConvertText()
        {
            Reset();
            if (string.IsNullOrEmpty(PlainText)) return;
            Base64Text = SimpleConverter.ConvertToBase64(PlainText);
        }

        private void ConvertBase64()
        {
            Reset();
            if (string.IsNullOrEmpty(Base64Text)) return;

            try
            {
                PlainText = SimpleConverter.ConvertToString(Base64Text);
            }
            catch (Exception)
            {
                ShowValidationBase64 = true;
            }
        }

        private void DownloadFile()
        {
            Reset();
            if (string.IsNullOrEmpty(Base64Text)) return;

            _blazorDownloadFileService.DownloadFile(FileName, Base64Text, FileType);
        }

        private bool Validate(IBrowserFile browserFile)
        {
            bool valid = browserFile.Size < (MaxSize - 1);
            ShowValidationFileSize = !valid;
            return valid;
        }

        private void Reset()
        {
            ShowValidationBase64 = false;
            ShowValidationFileSize = false;
        }
    }
}
