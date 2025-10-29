cmake_minimum_required(VERSION 3.27.0)
project(pufflang)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

set(LLVM_ENABLE_PROJECTS "llvm" CACHE STRING "")
set(LLVM_ENABLE_RTTI ON CACHE BOOL "")
set(LLVM_ENABLE_TERMINFO OFF CACHE BOOL "")

add_subdirectory(external)

add_definitions(${LLVM_DEFINITIONS})

llvm_map_components_to_libnames(llvm_libs core irreader support)

set(PUFF_INC_DIR "${CMAKE_CURRENT_SOURCE_DIR}/include")
set(PUFF_SRC_DIR "${CMAKE_CURRENT_SOURCE_DIR}/src")

file(GLOB PUFF_INC_FILES "${PUFF_INC_DIR}/*.hpp" "${PUFF_INC_DIR}/*.h")
file(GLOB PUFF_SRC_FILES "${PUFF_SRC_DIR}/*.cpp")

set(PUFF_FILES ${PUFF_INC_FILES} ${PUFF_SRC_FILES})
message(STATUS ${PUFF_FILES})

add_library(pufflang ${PUFF_FILES})
target_include_directories(pufflang PUBLIC ${PUFF_INC_DIR})
target_include_directories(pufflang PRIVATE ${LLVM_INCLUDE_DIRS})

target_link_libraries(pufflang PRIVATE ${llvm_libs})
