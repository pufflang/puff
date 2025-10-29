cmake_minimum_required(VERSION 3.27.0)
project(pufflang)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

set(LLVM_ENABLE_PROJECTS "llvm" CACHE STRING "")
set(LLVM_ENABLE_RTTI ON CACHE BOOL "")
set(LLVM_ENABLE_TERMINFO OFF CACHE BOOL "")

add_subdirectory(external)

llvm_map_components_to_libnames(llvm_libs core irreader support)
