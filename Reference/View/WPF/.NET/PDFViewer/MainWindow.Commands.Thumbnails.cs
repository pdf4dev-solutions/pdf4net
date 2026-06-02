using System.Windows;
using System.Windows.Input;

namespace PDFViewer
{
	public partial class MainWindow
	{

		private ICommand thumbnailsRotate90CCWCommand;
		public ICommand ThumbnailsRotate90CCWCommand
		{
			get
			{
				return thumbnailsRotate90CCWCommand ?? (thumbnailsRotate90CCWCommand = new CommandHandler(() => ThumbnailsRotate90CCWCommandExecute(), () => ThumbnailsRotate90CCWCommandCanExecute));
			}
		}

		public bool ThumbnailsRotate90CCWCommandCanExecute
		{
			get { return IsDocumentAvailable; }
		}

		public void ThumbnailsRotate90CCWCommandExecute()
		{
			int rotation = visualDocument.Pages[thumbnailsView.PageNumber].Rotation;
			rotation -= 90;
			if (rotation < 0)
			{
				rotation += 360;
			}
			visualDocument.Pages[thumbnailsView.PageNumber].Rotation = rotation;
		}

		private ICommand thumbnailsRotate90CWCommand;
		public ICommand ThumbnailsRotate90CWCommand
		{
			get
			{
				return thumbnailsRotate90CWCommand ?? (thumbnailsRotate90CWCommand = new CommandHandler(() => ThumbnailsRotate90CWCommandExecute(), () => ThumbnailsRotate90CWCommandCanExecute));
			}
		}

		public bool ThumbnailsRotate90CWCommandCanExecute
		{
			get { return IsDocumentAvailable; }
		}

		public void ThumbnailsRotate90CWCommandExecute()
		{
			int rotation = visualDocument.Pages[thumbnailsView.PageNumber].Rotation;
			rotation += 90;
			rotation %= 360;
			visualDocument.Pages[thumbnailsView.PageNumber].Rotation = rotation;
		}

		private ICommand thumbnailsDeleteCommand;
		public ICommand ThumbnailsDeleteCommand
		{
			get
			{
				return thumbnailsDeleteCommand ?? (thumbnailsDeleteCommand = new CommandHandler(() => ThumbnailsDeleteCommandExecute(), () => ThumbnailsDeleteCommandCanExecute));
			}
		}

		public bool ThumbnailsDeleteCommandCanExecute
		{
			get { return visualDocument.Pages.Count > 1; }
		}

		public void ThumbnailsDeleteCommandExecute()
		{
			if (MessageBox.Show("Are you sure you want to delete the current page?", ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				visualDocument.Pages.RemoveAt(thumbnailsView.PageNumber);
			}
		}
	}
}
