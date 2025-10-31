using System.Diagnostics.CodeAnalysis;

namespace Puff.Lang;

public record TokenizerError(string File, string Error, int Line, int Column);

public class Tokenizer
{
    private readonly List<TokenizerError> _errors = [];
    private string _currentFile = "(null)";
    private int _currentLine;
    private int _currentColumn;

    public bool Parse(string text, string filename, out List<Token> tokens)
    {
        tokens = [];
        _currentFile = filename;
        _errors.Clear();
        _currentLine = 1;
        _currentColumn = 0;

        List<char> buffer = [];

        bool nextLine = false;

        bool tokenMode = false;
        List<Token> possibleTokens = [];
        foreach (char c in text.Append(' '))
        {
            _currentColumn++;

            if (nextLine)
            {
                _currentLine++;
                _currentColumn = 0;
                nextLine = false;
            }

            possibleTokens.Clear();
            Token? token;

            if (c == '\n')
            {
                nextLine = true;
            }

            if (TokenTools.IsWhitespace(c))
            {
                if (buffer.Count == 0)
                    continue;

                if (!ParseWordBuffer(buffer, out token) && !ParseTokenBuffer(buffer, out possibleTokens, out token))
                {
                    AppendError($"Unexpected token {new string([.. buffer])}");
                    return false;
                }

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
                    AppendError($"Unexpected token {new string([.. prevBuffer])}");
                    return false;
                }

                tokens.Add(wordToken);

                tokenMode = true;
                continue;
            }
            else if (tokenMode)
            {
                AppendError($"Unknown token {new string([.. prevBuffer])}");
                return false;
            }
        }

        return true;
    }

    private void AppendError(string error)
    {
        TokenizerError err = new(_currentFile, error, _currentLine, _currentColumn);
        _errors.Add(err);
    }

    private bool ParseWordBuffer(IEnumerable<char> buffer, [NotNullWhen(true)] out Token? token)
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

    private bool ParseTokenBuffer(IEnumerable<char> buffer, out List<Token> possibleTokens, [NotNullWhen(true)] out Token? token)
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

    private bool IsValidNumber(string word, out TokenType numberType)
    {
        numberType = TokenType.NumberDecimal;

        if (word.Length > 1 && word.Last() == 'f')
        {
            numberType = TokenType.NumberFloating;
            word = word[..^1];
        }

        if (word.Length == 1 && TokenTools.IsNumberChar(word.Single()))
            return true;

        char sndChar = word[1];

        if (!TokenTools.IsNumberChar(sndChar))
        {
            if (word.Length <= 2)
            {
                AppendError($"'{word}' is not a valid number");
                return false;
            }

            if (!TokenTools.IsNumberBaseChar(sndChar, out var numberTokenType))
            {
                AppendError($"'{word}' is not a valid number");
                return false;
            }

            string afterBaseDigits = word[2..];

            switch (numberTokenType)
            {
                case TokenType.NumberBinary:
                    if (!afterBaseDigits.All(TokenTools.IsBinaryNumberChar))
                    {
                        AppendError($"'{word}' is not a valid binary number");
                        return false;
                    }
                    break;
                case TokenType.NumberHex:
                    if (!afterBaseDigits.All(TokenTools.IsHexNumberChar))
                    {
                        AppendError($"'{word}' is not a valid hex number");
                        return false;
                    }
                    break;
            }

            numberType = numberTokenType;
            return true;
        }

        bool floatSeparator = false;
        for (int i = 0; i < word.Length; i++)
        {
            char digit = word[i];
            if (digit == '.')
            {
            }
        }
    }
}
