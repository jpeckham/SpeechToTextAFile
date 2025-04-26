using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Transcription;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        // Build configuration to access user secrets
        var iconfig = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        // Set these to your Azure info
        string subscriptionKey = iconfig["ApiKey"];
        string serviceRegion = "eastus"; // based on your endpoint https://eastus.api.cognitive.microsoft.com/
        string audioFilePath = @"C:\Users\mike\Downloads\meeting1.wav";
        string outputFilePath = @$"C:\Users\mike\Downloads\meeting1transcription{DateTime.Now.ToString("yyyyMMddhhmmss")}.txt";
        Console.WriteLine($"writing to {outputFilePath}");
        using StreamWriter outputFile = new StreamWriter(outputFilePath, append: false);

        var config = SpeechConfig.FromSubscription(subscriptionKey, serviceRegion);
        config.RequestWordLevelTimestamps();
        config.SetServiceProperty("diarizationEnabled", "true", ServicePropertyChannel.UriQueryParameter);
        config.SetServiceProperty("diarization.mode", "speaker", ServicePropertyChannel.UriQueryParameter);

        using var audioInput = AudioConfig.FromWavFileInput(audioFilePath);
        using var recognizer = new ConversationTranscriber(config, audioInput);

        recognizer.Transcribed += (s, e) =>
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                string recognizedText = e.Result.Text;

                // Parse JSON to get the speaker ID
                string json = e.Result.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult);
                var parsed = JObject.Parse(json);
                var speakerId = parsed["SpeakerId"]?.ToString() ?? "UnknownSpeaker";

                string outputLine = $"[{speakerId}] {recognizedText}";

                Console.WriteLine(outputLine);
                outputFile.WriteLine(outputLine);
                outputFile.Flush(); // Flush to disk immediately in case something crashes
            }
            else
            {
                Console.WriteLine($"Unexpected result reason: {e.Result.Reason}");
            }
        };

        recognizer.Canceled += (s, e) =>
        {
            Console.WriteLine($"Canceled: {e.Reason}");
        };

        recognizer.SessionStarted += (s, e) =>
        {
            Console.WriteLine("Session started event.");
        };

        recognizer.SessionStopped += (s, e) =>
        {
            Console.WriteLine("Session stopped event.");
        };

        Console.WriteLine("Starting transcription...");
        await recognizer.StartTranscribingAsync().ConfigureAwait(false);

        Console.WriteLine("Press any key to stop...");
        Console.ReadKey();

        await recognizer.StopTranscribingAsync().ConfigureAwait(false);

        Console.WriteLine($"Transcription complete. Output saved to: {outputFilePath}");
    }
}
