using System.Net.Http.Json;

var http = new HttpClient
{
    BaseAddress = new Uri("http://localhost:11434"),
    Timeout = TimeSpan.FromMinutes(5)
};

const string Model = "qwen3:8b";

// The conversation history. THIS is the agent's memory.
// It starts with a system message: instructions the user doesn't see,
// which shape how the model behaves for the whole conversation.
var history = new List<ChatMessage>
{
    new("system", "You are a helpful folder assistant. Keep answers short.")
};

Console.WriteLine("Folder Agent ready. Type 'exit' to quit.\n");

while (true)
{
    Console.Write("You: ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;
    if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

    // 1. Add the user's message to memory.
    history.Add(new ChatMessage("user", input));

    // 2. Send the WHOLE history, not just the latest message.
    var reply = await SendAsync(history);

    // 3. Add the model's reply to memory too, so it remembers what it said.
    history.Add(reply);

    Console.WriteLine($"Agent: {reply.Content}\n");
}

// One round-trip to Ollama.
async Task<ChatMessage> SendAsync(List<ChatMessage> messages)
{
    var request = new ChatRequest(Model, messages, Stream: false, Think: false);

    var response = await http.PostAsJsonAsync("/api/chat", request);
    response.EnsureSuccessStatusCode();

    var chat = await response.Content.ReadFromJsonAsync<ChatResponse>();
    return chat!.Message;
}

record ChatRequest(string Model, List<ChatMessage> Messages, bool Stream, bool Think);
record ChatMessage(string Role, string Content);
record ChatResponse(ChatMessage Message, bool Done);