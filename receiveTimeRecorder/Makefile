CC=g++
CFLAGS=-Wall 
OBJECTS=recorder.o
BINARIES=recorder
LDFLAGS+= -lm -lwiringPi

all : $(BINARIES)

%.o : %.c
	$(CC) $(CFLAGS) -c -o $@ $< $(LDFLAGS)

clean:
	rm -f $(OBJECTS) $(BINARIES)

FORCE:
.PHONY: FORCE

