#include <wiringPi.h>
#include <wiringPiI2C.h> 
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <math.h>
#include <string.h>
#include <ctype.h>
#include <libgen.h>
#include <sys/time.h>
#include <chrono>
#include <iostream>
#include <fstream>

#define READY_WAIT_TIME 100
#define MAX_STATION 256
#define HCC_DEFAULT 1	// High Cut Control
#define SNC_DEFAULT 1	// Stereo Noise Cancelling(Signal dependant stereo)
#define FORCED_MONO 0	// 1:forced mono, 0:stereo on
#define SEARCH_MODE_DEFAULT 2	// 1: Low, 2: Middle, 3: High
#define RADIO_STATION_INFO "/var/local/radio/radio_station"
#define TUNED_FREQ "/var/local/radio/tuned_freq"
#define MAX_STATION_NAME_LEN 128

int fd;
int dID = 0x60; // i2c Channel the device is on
unsigned char frequencyH = 0;
unsigned char frequencyL = 0;
unsigned int frequencyB;
unsigned char write_radio[5] = {0};
unsigned char read_radio[5] = {0};

char *prog_name;

int	station_info_num = 0;
struct _station_info {
	double freq;
	char name[MAX_STATION_NAME_LEN];
} station_info[MAX_STATION];

int wait_ready(void) 
{
	unsigned char radio[5] = {0};
	int loop;

	loop = 0;
	read(fd, radio, 5);
	while((radio[0] & 0x80) == 0 && loop < 2000000/READY_WAIT_TIME) { // max 2 sec
		usleep(READY_WAIT_TIME);
		read(fd, radio, 5);
		loop++;
	}

	if ((radio[0] & 0x80) == 0)	{ // Ready fail
//		fprintf(stderr, "Ready Fail!\n");
		return -1;
	}
	else if (radio[0] & 0x40) { // Band limit reached
//		fprintf(stderr, " Band limit reached!\n");
		return -2;
	}

//	printf("Level ADC=%d\n", radio[4] >> 4);
	return 0;
}

void set()
{	
	write(fd, write_radio, 5);

	// if (standby) return;

	// save_freq(79.0);
	/*
	if (wait_ready() < 0) {
		fprintf(stderr, "Fail to tune!\n");
		return;
	}
	*/
}
/*
void set_freq(double freq, int hcc, int snc, unsigned char forcd_mono, int mute, int standby)
{
	unsigned char radio[5] = {0};

	frequencyB = 4 * (freq * 1000000 + 225000) / 32768; //calculating PLL word
	frequencyH = frequencyB >> 8;
	frequencyL = frequencyB & 0xFF;

	radio[0] = frequencyH; //FREQUENCY H
	if (mute) radio[0] |= 0x80;
	radio[1] = frequencyL; //FREQUENCY L
	radio[2] = 0xB0; //3 byte (0xB0): high side LO injection is on,.
	if (forcd_mono) radio[2] |= 0x08;
	radio[3] = 0x10; // Xtal is 32.768 kHz
	if (freq < 87.5) radio[3] |= 0x20;
	if (hcc) radio[3] |= 0x04;
	if (snc) radio[3] |= 0x02;
	if (standby) radio[3] |= 0x40;
	radio[4] = 0x40; // deemphasis is 75us in Korea and US

	write(fd, radio, 5);

	if (standby) return;

	save_freq(freq);

	if (wait_ready() < 0) {
		fprintf(stderr, "Fail to tune!\n");
		return;
	}
}
*/

int main(int argc, char* argv[]) {
    std::ofstream recordFd;
    double freq = 79.0;
    int hcc = HCC_DEFAULT, snc = SNC_DEFAULT;
    int forced_mono = FORCED_MONO;
    int readyFlag = 0;
	int filewriteflag = 0;
    int n = 0;
    unsigned char stereo;
    unsigned char level_adc;
    long currentTime;

	frequencyB = 4 * (freq * 1000000 + 225000) / 32768; //calculating PLL word
	frequencyH = frequencyB >> 8;
	frequencyL = frequencyB & 0xFF;

	write_radio[0] = frequencyH; //FREQUENCY H
	if (0) write_radio[0] |= 0x80;
	write_radio[1] = frequencyL; //FREQUENCY L
	write_radio[2] = 0xB0; //3 byte (0xB0): high side LO injection is on,.
	if (forced_mono) write_radio[2] |= 0x08;
	write_radio[3] = 0x10; // Xtal is 32.768 kHz
	if (freq < 87.5) write_radio[3] |= 0x20;
	if (hcc) write_radio[3] |= 0x04;
	if (snc) write_radio[3] |= 0x02;
	if (0) write_radio[3] |= 0x40;
	write_radio[4] = 0x40; // deemphasis is 75us in Korea and US

    prog_name = basename(argv[0]);

    if((fd = wiringPiI2CSetup(dID)) < 0) {
        fprintf(stderr, "error opening i2c channel\n");
        exit(1);
    }

	set();
    while(1){
		recordFd.open("timestamp.txt", std::ios_base::app);
		filewriteflag = 0;
		while(!filewriteflag){
			set();
			wait_ready();
	    	read(fd, read_radio, 5);
            readyFlag = (read_radio[3] > 0x80)? 1:0;
            if(readyFlag){ // ready flag on
				filewriteflag = 1;
				std::chrono::system_clock::time_point currentTime = std::chrono::system_clock::now();
				auto duration = currentTime.time_since_epoch();

				typedef std::chrono::duration<int, std::ratio_multiply<std::chrono::hours::period, std::ratio<8>>::type> Days; /* UTC: +8:00 */

				Days days = std::chrono::duration_cast<Days>(duration);
    			duration -= days;
				auto hours = std::chrono::duration_cast<std::chrono::hours>(duration);
    			duration -= hours;
				auto minutes = std::chrono::duration_cast<std::chrono::minutes>(duration);
				duration -= minutes;
				auto seconds = std::chrono::duration_cast<std::chrono::seconds>(duration);
				duration -= seconds;
				auto milliseconds = std::chrono::duration_cast<std::chrono::milliseconds>(duration);
				duration -= milliseconds;
				auto microseconds = std::chrono::duration_cast<std::chrono::microseconds>(duration);
				duration -= microseconds;
				auto nanoseconds = std::chrono::duration_cast<std::chrono::nanoseconds>(duration);

        		stereo = read_radio[2] & 0x80;
        		level_adc = read_radio[3] >> 4;
				auto result = seconds.count();
				result = result * 1000 + milliseconds.count();
				result = result * 1000 + microseconds.count();
				result = result * 1000 + nanoseconds.count();
				recordFd << result << std::endl;
				recordFd.close();
				sleep(5);
	    	}
        }
    } 

    close(fd);
    return 0;
}
