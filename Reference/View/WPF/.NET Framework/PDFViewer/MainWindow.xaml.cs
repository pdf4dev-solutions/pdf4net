using Microsoft.Win32;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.View;
using O2S.Components.PDF4NET.View.Content;
using O2S.Components.PDF4NET.View.Layouts;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDFViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            visualDocument = new PDFVisualDocument();

            InitializeComponent();
            documentView.Document = visualDocument;
            contentLocator = new PDFVisualContentLocator(documentView);
            isSearchInitialized = false;
        }

        private PDFVisualDocument visualDocument;

        private Activity currentActivity;
        public Activity CurrentActivity
        { 
            get { return currentActivity; } 
        }

        private bool IsDocumentAvailable
        {
            get
            {
                return visualDocument.Pages.Count > 0;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            visualDocument.Load(new PDFFixedDocument("..\\..\\..\\..\\..\\..\\..\\SupportFiles\\PDF4NET.View.pdf"));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            documentView.Dispose();
            visualDocument.Dispose();
        }

        private ICommand openPDFCommand;
        public ICommand OpenPDFCommand
        {
            get
            {
                return openPDFCommand ?? (openPDFCommand = new CommandHandler(() => OpenPDFCommandExecute(), () => OpenPDFCommandCanExecute));
            }
        }

        public bool OpenPDFCommandCanExecute
        {
            get { return true; }
        }

        public void OpenPDFCommandExecute()
        {
            ContextMenu openCtxMenu = FindResource("OpenFileContextMenu") as ContextMenu;
            openCtxMenu.PlacementTarget = btnOpen;
            openCtxMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            openCtxMenu.IsOpen = true;
        }

        private ICommand openIncrementalPDFCommand;
        public ICommand OpenIncrementalPDFCommand
        {
            get
            {
                return openIncrementalPDFCommand ?? (openIncrementalPDFCommand = new CommandHandler(() => OpenIncrementalPDFCommandExecute(), () => OpenIncrementalPDFCommandCanExecute));
            }
        }

        public bool OpenIncrementalPDFCommandCanExecute
        {
            get { return true; }
        }

        public void OpenIncrementalPDFCommandExecute()
        {
            // In incremental mode the viewer loads only the required parts for display.
            // Incremental load mode is achieved by loading the source file directly in the viewer.
            // The source PDF file is kept open and any changes on the PDF file must be saved in the source file as they are written in incremental update mode.

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".pdf";
            ofd.Filter = "PDF files (.pdf)|*.pdf|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                visualDocument.Load(ofd.FileName);
                UpdateLayoutButtons();
                UpdateCurrentActivity(Activity.PanAndScan);
            }
        }

        private ICommand openFullPDFCommand;
        public ICommand OpenFullPDFCommand
        {
            get
            {
                return openFullPDFCommand ?? (openFullPDFCommand = new CommandHandler(() => OpenFullPDFCommandExecute(), () => OpenFullPDFCommandCanExecute));
            }
        }

        public bool OpenFullPDFCommandCanExecute
        {
            get { return true; }
        }

        public void OpenFullPDFCommandExecute()
        {
            // In full mode the viewer loads the whole file in memory.
            // Full load mode is achieved by loading the source file in a PdfFixedDocument and then loading the fixed document in the viewer.
            // The source PDF file can be moved/deleted and any changes on the PDF file can be saved to any file as the PDF file is written in full.

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".pdf";
            ofd.Filter = "PDF files (.pdf)|*.pdf|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                visualDocument.Load(new PDFFixedDocument(ofd.FileName));
                UpdateLayoutButtons();
                UpdateCurrentActivity(Activity.PanAndScan);
            }
        }

        private ICommand savePDFCommand;
        public ICommand SavePDFCommand
        {
            get
            {
                return savePDFCommand ?? (savePDFCommand = new CommandHandler(() => SavePDFCommandExecute(), () => SavePDFCommandCanExecute));
            }
        }

        public bool SavePDFCommandCanExecute
        {
            get { return IsDocumentAvailable; }
        }

        public void SavePDFCommandExecute()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".pdf";
            sfd.Filter = "PDF files (.pdf)|*.pdf|All files (*.*)|*.*";

            if (sfd.ShowDialog() == true)
            {
                visualDocument.Save(sfd.FileName);
            }
        }

        private ICommand closePDFCommand;
        public ICommand ClosePDFCommand
        {
            get
            {
                return closePDFCommand ?? (closePDFCommand = new CommandHandler(() => ClosePDFCommandExecute(), () => ClosePDFCommandCanExecute));
            }
        }

        public bool ClosePDFCommandCanExecute
        {
            get { return IsDocumentAvailable; }
        }

        public void ClosePDFCommandExecute()
        {
            visualDocument.Close();
            UpdateLayoutButtons();
        }

        private ICommand zoomFitWidthCommand;
        public ICommand ZoomFitWidthCommand
        {
            get
            {
                return zoomFitWidthCommand ?? (zoomFitWidthCommand = new CommandHandler(() => ZoomFitWidthCommandExecute(), () => ZoomFitWidthCommandCanExecute));
            }
        }

        public bool ZoomFitWidthCommandCanExecute
        {
            get { return IsDocumentAvailable; }
        }

        public void ZoomFitWidthCommandExecute()
        {
            documentView.ZoomMode = PDFZoomMode.FitWidth;
        }

        public bool IsSingleColumnLayout
        {
            get { return IsDocumentAvailable && (documentView.PageDisplayLayout is PDFColumnBasedPageDisplayLayout); }
        }

        private ICommand layoutSingleColumnCommand;
        public ICommand LayoutSingleColumnCommand
        {
            get
            {
                return layoutSingleColumnCommand ?? (layoutSingleColumnCommand = new CommandHandler(() => LayoutSingleColumnCommandExecute(), () => LayoutSingleColumnCommandCanExecute));
            }
        }

        public bool LayoutSingleColumnCommandCanExecute
        {
            get { return IsDocumentAvailable; }
        }

        public void LayoutSingleColumnCommandExecute()
        {
            if (!IsSingleColumnLayout)
            {
                documentView.PageDisplayLayout = new PDFColumnBasedPageDisplayLayout();
            }
            UpdateLayoutButtons();
        }

        public bool IsSingleRowLayout
        {
            get { return IsDocumentAvailable && (documentView.PageDisplayLayout is PDFRowBasedPageDisplayLayout); }
        }

        private ICommand layoutSingleColumnRow;
        public ICommand LayoutSingleRowCommand
        {
            get
            {
                return layoutSingleColumnRow ?? (layoutSingleColumnRow = new CommandHandler(() => LayoutSingleRowCommandExecute(), () => LayoutSingleRowCommandCanExecute));
            }
        }

        public bool LayoutSingleRowCommandCanExecute
        {
            get { return IsDocumentAvailable; }
        }

        public void LayoutSingleRowCommandExecute()
        {
            if (!IsSingleRowLayout)
            {
                documentView.PageDisplayLayout = new PDFRowBasedPageDisplayLayout();
            }
            UpdateLayoutButtons();
        }

        private void UpdateLayoutButtons()
        {
            btnLayoutSingleColumn.IsChecked = IsSingleColumnLayout;
            btnLayoutSingleRow.IsChecked = IsSingleRowLayout;
        }

        private void documentView_UserInteractionModeChanged(object sender, EventArgs e)
        {
            btnAnnotationsEdit.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.EditAnnotations;
            btnAnnotationsText.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddTextAnnotation;
            btnAnnotationsStamp.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddRubberStampAnnotation;
            btnAnnotationsCircle.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddCircleAnnotation;
            btnAnnotationsSquare.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddSquareAnnotation;
            btnAnnotationsCloudSquare.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddCloudSquareAnnotation;
            btnAnnotationsLine.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddLineAnnotation;
            btnAnnotationsPolyline.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddPolylineAnnotation;
            btnAnnotationsPolygon.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddPolygonAnnotation;
            btnAnnotationsCloudPolygon.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddCloudPolygonAnnotation;
            btnAnnotationsInk.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddInkAnnotation;
            btnAnnotationsLink.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddLinkAnnotation;
            btnAnnotationsFileAttachment.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddFileAttachmentAnnotation;
            btnAnnotationsFreeText.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddFreeTextAnnotation;

            btnFormFieldsEdit.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.EditFormFields;
            btnFormFieldsAddTextBox.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddTextBoxField;
            btnFormFieldsAddCheckBox.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddCheckBoxField;
            btnFormFieldsAddRadioButton.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddRadioButtonField;
            btnFormFieldsAddComboBox.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddComboBoxField;
            btnFormFieldsAddListBox.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddListBoxField;
            btnFormFieldsAddButton.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddPushButtonField;
            btnFormFieldsAddSignature.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.AddSignatureField;

            btnTextHighlight.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.HighlightText;
            btnTextSquiggly.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.SquigglyUnderlineText;
            btnTextStrikeout.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.StrikeoutText;
            btnTextUnderline.IsChecked = documentView.UserInteractionMode == PDFUserInteractionMode.FlatUnderlineText;
        }

        private void documentView_BeforePageDelete(object sender, PDFVisualPageDeleteEventArgs e)
        {
            e.AllowDelete = MessageBox.Show("Are you sure you want to delete the current page?", "PDF4NET - PDF Viewer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private void documentView_BeforeAnnotationDelete(object sender, PDFVisualAnnotationDeleteEventArgs e)
        {
            e.AllowDelete = MessageBox.Show("Are you sure you want to delete the selected annotation?", "PDF4NET - PDF Viewer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private void cbxZoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (documentView != null)
            {
                ComboBoxItem item = cbxZoom.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    string selectedZoom = item.Content.ToString().Trim();
                    documentView.Zoom = GetZoom(item.Content.ToString().Trim());
                }
            }
        }

        private void cbxZoom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                documentView.Zoom = GetZoom(cbxZoom.Text.Trim());
            }
        }

        private void documentView_ZoomChanged(object sender, EventArgs e)
        {
            cbxZoom.Text = documentView.Zoom.ToString() + "%";
        }

        private int GetZoom(string userZoom)
        {
            if (userZoom.EndsWith("%"))
            {
                userZoom = userZoom.TrimEnd('%');
            }
            if (!int.TryParse(userZoom, NumberStyles.Integer, CultureInfo.InvariantCulture, out int zoom))
            {
                zoom = 100;
            }

            return zoom;
        }
    }
}
