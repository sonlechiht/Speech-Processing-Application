import subprocess
import time

def worker():
    for i in range(5):
        time.sleep(1)  # Giả lập công việc
        print(f"Kết quả {i}: {i*i}")

if __name__ == "__main__":
    process = subprocess.Popen(["python", "-c", worker.__name__], stdout=subprocess.PIPE)
    while True:
        output = process.stdout.readline()
        if output == b'':
            break
        print(f"Thời gian nhận được: {time.time()} - Nội dung: {output.decode('utf-8')}", end='')