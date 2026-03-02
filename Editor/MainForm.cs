using Editor.Extensions.ForPoint;
using Editor.Extensions.ForSize;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Editor;

public partial class MainForm : Form
{
	const int NEW_IMG_WORKSPACE_HORIZONTAL_PADDING = 24;
	const int NEW_IMG_WORKSPACE_VERTICAL_PADDING = 16;
	const int OPENED_IMG_WORKSPACE_PADDING = 16;

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
	bool _isDrawing = false;
	bool _cursorBeOnPictureInLastFrame = false;

	Point _dragStartMousePos;
	Point _dragStartPictureLocation;
	bool IsDragging
	{
		get => field;
		set
		{
			if (value)
				StopCenterImageAnimation();
			field = value;
		}
	} = false;

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
	const float ZOOM_EPSILON = 0.001f;

	float _zoom = 1f;

	/// <summary>
	/// Ms per second for 1 tick (1000 ms / Count in frame)
	/// </summary>
	const int ANIMATION_INTERVAL_MS = 1000 / 160;
	const double ANIMATION_SLOWDOWN_FACTOR = 0.125;
	const double ANIMATION_DISTANCE_EPSILON = 1.0;

	readonly object _animLock = new object();
	CancellationTokenSource _animCts = new();
	Point? _animTarget;

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
			_animCts?.Cancel();
			_animCts?.Dispose();

			_bm?.Dispose();
			_gfx?.Dispose();
			_pen?.Dispose();
		};
		workspacePanel.MouseWheel += workspacePanel_MouseWheel;
		selectedColorPanel.BackColor = SelectedColor;

		penWidthTrackBar.Minimum = MIN_PEN_WIDTH;
		penWidthTrackBar.Maximum = MAX_PEN_WIDTH;

		picture.SizeMode = PictureBoxSizeMode.StretchImage;

		WorkspaceNegativeMargin = -Math.Min(workspacePanel.Location.X, 0);
		_pen = new Pen(SelectedColor)
		{
			StartCap = LineCap.Round,
			EndCap = LineCap.Round,
			LineJoin = LineJoin.Round,
		};
		Shown += (s, e) => Task.Run(CenterImageAnimationLoop);


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

		_zoom = 1f;
		picture.Size = _bm.Size;

		// Хотел бы тут ещё zoom подбирать такой, чтобы картинка влезала
		// Ещё бы зум переработать, чтобы 100% - это picture размера workspacePanel с учётом OPEN_IMG...PADDING

		CenterImage(smoothly: false);
	}

	void NewImage()
	{
		SetImage(new Bitmap(
			workspacePanel.Width - NEW_IMG_WORKSPACE_HORIZONTAL_PADDING * 2,
			workspacePanel.Height - NEW_IMG_WORKSPACE_VERTICAL_PADDING * 2
		));
		_gfx.Clear(Color.White);
		_openedFileName = null;
	}

	void CenterImage(bool smoothly = true)
	{
		var target = new Point(
			((workspacePanel.Width - picture.Width) / 2) + WorkspaceNegativeMargin,
			((workspacePanel.Height - picture.Height) / 2) + WorkspaceNegativeMargin
		);

		if (smoothly)
			lock (_animLock) _animTarget = target;
		else
		{
			StopCenterImageAnimation();
			picture.Location = target;
		}
	}

	void SaveImageAs(string filename)
	{
		var ext = Path.GetExtension(filename);

		ExtensionToFileFormat.TryGetValue(ext, out var format);
		format ??= DefaultFileFormat;

		_bm.Save(filename, format);
		_openedFileName = filename;
	}

	void SaveImageAs()
	{
		SaveFileDialog dialog = new()
		{
			Filter = SaveDialogFilter,
			FileName = $"New image"
		};
		if (dialog.ShowDialog() != DialogResult.OK)
			return;

		SaveImageAs(dialog.FileName);
	}

	void SaveImage()
	{
		if (_openedFileName == null)
		{
			SaveImageAs();
			return;
		}

		SaveImageAs(_openedFileName);
	}

	void OpenImage()
	{
		OpenFileDialog dialog = new() { Filter = OpenDialogFilter };

		if (dialog.ShowDialog() != DialogResult.OK)
			return;

		using (var img = Image.FromFile(dialog.FileName))
			SetImage(img);

		_openedFileName = dialog.FileName;
	}

	void SetZoom(float newZoom, Point mouseWorkspace)
	{
		newZoom = Math.Clamp(newZoom, ZOOM_MIN, ZOOM_MAX);
		if (Math.Abs(newZoom - _zoom) < ZOOM_EPSILON) return;

		PointF imageCoord = new PointF(
			(mouseWorkspace.X - picture.Location.X) / _zoom,
			(mouseWorkspace.Y - picture.Location.Y) / _zoom
		);

		_zoom = newZoom;
		zoomLabel.Text = $"Zoom {_zoom * 100:0.#}%";

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
		StopCenterImageAnimation();
	}

	Point ScreenToBitmap(Point screenPoint)
	{
		return new Point(
			(int)((screenPoint.X - picture.Location.X) / _zoom),
			(int)((screenPoint.Y - picture.Location.Y) / _zoom)
		);
	}

	async Task CenterImageAnimationLoop()
	{
		void PerformAnimationStep(Point target)
		{
			if (IsDisposed) return;

			var curr = picture.Location;

			int deltaX = target.X - curr.X;
			int deltaY = target.Y - curr.Y;
			var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

			if (distance < ANIMATION_DISTANCE_EPSILON)
			{
				picture.Location = target;
				lock (_animLock)
					// _animTarget мог быть изменён (цель изменилась)
					if (_animTarget == target) _animTarget = null;
				return;
			}

			var step = distance * ANIMATION_SLOWDOWN_FACTOR;
			var ratio = step / distance;

			int newX = curr.X + (int)(deltaX * ratio);
			int newY = curr.Y + (int)(deltaY * ratio);

			picture.Location = new Point(newX, newY);
		}

		var token = _animCts.Token;
		while (!token.IsCancellationRequested)
		{
			try { await Task.Delay(ANIMATION_INTERVAL_MS, token).ConfigureAwait(false); }
			catch (OperationCanceledException) { break; }

			if (token.IsCancellationRequested) break;


			Point? target;
			lock (_animLock) target = _animTarget;

			if (!target.HasValue) continue;

			try { Invoke(() => PerformAnimationStep(target.Value)); }
			catch (ObjectDisposedException) { } // Форма была закрыта
			catch (InvalidOperationException) { }
		}
	}

	void StopCenterImageAnimation()
	{
		lock (_animLock)
			_animTarget = null;
	}


	// EVENTS //
	private void openToolStripMenuItem_Click(object sender, EventArgs e) => OpenImage();

	private void workspacePanel_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			_isDrawing = true;
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
			IsDragging = true;
			_dragStartPictureLocation = picture.Location;
			_dragStartMousePos = e.Location;

			if (workspacePanel.Cursor != Cursors.SizeAll)
				workspacePanel.Cursor = Cursors.SizeAll;
		}
	}

	private void workspacePanel_MouseMove(object sender, MouseEventArgs e)
	{
		var loc = e.Location.Sub(picture.Location);
		Point bitmapLoc = ScreenToBitmap(e.Location);
		bool isCursorOnPicture = bitmapLoc.X >= 0 && bitmapLoc.X < _bm.Width &&
								 bitmapLoc.Y >= 0 && bitmapLoc.Y < _bm.Height;

		if (_isDrawing && (isCursorOnPicture || _cursorBeOnPictureInLastFrame))
		{
			Point prevBitmap = ScreenToBitmap(_previousMousePos);
			Point currBitmap = ScreenToBitmap(e.Location);
			_gfx.DrawLine(_pen, prevBitmap, currBitmap);
			picture.Invalidate();
			_cursorBeOnPictureInLastFrame = isCursorOnPicture;
		}
		_previousMousePos = e.Location;

		if (IsDragging)
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

		var requiredCursor = IsDragging ? Cursors.SizeAll : (isCursorOnPicture ? Cursors.Cross : Cursors.Default);
		if (workspacePanel.Cursor != requiredCursor)
			workspacePanel.Cursor = requiredCursor;
	}

	private void workspacePanel_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
			_isDrawing = false;
		else if (e.Button == MouseButtons.Middle)
			IsDragging = false;
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

	private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveImage();

	private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) => SaveImageAs();

	private void centerCameraToolStripMenuItem_Click(object sender, EventArgs e) => CenterImage();

	private void withParamsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		var form = new NewImageForm();
		form.Show();
	}

	private void turnToGrayEffectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		const float percentsPerPixel = 0.01f;
		long pixelsForRefresh = Math.Max((long)(_bm.Height * _bm.Width * percentsPerPixel), 1);
		int width = _bm.Width;

		for (int y = 0; y < _bm.Height; y++)
			for (int x = 0; x < _bm.Width; x++)
			{
				var pixel = _bm.GetPixel(x, y);
				var grey = (pixel.R + pixel.B + pixel.G) / 3;
				_bm.SetPixel(x, y, Color.FromArgb(255, grey, grey, grey));

				// Для эффекта построчности (по приколу)
				if ((y * width + x) % pixelsForRefresh == 0)
					picture.Refresh();
			}

		Cursor = Cursors.Default;
	}

	private void MainForm_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.S)
		{
			SaveImage();
			e.SuppressKeyPress = true;
		}
	}

	private void MainForm_Resize(object sender, EventArgs e) => CenterImage();
	
}
