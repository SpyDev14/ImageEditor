using Editor.Extensions.Point;
using System.Collections.Immutable;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Editor;

public partial class MainForm : Form
{
	const int NEW_WORKSPACE_HORIZONTAL_PADDING = 24;
	const int NEW_WORKSPACE_VERTICAL_PADDING   = 16;

	static readonly ImmutableArray<(ImageFormat ImageFormat, string Name, ImmutableArray<string> Extensions)> SupportedFileFormats = [
		(ImageFormat.Png,  "PNG",    ["png"]),
		(ImageFormat.Jpeg, "JPEG",   ["jpg", "jpeg", "jpe", "jfif"]),
		(ImageFormat.Bmp,  "Bitmap", ["bmp"]),
		(ImageFormat.Gif,  "GIF",    ["gif"]),
		(ImageFormat.Icon, "Icon",   ["ico"]),
		(ImageFormat.Tiff, "TIFF",   ["tiff", "tif"])
	];
	static readonly ImageFormat DefaultFileFormat = ImageFormat.Png;
	static readonly ImmutableDictionary<string, ImageFormat> ExtensionToFileFormat;


	Bitmap _bm;
	Graphics _gfx;

	Point _currentMousePos;
	Pen _pen;
	bool _isDrawing = false;

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


	static readonly string SaveDialogFilter;
	static readonly string OpenDialogFilter;
	static string _OpenDialogFilter
	{
		get
		{
			if (field == null)
			{
				var allExtensions = SupportedFileFormats.SelectMany(x => x.Extensions).Select(x => $"*.{x}");

				field = $"Image Files ({string.Join(", ", allExtensions)})|{string.Join(";", allExtensions)}";
			}
			return field;
		}
	}
	static string _SaveDialogFilter
	{
		get
		{
			if (field == null)
			{
				var variants = new string[SupportedFileFormats.Length];

				for (int i = 0; i < SupportedFileFormats.Length; i++)
				{
					var fileType = SupportedFileFormats[i];

					var displayedPart = $"{fileType.Name} ({string.Join(", ", fileType.Extensions.Select(x => $"*.{x}"))})";
					var filterPart = string.Join(";", fileType.Extensions.Select(x => $"*.{x}"));

					variants[i] = $"{displayedPart}|{filterPart}";
				}

				field = string.Join('|', variants);
			}
			return field;
		}
	}

	static MainForm()
	{
		ExtensionToFileFormat = ImmutableDictionary.CreateRange(
			StringComparer.OrdinalIgnoreCase,
			SupportedFileFormats
				.SelectMany(suppFormat => suppFormat.Extensions.Select(ext => (ext, suppFormat.ImageFormat)))
				.Select(pair => new KeyValuePair<string, ImageFormat>(pair.ext, pair.ImageFormat))
		);

		SaveDialogFilter = string.Join('|', SupportedFileFormats.Select(f => {
			var formattedExts = f.Extensions.Select(ext => $"*.{ext}").ToArray();
			return $"{f.Name} ({string.Join(", ", formattedExts)})|{string.Join(";", formattedExts)}";
		}));

	
		var allExtsFormatted = SupportedFileFormats.SelectMany(x => x.Extensions).Select(x => $"*.{x}");
		OpenDialogFilter = $"Image Files ({string.Join(", ", allExtsFormatted)})|{string.Join(";", allExtsFormatted)}";
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
		openedPicture.Width  = _bm.Width;
		openedPicture.Location = new Point(
			(workspacePanel.Width  - _bm.Width)  / 2,
			(workspacePanel.Height - _bm.Height) / 2
		);

		workspacePanel.AutoScrollMinSize = openedPicture.Size;
	}

	void NewImage()
	{
		SetImage(new Bitmap(
			workspacePanel.Width  - NEW_WORKSPACE_HORIZONTAL_PADDING * 2,
			workspacePanel.Height - NEW_WORKSPACE_VERTICAL_PADDING   * 2
		));
		_gfx.Clear(Color.White);
	}

	void SaveAs(string filename)
	{
		ImageFormat format = DefaultFileFormat;


		_bm.Save(filename, format);
		_openedFileName = filename;
	}

	void SaveAs()
	{
		SaveFileDialog dialog = new() { Filter = _SaveDialogFilter };
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
		OpenFileDialog dialog = new() { Filter = _OpenDialogFilter };

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
