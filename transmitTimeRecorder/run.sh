#!/bin/bash
for run in {1..120}; do
	sudo ./fm_transmitter -f 79.0 ./timestamp.wav;
	sleep 8;
	done
