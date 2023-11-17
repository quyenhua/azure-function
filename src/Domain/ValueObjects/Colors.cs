using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public class Colors : ValueObject
    {
        static Colors()
        {
        }

        private Colors()
        {
        }

        private Colors(string code)
        {
            Code = code;
        }

        public static Colors From(string code)
        {
            var color = new Colors { Code = code };

            if (!SupportedColors.Contains(color))
            {
                throw new UnsupportedColorException(code);
            }

            return color;
        }

        public static Colors White => new("#FFFFFF");

        public static Colors Red => new("#FF5733");

        public static Colors Orange => new("#FFC300");

        public static Colors Yellow => new("#FFFF66");

        public static Colors Green => new("#CCFF99 ");

        public static Colors Blue => new("#6666FF");

        public static Colors Purple => new("#9966CC");

        public static Colors Grey => new("#999999");

        public string Code { get; private set; }

        public static implicit operator string(Colors color)
        {
            return color.ToString();
        }

        public static explicit operator Colors(string code)
        {
            return From(code);
        }

        public override string ToString()
        {
            return Code;
        }

        protected static IEnumerable<Colors> SupportedColors
        {
            get
            {
                yield return White;
                yield return Red;
                yield return Orange;
                yield return Yellow;
                yield return Green;
                yield return Blue;
                yield return Purple;
                yield return Grey;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
