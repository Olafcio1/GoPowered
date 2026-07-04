namespace GoPowered.Lang.Lexer.Char
{
    public static class CharUtils
    {
        public static bool IsLatin(char ch) =>
            (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');

        public static bool IsLiteral(char ch) =>
            ch == '_' || IsLatin(ch) || IsDigit(ch) || ch >= 127;

        public static bool IsDigit(char ch) =>
            ch >= '0' && ch <= '9';

        public static bool IsHexDigit(char ch) =>
            IsDigit(ch) || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
    }
}
