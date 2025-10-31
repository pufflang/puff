namespace Puff.Lang;

public enum TokenType
{
    Word = 0,
    NumberDecimal,
    NumberHex,
    NumberBinary,
    NumberFloating,
    Comma,
    Dot,
    Colon,
    LeftBrace,
    RightBrace,
    LeftSquare,
    RightSquare,
    LeftPara,
    RightPara,
    Minus,
    Plus,
    Star,
    Multiply = Star,
    Slash,
    Divide = Slash,
    Equals,
    LessThan,
    LargerThan,
    LeftArrow = LessThan,
    RightArrow = LargerThan,
    SingleQuot,
    DoubleQuot,
    Exclam,
    At,
    Hash,
    Dollar,
    Mod,
    Hat,
    Ampersand,
    Question,

    PointerAccess, // ->
    Direct  // =>
}

public record Token(TokenType Type, string Text);

public static class TokenTools
{
    private static Dictionary<TokenType, string>? _representations;
    public static Dictionary<TokenType, string> Representations => _representations ??= BuildRepresentations();

    public static bool IsWhitespace(char c) => char.IsWhiteSpace(c);
    public static bool IsWordChar(char c) => char.IsAsciiLetterOrDigit(c);
    public static bool IsNumberChar(char c) => char.IsAsciiDigit(c);
    public static bool IsHexNumberChar(char c) => char.IsAsciiHexDigit(c);
    public static bool IsBinaryNumberChar(char c) => c == '0' || c == '1';
    public static bool IsNumberBaseChar(char c, out TokenType numberTokenType)
    {
        numberTokenType = TokenType.NumberDecimal;
        switch (c)
        {
            case 'b':
                numberTokenType = TokenType.NumberBinary;
                return true;
            case 'x':
                numberTokenType = TokenType.NumberHex;
                return true;
            default:
                return false;
        }
    }

    private static Dictionary<TokenType, string> BuildRepresentations() => new()
    {
        [TokenType.Comma] = ",",
        [TokenType.Dot] = ".",
        [TokenType.Colon] = ";",
        [TokenType.LeftBrace] = "{",
        [TokenType.RightBrace] = "}",
        [TokenType.LeftSquare] = "[",
        [TokenType.RightSquare] = "]",
        [TokenType.LeftPara] = "(",
        [TokenType.RightPara] = ")",
        [TokenType.Minus] = "-",
        [TokenType.Plus] = "+",
        [TokenType.Star] = "*",
        [TokenType.Slash] = "/",
        [TokenType.Equals] = "=",
        [TokenType.LessThan] = "<",
        [TokenType.LargerThan] = ">",
        [TokenType.SingleQuot] = "'",
        [TokenType.DoubleQuot] = "\"",
        [TokenType.Exclam] = "!",
        [TokenType.At] = "@",
        [TokenType.Hash] = "#",
        [TokenType.Dollar] = "$",
        [TokenType.Mod] = "%",
        [TokenType.Hat] = "^",
        [TokenType.Ampersand] = "&",
        [TokenType.Question] = "?",
        [TokenType.PointerAccess] = "->",
        [TokenType.Direct] = "=>"
    };
}
