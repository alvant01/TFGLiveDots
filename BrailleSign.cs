using System;

namespace LiveDots
{

    public static class BrailleSign
    {
        public static int dotsToBits(int dots)
        {
            int bits = 0;
            for (int dec = dots; dec > 0; dec /= 10)
            {
                bits |= 1 << ((dec % 10) - 1);
            }
            return bits;
        }
        public static String braille(int dots)
        {
            return Char.ToString((char)(UNICODE_BRAILLE_MASK | dotsToBits(dots)));
        }
        public static String braille(int dots1, int dots2)
        {
            return braille(dots1) + braille(dots2);
        }
        public static String braille(int dots1, int dots2, int dots3)
        {
            return braille(dots1) + braille(dots2) + braille(dots3);
        }

        /** Start of Unicode braille range.
         * @see <a href="http://www.unicode.org/charts/PDF/U2800.pdf">Unicode range
         *      U+2800 to U+28FF</a>
         */
        private const int UNICODE_BRAILLE_MASK = 0X2800;
    }
}
