# echo-server.py
import socket

HOST = "127.0.0.1"  # Standard loopback interface address (localhost)
PORT = 65433  # Port to listen on (non-privileged ports are > 1023)
dictOfConnections = {}  # name:(addr1)


def getOtherConnection(name, myaddr):
    if name in dictOfConnections:
        if dictOfConnections[name] == myaddr:
            return ""
        else:
            return dictOfConnections[name]
    else:
        dictOfConnections[name] = myaddr
        return ""

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    while 1:
        s.listen()
        conn, addr = s.accept()
        straddr = str(addr[0]) + ";" + str(addr[1])
        with conn:
            print(f"Connected by {addr}")
            while True:
                data = conn.recv(1024)
                if not data:
                    break
                conn.sendall(bytes(straddr + ";" + getOtherConnection(data, straddr), 'utf-8'))
                print(bytes(straddr + ";" + getOtherConnection(data, straddr), 'utf-8'))
