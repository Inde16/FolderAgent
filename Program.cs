using System.Net.Http.Json;

// 1. The HTTP client pointing at the local Ollama server.
//    Local models can be slow, so we give it a long timeout.
var http = new HttpClient
{
    BaseAddress = new Uri("http://localhost:11434"),
    Timeout = TimeSpan.FromMinutes(5)
};

const string Model = "qwen3:8b";

// 2. Build the request: which model, the conversation so far, no streaming.
var request = new ChatRequest(
    Model: "qwen3:8b",
    Messages: [new ChatMessage("user", "In one sentence, what is an AI agent?")],
    Stream: false);

// 3. Send it and read the reply.
var response = await http.PostAsJsonAsync("/api/chat", request);
response.EnsureSuccessStatusCode();

var chat = await response.Content.ReadFromJsonAsync<ChatResponse>();
Console.WriteLine(chat!.Message.Content);

// --- Data shapes matching Ollama's JSON ---
record ChatRequest(string Model, List<ChatMessage> Messages, bool Stream);
record ChatMessage(string Role, string Content);
record ChatResponse(ChatMessage Message, bool Done);