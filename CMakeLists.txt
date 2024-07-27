cmake_minimum_required(VERSION 3.10)
project(LogMelSpectrogram)

# For ARM64, Compilation
# cmake .. -A arm64

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_COMPILE_WARNING_AS_ERROR OFF)

if (CMAKE_GENERATOR_PLATFORM STREQUAL "")
    set (CMAKE_GENERATOR_PLATFORM "x64")
endif()
message(STATUS "Compiling Platform ${CMAKE_GENERATOR_PLATFORM}\n")

include_directories(inc)

if (${CMAKE_GENERATOR_PLATFORM} STREQUAL "x64")
    add_subdirectory(extern/pybind11)
    pybind11_add_module(py_log_mel_spectrogram
        src/log_mel_spectrogram.cpp
        src/pybind11_binding.cpp
        src/wavreader.cpp
    )

    add_library(log_mel_spectrogram MODULE
        src/log_mel_spectrogram.cpp
        src/pybind11_binding.cpp
        src/wavreader.cpp
    )
    target_link_libraries(log_mel_spectrogram PRIVATE pybind11::module)
else()
    add_library(log_mel_spectrogram MODULE
        src/log_mel_spectrogram.cpp
        src/wavreader.cpp
    )
endif()

if(MSVC)
    add_library(cs_log_mel_spectrogram SHARED
        src/log_mel_spectrogram.cpp
        src/csharp_binding.cpp
        src/wavreader.cpp
    )

    if (${CMAKE_GENERATOR_PLATFORM} STREQUAL "arm64")
        SET(CMAKE_CSharp_FLAGS "${CMAKE_CSharp_FLAGS} /platform:arm64")
        set_property(TARGET cs_log_mel_spectrogram PROPERTY DOTNET_TARGET_FRAMEWORK "net8.0")
        set_property(TARGET cs_log_mel_spectrogram PROPERTY COMMON_LANGUAGE_RUNTIME "netcore")
    else()
        #set_target_properties(cs_log_mel_spectrogram PROPERTIES COMMON_LANGUAGE_RUNTIME "")
        set_property(TARGET cs_log_mel_spectrogram PROPERTY DOTNET_TARGET_FRAMEWORK "net8.0")
        set_property(TARGET cs_log_mel_spectrogram PROPERTY COMMON_LANGUAGE_RUNTIME "netcore")
    endif()

    target_include_directories(cs_log_mel_spectrogram PUBLIC ${CMAKE_CURRENT_SOURCE_DIR} inc)

    target_compile_options(cs_log_mel_spectrogram PRIVATE /EHsc)
    #target_link_libraries(cs_log_mel_spectrogram PRIVATE log_mel_spectrogram)
endif()