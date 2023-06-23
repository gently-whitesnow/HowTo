FROM ubuntu:latest

RUN apt-get install -y \
    python3 \
    clang \
    clang++ \
    mono-complete

COPY ./task_server.py /

ENTRYPOINT [ "/task_server.py" ]