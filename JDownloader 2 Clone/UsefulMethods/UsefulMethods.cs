using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace JDownloader_2_Clone.UsefulMethods
{
    class UsefulMethods
    {
        public static async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }


        public static async void ErrorMessage(String ErrorMessage)
        {
            ContentDialog error = new ContentDialog();
            error.Content = ErrorMessage;
            error.Title = "Error";
            error.IsSecondaryButtonEnabled = false;
            error.PrimaryButtonText = "Ok";
            ContentDialogResult errorResult = await error.ShowAsync();
        }
    }
}
