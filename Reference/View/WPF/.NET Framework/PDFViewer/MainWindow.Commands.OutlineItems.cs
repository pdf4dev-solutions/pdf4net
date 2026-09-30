using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.View;
using O2S.Components.PDF4NET.View.Content;

namespace PDFViewer
{
	public partial class MainWindow
	{

		private ICommand outlineItemsAddCommand;
		public ICommand OutlineItemsAddCommand
		{
			get
			{
				return outlineItemsAddCommand ?? (outlineItemsAddCommand = new CommandHandler(() => OutlineItemsAddCommandExecute(), () => OutlineItemsAddCommandCanExecute));
			}
		}

		public bool OutlineItemsAddCommandCanExecute
		{
			get { return IsDocumentAvailable; }
		}

		public void OutlineItemsAddCommandExecute()
		{
			BookmarkWindow addBookmarkWindow = new BookmarkWindow();
			addBookmarkWindow.LoadPages(visualDocument.Pages);
			if (addBookmarkWindow.ShowDialog().Value)
			{
				PDFVisualOutlineItemCollection items = outlineView.SelectedOutlineItem == null ? visualDocument.VisualOutline : outlineView.SelectedOutlineItem.Items;
				items.Add(addBookmarkWindow.OutlineItem);
			}
		}

		private ICommand outlineItemsAddBeforeCommand;
		public ICommand OutlineItemsAddBeforeCommand
		{
			get
			{
				return outlineItemsAddBeforeCommand ?? (outlineItemsAddBeforeCommand = new CommandHandler(() => OutlineItemsAddBeforeCommandExecute(), () => OutlineItemsAddBeforeCommandCanExecute));
			}
		}

		public bool OutlineItemsAddBeforeCommandCanExecute
		{
			get { return IsDocumentAvailable && (outlineView.SelectedOutlineItem != null); }
		}

		public void OutlineItemsAddBeforeCommandExecute()
		{
			BookmarkWindow addBookmarkWindow = new BookmarkWindow();
			addBookmarkWindow.LoadPages(visualDocument.Pages);
			if (addBookmarkWindow.ShowDialog().Value)
			{
				PDFVisualOutlineItemCollection items = outlineView.SelectedOutlineItem.Parent == null ? visualDocument.VisualOutline : outlineView.SelectedOutlineItem.Parent.Items;
				items.Insert(items.IndexOf(outlineView.SelectedOutlineItem), addBookmarkWindow.OutlineItem);
			}
		}

		private ICommand outlineItemsAddAfterCommand;
		public ICommand OutlineItemsAddAfterCommand
		{
			get
			{
				return outlineItemsAddAfterCommand ?? (outlineItemsAddAfterCommand = new CommandHandler(() => OutlineItemsAddAfterCommandExecute(), () => OutlineItemsAddAfterCommandCanExecute));
			}
		}

		public bool OutlineItemsAddAfterCommandCanExecute
		{
			get { return IsDocumentAvailable && (outlineView.SelectedOutlineItem != null); }
		}

		public void OutlineItemsAddAfterCommandExecute()
		{
			BookmarkWindow addBookmarkWindow = new BookmarkWindow();
			addBookmarkWindow.LoadPages(visualDocument.Pages);
			if (addBookmarkWindow.ShowDialog().Value)
			{
				PDFVisualOutlineItemCollection items = outlineView.SelectedOutlineItem.Parent == null ? visualDocument.VisualOutline : outlineView.SelectedOutlineItem.Parent.Items;
				items.Insert(items.IndexOf(outlineView.SelectedOutlineItem) + 1, addBookmarkWindow.OutlineItem);
			}
		}

		private ICommand outlineItemsDeleteCommand;
		public ICommand OutlineItemsDeleteCommand
		{
			get
			{
				return outlineItemsDeleteCommand ?? (outlineItemsDeleteCommand = new CommandHandler(() => OutlineItemsDeleteCommandExecute(), () => OutlineItemsDeleteCommandCanExecute));
			}
		}

		public bool OutlineItemsDeleteCommandCanExecute
		{
			get { return IsDocumentAvailable && (outlineView.SelectedOutlineItem != null); }
		}

		public void OutlineItemsDeleteCommandExecute()
		{
			if (MessageBox.Show("Are you sure you want to delete the current bookmark?", ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				PDFVisualOutlineItem selectedItem = outlineView.SelectedOutlineItem;
				if (selectedItem.Parent == null)
				{
					visualDocument.VisualOutline.Remove(selectedItem);
				}
				else
				{
					selectedItem.Parent.Items.Remove(selectedItem);
				}
			}
		}
	}
}
