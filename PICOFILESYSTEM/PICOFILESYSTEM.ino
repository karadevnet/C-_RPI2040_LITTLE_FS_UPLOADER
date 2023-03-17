//#include <Arduino.h>
#include <LittleFS.h>

// EXAMPLE RPI PICO FILE SYSTEM
//  MENAGE TROUGH SERIAL PORT
// LATE ON LAN CONNECTION

    unsigned char relay_8 = 0; unsigned char input_1 = 8;
    unsigned char relay_7 = 1; unsigned char input_2 = 9;
    unsigned char relay_6 = 2; unsigned char input_3 = 10;
    unsigned char relay_5 = 3; unsigned char input_4 = 11;
    unsigned char relay_4 = 4; unsigned char input_5 = 12;
    unsigned char relay_3 = 5; unsigned char input_6 = 13;
    unsigned char relay_2 = 6; unsigned char input_7 = 14;
    unsigned char relay_1 = 7; unsigned char input_8 = 15;

    bool flag_button_1 = false; byte flag_button_1_count = 0;
    bool flag_button_2 = false; byte flag_button_2_count = 0;
    bool flag_button_3 = false; byte flag_button_3_count = 0;
    bool flag_button_4 = false; byte flag_button_4_count = 0;
    bool flag_button_5 = false; byte flag_button_5_count = 0;
    bool flag_button_6 = false; byte flag_button_6_count = 0;
    bool flag_button_7 = false; byte flag_button_7_count = 0;
    bool flag_button_8 = false; byte flag_button_8_count = 0;
  //FSInfo fs_info;

void setup()
{
  // put your setup code here, to run once:
	pinMode(relay_1, OUTPUT); //relay 1 OUTPUT
    pinMode(relay_2, OUTPUT); //relay 2 OUTPUT
    pinMode(relay_3, OUTPUT);  //relay 3 OUTPUT 
    pinMode(relay_4, OUTPUT);   //relay 4 OUTPUT
    pinMode(relay_5, OUTPUT);  //relay 5 OUTPUT
    pinMode(relay_6, OUTPUT); //relay 6 OUTPUT
    pinMode(relay_7, OUTPUT);  //relay 7 OUTPUT 
    pinMode(relay_8, OUTPUT); //relay 8 OUTPUT

	pinMode(input_1, INPUT_PULLDOWN); //relay 1 OUTPUT
    pinMode(input_2, INPUT_PULLDOWN); //relay 2 OUTPUT
    pinMode(input_3, INPUT_PULLDOWN);  //relay 3 OUTPUT 
    pinMode(input_4, INPUT_PULLDOWN);   //relay 4 OUTPUT
    pinMode(input_5, INPUT_PULLDOWN);  //relay 5 OUTPUT
    pinMode(input_6, INPUT_PULLDOWN); //relay 6 OUTPUT
    pinMode(input_7, INPUT_PULLDOWN);  //relay 7 OUTPUT 
    pinMode(input_8, INPUT_PULLDOWN); //relay 8 OUTPUT
	
	pinMode(LED_BUILTIN, OUTPUT);

 }

void loop()
{
  // put your main code here, to run repeatedly:

	digitalWrite(LED_BUILTIN, HIGH);
	delay(500);
	digitalWrite(LED_BUILTIN, LOW);
	delay(500);

}

// END CORE 1 PROGRAMMING
//============================================
//============================================


void setup1() {
  Serial.begin(9600);
  //while (!Serial);
  LittleFS.begin();
  if(Serial.available())
  {Serial.print("\nFile system initialized\n");}
}

void loop1() {
  if (Serial.available() > 0) 
  {
    String cmdStr = Serial.readString();
    cmdStr.trim(); // remove any leading/trailing whitespace characters

    if (cmdStr.startsWith("WRITEFILE"))
    {
      String filename = cmdStr.substring(9);
	  filename.trim();
      File file = LittleFS.open(filename, "w");
      if (file) {
        Serial.print("\nReady to receive file content\n");
        while (Serial.available() <= 0);
        String dataStr = Serial.readString();
        dataStr.trim(); // remove any leading/trailing whitespace characters
        file.print(dataStr);
        file.flush();
        Serial.print("\nFile content received and written successfully\n");
        Serial.print("\nFile Name: " + filename + ", File Size: " + String(file.size()) + " bytes\n");
        file.close();
      } else {Serial.print("\nError opening file for writing\n"); }
    }

    if (cmdStr.startsWith("READFILE")) //READFILEdata.txt
    {
      String filename = cmdStr.substring(8);
	  filename.trim();
       String file_read = "";
       int file_size = 0;
      File file = LittleFS.open(filename, "r");
      if (file) {
        Serial.print("\nReading file content...\n\n");
        while (file_size < file.size())
        {
         char file_read_char = file.read();
		  file_read.trim();
          Serial.write(file_read_char);
          file_size++;
        }
        Serial.print("\n\nFile content read successfully\n");
        file.close();
      }
      else
      {
        Serial.print("\nError opening file for reading\n");
      }
    }

    if (cmdStr.startsWith("DELETEFILE")) {
      String filename = cmdStr.substring(10);
	  filename.trim();
      if (LittleFS.remove(filename)) {
        Serial.print("\nFile deleted successfully\n");
      } else {
        Serial.print("\nError deleting file\n");
      }
    }

    if (cmdStr.startsWith("LISTDIR")) {
      Dir dir = LittleFS.openDir("/");
      Serial.print("\nListing files in root directory:\n");
      while (dir.next()) {
        Serial.print(dir.fileName()+"\n");//Serial.print("\n");
      }
    }
  }
}
