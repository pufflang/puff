#pragma once

#define PUFF_CTOR_DEFAULT(clazz, impl) explicit clazz() = impl
#define PUFF_CTOR_ASSIGN(clazz, impl) \
clazz(const clazz&) = impl; \
clazz& operator=(const clazz&) = impl
#define PUFF_CTOR_MOVE(clazz, impl) \
clazz(clazz&&) = impl; \
clazz& operator=(clazz&&) = impl

#define PUFF_CTORS(clazz, def, assign, move) \
PUFF_CTOR_DEFAULT(clazz, def); \
PUFF_CTOR_ASSIGN(clazz, assign); \
PUFF_CTOR_MOVE(clazz, move)
