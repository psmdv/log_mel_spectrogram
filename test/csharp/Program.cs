using System;
using System.IO;
using System.Collections.Generic;
using LogMelSpectrogramCS;

class Program
{

    static float[] ReadWavFile(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            // Read the RIFF header
            string chunkID = new string(reader.ReadChars(4));
            if (chunkID != "RIFF")
                throw new FormatException("Invalid WAV file format.");

            int fileSize = reader.ReadInt32();
            string format = new string(reader.ReadChars(4));
            if (format != "WAVE")
                throw new FormatException("Invalid WAV file format.");

            // Read chunks in a loop
            bool fmtChunkRead = false;
            bool dataChunkRead = false;
            short[] samples = null;

            while (fs.Position < fs.Length)
            {
                string chunkName = new string(reader.ReadChars(4));
                int chunkSize = reader.ReadInt32();

                if (chunkName == "fmt ")
                {
                    fmtChunkRead = true;
                    int audioFormat = reader.ReadInt16();
                    int numChannels = reader.ReadInt16();
                    int sampleRate = reader.ReadInt32();
                    int byteRate = reader.ReadInt32();
                    int blockAlign = reader.ReadInt16();
                    int bitsPerSample = reader.ReadInt16();

                    if (audioFormat != 1 || numChannels != 1 || sampleRate != 16000 || bitsPerSample != 16)
                        throw new FormatException("Unsupported WAV file format.");

                    // Skip any remaining bytes in this chunk (if any)
                    fs.Seek(chunkSize - 16, SeekOrigin.Current);
                }
                else if (chunkName == "data")
                {
                    dataChunkRead = true;
                    // Read the audio data
                    byte[] data = reader.ReadBytes(chunkSize);
                    samples = new short[chunkSize / 2];
                    Buffer.BlockCopy(data, 0, samples, 0, chunkSize);
                }
                else
                {
                    // Skip unknown chunks
                    fs.Seek(chunkSize, SeekOrigin.Current);
                }
            }

            if (!fmtChunkRead || !dataChunkRead)
                throw new FormatException("Incomplete WAV file.");

            // Normalize the audio data
            float[] floatData = new float[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                floatData[i] = samples[i] / 32768.0f; // Normalize to [-1, 1]
            }

            return floatData;
        }
    }

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

    static void computeMelsFromAudioChunk(LogMelSpectrogramCSBinding log_mel_spectrogram)
    {
        // Prepare Audio Chunk
        List<float> audio_chunk = new List<float>();
        float[] audio_buffer = ReadWavFile("../../../../../assets/jfk.wav");
        for (int i=0; i<audio_buffer.Length; i++)
        {
            audio_chunk.Add(audio_buffer[i]);
        }
        // Can use C++ Wav read version as well
        //List<float> audio_chunk = log_mel_spectrogram.load_wav_audio("../../../../../assets/jfk.wav");
        // Compute
        List<float> output = log_mel_spectrogram.load_audio_chunk(audio_chunk);
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
        computeMelsFromAudioChunk(log_mel_spectrogram_instance);
    }
}