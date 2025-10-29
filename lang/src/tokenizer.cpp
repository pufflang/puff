#include "tokenizer.hpp"
#include "puff/tokens.h"

namespace puff
{

tokenizer& tokenizer::add_token(int token, const char* repr)
{
    m_tokens[token] = repr;
    return *this;
}

tokenizer& tokenizer::add_whitespace(char c)
{
    m_whitespace.push_back(c);
    return *this;
}

bool tokenizer::is_whitespace(char c) const
{
    return std::find(m_whitespace.begin(), m_whitespace.end(), c) != m_whitespace.end();
}

void tokenizer::parse(const std::string& text) const
{
    std::vector<int> codes{};
    std::vector<std::string> repr{};

    for (const auto& [code, r] : m_tokens)
    {
        codes.push_back(code);
        repr.push_back(r);
    }

    std::vector<int> possible_tokens{};
    possible_tokens.reserve(codes.size());

    bool is_word{ false };
    std::string cur_word{};

    for (std::size_t i = 0, n = text.size(); i < n; i++)
    {
        char c = text[i];
    }
}

tokenizer tokenizer::create_default()
{
    return tokenizer{}
        .add_token(END_INSTR, ";")
        .add_token(LEFT_BRACE, "{")
        .add_token(RIGHT_BRACE, "}")
        .add_token(LEFT_SQUARE, "[")
        .add_token(RIGHT_SQUARE, "]")
        .add_token(OBJ_ACCESS, ".")
        .add_token(TYPE_ACCESS, "::")
        .add_token(PTR_ACCESS, "->")
        .add_token(COMMA, ",")
        .add_token(LEFT_PARA, "(")
        .add_token(RIGHT_PARA, ")")
        .add_token(PLUS, "+")
        .add_token(MINUS, "-")
        .add_token(DIVIDE, "/")
        .add_token(MULTIPLY, "*")
        .add_token(EQUALS, "=")
        .add_token(DOUBLEQUOT, "\"")
        .add_token(QUOT, "'")

        .add_whitespace(32)
        .add_whitespace(9)
        .add_whitespace(10)
        .add_whitespace(13)
        .add_whitespace(12)
        .add_whitespace(11)
        ;
}

}
