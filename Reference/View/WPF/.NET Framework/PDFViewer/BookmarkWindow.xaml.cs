using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Actions;
using O2S.Components.PDF4NET.Destinations;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.View;

namespace PDFViewer
{
    /// <summary>
    /// Interaction logic for BookmarkWindow.xaml
    /// </summary>
    public partial class BookmarkWindow : Window
    {
        private class PageNumbers
        {
            public int Number {  get; set; }

            public PDFVisualPage VisualPage { get; set; }
        }

        public BookmarkWindow()
        {
            InitializeComponent();
        }

        private PDFVisualOutlineItem outlineItem;
        public PDFVisualOutlineItem OutlineItem
        {
            get { return outlineItem; } 
        }
        public void LoadPages(PDFVisualPageCollection pages)
        {
            List<PageNumbers> numbers = new List<PageNumbers>();
            for (int i = 0; i < pages.Count; i++)
            {
                numbers.Add(new PageNumbers() { Number = i + 1, VisualPage = pages[i] });
            }

            cbxPage.ItemsSource = numbers;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            outlineItem = new PDFVisualOutlineItem();
            outlineItem.Title = txtTitle.Text;
            outlineItem.VisualStyle = PDFOutlineItemVisualStyle.Regular;
            if (chkBold.IsChecked.Value)
            {
                outlineItem.VisualStyle |= PDFOutlineItemVisualStyle.Bold;
            }
            if (chkItalic.IsChecked.Value)
            {
                outlineItem.VisualStyle |= PDFOutlineItemVisualStyle.Italic;
            }

            Color clr = ((cbxColor.SelectedItem as Rectangle).Fill as SolidColorBrush).Color;
            outlineItem.Color = new PDFRgbColor(clr.R, clr.G, clr.B);

            PDFPageDirectDestination pdd = new PDFPageDirectDestination();
            pdd.Page = (cbxPage.SelectedValue as PageNumbers).VisualPage.Page;

            PDFGoToAction goToAction = new PDFGoToAction();
            goToAction.Destination = pdd;

            outlineItem.Action = goToAction;

            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
