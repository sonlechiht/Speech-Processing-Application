import librosa
import soundfile as sf


def split_audio(file_path, duration=15, overlap=5, sr=None):
  y, sr = librosa.load(file_path, sr=sr)

  samples_per_segment = int(duration * sr)
  samples_overlap = int(overlap * sr)

  indices = range(0, len(y) - samples_per_segment, samples_per_segment - samples_overlap)
  segments = [y[i:i+samples_per_segment] for i in indices]#32768.0
  return segments,sr

# audio, sr = librosa.load(r'03 Track 3.mp3')
# segments = split_audio_with_overlap(audio, sr, segment_length=15, overlap=5)

segments,sr = split_audio(r'8 Track 8.mp3')
for i, segment in enumerate(segments):
  sf.write(f"segment_{i}.wav", segment, sr)