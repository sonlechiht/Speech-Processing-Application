import whisper
import ChatTTS
import torch
import torchaudio

import os
import argparse
import json
import torch

# import librosa
import numpy as np

# def split_audio(file_path, duration=30, overlap=10, sr=None):
#   y, sr = librosa.load(file_path, sr=sr)

#   samples_per_segment = int(duration * sr)
#   samples_overlap = int(overlap * sr)

#   indices = range(0, len(y) - samples_per_segment, samples_per_segment - samples_overlap)

#   segments = [np.frombuffer(y[i:i+samples_per_segment], dtype=np.int16).astype(np.float32) / sr for i in indices]#32768.0
#   return segments

def STT(file_process):
    #STT
    model = whisper.load_model("medium.en")
    result = model.transcribe(file_process)
    # file1 = open(file_process[:-3] + "txt", "w", encoding='utf-8')  # write mode
    # file1.write(result["text"])
    # file1.close()
    try:
        with open(file_process[:-3] + "txt", "w", encoding="utf-8") as file1:
            file1.write(result["text"])
    except UnicodeEncodeError as e:
        # print(result["text"])
        with open(file_process[:-3] + "txt", "w", encoding="utf-8") as file1:
            file1.write(f"Error encoding text: {e}")

def TTS(file_process):
    #TTS
    # device = "cuda:0" if torch.cuda.is_available() else "cpu"
    chat = ChatTTS.Chat()
    chat.load(compile=False) # Set to True for better performance
    rand_spk = chat.sample_random_speaker()


    params_infer_code = ChatTTS.Chat.InferCodeParams(
        spk_emb = rand_spk, # add sampled speaker 
    )


    f = open(file_process, "r")
    orig_string = f.read()
    f.close()
    list_of_lines = []
    max_length = 400
    while len(orig_string) > max_length:
        line_length = orig_string[:max_length].rfind('. ')
        list_of_lines.append(orig_string[:line_length])
        orig_string = orig_string[line_length + 1:]
    list_of_lines.append(orig_string)

    texts = list_of_lines

    wavs = chat.infer(texts, params_infer_code=params_infer_code)
    outfile_name = file_process[:-3] + 'wav'

    
    try:
        result = torch.cat([torch.from_numpy(wavs[i]).unsqueeze(0) for i in range(len(texts))], dim=1)
        torchaudio.save(outfile_name, result, 24000)
    except:
        result = result = torch.cat([torch.from_numpy(wavs[i]) for i in range(len(texts))])
        torchaudio.save(outfile_name, result, 24000)
    # try:
    #     torchaudio.save(outfile_name, torch.from_numpy(wavs[0]).unsqueeze(0), 24000)
    # except:
    #     torchaudio.save(outfile_name, torch.from_numpy(wavs[0]), 24000)

def main(config):
    with open(config.input, 'r') as file:
        meta_data = json.load(file)
    
    task = meta_data['Task']
    file_process = meta_data['File']
    # working_folder = meta_data['WorkingFolder']

    if task == "STT":
        STT(file_process)
    if task == "TTS":
        TTS(file_process)


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--input', type=str, default='OWN')

    config = parser.parse_args()

    main(config)




