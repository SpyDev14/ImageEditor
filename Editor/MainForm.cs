using Editor.Extensions.Point;
using System.Collections.Immutable;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Editor;

public partial class MainForm : Form
{
	const int NEW_WORKSPACE_HORIZONTAL_PADDING = 24;
	const int NEW_WORKSPACE_VERTICAL_PADDING = 16;

	static readonly ImmutableArray<(string Name, ImmutableArray<string> Extensions)> supportedFileTypes = [
		("PNG",    ["png"]),
		("JPEG",   ["jpg", "jpeg", "jpe", "jfif"]),
		("Bitmap", ["bmp"]),
		("GIF",    ["gif"]),
		("Icon",   ["ico"]),
		("TIFF",   ["tiff", "tif"])
	];


	Bitmap _bm;
	Graphics _gfx;

	Point _currentMousePos;
	Pen _pen;
	bool _isDrawing = false;

	bool _isMoving = false;

	string? _openedFileName;

	Color SelectedColor
	{
		get => field;
		set
		{
			_pen.Color = value;
			selectedColorPanel.BackColor = value;
			field = value;
		}
	} = Color.Black;


	static string OpenDialogFilter
	{
		get
		{
			if (field == null)
			{
				var allExtensions = supportedFileTypes.SelectMany(x => x.Extensions).Select(x => $"*.{x}");

				field = $"Image Files ({string.Join(", ", allExtensions)})|{string.Join(";", allExtensions)}";
			}
			return field;
		}
	}
	static string SaveDialogFilter
	{
		get
		{
			if (field == null)
			{
				var variants = new string[supportedFileTypes.Length - 1];

				for (int i = 0; i < supportedFileTypes.Length; i++)
				{
					var fileType = supportedFileTypes[i];

					var displayedPart = $"{fileType.Name} ({string.Join(", ", fileType.Extensions.Select(x => $"*.{x}"))})";
					var filterPart = string.Join(";", fileType.Extensions.Select(x => $"*.{x}"));

					variants[i] = $"{displayedPart}|{filterPart}";
				}

				field = string.Join('|', variants);
			}
			return field;
		}
	}

	public MainForm()
	{
		InitializeComponent();

		selectedColorPanel.BackColor = SelectedColor;
		_pen = new Pen(SelectedColor)
		{
			StartCap = LineCap.Round,
			EndCap = LineCap.Round,
			LineJoin = LineJoin.Round,
		};

		NewImage();
		if (_gfx == null) // Почему нет assert & assert error?!
			throw new ArgumentNullException(nameof(_gfx));
		if (_bm == null)
			throw new ArgumentNullException(nameof(_bm));
	}

	void SetImage(Image img)
	{
		_bm = new(img);
		_gfx = Graphics.FromImage(_bm);
		_gfx.SmoothingMode = SmoothingMode.AntiAlias;
		openedPicture.Image = _bm;

		openedPicture.Height = _bm.Height;
		openedPicture.Width = _bm.Width;
		openedPicture.Location = new Point(
			(workspacePanel.Width - _bm.Width) / 2,
			(workspacePanel.Height - _bm.Height) / 2
		);

		workspacePanel.AutoScrollMinSize = openedPicture.Size;
	}

	void NewImage()
	{
		SetImage(new Bitmap(
			workspacePanel.Width - NEW_WORKSPACE_HORIZONTAL_PADDING * 2,
			workspacePanel.Height - NEW_WORKSPACE_VERTICAL_PADDING * 2
		));
		_gfx.Clear(Color.White);
	}

	void SaveAs(string filename)
	{
		_openedFileName = filename;
	}

	void SaveAs()
	{
		SaveFileDialog dialog = new() { Filter = SaveDialogFilter };
		if (dialog.ShowDialog() != DialogResult.OK)
			return;

		SaveAs(dialog.FileName);
	}

	void Save()
	{
		if (_openedFileName == null)
		{
			SaveAs();
			return;
		}

		SaveAs(_openedFileName);
	}

	// EVENTS //
	// Open //
	private void openToolStripMenuItem_Click(object sender, EventArgs e)
	{
		OpenFileDialog dialog = new() { Filter = OpenDialogFilter };

		if (dialog.ShowDialog() != DialogResult.OK)
			return;

		SetImage(Image.FromFile(dialog.FileName));
	}

	// Workspace //
	private void workspacePanel_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left)
			return;

		_isDrawing = true;
		_currentMousePos = e.Location;
	}

	private void workspacePanel_MouseMove(object sender, MouseEventArgs e)
	{
		if (!_isDrawing)
			return;

		var loc = e.Location.Sub(openedPicture.Location);

		if (loc.Y >= 0 && loc.X < _bm.Width && loc.Y >= 0 && loc.Y < _bm.Height)
		{
			_gfx.DrawLine(_pen, _currentMousePos.Sub(openedPicture.Location), loc);
			openedPicture.Invalidate();
		}
		_currentMousePos = e.Location;
	}

	private void workspacePanel_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
			_isDrawing = false;
	}

	private void penWidthTrackBar_ValueChanged(object sender, EventArgs e)
	{
		_pen.Width = penWidthTrackBar.Value;
	}

	private void newToolStripMenuItem_Click(object sender, EventArgs e) => NewImage();

	private void selectedColorPanel_Click(object sender, EventArgs e)
	{
		ColorDialog dialog = new();

		var result = dialog.ShowDialog();
		if (result != DialogResult.OK)
			return;

		SelectedColor = dialog.Color;
	}

	private void saveToolStripMenuItem_Click(object sender, EventArgs e) => Save();

	private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) => SaveAs();
}
