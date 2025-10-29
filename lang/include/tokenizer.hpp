#pragma once

#include "common.h"

#include <string>
#include <vector>
#include <unordered_map>

namespace puff
{

inline static constexpr int WORD = 0;

struct token
{
    int code{ 0 };
    std::string text{};
};

class tokenizer
{
public:
    PUFF_CTORS(tokenizer, default, default, default);

    static tokenizer create_default();

public:
    tokenizer& add_token(int token, const char* repr);
    tokenizer& add_whitespace(char c);

    bool is_whitespace(char c) const;

    void parse(const std::string& text) const;

private:
    std::unordered_map<int, std::string> m_tokens{};
    std::vector<char> m_whitespace{};
};

}
