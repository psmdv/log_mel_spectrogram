#pragma once

#include "log_mel_spectrogram.hpp"
#include <vector>
#include <vcclr.h>


using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace LogMelSpectrogramCS {

    public ref class LogMelSpectrogramCSBinding {
    private:
        mel_spectrogram::LogMelSpectrogram* melSpectrogram;

        void MarshalNetToStdString(System::String^ s, std::string& os)
        {
            using System::IntPtr;
            using System::Runtime::InteropServices::Marshal;

            const char* chars = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer( );
            os = chars;
            Marshal::FreeHGlobal(IntPtr((void*)chars));
        }

    public:
        LogMelSpectrogramCSBinding(System::String^ filename) {
            std::string filename_cpp = "";
            MarshalNetToStdString(filename, filename_cpp);
            melSpectrogram = new mel_spectrogram::LogMelSpectrogram(filename_cpp);
        }

        ~LogMelSpectrogramCSBinding() {
            this->!LogMelSpectrogramCSBinding();
        }

        !LogMelSpectrogramCSBinding() {
            delete melSpectrogram;
        }

        List<float>^ compute(List<float>^ input) {
            List<float>^ outputList = gcnew List<float>();
            
            std::vector<float> inputVector;          
            for each (float value in input) {
                inputVector.push_back(value);
            }
            
            std::vector<float> outputVector = melSpectrogram->compute(inputVector);
             
            for (float value : outputVector) {
                outputList->Add(value);
            }

            return outputList;
        }

        List<float>^ load_wav_audio_and_compute(System::String^ filename) {
            List<float>^ outputList = gcnew List<float>();

            std::string filename_cpp = "";
            MarshalNetToStdString(filename, filename_cpp);
           
            std::vector<float> outputVector = melSpectrogram->load_wav_audio_and_compute(filename_cpp);
             
            for (float value : outputVector) {
                outputList->Add(value);
            }

            return outputList;
        }

        List<float>^ load_wav_audio(System::String^ filename) {
            List<float>^ outputList = gcnew List<float>();

            std::string filename_cpp = "";
            MarshalNetToStdString(filename, filename_cpp);
           
            std::vector<float> outputVector = melSpectrogram->load_wav_audio(filename_cpp);
             
            for (float value : outputVector) {
                outputList->Add(value);
            }

            return outputList;
        }        

        List<float>^ load_audio_chunk(List<float>^ audio) {
            List<float>^ outputList = gcnew List<float>();
            
            std::vector<float> audio_samples;
            for each (float value in audio) {
                audio_samples.push_back(value);
            }
            
            std::vector<float> outputVector = melSpectrogram->load_audio_chunk(audio_samples);
             
            for (float value : outputVector) {
                outputList->Add(value);
            }

            return outputList;
        }
    };

}
