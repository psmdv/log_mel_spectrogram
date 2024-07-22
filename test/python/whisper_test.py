import sys
import platform

if platform.system() == "Windows":
    sys.path.append( '../../build/Release' )
else:
    sys.path.append( '../../build' )
    
import whisper
import torch
# Import our custom log_mel_spectrogram module
import py_log_mel_spectrogram

model = whisper.load_model("tiny")

"""
    Standard Way of Whisper to process audio and compute mel spectrogram

    # load audio and pad/trim it to fit 30 seconds
    audio = whisper.load_audio("../../assets/jfk.wav")
    audio = whisper.pad_or_trim(audio)
    # make log-Mel spectrogram and move to the same device as the model
    mel = whisper.log_mel_spectrogram(audio).to(model.device)
"""

log_mel_spectrogram_instance = py_log_mel_spectrogram.LogMelSpectrogram("../../assets/mel_80.bin")
"""
    Use Case 1: If you already have processed audio to wav 16Khz Mono, then you can use the below method to compute 
"""
#audio = whisper.load_audio("../../assets/jfk.wav")
#audio = whisper.pad_or_trim(audio)
#lmel = log_mel_spectrogram_instance.compute(audio)

"""
    Use Case 2: If you have only wav audio file
"""
lmel = log_mel_spectrogram_instance.load_wav_audio_and_compute("../../assets/jfk.wav")

"""
    Use Case 3: If you have audio chunk file
"""
#audio = whisper.load_audio("../../assets/jfk.wav")
#lmel = log_mel_spectrogram_instance.load_audio_chunk(audio)


mel = lmel[:(80 * 3000)].reshape(80, 3000)
mel = torch.from_numpy(mel)

# detect the spoken language
_, probs = model.detect_language(mel)
print(f"Detected language: {max(probs, key=probs.get)}")

# decode the audio
options = whisper.DecodingOptions(fp16=True)
result = whisper.decode(model, mel, options)

# print the recognized text
print(result.text)

