#include <pybind11/pybind11.h>
#include <pybind11/numpy.h>
#include "log_mel_spectrogram.hpp"

namespace py = pybind11;


PYBIND11_MODULE(py_log_mel_spectrogram, m) {
    py::class_<mel_spectrogram::LogMelSpectrogram>(m, "LogMelSpectrogram")
        .def(py::init<std::string>())
        .def("compute", [](mel_spectrogram::LogMelSpectrogram &self, const py::array_t<float> &input) {
            // Request a buffer descriptor from NumPy array
            py::buffer_info buf = input.request();

            // Ensure the input is a 1D array
            if (buf.ndim != 1)
                throw std::runtime_error("Input should be a 1D NumPy array");

            // Cast the data pointer to a float pointer
            float* ptr = static_cast<float*>(buf.ptr);
            std::vector<float> input_vector(ptr, ptr + buf.size);

            // Call the C++ function
            std::vector<float> result = self.compute(input_vector);

            // Convert the result to a NumPy array and return
            return py::array(result.size(), result.data());
        })
        .def("load_wav_audio_and_compute", [](mel_spectrogram::LogMelSpectrogram &self, const std::string& filename) {

            // Call the C++ function
            std::vector<float> result = self.load_wav_audio_and_compute(filename);

            // Convert the result to a NumPy array and return
            return py::array(result.size(), result.data());
        })
        .def("load_wav_audio", [](mel_spectrogram::LogMelSpectrogram &self, const std::string& filename) {

            // Call the C++ function
            std::vector<float> result = self.load_wav_audio(filename);

            // Convert the result to a NumPy array and return
            return py::array(result.size(), result.data());
        })        
        .def("load_audio_chunk", [](mel_spectrogram::LogMelSpectrogram &self, const py::array_t<float> &input) {

            // Request a buffer descriptor from NumPy array
            py::buffer_info buf = input.request();

            // Ensure the input is a 1D array
            if (buf.ndim != 1)
                throw std::runtime_error("Input should be a 1D NumPy array");

            // Cast the data pointer to a float pointer
            float* ptr = static_cast<float*>(buf.ptr);
            std::vector<float> audio_samples(ptr, ptr + buf.size);

            // Call the C++ function
            std::vector<float> result = self.load_audio_chunk(audio_samples);

            // Convert the result to a NumPy array and return
            return py::array(result.size(), result.data());
        });
}