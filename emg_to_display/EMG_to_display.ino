#include "U8g2lib.h"
#include "Arduino.h"

unsigned long startMillis;
unsigned long currentMillis;
const unsigned long period = 3000;

int collectEmg(void);

void setup() {
  Serial.begin(9600);
  pinMode(A0, INPUT);
  pinMode(A1, INPUT);

  startMillis = millis();
}

void loop() {
  currentMillis = millis();

  int sensor1, sensor2;
  sensor1 = analogRead(A0);
  sensor2 = analogRead(A1);

  Serial.print("sensor1: ");
  Serial.println(sensor1);

  Serial.print("sensor2: ");
  Serial.println(sensor2);

  delay(1000);
}

