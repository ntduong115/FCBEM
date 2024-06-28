using System.Drawing;

namespace Core.Commons
{
    public static class Constants
    {
        public const int ITEMS_PER_PAGE = 10;
        public const int ITEMS_PER_SEARCH = 20;
        public const int ARTICLE_ITEMS_PER_PAGE = 9;
    }
    public readonly struct ImageSize
    {
        public static readonly Size NORMAL = new(width: 960, height: 540);
        public static readonly Size MEDIUM = new(width: 460, height: 300);
        public static readonly Size SMALL = new(width: 330, height: 250);
    }
}
