// File: emg_source_code.ino
// Date: 1/19/24
// Author: Vikram Jangid
// Description: This program transmits EMG and ECG data to our API

#include <WiFi.h>
#include <ArduinoHttpClient.h>
#include <Arduino_JSON.h>
#include <MegunoLink.h>
#include <Filter.h>

// Function prototypes
void connectToWifi(void);
void sendToAPI(float);

// Wifi network information
char* ssid = "";      // <------------------------------------ FILL OUT
char* password = "";  // <------------------------------------ FILL OUT

// API information
char* api = "https://vik99.pythonanywhere.com/";

// Millis information
unsigned long startMillis;
unsigned long currentMillis;
const unsigned long period = 1000;

// WiFi objects
WiFiClient client;
HttpClient http(client, api);

// Filter objects
ExponentialFilter<long> ADCFilter(5, 0);

// Global variables


byte emgPin = ;  // <------------------------------------ FILL OUT


void setup() {
  connectToWiFi();
  Serial.begin(9600);

  pinMode(, INPUT);

  startMillis = millis();
}

void loop() {
  currentMillis = millis();

  if (currentMillis - startMillis >= period) {  // runs the code every second
    float emg = analogRead(emgPin);             // collect EMG
    float voltage = emg * (5.0 / 1023.0);       // convert analog output to millivolt
    ADCFilter.Filter(voltage);                  // filter emg data to reduct noise
    sendToAPI(voltage);

    startMillis = currentMillis;
  }
}

void connectToWiFi() {
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.println("Connecting to WiFi...");
    delay(1000);
  }
  Serial.println("\n Connected to WiFi!");
}

void sendToAPI(float data) {
  JsonDocument doc;
  doc["location"] = "";  // <------------------------------------ FILL OUT
  doc["value"] = data;

  client.connect(api, 80);
  client.println("POST")
  serializeJson(doc, client);
}
