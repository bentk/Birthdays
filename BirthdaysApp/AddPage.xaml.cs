using System;
using System.Collections.Generic;
using System.Globalization;
using BirthdaysApp.Common;
using BirthdaysApp.Data;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BirthdaysApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AddPage : BirthdaysApp.Common.LayoutAwarePage
    {
        public AddPage()
        {
            this.InitializeComponent();
            
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
           
            var dateFormat = new CultureInfo(GlobalizationPreferences.Languages[0]).DateTimeFormat;
            lbMonths.ItemsSource = dateFormat.MonthNames;
            var days = new List<int>();
            var months= new List<int>();
            for (var i = 1; i < 31; i++)
            {
                days.Add(i);
            }

            for (var i = 1900; i < DateTime.Today.Year; i++)
            {
                months.Add(i);
            }
            lbDay.ItemsSource = days;
            lbYear.ItemsSource = months;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void ImageTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
        private async void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            var ui = new CameraCaptureUI();
            ui.PhotoSettings.CroppedAspectRatio = new Size(4, 3);

            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file != null)
            {
                stream = await file.OpenAsync(FileAccessMode.Read);
                var bitmap = new BitmapImage();
                bitmap.SetSource(stream);
                Photo.Source = bitmap;
            }
        }
        IRandomAccessStream stream;
        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");

            // Open a stream for the selected file
            StorageFile file = await open.PickSingleFileAsync();
            if (file != null)
            {
                stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var bitmap = new BitmapImage();
                bitmap.SetSource(stream);
                Photo.Source = bitmap;
            }
        }

        private async void BackButtonClick1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                new MessageDialog("No person is added because of missing [Name]").ShowAsync();
                GoBack(sender,e);
                return;
            }

            var messageDialog = new MessageDialog("Do you want to store " + txtName.Text + "?");
            
            messageDialog.Commands.Add(new UICommand("Yes", Yes));
            messageDialog.Commands.Add(new UICommand("No", No));

            messageDialog.DefaultCommandIndex = 0;

            messageDialog.CancelCommandIndex = 1;

            await messageDialog.ShowAsync();
            GoBack(sender, e);
        }

        private void Yes(IUICommand command)
        {            
            //var item = SampleDataSource.AddItem(new SampleDataItem(txtName, txtName,"",Photo.BaseUri));

            //this.DefaultViewModel["Group"] = item.Group;
            //this.DefaultViewModel["Items"] = item.Group.Items;

        }
        private void No(IUICommand command)
        {

        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            GoBack(sender,e);
        }

        private void btnOk_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                new MessageDialog("No person is added because of missing [Name]").ShowAsync();
                GoBack(sender, e);
                return;
            }

            //new SampleDataItem()

            
        }
    }
}
