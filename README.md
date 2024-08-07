# Log Mel Spectrogram C++

- This project is heavily inspired from whisper.cpp and most of the code is from https://github.com/ggerganov/whisper.cpp
- Wav file reader is full port from https://github.com/mstorsjo/fdk-aac
- This is a quick portable C/C++ version of computing log-mel-spectrograms that can be used in conjunction with whisper Models

# Supported Bindings
- Python
- C#

# Instructions
- Checkout the Repository and init submodules
```shell
$ git clone https://github.com/psmdv/log_mel_spectrogram
$ git submodule init
$ git submodule update
```
- Build the Python binding module or c++ dynamic library module using CMakeLists
```shell
$ mkdir build
$ cd build
$ cmake ..
$ cmake --build . --config Release
# Ends up with two types of files
# For Linux - liblog_mel_spectrogram_cpp.so  py_log_mel_spectrogram.cpython-310-x86_64-linux-gnu.so
# For Windows - log_mel_spectrogram.dll py_log_mel_spectrogram.cp310-win_amd64.pyd cs_log_mel_spectrogram.dll
```

# License

- MIT License Project - You can freely distribute or use the code for your own projects.

# Thanks

- ggerganov (https://github.com/ggerganov)
- mstorsjo (https://github.com/mstorsjo)

for the valuable contributions to the OSS AI and AV community 