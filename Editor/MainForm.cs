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
		workspacePanel.MouseWheel += workspacePanel_MouseWheel;

		WorkspaceNegativeMargin = -Math.Min(workspacePanel.Location.X, 0);


		_pen = new Pen(SelectedColor)
		{
			StartCap = LineCap.Round,
			EndCap = LineCap.Round,
			LineJoin = LineJoin.Round,
		};

		selectedColorPanel.BackColor = SelectedColor;

		penWidthTrackBar.Minimum = MIN_PEN_WIDTH;
		penWidthTrackBar.Maximum = MAX_PEN_WIDTH;

		picture.SizeMode = PictureBoxSizeMode.StretchImage;


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

		_zoom = 1.0f;
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
			FileName = $"New image"
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

	void Open()
	{
		OpenFileDialog dialog = new() { Filter = OpenDialogFilter };

		if (dialog.ShowDialog() != DialogResult.OK)
			return;

		SetImage(Image.FromFile(dialog.FileName));
		_openedFileName = dialog.FileName;
	}

	void SetZoom(float newZoom, Point mouseWorkspace)
	{
		newZoom = Math.Clamp(newZoom, ZOOM_MIN, ZOOM_MAX);
		if (Math.Abs(newZoom - _zoom) < 0.001f) return;

		PointF imageCoord = new PointF(
			(mouseWorkspace.X - picture.Location.X) / _zoom,
			(mouseWorkspace.Y - picture.Location.Y) / _zoom
		);

		_zoom = newZoom;

		Size newSize = new Size((int)(_bm.Width * _zoom), (int)(_bm.Height * _zoom));
		picture.Size = newSize;

		int newX = (int)(mouseWorkspace.X - imageCoord.X * _zoom);
		int newY = (int)(mouseWorkspace.Y - imageCoord.Y * _zoom);

		var minX = -newSize.Width + MIN_VISIBLE_PICTURE_PART;
		var minY = -newSize.Height + MIN_VISIBLE_PICTURE_PART;
		var maxX = workspacePanel.ClientSize.Width - MIN_VISIBLE_PICTURE_PART;
		var maxY = workspacePanel.ClientSize.Height - MIN_VISIBLE_PICTURE_PART;

		picture.Location = new Point(
			Math.Clamp(newX, minX, maxX),
			Math.Clamp(newY, minY, maxY)
		);

		picture.Invalidate();
	}

	Point ScreenToBitmap(Point screenPoint)
	{
		return new Point(
			(int)((screenPoint.X - picture.Location.X) / _zoom),
			(int)((screenPoint.Y - picture.Location.Y) / _zoom)
		);
	}

	// EVENTS //
	private void openToolStripMenuItem_Click(object sender, EventArgs e) => Open();

	// Workspace //
	private void workspacePanel_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			_isDrawning = true;
			Point bitmapLoc = ScreenToBitmap(e.Location);
			using (var brush = new SolidBrush(SelectedColor))
			{
				float diameter = _pen.Width;
				float radius = diameter / 2;
				float x = bitmapLoc.X - radius;
				float y = bitmapLoc.Y - radius;

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
		Point bitmapLoc = ScreenToBitmap(e.Location);
		bool isCursorOnPicture = bitmapLoc.X >= 0 && bitmapLoc.X < _bm.Width &&
								 bitmapLoc.Y >= 0 && bitmapLoc.Y < _bm.Height;

		if (_isDrawning && (isCursorOnPicture || _beInPictureOnLastFrame))
		{
			Point prevBitmap = ScreenToBitmap(_previousMousePos);
			Point currBitmap = ScreenToBitmap(e.Location);
			_gfx.DrawLine(_pen, prevBitmap, currBitmap);
			picture.Invalidate();
			_beInPictureOnLastFrame = isCursorOnPicture;
		}
		_previousMousePos = e.Location;

		if (_isDragging)
		{
			var delta = e.Location.Sub(_dragStartMousePos);

			var newX = _dragStartPictureLocation.X + delta.X;
			var newY = _dragStartPictureLocation.Y + delta.Y;

			var minX = -picture.Width + MIN_VISIBLE_PICTURE_PART;
			var minY = -picture.Height + MIN_VISIBLE_PICTURE_PART;

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

	private void workspacePanel_MouseWheel(object? sender, MouseEventArgs e)
	{
		float delta = e.Delta > 0 ? ZOOM_FACTOR : 1f / ZOOM_FACTOR;
		float newZoom = _zoom * delta;
		SetZoom(newZoom, e.Location);
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

	private void centerCameraToolStripMenuItem_Click(object sender, EventArgs e) => CenterImage();

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
