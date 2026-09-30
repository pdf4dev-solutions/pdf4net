using System;
using System.Collections.Generic;
using System.Windows.Forms;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Actions;
using O2S.Components.PDF4NET.Destinations;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.View;

namespace PDFViewer
{
    public partial class BookmarkForm : Form
    {
        public BookmarkForm()
        {
            InitializeComponent();
        }

        private PDFVisualOutlineItem outlineItem;
        /// <summary>
        /// Gets the outline item created by the user.
        /// </summary>
        public PDFVisualOutlineItem OutlineItem
        {
            get { return outlineItem; }
        }

        public void LoadPages(PDFVisualPageCollection pages)
        {
            var dict = new Dictionary<PDFPage, string>();
            for (int i = 0; i < pages.Count; i++)
            {
                dict.Add(pages[i].Page, $"{i + 1}");
            }

            cbxPageNumber.DataSource = new BindingSource(dict, null);
            cbxPageNumber.DisplayMember = "Value";
            cbxPageNumber.ValueMember = "Key";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            outlineItem = new PDFVisualOutlineItem();
            outlineItem.Title = txtTitle.Text;
            outlineItem.VisualStyle = PDFOutlineItemVisualStyle.Regular;
            if (chkBold.Checked)
            {
                outlineItem.VisualStyle |= PDFOutlineItemVisualStyle.Bold;
            }
            if (chkItalic.Checked)
            {
                outlineItem.VisualStyle |= PDFOutlineItemVisualStyle.Italic;
            }
            outlineItem.Color = new PDFRgbColor(btnColor.BackColor.R, btnColor.BackColor.G, btnColor.BackColor.B);

            PDFPageDirectDestination pdd = new PDFPageDirectDestination();
            pdd.Page = cbxPageNumber.SelectedValue as PDFPage;
            
            PDFGoToAction goToAction = new PDFGoToAction();
            goToAction.Destination = pdd;

            outlineItem.Action = goToAction;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (cd.ShowDialog() == DialogResult.OK)
            {
                btnColor.BackColor = cd.Color;
            }
        }
    }
}
