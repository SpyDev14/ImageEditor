namespace Editor.Extensions.Point;

internal static class PointExtensions
{
	public static System.Drawing.Point Add(this System.Drawing.Point point, System.Drawing.Point other)
		=> new System.Drawing.Point(point.X + other.X, point.Y + other.Y);

	public static System.Drawing.Point Sub(this System.Drawing.Point point, System.Drawing.Point other)
		=> new System.Drawing.Point(point.X - other.X, point.Y - other.Y);
}
