# Speech to Text Transcription Console App

This is a .NET 8 console application that uses Azure Cognitive Services to transcribe speech from an audio file into text. The app supports speaker diarization, providing speaker identification for the transcribed text.

## Features
- Transcribes speech from a `.wav` audio file.
- Supports speaker diarization (identifies speakers in the audio).
- Outputs the transcription to a text file with timestamps.

## Prerequisites
1. **Azure Cognitive Services Speech API**:
   - Create an Azure Speech resource in the [Azure Portal](https://portal.azure.com/).
   - Note the **Subscription Key** and **Service Region** for the resource.

2. **.NET 8 SDK**:
   - Install the [.NET 8 SDK](https://dotnet.microsoft.com/download).

3. **Audio File**:
   - Ensure the input audio file is in `.wav` format.

## Setup

### 1. Clone the Repository
`git clone <repository-url> cd <repository-folder>`

### 2. Configure User Secrets
Set up your Azure Speech API key using .NET user secrets:

`dotnet user-secrets init dotnet user-secrets set "ApiKey" "<your-azure-speech-api-key>"`


### 3. Configure `appsettings.json`
Edit the `appsettings.json` file to specify the service region, input audio file path, and output file path template:

```{
  "AzureSettings": {
    "ApiKey": null,//override or put in secrets
    "ServiceRegion": "eastus",
    "AudioFilePath": "C:\\Users\\james\\Downloads\\meeting1.wav",
    "OutputFilePathTemplate": "C:\\Users\\james\\Downloads\\meeting1transcription{0}.txt"
  }
}
```


- **ServiceRegion**: The region of your Azure Speech resource (e.g., `eastus`).
- **AudioFilePath**: Path to the `.wav` audio file you want to transcribe.
- **OutputFilePathTemplate**: Template for the output file path. `{0}` will be replaced with a timestamp.

### 4. Restore Dependencies
Run the following command to restore NuGet packages:
`dotnet restore`


## Usage
1. Build and run the application:
`dotnet run`

2. The app will:
   - Transcribe the audio file specified in `appsettings.json`.
   - Save the transcription to the output file path specified in `appsettings.json`.

3. Check the console output for progress and the output file for the transcription.

## Example Output
The transcription output will look like this:

`[Speaker1] Hello, everyone. Welcome to the meeting. [Speaker2] Thank you. Let's get started with the agenda.`


## Dependencies
- [Microsoft.CognitiveServices.Speech](https://www.nuget.org/packages/Microsoft.CognitiveServices.Speech)
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration)
- [Microsoft.Extensions.Configuration.UserSecrets](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets)
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)

## Notes
- Ensure the audio file is in `.wav` format for compatibility.
- The app uses speaker diarization to identify speakers in the transcription.

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

