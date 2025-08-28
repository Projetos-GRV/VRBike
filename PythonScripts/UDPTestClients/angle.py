#!/usr/bin/env python3
import socket
import time

PORT = 8001
IP_ADDR = "127.0.0.1"

def main():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    while (True):
        sock.sendto(b"1;2;3", (IP_ADDR, PORT))
        time.sleep(0.100)

main()
