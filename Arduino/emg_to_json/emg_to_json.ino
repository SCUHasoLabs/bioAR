#include<ArduinoJson.h>
//#include<MegunoLink.h>
#include<Filter.h>

unsigned long startMillis;
unsigned long currentMillis;
const unsigned long period = 1000;

ExponentialFilter<float> ADCFilter(5, 0);

byte emg_pin = 12;

void sendToAPI(float data) {
  JsonDocument doc;
  doc["location"] = "Forearm";  // <------------------------------------ FILL OUT
  doc["value"] = data;

  serializeJson(doc, Serial);
}

void setup() {
  Serial.begin(9600);
  pinMode(emg_pin, INPUT);
  startMillis = millis();
}

void loop() {
  currentMillis = millis();

  if (currentMillis - startMillis >= period) {  // runs the code every second
    float emg = analogRead(emg_pin);             // collect EMG
    float voltage = emg * (5.0 / 1023.0);       // convert analog output to millivolt
    ADCFilter.Filter(voltage);                  // filter emg data to reduct noise
    sendToAPI(ADCFilter.Current());

    startMillis = currentMillis;
  }
}
