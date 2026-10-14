# FolderAgent

A from-scratch AI agent in C# that runs locally with [Ollama](https://ollama.com). The goal is to build an agent that can work on a local folder using tools (list, read, write files) — give it a task like “summarize every .md file into one report” and it plans and executes the steps itself.

Currently implements a multi-turn chat loop with conversation history as context memory.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Ollama](https://ollama.com) running locally on the default port (11434)
- The `qwen3:8b` model pulled:
  ```bash
  ollama pull qwen3:8b
  ```

## Getting Started

```bash
git clone https://github.com/Inde16/FolderAgent.git
cd FolderAgent
dotnet run
```

Type messages to chat with the agent. Type `exit` to quit.

## How It Works

The app is a single-file C# console application (`Program.cs`) using top-level statements. It:

1. Opens a REPL chat loop in the terminal
2. Maintains a conversation history (`List<ChatMessage>`) that grows with each exchange
3. Sends the **full** history to Ollama's `/api/chat` endpoint on every turn, so the model has context of the entire conversation
4. Prints the model's response and loops

No frameworks or SDKs — just raw HTTP calls with `System.Net.Http.Json`.

## Roadmap

- [ ] File tools (list, read, write) so the agent can operate on a local folder
- [ ] Autonomous planning loop — the agent decides which tools to call and in what order
- [ ] Persistent memory across sessions
