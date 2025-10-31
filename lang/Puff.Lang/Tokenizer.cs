using System.Diagnostics.CodeAnalysis;

namespace Puff.Lang;

public static class Tokenizer
{
    public static List<Token> Parse(string text)
    {
        List<Token> tokens = [];
        List<char> buffer = [];

        bool tokenMode = false;

        List<Token> possibleTokens = [];
        foreach (char c in text.Append(' '))
        {
            possibleTokens.Clear();
            Token? token;

            if (TokenTools.IsWhitespace(c))
            {
                if (buffer.Count == 0)
                    continue;

                if (!ParseWordBuffer(buffer, out token) && !ParseTokenBuffer(buffer, out possibleTokens, out token))
                    throw new Exception($"Unexpected token {new string([.. buffer])}");

                tokens.Add(token);
                buffer.Clear();
                tokenMode = false;
                continue;
            }

            buffer.Add(c);
            var prevBuffer = buffer.SkipLast(1);
            if (ParseTokenBuffer(buffer, out possibleTokens, out token))
            {
                tokenMode = false;
                tokens.Add(token);
                buffer.Clear();
                continue;
            }
            else if (possibleTokens.Count != 0)
            {
                if (!ParseWordBuffer(prevBuffer, out var wordToken))
                {
                    throw new Exception($"Unexpected token {new string([.. prevBuffer])}");
                }

                tokens.Add(wordToken);

                tokenMode = true;
                continue;
            }
            else if (tokenMode)
            {
                throw new Exception($"Unknown token {new string([.. prevBuffer])}");
            }
        }

        return tokens;
    }

    private static bool ParseWordBuffer(IEnumerable<char> buffer, [NotNullWhen(true)] out Token? token)
    {
        token = null;
        if (!buffer.Any() || buffer.Any(c => !TokenTools.IsWordChar(c) && !TokenTools.IsNumberChar(c)))
        {
            return false;
        }

        string wordText = new([.. buffer]);

        bool isNumber = TokenTools.IsNumberChar(wordText.First());
        if (isNumber)
        {
            if (wordText.Length == 1)
            {
                token = new Token(TokenType.NumberDecimal, wordText);
            }
            else if (TokenTools.IsNumberChar(wordText[1]))
            {
                
            }
        }
    }

    private static bool ParseTokenBuffer(IEnumerable<char> buffer, out List<Token> possibleTokens, [NotNullWhen(true)] out Token? token)
    {
        possibleTokens = [];
        string bufferText = new([.. buffer]);
        foreach (var tokenText in TokenTools.Representations)
        {
            if (tokenText.Value.StartsWith(bufferText))
            {
                possibleTokens.Add(new Token(tokenText.Key, bufferText));
            }
        }

        if (possibleTokens.Count == 1)
        {
            token = possibleTokens.Single();
            return true;
        }

        token = null;
        return false;
    }
}
