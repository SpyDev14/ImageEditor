using Editor.Extensions.ForPoint;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Editor;

public partial class MainForm : Form
{
	const int NEW_WORKSPACE_HORIZONTAL_PADDING = 24;
	const int NEW_WORKSPACE_VERTICAL_PADDING = 16;
	readonly int WorkspaceNegativeMargin;

	const int MIN_VISIBLE_PICTURE_PART = 48;

	const int MIN_PEN_WIDTH = 1;
	const int MAX_PEN_WIDTH = 10;

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

	static readonly string SaveDialogFilter;
	static readonly string OpenDialogFilter;

	Bitmap _bm;
	Graphics _gfx;

	Pen _pen;
	Point _previousMousePos;
	bool _isDrawning = false;
	bool _beInPictureOnLastFrame = false;

	Point _dragStartMousePos;
	Point _dragStartPictureLocation;
	bool _isDragging = false;

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

	const float ZOOM_MIN = 0.1f;
	const float ZOOM_MAX = 10f;
	const float ZOOM_FACTOR = 1.2f;

	float _zoom = 1f;

	static MainForm()
	{
		ExtensionToFileFormat = ImmutableDictionary.CreateRange(
			StringComparer.OrdinalIgnoreCase,
			SupportedFileFormats
				.SelectMany(suppFormat => suppFormat.Extensions.Select(ext => (ext, suppFormat.ImageFormat)))
				.Select(pair => new KeyValuePair<string, ImageFormat>(pair.ext, pair.ImageFormat))
		);

		SaveDialogFilter = string.Join('|', SupportedFileFormats.Select(f =>
		{
			var formattedExts = f.Extensions.Select(ext => $"*.{ext}").ToArray();
			return $"{f.Name} ({string.Join(", ", formattedExts)})|{string.Join(";", formattedExts)}";
		}));

		var allExtsFormatted = SupportedFileFormats.SelectMany(x => x.Extensions).Select(x => $"*.{x}");
		OpenDialogFilter = $"Image Files ({string.Join(", ", allExtsFormatted)})|{string.Join(";", allExtsFormatted)}";
	}

	public MainForm()
	{
		InitializeComponent();

		FormClosed += (s, e) =>
		{
			_bm?.Dispose();
			_gfx?.Dispose();
			_pen?.Dispose();
		};


		selectedColorPanel.BackColor = SelectedColor;
		_pen = new Pen(SelectedColor)
		{
			StartCap = LineCap.Round,
			EndCap = LineCap.Round,
			LineJoin = LineJoin.Round,
		};

		penWidthTrackBar.Minimum = MIN_PEN_WIDTH;
		penWidthTrackBar.Maximum = MAX_PEN_WIDTH;

		WorkspaceNegativeMargin = -Math.Min(workspacePanel.Location.X, 0);

		NewImage();

		Debug.Assert(_gfx != null);
		Debug.Assert(_bm != null);
	}

	void SetImage(Image img)
	{
		_gfx?.Dispose();
		_bm?.Dispose();

		_bm = new(img);
		_gfx = Graphics.FromImage(_bm);
		_gfx.SmoothingMode = SmoothingMode.AntiAlias;
		picture.Image = _bm;

		picture.Size = _bm.Size;

		CenterImage();
	}

	void NewImage()
	{
		SetImage(new Bitmap(
			workspacePanel.Width - NEW_WORKSPACE_HORIZONTAL_PADDING * 2,
			workspacePanel.Height - NEW_WORKSPACE_VERTICAL_PADDING * 2
		));
		_gfx.Clear(Color.White);
		_openedFileName = null;
	}

	void CenterImage()
	{
		picture.Location = new Point(
			((workspacePanel.Width - _bm.Width) / 2) + WorkspaceNegativeMargin,
			((workspacePanel.Height - _bm.Height) / 2) + WorkspaceNegativeMargin
		);
	}

	void SaveAs(string filename)
	{
		var ext = Path.GetExtension(filename);

		ExtensionToFileFormat.TryGetValue(ext, out var format);
		format ??= DefaultFileFormat;

		_bm.Save(filename, format);
		_openedFileName = filename;
	}

	void SaveAs()
	{
		SaveFileDialog dialog = new()
		{
			Filter = SaveDialogFilter,
			FileName = $"New image ({DateTime.Now})"
		};
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
		if (e.Button == MouseButtons.Left)
		{
			_isDrawning = true;
			var loc = e.Location.Sub(picture.Location);
			using (var brush = new SolidBrush(SelectedColor))
			{
				float diameter = _pen.Width;
				float radius = diameter / 2;
				float x = loc.X - radius - 0.5f;
				float y = loc.Y - radius - 0.5f;

				_gfx.FillEllipse(brush, x, y, diameter, diameter);
			}
			picture.Invalidate();
			_previousMousePos = e.Location;
		}
		else if (e.Button == MouseButtons.Middle)
		{
			_isDragging = true;
			_dragStartPictureLocation = picture.Location;
			_dragStartMousePos = e.Location;
			// А мне пихую
			workspacePanel.Cursor = Cursors.SizeAll;
		}
	}


	private void workspacePanel_MouseMove(object sender, MouseEventArgs e)
	{
		var loc = e.Location.Sub(picture.Location);
		bool isCursorOnPicture = loc.Y >= 0 && loc.Y < picture.Height &&
								 loc.X >= 0 && loc.X < picture.Width;

		if (_isDrawning && (isCursorOnPicture || _beInPictureOnLastFrame))
		{
			_gfx.DrawLine(_pen, _previousMousePos.Sub(picture.Location), loc);
			picture.Invalidate();
			_beInPictureOnLastFrame = isCursorOnPicture;
		}
		_previousMousePos = e.Location;

		if (_isDragging)
		{
			var delta = e.Location.Sub(_dragStartMousePos);

			var newX = _dragStartPictureLocation.X + delta.X;
			var newY = _dragStartPictureLocation.Y + delta.Y;

			var minX = (-picture.Width) + MIN_VISIBLE_PICTURE_PART;
			var minY = (-picture.Height) + MIN_VISIBLE_PICTURE_PART;

			var maxX = workspacePanel.ClientSize.Width - MIN_VISIBLE_PICTURE_PART;
			var maxY = workspacePanel.ClientSize.Height - MIN_VISIBLE_PICTURE_PART;

			picture.Location = new Point(
				Math.Clamp(newX, minX, maxX),
				Math.Clamp(newY, minY, maxY)
			);
		}

		var requiredCursor = _isDragging ? Cursors.SizeAll : (isCursorOnPicture ? Cursors.Cross : Cursors.Default);
		if (workspacePanel.Cursor != requiredCursor)
			workspacePanel.Cursor = requiredCursor;
	}

	private void workspacePanel_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
			_isDrawning = false;
		else if (e.Button == MouseButtons.Middle)
			_isDragging = false;
	}

	private void penWidthTrackBar_ValueChanged(object sender, EventArgs e)
	{
		_pen.Width = penWidthTrackBar.Value;
	}

	private void newToolStripMenuItem_Click(object sender, EventArgs e) => NewImage();

	private void selectedColorPanel_Click(object sender, EventArgs e)
	{
		ColorDialog dialog = new();
		if (dialog.ShowDialog() != DialogResult.OK)
			return;

		SelectedColor = dialog.Color;
	}

	private void saveToolStripMenuItem_Click(object sender, EventArgs e) => Save();

	private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) => SaveAs();

	private void centerCameraToolStripMenuItem_Click(object sender, EventArgs e)
		=> CenterImage();

	private void withParamsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		var form = new NewImageForm();
		form.Show();
	}

	private void turnToGrayEffectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		for (int x = 0; x < _bm.Width; x++)
			for (int y = 0; y < _bm.Height; y++)
			{
				var pixel = _bm.GetPixel(x, y);
				var grey = (pixel.R + pixel.B + pixel.G) / 3;
				_bm.SetPixel(x, y, Color.FromArgb(255, grey, grey, grey));
			}
		picture.Invalidate();
		Cursor = Cursors.Default;
	}

	private void MainForm_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.S)
		{
			Save();
			e.SuppressKeyPress = true;
		}
	}

	private void MainForm_ResizeBegin(object sender, EventArgs e) => CenterImage();
}
