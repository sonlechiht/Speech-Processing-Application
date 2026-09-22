import whisper
import time

import whisper.utils
model = whisper.load_model("turbo")
print('start')
tik = time.time()

result = model.transcribe(r'8 Track 8.mp3',language='en',verbose=True, fp16=False)
print(result)
print(time.time()-tik)