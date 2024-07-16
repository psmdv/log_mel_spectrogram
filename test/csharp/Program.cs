using System;
using System.IO;
using System.Collections.Generic;
using LogMelSpectrogramCS;

class Program
{

    static void computeMelsFromAudioArray(LogMelSpectrogramCSBinding log_mel_spectrogram)
    {
        int SAMPLE_SIZE = 16000  * 3;
        List<float> input = new List<float>(); // 3 Secs Empty Audio Sample
        for (int i=0; i<SAMPLE_SIZE; i++)
        {
            input.Add(0.0f);
        }
        Console.WriteLine("len:" + input.Count);
        List<float> output = log_mel_spectrogram.compute(input);

        Console.WriteLine("Mels From Empty Audio Array Output:");
        if (output.Count > 10)
        {
            for (int i=0; i<10; i++)
            {
                Console.WriteLine(output[i]);
            }
        }
        Console.WriteLine("");
    }

    static void computeMelsFromAudioFile(LogMelSpectrogramCSBinding log_mel_spectrogram)
    {
        List<float> output = log_mel_spectrogram.load_wav_audio_and_compute("../../../../../assets/jfk.wav");
        Console.WriteLine("Mels From Audio File Output:");
        if (output.Count > 10)
        {
            for (int i=0; i<10; i++)
            {
                Console.WriteLine(output[i]);
            }
        }
        Console.WriteLine("");
    }


    static void Main()
    {
        String mel_bin_file = "../../../../../assets/mel_80.bin";
        if (File.Exists(mel_bin_file) == false)
        {
            // Check current directory
            if (File.Exists("mel_80.bin") == true)
            {
                mel_bin_file = "mel_80.bin";
            }
        }
        LogMelSpectrogramCSBinding log_mel_spectrogram_instance = new LogMelSpectrogramCSBinding(mel_bin_file);

        computeMelsFromAudioArray(log_mel_spectrogram_instance);
        computeMelsFromAudioFile(log_mel_spectrogram_instance);
    }
}