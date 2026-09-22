# 🎙️ Speech Processing Application - TTS & STT System

[![Python Version](https://img.shields.io/badge/python-3.8%2B-blue.svg)](https://www.python.org/)
[![PyTorch](https://img.shields.io/badge/PyTorch-Deep%20Learning-EE4C2C.svg)](https://pytorch.org/)
[![Gradio / Streamlit](https://img.shields.io/badge/UI-Interactive%20App-FF4B4B.svg)](https://github.com/sonlechiht/Speech-Processing-Application)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

A comprehensive Speech Processing Application featuring state-of-the-art **Text-to-Speech (TTS)** and **Speech-to-Text (STT / ASR)** functionalities. Designed with an intuitive user interface, this project allows users to convert text to natural-sounding speech and transcribe spoken audio into accurate text seamlessly.

---

## 🎥 Application Demo & Test Results

### 🎬 Application Interface
<p align="center">
  <img src="https://raw.githubusercontent.com/sonlechiht/Speech-Processing-Application/main/Demo/Start-App.PNG" alt="Speech Processing App Demo" width="850">
</p>

*📌 **Note:** Make sure to place your application screenshot in `Demo/Start-App.PNG` or update the path accordingly.*

### 🎬 Speech To Text Interface
<p align="center">
  <img src="https://raw.githubusercontent.com/sonlechiht/Speech-Processing-Application/main/Demo/STT-UI.PNG" alt="Speech Processing App Demo" width="850">
</p>

*📌 **Note:** Make sure to place your application screenshot in `Demo/STT-UI.PNG` or update the path accordingly.*

### 🎬 Text To Speech Interface
<p align="center">
  <img src="https://raw.githubusercontent.com/sonlechiht/Speech-Processing-Application/main/Demo/TTS-UI.PNG" alt="Speech Processing App Demo" width="850">
</p>

*📌 **Note:** Make sure to place your application screenshot in `Demo/TTS-UI.PNG` or update the path accordingly.*

---

### 📊 TTS & STT Evaluation & Sample Output

| Task | Input | Output / Results |
| :--- | :--- | :--- |
| **Text-to-Speech (TTS)** | 📄 Text input: [`Demo/TextScript.txt`](https://github.com/sonlechiht/Speech-Processing-Application/blob/main/Demo/TextScript.txt) | 🔊 Generated Audio: [`Demo/TextScript-TTS-result.wav`](https://github.com/sonlechiht/Speech-Processing-Application/blob/main/Demo/TextScript-TTS-result.wav) |
| **Speech-to-Text (STT)** | 🎙️ Sample Audio: [`Demo/03-Track-3.mp3`](https://github.com/sonlechiht/Speech-Processing-Application/blob/main/Demo/03-Track-3.mp3) | 📝 Transcribed Text: [`Demo/03-Track-3-STT-result.txt`](https://github.com/sonlechiht/Speech-Processing-Application/blob/main/Demo/03-Track-3-STT-result.txt) |
---

## ✨ Key Features

- 🔊 **Text-to-Speech (TTS)**:
  - High-quality, natural voice synthesis.
  - Multi-speaker and multi-accent voice customization options.
  - Adjustable speech parameters (pitch, speed, and volume).
- 🎙️ **Speech-to-Text (STT / ASR)**:
  - Accurate automatic speech recognition across multiple languages and dialects.
  - Real-time live microphone transcription and batch audio file processing.
  - Noise robustness and domain-specific vocabulary adaptations.
- 💻 **Interactive User Interface**:
  - Clean, user-friendly Web UI built for seamless testing and demonstration.
  - Audio waveform rendering and text editing panel.

---

## 🏗️ Repository Structure

```text
Speech-Processing-Application/
├── Demo/                  # App screenshots, TTS, and STT test result images
│   ├── 03-Track-3.mp3
│   ├── 03-Track-3-STT-result.txt
│   ├── TextScript.txt
│   ├── TextScript-TTS-result.wav
│   └── Start-App.PNG
├── SpeechProcessing-Python-Core/                  # Source core infer speech and text
│   ├── SpeechProcessing.py
│   └── requirements.txt
├── SpeechProcessing-WPF-UI/                     # Source code WPF UI
└── README.md                # Project documentation
```

---

## 🛠️ Installation

### 1. Prerequisites
- Python >= 3.8 [cite: 1.1.1]
- PyTorch (CUDA recommended for GPU acceleration) [cite: 1.1.3, 1.1.5]
- FFmpeg (Required for audio processing)

### 2. Clone Repository & Install Dependencies

```bash
# Clone the repository
git clone https://github.com/sonlechiht/Speech-Processing-Application.git
cd Speech-Processing-Application

# Create a virtual environment (Recommended)
python -m venv venv
source venv/bin/activate  # On Linux/macOS
# On Windows: venv\Scripts\activate

# Install required dependencies
pip install -r requirements.txt
```

---

## 🚀 Usage

### 1. Run the Web Application
Build app core
```bash
pyinstaller SpeechProcessing.py --onedir
```
Copy result pyinstaller (SpeechProcessing.exe and _internel) into folder bin "SpeechProcessing-WPF-UI"
### 2. Run WPF application 
Run \SpeechProcessing-WPF-UI\SpeechProcessing.sln

---

## 📈 Performance & Metrics

- **STT Accuracy**: Evaluated on Word Error Rate (WER) and Character Error Rate (CER).
- **TTS Quality**: Measured via Mean Opinion Score (MOS) and audio spectrogram alignment.

---

## 📜 License

Distributed under the **MIT License**. See `LICENSE` for more information.

---

## 👤 Author

- **Son Le** - [@sonlechiht](https://github.com/sonlechiht)
