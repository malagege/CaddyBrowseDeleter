FROM python:3.9-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

# 建立 db 目錄並設定權限
RUN mkdir -p db && chmod 777 db

# COPY db/app.db /app/db/app.db

CMD ["sh", "run.sh"]

ENV FLASK_APP=app.py

EXPOSE 5000

# CMD ["flask", "run", "--host=0.0.0.0"]