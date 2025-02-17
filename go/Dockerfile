FROM golang:1.18-alpine

WORKDIR /app

RUN apk add --no-cache gcc musl-dev

COPY . .

RUN go mod init caddybrowse && go mod tidy && go build -o main .

EXPOSE 5000

CMD ["./main"]
