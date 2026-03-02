namespace Editor.Extensions.ForSize;

internal static class SizeExtensions
{
	public static Point AreaCenter(this Size size)
		=> new Point(size.Width / 2, size.Height / 2);
}
