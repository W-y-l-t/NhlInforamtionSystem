FROM python:3.13-alpine

WORKDIR /usr/src/app
RUN apk add --no-cache postgresql-client

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY . .
RUN sed -i 's/\r$//' entrypoint.sh && chmod +x entrypoint.sh

ENTRYPOINT ["sh", "/usr/src/app/entrypoint.sh"]