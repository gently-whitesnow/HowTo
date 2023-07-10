FROM rust:1.67 as build

WORKDIR /build

COPY . .

FROM ubuntu:latest

RUN apt-get install -y \
    python3 \
    clang \
    clang++ \
    mono-complete \
    build-essential \
    curl

COPY ./task_server.py /

ENTRYPOINT [ "/task_server.py" ]