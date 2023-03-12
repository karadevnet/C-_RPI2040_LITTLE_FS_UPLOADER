#include <LittleFS.h>
#include <FS.h>

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

      String read_port = "";
      String read_filestring =  "";
      String read_temp_serial =  "";

      char send_serial[1024];
      String BUILDLEDON =  "BUILDLEDON";
      String BUILDLEDOFF =  "BUILDLEDOFF";

       String DATAFILE1 =  "DATAFILE1";
      String DATAFILE2 =  "DATAFILE2";
      String DATAFILE3 =  "DATAFILE3";

      String WRITEFILE =  "WRITEFILE";
      String READFILE =  "READFILE";
      String DELETEFILE =  "DELETEFILE";
      
      String DIRFILE =  "DIRFILE";
      
      String DATAFILE1_DATA =  "SADFSDFSDFSDV#@#@#@#@#@%$^^$^23423423423499999999";
      String DATAFILE2_DATA =  "$^23423423423499999999rs232";
      String DATAFILE3_DATA =  "SADFSDFSDFSDV#@#@#@#@#@%$^^$";
    	
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
void setup1()
{
  // put your setup code here, to run once:
    
    Serial.begin(9600);
    Serial.setTimeout(100);
    delay(100);
    LittleFS.begin();
    delay(100);
	while(!Serial){}
	delay(100);


Dir dir = LittleFS.openDir("/");
delay(100);
// or Dir dir = LittleFS.openDir("/data");
while (dir.next()) {delay(100);
    Serial.print(dir.fileName());
    delay(100);
    if(dir.fileSize()) {delay(100);
        File f = dir.openFile("r");
        delay(100);
        Serial.println(f.size());
        delay(100);
    }
    delay(100);
}
/*
  Dir dir = LittleFS.openDir("/");
  
  while (dir.isFile())
  { Serial.print("CHECK FOR FILES\n"); Serial.flush();
    if(dir.fileName())
    {Serial.print(dir.fileName() + " > ");
    Serial.flush();}
    else
    {Serial.print("NO FILES\n"); Serial.flush();}
    
    if(dir.fileSize()) 
    {   File f = dir.openFile("r");
    Serial.print("filesize : " + f.size() + '\n');
        Serial.flush();
    }
    else
    { Serial.print("NO SIZE\n"); Serial.flush();}
  }
*/
  	digitalWrite(relay_1, HIGH);
	delay(1500);
	digitalWrite(relay_1, LOW);
	delay(1500);

}

void loop1()
{
  // put your main code here, to run repeatedly:


	if(Serial.available() > 0)
  {read_port = Serial.readString();
    read_port.trim();
    //Serial.println(read_port);Serial.flush();
  }

  if(read_port == BUILDLEDON)
  {digitalWrite(relay_1, HIGH); Serial.println(BUILDLEDON);
  Serial.flush(); read_port = "";
  }
  
  if(read_port == BUILDLEDOFF)
  {digitalWrite(relay_1, LOW); Serial.println(BUILDLEDOFF);
  Serial.flush(); read_port = "";
  }
  
  if(read_port == DATAFILE1)
  { Serial.println(DATAFILE1_DATA); Serial.flush(); read_port = "";}
  
  if(read_port == DATAFILE2)
  { Serial.println(DATAFILE2_DATA); Serial.flush(); read_port = "";}
  
  if(read_port == DATAFILE3)
  { Serial.println(DATAFILE3_DATA); Serial.flush(); read_port = "";}
//=========================================================
  
  //if (byte(incomingByte)!=13) Serial.readStringUntil(13); // empty readbuffer
  if(read_port == READFILE) // read until 0x0D, 0x0A
  {
    digitalWrite(relay_1, HIGH); 
      Serial.println(read_port);Serial.flush();
    //delay(200);
    File file = LittleFS.open("/datafile.txt", "r");
   // delay(200);
    if(file.available() > 0)
    { //delay(200);
      //char incomingByte;
      read_filestring = file.read();
    if (read_filestring != "D") {read_filestring += file.read();}
      //delay(200);
      Serial.print(read_filestring); Serial.flush();
      //delay(200);
        
    }
      file.close();
      digitalWrite(relay_1, LOW);
      read_port = ""; read_filestring = "";
  } 

  if(read_port == WRITEFILE)
  { //while(Serial.available() > 0)
    //{read_temp_serial = Serial.readString(); read_port = "";
    // Serial.println(read_temp_serial); Serial.flush();
    //}
    Serial.println(read_port);Serial.flush();
    delay(200);
    if(LittleFS.exists("/datafile.txt"))
    {
      File file = LittleFS.open("/datafile.txt", "a");
      if(file.available() > 0)
      {
        file.print(read_port); file.flush();
      file.close();
      }
      //Serial.println(WRITEFILE); Serial.flush();
    }
    //delay(500);
     Serial.println("write is done"); Serial.flush();
     read_port = ""; read_temp_serial = "";
  }

  if(read_port == DIRFILE)
  {   digitalWrite(relay_1, HIGH); 
      Serial.println(read_port);Serial.flush();
    delay(200);
    
    
    Dir dir = LittleFS.openDir("/");
      // or Dir dir = LittleFS.openDir("/data");
    while (dir.next()) {
    Serial.print(dir.fileName());
    if(dir.fileSize()) {
        File f = dir.openFile("r");
        Serial.println(f.size());
    }
}
      read_port = "";digitalWrite(relay_1, LOW); 
  }
read_port = "";
}
// END CORE 2 PROGRAMMING
//============================================