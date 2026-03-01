namespace Editor.Extensions.ForPoint;

internal static class PointExtensions
{
	public static Point Add(this Point point, Point other)
		=> new Point(point.X + other.X, point.Y + other.Y);

	public static Point Sub(this Point point, Point other)
		=> new Point(point.X - other.X, point.Y - other.Y);
}
